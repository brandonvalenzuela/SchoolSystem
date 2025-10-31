using Microsoft.EntityFrameworkCore;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Auditoria;
using SchoolSystem.Domain.Entities.Biblioteca;
using SchoolSystem.Domain.Entities.Calendario;
using SchoolSystem.Domain.Entities.Comunicacion;
using SchoolSystem.Domain.Entities.Conducta;
using SchoolSystem.Domain.Entities.Configuracion;
using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Documentos;
using SchoolSystem.Domain.Entities.Escuelas;
using SchoolSystem.Domain.Entities.Evaluacion;
using SchoolSystem.Domain.Entities.Finanzas;
using SchoolSystem.Domain.Entities.Medico;
using SchoolSystem.Domain.Entities.Tareas;
using SchoolSystem.Domain.Entities.Usuarios;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolSystem.Infrastructure.Persistence.Context
{
    /// <summary>
    /// DbContext principal del Sistema de Gestión Escolar.
    /// Maneja todas las entidades y configuraciones de la base de datos.
    /// </summary>
    public class SchoolSystemDbContext : DbContext
    {
        // TODO: Inyectar servicio para obtener el tenant (escuela) actual
        // private readonly ICurrentTenantService _currentTenantService;

        // TODO: Inyectar servicio para obtener el usuario actual (para auditoría)
        // private readonly ICurrentUserService _currentUserService;

        #region Constructor

        /// <summary>
        /// Constructor del DbContext
        /// </summary>
        /// <param name="options">Opciones de configuración del DbContext</param>
        public SchoolSystemDbContext(DbContextOptions<SchoolSystemDbContext> options)
            : base(options)
        {
        }

        #endregion

        #region DbSets - Escuelas

        /// <summary>
        /// Escuelas registradas en el sistema
        /// </summary>
        public DbSet<Escuela> Escuelas { get; set; }

        #endregion

        #region DbSets - Usuarios

        /// <summary>
        /// Usuarios del sistema (todos los roles)
        /// </summary>
        public DbSet<Usuario> Usuarios { get; set; }

        /// <summary>
        /// Dispositivos registrados por usuarios
        /// </summary>
        public DbSet<Dispositivo> Dispositivos { get; set; }

        #endregion

        #region DbSets - Académico

        /// <summary>
        /// Alumnos inscritos
        /// </summary>
        public DbSet<Alumno> Alumnos { get; set; }

        /// <summary>
        /// Padres de familia o tutores
        /// </summary>
        public DbSet<Padre> Padres { get; set; }

        /// <summary>
        /// Maestros o profesores
        /// </summary>
        public DbSet<Maestro> Maestros { get; set; }

        /// <summary>
        /// Relación entre alumnos y padres
        /// </summary>
        public DbSet<AlumnoPadre> AlumnoPadres { get; set; }

        /// <summary>
        /// Niveles educativos (Kinder, Primaria, Secundaria, etc.)
        /// </summary>
        public DbSet<NivelEducativo> NivelesEducativos { get; set; }

        /// <summary>
        /// Grados escolares
        /// </summary>
        public DbSet<Grado> Grados { get; set; }

        /// <summary>
        /// Grupos o secciones
        /// </summary>
        public DbSet<Grupo> Grupos { get; set; }

        /// <summary>
        /// Materias o asignaturas
        /// </summary>
        public DbSet<Materia> Materias { get; set; }

        /// <summary>
        /// Relación entre grados y materias
        /// </summary>
        public DbSet<GradoMateria> GradoMaterias { get; set; }

        /// <summary>
        /// Asignación de maestros a materias por grupo
        /// </summary>
        public DbSet<GrupoMateriaMaestro> GrupoMateriaMaestros { get; set; }

        /// <summary>
        /// Inscripciones de alumnos a grupos
        /// </summary>
        public DbSet<Inscripcion> Inscripciones { get; set; }

        #endregion

        #region DbSets - Evaluación

        /// <summary>
        /// Períodos de evaluación (bimestres, trimestres, etc.)
        /// </summary>
        public DbSet<PeriodoEvaluacion> PeriodosEvaluacion { get; set; }

        /// <summary>
        /// Calificaciones de alumnos
        /// </summary>
        public DbSet<Calificacion> Calificaciones { get; set; }

        /// <summary>
        /// Asistencias de alumnos
        /// </summary>
        public DbSet<Asistencia> Asistencias { get; set; }

        #endregion

        #region DbSets - Conducta

        /// <summary>
        /// Registros de conducta de alumnos
        /// </summary>
        public DbSet<RegistroConducta> RegistrosConducta { get; set; }

        /// <summary>
        /// Sistema de puntos por alumno
        /// </summary>
        public DbSet<AlumnoPuntos> AlumnoPuntos { get; set; }

        /// <summary>
        /// Historial de cambios de puntos
        /// </summary>
        public DbSet<HistorialPuntos> HistorialPuntos { get; set; }

        /// <summary>
        /// Insignias o logros disponibles
        /// </summary>
        public DbSet<Insignia> Insignias { get; set; }

        /// <summary>
        /// Relación entre alumnos e insignias ganadas
        /// </summary>
        public DbSet<AlumnoInsignia> AlumnosInsignias { get; set; }

        /// <summary>
        /// Sanciones disciplinarias
        /// </summary>
        public DbSet<Sancion> Sanciones { get; set; }

        #endregion

        #region DbSets - Finanzas

        /// <summary>
        /// Conceptos de pago (colegiaturas, materiales, etc.)
        /// </summary>
        public DbSet<ConceptoPago> ConceptosPago { get; set; }

        /// <summary>
        /// Cargos generados a alumnos
        /// </summary>
        public DbSet<Cargo> Cargos { get; set; }

        /// <summary>
        /// Pagos realizados
        /// </summary>
        public DbSet<Pago> Pagos { get; set; }

        /// <summary>
        /// Estados de cuenta de alumnos
        /// </summary>
        public DbSet<EstadoCuenta> EstadosCuenta { get; set; }

        #endregion

        #region DbSets - Biblioteca

        /// <summary>
        /// Libros y recursos de la biblioteca
        /// </summary>
        public DbSet<Libro> Libros { get; set; }

        /// <summary>
        /// Categorías de recursos de biblioteca
        /// </summary>
        public DbSet<CategoriaRecurso> CategoriasRecurso { get; set; }

        /// <summary>
        /// Préstamos de libros
        /// </summary>
        public DbSet<Prestamo> Prestamos { get; set; }

        #endregion

        #region DbSets - Médico

        /// <summary>
        /// Expedientes médicos de alumnos
        /// </summary>
        public DbSet<ExpedienteMedico> ExpedientesMedicos { get; set; }

        /// <summary>
        /// Registro de vacunas
        /// </summary>
        public DbSet<Vacuna> Vacunas { get; set; }

        /// <summary>
        /// Alergias de alumnos
        /// </summary>
        public DbSet<Alergia> Alergias { get; set; }

        /// <summary>
        /// Medicamentos administrados o prescritos
        /// </summary>
        public DbSet<Medicamento> Medicamentos { get; set; }

        /// <summary>
        /// Historial médico de alumnos
        /// </summary>
        public DbSet<HistorialMedico> HistorialMedico { get; set; }

        #endregion

        #region DbSets - Comunicación

        /// <summary>
        /// Mensajes entre usuarios
        /// </summary>
        public DbSet<Mensaje> Mensajes { get; set; }

        /// <summary>
        /// Notificaciones del sistema
        /// </summary>
        public DbSet<Notificacion> Notificaciones { get; set; }

        /// <summary>
        /// Comunicados escolares
        /// </summary>
        public DbSet<Comunicado> Comunicados { get; set; }

        /// <summary>
        /// Registro de lectura de comunicados
        /// </summary>
        public DbSet<ComunicadoLectura> ComunicadosLectura { get; set; }

        /// <summary>
        /// Log de notificaciones SMS enviadas
        /// </summary>
        public DbSet<NotificacionSmsLog> NotificacionesSmsLog { get; set; }

        #endregion

        #region DbSets - Documentos

        /// <summary>
        /// Plantillas de documentos
        /// </summary>
        public DbSet<PlantillaDocumento> PlantillasDocumento { get; set; }

        /// <summary>
        /// Documentos generados
        /// </summary>
        public DbSet<Documento> Documentos { get; set; }

        /// <summary>
        /// Reportes personalizados
        /// </summary>
        public DbSet<ReportePersonalizado> ReportesPersonalizados { get; set; }

        #endregion

        #region DbSets - Calendario

        /// <summary>
        /// Eventos del calendario escolar
        /// </summary>
        public DbSet<Evento> Eventos { get; set; }

        #endregion

        #region DbSets - Auditoría

        /// <summary>
        /// Logs de auditoría del sistema
        /// </summary>
        public DbSet<LogAuditoria> LogsAuditoria { get; set; }

        /// <summary>
        /// Cambios en entidades rastreados
        /// </summary>
        public DbSet<CambioEntidad> CambiosEntidad { get; set; }

        /// <summary>
        /// Registros de sincronización
        /// </summary>
        public DbSet<Sincronizacion> Sincronizaciones { get; set; }

        #endregion

        #region DbSets - Configuración

        /// <summary>
        /// Configuraciones específicas de cada escuela
        /// </summary>
        public DbSet<ConfiguracionEscuela> ConfiguracionesEscuela { get; set; }

        /// <summary>
        /// Parámetros del sistema
        /// </summary>
        public DbSet<ParametroSistema> ParametrosSistema { get; set; }

        /// <summary>
        /// Preferencias de usuarios
        /// </summary>
        public DbSet<PreferenciaUsuario> PreferenciasUsuario { get; set; }

        #endregion

        #region DbSets - Tareas

        /// <summary>
        /// Tareas escolares asignadas
        /// </summary>
        public DbSet<Tarea> Tareas { get; set; }

        /// <summary>
        /// Entregas de tareas por alumnos
        /// </summary>
        public DbSet<TareaEntrega> TareasEntregas { get; set; }

        #endregion

        #region Configuración del Modelo

        /// <summary>
        /// Configura el modelo de EF y aplica convenciones, filtros y configuraciones
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicar todas las configuraciones de Fluent API automáticamente
            // Busca todas las clases que implementan IEntityTypeConfiguration<T>
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Configurar filtros globales para Multi-tenancy
            ConfigurarFiltrosGlobales(modelBuilder);

            // COMENTADO: Ya configuramos DeleteBehavior específicamente en cada configuración
            // ConfigurarEliminacionEnCascada(modelBuilder);

            // COMENTADO: Si usas nombres normales en tus configuraciones, no necesitas esto
            // ConfigurarConvenciones(modelBuilder);
        }

        /// <summary>
        /// Configura filtros globales para Multi-tenancy
        /// Automáticamente filtra por EscuelaId en todas las consultas
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo</param>
        private void ConfigurarFiltrosGlobales(ModelBuilder modelBuilder)
        {
            // TODO: Descomentar cuando se implemente ICurrentTenantService
            // var tenantId = _currentTenantService?.GetCurrentTenantId();

            // Query Filter para todas las entidades que tengan EscuelaId
            // Esto asegura que cada escuela solo vea sus propios datos

            /* Ejemplo de implementación cuando tengas el servicio:
            
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Verificar si la entidad tiene la propiedad EscuelaId
                if (entityType.ClrType.GetProperty("EscuelaId") != null)
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, "EscuelaId");
                    var tenantIdConstant = Expression.Constant(tenantId);
                    var comparison = Expression.Equal(property, tenantIdConstant);
                    var lambda = Expression.Lambda(comparison, parameter);
                    
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
            */
        }

        /// <summary>
        /// COMENTADO: Ya configuramos DeleteBehavior específicamente en cada configuración
        /// Configura el comportamiento de eliminación en cascada
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo</param>
        /*
        private void ConfigurarEliminacionEnCascada(ModelBuilder modelBuilder)
        {
            // Por defecto, establecer Restrict para evitar eliminaciones accidentales en cascada
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
        */

        /// <summary>
        /// COMENTADO: Si usas nombres normales en tus configuraciones, esto puede causar conflictos
        /// Configura convenciones de nombres para las tablas
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo</param>
        /*
        private void ConfigurarConvenciones(ModelBuilder modelBuilder)
        {
            // Convertir nombres de tablas a snake_case (minúsculas con guión bajo)
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Nombre de la tabla en minúsculas
                entity.SetTableName(entity.GetTableName().ToLower());

                // Nombres de columnas en minúsculas
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToLower());
                }
            }
        }
        */

        #endregion

        #region Sobrescritura de SaveChanges para Auditoría Automática

        /// <summary>
        /// Sobrescribe SaveChanges para aplicar auditoría automática
        /// </summary>
        /// <returns>Cantidad de registros afectados</returns>
        public override int SaveChanges()
        {
            AplicarAuditoria();
            return base.SaveChanges();
        }

        /// <summary>
        /// Sobrescribe SaveChangesAsync para aplicar auditoría automática
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Cantidad de registros afectados de forma asíncrona</returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AplicarAuditoria();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Aplica auditoría automática a las entidades que implementan IAuditableEntity
        /// </summary>
        private void AplicarAuditoria()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is IAuditableEntity &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            // TODO: Obtener el ID del usuario actual desde ICurrentUserService
            // var currentUserId = _currentUserService?.GetCurrentUserId();
            var currentUserId = 1; // Temporal para desarrollo

            foreach (var entry in entries)
            {
                var entity = (IAuditableEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.Now;
                    entity.CreatedBy = currentUserId;
                }

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedBy = currentUserId;
            }

            // Aplicar Soft Delete
            var deletedEntries = ChangeTracker.Entries()
                .Where(e => e.Entity is ISoftDeletable && e.State == EntityState.Deleted);

            foreach (var entry in deletedEntries)
            {
                entry.State = EntityState.Modified;
                var entity = (ISoftDeletable)entry.Entity;

                entity.IsDeleted = true;
                entity.DeletedAt = DateTime.Now;
                entity.DeletedBy = currentUserId;
            }
        }

        #endregion

        #region Métodos Auxiliares

        /// <summary>
        /// Deshabilita temporalmente los filtros globales
        /// Útil para operaciones de administración que necesitan ver todos los datos
        /// </summary>
        /// <returns>Un IDisposable que restaura el comportamiento original al disponer</returns>
        public IDisposable DisableTenantFilter()
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return new TenantFilterDisabler(this);
        }

        /// <summary>
        /// Clase auxiliar para deshabilitar filtros de tenant temporalmente
        /// </summary>
        private class TenantFilterDisabler : IDisposable
        {
            private readonly SchoolSystemDbContext _context;

            /// <summary>
            /// Constructor del deshabilitador de filtros
            /// </summary>
            /// <param name="context">Instancia del DbContext</param>
            public TenantFilterDisabler(SchoolSystemDbContext context)
            {
                _context = context;
            }

            /// <summary>
            /// Restaura el comportamiento de seguimiento de cambios al disponer
            /// </summary>
            public void Dispose()
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            }
        }

        #endregion
    }
}