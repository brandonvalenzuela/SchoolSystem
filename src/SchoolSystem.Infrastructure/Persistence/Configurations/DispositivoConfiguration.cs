using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Dispositivo
    /// </summary>
    public class DispositivoConfiguration : IEntityTypeConfiguration<Dispositivo>
    {
        /// <summary>
        /// Configura la entidad Dispositivo
        /// </summary>
        /// <param name="builder">Constructor de la entidad</param>
        public void Configure(EntityTypeBuilder<Dispositivo> builder)
        {
            #region Tabla

            builder.ToTable("dispositivos");

            #endregion

            #region Clave Primaria

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            #endregion

            #region Propiedades

            // Usuario ID
            builder.Property(d => d.UsuarioId)
                .HasColumnName("usuario_id")
                .IsRequired();

            // Device ID
            builder.Property(d => d.DeviceId)
                .HasColumnName("device_id")
                .HasMaxLength(100)
                .IsRequired();

            // Device Name
            builder.Property(d => d.DeviceName)
                .HasColumnName("device_name")
                .HasMaxLength(200);

            // Tipo
            builder.Property(d => d.Tipo)
                .HasColumnName("tipo")
                .HasMaxLength(20);

            // SO
            builder.Property(d => d.SO)
                .HasColumnName("so")
                .HasMaxLength(50);

            // Navegador
            builder.Property(d => d.Navegador)
                .HasColumnName("navegador")
                .HasMaxLength(50);

            // Token FCM
            builder.Property(d => d.TokenFCM)
                .HasColumnName("token_fcm")
                .HasMaxLength(500);

            // IP Última Conexión
            builder.Property(d => d.IpUltimaConexion)
                .HasColumnName("ip_ultima_conexion")
                .HasMaxLength(45);

            // Fecha Registro
            builder.Property(d => d.FechaRegistro)
                .HasColumnName("fecha_registro")
                                .IsRequired();

            // Última Actividad
            builder.Property(d => d.UltimaActividad)
                .HasColumnName("ultima_actividad")
                                .IsRequired();

            // Activo
            builder.Property(d => d.Activo)
                .HasColumnName("activo")
                .HasDefaultValue(true)
                .IsRequired();

            #endregion

            #region Índices

            // Índice único en DeviceId
            builder.HasIndex(d => d.DeviceId)
                .HasDatabaseName("idx_dispositivo_device_id")
                .IsUnique();

            // Índice compuesto en UsuarioId y Activo
            builder.HasIndex(d => new { d.UsuarioId, d.Activo })
                .HasDatabaseName("idx_dispositivo_usuario_activo");

            #endregion

            #region Relaciones

            // Relación con Usuario ya está definida en UsuarioConfiguration

            #endregion
        }
    }
}
