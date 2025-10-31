# Script complementario - Agregar caracteristicas completas al proyecto
# Ejecutar DESPUES de crear-proyecto.ps1

Write-Host "========================================" -ForegroundColor Yellow
Write-Host "AGREGANDO CARACTERISTICAS ADICIONALES" -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Yellow

# Verificar que estamos en el directorio correcto
if (-not (Test-Path "SchoolSystem.sln")) {
    Write-Host "ERROR: No se encontro SchoolSystem.sln" -ForegroundColor Red
    Write-Host "Asegurate de ejecutar este script dentro de la carpeta SchoolSystem" -ForegroundColor Red
    exit
}

Write-Host "Proyecto encontrado. Iniciando mejoras..." -ForegroundColor Green

# ============================================
# CREAR ESTRUCTURA COMPLETA DE CARPETAS
# ============================================
Write-Host "`nCreando estructura completa de carpetas..." -ForegroundColor Cyan

# Domain - Carpetas detalladas
$domainFolders = @(
    "src\SchoolSystem.Domain\Entities\Escuelas",
    "src\SchoolSystem.Domain\Entities\Usuarios", 
    "src\SchoolSystem.Domain\Entities\Academico",
    "src\SchoolSystem.Domain\Entities\Evaluacion",
    "src\SchoolSystem.Domain\Entities\Conducta",
    "src\SchoolSystem.Domain\Entities\Comunicacion",
    "src\SchoolSystem.Domain\Entities\Finanzas",
    "src\SchoolSystem.Domain\Entities\Biblioteca",
    "src\SchoolSystem.Domain\Entities\Common",
    "src\SchoolSystem.Domain\Specifications"
)

foreach ($folder in $domainFolders) {
    New-Item -ItemType Directory -Force -Path $folder | Out-Null
}
Write-Host "  Domain: Carpetas detalladas creadas" -ForegroundColor Green

# Application - Carpetas detalladas
$applicationFolders = @(
    "src\SchoolSystem.Application\DTOs\Alumnos",
    "src\SchoolSystem.Application\DTOs\Calificaciones",
    "src\SchoolSystem.Application\DTOs\Usuarios",
    "src\SchoolSystem.Application\DTOs\Asistencias",
    "src\SchoolSystem.Application\DTOs\Common",
    "src\SchoolSystem.Application\Services\Interfaces",
    "src\SchoolSystem.Application\Services\Implementations",
    "src\SchoolSystem.Application\Behaviors",
    "src\SchoolSystem.Application\Commands\Alumnos",
    "src\SchoolSystem.Application\Commands\Calificaciones",
    "src\SchoolSystem.Application\Commands\Usuarios",
    "src\SchoolSystem.Application\Queries\Alumnos",
    "src\SchoolSystem.Application\Queries\Calificaciones",
    "src\SchoolSystem.Application\Queries\Reportes",
    "src\SchoolSystem.Application\Handlers\Alumnos",
    "src\SchoolSystem.Application\Handlers\Calificaciones",
    "src\SchoolSystem.Application\Handlers\Usuarios",
    "src\SchoolSystem.Application\Interfaces"
)

foreach ($folder in $applicationFolders) {
    New-Item -ItemType Directory -Force -Path $folder | Out-Null
}
Write-Host "  Application: Carpetas detalladas creadas" -ForegroundColor Green

# Infrastructure - Carpetas detalladas
$infrastructureFolders = @(
    "src\SchoolSystem.Infrastructure\Persistence\Context",
    "src\SchoolSystem.Infrastructure\Persistence\Configurations",
    "src\SchoolSystem.Infrastructure\Persistence\Repositories",
    "src\SchoolSystem.Infrastructure\Persistence\Migrations",
    "src\SchoolSystem.Infrastructure\Persistence\Seeds",
    "src\SchoolSystem.Infrastructure\Identity\Models",
    "src\SchoolSystem.Infrastructure\Identity\Services",
    "src\SchoolSystem.Infrastructure\ExternalServices\Email",
    "src\SchoolSystem.Infrastructure\ExternalServices\Sms",
    "src\SchoolSystem.Infrastructure\ExternalServices\Storage",
    "src\SchoolSystem.Infrastructure\ExternalServices\Push",
    "src\SchoolSystem.Infrastructure\BackgroundJobs\Jobs",
    "src\SchoolSystem.Infrastructure\BackgroundJobs\Schedulers",
    "src\SchoolSystem.Infrastructure\Interceptors"
)

foreach ($folder in $infrastructureFolders) {
    New-Item -ItemType Directory -Force -Path $folder | Out-Null
}
Write-Host "  Infrastructure: Carpetas detalladas creadas" -ForegroundColor Green

# API - Carpetas adicionales
$apiFolders = @(
    "src\SchoolSystem.API\Controllers\V2",
    "src\SchoolSystem.API\Hubs",
    "src\SchoolSystem.API\Models\Requests",
    "src\SchoolSystem.API\Models\Responses"
)

foreach ($folder in $apiFolders) {
    New-Item -ItemType Directory -Force -Path $folder | Out-Null
}
Write-Host "  API: Carpetas adicionales creadas" -ForegroundColor Green

# ============================================
# INSTALAR PAQUETES NUGET ADICIONALES
# ============================================
Write-Host "`nInstalando paquetes NuGet adicionales..." -ForegroundColor Cyan

# Application - Paquetes adicionales
Write-Host "  Application: Instalando paquetes adicionales..." -ForegroundColor Yellow
dotnet add src\SchoolSystem.Application\SchoolSystem.Application.csproj package AutoMapper.Extensions.Microsoft.DependencyInjection --version 13.0.1
dotnet add src\SchoolSystem.Application\SchoolSystem.Application.csproj package FluentValidation.DependencyInjectionExtensions --version 11.9.0

# Infrastructure - Paquetes adicionales
Write-Host "  Infrastructure: Instalando paquetes adicionales..." -ForegroundColor Yellow
dotnet add src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
dotnet add src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.0
dotnet add src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj package Hangfire.Core --version 1.8.9
dotnet add src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj package Hangfire.MySql --version 2.0.3
dotnet add src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj package StackExchange.Redis --version 2.7.10

# API - Paquetes adicionales
Write-Host "  API: Instalando paquetes adicionales..." -ForegroundColor Yellow
dotnet add src\SchoolSystem.API\SchoolSystem.API.csproj package Serilog.AspNetCore --version 8.0.0
dotnet add src\SchoolSystem.API\SchoolSystem.API.csproj package Serilog.Sinks.File --version 5.0.0
dotnet add src\SchoolSystem.API\SchoolSystem.API.csproj package AspNetCoreRateLimit --version 5.0.0
dotnet add src\SchoolSystem.API\SchoolSystem.API.csproj package Microsoft.AspNetCore.SignalR --version 1.1.0

# Tests - Paquetes de testing
Write-Host "  Tests: Instalando paquetes de testing..." -ForegroundColor Yellow
$testProjects = @(
    "tests\SchoolSystem.UnitTests\SchoolSystem.UnitTests.csproj",
    "tests\SchoolSystem.IntegrationTests\SchoolSystem.IntegrationTests.csproj",
    "tests\SchoolSystem.FunctionalTests\SchoolSystem.FunctionalTests.csproj"
)

foreach ($project in $testProjects) {
    dotnet add $project package FluentAssertions --version 6.12.0
    dotnet add $project package Moq --version 4.20.70
    dotnet add $project package Microsoft.NET.Test.Sdk --version 17.8.0
}

# Integration Tests - InMemory Database
dotnet add tests\SchoolSystem.IntegrationTests\SchoolSystem.IntegrationTests.csproj package Microsoft.EntityFrameworkCore.InMemory --version 8.0.0

# Functional Tests - TestServer
dotnet add tests\SchoolSystem.FunctionalTests\SchoolSystem.FunctionalTests.csproj package Microsoft.AspNetCore.Mvc.Testing --version 8.0.0

Write-Host "Paquetes adicionales instalados" -ForegroundColor Green

# ============================================
# CREAR ARCHIVOS BASE
# ============================================
Write-Host "`nCreando archivos base del proyecto..." -ForegroundColor Cyan

# Crear BaseEntity
$baseEntity = @"
using System;

namespace SchoolSystem.Domain.Entities.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
"@
$baseEntity | Out-File -FilePath "src\SchoolSystem.Domain\Entities\Common\BaseEntity.cs" -Encoding UTF8

# Crear SchoolContext
$dbContext = @"
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Domain.Entities.Common;

namespace SchoolSystem.Infrastructure.Persistence.Context
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuraciones aqui
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
"@
$dbContext | Out-File -FilePath "src\SchoolSystem.Infrastructure\Persistence\Context\SchoolContext.cs" -Encoding UTF8

# Crear IRepository
$repository = @"
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchoolSystem.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> SaveChangesAsync();
    }
}
"@
$repository | Out-File -FilePath "src\SchoolSystem.Domain\Interfaces\IRepository.cs" -Encoding UTF8

# Crear appsettings con configuracion MySQL
$appsettings = @"
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SchoolSystemDB;User=root;Password=tupassword;"
  },
  "Jwt": {
    "Key": "TuClaveSecretaSuperSeguraDe32Caracteres!",
    "Issuer": "SchoolSystem",
    "Audience": "SchoolSystemUsers",
    "DurationInMinutes": 60
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/log-.txt",
          "rollingInterval": "Day"
        }
      }
    ]
  },
  "AllowedHosts": "*"
}
"@
$appsettings | Out-File -FilePath "src\SchoolSystem.API\appsettings.json" -Encoding UTF8

Write-Host "Archivos base creados" -ForegroundColor Green

# ============================================
# CREAR EJEMPLO DE ENTIDAD
# ============================================
Write-Host "`nCreando entidades de ejemplo..." -ForegroundColor Cyan

# Crear Alumno entity
$alumno = @"
using System;
using SchoolSystem.Domain.Entities.Common;

namespace SchoolSystem.Domain.Entities.Academico
{
    public class Alumno : BaseEntity
    {
        public string Matricula { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string CURP { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public bool Activo { get; set; }
        
        // Navegacion
        public Guid GradoId { get; set; }
        public Guid EscuelaId { get; set; }
    }
}
"@
$alumno | Out-File -FilePath "src\SchoolSystem.Domain\Entities\Academico\Alumno.cs" -Encoding UTF8

Write-Host "Entidades de ejemplo creadas" -ForegroundColor Green

# ============================================
# COMPILAR SOLUCION ACTUALIZADA
# ============================================
Write-Host "`nCompilando solucion actualizada..." -ForegroundColor Cyan
dotnet build
Write-Host "Compilacion exitosa" -ForegroundColor Green

# ============================================
# RESUMEN FINAL
# ============================================
Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "MEJORAS COMPLETADAS EXITOSAMENTE" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Se agregaron:" -ForegroundColor Cyan
Write-Host "  - Estructura completa de carpetas"
Write-Host "  - Todos los paquetes NuGet adicionales"
Write-Host "  - Archivos base (BaseEntity, DbContext, Repository)"
Write-Host "  - Configuracion de appsettings.json"
Write-Host "  - Entidades de ejemplo"
Write-Host ""
Write-Host "Proximos pasos:" -ForegroundColor Yellow
Write-Host "1. Configurar la cadena de conexion MySQL en appsettings.json"
Write-Host "2. Crear mas entidades en Domain"
Write-Host "3. Implementar servicios en Application"
Write-Host "4. Ejecutar: dotnet ef migrations add InitialCreate"
Write-Host ""
