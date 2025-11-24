using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; // Necesario para configurar Swagger con JWT
using SchoolSystem.Application.Mappings;
using SchoolSystem.Application.Services.Implementations;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Interfaces;
using SchoolSystem.Infrastructure.Persistence.Context;
using SchoolSystem.Infrastructure.Persistence.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURACIÓN DE AUTENTICACIÓN (JWT) ---

// Validar que la clave secreta exista para evitar errores crípticos al arrancar
var jwtSecret = builder.Configuration["JwtSettings:Secret"];
if (string.IsNullOrEmpty(jwtSecret))
{
    throw new InvalidOperationException("JwtSettings:Secret no está configurado en appsettings.json");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.SaveToken = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
    };
});

builder.Services.AddScoped<IAuthService, AuthService>();

// --- 2. CONTROLADORES Y BASE DE DATOS ---

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

builder.Services.AddDbContext<SchoolSystemDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// --- 3. AUTOMAPPER Y SERVICIOS ---

builder.Services.AddAutoMapper(typeof(AlumnoProfile).Assembly);
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Inyección de Servicios de Aplicación
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

// --- 4. SWAGGER CON SOPORTE PARA JWT ---

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SchoolSystem API", Version = "v1" });

    // Definir el esquema de seguridad Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// --- 5. CORS ---
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

// --- PIPELINE DE PETICIONES HTTP ---

// Middleware de manejo de errores global (Debe ser el primero para capturar todo)
// Asegúrate de que el namespace coincida con tu carpeta (Middleware vs Middlewares)
app.UseMiddleware<SchoolSystem.API.Middleware.ErrorHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("PermitirTodo");

// Autenticación y Autorización (El orden es CRÍTICO)
app.UseAuthentication(); // 1. ¿Quién eres?
app.UseAuthorization();  // 2. ¿Qué puedes hacer?

app.MapControllers();

app.Run();
