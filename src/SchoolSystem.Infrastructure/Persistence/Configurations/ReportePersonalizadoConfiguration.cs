using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Documentos;
using SchoolSystem.Domain.Enums.Documentos;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad ReportePersonalizado
    /// </summary>
    public class ReportePersonalizadoConfiguration : IEntityTypeConfiguration<ReportePersonalizado>
    {
        public void Configure(EntityTypeBuilder<ReportePersonalizado> builder)
        {
            // Nombre de tabla
            builder.ToTable("ReportesPersonalizados");

            // Clave primaria
            builder.HasKey(rp => rp.Id);

            // Propiedades requeridas
            builder.Property(rp => rp.EscuelaId)
                .IsRequired();

            builder.Property(rp => rp.Nombre)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(rp => rp.Descripcion)
                .HasColumnType("LONGTEXT");

            builder.Property(rp => rp.TipoReporte)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30);

            // Configuración de consulta
            builder.Property(rp => rp.ConsultaSQL)
                .HasColumnType("LONGTEXT");

            builder.Property(rp => rp.ConfiguracionJSON)
                .HasColumnType("LONGTEXT");

            builder.Property(rp => rp.OrigenDatos)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("SQL");

            builder.Property(rp => rp.StoredProcedure)
                .HasMaxLength(200);

            // Parámetros
            builder.Property(rp => rp.ParametrosJSON)
                .HasColumnType("LONGTEXT");

            builder.Property(rp => rp.RequiereParametros)
                .HasDefaultValue(false);

            builder.Property(rp => rp.ParametrosObligatorios)
                .HasMaxLength(500);

            // Formato y presentación
            builder.Property(rp => rp.FormatoSalida)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(FormatoReporte.PDF);

            builder.Property(rp => rp.FormatosAdicionales)
                .HasMaxLength(200);

            builder.Property(rp => rp.PlantillaHTML)
                .HasColumnType("LONGTEXT");

            builder.Property(rp => rp.ConfiguracionColumnas)
                .HasColumnType("LONGTEXT");

            builder.Property(rp => rp.TieneGraficas)
                .HasDefaultValue(false);

            builder.Property(rp => rp.ConfiguracionGraficas)
                .HasColumnType("LONGTEXT");

            // Programación automática
            builder.Property(rp => rp.ProgramacionAutomatica)
                .HasDefaultValue(false);

            builder.Property(rp => rp.Frecuencia)
                .IsRequired(false)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(rp => rp.ExpresionCron)
                .HasMaxLength(100);

            builder.Property(rp => rp.DiaMes)
                .IsRequired(false);

            builder.Property(rp => rp.DiaSemana)
                .IsRequired(false);

            builder.Property(rp => rp.HoraEjecucion)
                .IsRequired(false);

            builder.Property(rp => rp.ProximaEjecucion)
                .IsRequired(false);

            builder.Property(rp => rp.UltimaEjecucion)
                .IsRequired(false);

            // Destinatarios
            builder.Property(rp => rp.EnvioAutomatico)
                .HasDefaultValue(false);

            builder.Property(rp => rp.CorreosDestinatarios)
                .HasColumnType("LONGTEXT");

            builder.Property(rp => rp.AsuntoCorreo)
                .HasMaxLength(300);

            builder.Property(rp => rp.CuerpoCorreo)
                .HasColumnType("LONGTEXT");

            // Filtros y ordenamiento
            builder.Property(rp => rp.FiltrosPredeterminados)
                .HasColumnType("LONGTEXT");

            builder.Property(rp => rp.OrdenamientoPredeterminado)
                .HasMaxLength(200);

            builder.Property(rp => rp.PermiteOrdenamientoDinamico)
                .HasDefaultValue(true);

            builder.Property(rp => rp.LimiteRegistros)
                .HasDefaultValue(0);

            // Caché y rendimiento
            builder.Property(rp => rp.HabilitarCache)
                .HasDefaultValue(false);

            builder.Property(rp => rp.TiempoCacheMinutos)
                .IsRequired(false);

            builder.Property(rp => rp.TimeoutSegundos)
                .IsRequired(false);

            // Seguridad y permisos
            builder.Property(rp => rp.EsPrivado)
                .HasDefaultValue(false);

            builder.Property(rp => rp.RolesPermitidos)
                .HasMaxLength(500);

            builder.Property(rp => rp.RequiereAprobacion)
                .HasDefaultValue(false);

            // Estado y estadísticas
            builder.Property(rp => rp.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(rp => rp.EsReporteSistema)
                .HasDefaultValue(false);

            builder.Property(rp => rp.VecesEjecutado)
                .HasDefaultValue(0);

            builder.Property(rp => rp.FechaUltimaEjecucionManual)
                .IsRequired(false);

            builder.Property(rp => rp.TiempoPromedioEjecucion)
                .IsRequired(false)
                .HasColumnType("decimal(10,2)");

            // Metadata
            builder.Property(rp => rp.Categoria)
                .HasMaxLength(100);

            builder.Property(rp => rp.Tags)
                .HasMaxLength(500);

            builder.Property(rp => rp.Icono)
                .HasMaxLength(50);

            builder.Property(rp => rp.Orden)
                .HasDefaultValue(0);

            builder.Property(rp => rp.Observaciones)
                .HasColumnType("LONGTEXT");

            // Auditoría
            builder.Property(rp => rp.CreatedAt)
                .IsRequired();

            builder.Property(rp => rp.CreatedBy)
                .IsRequired(false);

            builder.Property(rp => rp.UpdatedAt)
                .IsRequired();

            builder.Property(rp => rp.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(rp => rp.Creador)
                .WithMany()
                .HasForeignKey(rp => rp.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices
            builder.HasIndex(rp => rp.EscuelaId)
                .HasDatabaseName("IX_ReportesPersonalizados_EscuelaId");

            builder.HasIndex(rp => rp.Nombre)
                .HasDatabaseName("IX_ReportesPersonalizados_Nombre");

            builder.HasIndex(rp => rp.TipoReporte)
                .HasDatabaseName("IX_ReportesPersonalizados_TipoReporte");

            builder.HasIndex(rp => rp.Activo)
                .HasDatabaseName("IX_ReportesPersonalizados_Activo");

            builder.HasIndex(rp => rp.ProgramacionAutomatica)
                .HasDatabaseName("IX_ReportesPersonalizados_ProgramacionAutomatica");

            builder.HasIndex(rp => rp.ProximaEjecucion)
                .HasDatabaseName("IX_ReportesPersonalizados_ProximaEjecucion");

            builder.HasIndex(rp => rp.EsReporteSistema)
                .HasDatabaseName("IX_ReportesPersonalizados_EsReporteSistema");

            builder.HasIndex(rp => rp.EsPrivado)
                .HasDatabaseName("IX_ReportesPersonalizados_EsPrivado");

            builder.HasIndex(rp => new { rp.EscuelaId, rp.TipoReporte, rp.Activo })
                .HasDatabaseName("IX_ReportesPersonalizados_Escuela_Tipo_Activo");

            builder.HasIndex(rp => new { rp.ProgramacionAutomatica, rp.ProximaEjecucion })
                .HasDatabaseName("IX_ReportesPersonalizados_Programacion_Proxima");

            builder.HasIndex(rp => rp.Categoria)
                .HasDatabaseName("IX_ReportesPersonalizados_Categoria");

            builder.HasIndex(rp => rp.CreatedBy)
                .HasDatabaseName("IX_ReportesPersonalizados_CreatedBy");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(rp => rp.EsAcademico);
            builder.Ignore(rp => rp.EsFinanciero);
            builder.Ignore(rp => rp.EstaProgramado);
            builder.Ignore(rp => rp.DebeEjecutarse);
            builder.Ignore(rp => rp.DiasHastaProximaEjecucion);
            builder.Ignore(rp => rp.HaSidoEjecutado);
            builder.Ignore(rp => rp.EsPocoUsado);
            builder.Ignore(rp => rp.UsaSQLDirecto);
            builder.Ignore(rp => rp.UsaStoredProcedure);
            builder.Ignore(rp => rp.TieneCacheConfigurado);

            // Constraints
            builder.HasCheckConstraint("CK_ReportesPersonalizados_DiaMes",
                "`DiaMes` IS NULL OR (`DiaMes` >= 1 AND `DiaMes` <= 31)");

            builder.HasCheckConstraint("CK_ReportesPersonalizados_DiaSemana",
                "`DiaSemana` IS NULL OR (`DiaSemana` >= 0 AND `DiaSemana` <= 6)");

            builder.HasCheckConstraint("CK_ReportesPersonalizados_HoraEjecucion",
                "`HoraEjecucion` IS NULL OR (`HoraEjecucion` >= 0 AND `HoraEjecucion` <= 23)");

            builder.HasCheckConstraint("CK_ReportesPersonalizados_LimiteRegistros",
                "`LimiteRegistros` >= 0");

            builder.HasCheckConstraint("CK_ReportesPersonalizados_TiempoCacheMinutos",
                "`TiempoCacheMinutos` IS NULL OR `TiempoCacheMinutos` > 0");

            builder.HasCheckConstraint("CK_ReportesPersonalizados_TimeoutSegundos",
                "`TimeoutSegundos` IS NULL OR `TimeoutSegundos` > 0");

            builder.HasCheckConstraint("CK_ReportesPersonalizados_VecesEjecutado",
                "`VecesEjecutado` >= 0");

            builder.HasCheckConstraint("CK_ReportesPersonalizados_TiempoPromedioEjecucion",
                "`TiempoPromedioEjecucion` IS NULL OR `TiempoPromedioEjecucion` >= 0");
        }
    }
}