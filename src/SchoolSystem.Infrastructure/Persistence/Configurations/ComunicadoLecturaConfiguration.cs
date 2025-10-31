using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Comunicacion;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad ComunicadoLectura
    /// </summary>
    public class ComunicadoLecturaConfiguration : IEntityTypeConfiguration<ComunicadoLectura>
    {
        public void Configure(EntityTypeBuilder<ComunicadoLectura> builder)
        {
            // Nombre de tabla
            builder.ToTable("ComunicadoLecturas");

            // Clave primaria
            builder.HasKey(cl => cl.Id);

            // Propiedades requeridas
            builder.Property(cl => cl.ComunicadoId)
                .IsRequired();

            builder.Property(cl => cl.UsuarioId)
                .IsRequired();

            builder.Property(cl => cl.FechaLectura)
                .IsRequired();

            builder.Property(cl => cl.Confirmado)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(cl => cl.FechaConfirmacion)
                .IsRequired(false);

            builder.Property(cl => cl.Comentario)
                .HasColumnType("LONGTEXT");

            // Relaciones
            builder.HasOne(cl => cl.Comunicado)
                .WithMany(c => c.Lecturas)
                .HasForeignKey(cl => cl.ComunicadoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cl => cl.Usuario)
                .WithMany()
                .HasForeignKey(cl => cl.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(cl => cl.ComunicadoId)
                .HasDatabaseName("IX_ComunicadoLecturas_ComunicadoId");

            builder.HasIndex(cl => cl.UsuarioId)
                .HasDatabaseName("IX_ComunicadoLecturas_UsuarioId");

            // Índice único: Un usuario solo puede leer un comunicado una vez
            builder.HasIndex(cl => new { cl.ComunicadoId, cl.UsuarioId })
                .IsUnique()
                .HasDatabaseName("IX_ComunicadoLecturas_Comunicado_Usuario_Unique");

            builder.HasIndex(cl => new { cl.ComunicadoId, cl.Confirmado })
                .HasDatabaseName("IX_ComunicadoLecturas_Comunicado_Confirmado");
        }
    }
}