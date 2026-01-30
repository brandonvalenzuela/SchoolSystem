# ✅ MIGRATION VERIFICATION AT STARTUP

## 📋 Overview

Implementación completa de verificación de migraciones pendientes al iniciar la API SchoolSystem.

**Características:**
- ✅ Detección automática de migraciones pendientes
- ✅ Logging detallado con colores y emojis
- ✅ Configuración diferenciada por environment (Prod, Staging, Dev)
- ✅ Opción de detener la app en producción
- ✅ Opción de aplicar migraciones automáticamente en desarrollo
- ✅ Seguro y enterprise-grade

---

## 🏗️ Arquitectura

```
Program.cs (Startup)
    ↓
VerifyPendingMigrationsAsync()
    ├─ GetPendingMigrationsAsync() (EF Core)
    ├─ Detecta migraciones pendientes
    ├─ Obtiene configuración por environment
    ├─ Loguea estado
    ├─ Toma acciones según env:
    │  ├─ Production: Puede detener la app
    │  ├─ Staging: Advierte sin detener
    │  └─ Development: Advierte o aplica automáticamente
    └─ GetAppliedMigrationsAsync() (Reporte)
```

---

## 📁 Archivos Entregados

### 1. **Extensión Principal**
#### `src/SchoolSystem.API/Extensions/MigrationVerificationExtensions.cs`

```csharp
✅ Método principal: VerifyPendingMigrationsAsync(this WebApplication app)

Funcionalidad:
├─ Detecta migraciones pendientes
├─ Loguea errores en niveles Error/Critical
├─ Lee configuración por environment
├─ Ejecuta acciones según configuración
└─ Loguea migraciones aplicadas
```

**Líneas:** 150+

---

### 2. **Archivos de Configuración**

#### `src/SchoolSystem.API/appsettings.Production.json`
```json
{
  "Database": {
    "MigrationVerification": {
      "ThrowExceptionInProduction": true      // ← CRÍTICO
    }
  }
}
```

#### `src/SchoolSystem.API/appsettings.Staging.json`
```json
{
  "Database": {
    "MigrationVerification": {
      "ThrowExceptionInProduction": false,    // ← Advierte
      "WarnInStaging": true
    }
  }
}
```

#### `src/SchoolSystem.API/appsettings.Development.json`
```json
{
  "Database": {
    "MigrationVerification": {
      "ThrowExceptionInProduction": false,    // ← Nunca lanza
      "WarnInDevelopment": true,
      "AutoApplyInDevelopment": false
    }
  }
}
```

---

### 3. **Integración en Program.cs**

```csharp
// --- ✅ VERIFICACIÓN DE MIGRACIONES PENDIENTES ---
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        logger.LogInformation("🔍 Verificando migraciones pendientes...");
        await app.VerifyPendingMigrationsAsync();
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "❌ Error crítico. La aplicación se detiene.");
        throw;
    }
}

app.Run();
```

---

## 🎯 Comportamiento por Environment

### 🔴 PRODUCTION

```
┌─────────────────────────────────────────┐
│ PRODUCTION MODE                         │
├─────────────────────────────────────────┤
│ Migraciones pendientes encontradas:     │
│ - Migration_001                         │
│ - Migration_002                         │
├─────────────────────────────────────────┤
│ ❌ ACCIÓN: DETENER APLICACIÓN           │
│ 🛑 throw InvalidOperationException      │
│ 📋 Log Level: CRITICAL                  │
└─────────────────────────────────────────┘

Logs:
[14:30:15 ERR] ❌ MIGRACIONES PENDIENTES DETECTADAS: Migration_001, Migration_002
[14:30:15 ERR] ⚠️ La base de datos no está sincronizada con el código.
[14:30:15 ERR] 🛑 STOP: Modo Producción detectado. Deteniendo aplicación.
[14:30:15 CRIT] ❌ ERROR CRÍTICO al verificar migraciones
```

**Configuración:**
```json
"ThrowExceptionInProduction": true
```

---

### 🟡 STAGING

```
┌─────────────────────────────────────────┐
│ STAGING MODE                            │
├─────────────────────────────────────────┤
│ Migraciones pendientes encontradas:     │
│ - Migration_001                         │
├─────────────────────────────────────────┤
│ ⚠️ ACCIÓN: ADVERTIR                     │
│ ⏳ CONTINUAR NORMAL                     │
│ 📋 Log Level: WARNING                   │
└─────────────────────────────────────────┘

Logs:
[14:30:15 WRN] ⚠️ ADVERTENCIA: Migraciones pendientes detectadas
[14:30:15 WRN] Migraciones pendientes: Migration_001
[14:30:15 INF] ✅ Base de datos sincronizada...
```

**Configuración:**
```json
"ThrowExceptionInProduction": false,
"WarnInStaging": true
```

---

### 🟢 DEVELOPMENT

```
Opción 1: Solo Advertir (Recomendado)
┌─────────────────────────────────────────┐
│ DEVELOPMENT MODE (WarnOnly)             │
├─────────────────────────────────────────┤
│ Migraciones pendientes encontradas:     │
│ - Migration_001                         │
├─────────────────────────────────────────┤
│ ⚠️ ACCIÓN: ADVERTIR                     │
│ 🏃 CONTINUAR NORMAL                     │
│ 📋 Log Level: WARNING                   │
└─────────────────────────────────────────┘

Logs:
[14:30:15 WRN] ⚠️ ADVERTENCIA: Migraciones pendientes
[14:30:15 WRN] Ejecuta: dotnet ef database update

---

Opción 2: Aplicar Automáticamente
┌─────────────────────────────────────────┐
│ DEVELOPMENT MODE (AutoApply)            │
├─────────────────────────────────────────┤
│ Migraciones pendientes encontradas:     │
│ - Migration_001                         │
├─────────────────────────────────────────┤
│ 🔄 ACCIÓN: APLICAR AUTOMÁTICAMENTE      │
│ ✅ CONTINUAR NORMAL                     │
│ 📋 Log Level: WARNING → INFO            │
└─────────────────────────────────────────┘

Logs:
[14:30:15 WRN] 🔄 APLICANDO MIGRACIONES AUTOMÁTICAMENTE...
[14:30:16 INF] ✅ Migraciones aplicadas exitosamente.
[14:30:16 INF] 📊 Total de migraciones aplicadas: 5
```

**Configuración:**
```json
"ThrowExceptionInProduction": false,
"WarnInDevelopment": true,
"AutoApplyInDevelopment": false  // o true para auto-apply
```

---

## 🔐 Seguridad

### ✅ Protecciones Implementadas

1. **Producción: Strict Mode**
   ```csharp
   if (config.ThrowExceptionInProduction && !environment.IsDevelopment())
   {
       throw new InvalidOperationException(...);
   }
   ```
   - Nunca permite continuar con migraciones pendientes
   - Obliga a ejecutar `dotnet ef database update` manualmente

2. **Logging Seguro**
   ```csharp
   logger.LogError("❌ MIGRACIONES PENDIENTES DETECTADAS: {migrations}");
   logger.LogCritical(ex, "❌ ERROR CRÍTICO");
   ```
   - Logs en niveles Error/Critical (visible en prod)
   - No expone detalles innecesarios

3. **Scope Seguro**
   ```csharp
   using (var scope = app.Services.CreateScope())
   {
       // Operaciones aisladas
   }
   ```
   - No interfiere con middleware
   - No corrompe DI scope

4. **Exception Handling**
   ```csharp
   catch (Exception ex)
   {
       logger.LogCritical(ex, "❌ Error crítico...");
       if (!environment.IsDevelopment())
       {
           throw;  // ← Relalanzar en prod
       }
   }
   ```

---

## 📊 Flujo de Ejecución

```
1. Inicio de app (Program.cs)
   ↓
2. Todos los servicios configurados
   ↓
3. app.Build()
   ↓
4. VerifyPendingMigrationsAsync() ejecuta
   │
   ├─ Crea scope
   ├─ GetDbContext()
   ├─ GetPendingMigrationsAsync()
   │
   ├─ Si hay pendientes:
   │  ├─ LogError (siempre)
   │  ├─ GetMigrationVerificationConfig()
   │  ├─ if (Production && ThrowException)
   │  │  └─ throw InvalidOperationException
   │  ├─ elif (Development && AutoApply)
   │  │  └─ MigrateAsync() (auto-apply)
   │  └─ else
   │     └─ LogWarning
   │
   └─ GetAppliedMigrationsAsync()
      └─ LogInformation (conteo)
   ↓
5. app.Run()
   ↓
6. Aplicación aceptando solicitudes (si paso 4 exitoso)
```

---

## 🚀 Uso

### En Producción

```bash
# Ejecutar migraciones ANTES de iniciar la app
dotnet ef database update --project src/SchoolSystem.Infrastructure

# Iniciar aplicación (verificará que no hay pendientes)
dotnet run --project src/SchoolSystem.API
```

**Logs esperados:**
```
[14:30:15 INF] 🔍 Verificando migraciones pendientes...
[14:30:15 INF] ✅ Base de datos sincronizada. No hay migraciones pendientes.
[14:30:15 INF] 📊 Total de migraciones aplicadas: 15
```

### En Desarrollo (Opción 1: Manual)

```bash
# Aplicar migraciones cuando las crees
dotnet ef database update --project src/SchoolSystem.Infrastructure

# Iniciar app (verificará)
dotnet run --project src/SchoolSystem.API
```

### En Desarrollo (Opción 2: Automático)

**Editar `appsettings.Development.json`:**
```json
"AutoApplyInDevelopment": true
```

```bash
# Simplemente iniciar app
dotnet run --project src/SchoolSystem.API
```

**Logs:**
```
[14:30:15 INF] 🔍 Verificando migraciones pendientes...
[14:30:15 WRN] 🔄 APLICANDO MIGRACIONES AUTOMÁTICAMENTE (Desarrollo)...
[14:30:16 INF] ✅ Migraciones aplicadas exitosamente.
[14:30:16 INF] 📊 Total de migraciones aplicadas: 15
```

---

## 🧪 Testing

### Test Manual en Producción (Simular)

```csharp
// En launchSettings.json o environment variable
ASPNETCORE_ENVIRONMENT=Production

// Crear una migración sin aplicarla
dotnet ef migrations add TestMigration --project src/SchoolSystem.Infrastructure

// Iniciar app
dotnet run --project src/SchoolSystem.API
```

**Resultado esperado:**
- ❌ Aplicación se detiene
- 🛑 Error crítico en logs
- 📋 Mensaje claro sobre migraciones pendientes

---

## 📝 Configuración Completa

### Por Environment

| Env | ThrowException | AutoApply | Resultado |
|-----|---|---|---|
| Production | ✅ true | N/A | 🛑 STOP |
| Staging | ❌ false | ❌ false | ⚠️ WARN |
| Development | ❌ false | ❌ false | ⚠️ WARN |
| Development | ❌ false | ✅ true | 🔄 AUTO |

### Modificar Comportamiento

**Para detener en Staging:**
```json
// appsettings.Staging.json
"ThrowExceptionInProduction": true
```

**Para auto-aplicar en Producción (NO RECOMENDADO):**
```json
// appsettings.Production.json
"AutoApplyInDevelopment": true  // PELIGRO
```

---

## 🎯 Checklist

- [x] Extensión MigrationVerificationExtensions creada
- [x] Integrada en Program.cs
- [x] Configuración por environment
  - [x] appsettings.Production.json
  - [x] appsettings.Staging.json
  - [x] appsettings.Development.json
- [x] Logging detallado
- [x] Error handling robusto
- [x] Documentación completa
- [x] Seguro para producción

---

## 🔄 Flujo Recomendado

### Para Desarrolladores

```bash
1. Crear migración
   $ dotnet ef migrations add NewFeature

2. Verificar (el app la aplicará automáticamente si está configurado)
   $ dotnet run

3. Si AutoApply = false, ejecutar manualmente
   $ dotnet ef database update

4. Verificar logs
   ✅ "[INF] ✅ Base de datos sincronizada"
```

### Para Deployment (Producción)

```bash
1. En servidor de staging:
   $ dotnet ef database update
   $ dotnet run
   ✅ Verificar "No hay migraciones pendientes"

2. Si todo OK, ir a producción:
   $ dotnet ef database update  (En producción)
   $ dotnet run  (Verificará que no hay pendientes)
   ✅ Si tiene pendientes, FALLARÁ (SEGURO)
```

---

## ❌ Problemas y Soluciones

### Problema: "ThrowExceptionInProduction = true, pero no se detiene"

**Solución:** Verificar ambiente:
```csharp
if (!environment.IsDevelopment())  // ← Debe ser true para prod
{
    throw new InvalidOperationException(...);
}
```

**Verificar:**
```bash
echo $ASPNETCORE_ENVIRONMENT  # Debe ser "Production"
```

### Problema: "Auto-aplicar no funciona en Desarrollo"

**Solución:** Verificar configuración:
```json
// appsettings.Development.json
"AutoApplyInDevelopment": true  // ← Debe ser true
```

### Problema: "Migraciones no se detectan"

**Solución:** Verificar DbContext:
```csharp
await dbContext.Database.GetPendingMigrationsAsync()
```

Debe retornar lista no vacía si hay migraciones no aplicadas.

---

## 📞 Support

### Logs Importantes

Buscar estas líneas en logs para validar:

```
✅ ÉXITO:
[INF] 🔍 Verificando migraciones pendientes...
[INF] ✅ Base de datos sincronizada. No hay migraciones pendientes.
[INF] 📊 Total de migraciones aplicadas: 15

❌ PENDIENTES (Desarrollo):
[WRN] ⚠️ ADVERTENCIA: Migraciones pendientes
[WRN] Migraciones pendientes: Migration_001

❌ PENDIENTES (Producción):
[ERR] ❌ MIGRACIONES PENDIENTES DETECTADAS
[ERR] 🛑 STOP: Modo Producción detectado. Deteniendo...
[CRIT] ❌ ERROR CRÍTICO al verificar migraciones
```

---

## ✅ Status

| Componente | Status |
|-----------|--------|
| Extensión | ✅ Completada |
| Program.cs Integration | ✅ Completada |
| Configuración Prod | ✅ Completada |
| Configuración Staging | ✅ Completada |
| Configuración Dev | ✅ Completada |
| Logging | ✅ Completado |
| Security | ✅ Implementada |
| Documentation | ✅ Completada |
| Testing | ✅ Manual OK |

---

**Status: ✅ READY FOR PRODUCTION**

**Última actualización:** 2024
**Versión:** 1.0
