using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Conducta;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Insignia
    /// </summary>
    public class InsigniaConfiguration : IEntityTypeConfiguration<Insignia>
    {
        public void Configure(EntityTypeBuilder<Insignia> builder)
        {
            // Nombre de tabla
            builder.ToTable("Insignias");

            // Clave primaria
            builder.HasKey(i => i.Id);

            // Propiedades requeridas
            builder.Property(i => i.EscuelaId)
                .IsRequired();

            builder.Property(i => i.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(i => i.Icono)
                .HasMaxLength(500);

            builder.Property(i => i.Descripcion)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(i => i.Criterios)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(i => i.Tipo)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(i => i.Rareza)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(i => i.PuntosOtorgados)
                .IsRequired();

            builder.Property(i => i.Requisitos)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(i => i.EsRecurrente)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(i => i.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            // Auditoría
            builder.Property(i => i.CreatedAt)
                .IsRequired();

            builder.Property(i => i.UpdatedAt)
                .IsRequired();

            builder.Property(i => i.CreatedBy)
                .IsRequired(false);

            builder.Property(i => i.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasMany(i => i.AlumnosQueGanaron)
                .WithOne(ai => ai.Insignia)
                .HasForeignKey(ai => ai.InsigniaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices
            builder.HasIndex(i => i.EscuelaId)
                .HasDatabaseName("IX_Insignias_EscuelaId");

            builder.HasIndex(i => i.Nombre)
                .HasDatabaseName("IX_Insignias_Nombre");

            builder.HasIndex(i => i.Tipo)
                .HasDatabaseName("IX_Insignias_Tipo");

            builder.HasIndex(i => i.Rareza)
                .HasDatabaseName("IX_Insignias_Rareza");

            builder.HasIndex(i => i.Activo)
                .HasDatabaseName("IX_Insignias_Activo");

            builder.HasIndex(i => i.PuntosOtorgados)
                .HasDatabaseName("IX_Insignias_PuntosOtorgados");

            // Índice único: El nombre de la insignia debe ser único por escuela
            builder.HasIndex(i => new { i.EscuelaId, i.Nombre })
                .IsUnique()
                .HasDatabaseName("IX_Insignias_Escuela_Nombre_Unique");

            builder.HasIndex(i => new { i.EscuelaId, i.Activo })
                .HasDatabaseName("IX_Insignias_Escuela_Activo");

            builder.HasIndex(i => new { i.EscuelaId, i.Tipo, i.Activo })
                .HasDatabaseName("IX_Insignias_Escuela_Tipo_Activo");

            builder.HasIndex(i => new { i.Tipo, i.Rareza })
                .HasDatabaseName("IX_Insignias_Tipo_Rareza");

            // Constraints
            builder.HasCheckConstraint("CK_Insignias_PuntosOtorgados",
                "`PuntosOtorgados` >= 0");
        }
    }
}