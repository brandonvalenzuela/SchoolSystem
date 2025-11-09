using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Comunicacion;
using SchoolSystem.Domain.Enums.Comunicacion;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Comunicado
    /// </summary>
    public class ComunicadoConfiguration : IEntityTypeConfiguration<Comunicado>
    {
        public void Configure(EntityTypeBuilder<Comunicado> builder)
        {
            // Nombre de tabla
            builder.ToTable("Comunicados");

            // Clave primaria
            builder.HasKey(c => c.Id);

            // Propiedades requeridas
            builder.Property(c => c.EscuelaId)
                .IsRequired();

            builder.Property(c => c.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Contenido)
                .IsRequired()
                .HasColumnType("LONGTEXT");

            builder.Property(c => c.Destinatarios)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(c => c.GrupoId)
                .IsRequired(false);

            // Archivos adjuntos
            builder.Property(c => c.ArchivoAdjuntoUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(c => c.ArchivoAdjuntoNombre)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(c => c.ArchivoAdjuntoTamano)
                .IsRequired(false);

            builder.Property(c => c.ArchivoAdjuntoTipo)
                .HasMaxLength(100)
                .IsRequired(false);

            // Publicación
            builder.Property(c => c.PublicadoPorId)
                .IsRequired();

            builder.Property(c => c.FechaPublicacion)
                .IsRequired();

            builder.Property(c => c.FechaExpiracion)
                .IsRequired(false);

            // Configuración
            builder.Property(c => c.RequiereConfirmacion)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(c => c.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(c => c.Prioridad)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(10)
                .HasDefaultValue(PrioridadNotificacion.Normal);

            builder.Property(c => c.Categoria)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(c => c.PermiteComentarios)
                .HasDefaultValue(false);

            // Estadísticas
            builder.Property(c => c.TotalDestinatarios)
                .HasDefaultValue(0);

            builder.Property(c => c.TotalLecturas)
                .HasDefaultValue(0);

            builder.Property(c => c.TotalConfirmaciones)
                .HasDefaultValue(0);

            // Auditoría
            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.Property(c => c.CreatedBy)
                .IsRequired(false);

            builder.Property(c => c.UpdatedAt)
                .IsRequired();

            builder.Property(c => c.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(c => c.Grupo)
                .WithMany()
                .HasForeignKey(c => c.GrupoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.PublicadoPor)
                .WithMany()
                .HasForeignKey(c => c.PublicadoPorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Lecturas)
                .WithOne(l => l.Comunicado)
                .HasForeignKey(l => l.ComunicadoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices
            builder.HasIndex(c => c.EscuelaId)
                .HasDatabaseName("IX_Comunicados_EscuelaId");

            builder.HasIndex(c => c.Destinatarios)
                .HasDatabaseName("IX_Comunicados_Destinatarios");

            builder.HasIndex(c => c.FechaPublicacion)
                .HasDatabaseName("IX_Comunicados_FechaPublicacion");

            builder.HasIndex(c => c.Activo)
                .HasDatabaseName("IX_Comunicados_Activo");

            builder.HasIndex(c => new { c.EscuelaId, c.Activo, c.FechaPublicacion })
                .HasDatabaseName("IX_Comunicados_Escuela_Activo_Fecha");

            builder.HasIndex(c => c.GrupoId)
                .HasDatabaseName("IX_Comunicados_GrupoId");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(c => c.EstaExpirado);
            builder.Ignore(c => c.EsParaTodos);
            builder.Ignore(c => c.EsParaGrupoEspecifico);
            builder.Ignore(c => c.PorcentajeLecturas);
            builder.Ignore(c => c.PorcentajeConfirmaciones);
            builder.Ignore(c => c.TieneArchivo);
            builder.Ignore(c => c.TiempoDesdePublicacion);
            builder.Ignore(c => c.DiasDesdePublicacion);
            builder.Ignore(c => c.EsUrgente);
        }
    }
}