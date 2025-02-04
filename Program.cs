using FilmStudioSFF.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FilmStudioSFF API",
        Version = "v1",
        Description = "API för FilmStudioSFF med användarautentisering"
    });
});

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthenticationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "FilmStudioSFF API v1");
        options.RoutePrefix = ""; 
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); 

app.Run();
