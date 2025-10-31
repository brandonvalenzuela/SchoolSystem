using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Configuracion;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad ConfiguracionEscuela
    /// </summary>
    public class ConfiguracionEscuelaConfiguration : IEntityTypeConfiguration<ConfiguracionEscuela>
    {
        public void Configure(EntityTypeBuilder<ConfiguracionEscuela> builder)
        {
            // Nombre de tabla
            builder.ToTable("ConfiguracionesEscuela");

            // Clave primaria
            builder.HasKey(ce => ce.Id);

            // Propiedades requeridas
            builder.Property(ce => ce.EscuelaId)
                .IsRequired();

            // Información general
            builder.Property(ce => ce.NombreInstitucion)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(ce => ce.NombreCorto)
                .HasMaxLength(100);

            builder.Property(ce => ce.Lema)
                .HasMaxLength(500);

            builder.Property(ce => ce.Mision)
                .HasColumnType("LONGTEXT");

            builder.Property(ce => ce.Vision)
                .HasColumnType("LONGTEXT");

            builder.Property(ce => ce.Valores)
                .HasColumnType("LONGTEXT");

            // Datos de contacto
            builder.Property(ce => ce.Direccion)
                .HasMaxLength(500);

            builder.Property(ce => ce.Ciudad)
                .HasMaxLength(100);

            builder.Property(ce => ce.Estado)
                .HasMaxLength(100);

            builder.Property(ce => ce.CodigoPostal)
                .HasMaxLength(20);

            builder.Property(ce => ce.Pais)
                .HasMaxLength(100);

            builder.Property(ce => ce.Telefono)
                .HasMaxLength(20);

            builder.Property(ce => ce.TelefonoAlternativo)
                .HasMaxLength(20);

            builder.Property(ce => ce.Email)
                .HasMaxLength(200);

            builder.Property(ce => ce.SitioWeb)
                .HasMaxLength(300);

            // Identidad visual
            builder.Property(ce => ce.LogoUrl)
                .HasMaxLength(500);

            builder.Property(ce => ce.LogoPequenoUrl)
                .HasMaxLength(500);

            builder.Property(ce => ce.ColorPrimario)
                .HasMaxLength(7)
                .HasDefaultValue("#1976D2");

            builder.Property(ce => ce.ColorSecundario)
                .HasMaxLength(7)
                .HasDefaultValue("#424242");

            builder.Property(ce => ce.ColorAcento)
                .HasMaxLength(7)
                .HasDefaultValue("#FF9800");

            builder.Property(ce => ce.ImagenFondoLoginUrl)
                .HasMaxLength(500);

            // Configuración académica
            builder.Property(ce => ce.SistemaCalificacion)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Numerico");

            builder.Property(ce => ce.CalificacionMinimaAprobatoria)
                .HasColumnType("decimal(5,2)")
                .HasDefaultValue(6.0m);

            builder.Property(ce => ce.CalificacionMaxima)
                .HasColumnType("decimal(5,2)")
                .HasDefaultValue(10.0m);

            builder.Property(ce => ce.DecimalesCalificacion)
                .HasDefaultValue(1);

            builder.Property(ce => ce.PeriodosPorCiclo)
                .HasDefaultValue(3);

            builder.Property(ce => ce.DuracionClaseMinutos)
                .HasDefaultValue(50);

            builder.Property(ce => ce.DuracionRecesoMinutos)
                .HasDefaultValue(20);

            builder.Property(ce => ce.PorcentajeMinimoAsistencia)
                .HasColumnType("decimal(5,2)")
                .HasDefaultValue(80.0m);

            builder.Property(ce => ce.PermiteReprobacion)
                .HasDefaultValue(true);

            builder.Property(ce => ce.MaximaMateriasReprobadas)
                .IsRequired(false);

            // Configuración de notificaciones
            builder.Property(ce => ce.NotificacionesEmailHabilitadas)
                .HasDefaultValue(true);

            builder.Property(ce => ce.NotificacionesSMSHabilitadas)
                .HasDefaultValue(false);

            builder.Property(ce => ce.NotificacionesPushHabilitadas)
                .HasDefaultValue(true);

            builder.Property(ce => ce.NotificarCalificacionesAutomaticamente)
                .HasDefaultValue(true);

            builder.Property(ce => ce.NotificarAsistenciaAutomaticamente)
                .HasDefaultValue(true);

            builder.Property(ce => ce.NotificarTareasAutomaticamente)
                .HasDefaultValue(true);

            builder.Property(ce => ce.NotificarEventosAutomaticamente)
                .HasDefaultValue(true);

            // Configuración de seguridad
            builder.Property(ce => ce.RequiereCambioPasswordPrimerLogin)
                .HasDefaultValue(true);

            builder.Property(ce => ce.DiasVigenciaPassword)
                .HasDefaultValue(90);

            builder.Property(ce => ce.LongitudMinimaPassword)
                .HasDefaultValue(8);

            builder.Property(ce => ce.RequiereMayusculasPassword)
                .HasDefaultValue(true);

            builder.Property(ce => ce.RequiereNumerosPassword)
                .HasDefaultValue(true);

            builder.Property(ce => ce.RequiereCaracteresEspecialesPassword)
                .HasDefaultValue(false);

            builder.Property(ce => ce.IntentosLoginFallidosAntesBloQueo)
                .HasDefaultValue(5);

            builder.Property(ce => ce.MinutosBloqueoPorIntentosFallidos)
                .HasDefaultValue(15);

            builder.Property(ce => ce.Autenticacion2FactoresHabilitada)
                .HasDefaultValue(false);

            builder.Property(ce => ce.TiempoSesionMinutos)
                .HasDefaultValue(60);

            // Configuración de reportes
            builder.Property(ce => ce.FormatoPredeterminadoReportes)
                .HasMaxLength(20)
                .HasDefaultValue("PDF");

            builder.Property(ce => ce.IncluirLogoEnReportes)
                .HasDefaultValue(true);

            builder.Property(ce => ce.IncluirFirmaDigitalEnDocumentos)
                .HasDefaultValue(false);

            builder.Property(ce => ce.PlantillaBoletaPredeterminadaId)
                .IsRequired(false);

            builder.Property(ce => ce.PlantillaConstanciaPredeterminadaId)
                .IsRequired(false);

            // Configuración de pagos
            builder.Property(ce => ce.ModuloPagosHabilitado)
                .HasDefaultValue(true);

            builder.Property(ce => ce.PermitePagosEnLinea)
                .HasDefaultValue(false);

            builder.Property(ce => ce.ProveedorPagos)
                .HasMaxLength(50);

            builder.Property(ce => ce.MonedaPredeterminada)
                .HasMaxLength(10)
                .HasDefaultValue("MXN");

            builder.Property(ce => ce.SimboloMoneda)
                .HasMaxLength(10)
                .HasDefaultValue("$");

            builder.Property(ce => ce.DiasToleranciaParaPagos)
                .HasDefaultValue(5);

            builder.Property(ce => ce.PorcentajeRecargoMora)
                .IsRequired(false)
                .HasColumnType("decimal(5,2)");

            // Zona horaria y región
            builder.Property(ce => ce.ZonaHoraria)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("America/Mexico_City");

            builder.Property(ce => ce.IdiomaPredeterminado)
                .IsRequired()
                .HasMaxLength(10)
                .HasDefaultValue("es-MX");

            builder.Property(ce => ce.FormatoFecha)
                .HasMaxLength(50)
                .HasDefaultValue("dd/MM/yyyy");

            builder.Property(ce => ce.FormatoHora)
                .HasMaxLength(50)
                .HasDefaultValue("HH:mm");

            builder.Property(ce => ce.PrimerDiaSemana)
                .HasDefaultValue(1);

            // Integraciones
            builder.Property(ce => ce.GoogleApiKey)
                .HasMaxLength(500);

            builder.Property(ce => ce.IntegracionGoogleClassroomHabilitada)
                .HasDefaultValue(false);

            builder.Property(ce => ce.IntegracionMicrosoftTeamsHabilitada)
                .HasDefaultValue(false);

            builder.Property(ce => ce.IntegracionZoomHabilitada)
                .HasDefaultValue(false);

            builder.Property(ce => ce.ConfiguracionSMTP)
                .HasColumnType("LONGTEXT");

            builder.Property(ce => ce.ConfiguracionSMS)
                .HasColumnType("LONGTEXT");

            // Licencia y límites
            builder.Property(ce => ce.TipoPlan)
                .HasMaxLength(50)
                .HasDefaultValue("Basic");

            builder.Property(ce => ce.FechaExpiracionLicencia)
                .IsRequired(false);

            builder.Property(ce => ce.LimiteAlumnos)
                .IsRequired(false);

            builder.Property(ce => ce.LimiteMaestros)
                .IsRequired(false);

            builder.Property(ce => ce.LimiteAlmacenamientoGB)
                .IsRequired(false);

            // Metadata
            builder.Property(ce => ce.DatosAdicionales)
                .HasColumnType("LONGTEXT");

            builder.Property(ce => ce.Observaciones)
                .HasColumnType("LONGTEXT");

            // Auditoría
            builder.Property(ce => ce.CreatedAt)
                .IsRequired();

            builder.Property(ce => ce.CreatedBy)
                .IsRequired(false);

            builder.Property(ce => ce.UpdatedAt)
                .IsRequired();

            builder.Property(ce => ce.UpdatedBy)
                .IsRequired(false);

            // Índices
            // Índice único: Una escuela solo puede tener una configuración
            builder.HasIndex(ce => ce.EscuelaId)
                .IsUnique()
                .HasDatabaseName("IX_ConfiguracionesEscuela_EscuelaId_Unique");

            builder.HasIndex(ce => ce.NombreInstitucion)
                .HasDatabaseName("IX_ConfiguracionesEscuela_NombreInstitucion");

            builder.HasIndex(ce => ce.TipoPlan)
                .HasDatabaseName("IX_ConfiguracionesEscuela_TipoPlan");

            builder.HasIndex(ce => ce.FechaExpiracionLicencia)
                .HasDatabaseName("IX_ConfiguracionesEscuela_FechaExpiracionLicencia");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(ce => ce.TieneLogo);
            builder.Ignore(ce => ce.TieneColoresPersonalizados);
            builder.Ignore(ce => ce.LicenciaActiva);
            builder.Ignore(ce => ce.DiasHastaExpiracionLicencia);
            builder.Ignore(ce => ce.LicenciaProximaVencer);
            builder.Ignore(ce => ce.UsaSistemaNumerico);
            builder.Ignore(ce => ce.TienePagosEnLinea);
            builder.Ignore(ce => ce.TieneNotificacionesHabilitadas);
            builder.Ignore(ce => ce.RequierePasswordFuerte);
            builder.Ignore(ce => ce.TieneIntegraciones);
            builder.Ignore(ce => ce.EsPlanGratuito);
            builder.Ignore(ce => ce.EsPlanEnterprise);

            // Constraints
            builder.HasCheckConstraint("CK_ConfiguracionesEscuela_Calificaciones",
                "`CalificacionMinimaAprobatoria` < `CalificacionMaxima`");

            builder.HasCheckConstraint("CK_ConfiguracionesEscuela_DecimalesCalificacion",
                "`DecimalesCalificacion` >= 0 AND `DecimalesCalificacion` <= 2");

            builder.HasCheckConstraint("CK_ConfiguracionesEscuela_PeriodosPorCiclo",
                "`PeriodosPorCiclo` >= 1 AND `PeriodosPorCiclo` <= 6");

            builder.HasCheckConstraint("CK_ConfiguracionesEscuela_DuracionClase",
                "`DuracionClaseMinutos` >= 30 AND `DuracionClaseMinutos` <= 180");

            builder.HasCheckConstraint("CK_ConfiguracionesEscuela_PorcentajeAsistencia",
                "`PorcentajeMinimoAsistencia` >= 0 AND `PorcentajeMinimoAsistencia` <= 100");

            builder.HasCheckConstraint("CK_ConfiguracionesEscuela_LongitudPassword",
                "`LongitudMinimaPassword` >= 6 AND `LongitudMinimaPassword` <= 32");

            builder.HasCheckConstraint("CK_ConfiguracionesEscuela_IntentosLogin",
                "`IntentosLoginFallidosAntesBloQueo` >= 3 AND `IntentosLoginFallidosAntesBloQueo` <= 10");

            builder.HasCheckConstraint("CK_ConfiguracionesEscuela_TiempoSesion",
                "`TiempoSesionMinutos` >= 15 AND `TiempoSesionMinutos` <= 480");

            builder.HasCheckConstraint("CK_ConfiguracionesEscuela_PrimerDiaSemana",
                "`PrimerDiaSemana` >= 0 AND `PrimerDiaSemana` <= 6");

            builder.HasCheckConstraint("CK_ConfiguracionesEscuela_Limites",
                "(`LimiteAlumnos` IS NULL OR `LimiteAlumnos` > 0) AND " +
                "(`LimiteMaestros` IS NULL OR `LimiteMaestros` > 0) AND " +
                "(`LimiteAlmacenamientoGB` IS NULL OR `LimiteAlmacenamientoGB` > 0)");
        }
    }
}