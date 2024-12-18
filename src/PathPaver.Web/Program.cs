using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ML;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using PathPaver.Application.Repository.Entities;
using PathPaver.Application.Services.Auth;
using PathPaver.Application.Services.Entities;
using PathPaver.ML;
using PathPaver.Persistence;
using PathPaver.Persistence.Context;
using PathPaver.Persistence.Repository.Entities;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Configuration

/*
 * Serilog - Logging
 * 
 * It go look at the appsettings.json file to check Serilog Config
 */
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt")
    .MinimumLevel.Warning()
    .CreateLogger();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// To handle UserSecrets content for atlas cluster in DbSettings.cs file
builder.Configuration
    .GetSection("MongoCluster")
    .Get<DbSettings>();

// To handle UserSecrets content for Security related things in AuthSettings.cs file
builder.Configuration
    .GetSection("Security")
    .Get<AuthSettings>();

#endregion

#region DBContext

var clientConnection = new MongoClient(DbSettings.ConnectionURI);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMongoDB(clientConnection, DbSettings.DatabaseName);
});
#endregion

#region Scoped Services

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRentPredictionRepository, RentPredictionRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RentPredictionService>();

#endregion

#region Prediction Related

// PredictionEnginePool AKA the model that will predict our rent prices
builder.Services.AddPredictionEnginePool<ApartmentInput, ApartmentOutput>()
    .FromFile(modelName: "RentPricePredictor", filePath: "../PathPaver.ML/Model/model.zip",
        watchForChanges: true);

#endregion

#region Auth Related

// Default configuration for Authentication
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt => // Perform auth by extracting and valid JWT token from 'Authorization' header
{
    jwt.RequireHttpsMetadata = false; // Will work with HTTP
    jwt.SaveToken = false; // Token will not be saved in AuthenticationProperties 
    jwt.TokenValidationParameters = AuthSettings.GetTokenValidationParameters();
});

#endregion

#region Services

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PathPaver API",
        Version = "v1",
        Description = "API for searching and predicting rent prices for apartments"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

#endregion

var app = builder.Build();

#region Security

/*
 * Security Middleware for authorizations
 *
 * - Authentication related stuff should go here
 * 
 * CORS + Authorization + Authentication Middleware
 */

app.UseAuthentication();
app.UseAuthorization();
app.UseCors(policyBuilder =>
{
    policyBuilder.WithOrigins(builder.Configuration["FrontendUrl"]!)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

#endregion

#region Middlewares

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging(); // Change default logging to Serilog
app.UseHttpsRedirection();

#endregion

// To add endpoints for created controllers
app.MapControllers();

app.Run();