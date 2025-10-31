using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Conducta;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Sancion
    /// </summary>
    public class SancionConfiguration : IEntityTypeConfiguration<Sancion>
    {
        public void Configure(EntityTypeBuilder<Sancion> builder)
        {
            // Nombre de tabla
            builder.ToTable("Sanciones");

            // Clave primaria
            builder.HasKey(s => s.Id);

            // Propiedades requeridas
            builder.Property(s => s.EscuelaId)
                .IsRequired();

            builder.Property(s => s.AlumnoId)
                .IsRequired();

            builder.Property(s => s.ConductaId)
                .IsRequired(false);

            builder.Property(s => s.Tipo)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(s => s.FechaInicio)
                .IsRequired()
                .HasColumnType("DATE");

            builder.Property(s => s.FechaFin)
                .IsRequired(false)
                .HasColumnType("DATE");

            builder.Property(s => s.Descripcion)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(s => s.Motivo)
                .HasMaxLength(1000);

            // Autorización
            builder.Property(s => s.AutorizadoPor)
                .IsRequired();

            builder.Property(s => s.FechaAutorizacion)
                .IsRequired();

            // Estado de cumplimiento
            builder.Property(s => s.Cumplida)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(s => s.FechaCumplimiento)
                .IsRequired(false);

            builder.Property(s => s.ObservacionesCumplimiento)
                .HasMaxLength(1000);

            builder.Property(s => s.VerificadoPor)
                .IsRequired(false);

            // Apelación
            builder.Property(s => s.Apelada)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(s => s.FechaApelacion)
                .IsRequired(false);

            builder.Property(s => s.MotivoApelacion)
                .HasMaxLength(1000);

            builder.Property(s => s.ResultadoApelacion)
                .HasMaxLength(200);

            builder.Property(s => s.FechaResolucionApelacion)
                .IsRequired(false);

            // Notificación
            builder.Property(s => s.PadresNotificados)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(s => s.FechaNotificacionPadres)
                .IsRequired(false);

            builder.Property(s => s.MedioNotificacion)
                .HasMaxLength(100);

            builder.Property(s => s.FirmaEnterado)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(s => s.FechaFirmaEnterado)
                .IsRequired(false);

            // Documentación
            builder.Property(s => s.DocumentoUrl)
                .HasMaxLength(500);

            builder.Property(s => s.Observaciones)
                .HasColumnType("LONGTEXT");

            // Auditoría
            builder.Property(s => s.CreatedAt)
                .IsRequired();

            builder.Property(s => s.UpdatedAt)
                .IsRequired();

            builder.Property(s => s.CreatedBy)
                .IsRequired(false);

            builder.Property(s => s.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(s => s.Escuela)
                .WithMany()
                .HasForeignKey(s => s.EscuelaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Alumno)
                .WithMany()
                .HasForeignKey(s => s.AlumnoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Conducta)
                .WithOne(rc => rc.Sancion)
                .HasForeignKey<Sancion>(s => s.ConductaId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(s => s.UsuarioAutorizo)
                .WithMany()
                .HasForeignKey(s => s.AutorizadoPor)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(s => s.EscuelaId)
                .HasDatabaseName("IX_Sanciones_EscuelaId");

            builder.HasIndex(s => s.AlumnoId)
                .HasDatabaseName("IX_Sanciones_AlumnoId");

            builder.HasIndex(s => s.ConductaId)
                .HasDatabaseName("IX_Sanciones_ConductaId");

            builder.HasIndex(s => s.Tipo)
                .HasDatabaseName("IX_Sanciones_Tipo");

            builder.HasIndex(s => s.FechaInicio)
                .HasDatabaseName("IX_Sanciones_FechaInicio");

            builder.HasIndex(s => s.FechaFin)
                .HasDatabaseName("IX_Sanciones_FechaFin");

            builder.HasIndex(s => s.Cumplida)
                .HasDatabaseName("IX_Sanciones_Cumplida");

            builder.HasIndex(s => s.Apelada)
                .HasDatabaseName("IX_Sanciones_Apelada");

            builder.HasIndex(s => s.AutorizadoPor)
                .HasDatabaseName("IX_Sanciones_AutorizadoPor");

            builder.HasIndex(s => s.PadresNotificados)
                .HasDatabaseName("IX_Sanciones_PadresNotificados");

            builder.HasIndex(s => s.FirmaEnterado)
                .HasDatabaseName("IX_Sanciones_FirmaEnterado");

            // Índices compuestos para consultas frecuentes
            builder.HasIndex(s => new { s.AlumnoId, s.FechaInicio })
                .HasDatabaseName("IX_Sanciones_Alumno_FechaInicio");

            builder.HasIndex(s => new { s.EscuelaId, s.Cumplida })
                .HasDatabaseName("IX_Sanciones_Escuela_Cumplida");

            builder.HasIndex(s => new { s.AlumnoId, s.Tipo, s.Cumplida })
                .HasDatabaseName("IX_Sanciones_Alumno_Tipo_Cumplida");

            builder.HasIndex(s => new { s.EscuelaId, s.FechaInicio, s.FechaFin })
                .HasDatabaseName("IX_Sanciones_Escuela_Fechas");

            builder.HasIndex(s => new { s.Cumplida, s.FechaFin })
                .HasDatabaseName("IX_Sanciones_Cumplida_FechaFin");

            builder.HasIndex(s => new { s.PadresNotificados, s.FirmaEnterado })
                .HasDatabaseName("IX_Sanciones_Notificacion_Firma");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(s => s.EstaActiva);
            builder.Ignore(s => s.HaVencido);
            builder.Ignore(s => s.DiasRestantes);
            builder.Ignore(s => s.DuracionDias);

            // Constraints
            builder.HasCheckConstraint("CK_Sanciones_Fechas",
                "`FechaFin` IS NULL OR `FechaFin` >= `FechaInicio`");

            builder.HasCheckConstraint("CK_Sanciones_FechaAutorizacion",
                "`FechaAutorizacion` <= GETDATE()");

            builder.HasCheckConstraint("CK_Sanciones_FechaCumplimiento",
                "`FechaCumplimiento` IS NULL OR `FechaCumplimiento` >= `FechaInicio`");
        }
    }
}