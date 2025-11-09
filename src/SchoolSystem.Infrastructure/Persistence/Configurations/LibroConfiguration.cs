using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Biblioteca;
using SchoolSystem.Domain.Enums.Biblioteca;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Libro
    /// </summary>
    public class LibroConfiguration : IEntityTypeConfiguration<Libro>
    {
        public void Configure(EntityTypeBuilder<Libro> builder)
        {
            // Nombre de tabla
            builder.ToTable("Libros");

            // Clave primaria
            builder.HasKey(l => l.Id);

            // Propiedades requeridas
            builder.Property(l => l.EscuelaId)
                .IsRequired();

            builder.Property(l => l.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(l => l.Autor)
                .HasMaxLength(200);

            builder.Property(l => l.ISBN)
                .HasMaxLength(20);

            builder.Property(l => l.Editorial)
                .HasMaxLength(100);

            builder.Property(l => l.AnioPublicacion)
                .IsRequired(false);

            // Clasificación
            builder.Property(l => l.Tipo)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(l => l.CategoriaId)
                .IsRequired(false);

            builder.Property(l => l.CodigoClasificacion)
                .HasMaxLength(50)
                .IsRequired(false);

            // Detalles físicos
            builder.Property(l => l.NumeroPaginas)
                .IsRequired(false);

            builder.Property(l => l.Edicion)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(l => l.Idioma)
                .HasMaxLength(50)
                .HasDefaultValue("Español");

            builder.Property(l => l.Descripcion)
                .IsRequired(false)
                .HasColumnType("LONGTEXT");

            // Inventario
            builder.Property(l => l.CantidadTotal)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(l => l.CantidadDisponible)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(l => l.CantidadPrestada)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(l => l.CantidadExtraviada)
                .HasDefaultValue(0);

            builder.Property(l => l.CantidadDaniada)
                .HasDefaultValue(0);

            // Ubicación y estado
            builder.Property(l => l.Ubicacion)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(l => l.Estante)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(l => l.Estado)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(EstadoRecurso.Disponible);

            builder.Property(l => l.DisponiblePrestamo)
                .IsRequired()
                .HasDefaultValue(true);

            // Multimedia
            builder.Property(l => l.ImagenPortadaUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(l => l.RecursoDigitalUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            // Valoración
            builder.Property(l => l.CalificacionPromedio)
                .IsRequired(false)
                .HasColumnType("decimal(3,2)");

            builder.Property(l => l.VecesPrestado)
                .HasDefaultValue(0);

            builder.Property(l => l.Popularidad)
                .HasDefaultValue(0);

            // Metadata
            builder.Property(l => l.PrecioAdquisicion)
                .IsRequired(false)
                .HasColumnType("decimal(10,2)");

            builder.Property(l => l.FechaAdquisicion)
                .IsRequired(false);

            builder.Property(l => l.Proveedor)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(l => l.Notas)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(l => l.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            // Auditoría
            builder.Property(l => l.CreatedAt)
                .IsRequired();

            builder.Property(l => l.CreatedBy)
                .IsRequired(false);

            builder.Property(l => l.UpdatedAt)
                .IsRequired();

            builder.Property(l => l.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(l => l.Categoria)
                .WithMany(c => c.Recursos)
                .HasForeignKey(l => l.CategoriaId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(l => l.Prestamos)
                .WithOne(p => p.Libro)
                .HasForeignKey(p => p.LibroId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(l => l.EscuelaId)
                .HasDatabaseName("IX_Libros_EscuelaId");

            builder.HasIndex(l => l.Titulo)
                .HasDatabaseName("IX_Libros_Titulo");

            builder.HasIndex(l => l.ISBN)
                .HasDatabaseName("IX_Libros_ISBN");

            builder.HasIndex(l => l.Tipo)
                .HasDatabaseName("IX_Libros_Tipo");

            builder.HasIndex(l => l.CategoriaId)
                .HasDatabaseName("IX_Libros_CategoriaId");

            builder.HasIndex(l => l.Estado)
                .HasDatabaseName("IX_Libros_Estado");

            builder.HasIndex(l => l.Activo)
                .HasDatabaseName("IX_Libros_Activo");

            builder.HasIndex(l => new { l.EscuelaId, l.Activo, l.Estado })
                .HasDatabaseName("IX_Libros_Escuela_Activo_Estado");

            builder.HasIndex(l => l.CodigoClasificacion)
                .HasDatabaseName("IX_Libros_CodigoClasificacion");

            builder.HasIndex(l => l.DisponiblePrestamo)
                .HasDatabaseName("IX_Libros_DisponiblePrestamo");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(l => l.HayDisponibles);
            builder.Ignore(l => l.TodosPrestados);
            builder.Ignore(l => l.EsLibro);
            builder.Ignore(l => l.EsDigital);
            builder.Ignore(l => l.TienePortada);
            builder.Ignore(l => l.PorcentajeDisponibilidad);
            builder.Ignore(l => l.InformacionCompleta);
            builder.Ignore(l => l.AniosDesdeAdquisicion);
            builder.Ignore(l => l.EstadoInventario);

            // Constraints
            builder.HasCheckConstraint("CK_Libros_CantidadTotal",
                "`CantidadTotal` >= 0");

            builder.HasCheckConstraint("CK_Libros_CantidadDisponible",
                "`CantidadDisponible` >= 0");

            builder.HasCheckConstraint("CK_Libros_CantidadPrestada",
                "`CantidadPrestada` >= 0");

            builder.HasCheckConstraint("CK_Libros_CantidadesInventario",
                "`CantidadDisponible` + `CantidadPrestada` + `CantidadExtraviada` + `CantidadDaniada` = `CantidadTotal`");

            builder.HasCheckConstraint("CK_Libros_CalificacionPromedio",
                "`CalificacionPromedio` IS NULL OR (`CalificacionPromedio` >= 1 AND `CalificacionPromedio` <= 5)");

            builder.HasCheckConstraint("CK_Libros_PrecioAdquisicion",
                "`PrecioAdquisicion` IS NULL OR `PrecioAdquisicion` >= 0");

            builder.HasCheckConstraint("CK_Libros_AnioPublicacion",
                "`AnioPublicacion` IS NULL OR (`AnioPublicacion` >= 1000 AND `AnioPublicacion` <= 2100 + 1)");
        }
    }
}