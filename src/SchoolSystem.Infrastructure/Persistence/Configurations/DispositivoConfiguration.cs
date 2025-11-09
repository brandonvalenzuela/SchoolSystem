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

            builder.ToTable("Dispositivos");

            #endregion

            #region Clave Primaria

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                .ValueGeneratedOnAdd();

            #endregion

            #region Propiedades

            // Usuario ID
            builder.Property(d => d.UsuarioId)
                .IsRequired();

            // Device ID
            builder.Property(d => d.DeviceId)
                .HasMaxLength(100)
                .IsRequired();

            // Device Name
            builder.Property(d => d.DeviceName)
                .HasMaxLength(200)
                .IsRequired(false);

            // Tipo
            builder.Property(d => d.Tipo)
                .HasMaxLength(20)
                .IsRequired(false);

            // SO
            builder.Property(d => d.SO)
                .HasMaxLength(50)
                .IsRequired(false);

            // Navegador
            builder.Property(d => d.Navegador)
                .HasMaxLength(50)
                .IsRequired(false);

            // Token FCM
            builder.Property(d => d.TokenFCM)
                .HasMaxLength(500)
                .IsRequired(false);

            // IP Última Conexión
            builder.Property(d => d.IpUltimaConexion)
                .HasMaxLength(45)
                .IsRequired(false);

            // Fecha Registro
            builder.Property(d => d.FechaRegistro)
                .IsRequired();

            // Última Actividad
            builder.Property(d => d.UltimaActividad)
                .IsRequired();

            // Activo
            builder.Property(d => d.Activo)
                .HasDefaultValue(true)
                .IsRequired();

            #endregion

            #region Índices

            // Índice único en DeviceId
            builder.HasIndex(d => d.DeviceId)
                .HasDatabaseName("IX_Dispositivo_Device_Id")
                .IsUnique();

            // Índice compuesto en UsuarioId y Activo
            builder.HasIndex(d => new { d.UsuarioId, d.Activo })
                .HasDatabaseName("IX_Dispositivo_Usuario_Activo");

            #endregion

            #region Relaciones

            // Relación con Usuario ya está definida en UsuarioConfiguration

            #endregion
        }
    }
}
