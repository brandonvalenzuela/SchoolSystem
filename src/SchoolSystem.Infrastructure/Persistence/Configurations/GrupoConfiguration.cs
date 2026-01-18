using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Academico;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Grupo
    /// </summary>
    public class GrupoConfiguration : IEntityTypeConfiguration<Grupo>
    {
        public void Configure(EntityTypeBuilder<Grupo> builder)
        {
            // Nombre de tabla
            builder.ToTable("Grupos");

            // Clave primaria
            builder.HasKey(g => g.Id);

            // Propiedades requeridas
            builder.Property(g => g.EscuelaId)
                .IsRequired();

            builder.Property(g => g.GradoId)
                .IsRequired();

            builder.Property(g => g.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(g => g.CicloEscolar)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.CicloEscolarId)
                .IsRequired(false);

            builder.HasOne(x => x.Ciclo)
                .WithMany()
                .HasForeignKey(x => x.CicloEscolarId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(g => g.Descripcion)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(g => g.CapacidadMaxima)
                .IsRequired()
                .HasDefaultValue(30);

            builder.Property(g => g.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            // Propiedades opcionales
            builder.Property(g => g.MaestroTitularId)
                .IsRequired(false);

            builder.Property(g => g.Aula)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(g => g.Turno)
                .IsRequired(false)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(g => g.HoraInicio)
                .IsRequired(false);

            builder.Property(g => g.HoraFin)
                .IsRequired(false);

            builder.Property(g => g.DiasClase)
                .IsRequired() // Mantenlo como requerido
                .HasMaxLength(100) // Asegúrate de que tenga suficiente longitud
                .HasDefaultValue("Lunes a Viernes"); // <-- AÑADE ESTA LÍNEA con el valor que desees

            // Auditoría
            builder.Property(g => g.CreatedAt)
                .IsRequired();

            builder.Property(g => g.UpdatedAt)
                .IsRequired();

            builder.Property(g => g.CreatedBy)
                .IsRequired(false);

            builder.Property(g => g.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(g => g.Escuela)
                .WithMany(e => e.Grupos)
                .HasForeignKey(g => g.EscuelaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(g => g.Grado)
                .WithMany(gr => gr.Grupos)
                .HasForeignKey(g => g.GradoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(g => g.MaestroTitular)
                .WithMany()
                .HasForeignKey(g => g.MaestroTitularId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(g => g.Inscripciones)
                .WithOne(i => i.Grupo)
                .HasForeignKey(i => i.GrupoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(g => g.GrupoMateriaMaestros)
                .WithOne(am => am.Grupo)
                .HasForeignKey(am => am.GrupoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(g => g.Asistencias)
                .WithOne(a => a.Grupo)
                .HasForeignKey(a => a.GrupoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(g => g.Calificaciones)
                .WithOne(c => c.Grupo)
                .HasForeignKey(c => c.GrupoId)
                .OnDelete(DeleteBehavior.Restrict);

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

            // Índices
            builder.HasIndex(g => g.EscuelaId)
                .HasDatabaseName("IX_Grupos_EscuelaId");

            builder.HasIndex(g => g.GradoId)
                .HasDatabaseName("IX_Grupos_GradoId");

            builder.HasIndex(g => g.CicloEscolar)
                .HasDatabaseName("IX_Grupos_CicloEscolar");

            builder.HasIndex(g => g.Activo)
                .HasDatabaseName("IX_Grupos_Activo");

            builder.HasIndex(g => g.MaestroTitularId)
                .HasDatabaseName("IX_Grupos_MaestroTitularId");

            builder.HasIndex(g => g.Turno)
                .HasDatabaseName("IX_Grupos_Turno");

            builder.HasIndex(x => new { x.EscuelaId, x.CicloEscolarId })
                .HasDatabaseName("IX_Grupos_Escuela_CicloEscolarId");

            // Índice único compuesto: No puede haber dos grupos con el mismo nombre en el mismo grado y ciclo
            builder.HasIndex(g => new { g.EscuelaId, g.GradoId, g.Nombre, g.CicloEscolar })
                .IsUnique()
                .HasDatabaseName("IX_Grupos_Escuela_Grado_Nombre_Ciclo_Unique");

            builder.HasIndex(g => new { g.EscuelaId, g.CicloEscolar, g.Activo })
                .HasDatabaseName("IX_Grupos_Escuela_Ciclo_Activo");

            builder.HasIndex(g => new { g.GradoId, g.CicloEscolar, g.Activo })
                .HasDatabaseName("IX_Grupos_Grado_Ciclo_Activo");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(g => g.NombreCompleto);
            builder.Ignore(g => g.CantidadAlumnos);
            builder.Ignore(g => g.EspaciosDisponibles);
            builder.Ignore(g => g.PorcentajeOcupacion);
            builder.Ignore(g => g.CantidadMaterias);

            // Constraints
            builder.HasCheckConstraint("CK_Grupos_CapacidadMaxima",
                "`CapacidadMaxima` > 0");

            builder.HasCheckConstraint("CK_Grupos_Horarios",
                "`HoraInicio` IS NULL OR `HoraFin` IS NULL OR `HoraInicio` < `HoraFin`");
        }
    }
}