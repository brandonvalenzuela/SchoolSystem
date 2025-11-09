using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Biblioteca;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad CategoriaRecurso
    /// </summary>
    public class CategoriaRecursoConfiguration : IEntityTypeConfiguration<CategoriaRecurso>
    {
        public void Configure(EntityTypeBuilder<CategoriaRecurso> builder)
        {
            // Nombre de tabla
            builder.ToTable("CategoriasRecurso");

            // Clave primaria
            builder.HasKey(cr => cr.Id);

            // Propiedades requeridas
            builder.Property(cr => cr.EscuelaId)
                .IsRequired();

            builder.Property(cr => cr.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(cr => cr.Descripcion)
                .IsRequired(false)
                .HasColumnType("LONGTEXT");

            // Configuración
            builder.Property(cr => cr.Color)
                .HasMaxLength(20);

            builder.Property(cr => cr.Icono)
                .HasMaxLength(50);

            builder.Property(cr => cr.Codigo)
                .HasMaxLength(20);

            builder.Property(cr => cr.Orden)
                .HasDefaultValue(0);

            builder.Property(cr => cr.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            // Estadísticas
            builder.Property(cr => cr.CantidadRecursos)
                .HasDefaultValue(0);

            // Auditoría
            builder.Property(cr => cr.CreatedAt)
                .IsRequired();

            builder.Property(cr => cr.CreatedBy)
                .IsRequired(false);

            builder.Property(cr => cr.UpdatedAt)
                .IsRequired();

            builder.Property(cr => cr.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasMany(cr => cr.Recursos)
                .WithOne(l => l.Categoria)
                .HasForeignKey(l => l.CategoriaId)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices
            builder.HasIndex(cr => cr.EscuelaId)
                .HasDatabaseName("IX_CategoriasRecurso_EscuelaId");

            builder.HasIndex(cr => cr.Nombre)
                .HasDatabaseName("IX_CategoriasRecurso_Nombre");

            builder.HasIndex(cr => cr.Codigo)
                .HasDatabaseName("IX_CategoriasRecurso_Codigo");

            builder.HasIndex(cr => cr.Activo)
                .HasDatabaseName("IX_CategoriasRecurso_Activo");

            builder.HasIndex(cr => new { cr.EscuelaId, cr.Activo, cr.Orden })
                .HasDatabaseName("IX_CategoriasRecurso_Escuela_Activo_Orden");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(cr => cr.TieneRecursos);
            builder.Ignore(cr => cr.NombreCompleto);

            // Constraints
            builder.HasCheckConstraint("CK_CategoriasRecurso_CantidadRecursos",
                "`CantidadRecursos` >= 0");

            builder.HasCheckConstraint("CK_CategoriasRecurso_Orden",
                "`Orden` >= 0");
        }
    }
}