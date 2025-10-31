using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Evaluacion;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad PeriodoEvaluacion
    /// </summary>
    public class PeriodoEvaluacionConfiguration : IEntityTypeConfiguration<PeriodoEvaluacion>
    {
        public void Configure(EntityTypeBuilder<PeriodoEvaluacion> builder)
        {
            // Nombre de tabla
            builder.ToTable("PeriodosEvaluacion");

            // Clave primaria
            builder.HasKey(pe => pe.Id);

            // Propiedades requeridas
            builder.Property(pe => pe.EscuelaId)
                .IsRequired();

            builder.Property(pe => pe.CicloEscolar)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(pe => pe.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(pe => pe.Numero)
                .IsRequired();

            builder.Property(pe => pe.Descripcion)
                .HasMaxLength(500);

            builder.Property(pe => pe.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            // Fechas
            builder.Property(pe => pe.FechaInicio)
                .IsRequired()
                .HasColumnType("DATE");

            builder.Property(pe => pe.FechaFin)
                .IsRequired()
                .HasColumnType("DATE");

            builder.Property(pe => pe.FechaLimiteCaptura)
                .IsRequired(false)
                .HasColumnType("DATE");

            builder.Property(pe => pe.FechaPublicacion)
                .IsRequired(false)
                .HasColumnType("DATE");

            // Configuración de evaluación
            builder.Property(pe => pe.Porcentaje)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            builder.Property(pe => pe.TipoPeriodo)
                .HasMaxLength(50);

            builder.Property(pe => pe.CalificacionesDefinitivas)
                .HasDefaultValue(false);

            builder.Property(pe => pe.PermiteRecalificacion)
                .HasDefaultValue(false);

            builder.Property(pe => pe.FechaLimiteRecalificacion)
                .IsRequired(false)
                .HasColumnType("DATE");

            // Auditoría
            builder.Property(pe => pe.CreatedAt)
                .IsRequired();

            builder.Property(pe => pe.UpdatedAt)
                .IsRequired();

            builder.Property(pe => pe.CreatedBy)
                .IsRequired(false);

            builder.Property(pe => pe.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(pe => pe.Escuela)
                .WithMany()
                .HasForeignKey(pe => pe.EscuelaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(pe => pe.Calificaciones)
                .WithOne(c => c.Periodo)
                .HasForeignKey(c => c.PeriodoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(pe => pe.EscuelaId)
                .HasDatabaseName("IX_PeriodosEvaluacion_EscuelaId");

            builder.HasIndex(pe => pe.CicloEscolar)
                .HasDatabaseName("IX_PeriodosEvaluacion_CicloEscolar");

            builder.HasIndex(pe => pe.Activo)
                .HasDatabaseName("IX_PeriodosEvaluacion_Activo");

            builder.HasIndex(pe => pe.FechaInicio)
                .HasDatabaseName("IX_PeriodosEvaluacion_FechaInicio");

            builder.HasIndex(pe => pe.FechaFin)
                .HasDatabaseName("IX_PeriodosEvaluacion_FechaFin");

            builder.HasIndex(pe => pe.CalificacionesDefinitivas)
                .HasDatabaseName("IX_PeriodosEvaluacion_CalificacionesDefinitivas");

            // Índice único compuesto: No puede haber dos períodos con el mismo número en el mismo ciclo escolar
            builder.HasIndex(pe => new { pe.EscuelaId, pe.CicloEscolar, pe.Numero })
                .IsUnique()
                .HasDatabaseName("IX_PeriodosEvaluacion_Escuela_Ciclo_Numero_Unique");

            builder.HasIndex(pe => new { pe.EscuelaId, pe.CicloEscolar, pe.Activo })
                .HasDatabaseName("IX_PeriodosEvaluacion_Escuela_Ciclo_Activo");

            builder.HasIndex(pe => new { pe.EscuelaId, pe.FechaInicio, pe.FechaFin })
                .HasDatabaseName("IX_PeriodosEvaluacion_Escuela_Fechas");

            builder.HasIndex(pe => new { pe.CicloEscolar, pe.Numero })
                .HasDatabaseName("IX_PeriodosEvaluacion_Ciclo_Numero");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(pe => pe.NombreCompleto);
            builder.Ignore(pe => pe.DuracionDias);
            builder.Ignore(pe => pe.EstaEnCurso);
            builder.Ignore(pe => pe.HaFinalizado);
            builder.Ignore(pe => pe.NoHaIniciado);
            builder.Ignore(pe => pe.DentroDePlazoCap);
            builder.Ignore(pe => pe.DiasRestantes);
            builder.Ignore(pe => pe.PorcentajeAvance);

            // Constraints
            builder.HasCheckConstraint("CK_PeriodosEvaluacion_Numero",
                "`Numero` > 0");

            builder.HasCheckConstraint("CK_PeriodosEvaluacion_Fechas",
                "`FechaInicio` < `FechaFin`");

            builder.HasCheckConstraint("CK_PeriodosEvaluacion_Porcentaje",
                "`Porcentaje` >= 0 AND `Porcentaje` <= 100");

            builder.HasCheckConstraint("CK_PeriodosEvaluacion_FechaLimiteCaptura",
                "`FechaLimiteCaptura` IS NULL OR `FechaLimiteCaptura` >= `FechaFin`");

            builder.HasCheckConstraint("CK_PeriodosEvaluacion_FechaPublicacion",
                "`FechaPublicacion` IS NULL OR `FechaPublicacion` >= `FechaFin`");
        }
    }
}