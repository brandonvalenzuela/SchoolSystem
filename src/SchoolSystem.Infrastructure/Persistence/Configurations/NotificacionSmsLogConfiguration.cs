using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Comunicacion;
using SchoolSystem.Domain.Enums.Comunicacion;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad NotificacionSmsLog
    /// </summary>
    public class NotificacionSmsLogConfiguration : IEntityTypeConfiguration<NotificacionSmsLog>
    {
        public void Configure(EntityTypeBuilder<NotificacionSmsLog> builder)
        {
            // Nombre de tabla
            builder.ToTable("NotificacionSmsLog");

            // Clave primaria
            builder.HasKey(nsl => nsl.Id);

            // Propiedades requeridas
            builder.Property(nsl => nsl.NotificacionId)
                .IsRequired();

            builder.Property(nsl => nsl.Telefono)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(nsl => nsl.Mensaje)
                .IsRequired()
                .HasColumnType("LONGTEXT");

            // Proveedor
            builder.Property(nsl => nsl.Proveedor)
                .HasMaxLength(50);

            builder.Property(nsl => nsl.Estatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(10)
                .HasDefaultValue(EstatusSms.Pendiente);

            builder.Property(nsl => nsl.SidProveedor)
                .HasMaxLength(100);

            // Costos y fechas
            builder.Property(nsl => nsl.Costo)
                .IsRequired(false)
                .HasColumnType("decimal(10,4)");

            builder.Property(nsl => nsl.Moneda)
                .HasMaxLength(10)
                .HasDefaultValue("MXN");

            builder.Property(nsl => nsl.FechaEnvio)
                .IsRequired(false);

            builder.Property(nsl => nsl.FechaEntrega)
                .IsRequired(false);

            // Error y reintentos
            builder.Property(nsl => nsl.ErrorMensaje)
                .HasColumnType("LONGTEXT");

            builder.Property(nsl => nsl.CodigoError)
                .HasMaxLength(50);

            builder.Property(nsl => nsl.NumeroIntentos)
                .HasDefaultValue(0);

            builder.Property(nsl => nsl.FechaUltimoIntento)
                .IsRequired(false);

            // Metadata
            builder.Property(nsl => nsl.Metadata)
                .HasColumnType( "LONGTEXT");

            // Relaciones
            builder.HasOne(nsl => nsl.Notificacion)
                .WithMany()
                .HasForeignKey(nsl => nsl.NotificacionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices
            builder.HasIndex(nsl => nsl.NotificacionId)
                .HasDatabaseName("IX_NotificacionSmsLog_NotificacionId");

            builder.HasIndex(nsl => nsl.Estatus)
                .HasDatabaseName("IX_NotificacionSmsLog_Estatus");

            builder.HasIndex(nsl => nsl.Telefono)
                .HasDatabaseName("IX_NotificacionSmsLog_Telefono");

            builder.HasIndex(nsl => nsl.FechaEnvio)
                .HasDatabaseName("IX_NotificacionSmsLog_FechaEnvio");

            builder.HasIndex(nsl => new { nsl.Estatus, nsl.FechaUltimoIntento })
                .HasDatabaseName("IX_NotificacionSmsLog_Estatus_FechaIntento");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(nsl => nsl.Enviado);
            builder.Ignore(nsl => nsl.Fallido);
            builder.Ignore(nsl => nsl.Pendiente);
            builder.Ignore(nsl => nsl.TiempoDesdeEnvio);
            builder.Ignore(nsl => nsl.TiempoEntrega);

            // Constraints
            builder.HasCheckConstraint("CK_NotificacionSmsLog_NumeroIntentos",
                "`NumeroIntentos` >= 0");
        }
    }
}