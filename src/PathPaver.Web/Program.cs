using PathPaver.Application.Repository.Entities;
using PathPaver.Application.Services.Auth;
using PathPaver.Application.Services.Entities;
using PathPaver.Domain.Entities;
using PathPaver.Persistence.Repository.Entities;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

var app = builder.Build();

#region Security

// Authentication related stuff should go here
// app.MapPost("/authenticate", (User user, AuthService auth) => auth.GenerateToken(user));

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

