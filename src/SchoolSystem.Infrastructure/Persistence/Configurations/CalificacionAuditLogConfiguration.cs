using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Evaluacion;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad CalificacionAuditLog
    /// </summary>
    public class CalificacionAuditLogConfiguration : IEntityTypeConfiguration<CalificacionAuditLog>
    {
        public void Configure(EntityTypeBuilder<CalificacionAuditLog> builder)
        {
            // Nombre de tabla
            builder.ToTable("CalificacionesAuditLog");

            // Clave primaria
            builder.HasKey(a => a.Id);

            // Propiedades requeridas
            builder.Property(a => a.EscuelaId)
                .IsRequired();

            builder.Property(a => a.CalificacionId)
                .IsRequired();

            builder.Property(a => a.AlumnoId)
                .IsRequired();

            builder.Property(a => a.GrupoId)
                .IsRequired();

            builder.Property(a => a.MateriaId)
                .IsRequired();

            builder.Property(a => a.PeriodoId)
                .IsRequired();

            // Calificaciones
            builder.Property(a => a.CalificacionAnterior)
                .IsRequired()
                .HasColumnType("DECIMAL(5,2)");

            builder.Property(a => a.CalificacionNueva)
                .IsRequired()
                .HasColumnType("DECIMAL(5,2)");

            // Observaciones (nullable)
            builder.Property(a => a.ObservacionesAnteriores)
                .IsRequired(false)
                .HasColumnType("TEXT");

            builder.Property(a => a.ObservacionesNuevas)
                .IsRequired(false)
                .HasColumnType("TEXT");

            // Motivo (requerido)
            builder.Property(a => a.Motivo)
                .IsRequired()
                .HasMaxLength(300);

            // Metadata de cambio
            builder.Property(a => a.RecalificadoPor)
                .IsRequired();

            builder.Property(a => a.RecalificadoAtUtc)
                .IsRequired();

            // Origen (opcional)
            builder.Property(a => a.Origen)
                .IsRequired(false)
                .HasMaxLength(30);

            // CorrelationId (opcional, para vincular cambios en lote)
            builder.Property(a => a.CorrelationId)
                .IsRequired(false)
                .HasMaxLength(64);

            // Auditoría
            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(a => a.UpdatedAt)
                .IsRequired();

            builder.Property(a => a.CreatedBy)
                .IsRequired(false);

            builder.Property(a => a.UpdatedBy)
                .IsRequired(false);

            // Soft Delete
            builder.Property(a => a.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(a => a.DeletedAt)
                .IsRequired(false);

            builder.Property(a => a.DeletedBy)
                .IsRequired(false);

            // Query filter para Soft Delete
            builder.HasQueryFilter(a => !a.IsDeleted);

            // Relaciones
            builder.HasOne(a => a.Escuela)
                .WithMany()
                .HasForeignKey(a => a.EscuelaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con Calificación (sin cascada)
            builder.HasOne(a => a.Calificacion)
                .WithMany()
                .HasForeignKey(a => a.CalificacionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices para consultas rápidas
            
            // Búsqueda por escuela y calificación
            builder.HasIndex(a => new { a.EscuelaId, a.CalificacionId })
                .HasDatabaseName("IX_Audit_Escuela_Calificacion");

            // Búsqueda por escuela, alumno y período (reportes de auditoría por alumno)
            builder.HasIndex(a => new { a.EscuelaId, a.AlumnoId, a.PeriodoId })
                .HasDatabaseName("IX_Audit_Escuela_Alumno_Periodo");

            // Búsqueda por escuela, grupo y período (auditoría por grupo)
            builder.HasIndex(a => new { a.EscuelaId, a.GrupoId, a.PeriodoId })
                .HasDatabaseName("IX_Audit_Escuela_Grupo_Periodo");

            // Búsqueda por escuela, materia y período
            builder.HasIndex(a => new { a.EscuelaId, a.MateriaId, a.PeriodoId })
                .HasDatabaseName("IX_Audit_Escuela_Materia_Periodo");

            // Búsqueda por fecha (auditoría temporal)
            builder.HasIndex(a => a.RecalificadoAtUtc)
                .HasDatabaseName("IX_Audit_RecalificadoAtUtc");

            // Búsqueda por usuario que recalificó
            builder.HasIndex(a => new { a.EscuelaId, a.RecalificadoPor })
                .HasDatabaseName("IX_Audit_Escuela_RecalificadoPor");

            // Búsqueda por CorrelationId (operaciones en lote)
            builder.HasIndex(a => a.CorrelationId)
                .HasDatabaseName("IX_Audit_CorrelationId");
        }
    }
}
