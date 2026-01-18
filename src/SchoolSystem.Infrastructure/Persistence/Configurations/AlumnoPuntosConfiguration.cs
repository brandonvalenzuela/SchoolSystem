using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Conducta;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad AlumnoPuntos
    /// </summary>
    public class AlumnoPuntosConfiguration : IEntityTypeConfiguration<AlumnoPuntos>
    {
        public void Configure(EntityTypeBuilder<AlumnoPuntos> builder)
        {
            // Aplicar un filtro que coincida con el filtro de Alumno.
            // Esto asegura que si un Alumno es "soft-deleted", su registro de puntos
            // también se oculte automáticamente de todas las consultas.
            builder.HasQueryFilter(ap => !ap.Alumno.IsDeleted);

            // Nombre de tabla
            builder.ToTable("AlumnoPuntos");

            // Clave primaria
            builder.HasKey(ap => ap.Id);

            // Propiedades requeridas
            builder.Property(ap => ap.EscuelaId)
                .IsRequired();

            builder.Property(ap => ap.AlumnoId)
                .IsRequired();

            builder.Property(ap => ap.PeriodoEscolarId)
                .IsRequired(false);

            builder.Property(ap => ap.CicloEscolar)
                .HasMaxLength(10);

            builder.Property(x => x.CicloEscolarId)
                .IsRequired(false);

            builder.HasOne(x => x.Ciclo)
                .WithMany()
                .HasForeignKey(x => x.CicloEscolarId)
                .OnDelete(DeleteBehavior.Restrict);

            // Puntos
            builder.Property(ap => ap.PuntosTotales)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(ap => ap.PuntosPeriodoActual)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(ap => ap.PuntosAcademicos)
                .HasDefaultValue(0);

            builder.Property(ap => ap.PuntosConducta)
                .HasDefaultValue(0);

            builder.Property(ap => ap.PuntosDeportivos)
                .HasDefaultValue(0);

            builder.Property(ap => ap.PuntosCulturales)
                .HasDefaultValue(0);

            builder.Property(ap => ap.PuntosSociales)
                .HasDefaultValue(0);

            builder.Property(ap => ap.PuntosAsistencia)
                .HasDefaultValue(0);

            builder.Property(ap => ap.PuntosExtra)
                .HasDefaultValue(0);

            // Sistema de niveles
            builder.Property(ap => ap.NivelActual)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(ap => ap.ExperienciaActual)
                .HasDefaultValue(0);

            builder.Property(ap => ap.ExperienciaSiguienteNivel)
                .HasDefaultValue(100);

            builder.Property(ap => ap.TituloNivel)
                .HasMaxLength(50)
                .HasDefaultValue("Principiante");

            builder.Property(ap => ap.ColorNivel)
                .HasMaxLength(20)
                .HasDefaultValue("#4CAF50");

            // Rankings
            builder.Property(ap => ap.RankingGrupo)
                .IsRequired(false);

            builder.Property(ap => ap.TotalAlumnosGrupo)
                .IsRequired(false);

            builder.Property(ap => ap.RankingGrado)
                .IsRequired(false);

            builder.Property(ap => ap.TotalAlumnosGrado)
                .IsRequired(false);

            builder.Property(ap => ap.RankingEscuela)
                .IsRequired(false);

            builder.Property(ap => ap.TotalAlumnosEscuela)
                .IsRequired(false);

            builder.Property(ap => ap.CambioRanking)
                .HasDefaultValue(0);

            // Rachas
            builder.Property(ap => ap.RachaAsistencia)
                .HasDefaultValue(0);

            builder.Property(ap => ap.RachaBuenaConducta)
                .HasDefaultValue(0);

            builder.Property(ap => ap.RachaTareas)
                .HasDefaultValue(0);

            builder.Property(ap => ap.MejorRachaAsistencia)
                .HasDefaultValue(0);

            builder.Property(ap => ap.MejorRachaConducta)
                .HasDefaultValue(0);

            builder.Property(ap => ap.MejorRachaTareas)
                .HasDefaultValue(0);

            // Estadísticas
            builder.Property(ap => ap.PromedioPuntosDiario)
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0);

            builder.Property(ap => ap.PromedioPuntosSemanal)
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0);

            builder.Property(ap => ap.Tendencia)
                .HasMaxLength(20)
                .HasDefaultValue("Estable");

            builder.Property(ap => ap.PorcentajeMejora)
                .HasColumnType("decimal(5,2)")
                .HasDefaultValue(0);

            // Fechas
            builder.Property(ap => ap.UltimaActualizacionPuntos)
                .IsRequired(false);

            builder.Property(ap => ap.FechaUltimoNivel)
                .IsRequired(false);

            builder.Property(ap => ap.FechaUltimoLogro)
                .IsRequired(false);

            builder.Property(ap => ap.FechaReinicioPeriodo)
                .IsRequired(false);

            // Configuración
            builder.Property(ap => ap.NotificacionesActivas)
                .HasDefaultValue(true);

            builder.Property(ap => ap.MostrarEnRankings)
                .HasDefaultValue(true);

            builder.Property(ap => ap.AvatarUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(ap => ap.Lema)
                .HasMaxLength(100)
                .IsRequired(false);

            // Auditoría
            builder.Property(ap => ap.CreatedAt)
                .IsRequired();

            builder.Property(ap => ap.UpdatedAt)
                .IsRequired();

            builder.Property(ap => ap.CreatedBy)
                .IsRequired(false);

            builder.Property(ap => ap.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(ap => ap.Alumno)
                .WithMany(a => a.Puntos) // Asumiendo que Alumno tiene una ICollection<AlumnoPuntos> llamada "Puntos"
                .HasForeignKey(ap => ap.AlumnoId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); // Comportamiento lógico: si borras al alumno, borras sus puntos.

            builder.HasMany(ap => ap.HistorialPuntos)
                .WithOne()
                .HasForeignKey("AlumnoPuntosId")
                .OnDelete(DeleteBehavior.Cascade);

            // Índices
            builder.HasIndex(ap => ap.EscuelaId)
                .HasDatabaseName("IX_AlumnoPuntos_EscuelaId");

            builder.HasIndex(ap => ap.AlumnoId)
                .HasDatabaseName("IX_AlumnoPuntos_AlumnoId");

            builder.HasIndex(ap => ap.PeriodoEscolarId)
                .HasDatabaseName("IX_AlumnoPuntos_PeriodoEscolarId");

            builder.HasIndex(ap => ap.CicloEscolar)
                .HasDatabaseName("IX_AlumnoPuntos_CicloEscolar");

            builder.HasIndex(ap => ap.PuntosTotales)
                .HasDatabaseName("IX_AlumnoPuntos_PuntosTotales");

            builder.HasIndex(ap => ap.NivelActual)
                .HasDatabaseName("IX_AlumnoPuntos_NivelActual");

            builder.HasIndex(ap => ap.RankingGrupo)
                .HasDatabaseName("IX_AlumnoPuntos_RankingGrupo");

            builder.HasIndex(ap => ap.RankingGrado)
                .HasDatabaseName("IX_AlumnoPuntos_RankingGrado");

            builder.HasIndex(ap => ap.RankingEscuela)
                .HasDatabaseName("IX_AlumnoPuntos_RankingEscuela");

            builder.HasIndex(x => new { x.EscuelaId, x.CicloEscolarId })
                .HasDatabaseName("IX_AlumnoPuntos_Escuela_CicloEscolarId");

            // Índice único: Un alumno solo puede tener un registro de puntos por ciclo escolar
            builder.HasIndex(ap => new { ap.AlumnoId, ap.CicloEscolar })
                .IsUnique()
                .HasDatabaseName("IX_AlumnoPuntos_Alumno_Ciclo_Unique");

            builder.HasIndex(ap => new { ap.EscuelaId, ap.CicloEscolar, ap.PuntosTotales })
                .HasDatabaseName("IX_AlumnoPuntos_Escuela_Ciclo_Puntos");

            builder.HasIndex(ap => new { ap.EscuelaId, ap.RankingEscuela })
                .HasDatabaseName("IX_AlumnoPuntos_Escuela_Ranking");

            builder.HasIndex(ap => new { ap.NivelActual, ap.PuntosTotales })
                .HasDatabaseName("IX_AlumnoPuntos_Nivel_Puntos");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(ap => ap.PorcentajeProgresoNivel);
            builder.Ignore(ap => ap.PercentilGrupo);
            builder.Ignore(ap => ap.EsTop10Grupo);
            builder.Ignore(ap => ap.EsTop3Grupo);
            builder.Ignore(ap => ap.TotalInsignias);

            // Constraints
            builder.HasCheckConstraint("CK_AlumnoPuntos_PuntosTotales",
                "`PuntosTotales` >= 0");

            builder.HasCheckConstraint("CK_AlumnoPuntos_NivelActual",
                "`NivelActual` >= 1");

            builder.HasCheckConstraint("CK_AlumnoPuntos_ExperienciaActual",
                "`ExperienciaActual` >= 0");

            builder.HasCheckConstraint("CK_AlumnoPuntos_Rankings",
                "(`RankingGrupo` IS NULL OR `RankingGrupo` > 0) AND " +
                "(`RankingGrado` IS NULL OR `RankingGrado` > 0) AND " +
                "(`RankingEscuela` IS NULL OR `RankingEscuela` > 0)");

            builder.HasCheckConstraint("CK_AlumnoPuntos_Rachas",
                "`RachaAsistencia` >= 0 AND `RachaBuenaConducta` >= 0 AND `RachaTareas` >= 0");

            builder.HasCheckConstraint("CK_AlumnoPuntos_Promedios",
                "`PromedioPuntosDiario` >= 0 AND `PromedioPuntosSemanal` >= 0");
        }
    }
}