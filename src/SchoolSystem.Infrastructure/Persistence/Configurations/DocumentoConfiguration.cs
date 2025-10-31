using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Documentos;
using SchoolSystem.Domain.Enums.Documentos;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Documento
    /// </summary>
    public class DocumentoConfiguration : IEntityTypeConfiguration<Documento>
    {
        public void Configure(EntityTypeBuilder<Documento> builder)
        {
            // Nombre de tabla
            builder.ToTable("Documentos");

            // Clave primaria
            builder.HasKey(d => d.Id);

            // Propiedades requeridas
            builder.Property(d => d.EscuelaId)
                .IsRequired();

            builder.Property(d => d.PlantillaDocumentoId)
                .IsRequired(false);

            builder.Property(d => d.TipoDocumento)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(d => d.Titulo)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(d => d.Folio)
                .HasMaxLength(50);

            // Entidad relacionada
            builder.Property(d => d.TipoEntidad)
                .HasMaxLength(50);

            builder.Property(d => d.EntidadRelacionadaId)
                .IsRequired(false);

            builder.Property(d => d.NombreEntidadRelacionada)
                .HasMaxLength(300);

            // Contenido
            builder.Property(d => d.ContenidoHtml)
                .HasColumnType("LONGTEXT");

            builder.Property(d => d.ArchivoUrl)
                .HasMaxLength(500);

            builder.Property(d => d.NombreArchivo)
                .HasMaxLength(200);

            builder.Property(d => d.TamanioArchivo)
                .IsRequired(false);

            // Fechas
            builder.Property(d => d.FechaGeneracion)
                .IsRequired();

            builder.Property(d => d.FechaVigencia)
                .IsRequired(false);

            builder.Property(d => d.FechaVencimiento)
                .IsRequired(false);

            // Estado
            builder.Property(d => d.Estado)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(EstadoDocumento.Borrador);

            builder.Property(d => d.FechaEnvio)
                .IsRequired(false);

            builder.Property(d => d.CorreosEnvio)
                .HasMaxLength(500);

            // Firma digital
            builder.Property(d => d.TieneFirmaDigital)
                .HasDefaultValue(false);

            builder.Property(d => d.HashFirma)
                .HasMaxLength(500);

            builder.Property(d => d.FechaFirma)
                .IsRequired(false);

            builder.Property(d => d.FirmadoPorId)
                .IsRequired(false);

            builder.Property(d => d.CertificadoFirma)
                .HasMaxLength(500);

            // Usuario generador
            builder.Property(d => d.GeneradoPorId)
                .IsRequired();

            builder.Property(d => d.GeneradoAutomaticamente)
                .HasDefaultValue(false);

            // Metadata
            builder.Property(d => d.Descripcion)
                .HasColumnType("LONGTEXT");

            builder.Property(d => d.Tags)
                .HasMaxLength(500);

            builder.Property(d => d.DatosAdicionales)
                .HasColumnType("LONGTEXT");

            builder.Property(d => d.CantidadDescargas)
                .HasDefaultValue(0);

            builder.Property(d => d.FechaUltimaDescarga)
                .IsRequired(false);

            // Control
            builder.Property(d => d.EsPublico)
                .HasDefaultValue(false);

            builder.Property(d => d.Archivado)
                .HasDefaultValue(false);

            builder.Property(d => d.FechaArchivado)
                .IsRequired(false);

            builder.Property(d => d.Observaciones)
                .HasColumnType("LONGTEXT");

            // Auditoría
            builder.Property(d => d.CreatedAt)
                .IsRequired();

            builder.Property(d => d.CreatedBy)
                .IsRequired(false);

            builder.Property(d => d.UpdatedAt)
                .IsRequired();

            builder.Property(d => d.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(d => d.PlantillaDocumento)
                .WithMany(pd => pd.Documentos)
                .HasForeignKey(d => d.PlantillaDocumentoId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(d => d.GeneradoPor)
                .WithMany()
                .HasForeignKey(d => d.GeneradoPorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.FirmadoPor)
                .WithMany()
                .HasForeignKey(d => d.FirmadoPorId)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices
            builder.HasIndex(d => d.EscuelaId)
                .HasDatabaseName("IX_Documentos_EscuelaId");

            builder.HasIndex(d => d.PlantillaDocumentoId)
                .HasDatabaseName("IX_Documentos_PlantillaDocumentoId");

            builder.HasIndex(d => d.TipoDocumento)
                .HasDatabaseName("IX_Documentos_TipoDocumento");

            builder.HasIndex(d => d.Folio)
                .HasDatabaseName("IX_Documentos_Folio");

            builder.HasIndex(d => d.Estado)
                .HasDatabaseName("IX_Documentos_Estado");

            builder.HasIndex(d => d.FechaGeneracion)
                .HasDatabaseName("IX_Documentos_FechaGeneracion");

            builder.HasIndex(d => d.FechaVencimiento)
                .HasDatabaseName("IX_Documentos_FechaVencimiento");

            builder.HasIndex(d => new { d.TipoEntidad, d.EntidadRelacionadaId })
                .HasDatabaseName("IX_Documentos_TipoEntidad_EntidadId");

            builder.HasIndex(d => d.GeneradoPorId)
                .HasDatabaseName("IX_Documentos_GeneradoPorId");

            builder.HasIndex(d => d.TieneFirmaDigital)
                .HasDatabaseName("IX_Documentos_TieneFirmaDigital");

            builder.HasIndex(d => d.Archivado)
                .HasDatabaseName("IX_Documentos_Archivado");

            builder.HasIndex(d => d.EsPublico)
                .HasDatabaseName("IX_Documentos_EsPublico");

            builder.HasIndex(d => new { d.EscuelaId, d.Estado, d.FechaGeneracion })
                .HasDatabaseName("IX_Documentos_Escuela_Estado_Fecha");

            builder.HasIndex(d => new { d.EscuelaId, d.TipoDocumento, d.Estado })
                .HasDatabaseName("IX_Documentos_Escuela_Tipo_Estado");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(d => d.EsBorrador);
            builder.Ignore(d => d.EstaGenerado);
            builder.Ignore(d => d.FueEnviado);
            builder.Ignore(d => d.EstaFirmado);
            builder.Ignore(d => d.TieneArchivo);
            builder.Ignore(d => d.EsBoleta);
            builder.Ignore(d => d.EsConstancia);
            builder.Ignore(d => d.EstaVigente);
            builder.Ignore(d => d.EstaVencido);
            builder.Ignore(d => d.DiasHastaVencimiento);
            builder.Ignore(d => d.DiasDesdeGeneracion);
            builder.Ignore(d => d.HaSidoDescargado);
            builder.Ignore(d => d.TamanioArchivoKB);
            builder.Ignore(d => d.TamanioArchivoMB);

            // Constraints
            builder.HasCheckConstraint("CK_Documentos_FechaVencimiento",
                "`FechaVencimiento` IS NULL OR (`FechaVigencia` IS NOT NULL AND `FechaVencimiento` >= `FechaVigencia`)");

            builder.HasCheckConstraint("CK_Documentos_TamanioArchivo",
                "`TamanioArchivo` IS NULL OR `TamanioArchivo` >= 0");

            builder.HasCheckConstraint("CK_Documentos_CantidadDescargas",
                "`CantidadDescargas` >= 0");

            builder.HasCheckConstraint("CK_Documentos_FirmaDigital",
                "(`TieneFirmaDigital` = 0) OR (`TieneFirmaDigital` = 1 AND `HashFirma` IS NOT NULL AND `FirmadoPorId` IS NOT NULL)");
        }
    }
}