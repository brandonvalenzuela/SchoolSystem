using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Conducta;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad HistorialPuntos
    /// </summary>
    public class HistorialPuntosConfiguration : IEntityTypeConfiguration<HistorialPuntos>
    {
        public void Configure(EntityTypeBuilder<HistorialPuntos> builder)
        {
            // Aplicar un filtro que siga la cadena de relaciones hasta Alumno.
            // Si el Alumno de un AlumnoPuntos es "soft-deleted", entonces el historial
            // de esos puntos también se debe ocultar de las consultas.
            builder.HasQueryFilter(hp => !hp.AlumnoPuntos.Alumno.IsDeleted);

            // Nombre de tabla
            builder.ToTable("HistorialPuntos");

            // Clave primaria
            builder.HasKey(hp => hp.Id);

            // Propiedades requeridas
            builder.Property(hp => hp.AlumnoPuntosId)
                .IsRequired();

            builder.Property(hp => hp.Fecha)
                .IsRequired();

            builder.Property(hp => hp.PuntosObtenidos)
                .IsRequired();

            builder.Property(hp => hp.Categoria)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(hp => hp.Descripcion)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(hp => hp.OrigenId)
                .HasMaxLength(50);

            builder.Property(hp => hp.TipoOrigen)
                .HasMaxLength(50);

            builder.Property(hp => hp.PuntosAcumulados)
                .IsRequired();

            // Relaciones
            builder.HasOne(hp => hp.AlumnoPuntos)
                .WithMany(ap => ap.HistorialPuntos)
                .HasForeignKey(hp => hp.AlumnoPuntosId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Índices
            builder.HasIndex(hp => hp.AlumnoPuntosId)
                .HasDatabaseName("IX_HistorialPuntos_AlumnoPuntosId");

            builder.HasIndex(hp => hp.Fecha)
                .HasDatabaseName("IX_HistorialPuntos_Fecha");

            builder.HasIndex(hp => hp.Categoria)
                .HasDatabaseName("IX_HistorialPuntos_Categoria");

            builder.HasIndex(hp => hp.TipoOrigen)
                .HasDatabaseName("IX_HistorialPuntos_TipoOrigen");

            builder.HasIndex(hp => hp.PuntosObtenidos)
                .HasDatabaseName("IX_HistorialPuntos_PuntosObtenidos");

            // Índices compuestos para consultas frecuentes
            builder.HasIndex(hp => new { hp.AlumnoPuntosId, hp.Fecha })
                .HasDatabaseName("IX_HistorialPuntos_AlumnoPuntos_Fecha");

            builder.HasIndex(hp => new { hp.AlumnoPuntosId, hp.Categoria })
                .HasDatabaseName("IX_HistorialPuntos_AlumnoPuntos_Categoria");

            builder.HasIndex(hp => new { hp.Fecha, hp.Categoria })
                .HasDatabaseName("IX_HistorialPuntos_Fecha_Categoria");

            builder.HasIndex(hp => new { hp.TipoOrigen, hp.OrigenId })
                .HasDatabaseName("IX_HistorialPuntos_TipoOrigen_OrigenId");
        }
    }
}