using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Evaluacion;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Calificacion
    /// </summary>
    public class CalificacionConfiguration : IEntityTypeConfiguration<Calificacion>
    {
        /// <summary>
        /// Configura la entidad Calificacion para EF Core
        /// </summary>
        /// <param name="builder">Constructor de la entidad</param>
        public void Configure(EntityTypeBuilder<Calificacion> builder)
        {
            #region Tabla

            builder.ToTable("calificaciones");

            #endregion

            #region Clave Primaria

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            #endregion

            #region Foreign Keys

            // Escuela ID (Multi-tenant)
            builder.Property(c => c.EscuelaId)
                .HasColumnName("escuela_id")
                .IsRequired();

            // Alumno ID
            builder.Property(c => c.AlumnoId)
                .HasColumnName("alumno_id")
                .IsRequired();

            // Materia ID
            builder.Property(c => c.MateriaId)
                .HasColumnName("materia_id")
                .IsRequired();

            // Grupo ID
            builder.Property(c => c.GrupoId)
                .HasColumnName("grupo_id")
                .IsRequired();

            // Periodo ID
            builder.Property(c => c.PeriodoId)
                .HasColumnName("periodo_id")
                .IsRequired();

            #endregion

            #region Calificación

            // Calificación Numérica
            builder.Property(c => c.CalificacionNumerica)
                .HasColumnName("calificacion_numerica")
                .HasColumnType("DECIMAL(5,2)")
                .IsRequired();

            // Calificación Letra
            builder.Property(c => c.CalificacionLetra)
                .HasColumnName("calificacion_letra")
                .HasMaxLength(5);

            // Aprobado
            builder.Property(c => c.Aprobado)
                .HasColumnName("aprobado")
                .IsRequired();

            // Calificación Mínima
            builder.Property(c => c.CalificacionMinima)
                .HasColumnName("calificacion_minima")
                .HasColumnType("DECIMAL(5,2)");

            #endregion

            #region Detalles de la Evaluación

            // Tipo Evaluación
            builder.Property(c => c.TipoEvaluacion)
                .HasColumnName("tipo_evaluacion")
                .HasMaxLength(50);

            // Peso
            builder.Property(c => c.Peso)
                .HasColumnName("peso")
                .HasColumnType("DECIMAL(5,2)");

            // Observaciones
            builder.Property(c => c.Observaciones)
                .HasColumnName("observaciones")
                .HasColumnType("TEXT");

            // Fortalezas
            builder.Property(c => c.Fortalezas)
                .HasColumnName("fortalezas")
                .HasColumnType("TEXT");

            // Áreas Oportunidad
            builder.Property(c => c.AreasOportunidad)
                .HasColumnName("areas_oportunidad")
                .HasColumnType("TEXT");

            // Recomendaciones
            builder.Property(c => c.Recomendaciones)
                .HasColumnName("recomendaciones")
                .HasColumnType("TEXT");

            #endregion

            #region Control de Captura

            // Fecha Captura
            builder.Property(c => c.FechaCaptura)
                .HasColumnName("fecha_captura")
                                .IsRequired();

            // Capturado Por
            builder.Property(c => c.CapturadoPor)
                .HasColumnName("capturado_por");

            // Fue Modificada
            builder.Property(c => c.FueModificada)
                .HasColumnName("fue_modificada")
                .HasDefaultValue(false)
                .IsRequired();

            // Fecha Última Modificación
            builder.Property(c => c.FechaUltimaModificacion)
                .HasColumnName("fecha_ultima_modificacion")
                .HasColumnType("DATETIME");

            // Modificado Por
            builder.Property(c => c.ModificadoPor)
                .HasColumnName("modificado_por");

            // Motivo Modificación
            builder.Property(c => c.MotivoModificacion)
                .HasColumnName("motivo_modificacion")
                .HasColumnType("TEXT");

            #endregion

            #region Recalificación

            // Es Recalificación
            builder.Property(c => c.EsRecalificacion)
                .HasColumnName("es_recalificacion")
                .HasDefaultValue(false)
                .IsRequired();

            // Calificación Original
            builder.Property(c => c.CalificacionOriginal)
                .HasColumnName("calificacion_original")
                .HasColumnType("DECIMAL(5,2)");

            // Fecha Recalificación
            builder.Property(c => c.FechaRecalificacion)
                .HasColumnName("fecha_recalificacion")
                .HasColumnType("DATETIME");

            // Tipo Recalificación
            builder.Property(c => c.TipoRecalificacion)
                .HasColumnName("tipo_recalificacion")
                .HasMaxLength(50);

            #endregion

            #region Estado

            // Bloqueada
            builder.Property(c => c.Bloqueada)
                .HasColumnName("bloqueada")
                .HasDefaultValue(false)
                .IsRequired();

            // Fecha Bloqueo
            builder.Property(c => c.FechaBloqueo)
                .HasColumnName("fecha_bloqueo")
                .HasColumnType("DATETIME");

            // Visible Para Padres
            builder.Property(c => c.VisibleParaPadres)
                .HasColumnName("visible_para_padres")
                .HasDefaultValue(true)
                .IsRequired();

            #endregion

            #region Auditoría

            builder.Property(c => c.CreatedAt)
                .HasColumnName("created_at")
                                .IsRequired();

            builder.Property(c => c.UpdatedAt)
                .HasColumnName("updated_at")
                                .IsRequired();

            builder.Property(c => c.CreatedBy)
                .HasColumnName("created_by");

            builder.Property(c => c.UpdatedBy)
                .HasColumnName("updated_by");

            #endregion

            #region Propiedades Calculadas (Ignorar)

            builder.Ignore(c => c.EsReprobatoria);
            builder.Ignore(c => c.EsExcelente);
            builder.Ignore(c => c.PuedeSerModificada);

            #endregion

            #region Índices

            // Índice único compuesto: Un alumno solo puede tener una calificación por materia-período
            builder.HasIndex(c => new { c.AlumnoId, c.MateriaId, c.PeriodoId })
                .HasDatabaseName("idx_calificacion_alumno_materia_periodo")
                .IsUnique();

            // Índice compuesto en Grupo y Período (para consultas de maestros)
            builder.HasIndex(c => new { c.GrupoId, c.PeriodoId })
                .HasDatabaseName("idx_calificacion_grupo_periodo");

            // Índice en AlumnoId
            builder.HasIndex(c => c.AlumnoId)
                .HasDatabaseName("idx_calificacion_alumno");

            // Índice en Bloqueada (para filtrar calificaciones editables)
            builder.HasIndex(c => c.Bloqueada)
                .HasDatabaseName("idx_calificacion_bloqueada");

            // Índice en VisibleParaPadres
            builder.HasIndex(c => c.VisibleParaPadres)
                .HasDatabaseName("idx_calificacion_visible_padres");

            #endregion

            #region Relaciones

            // Relación con Escuela
            builder.HasOne(c => c.Escuela)
                .WithMany()
                .HasForeignKey(c => c.EscuelaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación con Alumno
            builder.HasOne(c => c.Alumno)
                .WithMany(a => a.Calificaciones)
                .HasForeignKey(c => c.AlumnoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con Materia
            builder.HasOne(c => c.Materia)
                .WithMany(m => m.Calificaciones)
                .HasForeignKey(c => c.MateriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con Grupo
            builder.HasOne(c => c.Grupo)
                .WithMany(g => g.Calificaciones)
                .HasForeignKey(c => c.GrupoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con Periodo
            builder.HasOne(c => c.Periodo)
                .WithMany(p => p.Calificaciones)
                .HasForeignKey(c => c.PeriodoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con Maestro que capturó (opcional)
            builder.HasOne(c => c.MaestroCaptura)
                .WithMany()
                .HasForeignKey(c => c.CapturadoPor)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region Validaciones a Nivel de BD (Constraints)

            // Constraint: La calificación debe estar entre0 y10
            builder.HasCheckConstraint(
                "CHK_Calificacion_Rango",
                "calificacion_numerica >=0 AND calificacion_numerica <=10"
            );

            // Constraint: El peso debe estar entre0 y100 si existe
            builder.HasCheckConstraint(
                "CHK_Peso_Rango",
                "peso IS NULL OR (peso >=0 AND peso <=100)"
            );

            #endregion
        }
    }
}