# ============================================
# Script de Backup SQL Server
# Sistema de Gestión Escolar
# ============================================

# Funciones de colores para output
function Write-Success {
    param([string]$Message)
    Write-Host "✓ $Message" -ForegroundColor Green
}

function Write-Info {
    param([string]$Message)
    Write-Host "ℹ $Message" -ForegroundColor Cyan
}

function Write-Warning {
    param([string]$Message)
    Write-Host "⚠ $Message" -ForegroundColor Yellow
}

function Write-Error {
    param([string]$Message)
    Write-Host "✗ $Message" -ForegroundColor Red
}

function Write-Header {
    param([string]$Message)
    Write-Host "`n╔════════════════════════════════════════════════════════╗" -ForegroundColor Magenta
    Write-Host "║  $Message" -ForegroundColor Magenta
    Write-Host "╚════════════════════════════════════════════════════════╝`n" -ForegroundColor Magenta
}

# Configuración
$script:DbName = "school_system"
$script:BackupDir = ".\backups\sqlserver"
$script:RetentionDays = 30
$script:ServerInstance = $null
$script:Credential = $null

# Función para obtener credenciales
function Get-DatabaseCredentials {
    Write-Header "CONFIGURACIÓN DE CONEXIÓN"

    $server = Read-Host "Servidor SQL Server (ej: localhost, .\SQLEXPRESS)"
    if ([string]::IsNullOrWhiteSpace($server)) {
        $server = "localhost"
    }
    $script:ServerInstance = $server

    Write-Info "`nTipo de autenticación:"
    Write-Host "  1. Windows Authentication (Integrated Security)"
    Write-Host "  2. SQL Server Authentication (Usuario/Contraseña)"
    $authType = Read-Host "`nSelecciona tipo de autenticación"

    if ($authType -eq "2") {
        $username = Read-Host "Usuario SQL Server"
        $password = Read-Host "Contraseña" -AsSecureString
        $script:Credential = New-Object System.Management.Automation.PSCredential($username, $password)
    }

    Write-Success "Configuración establecida"
}

# Función para crear directorio de backups
function Initialize-BackupDirectory {
    if (-not (Test-Path $script:BackupDir)) {
        New-Item -ItemType Directory -Force -Path $script:BackupDir | Out-Null
        Write-Success "Directorio de backups creado: $script:BackupDir"
    }
}

# Función para realizar backup
function Invoke-DatabaseBackup {
    Write-Header "INICIANDO BACKUP DE SQL SERVER"

    $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
    $backupFile = "$script:BackupDir\${script:DbName}_backup_$timestamp.bak"

    Write-Info "Base de datos: $script:DbName"
    Write-Info "Servidor: $script:ServerInstance"
    Write-Info "Archivo: $backupFile"
    Write-Info "Fecha: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
    Write-Host ""

    try {
        Write-Info "Ejecutando backup..."

        # Construir comando SQL
        $backupQuery = @"
BACKUP DATABASE [$script:DbName]
TO DISK = N'$backupFile'
WITH
    COMPRESSION,
    CHECKSUM,
    STATS = 10,
    NAME = N'$script:DbName-Full Database Backup',
    DESCRIPTION = N'Backup completo realizado el $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')'
"@

        # Ejecutar backup
        if ($script:Credential) {
            Invoke-Sqlcmd -ServerInstance $script:ServerInstance `
                          -Credential $script:Credential `
                          -Query $backupQuery `
                          -QueryTimeout 0 `
                          -ErrorAction Stop
        } else {
            Invoke-Sqlcmd -ServerInstance $script:ServerInstance `
                          -Query $backupQuery `
                          -QueryTimeout 0 `
                          -ErrorAction Stop
        }

        Write-Success "Backup completado exitosamente"

        # Obtener información del archivo
        if (Test-Path $backupFile) {
            $fileInfo = Get-Item $backupFile
            $fileSize = "{0:N2} MB" -f ($fileInfo.Length / 1MB)
            Write-Info "Tamaño del backup: $fileSize"
            Write-Info "Ubicación: $backupFile"

            return $backupFile
        }

    } catch {
        Write-Error "Error al realizar el backup: $($_.Exception.Message)"
        return $null
    }
}

# Función para verificar backup
function Test-BackupFile {
    param([string]$BackupFile)

    Write-Header "VERIFICANDO BACKUP"

    if (-not (Test-Path $BackupFile)) {
        Write-Error "El archivo de backup no existe: $BackupFile"
        return $false
    }

    Write-Info "Verificando integridad del backup..."

    try {
        $verifyQuery = @"
RESTORE VERIFYONLY
FROM DISK = N'$BackupFile'
WITH CHECKSUM
"@

        if ($script:Credential) {
            Invoke-Sqlcmd -ServerInstance $script:ServerInstance `
                          -Credential $script:Credential `
                          -Query $verifyQuery `
                          -QueryTimeout 0 `
                          -ErrorAction Stop
        } else {
            Invoke-Sqlcmd -ServerInstance $script:ServerInstance `
                          -Query $verifyQuery `
                          -QueryTimeout 0 `
                          -ErrorAction Stop
        }

        Write-Success "El backup es válido y está íntegro"

        # Mostrar información del backup
        $fileInfo = Get-Item $BackupFile
        Write-Info "Información del backup:"
        Write-Host "  - Archivo: $($fileInfo.Name)"
        Write-Host "  - Tamaño: $("{0:N2} MB" -f ($fileInfo.Length / 1MB))"
        Write-Host "  - Fecha: $($fileInfo.LastWriteTime.ToString('yyyy-MM-dd HH:mm:ss'))"

        return $true

    } catch {
        Write-Error "Error al verificar el backup: $($_.Exception.Message)"
        return $false
    }
}

# Función para listar backups
function Get-BackupList {
    Write-Header "BACKUPS DISPONIBLES"

    if (-not (Test-Path $script:BackupDir)) {
        Write-Warning "El directorio de backups no existe"
        return
    }

    $backups = Get-ChildItem -Path $script:BackupDir -Filter "${script:DbName}_backup_*.bak" |
               Sort-Object LastWriteTime -Descending

    if ($backups.Count -eq 0) {
        Write-Warning "No hay backups disponibles"
        return
    }

    Write-Info "Total de backups: $($backups.Count)"
    Write-Host ""

    $backups | ForEach-Object {
        $size = "{0:N2} MB" -f ($_.Length / 1MB)
        $date = $_.LastWriteTime.ToString('yyyy-MM-dd HH:mm:ss')
        Write-Host "  - $($_.Name)"
        Write-Host "    Tamaño: $size | Fecha: $date" -ForegroundColor Gray
    }
}

# Función para restaurar backup
function Restore-DatabaseBackup {
    Write-Header "RESTAURAR BACKUP"

    Get-BackupList
    Write-Host ""

    $backupName = Read-Host "Ingresa el nombre del archivo a restaurar (o 'cancelar' para salir)"

    if ($backupName -eq "cancelar") {
        Write-Info "Operación cancelada"
        return
    }

    $backupPath = Join-Path $script:BackupDir $backupName

    if (-not (Test-Path $backupPath)) {
        Write-Error "El archivo no existe: $backupPath"
        return
    }

    Write-Warning "`n⚠️  ADVERTENCIA: Esta operación sobrescribirá la base de datos actual"
    Write-Warning "⚠️  Se cerrarán todas las conexiones activas a la base de datos"
    $confirm = Read-Host "`n¿Estás seguro? (escriba 'SI' para confirmar)"

    if ($confirm -ne "SI") {
        Write-Info "Operación cancelada"
        return
    }

    try {
        Write-Info "`nCerrando conexiones activas..."

        # Poner la base de datos en modo SINGLE_USER para cerrar conexiones
        $setSingleUserQuery = @"
ALTER DATABASE [$script:DbName]
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE
"@

        if ($script:Credential) {
            Invoke-Sqlcmd -ServerInstance $script:ServerInstance `
                          -Credential $script:Credential `
                          -Query $setSingleUserQuery `
                          -ErrorAction Stop
        } else {
            Invoke-Sqlcmd -ServerInstance $script:ServerInstance `
                          -Query $setSingleUserQuery `
                          -ErrorAction Stop
        }

        Write-Info "Restaurando base de datos..."

        # Obtener archivos lógicos del backup
        $fileListQuery = "RESTORE FILELISTONLY FROM DISK = N'$backupPath'"

        if ($script:Credential) {
            $files = Invoke-Sqlcmd -ServerInstance $script:ServerInstance `
                                   -Credential $script:Credential `
                                   -Query $fileListQuery
        } else {
            $files = Invoke-Sqlcmd -ServerInstance $script:ServerInstance `
                                   -Query $fileListQuery
        }

        # Construir comando RESTORE
        $restoreQuery = @"
RESTORE DATABASE [$script:DbName]
FROM DISK = N'$backupPath'
WITH
    REPLACE,
    STATS = 10
"@

        # Ejecutar restauración
        if ($script:Credential) {
            Invoke-Sqlcmd -ServerInstance $script:ServerInstance `
                          -Credential $script:Credential `
                          -Query $restoreQuery `
                          -QueryTimeout 0 `
                          -ErrorAction Stop
        } else {
            Invoke-Sqlcmd -ServerInstance $script:ServerInstance `
                          -Query $restoreQuery `
                          -QueryTimeout 0 `
                          -ErrorAction Stop
        }

        # Restaurar modo MULTI_USER
        $setMultiUserQuery = @"
ALTER DATABASE [$script:DbName]
SET MULTI_USER
"@

        if ($script:Credential) {
            Invoke-Sqlcmd -ServerInstance $script:ServerInstance `
                          -Credential $script:Credential `
                          -Query $setMultiUserQuery `
                          -ErrorAction Stop
        } else {
            Invoke-Sqlcmd -ServerInstance $script:ServerInstance `
                          -Query $setMultiUserQuery `
                          -ErrorAction Stop
        }

        Write-Success "Base de datos restaurada exitosamente"

    } catch {
        Write-Error "Error al restaurar la base de datos: $($_.Exception.Message)"

        # Intentar restaurar modo MULTI_USER en caso de error
        try {
            $setMultiUserQuery = "ALTER DATABASE [$script:DbName] SET MULTI_USER"
            if ($script:Credential) {
                Invoke-Sqlcmd -ServerInstance $script:ServerInstance `
                              -Credential $script:Credential `
                              -Query $setMultiUserQuery `
                              -ErrorAction SilentlyContinue
            } else {
                Invoke-Sqlcmd -ServerInstance $script:ServerInstance `
                              -Query $setMultiUserQuery `
                              -ErrorAction SilentlyContinue
            }
        } catch {
            # Ignorar errores al restaurar modo MULTI_USER
        }
    }
}

# Función para limpiar backups antiguos
function Remove-OldBackups {
    Write-Header "LIMPIANDO BACKUPS ANTIGUOS"

    Write-Info "Buscando backups con más de $script:RetentionDays días..."

    $cutoffDate = (Get-Date).AddDays(-$script:RetentionDays)
    $oldBackups = Get-ChildItem -Path $script:BackupDir -Filter "${script:DbName}_backup_*.bak" |
                  Where-Object { $_.LastWriteTime -lt $cutoffDate }

    if ($oldBackups.Count -eq 0) {
        Write-Info "No hay backups antiguos para eliminar"
        return
    }

    Write-Warning "Se encontraron $($oldBackups.Count) backup(s) antiguo(s)"

    foreach ($backup in $oldBackups) {
        Remove-Item $backup.FullName -Force
        Write-Info "Eliminado: $($backup.Name)"
    }

    Write-Success "Se eliminaron $($oldBackups.Count) backup(s) antiguo(s)"
}

# Función para crear backup programado
function New-ScheduledBackup {
    Write-Header "CREAR BACKUP PROGRAMADO"

    Write-Info "Esta función creará una tarea programada en Windows para realizar backups automáticos"
    Write-Host ""

    $taskName = "SchoolSystem_SQLServer_Backup"

    # Verificar si ya existe la tarea
    $existingTask = Get-ScheduledTask -TaskName $taskName -ErrorAction SilentlyContinue

    if ($existingTask) {
        Write-Warning "Ya existe una tarea programada con el nombre: $taskName"
        $overwrite = Read-Host "¿Deseas sobrescribirla? (S/N)"

        if ($overwrite -ne 'S' -and $overwrite -ne 's') {
            Write-Info "Operación cancelada"
            return
        }

        Unregister-ScheduledTask -TaskName $taskName -Confirm:$false
    }

    Write-Info "Configuración del backup programado:"
    $hour = Read-Host "Hora del día para ejecutar (0-23)"
    $minute = Read-Host "Minuto (0-59)"

    # Crear acción
    $action = New-ScheduledTaskAction -Execute "PowerShell.exe" `
                                      -Argument "-ExecutionPolicy Bypass -File `"$PSCommandPath`" -AutoBackup"

    # Crear trigger (diario)
    $trigger = New-ScheduledTaskTrigger -Daily -At "$hour:$minute"

    # Configurar settings
    $settings = New-ScheduledTaskSettingsSet -StartWhenAvailable -RunOnlyIfNetworkAvailable

    # Registrar tarea
    Register-ScheduledTask -TaskName $taskName `
                          -Action $action `
                          -Trigger $trigger `
                          -Settings $settings `
                          -Description "Backup automático de la base de datos del Sistema Escolar" `
                          -RunLevel Highest

    Write-Success "Tarea programada creada exitosamente"
    Write-Info "Nombre: $taskName"
    Write-Info "Frecuencia: Diaria a las $hour:$minute"
}

# Menú principal
function Show-Menu {
    Clear-Host
    Write-Host "╔════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
    Write-Host "║                                                            ║" -ForegroundColor Cyan
    Write-Host "║          BACKUP Y RESTAURACIÓN SQL SERVER                  ║" -ForegroundColor Cyan
    Write-Host "║          Sistema de Gestión Escolar                        ║" -ForegroundColor Cyan
    Write-Host "║                                                            ║" -ForegroundColor Cyan
    Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "  1. Realizar backup"
    Write-Host "  2. Listar backups disponibles"
    Write-Host "  3. Restaurar backup"
    Write-Host "  4. Verificar backup"
    Write-Host "  5. Limpiar backups antiguos"
    Write-Host "  6. Crear backup programado"
    Write-Host "  7. Salir"
    Write-Host ""
}

# Script principal
function Main {
    param([switch]$AutoBackup)

    # Verificar que el módulo SqlServer está instalado
    if (-not (Get-Module -ListAvailable -Name SqlServer)) {
        Write-Error "El módulo SqlServer no está instalado"
        Write-Info "Instálalo con: Install-Module -Name SqlServer -Scope CurrentUser"
        exit 1
    }

    # Importar módulo
    Import-Module SqlServer -ErrorAction Stop

    # Si es backup automático, ejecutar y salir
    if ($AutoBackup) {
        Get-DatabaseCredentials
        Initialize-BackupDirectory
        $backupFile = Invoke-DatabaseBackup
        if ($backupFile) {
            Test-BackupFile -BackupFile $backupFile
            Remove-OldBackups
        }
        exit 0
    }

    # Obtener credenciales
    Get-DatabaseCredentials

    # Crear directorio de backups
    Initialize-BackupDirectory

    # Menú interactivo
    do {
        Show-Menu
        $option = Read-Host "Selecciona una opción"

        switch ($option) {
            '1' {
                $backupFile = Invoke-DatabaseBackup
                if ($backupFile) {
                    Test-BackupFile -BackupFile $backupFile
                }
            }
            '2' { Get-BackupList }
            '3' { Restore-DatabaseBackup }
            '4' {
                $backupName = Read-Host "Ingresa el nombre del archivo a verificar"
                $backupPath = Join-Path $script:BackupDir $backupName
                Test-BackupFile -BackupFile $backupPath
            }
            '5' { Remove-OldBackups }
            '6' { New-ScheduledBackup }
            '7' {
                Write-Success "`n¡Hasta luego!"
                return
            }
            default { Write-Warning "`n⚠️  Opción no válida. Por favor selecciona 1-7." }
        }

        if ($option -ne '7') {
            Write-Host "`nPresiona Enter para continuar..." -ForegroundColor Gray
            Read-Host
        }
    } while ($option -ne '7')
}

# Ejecutar script
Main @args
