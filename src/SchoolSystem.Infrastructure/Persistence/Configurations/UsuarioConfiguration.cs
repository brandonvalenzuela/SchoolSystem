using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Entities.Academico;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Usuario
    /// </summary>
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        /// <summary>
        /// Configura la entidad Usuario para EF Core
        /// </summary>
        /// <param name="builder">Constructor de la entidad</param>
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            #region Tabla

            builder.ToTable("Usuarios");

            #endregion

            #region Clave Primaria

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            #endregion

            #region Foreign Keys

            // Escuela ID (Multi-tenant)
            builder.Property(u => u.EscuelaId)
                .IsRequired();

            #endregion

            #region Credenciales de Autenticación

            // Username
            builder.Property(u => u.Username)
                .HasMaxLength(50)
                .IsRequired();

            // Email
            builder.Property(u => u.Email)
                .HasMaxLength(100)
                .IsRequired();

            // Password Hash
            builder.Property(u => u.PasswordHash)
                .HasMaxLength(255)
                .IsRequired();

            // Rol
            builder.Property(u => u.Rol)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            #endregion

            #region Datos Personales

            // Nombre
            builder.Property(u => u.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            // Apellido Paterno
            builder.Property(u => u.ApellidoPaterno)
                .HasMaxLength(100)
                .IsRequired();

            // Apellido Materno
            builder.Property(u => u.ApellidoMaterno)
                .HasMaxLength(100);

            // Teléfono
            builder.Property(u => u.Telefono)
                .HasMaxLength(20);

            // Teléfono Emergencia
            builder.Property(u => u.TelefonoEmergencia)
                .HasMaxLength(20)
                .IsRequired(false);

            // Foto URL
            builder.Property(u => u.FotoUrl)
                .IsRequired(false) // <-- Le dices que SÍ puede ser nula
                .HasMaxLength(500);

            // Fecha Nacimiento
            builder.Property(u => u.FechaNacimiento)
                .HasColumnType("DATE");

            // Género
            builder.Property(u => u.Genero)
                .HasConversion<string>()
                .HasMaxLength(20);

            #endregion

            #region Control de Estado y Sesión

            // Activo
            builder.Property(u => u.Activo)
                .HasDefaultValue(true)
                .IsRequired();

            // Último Acceso
            builder.Property(u => u.UltimoAcceso)
                .HasColumnType("DATETIME");

            // Token Recuperación
            builder.Property(u => u.TokenRecuperacion)
                .HasMaxLength(255)
                .IsRequired(false);

            // Token Expiración
            builder.Property(u => u.TokenExpiracion)
                .HasColumnType("DATETIME")
                .IsRequired(false);

            // Intentos Fallidos
            builder.Property(u => u.IntentosFallidos)
                .HasDefaultValue(0)
                .IsRequired();

            // Bloqueado Hasta
            builder.Property(u => u.BloqueadoHasta)
                .HasColumnType("DATETIME");

            #endregion

            #region Auditoría

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(a => a.UpdatedAt)
                .IsRequired();

            builder.Property(a => a.CreatedBy);

            builder.Property(a => a.UpdatedBy);

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

            // NombreCompleto es una propiedad calculada, no se mapea a la BD
            builder.Ignore(u => u.NombreCompleto);

            // Edad es una propiedad calculada
            builder.Ignore(u => u.Edad);

            #endregion

            #region Índices

            // Índice único en Username
            builder.HasIndex(u => u.Username)
                .HasDatabaseName("IX_Usuario_Username")
                .IsUnique();

            // Índice único en Email
            builder.HasIndex(u => u.Email)
                .HasDatabaseName("IX_Usuario_Email")
                .IsUnique();

            // Índice compuesto en EscuelaId y Rol (búsquedas frecuentes)
            builder.HasIndex(u => new { u.EscuelaId, u.Rol })
                .HasDatabaseName("IX_Usuario_Escuela_Rol");

            // Índice en Activo
            builder.HasIndex(u => u.Activo)
                .HasDatabaseName("IX_Usuario_Activo");

            #endregion

            #region Relaciones

            // Relación con Escuela (muchos usuarios pertenecen a una escuela)
            builder.HasOne(u => u.Escuela)
                .WithMany(e => e.Usuarios)
                .HasForeignKey(u => u.EscuelaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación uno a uno con Maestro (opcional)
            builder.HasOne(u => u.Maestro)
                .WithOne(m => m.Usuario)
                .HasForeignKey<Maestro>(m => m.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación uno a uno con Padre (opcional)
            builder.HasOne(u => u.Padre)
                .WithOne(p => p.Usuario)
                .HasForeignKey<Padre>(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación uno a uno con Alumno (opcional)
            builder.HasOne(u => u.Alumno)
                .WithOne(a => a.Usuario)
                .HasForeignKey<Alumno>(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con Dispositivos (un usuario tiene muchos dispositivos)
            builder.HasMany(u => u.Dispositivos)
                .WithOne(d => d.Usuario)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}