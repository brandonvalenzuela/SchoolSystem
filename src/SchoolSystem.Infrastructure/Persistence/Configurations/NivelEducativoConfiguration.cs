using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Academico;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad NivelEducativo
    /// </summary>
    public class NivelEducativoConfiguration : IEntityTypeConfiguration<NivelEducativo>
    {
        public void Configure(EntityTypeBuilder<NivelEducativo> builder)
        {
            // Nombre de tabla
            builder.ToTable("NivelesEducativos");

            // Clave primaria
            builder.HasKey(ne => ne.Id);

            // Propiedades requeridas
            builder.Property(ne => ne.EscuelaId)
                .IsRequired();

            builder.Property(ne => ne.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(ne => ne.Abreviatura)
                .HasMaxLength(10)
                .IsRequired(false);

            builder.Property(ne => ne.Descripcion)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(ne => ne.Orden)
                .IsRequired();

            builder.Property(ne => ne.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            // Propiedades opcionales
            builder.Property(ne => ne.EdadMinima)
                .IsRequired(false);

            builder.Property(ne => ne.EdadMaxima)
                .IsRequired(false);

            builder.Property(ne => ne.DuracionAños)
                .IsRequired(false);

            builder.Property(ne => ne.Color)
                .HasMaxLength(7) // Para formato hexadecimal #FFFFFF
                .IsRequired(false);
            // Auditoría
            builder.Property(ne => ne.CreatedAt)
                .IsRequired();

            builder.Property(ne => ne.UpdatedAt)
                .IsRequired();

            builder.Property(ne => ne.CreatedBy)
                .IsRequired(false);

            builder.Property(ne => ne.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(ne => ne.Escuela)
                .WithMany(e => e.NivelesEducativos)
                .HasForeignKey(ne => ne.EscuelaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(ne => ne.Grados)
                .WithOne(g => g.NivelEducativo)
                .HasForeignKey(g => g.NivelEducativoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(ne => ne.EscuelaId)
                .HasDatabaseName("IX_NivelesEducativos_EscuelaId");

            builder.HasIndex(ne => ne.Nombre)
                .HasDatabaseName("IX_NivelesEducativos_Nombre");

            builder.HasIndex(ne => ne.Activo)
                .HasDatabaseName("IX_NivelesEducativos_Activo");

            builder.HasIndex(ne => ne.Orden)
                .HasDatabaseName("IX_NivelesEducativos_Orden");

            // Índice único compuesto: El nombre del nivel debe ser único por escuela
            builder.HasIndex(ne => new { ne.EscuelaId, ne.Nombre })
                .IsUnique()
                .HasDatabaseName("IX_NivelesEducativos_Escuela_Nombre_Unique");

            builder.HasIndex(ne => new { ne.EscuelaId, ne.Activo, ne.Orden })
                .HasDatabaseName("IX_NivelesEducativos_Escuela_Activo_Orden");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(ne => ne.CantidadGrados);
            builder.Ignore(ne => ne.CantidadGradosActivos);

            // Constraints
            builder.HasCheckConstraint("CK_NivelesEducativos_Orden",
                "`Orden` >= 0");

            builder.HasCheckConstraint("CK_NivelesEducativos_Edades",
                "`EdadMinima` IS NULL OR `EdadMaxima` IS NULL OR `EdadMinima` <= `EdadMaxima`");

            builder.HasCheckConstraint("CK_NivelesEducativos_EdadMinima",
                "`EdadMinima` IS NULL OR `EdadMinima` >= 0");

            builder.HasCheckConstraint("CK_NivelesEducativos_EdadMaxima",
                "`EdadMaxima` IS NULL OR `EdadMaxima` >= 0");

            builder.HasCheckConstraint("CK_NivelesEducativos_DuracionAños",
                "`DuracionAños` IS NULL OR `DuracionAños` > 0");
        }
    }
}