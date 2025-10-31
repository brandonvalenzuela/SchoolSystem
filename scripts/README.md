# Scripts de Base de Datos - Sistema de Gestión Escolar

Este directorio contiene todos los scripts necesarios para la gestión de bases de datos del Sistema de Gestión Escolar, incluyendo scripts de migración y respaldo para MySQL y SQL Server.

## 📁 Contenido

### Scripts de Migración

#### `DB_MySQL.sql`
Script completo de creación de base de datos para **MySQL**.

**Características:**
- Creación completa del esquema de base de datos
- 13 módulos funcionales (Multi-tenancy, Académico, Evaluación, Conducta, Finanzas, Biblioteca, Médico, Comunicación, Documentos, Calendario, Auditoría, Configuración, Tareas)
- Más de 50 tablas con relaciones
- Índices optimizados para rendimiento
- Soporte para soft delete y auditoría
- Constraints y validaciones

**Uso:**
```bash
# Conectar a MySQL
mysql -u root -p

# Ejecutar script
source /ruta/a/DB_MySQL.sql

# O desde la línea de comandos
mysql -u usuario -p < DB_MySQL.sql
```

#### `DB_SQLServer.sql`
Script completo de creación de base de datos para **SQL Server**.

**Características:**
- Creación completa del esquema de base de datos
- Compatible con SQL Server 2016+
- Triggers automáticos para updated_at
- Validación JSON para columnas de configuración
- Índices non-clustered para optimización
- Soporte para auditoría y soft delete

**Uso:**
```sql
-- Desde SQL Server Management Studio (SSMS)
-- Abrir el archivo y ejecutar (F5)

-- O desde sqlcmd
sqlcmd -S localhost -i DB_SQLServer.sql
```

#### `DB.sql` (Legacy)
Script original de SQL Server. Se recomienda usar `DB_SQLServer.sql` para nuevas instalaciones.

---

### Scripts de Backup

#### `backup_mysql.sh`
Script interactivo de backup y restauración para **MySQL**.

**Características:**
- ✅ Backup completo con compresión (gzip)
- ✅ Restauración de backups
- ✅ Verificación de integridad
- ✅ Limpieza automática de backups antiguos (30 días por defecto)
- ✅ Listado de backups disponibles
- ✅ Interfaz interactiva con menú
- ✅ Colores y mensajes descriptivos

**Requisitos:**
- MySQL Client (mysqldump)
- gzip
- Bash shell

**Uso:**
```bash
# Dar permisos de ejecución (solo primera vez)
chmod +x backup_mysql.sh

# Ejecutar script
./backup_mysql.sh
```

**Opciones del menú:**
1. **Realizar backup**: Crea un nuevo backup comprimido
2. **Listar backups**: Muestra todos los backups disponibles
3. **Restaurar backup**: Restaura desde un backup existente
4. **Verificar backup**: Verifica la integridad del último backup
5. **Limpiar backups antiguos**: Elimina backups con más de 30 días
6. **Salir**: Cierra el script

**Ubicación de backups:**
```
./backups/mysql/
  ├── school_system_backup_20250131_143022.sql.gz
  ├── school_system_backup_20250130_143022.sql.gz
  └── ...
```

#### `backup_sqlserver.ps1`
Script interactivo de backup y restauración para **SQL Server**.

**Características:**
- ✅ Backup completo con compresión nativa
- ✅ Restauración de backups
- ✅ Verificación de integridad (RESTORE VERIFYONLY)
- ✅ Limpieza automática de backups antiguos
- ✅ Creación de tareas programadas de Windows
- ✅ Soporte para Windows Authentication y SQL Authentication
- ✅ Interfaz interactiva con menú
- ✅ Colores y mensajes descriptivos

**Requisitos:**
- PowerShell 5.1 o superior
- Módulo SqlServer
- SQL Server 2012 o superior

**Instalación del módulo SqlServer:**
```powershell
Install-Module -Name SqlServer -Scope CurrentUser
```

**Uso:**
```powershell
# Ejecutar script
.\backup_sqlserver.ps1

# Para backup automático (usado por tareas programadas)
.\backup_sqlserver.ps1 -AutoBackup
```

**Opciones del menú:**
1. **Realizar backup**: Crea un nuevo backup con compresión
2. **Listar backups**: Muestra todos los backups disponibles
3. **Restaurar backup**: Restaura desde un backup existente
4. **Verificar backup**: Verifica la integridad de un backup
5. **Limpiar backups antiguos**: Elimina backups con más de 30 días
6. **Crear backup programado**: Crea una tarea programada de Windows
7. **Salir**: Cierra el script

**Ubicación de backups:**
```
.\backups\sqlserver\
  ├── school_system_backup_20250131_143022.bak
  ├── school_system_backup_20250130_143022.bak
  └── ...
```

---

## 🚀 Guía de Inicio Rápido

### Instalación Inicial - MySQL

```bash
# 1. Crear la base de datos
mysql -u root -p < DB_MySQL.sql

# 2. Verificar creación
mysql -u root -p school_system -e "SHOW TABLES;"

# 3. Configurar backups
./backup_mysql.sh
```

### Instalación Inicial - SQL Server

```powershell
# 1. Desde SSMS o sqlcmd
sqlcmd -S localhost -i DB_SQLServer.sql

# 2. Verificar creación
sqlcmd -S localhost -d school_system -Q "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES"

# 3. Configurar backups
.\backup_sqlserver.ps1
```

---

## 🔧 Configuración

### Parámetros de Backup MySQL

Editar variables en `backup_mysql.sh`:
```bash
DB_NAME="school_system"           # Nombre de la base de datos
BACKUP_DIR="./backups/mysql"      # Directorio de backups
RETENTION_DAYS=30                 # Días de retención
```

### Parámetros de Backup SQL Server

Editar variables en `backup_sqlserver.ps1`:
```powershell
$DbName = "school_system"           # Nombre de la base de datos
$BackupDir = ".\backups\sqlserver"  # Directorio de backups
$RetentionDays = 30                 # Días de retención
```

---

## 📊 Estructura de la Base de Datos

La base de datos consta de 13 módulos principales:

### 1. **Multi-tenancy y Seguridad**
- Escuelas (tenants)
- Usuarios (multi-rol)
- Dispositivos

### 2. **Académico**
- Niveles educativos
- Grados
- Materias
- Grupos
- Alumnos
- Padres/Tutores
- Maestros
- Inscripciones

### 3. **Evaluación**
- Períodos de evaluación
- Calificaciones
- Asistencias

### 4. **Conducta**
- Registros de conducta
- Sistema de puntos (gamificación)
- Insignias
- Sanciones

### 5. **Finanzas**
- Conceptos de pago
- Cargos
- Pagos
- Estados de cuenta

### 6. **Biblioteca**
- Categorías de recursos
- Libros
- Préstamos

### 7. **Médico**
- Expedientes médicos
- Vacunas
- Alergias
- Medicamentos
- Historial médico

### 8. **Comunicación**
- Mensajes
- Notificaciones
- Comunicados
- Log de SMS

### 9. **Documentos**
- Plantillas de documentos
- Documentos generados
- Reportes personalizados

### 10. **Calendario**
- Eventos

### 11. **Auditoría**
- Logs de auditoría
- Cambios en entidades
- Sincronizaciones

### 12. **Configuración**
- Configuración por escuela
- Parámetros del sistema
- Preferencias de usuario

### 13. **Tareas**
- Tareas escolares
- Entregas de tareas

---

## 🔒 Seguridad y Mejores Prácticas

### Backups

1. **Frecuencia recomendada**: Diaria
2. **Retención**: Mínimo 30 días, recomendado 90 días para producción
3. **Ubicación**: Almacenar backups en ubicación diferente al servidor de base de datos
4. **Verificación**: Siempre verificar la integridad de los backups
5. **Restauración**: Probar restauraciones periódicamente

### Seguridad

1. **Credenciales**: Nunca almacenar credenciales en texto plano
2. **Permisos**: Usar principio de menor privilegio
3. **Auditoría**: Revisar logs de auditoría regularmente
4. **Encriptación**: Considerar encriptación de datos sensibles
5. **Backups**: Encriptar backups en producción

### Monitoreo

1. Monitorear espacio en disco
2. Revisar logs de errores
3. Monitorear tamaño de backups
4. Alertas de fallos en backups
5. Monitorear rendimiento de consultas

---

## 🆘 Solución de Problemas

### MySQL

**Error: Access denied**
```bash
# Verificar credenciales
mysql -u usuario -p

# Verificar permisos
SHOW GRANTS FOR 'usuario'@'localhost';
```

**Error: Disk full**
```bash
# Verificar espacio
df -h

# Limpiar backups antiguos
./backup_mysql.sh # Opción 5
```

### SQL Server

**Error: No se puede conectar**
```powershell
# Verificar servicio
Get-Service MSSQLSERVER

# Iniciar servicio
Start-Service MSSQLSERVER
```

**Error: Insufficient permissions**
```sql
-- Verificar permisos
SELECT * FROM fn_my_permissions(NULL, 'DATABASE');

-- Otorgar permisos de backup
GRANT BACKUP DATABASE TO [usuario];
```

---

## 📝 Logs y Auditoría

Los scripts generan logs automáticos:

- **MySQL**: Los errores se muestran en pantalla
- **SQL Server**: Los errores se muestran en pantalla y en el Event Viewer de Windows

Para auditoría completa, revisar:
- Tabla `logs_auditoria`
- Tabla `cambios_entidad`
- Event Viewer de Windows (SQL Server)
- MySQL logs (`/var/log/mysql/error.log`)

---

## 🤝 Contribución

Para contribuir con mejoras a los scripts:

1. Documentar cambios en este README
2. Mantener compatibilidad con versiones existentes
3. Probar exhaustivamente antes de hacer commit
4. Seguir convenciones de código existentes

---

## 📞 Soporte

Para soporte técnico:
- Revisar este README
- Consultar documentación oficial de MySQL/SQL Server
- Revisar logs de error
- Contactar al equipo de desarrollo

---

## 📜 Licencia

Este proyecto es parte del Sistema de Gestión Escolar.
Todos los derechos reservados © 2025

---

## 🔄 Historial de Cambios

### Versión 1.0 (2025-01-31)
- ✨ Script inicial de MySQL (DB_MySQL.sql)
- ✨ Script inicial de SQL Server (DB_SQLServer.sql)
- ✨ Script de backup MySQL (backup_mysql.sh)
- ✨ Script de backup SQL Server (backup_sqlserver.ps1)
- ✨ Documentación completa (README.md)
- ✨ Soporte para 13 módulos funcionales
- ✨ Más de 50 tablas con relaciones
- ✨ Sistema de auditoría y soft delete
- ✨ Limpieza automática de backups antiguos
