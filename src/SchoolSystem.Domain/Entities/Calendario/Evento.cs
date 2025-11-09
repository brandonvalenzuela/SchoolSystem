using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Enums.Calendario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SchoolSystem.Domain.Entities.Calendario
{
    /// <summary>
    /// Eventos del calendario escolar
    /// </summary>
    [Table("Eventos")]
    public class Evento : BaseEntity, IAuditableEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// Título del evento
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        /// <summary>
        /// Descripción del evento
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Tipo de evento
        /// </summary>
        [Required]
        public TipoEvento Tipo { get; set; }

        #endregion

        #region Fechas

        /// <summary>
        /// Fecha y hora de inicio del evento
        /// </summary>
        [Required]
        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// Fecha y hora de fin del evento
        /// </summary>
        public DateTime? FechaFin { get; set; }

        /// <summary>
        /// Indica si el evento dura todo el día
        /// </summary>
        [Required]
        public bool TodoElDia { get; set; }

        #endregion

        #region Destinatarios

        /// <summary>
        /// IDs de grupos afectados en formato JSON (null = todos los grupos)
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string GruposAfectadosJson { get; set; }

        /// <summary>
        /// Indica si aplica a toda la escuela
        /// </summary>
        [Required]
        public bool AplicaATodos { get; set; }

        #endregion

        #region Detalles

        /// <summary>
        /// Ubicación del evento
        /// </summary>
        [StringLength(200)]
        public string Ubicacion { get; set; }

        /// <summary>
        /// Minutos antes del evento para enviar recordatorio
        /// </summary>
        public int? RecordatorioMinutos { get; set; }

        /// <summary>
        /// Color del evento (para calendario visual)
        /// </summary>
        [StringLength(20)]
        public string Color { get; set; }

        /// <summary>
        /// Prioridad del evento
        /// </summary>
        public PrioridadEvento Prioridad { get; set; }

        #endregion

        #region Configuración

        /// <summary>
        /// Usuario que creó el evento
        /// </summary>
        [Required]
        public int CreadoPorId { get; set; }

        /// <summary>
        /// Indica si el evento está activo
        /// </summary>
        [Required]
        public bool Activo { get; set; }

        /// <summary>
        /// Indica si se enviaron recordatorios
        /// </summary>
        public bool RecordatoriosEnviados { get; set; }

        /// <summary>
        /// Fecha en que se enviaron los recordatorios
        /// </summary>
        public DateTime? FechaEnvioRecordatorios { get; set; }

        /// <summary>
        /// Indica si el evento se repite
        /// </summary>
        public bool EsRecurrente { get; set; }

        /// <summary>
        /// Configuración de recurrencia en formato JSON
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ConfiguracionRecurrencia { get; set; }

        #endregion

        #region Archivos

        /// <summary>
        /// URL de archivo adjunto (agenda, documento, etc.)
        /// </summary>
        [StringLength(500)]
        public string ArchivoAdjuntoUrl { get; set; }

        /// <summary>
        /// Nombre del archivo adjunto
        /// </summary>
        [StringLength(200)]
        public string ArchivoAdjuntoNombre { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Usuario que creó el evento
        /// </summary>
        [ForeignKey("CreadoPorId")]
        public virtual Usuario CreadoPor { get; set; }

        #endregion

        #region Auditoría (IAuditableEntity)

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        #endregion

        #region Constructor

        public Evento()
        {
            TodoElDia = false;
            AplicaATodos = true;
            Activo = true;
            RecordatoriosEnviados = false;
            EsRecurrente = false;
            Prioridad = PrioridadEvento.Normal;
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si el evento ya pasó
        /// </summary>
        public bool YaPaso
        {
            get
            {
                var fechaComparacion = FechaFin ?? FechaInicio;
                return DateTime.Now > fechaComparacion;
            }
        }

        /// <summary>
        /// Indica si el evento está en curso
        /// </summary>
        public bool EnCurso
        {
            get
            {
                if (!FechaFin.HasValue) return false;
                return DateTime.Now >= FechaInicio && DateTime.Now <= FechaFin.Value;
            }
        }

        /// <summary>
        /// Indica si el evento es próximo (dentro de las próximas 24 horas)
        /// </summary>
        public bool EsProximo
        {
            get
            {
                if (YaPaso) return false;
                var horasHastaEvento = (FechaInicio - DateTime.Now).TotalHours;
                return horasHastaEvento <= 24 && horasHastaEvento > 0;
            }
        }

        /// <summary>
        /// Indica si el evento es hoy
        /// </summary>
        public bool EsHoy => FechaInicio.Date == DateTime.Now.Date;

        /// <summary>
        /// Duración del evento en horas
        /// </summary>
        public double? DuracionHoras
        {
            get
            {
                if (!FechaFin.HasValue) return null;
                return (FechaFin.Value - FechaInicio).TotalHours;
            }
        }

        /// <summary>
        /// Duración del evento en días
        /// </summary>
        public int? DuracionDias
        {
            get
            {
                if (!FechaFin.HasValue) return null;
                return (FechaFin.Value.Date - FechaInicio.Date).Days + 1;
            }
        }

        /// <summary>
        /// Días hasta el evento (negativo si ya pasó)
        /// </summary>
        public int DiasHastaEvento => (FechaInicio.Date - DateTime.Now.Date).Days;

        /// <summary>
        /// Horas hasta el evento
        /// </summary>
        public double HorasHastaEvento => (FechaInicio - DateTime.Now).TotalHours;

        /// <summary>
        /// Lista de IDs de grupos afectados
        /// </summary>
        public List<int> GruposAfectados
        {
            get
            {
                if (string.IsNullOrWhiteSpace(GruposAfectadosJson))
                    return new List<int>();

                try
                {
                    return System.Text.Json.JsonSerializer.Deserialize<List<int>>(GruposAfectadosJson);
                }
                catch
                {
                    return new List<int>();
                }
            }
            set
            {
                GruposAfectadosJson = value != null && value.Any()
                    ? System.Text.Json.JsonSerializer.Serialize(value)
                    : null;
            }
        }

        /// <summary>
        /// Indica si tiene archivo adjunto
        /// </summary>
        public bool TieneArchivo => !string.IsNullOrWhiteSpace(ArchivoAdjuntoUrl);

        /// <summary>
        /// Indica si debe enviar recordatorios
        /// </summary>
        public bool DebeEnviarRecordatorios => RecordatorioMinutos.HasValue && !RecordatoriosEnviados && Activo;

        /// <summary>
        /// Fecha y hora en que se debe enviar el recordatorio
        /// </summary>
        public DateTime? FechaEnvioRecordatorio
        {
            get
            {
                if (!RecordatorioMinutos.HasValue) return null;
                return FechaInicio.AddMinutes(-RecordatorioMinutos.Value);
            }
        }

        /// <summary>
        /// Indica si es momento de enviar recordatorios
        /// </summary>
        public bool EsMomentoDeRecordar
        {
            get
            {
                if (!DebeEnviarRecordatorios || !FechaEnvioRecordatorio.HasValue) return false;
                return DateTime.Now >= FechaEnvioRecordatorio.Value;
            }
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Establece los grupos afectados por el evento
        /// </summary>
        public void EstablecerGruposAfectados(List<int> gruposIds)
        {
            if (gruposIds == null || !gruposIds.Any())
            {
                AplicaATodos = true;
                GruposAfectados = new List<int>();
            }
            else
            {
                AplicaATodos = false;
                GruposAfectados = gruposIds;
            }
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Marca el evento para todos los grupos
        /// </summary>
        public void AplicarATodosLosGrupos()
        {
            AplicaATodos = true;
            GruposAfectados = new List<int>();
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Agrega un grupo a los afectados
        /// </summary>
        public void AgregarGrupo(int grupoId)
        {
            var grupos = GruposAfectados;
            if (!grupos.Contains(grupoId))
            {
                grupos.Add(grupoId);
                GruposAfectados = grupos;
                AplicaATodos = false;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Remueve un grupo de los afectados
        /// </summary>
        public void RemoverGrupo(int grupoId)
        {
            var grupos = GruposAfectados;
            if (grupos.Remove(grupoId))
            {
                GruposAfectados = grupos;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Establece el recordatorio
        /// </summary>
        public void EstablecerRecordatorio(int minutos)
        {
            if (minutos < 0)
                throw new ArgumentException("Los minutos deben ser positivos");

            RecordatorioMinutos = minutos;
            RecordatoriosEnviados = false;
            FechaEnvioRecordatorios = null;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Remueve el recordatorio
        /// </summary>
        public void RemoverRecordatorio()
        {
            RecordatorioMinutos = null;
            RecordatoriosEnviados = false;
            FechaEnvioRecordatorios = null;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Marca los recordatorios como enviados
        /// </summary>
        public void MarcarRecordatoriosEnviados()
        {
            RecordatoriosEnviados = true;
            FechaEnvioRecordatorios = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Extiende la fecha de fin del evento
        /// </summary>
        public void ExtenderFechaFin(DateTime nuevaFechaFin)
        {
            if (nuevaFechaFin < FechaInicio)
                throw new InvalidOperationException("La fecha de fin debe ser posterior a la fecha de inicio");

            FechaFin = nuevaFechaFin;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Cambia las fechas del evento
        /// </summary>
        public void CambiarFechas(DateTime nuevaFechaInicio, DateTime? nuevaFechaFin = null)
        {
            if (nuevaFechaFin.HasValue && nuevaFechaFin.Value < nuevaFechaInicio)
                throw new InvalidOperationException("La fecha de fin debe ser posterior a la fecha de inicio");

            FechaInicio = nuevaFechaInicio;
            FechaFin = nuevaFechaFin;

            // Resetear recordatorios si se cambian las fechas
            if (RecordatoriosEnviados)
            {
                RecordatoriosEnviados = false;
                FechaEnvioRecordatorios = null;
            }

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Marca el evento como todo el día
        /// </summary>
        public void MarcarComoTodoElDia()
        {
            TodoElDia = true;
            // Ajustar a inicio y fin de día
            FechaInicio = FechaInicio.Date;
            if (FechaFin.HasValue)
            {
                FechaFin = FechaFin.Value.Date.AddDays(1).AddSeconds(-1);
            }
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Desmarca el evento como todo el día
        /// </summary>
        public void DesmarcarComoTodoElDia()
        {
            TodoElDia = false;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Cancela el evento
        /// </summary>
        public void Cancelar()
        {
            Activo = false;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Reactiva el evento
        /// </summary>
        public void Reactivar()
        {
            Activo = true;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Adjunta un archivo al evento
        /// </summary>
        public void AdjuntarArchivo(string url, string nombreArchivo)
        {
            ArchivoAdjuntoUrl = url;
            ArchivoAdjuntoNombre = nombreArchivo;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Remueve el archivo adjunto
        /// </summary>
        public void RemoverArchivo()
        {
            ArchivoAdjuntoUrl = null;
            ArchivoAdjuntoNombre = null;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Establece la configuración de recurrencia
        /// </summary>
        public void EstablecerRecurrencia(string configuracionJson)
        {
            EsRecurrente = true;
            ConfiguracionRecurrencia = configuracionJson;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Remueve la recurrencia
        /// </summary>
        public void RemoverRecurrencia()
        {
            EsRecurrente = false;
            ConfiguracionRecurrencia = null;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Verifica si un grupo está afectado por este evento
        /// </summary>
        public bool AfectaAlGrupo(int grupoId)
        {
            if (AplicaATodos) return true;
            return GruposAfectados.Contains(grupoId);
        }

        /// <summary>
        /// Valida que el evento sea correcto
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Titulo))
                errores.Add("El título es requerido");

            if (Titulo?.Length > 200)
                errores.Add("El título no puede exceder 200 caracteres");

            if (FechaFin.HasValue && FechaFin.Value < FechaInicio)
                errores.Add("La fecha de fin debe ser posterior a la fecha de inicio");

            if (RecordatorioMinutos.HasValue && RecordatorioMinutos.Value < 0)
                errores.Add("Los minutos de recordatorio deben ser positivos");

            if (!AplicaATodos && !GruposAfectados.Any())
                errores.Add("Debe especificar al menos un grupo o marcar como aplicable a todos");

            return errores;
        }

        #endregion
    }
}