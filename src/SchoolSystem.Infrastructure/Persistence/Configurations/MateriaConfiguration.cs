using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Academico;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Materia
    /// </summary>
    public class MateriaConfiguration : IEntityTypeConfiguration<Materia>
    {
        public void Configure(EntityTypeBuilder<Materia> builder)
        {
            // Nombre de tabla
            builder.ToTable("Materias");

            // Clave primaria
            builder.HasKey(m => m.Id);

            // Propiedades requeridas
            builder.Property(m => m.EscuelaId)
                .IsRequired();

            builder.Property(m => m.Nombre)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(m => m.Clave)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(m => m.Descripcion)
                .IsRequired(false)
                .HasMaxLength(1000);

            // Icono
            builder.Property(m => m.Icono)
                .IsRequired(false)
                .HasConversion<string>()
                .HasMaxLength(50);

            // Color
            builder.Property(m => m.Color)
                .HasMaxLength(7); // Para formato hexadecimal #FFFFFF

            builder.Property(m => m.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            // Configuración de la materia
            builder.Property(m => m.Area)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(100);

            builder.Property(m => m.Tipo)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(m => m.NivelDificultad)
                .IsRequired(false);

            builder.Property(m => m.RequiereMateriales)
                .HasDefaultValue(false);

            builder.Property(m => m.MaterialesRequeridos)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(m => m.RequiereInstalacionesEspeciales)
                .HasDefaultValue(false);

            builder.Property(m => m.InstalacionesRequeridas)
                .HasMaxLength(500)
                .IsRequired(false);

            // Objetivos y contenido
            builder.Property(m => m.Objetivos)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(m => m.Competencias)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(m => m.ContenidoTematico)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(m => m.Bibliografia)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            // Auditoría
            builder.Property(m => m.CreatedAt)
                .IsRequired();

            builder.Property(m => m.UpdatedAt)
                .IsRequired();

            builder.Property(m => m.CreatedBy)
                .IsRequired(false);

            builder.Property(m => m.UpdatedBy)
                .IsRequired(false);

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

            // Relaciones
            builder.HasOne(m => m.Escuela)
                .WithMany(e => e.Materias)
                .HasForeignKey(m => m.EscuelaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.GradoMaterias)
                .WithOne(gm => gm.Materia)
                .HasForeignKey(gm => gm.MateriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.AsignacionesGrupos)
                .WithOne(ag => ag.Materia)
                .HasForeignKey(ag => ag.MateriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.Calificaciones)
                .WithOne(c => c.Materia)
                .HasForeignKey(c => c.MateriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(m => m.EscuelaId)
                .HasDatabaseName("IX_Materias_EscuelaId");

            builder.HasIndex(m => m.Nombre)
                .HasDatabaseName("IX_Materias_Nombre");

            builder.HasIndex(m => m.Clave)
                .HasDatabaseName("IX_Materias_Clave");

            builder.HasIndex(m => m.Activo)
                .HasDatabaseName("IX_Materias_Activo");

            builder.HasIndex(m => m.Area)
                .HasDatabaseName("IX_Materias_Area");

            builder.HasIndex(m => m.Tipo)
                .HasDatabaseName("IX_Materias_Tipo");

            // Índice único compuesto: La clave de la materia debe ser única por escuela
            builder.HasIndex(m => new { m.EscuelaId, m.Clave })
                .IsUnique()
                .HasDatabaseName("IX_Materias_Escuela_Clave_Unique");

            builder.HasIndex(m => new { m.EscuelaId, m.Activo })
                .HasDatabaseName("IX_Materias_Escuela_Activo");

            builder.HasIndex(m => new { m.EscuelaId, m.Area, m.Activo })
                .HasDatabaseName("IX_Materias_Escuela_Area_Activo");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(m => m.CantidadGrados);
            builder.Ignore(m => m.CantidadGrupos);
            builder.Ignore(m => m.CantidadMaestros);

            // Constraints
            builder.HasCheckConstraint("CK_Materias_NivelDificultad",
                "`NivelDificultad` IS NULL OR (`NivelDificultad` >= 1 AND `NivelDificultad` <= 5)");
        }
    }
}