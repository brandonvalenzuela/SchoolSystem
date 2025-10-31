using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Evaluacion;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Asistencia
    /// </summary>
    public class AsistenciaConfiguration : IEntityTypeConfiguration<Asistencia>
    {
        public void Configure(EntityTypeBuilder<Asistencia> builder)
        {
            // Nombre de tabla
            builder.ToTable("Asistencias");

            // Clave primaria
            builder.HasKey(a => a.Id);

            // Propiedades requeridas
            builder.Property(a => a.EscuelaId)
                .IsRequired();

            builder.Property(a => a.AlumnoId)
                .IsRequired();

            builder.Property(a => a.GrupoId)
                .IsRequired();

            builder.Property(a => a.Fecha)
                .IsRequired()
                .HasColumnType("DATE");

            builder.Property(a => a.Estatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(a => a.FechaRegistro)
                .IsRequired();

            // Propiedades de horarios
            builder.Property(a => a.HoraEntrada)
                .IsRequired(false);

            builder.Property(a => a.HoraSalida)
                .IsRequired(false);

            builder.Property(a => a.MinutosRetardo)
                .IsRequired(false);

            // Justificación
            builder.Property(a => a.Justificado)
                .HasDefaultValue(false);

            builder.Property(a => a.Motivo)
                .HasMaxLength(500);

            builder.Property(a => a.JustificanteUrl)
                .HasMaxLength(500);

            builder.Property(a => a.FechaJustificacion)
                .IsRequired(false);

            builder.Property(a => a.AproboJustificanteId)
                .IsRequired(false);

            // Observaciones
            builder.Property(a => a.Observaciones)
                .HasMaxLength(1000);

            builder.Property(a => a.PadresNotificados)
                .HasDefaultValue(false);

            builder.Property(a => a.FechaNotificacionPadres)
                .IsRequired(false);

            // Control de registro
            builder.Property(a => a.RegistradoPor)
                .IsRequired(false);

            builder.Property(a => a.FueModificada)
                .HasDefaultValue(false);

            builder.Property(a => a.FechaUltimaModificacion)
                .IsRequired(false);

            builder.Property(a => a.MotivoModificacion)
                .HasMaxLength(500);

            // Auditoría
            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(a => a.UpdatedAt)
                .IsRequired();

            builder.Property(a => a.CreatedBy)
                .IsRequired(false);

            builder.Property(a => a.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(a => a.Escuela)
                .WithMany()
                .HasForeignKey(a => a.EscuelaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Alumno)
                .WithMany()
                .HasForeignKey(a => a.AlumnoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Grupo)
                .WithMany(g => g.Asistencias)
                .HasForeignKey(a => a.GrupoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.UsuarioRegistro)
                .WithMany()
                .HasForeignKey(a => a.RegistradoPor)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices
            builder.HasIndex(a => a.EscuelaId)
                .HasDatabaseName("IX_Asistencias_EscuelaId");

            builder.HasIndex(a => a.AlumnoId)
                .HasDatabaseName("IX_Asistencias_AlumnoId");

            builder.HasIndex(a => a.GrupoId)
                .HasDatabaseName("IX_Asistencias_GrupoId");

            builder.HasIndex(a => a.Fecha)
                .HasDatabaseName("IX_Asistencias_Fecha");

            builder.HasIndex(a => a.Estatus)
                .HasDatabaseName("IX_Asistencias_Estatus");

            builder.HasIndex(a => a.Justificado)
                .HasDatabaseName("IX_Asistencias_Justificado");

            builder.HasIndex(a => a.FechaRegistro)
                .HasDatabaseName("IX_Asistencias_FechaRegistro");

            // Índice único compuesto: Un alumno no puede tener dos registros de asistencia en la misma fecha
            builder.HasIndex(a => new { a.AlumnoId, a.Fecha })
                .IsUnique()
                .HasDatabaseName("IX_Asistencias_Alumno_Fecha_Unique");

            builder.HasIndex(a => new { a.EscuelaId, a.Fecha, a.Estatus })
                .HasDatabaseName("IX_Asistencias_Escuela_Fecha_Estatus");

            builder.HasIndex(a => new { a.GrupoId, a.Fecha })
                .HasDatabaseName("IX_Asistencias_Grupo_Fecha");

            builder.HasIndex(a => new { a.AlumnoId, a.Estatus })
                .HasDatabaseName("IX_Asistencias_Alumno_Estatus");

            builder.HasIndex(a => new { a.AlumnoId, a.Fecha, a.Estatus })
                .HasDatabaseName("IX_Asistencias_Alumno_Fecha_Estatus");

            builder.HasIndex(a => a.RegistradoPor)
                .HasDatabaseName("IX_Asistencias_RegistradoPor");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(a => a.EstaPresente);
            builder.Ignore(a => a.Falto);
            builder.Ignore(a => a.LlegoTarde);
            builder.Ignore(a => a.EsFaltaJustificada);
            builder.Ignore(a => a.TienePermiso);
            builder.Ignore(a => a.AfectaNegat);
            builder.Ignore(a => a.RequiereSeguim);

            // Constraints
            builder.HasCheckConstraint("CK_Asistencias_MinutosRetardo",
                "`MinutosRetardo` IS NULL OR `MinutosRetardo` >= 0");

            builder.HasCheckConstraint("CK_Asistencias_Horarios",
                "`HoraEntrada` IS NULL OR `HoraSalida` IS NULL OR `HoraEntrada` < `HoraSalida`");

            builder.HasCheckConstraint("CK_Asistencias_Fecha",
                "`Fecha` <= CAST(GETDATE() AS DATE)");
        }
    }
}