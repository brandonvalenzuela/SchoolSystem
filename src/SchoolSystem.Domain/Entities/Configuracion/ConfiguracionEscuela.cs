using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolSystem.Domain.Entities.Common;

namespace SchoolSystem.Domain.Entities.Configuracion
{
    /// <summary>
    /// Configuración general de la escuela
    /// </summary>
    [Table("ConfiguracionesEscuela")]
    public class ConfiguracionEscuela : BaseEntity, IAuditableEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        #endregion

        #region Información General

        /// <summary>
        /// Nombre completo de la institución
        /// </summary>
        [Required]
        [StringLength(300)]
        public string NombreInstitucion { get; set; }

        /// <summary>
        /// Nombre corto o siglas
        /// </summary>
        [StringLength(100)]
        public string NombreCorto { get; set; }

        /// <summary>
        /// Lema o slogan institucional
        /// </summary>
        [StringLength(500)]
        public string Lema { get; set; }

        /// <summary>
        /// Misión
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Mision { get; set; }

        /// <summary>
        /// Visión
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Vision { get; set; }

        /// <summary>
        /// Valores institucionales
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Valores { get; set; }

        #endregion

        #region Datos de Contacto

        /// <summary>
        /// Dirección completa
        /// </summary>
        [StringLength(500)]
        public string Direccion { get; set; }

        /// <summary>
        /// Ciudad
        /// </summary>
        [StringLength(100)]
        public string Ciudad { get; set; }

        /// <summary>
        /// Estado/Provincia
        /// </summary>
        [StringLength(100)]
        public string Estado { get; set; }

        /// <summary>
        /// Código postal
        /// </summary>
        [StringLength(20)]
        public string CodigoPostal { get; set; }

        /// <summary>
        /// País
        /// </summary>
        [StringLength(100)]
        public string Pais { get; set; }

        /// <summary>
        /// Teléfono principal
        /// </summary>
        [StringLength(20)]
        public string Telefono { get; set; }

        /// <summary>
        /// Teléfono alternativo
        /// </summary>
        [StringLength(20)]
        public string TelefonoAlternativo { get; set; }

        /// <summary>
        /// Correo electrónico institucional
        /// </summary>
        [StringLength(200)]
        public string Email { get; set; }

        /// <summary>
        /// Sitio web
        /// </summary>
        [StringLength(300)]
        public string SitioWeb { get; set; }

        #endregion

        #region Identidad Visual

        /// <summary>
        /// URL del logo principal
        /// </summary>
        [StringLength(500)]
        public string LogoUrl { get; set; }

        /// <summary>
        /// URL del logo pequeño (favicon)
        /// </summary>
        [StringLength(500)]
        public string LogoPequenoUrl { get; set; }

        /// <summary>
        /// Color primario (hexadecimal)
        /// </summary>
        [StringLength(7)]
        public string ColorPrimario { get; set; }

        /// <summary>
        /// Color secundario (hexadecimal)
        /// </summary>
        [StringLength(7)]
        public string ColorSecundario { get; set; }

        /// <summary>
        /// Color de acento (hexadecimal)
        /// </summary>
        [StringLength(7)]
        public string ColorAcento { get; set; }

        /// <summary>
        /// URL de imagen de fondo para login
        /// </summary>
        [StringLength(500)]
        public string ImagenFondoLoginUrl { get; set; }

        #endregion

        #region Configuración Académica

        /// <summary>
        /// Sistema de calificación (numérico, letras, conceptos)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string SistemaCalificacion { get; set; }

        /// <summary>
        /// Calificación mínima aprobatoria
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal CalificacionMinimaAprobatoria { get; set; }

        /// <summary>
        /// Calificación máxima
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal CalificacionMaxima { get; set; }

        /// <summary>
        /// Cantidad de decimales en calificaciones
        /// </summary>
        public int DecimalesCalificacion { get; set; }

        /// <summary>
        /// Cantidad de periodos por ciclo escolar
        /// </summary>
        public int PeriodosPorCiclo { get; set; }

        /// <summary>
        /// Duración de clase en minutos
        /// </summary>
        public int DuracionClaseMinutos { get; set; }

        /// <summary>
        /// Duración de receso en minutos
        /// </summary>
        public int DuracionRecesoMinutos { get; set; }

        /// <summary>
        /// Porcentaje mínimo de asistencia
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal PorcentajeMinimoAsistencia { get; set; }

        /// <summary>
        /// Permite reprobación
        /// </summary>
        public bool PermiteReprobacion { get; set; }

        /// <summary>
        /// Cantidad máxima de materias reprobadas
        /// </summary>
        public int? MaximaMateriasReprobadas { get; set; }

        #endregion

        #region Configuración de Notificaciones

        /// <summary>
        /// Habilitar notificaciones por email
        /// </summary>
        public bool NotificacionesEmailHabilitadas { get; set; }

        /// <summary>
        /// Habilitar notificaciones por SMS
        /// </summary>
        public bool NotificacionesSMSHabilitadas { get; set; }

        /// <summary>
        /// Habilitar notificaciones push
        /// </summary>
        public bool NotificacionesPushHabilitadas { get; set; }

        /// <summary>
        /// Enviar notificaciones de calificaciones automáticamente
        /// </summary>
        public bool NotificarCalificacionesAutomaticamente { get; set; }

        /// <summary>
        /// Enviar notificaciones de asistencia automáticamente
        /// </summary>
        public bool NotificarAsistenciaAutomaticamente { get; set; }

        /// <summary>
        /// Enviar notificaciones de tareas automáticamente
        /// </summary>
        public bool NotificarTareasAutomaticamente { get; set; }

        /// <summary>
        /// Enviar notificaciones de eventos automáticamente
        /// </summary>
        public bool NotificarEventosAutomaticamente { get; set; }

        #endregion

        #region Configuración de Seguridad

        /// <summary>
        /// Requiere cambio de contraseña al primer login
        /// </summary>
        public bool RequiereCambioPasswordPrimerLogin { get; set; }

        /// <summary>
        /// Días de vigencia de contraseña (0 = sin expiración)
        /// </summary>
        public int DiasVigenciaPassword { get; set; }

        /// <summary>
        /// Longitud mínima de contraseña
        /// </summary>
        public int LongitudMinimaPassword { get; set; }

        /// <summary>
        /// Requiere mayúsculas en contraseña
        /// </summary>
        public bool RequiereMayusculasPassword { get; set; }

        /// <summary>
        /// Requiere números en contraseña
        /// </summary>
        public bool RequiereNumerosPassword { get; set; }

        /// <summary>
        /// Requiere caracteres especiales en contraseña
        /// </summary>
        public bool RequiereCaracteresEspecialesPassword { get; set; }

        /// <summary>
        /// Intentos de login fallidos antes de bloqueo
        /// </summary>
        public int IntentosLoginFallidosAntesBloQueo { get; set; }

        /// <summary>
        /// Minutos de bloqueo por intentos fallidos
        /// </summary>
        public int MinutosBloqueoPorIntentosFallidos { get; set; }

        /// <summary>
        /// Habilitar autenticación de dos factores
        /// </summary>
        public bool Autenticacion2FactoresHabilitada { get; set; }

        /// <summary>
        /// Tiempo de sesión en minutos
        /// </summary>
        public int TiempoSesionMinutos { get; set; }

        #endregion

        #region Configuración de Reportes

        /// <summary>
        /// Formato predeterminado de reportes
        /// </summary>
        [StringLength(20)]
        public string FormatoPredeterminadoReportes { get; set; }

        /// <summary>
        /// Incluir logo en reportes
        /// </summary>
        public bool IncluirLogoEnReportes { get; set; }

        /// <summary>
        /// Incluir firma digital en documentos
        /// </summary>
        public bool IncluirFirmaDigitalEnDocumentos { get; set; }

        /// <summary>
        /// Plantilla de boleta predeterminada
        /// </summary>
        public int? PlantillaBoletaPredeterminadaId { get; set; }

        /// <summary>
        /// Plantilla de constancia predeterminada
        /// </summary>
        public int? PlantillaConstanciaPredeterminadaId { get; set; }

        #endregion

        #region Configuración de Pagos

        /// <summary>
        /// Módulo de pagos habilitado
        /// </summary>
        public bool ModuloPagosHabilitado { get; set; }

        /// <summary>
        /// Permite pagos en línea
        /// </summary>
        public bool PermitePagosEnLinea { get; set; }

        /// <summary>
        /// Proveedor de pagos (Stripe, PayPal, etc.)
        /// </summary>
        [StringLength(50)]
        public string ProveedorPagos { get; set; }

        /// <summary>
        /// Moneda predeterminada
        /// </summary>
        [StringLength(10)]
        public string MonedaPredeterminada { get; set; }

        /// <summary>
        /// Símbolo de moneda
        /// </summary>
        [StringLength(10)]
        public string SimboloMoneda { get; set; }

        /// <summary>
        /// Días de tolerancia para pagos
        /// </summary>
        public int DiasToleranciaParaPagos { get; set; }

        /// <summary>
        /// Porcentaje de recargo por mora
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal? PorcentajeRecargoMora { get; set; }

        #endregion

        #region Zona Horaria y Región

        /// <summary>
        /// Zona horaria (IANA Time Zone)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ZonaHoraria { get; set; }

        /// <summary>
        /// Idioma predeterminado
        /// </summary>
        [Required]
        [StringLength(10)]
        public string IdiomaPredeterminado { get; set; }

        /// <summary>
        /// Formato de fecha
        /// </summary>
        [StringLength(50)]
        public string FormatoFecha { get; set; }

        /// <summary>
        /// Formato de hora
        /// </summary>
        [StringLength(50)]
        public string FormatoHora { get; set; }

        /// <summary>
        /// Primer día de la semana (0=Domingo, 1=Lunes)
        /// </summary>
        public int PrimerDiaSemana { get; set; }

        #endregion

        #region Integraciones

        /// <summary>
        /// API Key de Google (Maps, Calendar, etc.)
        /// </summary>
        [StringLength(500)]
        public string GoogleApiKey { get; set; }

        /// <summary>
        /// Integración con Google Classroom habilitada
        /// </summary>
        public bool IntegracionGoogleClassroomHabilitada { get; set; }

        /// <summary>
        /// Integración con Microsoft Teams habilitada
        /// </summary>
        public bool IntegracionMicrosoftTeamsHabilitada { get; set; }

        /// <summary>
        /// Integración con Zoom habilitada
        /// </summary>
        public bool IntegracionZoomHabilitada { get; set; }

        /// <summary>
        /// Configuración de SMTP para correos (JSON)
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ConfiguracionSMTP { get; set; }

        /// <summary>
        /// Configuración de SMS (JSON)
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ConfiguracionSMS { get; set; }

        #endregion

        #region Licencia y Límites

        /// <summary>
        /// Tipo de plan (Free, Basic, Premium, Enterprise)
        /// </summary>
        [StringLength(50)]
        public string TipoPlan { get; set; }

        /// <summary>
        /// Fecha de expiración de la licencia
        /// </summary>
        public DateTime? FechaExpiracionLicencia { get; set; }

        /// <summary>
        /// Límite de alumnos
        /// </summary>
        public int? LimiteAlumnos { get; set; }

        /// <summary>
        /// Límite de maestros
        /// </summary>
        public int? LimiteMaestros { get; set; }

        /// <summary>
        /// Límite de almacenamiento en GB
        /// </summary>
        public int? LimiteAlmacenamientoGB { get; set; }

        #endregion

        #region Metadata

        /// <summary>
        /// Datos adicionales en formato JSON
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string DatosAdicionales { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Observaciones { get; set; }

        #endregion

        #region Auditoría (IAuditableEntity)

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        #endregion

        #region Constructor

        public ConfiguracionEscuela()
        {
            SistemaCalificacion = "Numerico";
            CalificacionMinimaAprobatoria = 6.0m;
            CalificacionMaxima = 10.0m;
            DecimalesCalificacion = 1;
            PeriodosPorCiclo = 3;
            DuracionClaseMinutos = 50;
            DuracionRecesoMinutos = 20;
            PorcentajeMinimoAsistencia = 80.0m;
            PermiteReprobacion = true;
            MaximaMateriasReprobadas = 3;

            NotificacionesEmailHabilitadas = true;
            NotificacionesSMSHabilitadas = false;
            NotificacionesPushHabilitadas = true;
            NotificarCalificacionesAutomaticamente = true;
            NotificarAsistenciaAutomaticamente = true;
            NotificarTareasAutomaticamente = true;
            NotificarEventosAutomaticamente = true;

            RequiereCambioPasswordPrimerLogin = true;
            DiasVigenciaPassword = 90;
            LongitudMinimaPassword = 8;
            RequiereMayusculasPassword = true;
            RequiereNumerosPassword = true;
            RequiereCaracteresEspecialesPassword = false;
            IntentosLoginFallidosAntesBloQueo = 5;
            MinutosBloqueoPorIntentosFallidos = 15;
            Autenticacion2FactoresHabilitada = false;
            TiempoSesionMinutos = 60;

            FormatoPredeterminadoReportes = "PDF";
            IncluirLogoEnReportes = true;
            IncluirFirmaDigitalEnDocumentos = false;

            ModuloPagosHabilitado = true;
            PermitePagosEnLinea = false;
            MonedaPredeterminada = "MXN";
            SimboloMoneda = "$";
            DiasToleranciaParaPagos = 5;

            ZonaHoraria = "America/Mexico_City";
            IdiomaPredeterminado = "es-MX";
            FormatoFecha = "dd/MM/yyyy";
            FormatoHora = "HH:mm";
            PrimerDiaSemana = 1;

            IntegracionGoogleClassroomHabilitada = false;
            IntegracionMicrosoftTeamsHabilitada = false;
            IntegracionZoomHabilitada = false;

            TipoPlan = "Basic";

            ColorPrimario = "#1976D2";
            ColorSecundario = "#424242";
            ColorAcento = "#FF9800";
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si tiene logo configurado
        /// </summary>
        public bool TieneLogo => !string.IsNullOrWhiteSpace(LogoUrl);

        /// <summary>
        /// Indica si tiene colores personalizados
        /// </summary>
        public bool TieneColoresPersonalizados => !string.IsNullOrWhiteSpace(ColorPrimario);

        /// <summary>
        /// Indica si la licencia está activa
        /// </summary>
        public bool LicenciaActiva => !FechaExpiracionLicencia.HasValue || FechaExpiracionLicencia.Value > DateTime.Now;

        /// <summary>
        /// Días hasta la expiración de la licencia
        /// </summary>
        public int? DiasHastaExpiracionLicencia
        {
            get
            {
                if (!FechaExpiracionLicencia.HasValue) return null;
                return (FechaExpiracionLicencia.Value.Date - DateTime.Now.Date).Days;
            }
        }

        /// <summary>
        /// Indica si la licencia está próxima a vencer (menos de 30 días)
        /// </summary>
        public bool LicenciaProximaVencer
        {
            get
            {
                var dias = DiasHastaExpiracionLicencia;
                return dias.HasValue && dias.Value > 0 && dias.Value <= 30;
            }
        }

        /// <summary>
        /// Indica si usa sistema de calificación numérico
        /// </summary>
        public bool UsaSistemaNumerico => SistemaCalificacion == "Numerico";

        /// <summary>
        /// Indica si tiene pagos en línea habilitados
        /// </summary>
        public bool TienePagosEnLinea => ModuloPagosHabilitado && PermitePagosEnLinea;

        /// <summary>
        /// Indica si tiene notificaciones habilitadas
        /// </summary>
        public bool TieneNotificacionesHabilitadas => NotificacionesEmailHabilitadas ||
                                                      NotificacionesSMSHabilitadas ||
                                                      NotificacionesPushHabilitadas;

        /// <summary>
        /// Indica si requiere contraseña fuerte
        /// </summary>
        public bool RequierePasswordFuerte => LongitudMinimaPassword >= 8 &&
                                              RequiereMayusculasPassword &&
                                              RequiereNumerosPassword;

        /// <summary>
        /// Indica si tiene integraciones configuradas
        /// </summary>
        public bool TieneIntegraciones => IntegracionGoogleClassroomHabilitada ||
                                          IntegracionMicrosoftTeamsHabilitada ||
                                          IntegracionZoomHabilitada;

        /// <summary>
        /// Indica si es plan gratuito
        /// </summary>
        public bool EsPlanGratuito => TipoPlan == "Free";

        /// <summary>
        /// Indica si es plan enterprise
        /// </summary>
        public bool EsPlanEnterprise => TipoPlan == "Enterprise";

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Establece los colores institucionales
        /// </summary>
        public void EstablecerColores(string primario, string secundario, string acento)
        {
            if (!EsColorHexadecimalValido(primario))
                throw new ArgumentException("Color primario inválido");

            if (!string.IsNullOrWhiteSpace(secundario) && !EsColorHexadecimalValido(secundario))
                throw new ArgumentException("Color secundario inválido");

            if (!string.IsNullOrWhiteSpace(acento) && !EsColorHexadecimalValido(acento))
                throw new ArgumentException("Color de acento inválido");

            ColorPrimario = primario;
            ColorSecundario = secundario;
            ColorAcento = acento;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Configura el sistema de calificación
        /// </summary>
        public void ConfigurarSistemaCalificacion(string sistema, decimal minimaAprobatoria,
                                                  decimal maxima, int decimales)
        {
            if (minimaAprobatoria >= maxima)
                throw new ArgumentException("La calificación mínima debe ser menor que la máxima");

            if (decimales < 0 || decimales > 2)
                throw new ArgumentException("Los decimales deben estar entre 0 y 2");

            SistemaCalificacion = sistema;
            CalificacionMinimaAprobatoria = minimaAprobatoria;
            CalificacionMaxima = maxima;
            DecimalesCalificacion = decimales;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Habilita/deshabilita notificaciones
        /// </summary>
        public void ConfigurarNotificaciones(bool email, bool sms, bool push)
        {
            NotificacionesEmailHabilitadas = email;
            NotificacionesSMSHabilitadas = sms;
            NotificacionesPushHabilitadas = push;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Configura la seguridad de contraseñas
        /// </summary>
        public void ConfigurarSeguridadPassword(int longitudMinima, bool requiereMayusculas,
                                               bool requiereNumeros, bool requiereEspeciales)
        {
            if (longitudMinima < 6 || longitudMinima > 32)
                throw new ArgumentException("La longitud debe estar entre 6 y 32");

            LongitudMinimaPassword = longitudMinima;
            RequiereMayusculasPassword = requiereMayusculas;
            RequiereNumerosPassword = requiereNumeros;
            RequiereCaracteresEspecialesPassword = requiereEspeciales;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Configura los intentos de login
        /// </summary>
        public void ConfigurarIntentosLogin(int intentosMaximos, int minutosBloQueo)
        {
            if (intentosMaximos < 3 || intentosMaximos > 10)
                throw new ArgumentException("Los intentos deben estar entre 3 y 10");

            if (minutosBloQueo < 5 || minutosBloQueo > 60)
                throw new ArgumentException("Los minutos de bloqueo deben estar entre 5 y 60");

            IntentosLoginFallidosAntesBloQueo = intentosMaximos;
            MinutosBloqueoPorIntentosFallidos = minutosBloQueo;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Actualiza la licencia
        /// </summary>
        public void ActualizarLicencia(string tipoPlan, DateTime? fechaExpiracion,
                                       int? limiteAlumnos = null, int? limiteMaestros = null,
                                       int? limiteAlmacenamientoGB = null)
        {
            TipoPlan = tipoPlan;
            FechaExpiracionLicencia = fechaExpiracion;
            LimiteAlumnos = limiteAlumnos;
            LimiteMaestros = limiteMaestros;
            LimiteAlmacenamientoGB = limiteAlmacenamientoGB;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Habilita/deshabilita una integración
        /// </summary>
        public void ConfigurarIntegracion(string nombreIntegracion, bool habilitar)
        {
            switch (nombreIntegracion.ToLower())
            {
                case "googleclassroom":
                    IntegracionGoogleClassroomHabilitada = habilitar;
                    break;
                case "microsoftteams":
                    IntegracionMicrosoftTeamsHabilitada = habilitar;
                    break;
                case "zoom":
                    IntegracionZoomHabilitada = habilitar;
                    break;
                default:
                    throw new ArgumentException($"Integración '{nombreIntegracion}' no reconocida");
            }
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Configura el módulo de pagos
        /// </summary>
        public void ConfigurarPagos(bool habilitado, bool pagosEnLinea, string proveedor,
                                   string moneda, string simbolo, int diasTolerancia)
        {
            ModuloPagosHabilitado = habilitado;
            PermitePagosEnLinea = pagosEnLinea;
            ProveedorPagos = proveedor;
            MonedaPredeterminada = moneda;
            SimboloMoneda = simbolo;
            DiasToleranciaParaPagos = diasTolerancia;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Valida un color hexadecimal
        /// </summary>
        private bool EsColorHexadecimalValido(string color)
        {
            if (string.IsNullOrWhiteSpace(color)) return false;
            return System.Text.RegularExpressions.Regex.IsMatch(color, @"^#[0-9A-Fa-f]{6}$");
        }

        /// <summary>
        /// Valida que la configuración sea correcta
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(NombreInstitucion))
                errores.Add("El nombre de la institución es requerido");

            if (CalificacionMinimaAprobatoria >= CalificacionMaxima)
                errores.Add("La calificación mínima debe ser menor que la máxima");

            if (DecimalesCalificacion < 0 || DecimalesCalificacion > 2)
                errores.Add("Los decimales deben estar entre 0 y 2");

            if (PeriodosPorCiclo < 1 || PeriodosPorCiclo > 6)
                errores.Add("Los periodos por ciclo deben estar entre 1 y 6");

            if (DuracionClaseMinutos < 30 || DuracionClaseMinutos > 180)
                errores.Add("La duración de clase debe estar entre 30 y 180 minutos");

            if (PorcentajeMinimoAsistencia < 0 || PorcentajeMinimoAsistencia > 100)
                errores.Add("El porcentaje de asistencia debe estar entre 0 y 100");

            if (LongitudMinimaPassword < 6 || LongitudMinimaPassword > 32)
                errores.Add("La longitud mínima de contraseña debe estar entre 6 y 32");

            if (IntentosLoginFallidosAntesBloQueo < 3 || IntentosLoginFallidosAntesBloQueo > 10)
                errores.Add("Los intentos de login deben estar entre 3 y 10");

            if (TiempoSesionMinutos < 15 || TiempoSesionMinutos > 480)
                errores.Add("El tiempo de sesión debe estar entre 15 y 480 minutos");

            if (!string.IsNullOrWhiteSpace(ColorPrimario) && !EsColorHexadecimalValido(ColorPrimario))
                errores.Add("El color primario debe ser un color hexadecimal válido");

            if (string.IsNullOrWhiteSpace(ZonaHoraria))
                errores.Add("La zona horaria es requerida");

            if (string.IsNullOrWhiteSpace(IdiomaPredeterminado))
                errores.Add("El idioma predeterminado es requerido");

            if (PrimerDiaSemana < 0 || PrimerDiaSemana > 6)
                errores.Add("El primer día de la semana debe estar entre 0 y 6");

            if (LimiteAlumnos.HasValue && LimiteAlumnos <= 0)
                errores.Add("El límite de alumnos debe ser mayor a cero");

            if (LimiteMaestros.HasValue && LimiteMaestros <= 0)
                errores.Add("El límite de maestros debe ser mayor a cero");

            if (LimiteAlmacenamientoGB.HasValue && LimiteAlmacenamientoGB <= 0)
                errores.Add("El límite de almacenamiento debe ser mayor a cero");

            return errores;
        }

        #endregion
    }
}