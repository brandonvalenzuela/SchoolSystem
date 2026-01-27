using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Evaluacion;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Calificacion
    /// </summary>
    public class CalificacionConfiguration : IEntityTypeConfiguration<Calificacion>
    {
        /// <summary>
        /// Configura la entidad Calificacion para EF Core
        /// </summary>
        /// <param name="builder">Constructor de la entidad</param>
        public void Configure(EntityTypeBuilder<Calificacion> builder)
        {
            #region Tabla

            // Configurar tabla con check constraints usando EF Core 8 overload
            builder.ToTable("Calificaciones", t =>
            {
                // Check Constraint: La calificación numérica debe estar entre 0 y 10
                t.HasCheckConstraint(
                    "CK_Calificaciones_CalificacionNumerica",
                    "`CalificacionNumerica` >= 0 AND `CalificacionNumerica` <= 10");

                // Check Constraint: El peso debe estar entre 0 y 100 si existe
                t.HasCheckConstraint(
                    "CK_Calificaciones_Peso",
                    "`Peso` IS NULL OR (`Peso` >= 0 AND `Peso` <= 100)");
            });

            #endregion

            #region Clave Primaria

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd();

            #endregion

            #region Foreign Keys

            // Escuela ID (Multi-tenant)
            builder.Property(c => c.EscuelaId)
                .IsRequired();

            // Alumno ID
            builder.Property(c => c.AlumnoId)
                .IsRequired();

            // Materia ID
            builder.Property(c => c.MateriaId)
                .IsRequired();

            // Grupo ID
            builder.Property(c => c.GrupoId)
                .IsRequired();

            // Periodo ID
            builder.Property(c => c.PeriodoId)
                .IsRequired();

            #endregion

            #region Calificación

            // Calificación Numérica
            builder.Property(c => c.CalificacionNumerica)
                .HasColumnType("DECIMAL(5,2)")
                .IsRequired();

            // Calificación Letra
            builder.Property(c => c.CalificacionLetra)
                .HasMaxLength(5);

            // Aprobado
            builder.Property(c => c.Aprobado)
                .IsRequired();

            // Calificación Mínima
            builder.Property(c => c.CalificacionMinima)
                .HasColumnType("DECIMAL(5,2)");

            #endregion

            #region Detalles de la Evaluación

            // Tipo Evaluación
            builder.Property(c => c.TipoEvaluacion)
                .HasMaxLength(50)
                .IsRequired(false);

            // Peso
            builder.Property(c => c.Peso)
                .HasColumnType("DECIMAL(5,2)");

            // Observaciones
            builder.Property(c => c.Observaciones)
                .HasColumnType("TEXT")
                .IsRequired(false);

            // Fortalezas
            builder.Property(c => c.Fortalezas)
                .HasColumnType("TEXT")
                .IsRequired(false);

            // Áreas Oportunidad
            builder.Property(c => c.AreasOportunidad)
                .HasColumnType("TEXT")
                .IsRequired(false);

            // Recomendaciones
            builder.Property(c => c.Recomendaciones)
                .HasColumnType("TEXT")
                .IsRequired(false);

            #endregion

            #region Control de Captura

            // Fecha Captura
            builder.Property(c => c.FechaCaptura)
                .IsRequired();

            // Capturado Por
            builder.Property(c => c.CapturadoPor);

            // Fue Modificada
            builder.Property(c => c.FueModificada)
                .HasDefaultValue(false)
                .IsRequired();

            // Fecha Última Modificación
            builder.Property(c => c.FechaUltimaModificacion)
                .HasColumnType("DATETIME");

            // Modificado Por
            builder.Property(c => c.ModificadoPor);

            // Motivo Modificación
            builder.Property(c => c.MotivoModificacion)
                .HasColumnType("TEXT")
                .IsRequired(false);

            #endregion

            #region Recalificación

            // Es Recalificación
            builder.Property(c => c.EsRecalificacion)
                .HasDefaultValue(false)
                .IsRequired();

            // Calificación Original
            builder.Property(c => c.CalificacionOriginal)
                .HasColumnType("DECIMAL(5,2)");

            // Fecha Recalificación
            builder.Property(c => c.FechaRecalificacion)
                .HasColumnType("DATETIME");

            // Tipo Recalificación
            builder.Property(c => c.TipoRecalificacion)
                .HasMaxLength(50)
                .IsRequired(false);

            #endregion

            #region Estado

            // Bloqueada
            builder.Property(c => c.Bloqueada)
                .HasDefaultValue(false)
                .IsRequired();

            // Fecha Bloqueo
            builder.Property(c => c.FechaBloqueo)
                .HasColumnType("DATETIME");

            // Visible Para Padres
            builder.Property(c => c.VisibleParaPadres)
                .HasDefaultValue(true)
                .IsRequired();

            #endregion

            #region Auditoría

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.Property(c => c.UpdatedAt)
                .IsRequired();

            builder.Property(c => c.CreatedBy);

            builder.Property(c => c.UpdatedBy);

            #endregion

            #region Soft Delete

            // Is Deleted
            builder.Property(a => a.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            // Deleted At
            builder.Property(a => a.DeletedAt)
                .HasColumnType("DATETIME");

            // Deleted By
            builder.Property(a => a.DeletedBy);

            // Query Filter para Soft Delete (solo mostrar no eliminados por defecto)
            builder.HasQueryFilter(a => !a.IsDeleted);

            #endregion

            #region Propiedades Calculadas (Ignorar)

            builder.Ignore(c => c.EsReprobatoria);
            builder.Ignore(c => c.EsExcelente);
            builder.Ignore(c => c.PuedeSerModificada);

            #endregion

            #region Índices

            // Índice único compuesto: Evita duplicados por concurrencia
            // (EscuelaId, GrupoId, MateriaId, PeriodoId, AlumnoId) deben ser únicos
            // Protege contra race conditions en captura masiva
            builder.HasIndex(c => new { c.EscuelaId, c.GrupoId, c.MateriaId, c.PeriodoId, c.AlumnoId })
                .HasDatabaseName("UX_Calificaciones_Escuela_Grupo_Materia_Periodo_Alumno")
                .IsUnique();

            // Índice compuesto (no único): Un alumno puede tener múltiples calificaciones
            // Se mantiene como índice normal para performance en búsquedas
            builder.HasIndex(c => new { c.AlumnoId, c.MateriaId, c.PeriodoId })
                .HasDatabaseName("IX_Calificacion_Alumno_Materia_Periodo");

            // Índice compuesto en Grupo y Período (para consultas de maestros)
            builder.HasIndex(c => new { c.GrupoId, c.PeriodoId })
                .HasDatabaseName("IX_Calificacion_Grupo_Periodo");

            // Índice en AlumnoId
            builder.HasIndex(c => c.AlumnoId)
                .HasDatabaseName("IX_Calificacion_Alumno");

            // Índice en Bloqueada (para filtrar calificaciones editables)
            builder.HasIndex(c => c.Bloqueada)
                .HasDatabaseName("IX_Calificacion_Bloqueada");

            // Índice en VisibleParaPadres
            builder.HasIndex(c => c.VisibleParaPadres)
                .HasDatabaseName("IX_Calificacion_Visible_Padres");

            // Índices para optimizar búsquedas en flujo de captura masiva
            // Performance: búsqueda por grupo, materia, período y alumno
            builder.HasIndex(c => new { c.GrupoId, c.MateriaId, c.PeriodoId, c.AlumnoId })
                .HasDatabaseName("IX_Calificaciones_Grupo_Materia_Periodo_Alumno");

            // Performance: búsqueda por grupo y alumno (recalcular promedios)
            builder.HasIndex(c => new { c.GrupoId, c.AlumnoId })
                .HasDatabaseName("IX_Calificaciones_Grupo_Alumno");

            #endregion

            #region Relaciones

            // Relación con Escuela
            builder.HasOne(c => c.Escuela)
                .WithMany()
                .HasForeignKey(c => c.EscuelaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación con Alumno
            builder.HasOne(c => c.Alumno)
                .WithMany(a => a.Calificaciones)
                .HasForeignKey(c => c.AlumnoId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            // Relación con Materia
            builder.HasOne(c => c.Materia)
                .WithMany(m => m.Calificaciones)
                .HasForeignKey(c => c.MateriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con Grupo
            builder.HasOne(c => c.Grupo)
                .WithMany(g => g.Calificaciones)
                .HasForeignKey(c => c.GrupoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con Periodo
            builder.HasOne(c => c.Periodo)
                .WithMany(p => p.Calificaciones)
                .HasForeignKey(c => c.PeriodoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con Maestro que capturó (opcional)
            builder.HasOne(c => c.MaestroCaptura)
                .WithMany()
                .HasForeignKey(c => c.CapturadoPor)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion
        }
    }
}