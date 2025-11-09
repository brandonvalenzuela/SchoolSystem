using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Academico;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Grado
    /// </summary>
    public class GradoConfiguration : IEntityTypeConfiguration<Grado>
    {
        public void Configure(EntityTypeBuilder<Grado> builder)
        {
            // Nombre de tabla
            builder.ToTable("Grados");

            // Clave primaria
            builder.HasKey(g => g.Id);

            // Propiedades requeridas
            builder.Property(g => g.EscuelaId)
                .IsRequired();

            builder.Property(g => g.NivelEducativoId)
                .IsRequired();

            builder.Property(g => g.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(g => g.Descripcion)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(g => g.Orden)
                .IsRequired();

            builder.Property(g => g.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            // Configuración del grado
            builder.Property(g => g.EdadRecomendada)
                .IsRequired(false);

            builder.Property(g => g.CapacidadMaximaPorGrupo)
                .IsRequired(false);

            builder.Property(g => g.HorasSemanales)
                .IsRequired(false);

            builder.Property(g => g.Requisitos)
                .HasMaxLength(1000)
                .IsRequired(false);

            // Auditoría
            builder.Property(g => g.CreatedAt)
                .IsRequired();

            builder.Property(g => g.UpdatedAt)
                .IsRequired();

            builder.Property(g => g.CreatedBy)
                .IsRequired(false);

            builder.Property(g => g.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(g => g.Escuela)
                .WithMany()
                .HasForeignKey(g => g.EscuelaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(g => g.NivelEducativo)
                .WithMany(ne => ne.Grados)
                .HasForeignKey(g => g.NivelEducativoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(g => g.Grupos)
                .WithOne(gr => gr.Grado)
                .HasForeignKey(gr => gr.GradoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(g => g.GradoMaterias)
                .WithOne(gm => gm.Grado)
                .HasForeignKey(gm => gm.GradoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(g => g.EscuelaId)
                .HasDatabaseName("IX_Grados_EscuelaId");

            builder.HasIndex(g => g.NivelEducativoId)
                .HasDatabaseName("IX_Grados_NivelEducativoId");

            builder.HasIndex(g => g.Nombre)
                .HasDatabaseName("IX_Grados_Nombre");

            builder.HasIndex(g => g.Activo)
                .HasDatabaseName("IX_Grados_Activo");

            builder.HasIndex(g => g.Orden)
                .HasDatabaseName("IX_Grados_Orden");

            // Índice único compuesto: No puede haber dos grados con el mismo nombre en el mismo nivel educativo
            builder.HasIndex(g => new { g.EscuelaId, g.NivelEducativoId, g.Nombre })
                .IsUnique()
                .HasDatabaseName("IX_Grados_Escuela_Nivel_Nombre_Unique");

            // Índice único compuesto: No puede haber dos grados con el mismo orden en el mismo nivel educativo
            builder.HasIndex(g => new { g.EscuelaId, g.NivelEducativoId, g.Orden })
                .IsUnique()
                .HasDatabaseName("IX_Grados_Escuela_Nivel_Orden_Unique");

            builder.HasIndex(g => new { g.EscuelaId, g.Activo })
                .HasDatabaseName("IX_Grados_Escuela_Activo");

            builder.HasIndex(g => new { g.NivelEducativoId, g.Activo, g.Orden })
                .HasDatabaseName("IX_Grados_Nivel_Activo_Orden");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(g => g.NombreCompleto);
            builder.Ignore(g => g.CantidadGrupos);
            builder.Ignore(g => g.CantidadGruposActivos);
            builder.Ignore(g => g.CantidadMaterias);
            builder.Ignore(g => g.TotalHorasMaterias);

            // Constraints
            builder.HasCheckConstraint("CK_Grados_Orden",
                "`Orden` >= 0");

            builder.HasCheckConstraint("CK_Grados_EdadRecomendada",
                "`EdadRecomendada` IS NULL OR (`EdadRecomendada` >= 0 AND `EdadRecomendada` <= 25)");

            builder.HasCheckConstraint("CK_Grados_CapacidadMaximaPorGrupo",
                "`CapacidadMaximaPorGrupo` IS NULL OR `CapacidadMaximaPorGrupo` > 0");

            builder.HasCheckConstraint("CK_Grados_HorasSemanales",
                "`HorasSemanales` IS NULL OR `HorasSemanales` > 0");
        }
    }
}