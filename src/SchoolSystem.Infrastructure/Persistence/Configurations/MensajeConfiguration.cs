using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Comunicacion;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Mensaje
    /// </summary>
    public class MensajeConfiguration : IEntityTypeConfiguration<Mensaje>
    {
        public void Configure(EntityTypeBuilder<Mensaje> builder)
        {
            // Nombre de tabla
            builder.ToTable("Mensajes");

            // Clave primaria
            builder.HasKey(m => m.Id);

            // Propiedades requeridas
            builder.Property(m => m.EscuelaId)
                .IsRequired();

            builder.Property(m => m.EmisorId)
                .IsRequired();

            builder.Property(m => m.ReceptorId)
                .IsRequired();

            builder.Property(m => m.AlumnoRelacionadoId)
                .IsRequired(false);

            builder.Property(m => m.Asunto)
                .HasMaxLength(200);

            builder.Property(m => m.Contenido)
                .IsRequired()
                .HasColumnType("LONGTEXT");

            // Fechas y estado
            builder.Property(m => m.FechaEnvio)
                .IsRequired();

            builder.Property(m => m.Leido)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(m => m.FechaLectura)
                .IsRequired(false);

            // Archivos adjuntos
            builder.Property(m => m.ArchivoAdjuntoUrl)
                .HasMaxLength(500);

            builder.Property(m => m.ArchivoAdjuntoNombre)
                .HasMaxLength(200);

            builder.Property(m => m.ArchivoAdjuntoTamano)
                .IsRequired(false);

            builder.Property(m => m.ArchivoAdjuntoTipo)
                .HasMaxLength(100);

            // Control de mensajes
            builder.Property(m => m.MensajePadreId)
                .IsRequired(false);

            builder.Property(m => m.EliminadoPorEmisor)
                .HasDefaultValue(false);

            builder.Property(m => m.EliminadoPorReceptor)
                .HasDefaultValue(false);

            builder.Property(m => m.Importante)
                .HasDefaultValue(false);

            builder.Property(m => m.Archivado)
                .HasDefaultValue(false);

            // Auditoría
            builder.Property(m => m.CreatedAt)
                .IsRequired();

            builder.Property(m => m.CreatedBy)
                .IsRequired(false);

            builder.Property(m => m.UpdatedAt)
                .IsRequired();

            builder.Property(m => m.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(m => m.Emisor)
                .WithMany()
                .HasForeignKey(m => m.EmisorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Receptor)
                .WithMany()
                .HasForeignKey(m => m.ReceptorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.AlumnoRelacionado)
                .WithMany()
                .HasForeignKey(m => m.AlumnoRelacionadoId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(m => m.MensajePadre)
                .WithMany(mp => mp.Respuestas)
                .HasForeignKey(m => m.MensajePadreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(m => m.EscuelaId)
                .HasDatabaseName("IX_Mensajes_EscuelaId");

            builder.HasIndex(m => new { m.ReceptorId, m.Leido })
                .HasDatabaseName("IX_Mensajes_Receptor_Leido");

            builder.HasIndex(m => m.EmisorId)
                .HasDatabaseName("IX_Mensajes_EmisorId");

            builder.HasIndex(m => m.FechaEnvio)
                .HasDatabaseName("IX_Mensajes_FechaEnvio");

            builder.HasIndex(m => m.AlumnoRelacionadoId)
                .HasDatabaseName("IX_Mensajes_AlumnoRelacionadoId");

            builder.HasIndex(m => m.MensajePadreId)
                .HasDatabaseName("IX_Mensajes_MensajePadreId");

            builder.HasIndex(m => new { m.EmisorId, m.ReceptorId, m.FechaEnvio })
                .HasDatabaseName("IX_Mensajes_Emisor_Receptor_Fecha");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(m => m.PendienteLectura);
            builder.Ignore(m => m.EsRespuesta);
            builder.Ignore(m => m.TieneArchivo);
            builder.Ignore(m => m.EstaActivo);
            builder.Ignore(m => m.TiempoDesdeEnvio);
            builder.Ignore(m => m.MinutosDesdeEnvio);
            builder.Ignore(m => m.HorasDesdeEnvio);
            builder.Ignore(m => m.DiasDesdeEnvio);
            builder.Ignore(m => m.TotalRespuestas);
            builder.Ignore(m => m.TiempoTranscurridoTexto);

            // Constraints
            builder.HasCheckConstraint("CK_Mensajes_Emisor_Receptor",
                "`EmisorId` <> `ReceptorId`");
        }
    }
}