using Microsoft.EntityFrameworkCore;
using SchoolSystem.Infrastructure.Persistence.Context;
using SchoolSystem.Application.Mappings;
using SchoolSystem.Application.Services.Implementations;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Interfaces;
using SchoolSystem.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Registra AutoMapper e indica el ensamblado donde buscar los perfiles
builder.Services.AddAutoMapper(typeof(AlumnoProfile).Assembly);

// Registrar DbContext: asegúrate de tener "DefaultConnection" en appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found. Añade la cadena en appsettings.json.");
}

builder.Services.AddDbContext<SchoolSystemDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IRepository<Alumno>, Repository<Alumno>>(); // Implementa tu repositorio concreto
builder.Services.AddScoped<IAlumnoService, AlumnoService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Mapear controladores para que las rutas de MVC funcionen y los breakpoints se alcancen
app.MapControllers();

app.Run();