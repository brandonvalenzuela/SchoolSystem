using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Comunicacion;
using SchoolSystem.Domain.Enums.Comunicacion;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Notificacion
    /// </summary>
    public class NotificacionConfiguration : IEntityTypeConfiguration<Notificacion>
    {
        public void Configure(EntityTypeBuilder<Notificacion> builder)
        {
            // Nombre de tabla
            builder.ToTable("Notificaciones");

            // Clave primaria
            builder.HasKey(n => n.Id);

            // Propiedades requeridas
            builder.Property(n => n.EscuelaId)
                .IsRequired();

            builder.Property(n => n.UsuarioDestinatarioId)
                .IsRequired(false);

            builder.Property(n => n.Tipo)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(n => n.Prioridad)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(10)
                .HasDefaultValue(PrioridadNotificacion.Normal);

            builder.Property(n => n.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(n => n.Mensaje)
                .IsRequired()
                .HasColumnType("LONGTEXT");

            builder.Property(n => n.UrlAccion)
                .HasMaxLength(500);

            builder.Property(n => n.EnviadoPorId)
                .IsRequired();

            // Fechas
            builder.Property(n => n.FechaEnvio)
                .IsRequired();

            builder.Property(n => n.FechaProgramada)
                .IsRequired(false);

            builder.Property(n => n.FechaLectura)
                .IsRequired(false);

            // Estado
            builder.Property(n => n.Leida)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(n => n.Canal)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(10)
                .HasDefaultValue(CanalNotificacion.Sistema);

            // Metadata
            builder.Property(n => n.Metadata)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(n => n.Icono)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(n => n.Color)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(n => n.ReproducirSonido)
                .HasDefaultValue(false);

            builder.Property(n => n.FechaExpiracion)
                .IsRequired(false);

            // Auditoría
            builder.Property(n => n.CreatedAt)
                .IsRequired();

            builder.Property(n => n.CreatedBy)
                .IsRequired(false);

            builder.Property(n => n.UpdatedAt)
                .IsRequired();

            builder.Property(n => n.UpdatedBy)
                .IsRequired(false);

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

            // Relaciones
            builder.HasOne(n => n.UsuarioDestinatario)
                .WithMany()
                .HasForeignKey(n => n.UsuarioDestinatarioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(n => n.EnviadoPor)
                .WithMany()
                .HasForeignKey(n => n.EnviadoPorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(n => n.EscuelaId)
                .HasDatabaseName("IX_Notificaciones_EscuelaId");

            builder.HasIndex(n => new { n.UsuarioDestinatarioId, n.Leida })
                .HasDatabaseName("IX_Notificaciones_Usuario_Leida");

            builder.HasIndex(n => n.FechaEnvio)
                .HasDatabaseName("IX_Notificaciones_FechaEnvio");

            builder.HasIndex(n => n.Prioridad)
                .HasDatabaseName("IX_Notificaciones_Prioridad");

            builder.HasIndex(n => n.Tipo)
                .HasDatabaseName("IX_Notificaciones_Tipo");

            builder.HasIndex(n => new { n.UsuarioDestinatarioId, n.FechaEnvio })
                .HasDatabaseName("IX_Notificaciones_Usuario_Fecha");

            builder.HasIndex(n => n.FechaProgramada)
                .HasDatabaseName("IX_Notificaciones_FechaProgramada");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(n => n.PendienteLectura);
            builder.Ignore(n => n.EsUrgente);
            builder.Ignore(n => n.EstaExpirada);
            builder.Ignore(n => n.EstaProgramada);
            builder.Ignore(n => n.TiempoDesdeEnvio);
            builder.Ignore(n => n.MinutosDesdeEnvio);
            builder.Ignore(n => n.HorasDesdeEnvio);
            builder.Ignore(n => n.DiasDesdeEnvio);
            builder.Ignore(n => n.TiempoTranscurridoTexto);
        }
    }
}