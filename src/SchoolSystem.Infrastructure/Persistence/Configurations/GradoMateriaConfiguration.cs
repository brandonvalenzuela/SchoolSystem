using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Academico;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad GradoMateria
    /// </summary>
    public class GradoMateriaConfiguration : IEntityTypeConfiguration<GradoMateria>
    {
        public void Configure(EntityTypeBuilder<GradoMateria> builder)
        {
            // Nombre de tabla
            builder.ToTable("GradoMaterias");

            // Clave primaria
            builder.HasKey(gm => gm.Id);

            // Propiedades requeridas
            builder.Property(gm => gm.EscuelaId)
                .IsRequired();

            builder.Property(gm => gm.GradoId)
                .IsRequired();

            builder.Property(gm => gm.MateriaId)
                .IsRequired();

            // Propiedades opcionales
            builder.Property(gm => gm.HorasSemanales)
                .IsRequired(false);

            builder.Property(gm => gm.Orden)
                .IsRequired(false);

            builder.Property(gm => gm.Obligatoria)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(gm => gm.PorcentajePeso)
                .IsRequired(false)
                .HasColumnType("decimal(5,2)");

            // Relaciones
            builder.HasOne(gm => gm.Grado)
                .WithMany()
                .HasForeignKey(gm => gm.GradoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(gm => gm.Materia)
                .WithMany()
                .HasForeignKey(gm => gm.MateriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(gm => gm.EscuelaId)
                .HasDatabaseName("IX_GradoMaterias_EscuelaId");

            builder.HasIndex(gm => gm.GradoId)
                .HasDatabaseName("IX_GradoMaterias_GradoId");

            builder.HasIndex(gm => gm.MateriaId)
                .HasDatabaseName("IX_GradoMaterias_MateriaId");

            // Índice único compuesto: Un grado no puede tener la misma materia dos veces
            builder.HasIndex(gm => new { gm.EscuelaId, gm.GradoId, gm.MateriaId })
                .IsUnique()
                .HasDatabaseName("IX_GradoMaterias_Escuela_Grado_Materia_Unique");

            builder.HasIndex(gm => new { gm.GradoId, gm.Orden })
                .HasDatabaseName("IX_GradoMaterias_Grado_Orden");

            builder.HasIndex(gm => gm.Obligatoria)
                .HasDatabaseName("IX_GradoMaterias_Obligatoria");

            // Constraints
            builder.HasCheckConstraint("CK_GradoMaterias_HorasSemanales",
                "`HorasSemanales` IS NULL OR `HorasSemanales` > 0");

            builder.HasCheckConstraint("CK_GradoMaterias_Orden",
                "`Orden` IS NULL OR `Orden` >= 0");

            builder.HasCheckConstraint("CK_GradoMaterias_PorcentajePeso",
                "`PorcentajePeso` IS NULL OR (`PorcentajePeso` >= 0 AND `PorcentajePeso` <= 100)");
        }
    }
}