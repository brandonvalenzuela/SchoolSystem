# ============================================
# Script de Gestión de Migraciones
# Entity Framework Core - Sistema Escolar
# ============================================

# Colores para output
function Write-Success {
    param([string]$Message)
    Write-Host $Message -ForegroundColor Green
}

function Write-Info {
    param([string]$Message)
    Write-Host $Message -ForegroundColor Cyan
}

function Write-Warning {
    param([string]$Message)
    Write-Host $Message -ForegroundColor Yellow
}

function Write-Error {
    param([string]$Message)
    Write-Host $Message -ForegroundColor Red
}

function Write-Header {
    param([string]$Message)
    Write-Host "`n╔════════════════════════════════════════════════╗" -ForegroundColor Magenta
    Write-Host "║  $Message" -ForegroundColor Magenta
    Write-Host "╚════════════════════════════════════════════════╝`n" -ForegroundColor Magenta
}

# Verificar que estamos en el directorio correcto
function Test-ProjectStructure {
    if (-not (Test-Path "SchoolSystem.sln")) {
        Write-Error "❌ Error: No se encontró SchoolSystem.sln"
        Write-Warning "Por favor, ejecuta este script desde el directorio raíz del proyecto."
        return $false
    }
    
    if (-not (Test-Path "src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj")) {
        Write-Error "❌ Error: No se encontró el proyecto Infrastructure"
        return $false
    }
    
    if (-not (Test-Path "src\SchoolSystem.API\SchoolSystem.API.csproj")) {
        Write-Error "❌ Error: No se encontró el proyecto API"
        return $false
    }
    
    return $true
}

# Verificar que EF Core Tools está instalado
function Test-EFCoreTools {
    $efVersion = dotnet ef --version 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Warning "⚠️  EF Core Tools no está instalado."
        Write-Info "Instalando EF Core Tools..."
        dotnet tool install --global dotnet-ef
        
        if ($LASTEXITCODE -eq 0) {
            Write-Success "✓ EF Core Tools instalado exitosamente"
            return $true
        } else {
            Write-Error "❌ Error al instalar EF Core Tools"
            return $false
        }
    }
    
    Write-Success "✓ EF Core Tools está instalado: $efVersion"
    return $true
}

# Función para crear una nueva migración
function New-Migration {
    Write-Header "CREAR NUEVA MIGRACIÓN"
    
    Write-Info "Ingresa el nombre de la migración (sin espacios, usa PascalCase):"
    Write-Info "Ejemplos: InitialCreate, AddAlumnosTable, UpdateCalificacionesSchema"
    $migrationName = Read-Host "Nombre"
    
    if ([string]::IsNullOrWhiteSpace($migrationName)) {
        Write-Error "❌ El nombre de la migración no puede estar vacío"
        return
    }
    
    # Validar que no tenga espacios
    if ($migrationName -match '\s') {
        Write-Error "❌ El nombre no debe contener espacios. Usa PascalCase (ej: AddAlumnosTable)"
        return
    }
    
    Write-Info "`nCreando migración: $migrationName..."
    Write-Info "Proyecto: SchoolSystem.Infrastructure"
    Write-Info "Startup: SchoolSystem.API`n"
    
    dotnet ef migrations add $migrationName `
        --project src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj `
        --startup-project src\SchoolSystem.API\SchoolSystem.API.csproj `
        --output-dir Persistence\Migrations
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "`n✓ Migración '$migrationName' creada exitosamente"
        Write-Info "Ubicación: src\SchoolSystem.Infrastructure\Persistence\Migrations\"
        Write-Warning "`n⚠️  Recuerda revisar el archivo de migración antes de aplicarla"
    } else {
        Write-Error "`n❌ Error al crear la migración"
    }
}

# Función para aplicar migraciones
function Update-Database {
    Write-Header "APLICAR MIGRACIONES A LA BASE DE DATOS"
    
    Write-Warning "⚠️  Esta operación aplicará todas las migraciones pendientes a la base de datos."
    Write-Info "¿Estás seguro? (S/N)"
    $confirm = Read-Host
    
    if ($confirm -ne 'S' -and $confirm -ne 's') {
        Write-Info "Operación cancelada."
        return
    }
    
    Write-Info "`nAplicando migraciones...`n"
    
    dotnet ef database update `
        --project src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj `
        --startup-project src\SchoolSystem.API\SchoolSystem.API.csproj
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "`n✓ Base de datos actualizada exitosamente"
    } else {
        Write-Error "`n❌ Error al actualizar la base de datos"
    }
}

# Función para revertir migración
function Remove-LastMigration {
    Write-Header "REVERTIR ÚLTIMA MIGRACIÓN"
    
    Write-Warning "⚠️  Esta operación revertirá la última migración aplicada."
    Write-Info "Esto ejecutará el método 'Down' de la migración."
    Write-Info "`n¿Estás seguro? (S/N)"
    $confirm = Read-Host
    
    if ($confirm -ne 'S' -and $confirm -ne 's') {
        Write-Info "Operación cancelada."
        return
    }
    
    Write-Info "`nRevirtiendo última migración...`n"
    
    # Primero revertir en la BD
    dotnet ef database update 0 `
        --project src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj `
        --startup-project src\SchoolSystem.API\SchoolSystem.API.csproj
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "✓ Migración revertida en la base de datos"
    } else {
        Write-Error "❌ Error al revertir la migración en la base de datos"
    }
}

# Función para eliminar última migración
function Delete-LastMigration {
    Write-Header "ELIMINAR ÚLTIMA MIGRACIÓN"
    
    Write-Warning "⚠️  PELIGRO: Esta operación eliminará permanentemente el archivo de la última migración."
    Write-Warning "⚠️  Solo usa esto si la migración NO ha sido aplicada a la base de datos."
    Write-Info "`n¿Estás seguro? (S/N)"
    $confirm = Read-Host
    
    if ($confirm -ne 'S' -and $confirm -ne 's') {
        Write-Info "Operación cancelada."
        return
    }
    
    Write-Info "`nEliminando última migración...`n"
    
    dotnet ef migrations remove `
        --project src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj `
        --startup-project src\SchoolSystem.API\SchoolSystem.API.csproj
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "`n✓ Última migración eliminada exitosamente"
    } else {
        Write-Error "`n❌ Error al eliminar la migración"
        Write-Warning "Si la migración ya fue aplicada, primero debes revertirla."
    }
}

# Función para ver estado de migraciones
function Get-MigrationStatus {
    Write-Header "ESTADO DE MIGRACIONES"
    
    Write-Info "Consultando estado...`n"
    
    dotnet ef migrations list `
        --project src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj `
        --startup-project src\SchoolSystem.API\SchoolSystem.API.csproj
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "`n✓ Listado de migraciones completado"
    } else {
        Write-Error "`n❌ Error al listar migraciones"
    }
}

# Función para recrear base de datos (desarrollo)
function Reset-Database {
    Write-Header "RECREAR BASE DE DATOS COMPLETA"
    
    Write-Warning "⚠️  PELIGRO: Esta operación eliminará y recreará la base de datos completa."
    Write-Warning "⚠️  TODOS LOS DATOS SE PERDERÁN."
    Write-Warning "⚠️  Solo usar en ambiente de desarrollo.`n"
    Write-Info "¿Estás ABSOLUTAMENTE seguro? Escribe 'ELIMINAR' para confirmar:"
    $confirm = Read-Host
    
    if ($confirm -ne 'ELIMINAR') {
        Write-Info "Operación cancelada."
        return
    }
    
    Write-Info "`nEliminando base de datos...`n"
    
    # Eliminar BD
    dotnet ef database drop --force `
        --project src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj `
        --startup-project src\SchoolSystem.API\SchoolSystem.API.csproj
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "✓ Base de datos eliminada"
        
        Write-Info "`nCreando base de datos nuevamente...`n"
        
        # Crear BD de nuevo
        dotnet ef database update `
            --project src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj `
            --startup-project src\SchoolSystem.API\SchoolSystem.API.csproj
        
        if ($LASTEXITCODE -eq 0) {
            Write-Success "`n✓ Base de datos recreada exitosamente"
        } else {
            Write-Error "`n❌ Error al crear la base de datos"
        }
    } else {
        Write-Error "❌ Error al eliminar la base de datos"
    }
}

# Función para aplicar una migración específica
function Update-ToSpecificMigration {
    Write-Header "APLICAR MIGRACIÓN ESPECÍFICA"
    
    Write-Info "Primero, veamos las migraciones disponibles:`n"
    Get-MigrationStatus
    
    Write-Info "`nIngresa el nombre exacto de la migración destino:"
    Write-Info "(Deja vacío y presiona Enter para cancelar)"
    $targetMigration = Read-Host "Migración"
    
    if ([string]::IsNullOrWhiteSpace($targetMigration)) {
        Write-Info "Operación cancelada."
        return
    }
    
    Write-Warning "`n⚠️  Se aplicarán/revertirán migraciones hasta llegar a: $targetMigration"
    Write-Info "¿Continuar? (S/N)"
    $confirm = Read-Host
    
    if ($confirm -ne 'S' -and $confirm -ne 's') {
        Write-Info "Operación cancelada."
        return
    }
    
    Write-Info "`nAplicando migración específica...`n"
    
    dotnet ef database update $targetMigration `
        --project src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj `
        --startup-project src\SchoolSystem.API\SchoolSystem.API.csproj
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "`n✓ Migración aplicada exitosamente"
    } else {
        Write-Error "`n❌ Error al aplicar la migración"
    }
}

# Función para generar script SQL de migración
function Export-MigrationScript {
    Write-Header "GENERAR SCRIPT SQL DE MIGRACIONES"
    
    Write-Info "Este script generará un archivo SQL con todas las migraciones pendientes."
    Write-Info "Útil para aplicar manualmente en producción.`n"
    
    Write-Info "Ingresa el nombre del archivo SQL (sin extensión):"
    $scriptName = Read-Host "Nombre"
    
    if ([string]::IsNullOrWhiteSpace($scriptName)) {
        $scriptName = "migration_script_$(Get-Date -Format 'yyyyMMdd_HHmmss')"
    }
    
    $scriptPath = "scripts\$scriptName.sql"
    
    Write-Info "`nGenerando script SQL...`n"
    
    # Crear carpeta scripts si no existe
    if (-not (Test-Path "scripts")) {
        New-Item -ItemType Directory -Force -Path "scripts" | Out-Null
    }
    
    dotnet ef migrations script `
        --project src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj `
        --startup-project src\SchoolSystem.API\SchoolSystem.API.csproj `
        --output $scriptPath `
        --idempotent
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "`n✓ Script SQL generado exitosamente"
        Write-Info "Ubicación: $scriptPath"
    } else {
        Write-Error "`n❌ Error al generar el script SQL"
    }
}

# ============================================
# MENÚ PRINCIPAL
# ============================================

function Show-Menu {
    Clear-Host
    Write-Host "╔════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
    Write-Host "║                                                            ║" -ForegroundColor Cyan
    Write-Host "║        GESTIÓN DE MIGRACIONES - SISTEMA ESCOLAR           ║" -ForegroundColor Cyan
    Write-Host "║                Entity Framework Core                       ║" -ForegroundColor Cyan
    Write-Host "║                                                            ║" -ForegroundColor Cyan
    Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "  1. Crear nueva migración" -ForegroundColor White
    Write-Host "  2. Aplicar migraciones pendientes" -ForegroundColor White
    Write-Host "  3. Ver estado de migraciones" -ForegroundColor White
    Write-Host "  4. Aplicar migración específica" -ForegroundColor White
    Write-Host "  5. Revertir última migración (en BD)" -ForegroundColor Yellow
    Write-Host "  6. Eliminar última migración (archivo)" -ForegroundColor Yellow
    Write-Host "  7. Generar script SQL" -ForegroundColor White
    Write-Host "  8. Recrear base de datos completa" -ForegroundColor Red -BackgroundColor Black
    Write-Host "  9. Salir" -ForegroundColor White
    Write-Host ""
}

# ============================================
# SCRIPT PRINCIPAL
# ============================================

# Verificar estructura del proyecto
if (-not (Test-ProjectStructure)) {
    Write-Host "`nPresiona cualquier tecla para salir..."
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    exit
}

# Verificar EF Core Tools
if (-not (Test-EFCoreTools)) {
    Write-Host "`nPresiona cualquier tecla para salir..."
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    exit
}

# Loop principal del menú
do {
    Show-Menu
    $option = Read-Host "Selecciona una opción"
    
    switch ($option) {
        '1' { New-Migration }
        '2' { Update-Database }
        '3' { Get-MigrationStatus }
        '4' { Update-ToSpecificMigration }
        '5' { Remove-LastMigration }
        '6' { Delete-LastMigration }
        '7' { Export-MigrationScript }
        '8' { Reset-Database }
        '9' { 
            Write-Success "`n¡Hasta luego!"
            return 
        }
        default { 
            Write-Warning "`n⚠️  Opción no válida. Por favor selecciona 1-9."
        }
    }
    
    if ($option -ne '9') {
        Write-Host "`nPresiona cualquier tecla para continuar..."
        $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    }
} while ($option -ne '9')