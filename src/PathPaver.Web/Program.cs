using Microsoft.Extensions.ML;
using PathPaver.Application.Repository.Entities;
using PathPaver.Application.Services.Auth;
using PathPaver.Application.Services.Entities;
using PathPaver.ML;
using PathPaver.Persistence;
using PathPaver.Persistence.Repository.Entities;

var builder = WebApplication.CreateBuilder(args);

#region Services

// To handle appsettings.json content for atlas cluster
builder.Configuration.GetSection("MongoCluster").Get<DbSettings>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();

// PredictionEnginePool AKA the model that will predict our rent prices
builder.Services.AddPredictionEnginePool<ApartmentInput, ApartmentOutput>()
    .FromFile(modelName: "RentPricePredictor", filePath: "../PathPaver.ML/Model/", watchForChanges: true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

var app = builder.Build();

/*
 * Security Middleware for authorizations
 */
#region Security

// Authentication related stuff should go here

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

