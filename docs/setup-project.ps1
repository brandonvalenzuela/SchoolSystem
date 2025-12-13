# Script para crear Sistema de Gestion Escolar
Write-Host "INICIANDO CREACION DEL PROYECTO" -ForegroundColor Yellow

# Crear directorio principal
$rootPath = "SchoolSystem"
if (Test-Path $rootPath) {
    Remove-Item -Path $rootPath -Recurse -Force
}
New-Item -ItemType Directory -Force -Path $rootPath | Out-Null
Set-Location $rootPath
Write-Host "Directorio creado: $rootPath" -ForegroundColor Green

# Crear solucion
dotnet new sln -n SchoolSystem
Write-Host "Solucion creada" -ForegroundColor Green

# Crear carpetas principales
New-Item -ItemType Directory -Force -Path "src" | Out-Null
New-Item -ItemType Directory -Force -Path "tests" | Out-Null
New-Item -ItemType Directory -Force -Path "docs" | Out-Null
New-Item -ItemType Directory -Force -Path "scripts" | Out-Null
Write-Host "Carpetas principales creadas" -ForegroundColor Green

# Crear proyectos en src
Set-Location src

# Domain
dotnet new classlib -n SchoolSystem.Domain -f net8.0
Remove-Item -Path "SchoolSystem.Domain\Class1.cs" -ErrorAction SilentlyContinue

# Crear carpetas de Domain
$domainPaths = "Entities", "ValueObjects", "Enums", "Interfaces", "Events", "Exceptions"
foreach ($p in $domainPaths) {
    New-Item -ItemType Directory -Force -Path "SchoolSystem.Domain\$p" | Out-Null
}
Write-Host "Domain creado" -ForegroundColor Green

# Application
dotnet new classlib -n SchoolSystem.Application -f net8.0
Remove-Item -Path "SchoolSystem.Application\Class1.cs" -ErrorAction SilentlyContinue

# Crear carpetas de Application
$appPaths = "DTOs", "Services", "Mappings", "Validators", "Commands", "Queries", "Handlers"
foreach ($p in $appPaths) {
    New-Item -ItemType Directory -Force -Path "SchoolSystem.Application\$p" | Out-Null
}
Write-Host "Application creado" -ForegroundColor Green

# Infrastructure
dotnet new classlib -n SchoolSystem.Infrastructure -f net8.0
Remove-Item -Path "SchoolSystem.Infrastructure\Class1.cs" -ErrorAction SilentlyContinue

# Crear carpetas de Infrastructure
$infraPaths = "Persistence", "Identity", "ExternalServices", "Caching"
foreach ($p in $infraPaths) {
    New-Item -ItemType Directory -Force -Path "SchoolSystem.Infrastructure\$p" | Out-Null
}
Write-Host "Infrastructure creado" -ForegroundColor Green

# API
dotnet new webapi -n SchoolSystem.API -f net8.0

# Crear carpetas de API
$apiPaths = "Controllers\V1", "Middleware", "Filters", "Extensions"
foreach ($p in $apiPaths) {
    New-Item -ItemType Directory -Force -Path "SchoolSystem.API\$p" | Out-Null
}
Write-Host "API creado" -ForegroundColor Green

# Shared
dotnet new classlib -n SchoolSystem.Shared -f net8.0
Remove-Item -Path "SchoolSystem.Shared\Class1.cs" -ErrorAction SilentlyContinue

# Crear carpetas de Shared
$sharedPaths = "Constants", "Helpers", "Extensions"
foreach ($p in $sharedPaths) {
    New-Item -ItemType Directory -Force -Path "SchoolSystem.Shared\$p" | Out-Null
}
Write-Host "Shared creado" -ForegroundColor Green

# Crear proyectos de test
Set-Location ..
Set-Location tests

dotnet new xunit -n SchoolSystem.UnitTests -f net8.0
Write-Host "UnitTests creado" -ForegroundColor Green

dotnet new xunit -n SchoolSystem.IntegrationTests -f net8.0
Write-Host "IntegrationTests creado" -ForegroundColor Green

dotnet new xunit -n SchoolSystem.FunctionalTests -f net8.0
Write-Host "FunctionalTests creado" -ForegroundColor Green

# Volver a raiz
Set-Location ..

# Agregar proyectos a la solucion
Write-Host "Agregando proyectos a la solucion..." -ForegroundColor Cyan
dotnet sln add src\SchoolSystem.Domain\SchoolSystem.Domain.csproj
dotnet sln add src\SchoolSystem.Application\SchoolSystem.Application.csproj
dotnet sln add src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj
dotnet sln add src\SchoolSystem.API\SchoolSystem.API.csproj
dotnet sln add src\SchoolSystem.Shared\SchoolSystem.Shared.csproj
dotnet sln add tests\SchoolSystem.UnitTests\SchoolSystem.UnitTests.csproj
dotnet sln add tests\SchoolSystem.IntegrationTests\SchoolSystem.IntegrationTests.csproj
dotnet sln add tests\SchoolSystem.FunctionalTests\SchoolSystem.FunctionalTests.csproj
Write-Host "Proyectos agregados" -ForegroundColor Green

# Configurar referencias
Write-Host "Configurando referencias..." -ForegroundColor Cyan

# Application references
dotnet add src\SchoolSystem.Application\SchoolSystem.Application.csproj reference src\SchoolSystem.Domain\SchoolSystem.Domain.csproj
dotnet add src\SchoolSystem.Application\SchoolSystem.Application.csproj reference src\SchoolSystem.Shared\SchoolSystem.Shared.csproj

# Infrastructure references  
dotnet add src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj reference src\SchoolSystem.Domain\SchoolSystem.Domain.csproj
dotnet add src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj reference src\SchoolSystem.Application\SchoolSystem.Application.csproj
dotnet add src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj reference src\SchoolSystem.Shared\SchoolSystem.Shared.csproj

# API references
dotnet add src\SchoolSystem.API\SchoolSystem.API.csproj reference src\SchoolSystem.Application\SchoolSystem.Application.csproj
dotnet add src\SchoolSystem.API\SchoolSystem.API.csproj reference src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj
dotnet add src\SchoolSystem.API\SchoolSystem.API.csproj reference src\SchoolSystem.Shared\SchoolSystem.Shared.csproj

Write-Host "Referencias configuradas" -ForegroundColor Green

# Instalar paquetes NuGet basicos
Write-Host "Instalando paquetes NuGet..." -ForegroundColor Cyan

# Application
dotnet add src\SchoolSystem.Application\SchoolSystem.Application.csproj package AutoMapper --version 13.0.1
dotnet add src\SchoolSystem.Application\SchoolSystem.Application.csproj package FluentValidation --version 11.9.0
dotnet add src\SchoolSystem.Application\SchoolSystem.Application.csproj package MediatR --version 12.2.0

# Infrastructure
dotnet add src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj package Pomelo.EntityFrameworkCore.MySql --version 8.0.0

# API
dotnet add src\SchoolSystem.API\SchoolSystem.API.csproj package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0
dotnet add src\SchoolSystem.API\SchoolSystem.API.csproj package Swashbuckle.AspNetCore --version 6.5.0

Write-Host "Paquetes instalados" -ForegroundColor Green

# Compilar
Write-Host "Compilando solucion..." -ForegroundColor Cyan
dotnet build
Write-Host "Compilacion exitosa" -ForegroundColor Green

# Crear README
$readme = "# Sistema de Gestion Escolar`n`nSistema con arquitectura Clean Architecture`n`n## Estructura`n- Domain: Entidades`n- Application: Casos de uso`n- Infrastructure: Persistencia`n- API: Web API`n`n## Ejecutar`ndotnet run --project src/SchoolSystem.API"
$readme | Out-File -FilePath "README.md" -Encoding UTF8

# Crear .gitignore
$gitignore = "bin/`nobj/`n.vs/`n*.user`n*.suo`n.vscode/`n.idea/`n*.log"
$gitignore | Out-File -FilePath ".gitignore" -Encoding UTF8

Write-Host "" 
Write-Host "========================================" -ForegroundColor Green
Write-Host "PROYECTO CREADO EXITOSAMENTE" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Ubicacion: $(Get-Location)" -ForegroundColor Cyan
Write-Host ""
Write-Host "Proximos pasos:" -ForegroundColor Yellow
Write-Host "1. Abrir SchoolSystem.sln en Visual Studio"
Write-Host "2. Configurar connection string en appsettings.json"
Write-Host "3. Crear entidades en Domain"
Write-Host ""