using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Tareas;
using SchoolSystem.Domain.Enums.Academico;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Tarea
    /// </summary>
    public class TareaConfiguration : IEntityTypeConfiguration<Tarea>
    {
        public void Configure(EntityTypeBuilder<Tarea> builder)
        {
            // Nombre de tabla
            builder.ToTable("Tareas");

            // Clave primaria
            builder.HasKey(t => t.Id);

            // Propiedades requeridas
            builder.Property(t => t.EscuelaId)
                .IsRequired();

            builder.Property(t => t.GrupoId)
                .IsRequired();

            builder.Property(t => t.MateriaId)
                .IsRequired();

            builder.Property(t => t.MaestroId)
                .IsRequired();

            builder.Property(t => t.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Descripcion)
                .IsRequired()
                .HasColumnType("LONGTEXT");

            builder.Property(t => t.FechaAsignacion)
                .IsRequired();

            builder.Property(t => t.FechaEntrega)
                .IsRequired();

            builder.Property(t => t.FechaLimiteTardia)
                .IsRequired(false);

            // Tipo de tarea (Enum)
            builder.Property(t => t.Tipo)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            // Campos decimales
            builder.Property(t => t.ValorPuntos)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            builder.Property(t => t.PenalizacionTardia)
                .IsRequired(false)
                .HasColumnType("decimal(5,2)");

            // Archivos adjuntos
            builder.Property(t => t.ArchivoAdjuntoUrl)
                .HasMaxLength(500);

            builder.Property(t => t.ArchivoAdjuntoNombre)
                .HasMaxLength(200);

            builder.Property(t => t.ArchivoAdjuntoTamano)
                .IsRequired(false);

            // Estado
            builder.Property(t => t.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(t => t.Observaciones)
                .HasColumnType("LONGTEXT");

            // Auditoría
            builder.Property(t => t.CreatedAt)
                .IsRequired();

            builder.Property(t => t.CreatedBy)
                .IsRequired(false);

            builder.Property(t => t.UpdatedAt)
                .IsRequired();

            builder.Property(t => t.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(t => t.Grupo)
                .WithMany()
                .HasForeignKey(t => t.GrupoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Materia)
                .WithMany()
                .HasForeignKey(t => t.MateriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Maestro)
                .WithMany()
                .HasForeignKey(t => t.MaestroId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.Entregas)
                .WithOne(e => e.Tarea)
                .HasForeignKey(e => e.TareaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices
            builder.HasIndex(t => t.EscuelaId)
                .HasDatabaseName("IX_Tareas_EscuelaId");

            builder.HasIndex(t => new { t.GrupoId, t.MateriaId })
                .HasDatabaseName("IX_Tareas_Grupo_Materia");

            builder.HasIndex(t => t.MaestroId)
                .HasDatabaseName("IX_Tareas_MaestroId");

            builder.HasIndex(t => t.FechaEntrega)
                .HasDatabaseName("IX_Tareas_FechaEntrega");

            builder.HasIndex(t => t.Activo)
                .HasDatabaseName("IX_Tareas_Activo");

            builder.HasIndex(t => new { t.GrupoId, t.FechaEntrega, t.Activo })
                .HasDatabaseName("IX_Tareas_Grupo_Fecha_Activo");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(t => t.EstaVencida);
            builder.Ignore(t => t.EnPeriodoTardio);
            builder.Ignore(t => t.FueraDeTiempo);
            builder.Ignore(t => t.DiasParaEntrega);
            builder.Ignore(t => t.HorasParaEntrega);
            builder.Ignore(t => t.TotalEntregas);

            // Constraints
            builder.HasCheckConstraint("CK_Tareas_ValorPuntos", "(`ValorPuntos` > 0)");
            builder.HasCheckConstraint("CK_Tareas_Penalizacion", "(`PenalizacionTardia` IS NULL OR (`PenalizacionTardia` >= 0 AND `PenalizacionTardia` <= 100))");
            builder.HasCheckConstraint("CK_Tareas_Fechas", "(`FechaEntrega` > `FechaAsignacion`)");
        }
    }
}