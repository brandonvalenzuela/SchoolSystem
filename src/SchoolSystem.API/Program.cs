using Microsoft.EntityFrameworkCore;
using SchoolSystem.Infrastructure.Persistence.Context;
using SchoolSystem.Application.Mappings;
using SchoolSystem.Application.Services.Implementations;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Interfaces;
using SchoolSystem.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar controladores
builder.Services.AddControllers();

// 2. Configuración de Base de Datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found. Añade la cadena en appsettings.json.");
}

builder.Services.AddDbContext<SchoolSystemDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// 3. Configuración de AutoMapper (OPTIMIZADO)
// Basta con apuntar a UNA clase que esté en la capa 'Application'. 
// AutoMapper escaneará todo ese proyecto y encontrará todos los perfiles automáticamente.
builder.Services.AddAutoMapper(typeof(AlumnoProfile).Assembly);

// 4. Repositorio Genérico
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// 5. Servicios de Aplicación (Inyección de Dependencias)
builder.Services.AddScoped<IAlumnoService, AlumnoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IEscuelaService, EscuelaService>();
builder.Services.AddScoped<IMaestroService, MaestroService>();
builder.Services.AddScoped<IPadreService, PadreService>();
builder.Services.AddScoped<IGradoService, GradoService>();
builder.Services.AddScoped<IGrupoService, GrupoService>();
builder.Services.AddScoped<IMateriaService, MateriaService>();
builder.Services.AddScoped<IInscripcionService, InscripcionService>();
builder.Services.AddScoped<ICalificacionService, CalificacionService>();
builder.Services.AddScoped<IAsistenciaService, AsistenciaService>();

// 6. Configuración de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 7. CORS (Opcional pero recomendado si vas a conectar un Frontend como React/Angular)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// --- Pipeline de Peticiones HTTP ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Usar CORS (debe ir antes de MapControllers)
app.UseCors("PermitirTodo");

app.UseAuthorization();

app.MapControllers();

app.Run();
