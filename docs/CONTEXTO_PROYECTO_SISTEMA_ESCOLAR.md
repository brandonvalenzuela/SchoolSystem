# 📚 CONTEXTO DEL PROYECTO: SISTEMA DE GESTIÓN ESCOLAR SAAS

## 🎯 DESCRIPCIÓN GENERAL

Sistema integral de gestión escolar multi-tenant diseñado para escuelas de todos los niveles educativos (Kinder a Preparatoria) con capacidades offline-first para áreas rurales con conectividad limitada.

## 🏗️ ARQUITECTURA TÉCNICA

### Stack Tecnológico
- **Backend:** C# .NET Core 8.0 con ASP.NET Core Web API
- **Base de Datos:** MySQL 8.0 con Entity Framework Core
- **Móvil:** Flutter/Dart para app de padres de familia
- **Frontend Web:** Blazor WebAssembly / PWA
- **Desktop:** .NET MAUI para escuelas rurales
- **Caché:** Redis
- **Mensajería:** SignalR para tiempo real
- **Tareas en Background:** Hangfire

### Patrón de Arquitectura
- **Clean Architecture + Domain-Driven Design (DDD)**
- **CQRS con MediatR**
- **Repository + Unit of Work Pattern**
- **Code-First con Entity Framework Core**

### Estructura del Proyecto
```
SchoolSystem/
├── src/
│   ├── SchoolSystem.Domain/        # Entidades y lógica de negocio
│   ├── SchoolSystem.Application/   # Casos de uso y DTOs
│   ├── SchoolSystem.Infrastructure/ # Implementación de BD y servicios externos
│   ├── SchoolSystem.API/           # Controllers y configuración
│   └── SchoolSystem.Shared/        # Código compartido
└── tests/
    ├── SchoolSystem.UnitTests/
    └── SchoolSystem.IntegrationTests/
```

## 📊 MÓDULOS IMPLEMENTADOS

### 1. Multi-Tenancy y Seguridad
- Gestión de múltiples escuelas en una sola BD
- Autenticación JWT
- Roles y permisos granulares
- Aislamiento automático de datos por escuela

### 2. Módulo Académico
- **Entidades:** Alumno, Padre, Maestro, NivelEducativo, Grado, Grupo, Materia, Inscripción, CicloEscolar, PeriodoEscolar
- Soporte para múltiples niveles educativos
- Relaciones muchos-a-muchos (alumno-padre, maestro-materia)

### 3. Módulo de Evaluación
- **Entidades:** PeriodoEvaluacion, Calificacion, Asistencia
- Sistema de calificaciones configurable
- Control de asistencias con justificaciones
- Cálculo automático de promedios

### 4. Módulo de Conducta y Gamificación
- **Entidades:** RegistroConducta, Sancion, AlumnoPuntos, Insignia
- Sistema de puntos y rankings
- Insignias y recompensas
- Registro de incidentes positivos/negativos

### 5. Módulo de Tareas y Actividades
- **Entidades:** Tarea, EntregaTarea
- Control de tareas y entregas
- Archivos adjuntos
- Calificación de tareas

### 6. Módulo de Notificaciones y Comunicación
- **Entidades:** Notificacion, Comunicado, ComunicadoLectura, Mensaje, NotificacionSmsLog
- Sistema multi-canal (Push, SMS, Email)
- Mensajería directa maestro-padre
- Comunicados generales

### 7. Módulo de Calendario y Eventos
- **Entidades:** Evento
- Calendario escolar
- Recordatorios automáticos
- Eventos por nivel/grado/grupo

### 8. Módulo Financiero
- **Entidades:** ConceptoPago, Cargo, Pago, EstadoCuenta
- Control de colegiaturas
- Gestión de adeudos
- Reportes financieros

### 9. Módulo de Expediente Médico
- **Entidades:** ExpedienteMedico, Alergia, Vacuna, Medicamento, HistorialMedico
- Control de información médica
- Alergias y condiciones
- Historial de vacunas

### 10. Módulo de Biblioteca
- **Entidades:** Libro, CategoriaRecurso, Prestamo
- Control de recursos
- Sistema de préstamos

### 11. Módulo de Documentos
- **Entidades:** PlantillaDocumento, Documento, ReportePersonalizado
- Generación de documentos
- Plantillas personalizables

### 12. Módulo de Auditoría
- **Entidades:** LogAuditoria, CambioEntidad, Sincronizacion
- Registro completo de cambios
- Trazabilidad de operaciones

### 13. Módulo de Configuración
- **Entidades:** ConfiguracionEscuela, ParametroSistema, PreferenciaUsuario
- Configuración flexible por escuela
- Parámetros del sistema

## 🔄 CARACTERÍSTICAS ESPECIALES

### Sincronización Offline-First
- Base de datos local SQLite en cliente
- Sincronización diferida cuando hay internet
- Resolución de conflictos por timestamp
- Cola de operaciones pendientes

### Sistema de Notificaciones en Cascada
1. Push Notification (prioridad alta)
2. SMS (si falla push)
3. Email (respaldo)
4. Notificación al maestro (último recurso)

### Multi-Tenant con Aislamiento
- Filtrado automático por EscuelaId
- Interceptores en EF Core
- Validación en todos los endpoints

## 🎯 CASOS DE USO PRINCIPALES

### Para Escuelas Rurales (Offline-First)
- Aplicación de escritorio con BD local
- Sincronización cuando hay conexión
- Operación completa sin internet
- Actualización incremental de datos

### Para Escuelas Urbanas (Online)
- Aplicación web PWA
- Tiempo real con SignalR
- Acceso desde cualquier dispositivo
- Respaldo automático en la nube

### Para Padres de Familia
- App móvil Flutter
- Consulta de calificaciones
- Recepción de notificaciones
- Chat con maestros
- Pago de colegiaturas

## 📈 ESCALABILIDAD

### Capacidades
- Soporta múltiples escuelas (SaaS)
- De 50 a 5,000+ alumnos por escuela
- Múltiples niveles educativos
- Personalización por escuela

### Performance
- Índices optimizados (500+)
- Paginación en todas las consultas
- Caché con Redis
- Lazy loading donde aplica

## 🔐 SEGURIDAD

- Autenticación JWT con refresh tokens
- Encriptación de datos sensibles
- Auditoría completa de operaciones
- Respaldo automático de BD
- HTTPS obligatorio
- Validación en cliente y servidor

## 📋 ESTADO ACTUAL DEL PROYECTO

### ✅ Completado
- 83 entidades principales
- 58 enums
- 83 configuraciones de EF Core
- 500+ índices de BD
- 200+ check constraints
- Estructura completa del proyecto

### 🔄 En Proceso
- Implementación de servicios
- Creación de DTOs
- Controllers de API
- Pruebas unitarias

### ❌ Pendiente
- UI de administración
- App móvil Flutter
- Integración con pasarelas de pago
- Integración con portales gubernamentales
- Documentación de API

## 🚀 COMANDOS ÚTILES

### Crear migraciones
```bash
dotnet ef migrations add NombreMigracion --project src/SchoolSystem.Infrastructure
dotnet ef database update --project src/SchoolSystem.Infrastructure
```

### Ejecutar proyecto
```bash
cd src/SchoolSystem.API
dotnet run
```

### Ejecutar pruebas
```bash
dotnet test
```

## 📝 CONVENCIONES DEL CÓDIGO

- **Idioma:** Entidades en español, infraestructura en inglés
- **Nomenclatura:** PascalCase para clases, camelCase para parámetros
- **Async:** Todos los métodos I/O son async con sufijo Async
- **Logging:** Serilog con structured logging
- **Validaciones:** FluentValidation para DTOs
- **Documentación:** XML comments en español

## 🎯 PRIORIDADES DE DESARROLLO

1. **MVP Inicial:** Gestión básica de alumnos, calificaciones y asistencias
2. **Fase 2:** Comunicación y notificaciones
3. **Fase 3:** Sistema financiero y pagos
4. **Fase 4:** Gamificación y conducta
5. **Fase 5:** Integraciones externas

## 💡 CONSIDERACIONES IMPORTANTES

- El sistema está diseñado para operar tanto online como offline
- La sincronización es crítica para escuelas rurales
- La seguridad y privacidad de datos es prioritaria
- El sistema debe ser intuitivo para usuarios no técnicos
- Debe cumplir con regulaciones educativas mexicanas

## 📧 INFORMACIÓN ADICIONAL

- **Versión de .NET:** 8.0
- **Entity Framework Core:** 8.0
- **MySQL:** 8.0
- **Flutter:** Latest stable
- **Encoding:** UTF-8
- **Timezone:** America/Mexico_City

---

Este documento proporciona el contexto completo del proyecto. Para cualquier desarrollo o modificación, asegúrese de mantener la consistencia con la arquitectura y convenciones establecidas.
