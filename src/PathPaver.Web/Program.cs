using PathPaver.Application.Services.Entities;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddScoped<UserService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

var app = builder.Build();

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

