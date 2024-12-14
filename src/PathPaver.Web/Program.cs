using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.ML;
using Microsoft.IdentityModel.Tokens;
using PathPaver.Application.Repository.Entities;
using PathPaver.Application.Services.Auth;
using PathPaver.Application.Services.Entities;
using PathPaver.ML;
using PathPaver.Persistence;
using PathPaver.Persistence.Repository.Entities;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Configuration.AddUserSecrets<Program>();

// To handle UserSecrets content for atlas cluster in DbSettings.cs file
builder.Configuration.GetSection("MongoCluster").Get<DbSettings>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();

// PredictionEnginePool AKA the model that will predict our rent prices
builder.Services.AddPredictionEnginePool<ApartmentInput, ApartmentOutput>()
    .FromFile(modelName: "RentPricePredictor", filePath: "../PathPaver.ML/Model/TheSavedModelFile", watchForChanges: true);


// Default configuration for Authentication
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
    
}).AddJwtBearer(jwt => // Perform auth by extracting and valid JWT token from 'Authorization' header
{
    jwt.RequireHttpsMetadata = false; // Will work with HTTP
    jwt.SaveToken = false; // Token will not be saved in AuthenticationProperties 
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        // Key used in signature
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthSettings.PrivateKey)),
        
        ValidateIssuer = false,
        ValidateAudience = false 
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

var app = builder.Build();

/*
 * Security Middleware for authorizations
 * 
 * - Authentication related stuff should go here
 */

#region Security

app.UseAuthentication();
app.UseAuthorization();

#endregion

#region Middlewares

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
#endregion

// To add endpoints for created controllers
app.MapControllers();

app.Run();

