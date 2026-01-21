using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SchoolSystem.API.Services;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.Mappings;
using SchoolSystem.Application.Services.Implementations;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Application.Validations.Alumnos;
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

// Interceptar la validación automática para usar nuestro formato ApiResponse
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        // 1. Obtener todos los mensajes de error del ModelState
        var errors = context.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        // 2. Crear nuestra respuesta estándar
        var response = new ApiResponse<object>(errors, "Errores de validación en los datos enviados.");

        // 3. Devolver un BadRequest (400) con nuestra estructura
        return new BadRequestObjectResult(response);
    };
});

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
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(typeof(CreateAlumnoValidator).Assembly);
builder.Services.AddHttpContextAccessor();

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
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRegistroConductaService, RegistroConductaService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IPagoService, PagoService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IMedicoService, MedicoService>();
builder.Services.AddScoped<INotificacionService, NotificacionService>();
builder.Services.AddScoped<IRelacionService, RelacionService>();
builder.Services.AddScoped<ICicloEscolarService, CicloEscolarService>();
builder.Services.AddScoped<IPeriodoEvaluacionService, PeriodoEvaluacionService>();




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
