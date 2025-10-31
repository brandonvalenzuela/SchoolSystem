using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Enums.Conducta;
using System;

namespace SchoolSystem.Domain.Entities.Conducta
{
    /// <summary>
    /// Entidad Sancion - Representa una sanción disciplinaria aplicada a un alumno
    /// </summary>
    public class Sancion : BaseEntity, IAuditableEntity
    {
        #region Propiedades de la Escuela (Multi-tenant)

        /// <summary>
        /// ID de la escuela a la que pertenece
        /// </summary>
        public int EscuelaId { get; set; }

        /// <summary>
        /// Escuela asociada (Navigation Property)
        /// </summary>
        public virtual Escuelas.Escuela Escuela { get; set; }

        #endregion

        #region Relaciones Principales

        /// <summary>
        /// ID del alumno sancionado
        /// </summary>
        public int AlumnoId { get; set; }

        /// <summary>
        /// Alumno (Navigation Property)
        /// </summary>
        public virtual Academico.Alumno Alumno { get; set; }

        /// <summary>
        /// ID del registro de conducta que originó la sanción (opcional)
        /// </summary>
        public int? ConductaId { get; set; }

        /// <summary>
        /// Registro de conducta (Navigation Property)
        /// </summary>
        public virtual RegistroConducta Conducta { get; set; }

        #endregion

        #region Datos de la Sanción

        /// <summary>
        /// Tipo de sanción aplicada
        /// </summary>
        public TipoSancion Tipo { get; set; }

        /// <summary>
        /// Fecha de inicio de la sanción
        /// </summary>
        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// Fecha de fin de la sanción (si aplica)
        /// </summary>
        public DateTime? FechaFin { get; set; }

        /// <summary>
        /// Descripción detallada de la sanción
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Motivo o justificación de la sanción
        /// </summary>
        public string Motivo { get; set; }

        #endregion

        #region Autorización

        /// <summary>
        /// ID del usuario que autorizó la sanción (director, subdirector)
        /// </summary>
        public int AutorizadoPor { get; set; }

        /// <summary>
        /// Usuario que autorizó (Navigation Property)
        /// </summary>
        public virtual Usuarios.Usuario UsuarioAutorizo { get; set; }

        /// <summary>
        /// Fecha y hora de autorización
        /// </summary>
        public DateTime FechaAutorizacion { get; set; }

        #endregion

        #region Estado de Cumplimiento

        /// <summary>
        /// Indica si la sanción ha sido cumplida
        /// </summary>
        public bool Cumplida { get; set; }

        /// <summary>
        /// Fecha en que se cumplió la sanción
        /// </summary>
        public DateTime? FechaCumplimiento { get; set; }

        /// <summary>
        /// Observaciones sobre el cumplimiento
        /// </summary>
        public string ObservacionesCumplimiento { get; set; }

        /// <summary>
        /// ID del usuario que verificó el cumplimiento
        /// </summary>
        public int? VerificadoPor { get; set; }

        #endregion

        #region Apelación

        /// <summary>
        /// Indica si se presentó una apelación
        /// </summary>
        public bool Apelada { get; set; }

        /// <summary>
        /// Fecha de la apelación
        /// </summary>
        public DateTime? FechaApelacion { get; set; }

        /// <summary>
        /// Motivo de la apelación
        /// </summary>
        public string MotivoApelacion { get; set; }

        /// <summary>
        /// Resultado de la apelación
        /// Ejemplo: "Aceptada", "Rechazada", "Modificada"
        /// </summary>
        public string ResultadoApelacion { get; set; }

        /// <summary>
        /// Fecha de resolución de la apelación
        /// </summary>
        public DateTime? FechaResolucionApelacion { get; set; }

        #endregion

        #region Notificación

        /// <summary>
        /// Indica si se notificó a los padres
        /// </summary>
        public bool PadresNotificados { get; set; }

        /// <summary>
        /// Fecha de notificación a los padres
        /// </summary>
        public DateTime? FechaNotificacionPadres { get; set; }

        /// <summary>
        /// Medio de notificación
        /// </summary>
        public string MedioNotificacion { get; set; }

        /// <summary>
        /// Indica si los padres firmaron el enterado
        /// </summary>
        public bool FirmaEnterado { get; set; }

        /// <summary>
        /// Fecha en que firmaron el enterado
        /// </summary>
        public DateTime? FechaFirmaEnterado { get; set; }

        #endregion

        #region Documentación

        /// <summary>
        /// URL del documento de la sanción (PDF, imagen)
        /// </summary>
        public string DocumentoUrl { get; set; }

        /// <summary>
        /// Observaciones adicionales
        /// </summary>
        public string Observaciones { get; set; }

        #endregion

        #region Propiedades de Auditoría (IAuditableEntity)

        /// <summary>
        /// Fecha y hora de creación del registro
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Fecha y hora de la última actualización
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// ID del usuario que creó el registro
        /// </summary>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// ID del usuario que realizó la última actualización
        /// </summary>
        public int? UpdatedBy { get; set; }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si la sanción está activa (dentro del período)
        /// </summary>
        public bool EstaActiva
        {
            get
            {
                var hoy = DateTime.Today;

                if (Cumplida)
                    return false;

                if (hoy < FechaInicio.Date)
                    return false;

                if (FechaFin.HasValue && hoy > FechaFin.Value.Date)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Indica si la sanción ha vencido
        /// </summary>
        public bool HaVencido => FechaFin.HasValue && DateTime.Today > FechaFin.Value.Date;

        /// <summary>
        /// Días restantes de la sanción
        /// </summary>
        public int? DiasRestantes
        {
            get
            {
                if (!FechaFin.HasValue || Cumplida || HaVencido)
                    return null;

                return (FechaFin.Value.Date - DateTime.Today).Days;
            }
        }

        /// <summary>
        /// Duración total de la sanción en días
        /// </summary>
        public int? DuracionDias
        {
            get
            {
                if (!FechaFin.HasValue)
                    return null;

                return (FechaFin.Value.Date - FechaInicio.Date).Days + 1;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Sancion()
        {
            FechaInicio = DateTime.Today;
            FechaAutorizacion = DateTime.Now;
            Cumplida = false;
            Apelada = false;
            PadresNotificados = false;
            FirmaEnterado = false;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Marca la sanción como cumplida
        /// </summary>
        public void MarcarComoCumplida(string observaciones, int verificadoPor)
        {
            Cumplida = true;
            FechaCumplimiento = DateTime.Now;
            ObservacionesCumplimiento = observaciones;
            VerificadoPor = verificadoPor;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Registra la notificación a los padres
        /// </summary>
        public void NotificarPadres(string medio, int usuarioId)
        {
            PadresNotificados = true;
            FechaNotificacionPadres = DateTime.Now;
            MedioNotificacion = medio;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Registra la firma de enterado de los padres
        /// </summary>
        public void RegistrarFirmaEnterado(int usuarioId)
        {
            FirmaEnterado = true;
            FechaFirmaEnterado = DateTime.Now;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Presenta una apelación
        /// </summary>
        public void PresentarApelacion(string motivo, int usuarioId)
        {
            Apelada = true;
            FechaApelacion = DateTime.Now;
            MotivoApelacion = motivo;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Resuelve la apelación
        /// </summary>
        public void ResolverApelacion(string resultado, int usuarioId)
        {
            ResultadoApelacion = resultado;
            FechaResolucionApelacion = DateTime.Now;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;

            // Si la apelación fue aceptada, cancelar la sanción
            if (resultado?.ToLower().Contains("aceptada") == true)
            {
                Cumplida = true;
                FechaCumplimiento = DateTime.Now;
                ObservacionesCumplimiento = "Cancelada por apelación aceptada";
            }
        }

        /// <summary>
        /// Extiende la duración de la sanción
        /// </summary>
        public void Extender(DateTime nuevaFechaFin, string motivo, int usuarioId)
        {
            FechaFin = nuevaFechaFin;
            Observaciones = $"{Observaciones}\n\nExtendida hasta {nuevaFechaFin:dd/MM/yyyy}. Motivo: {motivo}";
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Reduce la sanción (clemencia)
        /// </summary>
        public void Reducir(DateTime nuevaFechaFin, string motivo, int usuarioId)
        {
            if (nuevaFechaFin < FechaInicio)
                throw new InvalidOperationException("La nueva fecha de fin no puede ser anterior a la fecha de inicio");

            FechaFin = nuevaFechaFin;
            Observaciones = $"{Observaciones}\n\nReducida hasta {nuevaFechaFin:dd/MM/yyyy}. Motivo: {motivo}";
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Verifica si la sanción es severa (suspensión o expulsión)
        /// </summary>
        public bool EsSevera()
        {
            return Tipo == TipoSancion.Suspension || Tipo == TipoSancion.Expulsion;
        }

        /// <summary>
        /// Verifica si requiere seguimiento (no cumplida y vencida)
        /// </summary>
        public bool RequiereSeguimiento()
        {
            return !Cumplida && HaVencido;
        }

        /// <summary>
        /// Verifica si está pendiente de firma de enterado
        /// </summary>
        public bool PendienteFirma()
        {
            return PadresNotificados && !FirmaEnterado;
        }

        /// <summary>
        /// Valida que la sanción sea consistente
        /// </summary>
        public bool EsValida(out string mensajeError)
        {
            mensajeError = string.Empty;

            if (AlumnoId <= 0)
            {
                mensajeError = "El alumno es requerido";
                return false;
            }

            if (AutorizadoPor <= 0)
            {
                mensajeError = "La autorización es requerida";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Descripcion))
            {
                mensajeError = "La descripción de la sanción es requerida";
                return false;
            }

            if (FechaFin.HasValue && FechaFin.Value < FechaInicio)
            {
                mensajeError = "La fecha de fin no puede ser anterior a la fecha de inicio";
                return false;
            }

            return true;
        }

        #endregion
    }
}