using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Conducta;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad AlumnoInsignia
    /// </summary>
    public class AlumnoInsigniaConfiguration : IEntityTypeConfiguration<AlumnoInsignia>
    {
        public void Configure(EntityTypeBuilder<AlumnoInsignia> builder)
        {
            // Nombre de tabla
            builder.ToTable("AlumnosInsignias");

            // Clave primaria
            builder.HasKey(ai => ai.Id);

            // Propiedades requeridas
            builder.Property(ai => ai.AlumnoPuntosId)
                .IsRequired();

            builder.Property(ai => ai.InsigniaId)
                .IsRequired();

            builder.Property(ai => ai.FechaObtencion)
                .IsRequired();

            builder.Property(ai => ai.Motivo)
                .HasMaxLength(500);

            builder.Property(ai => ai.EsFavorita)
                .HasDefaultValue(false);

            builder.Property(ai => ai.VecesObtenida)
                .IsRequired()
                .HasDefaultValue(1);

            // Relaciones
            builder.HasOne(ai => ai.AlumnoPuntos)
                .WithMany(ap => ap.InsigniasGanadas)
                .HasForeignKey(ai => ai.AlumnoPuntosId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ai => ai.Insignia)
                .WithMany(i => i.AlumnosQueGanaron)
                .HasForeignKey(ai => ai.InsigniaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices
            builder.HasIndex(ai => ai.AlumnoPuntosId)
                .HasDatabaseName("IX_AlumnosInsignias_AlumnoPuntosId");

            builder.HasIndex(ai => ai.InsigniaId)
                .HasDatabaseName("IX_AlumnosInsignias_InsigniaId");

            builder.HasIndex(ai => ai.FechaObtencion)
                .HasDatabaseName("IX_AlumnosInsignias_FechaObtencion");

            builder.HasIndex(ai => ai.EsFavorita)
                .HasDatabaseName("IX_AlumnosInsignias_EsFavorita");

            // Índices compuestos para consultas frecuentes
            builder.HasIndex(ai => new { ai.AlumnoPuntosId, ai.InsigniaId })
                .HasDatabaseName("IX_AlumnosInsignias_AlumnoPuntos_Insignia");

            builder.HasIndex(ai => new { ai.AlumnoPuntosId, ai.FechaObtencion })
                .HasDatabaseName("IX_AlumnosInsignias_AlumnoPuntos_Fecha");

            builder.HasIndex(ai => new { ai.InsigniaId, ai.FechaObtencion })
                .HasDatabaseName("IX_AlumnosInsignias_Insignia_Fecha");

            builder.HasIndex(ai => new { ai.AlumnoPuntosId, ai.EsFavorita })
                .HasDatabaseName("IX_AlumnosInsignias_AlumnoPuntos_Favorita");

            // Constraints
            builder.HasCheckConstraint("CK_AlumnosInsignias_VecesObtenida",
                "`VecesObtenida` >= 1");

            builder.HasCheckConstraint("CK_AlumnosInsignias_FechaObtencion",
                "`FechaObtencion` <= GETDATE()");
        }
    }
}