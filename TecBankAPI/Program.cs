using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using TecBankAPI.Services;
using TecBankAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddNewtonsoftJson(); // Para mejor manejo de JSON

// Configurar nuestros servicios
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<CuentaService>();
builder.Services.AddSingleton<FileDataService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Permite las peticiones desde la App
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors("AllowAll"); //Permite las peticiones desde la App

app.UseAuthorization();

app.MapControllers();

// Crear archivos de datos si no existen
var fileDataService = app.Services.GetRequiredService<FileDataService>();
fileDataService.InitializeFiles();

app.Run();
