using FilmStudioSFF.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Lägg till autentisering och JWT-bearer-token
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "FilmStudioSFF", // Samma som i GenerateJwtToken
            ValidAudience = "FilmStudioSFF", // Samma som i GenerateJwtToken
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("YourSuperSecretKey123!"))
        };
    });

builder.Services.AddControllers();

// Registrera dina tjänster
builder.Services.AddScoped<FilmStudioService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<FilmService>();

// Lägg till Swagger (valfritt, för API-dokumentation)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); // Lägg till detta för att aktivera autentisering
app.UseAuthorization();

app.MapControllers();

app.Run();

