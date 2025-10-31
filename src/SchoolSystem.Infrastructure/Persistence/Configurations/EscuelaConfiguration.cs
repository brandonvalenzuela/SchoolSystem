using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Escuelas;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Escuela
    /// Define el esquema de la tabla, relaciones, índices y constraints
    /// </summary>
    public class EscuelaConfiguration : IEntityTypeConfiguration<Escuela>
    {
        public void Configure(EntityTypeBuilder<Escuela> builder)
        {
            #region Tabla

            // Nombre de tabla
            builder.ToTable("Escuelas");

            #endregion

            #region Clave Primaria

            builder.HasKey(e => e.Id);

            #endregion

            #region Propiedades Básicas

            // Código
            builder.Property(e => e.Codigo)
                .HasMaxLength(20)
                .IsRequired();

            // Nombre
            builder.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsRequired();

            // Razón Social
            builder.Property(e => e.RazonSocial)
                .HasMaxLength(250);

            // RFC
            builder.Property(e => e.RFC)
                .HasMaxLength(13);

            #endregion

            #region Datos de Contacto

            // Dirección
            builder.Property(e => e.Direccion)
                .HasMaxLength(300);

            // Teléfono
            builder.Property(e => e.Telefono)
                .HasMaxLength(20);

            // Email
            builder.Property(e => e.Email)
                .HasMaxLength(100);

            // Sitio Web
            builder.Property(e => e.SitioWeb)
                .HasMaxLength(200);

            // Logo URL
            builder.Property(e => e.LogoUrl)
                .HasMaxLength(500);

            #endregion

            #region Suscripción

            // Plan ID
            builder.Property(e => e.PlanId)
                .IsRequired(false);

            // Fecha Registro
            builder.Property(e => e.FechaRegistro)
                .IsRequired();

            // Fecha Expiración
            builder.Property(e => e.FechaExpiracion)
                .IsRequired(false);

            // Activo
            builder.Property(e => e.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            #endregion

            #region Configuración Personalizada

            // Configuración (JSON)
            builder.Property(e => e.Configuracion)
                .HasColumnType("JSON");

            // Max Alumnos
            builder.Property(e => e.MaxAlumnos)
                .IsRequired(false);

            // Max Maestros
            builder.Property(e => e.MaxMaestros)
                .IsRequired(false);

            // Espacio Almacenamiento
            builder.Property(e => e.EspacioAlmacenamiento)
                .IsRequired(false);

            #endregion

            #region Auditoría

            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.Property(e => e.UpdatedAt)
                .IsRequired();

            builder.Property(e => e.CreatedBy)
                .IsRequired(false);

            builder.Property(e => e.UpdatedBy)
                .IsRequired(false);

            #endregion

            #region Índices

            // Índice único en Codigo
            builder.HasIndex(e => e.Codigo)
                .IsUnique()
                .HasDatabaseName("IX_Escuelas_Codigo_Unique");

            // Índice en Activo
            builder.HasIndex(e => e.Activo)
                .HasDatabaseName("IX_Escuelas_Activo");

            // Índice en Email
            builder.HasIndex(e => e.Email)
                .HasDatabaseName("IX_Escuelas_Email");

            builder.HasIndex(e => e.RFC)
                .HasDatabaseName("IX_Escuelas_RFC");

            #endregion

            #region Relaciones

            // ❌ NO CONFIGURAR RELACIONES AQUÍ
            // Las relaciones ya están configuradas en:
            // - UsuarioConfiguration
            // - AlumnoConfiguration
            // - NivelEducativoConfiguration
            // - GrupoConfiguration
            // - MateriaConfiguration
            // etc.

            // ✅ EF Core detectará automáticamente las navigation properties
            // basándose en las configuraciones de las entidades hijas

            #endregion

            #region Constraints

            builder.HasCheckConstraint("CK_Escuelas_MaxAlumnos",
                "`MaxAlumnos` IS NULL OR `MaxAlumnos` > 0");

            builder.HasCheckConstraint("CK_Escuelas_MaxMaestros",
                "`MaxMaestros` IS NULL OR `MaxMaestros` > 0");

            builder.HasCheckConstraint("CK_Escuelas_EspacioAlmacenamiento",
                "`EspacioAlmacenamiento` IS NULL OR `EspacioAlmacenamiento` > 0");

            #endregion
        }
    }
}