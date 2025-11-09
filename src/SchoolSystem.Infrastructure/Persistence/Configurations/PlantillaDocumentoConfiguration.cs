using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Documentos;
using SchoolSystem.Domain.Enums.Documentos;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad PlantillaDocumento
    /// </summary>
    public class PlantillaDocumentoConfiguration : IEntityTypeConfiguration<PlantillaDocumento>
    {
        public void Configure(EntityTypeBuilder<PlantillaDocumento> builder)
        {
            // Nombre de tabla
            builder.ToTable("PlantillasDocumento");

            // Clave primaria
            builder.HasKey(pd => pd.Id);

            // Propiedades requeridas
            builder.Property(pd => pd.EscuelaId)
                .IsRequired();

            builder.Property(pd => pd.Nombre)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(pd => pd.TipoDocumento)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(pd => pd.Descripcion)
                .IsRequired(false)
                .HasColumnType("LONGTEXT");

            // Contenido
            builder.Property(pd => pd.ContenidoHtml)
                .IsRequired()
                .HasColumnType("LONGTEXT");

            builder.Property(pd => pd.EstilosCSS)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(pd => pd.VariablesDisponibles)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            // Configuración de página
            builder.Property(pd => pd.TamanioPagina)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("A4");

            builder.Property(pd => pd.Orientacion)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(OrientacionPagina.Vertical);

            builder.Property(pd => pd.MargenSuperior)
                .HasDefaultValue(20);

            builder.Property(pd => pd.MargenInferior)
                .HasDefaultValue(20);

            builder.Property(pd => pd.MargenIzquierdo)
                .HasDefaultValue(20);

            builder.Property(pd => pd.MargenDerecho)
                .HasDefaultValue(20);

            // Encabezado y pie de página
            builder.Property(pd => pd.TieneEncabezado)
                .HasDefaultValue(false);

            builder.Property(pd => pd.EncabezadoHtml)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(pd => pd.AlturaEncabezado)
                .IsRequired(false);

            builder.Property(pd => pd.TienePiePagina)
                .HasDefaultValue(false);

            builder.Property(pd => pd.PiePaginaHtml)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(pd => pd.AlturaPiePagina)
                .IsRequired(false);

            // Marca de agua
            builder.Property(pd => pd.TieneMarcaAgua)
                .HasDefaultValue(false);

            builder.Property(pd => pd.TextoMarcaAgua)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(pd => pd.ImagenMarcaAguaUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(pd => pd.OpacidadMarcaAgua)
                .IsRequired(false);

            // Configuración
            builder.Property(pd => pd.EsPlantillaPorDefecto)
                .HasDefaultValue(false);

            builder.Property(pd => pd.RequiereFirmaDigital)
                .HasDefaultValue(false);

            builder.Property(pd => pd.RequiereFolio)
                .HasDefaultValue(false);

            builder.Property(pd => pd.PrefijoFolio)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(pd => pd.ConsecutivoFolio)
                .IsRequired(false);

            // Estado
            builder.Property(pd => pd.Activa)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(pd => pd.Version)
                .HasMaxLength(20)
                .HasDefaultValue("1.0");

            builder.Property(pd => pd.NotasVersion)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            // Metadata
            builder.Property(pd => pd.Categoria)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(pd => pd.Tags)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(pd => pd.VecesUsada)
                .HasDefaultValue(0);

            builder.Property(pd => pd.FechaUltimaGeneracion)
                .IsRequired(false);

            // Auditoría
            builder.Property(pd => pd.CreatedAt)
                .IsRequired();

            builder.Property(pd => pd.CreatedBy)
                .IsRequired(false);

            builder.Property(pd => pd.UpdatedAt)
                .IsRequired();

            builder.Property(pd => pd.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasMany(pd => pd.Documentos)
                .WithOne(d => d.PlantillaDocumento)
                .HasForeignKey(d => d.PlantillaDocumentoId)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices
            builder.HasIndex(pd => pd.EscuelaId)
                .HasDatabaseName("IX_PlantillasDocumento_EscuelaId");

            builder.HasIndex(pd => pd.Nombre)
                .HasDatabaseName("IX_PlantillasDocumento_Nombre");

            builder.HasIndex(pd => pd.TipoDocumento)
                .HasDatabaseName("IX_PlantillasDocumento_TipoDocumento");

            builder.HasIndex(pd => pd.Activa)
                .HasDatabaseName("IX_PlantillasDocumento_Activa");

            builder.HasIndex(pd => pd.EsPlantillaPorDefecto)
                .HasDatabaseName("IX_PlantillasDocumento_EsPlantillaPorDefecto");

            builder.HasIndex(pd => new { pd.EscuelaId, pd.TipoDocumento, pd.Activa })
                .HasDatabaseName("IX_PlantillasDocumento_Escuela_Tipo_Activa");

            builder.HasIndex(pd => new { pd.EscuelaId, pd.TipoDocumento, pd.EsPlantillaPorDefecto })
                .HasDatabaseName("IX_PlantillasDocumento_Escuela_Tipo_Default");

            builder.HasIndex(pd => pd.Categoria)
                .HasDatabaseName("IX_PlantillasDocumento_Categoria");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(pd => pd.EsBoleta);
            builder.Ignore(pd => pd.EsConstancia);
            builder.Ignore(pd => pd.EsDiploma);
            builder.Ignore(pd => pd.TieneContenido);
            builder.Ignore(pd => pd.HaSidoUsada);
            builder.Ignore(pd => pd.DiasSinUsar);
            builder.Ignore(pd => pd.EsPocoUsada);
            builder.Ignore(pd => pd.NombreCompleto);

            // Constraints
            builder.HasCheckConstraint("CK_PlantillasDocumento_Margenes",
                "`MargenSuperior` >= 0 AND `MargenInferior` >= 0 AND `MargenIzquierdo` >= 0 AND `MargenDerecho` >= 0");

            builder.HasCheckConstraint("CK_PlantillasDocumento_OpacidadMarcaAgua",
                "`OpacidadMarcaAgua` IS NULL OR (`OpacidadMarcaAgua` >= 0 AND `OpacidadMarcaAgua` <= 100)");

            builder.HasCheckConstraint("CK_PlantillasDocumento_ConsecutivoFolio",
                "`ConsecutivoFolio` IS NULL OR `ConsecutivoFolio` >= 1");

            builder.HasCheckConstraint("CK_PlantillasDocumento_VecesUsada",
                "`VecesUsada` >= 0");
        }
    }
}