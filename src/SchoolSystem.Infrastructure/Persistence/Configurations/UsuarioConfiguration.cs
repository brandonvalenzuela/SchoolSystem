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

            builder.ToTable("usuarios");

            #endregion

            #region Clave Primaria

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            #endregion

            #region Foreign Keys

            // Escuela ID (Multi-tenant)
            builder.Property(u => u.EscuelaId)
                .HasColumnName("escuela_id")
                .IsRequired();

            #endregion

            #region Credenciales de Autenticación

            // Username
            builder.Property(u => u.Username)
                .HasColumnName("username")
                .HasMaxLength(50)
                .IsRequired();

            // Email
            builder.Property(u => u.Email)
                .HasColumnName("email")
                .HasMaxLength(100)
                .IsRequired();

            // Password Hash
            builder.Property(u => u.PasswordHash)
                .HasColumnName("password_hash")
                .HasMaxLength(255)
                .IsRequired();

            // Rol
            builder.Property(u => u.Rol)
                .HasColumnName("rol")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            #endregion

            #region Datos Personales

            // Nombre
            builder.Property(u => u.Nombre)
                .HasColumnName("nombre")
                .HasMaxLength(100)
                .IsRequired();

            // Apellido Paterno
            builder.Property(u => u.ApellidoPaterno)
                .HasColumnName("apellido_paterno")
                .HasMaxLength(100)
                .IsRequired();

            // Apellido Materno
            builder.Property(u => u.ApellidoMaterno)
                .HasColumnName("apellido_materno")
                .HasMaxLength(100);

            // Teléfono
            builder.Property(u => u.Telefono)
                .HasColumnName("telefono")
                .HasMaxLength(20);

            // Teléfono Emergencia
            builder.Property(u => u.TelefonoEmergencia)
                .HasColumnName("telefono_emergencia")
                .HasMaxLength(20);

            // Foto URL
            builder.Property(u => u.FotoUrl)
                .HasColumnName("foto_url")
                .HasMaxLength(500);

            // Fecha Nacimiento
            builder.Property(u => u.FechaNacimiento)
                .HasColumnName("fecha_nacimiento")
                .HasColumnType("DATE");

            // Género
            builder.Property(u => u.Genero)
                .HasColumnName("genero")
                .HasConversion<string>()
                .HasMaxLength(20);

            #endregion

            #region Control de Estado y Sesión

            // Activo
            builder.Property(u => u.Activo)
                .HasColumnName("activo")
                .HasDefaultValue(true)
                .IsRequired();

            // Último Acceso
            builder.Property(u => u.UltimoAcceso)
                .HasColumnName("ultimo_acceso")
                .HasColumnType("DATETIME");

            // Token Recuperación
            builder.Property(u => u.TokenRecuperacion)
                .HasColumnName("token_recuperacion")
                .HasMaxLength(255);

            // Token Expiración
            builder.Property(u => u.TokenExpiracion)
                .HasColumnName("token_expiracion")
                .HasColumnType("DATETIME");

            // Intentos Fallidos
            builder.Property(u => u.IntentosFallidos)
                .HasColumnName("intentos_fallidos")
                .HasDefaultValue(0)
                .IsRequired();

            // Bloqueado Hasta
            builder.Property(u => u.BloqueadoHasta)
                .HasColumnName("bloqueado_hasta")
                .HasColumnType("DATETIME");

            #endregion

            #region Auditoría

            builder.Property(u => u.CreatedAt)
                .HasColumnName("created_at")
                                .IsRequired();

            builder.Property(u => u.UpdatedAt)
                .HasColumnName("updated_at")
                                .IsRequired();

            builder.Property(u => u.CreatedBy)
                .HasColumnName("created_by");

            builder.Property(u => u.UpdatedBy)
                .HasColumnName("updated_by");

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
                .HasDatabaseName("idx_usuario_username")
                .IsUnique();

            // Índice único en Email
            builder.HasIndex(u => u.Email)
                .HasDatabaseName("idx_usuario_email")
                .IsUnique();

            // Índice compuesto en EscuelaId y Rol (búsquedas frecuentes)
            builder.HasIndex(u => new { u.EscuelaId, u.Rol })
                .HasDatabaseName("idx_usuario_escuela_rol");

            // Índice en Activo
            builder.HasIndex(u => u.Activo)
                .HasDatabaseName("idx_usuario_activo");

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