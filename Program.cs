using FilmStudioSFF.Data;
using FilmStudioSFF.Services;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKey1234567890abcdef")), 
            ValidIssuer = "FilmStudioSFF",
            ValidAudience = "FilmStudioSFF"
        };
    });

builder.Services.AddDbContext<FilmStudioDbContext>(options =>
    options.UseInMemoryDatabase("FilmStudioSFF"));

builder.Services.AddControllers();

builder.Services.AddSingleton<UserService>(); 
builder.Services.AddSingleton<AuthenticationService>(); 
builder.Services.AddScoped<FilmStudioService>();  
builder.Services.AddScoped<FilmService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
