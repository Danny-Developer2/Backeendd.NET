using prueba.Interfaces;
using Microsoft.OpenApi.Models;
using prueba.Repositories;
using prueba.Data;
using Microsoft.EntityFrameworkCore;
using prueba.Helpers;
using FluentValidation.AspNetCore;
using prueba.Services;
using prueba.Formatos;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
// Configurar OpenAPI/Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Mi API",
        Version = "v1",
        Description = "Ejemplo de API con Swagger en ASP.NET Core"
    });
});

// Agregar DbContext para SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar los servicios de repositorio
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IMarcaRespository, MarcaRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();

// Agregar controladores
builder.Services.AddControllers();

// Habilitar CORS (si necesitas permitir solicitudes desde otros dominios)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});


// ... existing code ...
builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// ... existing code ...

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IVehicleFormatterService, VehicleFormatterService>();
builder.Services.AddScoped<IMarcaFormatterService, MarcaFormatterService>();

builder.Services.AddSwaggerGen();
// Agregar Memory Cache
builder.Services.AddMemoryCache();


// Crear la aplicación web y iniciarla

var app = builder.Build();

// Habilitar Swagger en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API v1");
        c.RoutePrefix = string.Empty; // Hace que Swagger esté disponible en la raíz
    });
}

// Configuración de CORS (si es necesario)
app.UseCors("AllowAll");

// Habilitar redirección HTTPS (comentarlo si no estás usando HTTPS en desarrollo)
app.UseHttpsRedirection();

// Configurar las rutas de la aplicación
app.UseRouting();

// Mapeo de controladores
app.MapControllers();

// Iniciar la aplicación
app.Run();
