using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Academico;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Padre
    /// </summary>
    public class PadreConfiguration : IEntityTypeConfiguration<Padre>
    {
        public void Configure(EntityTypeBuilder<Padre> builder)
        {
            // Nombre de tabla
            builder.ToTable("Padres");

            // Clave primaria
            builder.HasKey(p => p.Id);

            // Propiedades requeridas
            builder.Property(p => p.EscuelaId)
                .IsRequired();

            builder.Property(p => p.UsuarioId)
                .IsRequired();

            // Información laboral
            builder.Property(p => p.Ocupacion)
                .HasMaxLength(200);

            builder.Property(p => p.LugarTrabajo)
                .HasMaxLength(200);

            builder.Property(p => p.TelefonoTrabajo)
                .HasMaxLength(20);

            builder.Property(p => p.DireccionTrabajo)
                .HasMaxLength(500);

            builder.Property(p => p.Puesto)
                .HasMaxLength(200);

            // Información educativa
            builder.Property(p => p.NivelEstudios)
                .HasMaxLength(100);

            builder.Property(p => p.Carrera)
                .HasMaxLength(200)
                .IsRequired(false);

            // Información adicional
            builder.Property(p => p.EstadoCivil)
                .HasMaxLength(50);

            builder.Property(p => p.Observaciones)
                .HasColumnType("LONGTEXT");

            builder.Property(p => p.AceptaSMS)
                .HasDefaultValue(true);

            builder.Property(p => p.AceptaEmail)
                .HasDefaultValue(true);

            builder.Property(p => p.AceptaPush)
                .HasDefaultValue(true);

            // Relaciones
            builder.HasOne(p => p.Escuela)
                .WithMany()
                .HasForeignKey(p => p.EscuelaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.AlumnoPadres)
                .WithOne(ap => ap.Padre)
                .HasForeignKey(ap => ap.PadreId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices
            builder.HasIndex(p => p.EscuelaId)
                .HasDatabaseName("IX_Padres_EscuelaId");

            builder.HasIndex(p => p.UsuarioId)
                .HasDatabaseName("IX_Padres_UsuarioId");

            builder.HasIndex(p => p.Ocupacion)
                .HasDatabaseName("IX_Padres_Ocupacion");

            builder.HasIndex(p => p.NivelEstudios)
                .HasDatabaseName("IX_Padres_NivelEstudios");

            // Índice único: Un usuario solo puede tener un registro de padre por escuela
            builder.HasIndex(p => new { p.EscuelaId, p.UsuarioId })
                .IsUnique()
                .HasDatabaseName("IX_Padres_Escuela_Usuario_Unique");

            builder.HasIndex(p => new { p.EscuelaId, p.AceptaSMS })
                .HasDatabaseName("IX_Padres_Escuela_AceptaSMS");

            builder.HasIndex(p => new { p.EscuelaId, p.AceptaEmail })
                .HasDatabaseName("IX_Padres_Escuela_AceptaEmail");

            builder.HasIndex(p => new { p.EscuelaId, p.AceptaPush })
                .HasDatabaseName("IX_Padres_Escuela_AceptaPush");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(p => p.CantidadHijos);
            builder.Ignore(p => p.AlumnosIds);
        }
    }
}