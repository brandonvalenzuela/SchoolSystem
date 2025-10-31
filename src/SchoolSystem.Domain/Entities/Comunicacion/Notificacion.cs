using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Enums.Comunicacion;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Domain.Entities.Comunicacion
{
    /// <summary>
    /// Notificaciones enviadas a usuarios del sistema
    /// </summary>
    [Table("Notificaciones")]
    public class Notificacion : BaseEntity, IAuditableEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// Usuario destinatario de la notificación
        /// </summary>
        public int? UsuarioDestinatarioId { get; set; }

        /// <summary>
        /// Tipo de notificación
        /// </summary>
        [Required]
        public TipoNotificacion Tipo { get; set; }

        /// <summary>
        /// Prioridad de la notificación
        /// </summary>
        [Required]
        public PrioridadNotificacion Prioridad { get; set; }

        /// <summary>
        /// Título de la notificación
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        /// <summary>
        /// Mensaje completo de la notificación
        /// </summary>
        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Mensaje { get; set; }

        /// <summary>
        /// URL a la que redirige la notificación (opcional)
        /// </summary>
        [StringLength(500)]
        public string UrlAccion { get; set; }

        /// <summary>
        /// Usuario que envió la notificación
        /// </summary>
        [Required]
        public int EnviadoPorId { get; set; }

        #endregion

        #region Fechas

        /// <summary>
        /// Fecha y hora de envío
        /// </summary>
        [Required]
        public DateTime FechaEnvio { get; set; }

        /// <summary>
        /// Fecha programada para envío (si aplica)
        /// </summary>
        public DateTime? FechaProgramada { get; set; }

        /// <summary>
        /// Fecha en que se leyó la notificación
        /// </summary>
        public DateTime? FechaLectura { get; set; }

        #endregion

        #region Estado

        /// <summary>
        /// Indica si la notificación fue leída
        /// </summary>
        [Required]
        public bool Leida { get; set; }

        /// <summary>
        /// Canal por el que se envió
        /// </summary>
        [Required]
        public CanalNotificacion Canal { get; set; }

        #endregion

        #region Metadata

        /// <summary>
        /// Información adicional en formato JSON
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Metadata { get; set; }

        /// <summary>
        /// Icono de la notificación (nombre o URL)
        /// </summary>
        [StringLength(200)]
        public string Icono { get; set; }

        /// <summary>
        /// Color de la notificación (para UI)
        /// </summary>
        [StringLength(20)]
        public string Color { get; set; }

        /// <summary>
        /// Indica si se debe reproducir sonido
        /// </summary>
        public bool ReproducirSonido { get; set; }

        /// <summary>
        /// Indica si la notificación expira
        /// </summary>
        public DateTime? FechaExpiracion { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Usuario destinatario
        /// </summary>
        [ForeignKey("UsuarioDestinatarioId")]
        public virtual Usuario UsuarioDestinatario { get; set; }

        /// <summary>
        /// Usuario que envió
        /// </summary>
        [ForeignKey("EnviadoPorId")]
        public virtual Usuario EnviadoPor { get; set; }

        #endregion

        #region Auditoría (IAuditableEntity)

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        #endregion

        #region Constructor

        public Notificacion()
        {
            FechaEnvio = DateTime.Now;
            Leida = false;
            Prioridad = PrioridadNotificacion.Normal;
            Canal = CanalNotificacion.Sistema;
            ReproducirSonido = false;
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si la notificación está pendiente de leer
        /// </summary>
        public bool PendienteLectura => !Leida;

        /// <summary>
        /// Indica si la notificación es urgente
        /// </summary>
        public bool EsUrgente => Prioridad == PrioridadNotificacion.Urgente;

        /// <summary>
        /// Indica si la notificación está expirada
        /// </summary>
        public bool EstaExpirada => FechaExpiracion.HasValue && DateTime.Now > FechaExpiracion.Value;

        /// <summary>
        /// Indica si la notificación está programada para el futuro
        /// </summary>
        public bool EstaProgramada => FechaProgramada.HasValue && FechaProgramada.Value > DateTime.Now;

        /// <summary>
        /// Tiempo transcurrido desde el envío
        /// </summary>
        public TimeSpan TiempoDesdeEnvio => DateTime.Now - FechaEnvio;

        /// <summary>
        /// Minutos desde el envío
        /// </summary>
        public int MinutosDesdeEnvio => (int)TiempoDesdeEnvio.TotalMinutes;

        /// <summary>
        /// Horas desde el envío
        /// </summary>
        public int HorasDesdeEnvio => (int)TiempoDesdeEnvio.TotalHours;

        /// <summary>
        /// Días desde el envío
        /// </summary>
        public int DiasDesdeEnvio => (int)TiempoDesdeEnvio.TotalDays;

        /// <summary>
        /// Descripción del tiempo transcurrido
        /// </summary>
        public string TiempoTranscurridoTexto
        {
            get
            {
                var minutos = MinutosDesdeEnvio;
                if (minutos < 1) return "Hace un momento";
                if (minutos < 60) return $"Hace {minutos} minuto{(minutos > 1 ? "s" : "")}";

                var horas = HorasDesdeEnvio;
                if (horas < 24) return $"Hace {horas} hora{(horas > 1 ? "s" : "")}";

                var dias = DiasDesdeEnvio;
                if (dias < 7) return $"Hace {dias} día{(dias > 1 ? "s" : "")}";

                var semanas = dias / 7;
                if (semanas < 4) return $"Hace {semanas} semana{(semanas > 1 ? "s" : "")}";

                var meses = dias / 30;
                return $"Hace {meses} mes{(meses > 1 ? "es" : "")}";
            }
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Marca la notificación como leída
        /// </summary>
        public void MarcarComoLeida()
        {
            if (!Leida)
            {
                Leida = true;
                FechaLectura = DateTime.Now;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Marca la notificación como no leída
        /// </summary>
        public void MarcarComoNoLeida()
        {
            if (Leida)
            {
                Leida = false;
                FechaLectura = null;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Programa la notificación para envío futuro
        /// </summary>
        public void Programar(DateTime fechaProgramada)
        {
            if (fechaProgramada <= DateTime.Now)
                throw new InvalidOperationException("La fecha programada debe ser futura");

            FechaProgramada = fechaProgramada;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Cancela la programación
        /// </summary>
        public void CancelarProgramacion()
        {
            FechaProgramada = null;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Establece una fecha de expiración
        /// </summary>
        public void EstablecerExpiracion(DateTime fechaExpiracion)
        {
            if (fechaExpiracion <= DateTime.Now)
                throw new InvalidOperationException("La fecha de expiración debe ser futura");

            FechaExpiracion = fechaExpiracion;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Verifica si la notificación debe ser enviada ahora
        /// </summary>
        public bool DebeEnviarse()
        {
            if (EstaExpirada) return false;
            if (!EstaProgramada) return true;
            return FechaProgramada.HasValue && DateTime.Now >= FechaProgramada.Value;
        }

        /// <summary>
        /// Actualiza la metadata de la notificación
        /// </summary>
        public void ActualizarMetadata(string metadata)
        {
            Metadata = metadata;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Establece la apariencia de la notificación
        /// </summary>
        public void EstablecerApariencia(string icono, string color, bool reproducirSonido = false)
        {
            Icono = icono;
            Color = color;
            ReproducirSonido = reproducirSonido;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Crea una copia de la notificación para reenviar
        /// </summary>
        public Notificacion Clonar()
        {
            return new Notificacion
            {
                EscuelaId = this.EscuelaId,
                UsuarioDestinatarioId = this.UsuarioDestinatarioId,
                Tipo = this.Tipo,
                Prioridad = this.Prioridad,
                Titulo = this.Titulo,
                Mensaje = this.Mensaje,
                UrlAccion = this.UrlAccion,
                EnviadoPorId = this.EnviadoPorId,
                Canal = this.Canal,
                Metadata = this.Metadata,
                Icono = this.Icono,
                Color = this.Color,
                ReproducirSonido = this.ReproducirSonido
            };
        }

        /// <summary>
        /// Valida que la notificación sea correcta
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Titulo))
                errores.Add("El título es requerido");

            if (string.IsNullOrWhiteSpace(Mensaje))
                errores.Add("El mensaje es requerido");

            if (Titulo?.Length > 200)
                errores.Add("El título no puede exceder 200 caracteres");

            if (FechaProgramada.HasValue && FechaProgramada.Value <= DateTime.Now)
                errores.Add("La fecha programada debe ser futura");

            if (FechaExpiracion.HasValue && FechaExpiracion.Value <= DateTime.Now)
                errores.Add("La fecha de expiración debe ser futura");

            if (FechaExpiracion.HasValue && FechaProgramada.HasValue && FechaExpiracion.Value <= FechaProgramada.Value)
                errores.Add("La fecha de expiración debe ser posterior a la fecha programada");

            return errores;
        }

        #endregion
    }
}