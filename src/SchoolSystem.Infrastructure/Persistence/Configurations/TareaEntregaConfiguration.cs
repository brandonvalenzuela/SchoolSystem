using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Tareas;
using SchoolSystem.Domain.Enums.Academico;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad TareaEntrega
    /// </summary>
    public class TareaEntregaConfiguration : IEntityTypeConfiguration<TareaEntrega>
    {
        public void Configure(EntityTypeBuilder<TareaEntrega> builder)
        {
            // Nombre de tabla
            builder.ToTable("TareaEntregas");

            // Clave primaria
            builder.HasKey(te => te.Id);

            // Propiedades requeridas
            builder.Property(te => te.EscuelaId)
                .IsRequired();

            builder.Property(te => te.TareaId)
                .IsRequired();

            builder.Property(te => te.AlumnoId)
                .IsRequired();

            // Información de entrega
            builder.Property(te => te.FechaEntrega)
                .IsRequired(false);

            builder.Property(te => te.ComentariosAlumno)
                .HasColumnType("LONGTEXT");

            builder.Property(te => te.ArchivoUrl)
                .HasMaxLength(500);

            builder.Property(te => te.ArchivoNombre)
                .HasMaxLength(200);

            builder.Property(te => te.ArchivoTamano)
                .IsRequired(false);

            // Estado (Enum)
            builder.Property(te => te.Estatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(EstatusEntrega.Pendiente);

            builder.Property(te => te.EsTardia)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(te => te.NumeroIntento)
                .IsRequired()
                .HasDefaultValue(1);

            // Evaluación
            builder.Property(te => te.Calificacion)
                .IsRequired(false)
                .HasColumnType("decimal(5,2)");

            builder.Property(te => te.CalificacionOriginal)
                .IsRequired(false)
                .HasColumnType("decimal(5,2)");

            builder.Property(te => te.PenalizacionAplicada)
                .IsRequired(false)
                .HasColumnType("decimal(5,2)");

            builder.Property(te => te.Retroalimentacion)
                .HasColumnType("LONGTEXT");

            builder.Property(te => te.RevisadoPorId)
                .IsRequired(false);

            builder.Property(te => te.FechaRevision)
                .IsRequired(false);

            // Archivos de retroalimentación
            builder.Property(te => te.ArchivoRetroalimentacionUrl)
                .HasMaxLength(500);

            builder.Property(te => te.ArchivoRetroalimentacionNombre)
                .HasMaxLength(200);

            // Auditoría
            builder.Property(te => te.CreatedAt)
                .IsRequired();

            builder.Property(te => te.CreatedBy)
                .IsRequired(false);

            builder.Property(te => te.UpdatedAt)
                .IsRequired();

            builder.Property(te => te.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(te => te.Tarea)
                .WithMany(t => t.Entregas)
                .HasForeignKey(te => te.TareaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(te => te.Alumno)
                .WithMany()
                .HasForeignKey(te => te.AlumnoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(te => te.RevisadoPor)
                .WithMany()
                .HasForeignKey(te => te.RevisadoPorId)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices
            builder.HasIndex(te => te.EscuelaId)
                .HasDatabaseName("IX_TareaEntregas_EscuelaId");

            builder.HasIndex(te => te.TareaId)
                .HasDatabaseName("IX_TareaEntregas_TareaId");

            builder.HasIndex(te => te.AlumnoId)
                .HasDatabaseName("IX_TareaEntregas_AlumnoId");

            builder.HasIndex(te => te.Estatus)
                .HasDatabaseName("IX_TareaEntregas_Estatus");

            builder.HasIndex(te => te.FechaEntrega)
                .HasDatabaseName("IX_TareaEntregas_FechaEntrega");

            builder.HasIndex(te => new { te.AlumnoId, te.Estatus })
                .HasDatabaseName("IX_TareaEntregas_Alumno_Estatus");

            // Índice único: Un alumno solo puede tener una entrega por tarea
            builder.HasIndex(te => new { te.TareaId, te.AlumnoId })
                .IsUnique()
                .HasDatabaseName("IX_TareaEntregas_Tarea_Alumno_Unique");

            // Índice para búsquedas de entregas pendientes de revisión
            builder.HasIndex(te => new { te.TareaId, te.Estatus })
                .HasDatabaseName("IX_TareaEntregas_Tarea_Estatus");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(te => te.EstaCalificada);
            builder.Ignore(te => te.Aprobada);
            builder.Ignore(te => te.DiasRetraso);
            builder.Ignore(te => te.NoEntregada);
            builder.Ignore(te => te.Pendiente);
            builder.Ignore(te => te.PendienteRevision);

            // Constraints
            builder.HasCheckConstraint("CK_TareaEntregas_Calificacion",
                "`Calificacion` IS NULL OR (`Calificacion` >= 0 AND `Calificacion` <= 100)");

            builder.HasCheckConstraint("CK_TareaEntregas_CalificacionOriginal",
                "`CalificacionOriginal` IS NULL OR (`CalificacionOriginal` >= 0 AND `CalificacionOriginal` <= 100)");

            builder.HasCheckConstraint("CK_TareaEntregas_Penalizacion",
                "`PenalizacionAplicada` IS NULL OR `PenalizacionAplicada` >= 0");

            builder.HasCheckConstraint("CK_TareaEntregas_NumeroIntento",
                "`NumeroIntento` > 0");
        }
    }
}