using System.IO;
using Biblioteca.Data;
using Biblioteca.Service;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<LivroService>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var diretorio = new DirectoryInfo(AppContext.BaseDirectory);

    while (diretorio != null &&
           !File.Exists(Path.Combine(
               diretorio.FullName,
               "Sistema Biblioteca",
               "Sistema Biblioteca.csproj")))
    {
        diretorio = diretorio.Parent;
    }

    if (diretorio == null)
        throw new DirectoryNotFoundException("Projeto Sistema Biblioteca não encontrado.");

    var caminhoBanco = Path.Combine(
        diretorio.FullName,
        "Sistema Biblioteca",
        "biblioteca.db");

    options.UseSqlite($"Data Source={caminhoBanco}");
});

var app = builder.Build();
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
