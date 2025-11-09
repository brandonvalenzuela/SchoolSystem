using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Enums.Documentos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Domain.Entities.Documentos
{
    /// <summary>
    /// Definición de reportes personalizados del sistema
    /// </summary>
    [Table("ReportesPersonalizados")]
    public class ReportePersonalizado : BaseEntity, IAuditableEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// Nombre del reporte
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Nombre { get; set; }

        /// <summary>
        /// Descripción del reporte
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Tipo de reporte
        /// </summary>
        [Required]
        public TipoReporte TipoReporte { get; set; }

        #endregion

        #region Configuración de Consulta

        /// <summary>
        /// Consulta SQL del reporte
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ConsultaSQL { get; set; }

        /// <summary>
        /// Configuración del reporte en formato JSON
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ConfiguracionJSON { get; set; }

        /// <summary>
        /// Origen de datos (SQL, API, Entity, Stored Procedure)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string OrigenDatos { get; set; }

        /// <summary>
        /// Nombre del procedimiento almacenado (si aplica)
        /// </summary>
        [StringLength(200)]
        public string StoredProcedure { get; set; }

        #endregion

        #region Parámetros

        /// <summary>
        /// Definición de parámetros en formato JSON
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ParametrosJSON { get; set; }

        /// <summary>
        /// Indica si requiere parámetros
        /// </summary>
        public bool RequiereParametros { get; set; }

        /// <summary>
        /// Parámetros obligatorios
        /// </summary>
        [StringLength(500)]
        public string ParametrosObligatorios { get; set; }

        #endregion

        #region Formato y Presentación

        /// <summary>
        /// Formato de salida del reporte
        /// </summary>
        [Required]
        public FormatoReporte FormatoSalida { get; set; }

        /// <summary>
        /// Formatos adicionales permitidos (JSON array)
        /// </summary>
        [StringLength(200)]
        public string FormatosAdicionales { get; set; }

        /// <summary>
        /// Plantilla de presentación HTML
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string PlantillaHTML { get; set; }

        /// <summary>
        /// Configuración de columnas en formato JSON
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ConfiguracionColumnas { get; set; }

        /// <summary>
        /// Tiene gráficas
        /// </summary>
        public bool TieneGraficas { get; set; }

        /// <summary>
        /// Configuración de gráficas en formato JSON
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ConfiguracionGraficas { get; set; }

        #endregion

        #region Programación Automática

        /// <summary>
        /// Programación automática habilitada
        /// </summary>
        public bool ProgramacionAutomatica { get; set; }

        /// <summary>
        /// Frecuencia de ejecución
        /// </summary>
        public FrecuenciaReporte? Frecuencia { get; set; }

        /// <summary>
        /// Expresión CRON para programación
        /// </summary>
        [StringLength(100)]
        public string ExpresionCron { get; set; }

        /// <summary>
        /// Día del mes para ejecución (1-31)
        /// </summary>
        public int? DiaMes { get; set; }

        /// <summary>
        /// Día de la semana para ejecución (0-6, 0=Domingo)
        /// </summary>
        public int? DiaSemana { get; set; }

        /// <summary>
        /// Hora de ejecución (0-23)
        /// </summary>
        public int? HoraEjecucion { get; set; }

        /// <summary>
        /// Fecha de próxima ejecución
        /// </summary>
        public DateTime? ProximaEjecucion { get; set; }

        /// <summary>
        /// Fecha de última ejecución
        /// </summary>
        public DateTime? UltimaEjecucion { get; set; }

        #endregion

        #region Destinatarios

        /// <summary>
        /// Envío automático por correo
        /// </summary>
        public bool EnvioAutomatico { get; set; }

        /// <summary>
        /// Correos destinatarios (separados por coma)
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string CorreosDestinatarios { get; set; }

        /// <summary>
        /// Asunto del correo
        /// </summary>
        [StringLength(300)]
        public string AsuntoCorreo { get; set; }

        /// <summary>
        /// Cuerpo del correo
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string CuerpoCorreo { get; set; }

        #endregion

        #region Filtros y Ordenamiento

        /// <summary>
        /// Filtros predeterminados en formato JSON
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string FiltrosPredeterminados { get; set; }

        /// <summary>
        /// Ordenamiento predeterminado
        /// </summary>
        [StringLength(200)]
        public string OrdenamientoPredeterminado { get; set; }

        /// <summary>
        /// Permite ordenamiento dinámico
        /// </summary>
        public bool PermiteOrdenamientoDinamico { get; set; }

        /// <summary>
        /// Límite de registros (0 = sin límite)
        /// </summary>
        public int LimiteRegistros { get; set; }

        #endregion

        #region Caché y Rendimiento

        /// <summary>
        /// Habilitar caché de resultados
        /// </summary>
        public bool HabilitarCache { get; set; }

        /// <summary>
        /// Tiempo de caché en minutos
        /// </summary>
        public int? TiempoCacheMinutos { get; set; }

        /// <summary>
        /// Tiempo máximo de ejecución en segundos
        /// </summary>
        public int? TimeoutSegundos { get; set; }

        #endregion

        #region Seguridad y Permisos

        /// <summary>
        /// Es privado (solo creador puede verlo)
        /// </summary>
        public bool EsPrivado { get; set; }

        /// <summary>
        /// Roles que pueden ejecutar el reporte (JSON array)
        /// </summary>
        [StringLength(500)]
        public string RolesPermitidos { get; set; }

        /// <summary>
        /// Requiere aprobación para ejecutar
        /// </summary>
        public bool RequiereAprobacion { get; set; }

        #endregion

        #region Estado y Estadísticas

        /// <summary>
        /// Indica si está activo
        /// </summary>
        [Required]
        public bool Activo { get; set; }

        /// <summary>
        /// Indica si es reporte del sistema (no editable)
        /// </summary>
        public bool EsReporteSistema { get; set; }

        /// <summary>
        /// Cantidad de veces que se ha ejecutado
        /// </summary>
        public int VecesEjecutado { get; set; }

        /// <summary>
        /// Fecha de última ejecución manual
        /// </summary>
        public DateTime? FechaUltimaEjecucionManual { get; set; }

        /// <summary>
        /// Tiempo promedio de ejecución en segundos
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal? TiempoPromedioEjecucion { get; set; }

        #endregion

        #region Metadata

        /// <summary>
        /// Categoría del reporte
        /// </summary>
        [StringLength(100)]
        public string Categoria { get; set; }

        /// <summary>
        /// Tags o etiquetas
        /// </summary>
        [StringLength(500)]
        public string Tags { get; set; }

        /// <summary>
        /// Icono del reporte
        /// </summary>
        [StringLength(50)]
        public string Icono { get; set; }

        /// <summary>
        /// Orden de visualización
        /// </summary>
        public int Orden { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Observaciones { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Usuario que creó el reporte
        /// </summary>
        [ForeignKey("CreatedBy")]
        public virtual Usuario Creador { get; set; }

        #endregion

        #region Auditoría (IAuditableEntity)

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        #endregion

        #region Constructor

        public ReportePersonalizado()
        {
            OrigenDatos = "SQL";
            FormatoSalida = FormatoReporte.PDF;
            RequiereParametros = false;
            TieneGraficas = false;
            ProgramacionAutomatica = false;
            EnvioAutomatico = false;
            PermiteOrdenamientoDinamico = true;
            LimiteRegistros = 0;
            HabilitarCache = false;
            EsPrivado = false;
            RequiereAprobacion = false;
            Activo = true;
            EsReporteSistema = false;
            VecesEjecutado = 0;
            Orden = 0;
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si es académico
        /// </summary>
        public bool EsAcademico => TipoReporte == TipoReporte.Academico;

        /// <summary>
        /// Indica si es financiero
        /// </summary>
        public bool EsFinanciero => TipoReporte == TipoReporte.Financiero;

        /// <summary>
        /// Indica si está programado
        /// </summary>
        public bool EstaProgramado => ProgramacionAutomatica && ProximaEjecucion.HasValue;

        /// <summary>
        /// Indica si debe ejecutarse próximamente
        /// </summary>
        public bool DebeEjecutarse => EstaProgramado && ProximaEjecucion.Value <= DateTime.Now;

        /// <summary>
        /// Días hasta la próxima ejecución
        /// </summary>
        public int? DiasHastaProximaEjecucion
        {
            get
            {
                if (!ProximaEjecucion.HasValue) return null;
                return (ProximaEjecucion.Value.Date - DateTime.Now.Date).Days;
            }
        }

        /// <summary>
        /// Indica si ha sido ejecutado
        /// </summary>
        public bool HaSidoEjecutado => VecesEjecutado > 0;

        /// <summary>
        /// Indica si es poco usado
        /// </summary>
        public bool EsPocoUsado => VecesEjecutado < 5;

        /// <summary>
        /// Indica si usa SQL directo
        /// </summary>
        public bool UsaSQLDirecto => OrigenDatos == "SQL" && !string.IsNullOrWhiteSpace(ConsultaSQL);

        /// <summary>
        /// Indica si usa stored procedure
        /// </summary>
        public bool UsaStoredProcedure => OrigenDatos == "StoredProcedure" && !string.IsNullOrWhiteSpace(StoredProcedure);

        /// <summary>
        /// Indica si tiene caché habilitado y configurado
        /// </summary>
        public bool TieneCacheConfigurado => HabilitarCache && TiempoCacheMinutos.HasValue && TiempoCacheMinutos > 0;

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Registra una ejecución del reporte
        /// </summary>
        public void RegistrarEjecucion(decimal tiempoEjecucionSegundos, bool esAutomatica = false)
        {
            VecesEjecutado++;

            if (esAutomatica)
                UltimaEjecucion = DateTime.Now;
            else
                FechaUltimaEjecucionManual = DateTime.Now;

            // Calcular tiempo promedio
            if (TiempoPromedioEjecucion.HasValue)
                TiempoPromedioEjecucion = ((TiempoPromedioEjecucion.Value * (VecesEjecutado - 1)) + tiempoEjecucionSegundos) / VecesEjecutado;
            else
                TiempoPromedioEjecucion = tiempoEjecucionSegundos;

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Programa la próxima ejecución automática
        /// </summary>
        public void ProgramarProximaEjecucion()
        {
            if (!ProgramacionAutomatica || !Frecuencia.HasValue)
                throw new InvalidOperationException("El reporte no tiene programación automática habilitada");

            var ahora = DateTime.Now;
            DateTime proximaFecha;

            switch (Frecuencia.Value)
            {
                case FrecuenciaReporte.Diaria:
                    proximaFecha = ahora.Date.AddDays(1);
                    if (HoraEjecucion.HasValue)
                        proximaFecha = proximaFecha.AddHours(HoraEjecucion.Value);
                    break;

                case FrecuenciaReporte.Semanal:
                    if (!DiaSemana.HasValue)
                        throw new InvalidOperationException("Debe especificar el día de la semana");
                    proximaFecha = ahora.Date.AddDays(1);
                    while ((int)proximaFecha.DayOfWeek != DiaSemana.Value)
                        proximaFecha = proximaFecha.AddDays(1);
                    if (HoraEjecucion.HasValue)
                        proximaFecha = proximaFecha.AddHours(HoraEjecucion.Value);
                    break;

                case FrecuenciaReporte.Mensual:
                    if (!DiaMes.HasValue)
                        throw new InvalidOperationException("Debe especificar el día del mes");
                    proximaFecha = new DateTime(ahora.Year, ahora.Month, Math.Min(DiaMes.Value, DateTime.DaysInMonth(ahora.Year, ahora.Month)));
                    if (proximaFecha <= ahora)
                        proximaFecha = proximaFecha.AddMonths(1);
                    if (HoraEjecucion.HasValue)
                        proximaFecha = proximaFecha.AddHours(HoraEjecucion.Value);
                    break;

                default:
                    throw new NotImplementedException($"Frecuencia {Frecuencia} no implementada");
            }

            ProximaEjecucion = proximaFecha;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Configura la programación automática
        /// </summary>
        public void ConfigurarProgramacion(FrecuenciaReporte frecuencia, int? horaEjecucion = null,
                                          int? diaSemana = null, int? diaMes = null)
        {
            ProgramacionAutomatica = true;
            Frecuencia = frecuencia;
            HoraEjecucion = horaEjecucion ?? 8;

            if (frecuencia == FrecuenciaReporte.Semanal && !diaSemana.HasValue)
                throw new ArgumentException("Debe especificar el día de la semana para reportes semanales");

            if (frecuencia == FrecuenciaReporte.Mensual && !diaMes.HasValue)
                throw new ArgumentException("Debe especificar el día del mes para reportes mensuales");

            DiaSemana = diaSemana;
            DiaMes = diaMes;

            ProgramarProximaEjecucion();
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Desactiva la programación automática
        /// </summary>
        public void DesactivarProgramacion()
        {
            ProgramacionAutomatica = false;
            Frecuencia = null;
            DiaSemana = null;
            DiaMes = null;
            HoraEjecucion = null;
            ProximaEjecucion = null;
            ExpresionCron = null;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Configura el envío automático
        /// </summary>
        public void ConfigurarEnvioAutomatico(string correos, string asunto, string cuerpo)
        {
            if (string.IsNullOrWhiteSpace(correos))
                throw new ArgumentException("Los correos son requeridos");

            EnvioAutomatico = true;
            CorreosDestinatarios = correos;
            AsuntoCorreo = asunto;
            CuerpoCorreo = cuerpo;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Desactiva el envío automático
        /// </summary>
        public void DesactivarEnvioAutomatico()
        {
            EnvioAutomatico = false;
            CorreosDestinatarios = null;
            AsuntoCorreo = null;
            CuerpoCorreo = null;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Configura el caché
        /// </summary>
        public void ConfigurarCache(bool habilitar, int? tiempoMinutos = null)
        {
            HabilitarCache = habilitar;

            if (habilitar)
            {
                TiempoCacheMinutos = tiempoMinutos ?? 60;
            }
            else
            {
                TiempoCacheMinutos = null;
            }

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Activa el reporte
        /// </summary>
        public void Activar()
        {
            if (!Activo)
            {
                Activo = true;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Desactiva el reporte
        /// </summary>
        public void Desactivar()
        {
            if (Activo)
            {
                Activo = false;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Marca como reporte del sistema
        /// </summary>
        public void MarcarComoReporteSistema()
        {
            EsReporteSistema = true;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Duplica el reporte
        /// </summary>
        public ReportePersonalizado Duplicar(string nuevoNombre)
        {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                throw new ArgumentException("El nuevo nombre es requerido");

            return new ReportePersonalizado
            {
                EscuelaId = this.EscuelaId,
                Nombre = nuevoNombre,
                Descripcion = this.Descripcion,
                TipoReporte = this.TipoReporte,
                ConsultaSQL = this.ConsultaSQL,
                ConfiguracionJSON = this.ConfiguracionJSON,
                OrigenDatos = this.OrigenDatos,
                StoredProcedure = this.StoredProcedure,
                ParametrosJSON = this.ParametrosJSON,
                RequiereParametros = this.RequiereParametros,
                ParametrosObligatorios = this.ParametrosObligatorios,
                FormatoSalida = this.FormatoSalida,
                FormatosAdicionales = this.FormatosAdicionales,
                PlantillaHTML = this.PlantillaHTML,
                ConfiguracionColumnas = this.ConfiguracionColumnas,
                TieneGraficas = this.TieneGraficas,
                ConfiguracionGraficas = this.ConfiguracionGraficas,
                FiltrosPredeterminados = this.FiltrosPredeterminados,
                OrdenamientoPredeterminado = this.OrdenamientoPredeterminado,
                PermiteOrdenamientoDinamico = this.PermiteOrdenamientoDinamico,
                LimiteRegistros = this.LimiteRegistros,
                HabilitarCache = this.HabilitarCache,
                TiempoCacheMinutos = this.TiempoCacheMinutos,
                TimeoutSegundos = this.TimeoutSegundos,
                Categoria = this.Categoria,
                Tags = this.Tags,
                Icono = this.Icono,
                Activo = true,
                EsReporteSistema = false,
                EsPrivado = false
            };
        }

        /// <summary>
        /// Valida que el reporte sea correcto
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Nombre))
                errores.Add("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(OrigenDatos))
                errores.Add("El origen de datos es requerido");

            if (UsaSQLDirecto && string.IsNullOrWhiteSpace(ConsultaSQL))
                errores.Add("Si el origen es SQL, debe proporcionar la consulta");

            if (UsaStoredProcedure && string.IsNullOrWhiteSpace(StoredProcedure))
                errores.Add("Si el origen es StoredProcedure, debe proporcionar el nombre del procedimiento");

            if (ProgramacionAutomatica)
            {
                if (!Frecuencia.HasValue)
                    errores.Add("Si tiene programación automática, debe especificar la frecuencia");

                if (Frecuencia == FrecuenciaReporte.Semanal && !DiaSemana.HasValue)
                    errores.Add("Los reportes semanales requieren especificar el día de la semana");

                if (Frecuencia == FrecuenciaReporte.Mensual && !DiaMes.HasValue)
                    errores.Add("Los reportes mensuales requieren especificar el día del mes");

                if (DiaMes.HasValue && (DiaMes < 1 || DiaMes > 31))
                    errores.Add("El día del mes debe estar entre 1 y 31");

                if (DiaSemana.HasValue && (DiaSemana < 0 || DiaSemana > 6))
                    errores.Add("El día de la semana debe estar entre 0 y 6");

                if (HoraEjecucion.HasValue && (HoraEjecucion < 0 || HoraEjecucion > 23))
                    errores.Add("La hora de ejecución debe estar entre 0 y 23");
            }

            if (EnvioAutomatico)
            {
                if (string.IsNullOrWhiteSpace(CorreosDestinatarios))
                    errores.Add("Si tiene envío automático, debe especificar los correos destinatarios");
            }

            if (HabilitarCache && (!TiempoCacheMinutos.HasValue || TiempoCacheMinutos <= 0))
                errores.Add("Si el caché está habilitado, debe especificar el tiempo de caché en minutos");

            if (LimiteRegistros < 0)
                errores.Add("El límite de registros no puede ser negativo");

            if (TimeoutSegundos.HasValue && TimeoutSegundos <= 0)
                errores.Add("El timeout debe ser mayor a cero");

            return errores;
        }

        #endregion
    }
}