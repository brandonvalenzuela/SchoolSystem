using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Academico;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Inscripcion
    /// </summary>
    public class InscripcionConfiguration : IEntityTypeConfiguration<Inscripcion>
    {
        public void Configure(EntityTypeBuilder<Inscripcion> builder)
        {
            // Nombre de tabla
            builder.ToTable("Inscripciones");

            // Clave primaria
            builder.HasKey(i => i.Id);

            // Propiedades requeridas
            builder.Property(i => i.EscuelaId)
                .IsRequired();

            builder.Property(i => i.AlumnoId)
                .IsRequired();

            builder.Property(i => i.GrupoId)
                .IsRequired();

            builder.Property(x => x.CicloEscolarId)
                .IsRequired();

            builder.HasOne(x => x.Ciclo)
                .WithMany()
                .HasForeignKey(x => x.CicloEscolarId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(i => i.FechaInscripcion)
                .IsRequired();

            builder.Property(i => i.Estatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(SchoolSystem.Domain.Enums.Academico.EstatusInscripcion.Inscrito);

            // Fechas
            builder.Property(i => i.FechaInicio)
                .IsRequired(false);

            builder.Property(i => i.FechaFin)
                .IsRequired(false);

            builder.Property(i => i.FechaBaja)
                .IsRequired(false);

            builder.Property(i => i.FechaCambioGrupo)
                .IsRequired(false);

            // Calificaciones
            builder.Property(i => i.NumeroLista)
                .IsRequired(false);

            builder.Property(i => i.PromedioFinal)
                .IsRequired(false)
                .HasColumnType("decimal(5,2)");

            builder.Property(i => i.PromedioAcumulado)
                .IsRequired(false)
                .HasColumnType("decimal(5,2)");

            builder.Property(i => i.Aprobado)
                .IsRequired(false);

            builder.Property(i => i.MateriasReprobadas)
                .IsRequired(false);

            builder.Property(i => i.LugarEnGrupo)
                .IsRequired(false);

            // Asistencias
            builder.Property(i => i.TotalDiasClase)
                .IsRequired(false);

            builder.Property(i => i.DiasAsistidos)
                .IsRequired(false);

            builder.Property(i => i.DiasFaltados)
                .IsRequired(false);

            builder.Property(i => i.DiasRetardo)
                .IsRequired(false);

            builder.Property(i => i.PorcentajeAsistencia)
                .IsRequired(false)
                .HasColumnType("decimal(5,2)");

            // Bajas y cambios
            builder.Property(i => i.MotivoBaja)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(i => i.GrupoAnteriorId)
                .IsRequired(false);

            builder.Property(i => i.MotivoCambioGrupo)
                .HasMaxLength(500)
                .IsRequired(false);

            // Información adicional
            builder.Property(i => i.Observaciones)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(i => i.Becado)
                .HasDefaultValue(false);

            builder.Property(i => i.TipoBeca)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(i => i.PorcentajeBeca)
                .IsRequired(false)
                .HasColumnType("decimal(5,2)");

            builder.Property(i => i.Repetidor)
                .HasDefaultValue(false);

            // Auditoría
            builder.Property(i => i.CreatedAt)
                .IsRequired();

            builder.Property(i => i.UpdatedAt)
                .IsRequired();

            builder.Property(i => i.CreatedBy)
                .IsRequired(false);

            builder.Property(i => i.UpdatedBy)
                .IsRequired(false);

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

            // Relaciones
            builder.HasOne(i => i.Escuela)
                .WithMany()
                .HasForeignKey(i => i.EscuelaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.Alumno)
                .WithMany(a => a.Inscripciones)
                .HasForeignKey(i => i.AlumnoId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(i => i.Grupo)
                .WithMany(g => g.Inscripciones)
                .HasForeignKey(i => i.GrupoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(i => i.EscuelaId)
                .HasDatabaseName("IX_Inscripciones_EscuelaId");

            builder.HasIndex(i => i.AlumnoId)
                .HasDatabaseName("IX_Inscripciones_AlumnoId");

            builder.HasIndex(i => i.GrupoId)
                .HasDatabaseName("IX_Inscripciones_GrupoId");

            builder.HasIndex(i => i.Estatus)
                .HasDatabaseName("IX_Inscripciones_Estatus");

            builder.HasIndex(i => i.FechaInscripcion)
                .HasDatabaseName("IX_Inscripciones_FechaInscripcion");

            builder.HasIndex(i => i.Becado)
                .HasDatabaseName("IX_Inscripciones_Becado");

            builder.HasIndex(x => new { x.EscuelaId, x.CicloEscolarId })
                .HasDatabaseName("IX_Inscripciones_Escuela_CicloEscolarId");

            // Índice único compuesto: Un alumno no puede inscribirse dos veces en el mismo grupo en el mismo ciclo
            builder.HasIndex(i => new { i.AlumnoId, i.GrupoId, i.CicloEscolarId })
                .IsUnique()
                .HasDatabaseName("IX_Inscripciones_Alumno_Grupo_Ciclo_Unique");

            builder.HasIndex(i => new { i.EscuelaId, i.CicloEscolarId, i.Estatus })
                .HasDatabaseName("IX_Inscripciones_Escuela_Ciclo_Estatus");

            builder.HasIndex(i => new { i.AlumnoId, i.CicloEscolarId })
                .HasDatabaseName("IX_Inscripciones_Alumno_Ciclo");

            // Índice para optimizar búsquedas en flujo de recalcular promedios
            // Performance: filtro por grupo, ciclo escolar, estatus y alumno
            builder.HasIndex(i => new { i.GrupoId, i.CicloEscolarId, i.Estatus, i.AlumnoId })
                .HasDatabaseName("IX_Inscripciones_Grupo_Ciclo_Estatus_Alumno");

            builder.HasIndex(i => new { i.GrupoId, i.CicloEscolarId, i.Estatus })
                .HasDatabaseName("IX_Inscripciones_Grupo_Ciclo_Estatus");

            builder.HasIndex(i => new { i.GrupoId, i.NumeroLista })
                .HasDatabaseName("IX_Inscripciones_Grupo_NumeroLista");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(i => i.EstaActiva);
            builder.Ignore(i => i.CicloFinalizado);
            builder.Ignore(i => i.EstaDeBaja);

            // Constraints
            builder.HasCheckConstraint("CK_Inscripciones_NumeroLista",
                "`NumeroLista` IS NULL OR `NumeroLista` > 0");

            builder.HasCheckConstraint("CK_Inscripciones_PromedioFinal",
                "`PromedioFinal` IS NULL OR (`PromedioFinal` >= 0 AND `PromedioFinal` <= 10)");

            builder.HasCheckConstraint("CK_Inscripciones_PromedioAcumulado",
                "`PromedioAcumulado` IS NULL OR (`PromedioAcumulado` >= 0 AND `PromedioAcumulado` <= 10)");

            builder.HasCheckConstraint("CK_Inscripciones_MateriasReprobadas",
                "`MateriasReprobadas` IS NULL OR `MateriasReprobadas` >= 0");

            builder.HasCheckConstraint("CK_Inscripciones_Asistencias",
                "(`TotalDiasClase` IS NULL OR `TotalDiasClase` >= 0) AND " +
                "(`DiasAsistidos` IS NULL OR `DiasAsistidos` >= 0) AND " +
                "(`DiasFaltados` IS NULL OR `DiasFaltados` >= 0) AND " +
                "(`DiasRetardo` IS NULL OR `DiasRetardo` >= 0)");

            builder.HasCheckConstraint("CK_Inscripciones_PorcentajeAsistencia",
                "`PorcentajeAsistencia` IS NULL OR (`PorcentajeAsistencia` >= 0 AND `PorcentajeAsistencia` <= 100)");

            builder.HasCheckConstraint("CK_Inscripciones_PorcentajeBeca",
                "`PorcentajeBeca` IS NULL OR (`PorcentajeBeca` >= 0 AND `PorcentajeBeca` <= 100)");

            builder.HasCheckConstraint("CK_Inscripciones_Fechas",
                "`FechaInicio` IS NULL OR `FechaFin` IS NULL OR `FechaInicio` <= `FechaFin`");

            builder.HasCheckConstraint("CK_Inscripciones_LugarEnGrupo",
                "`LugarEnGrupo` IS NULL OR `LugarEnGrupo` > 0");
        }
    }
}