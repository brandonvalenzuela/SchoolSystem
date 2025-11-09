using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Escuelas;
using SchoolSystem.Domain.Enums;
using SchoolSystem.Domain.Enums.Asistencia;
using System;

namespace SchoolSystem.Domain.Entities.Evaluacion
{
    /// <summary>
    /// Entidad Asistencia - Representa el registro de asistencia de un alumno en una fecha específica
    /// </summary>
    public class Asistencia : BaseEntity, IAuditableEntity
    {
        #region Propiedades de la Escuela (Multi-tenant)

        /// <summary>
        /// ID de la escuela a la que pertenece
        /// </summary>
        public int EscuelaId { get; set; }

        /// <summary>
        /// Escuela asociada (Navigation Property)
        /// </summary>
        public virtual Escuela Escuela { get; set; }

        #endregion

        #region Relaciones Principales

        /// <summary>
        /// ID del alumno
        /// </summary>
        public int AlumnoId { get; set; }

        /// <summary>
        /// Alumno (Navigation Property)
        /// </summary>
        public virtual Alumno Alumno { get; set; }

        /// <summary>
        /// ID del grupo al que pertenece el alumno
        /// </summary>
        public int GrupoId { get; set; }

        /// <summary>
        /// Grupo (Navigation Property)
        /// </summary>
        public virtual Academico.Grupo Grupo { get; set; }

        #endregion

        #region Datos de Asistencia

        /// <summary>
        /// Fecha de la asistencia
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Estado de la asistencia
        /// </summary>
        public EstadoAsistencia Estatus { get; set; }

        /// <summary>
        /// Hora de entrada del alumno
        /// </summary>
        public TimeSpan? HoraEntrada { get; set; }

        /// <summary>
        /// Hora de salida del alumno
        /// </summary>
        public TimeSpan? HoraSalida { get; set; }

        /// <summary>
        /// Minutos de retardo (si aplica)
        /// </summary>
        public int? MinutosRetardo { get; set; }

        #endregion

        #region Justificación

        /// <summary>
        /// Indica si la falta o retardo está justificado
        /// </summary>
        public bool Justificado { get; set; }

        /// <summary>
        /// Motivo de la falta o retardo
        /// </summary>
        public string Motivo { get; set; }

        /// <summary>
        /// URL del documento de justificación (PDF, imagen, etc.)
        /// </summary>
        public string JustificanteUrl { get; set; }

        /// <summary>
        /// Fecha en que se presentó el justificante
        /// </summary>
        public DateTime? FechaJustificacion { get; set; }

        /// <summary>
        /// ID del usuario que aprobó el justificante
        /// </summary>
        public int? AproboJustificanteId { get; set; }

        #endregion

        #region Observaciones

        /// <summary>
        /// Observaciones generales sobre la asistencia
        /// </summary>
        public string Observaciones { get; set; }

        /// <summary>
        /// Indica si se notificó a los padres sobre la inasistencia
        /// </summary>
        public bool PadresNotificados { get; set; }

        /// <summary>
        /// Fecha y hora en que se notificó a los padres
        /// </summary>
        public DateTime? FechaNotificacionPadres { get; set; }

        #endregion

        #region Control de Registro

        /// <summary>
        /// ID del usuario que registró la asistencia (maestro o administrativo)
        /// </summary>
        public int? RegistradoPor { get; set; }

        /// <summary>
        /// Usuario que registró (Navigation Property)
        /// </summary>
        public virtual Usuarios.Usuario UsuarioRegistro { get; set; }

        /// <summary>
        /// Fecha y hora del registro inicial
        /// </summary>
        public DateTime FechaRegistro { get; set; }

        /// <summary>
        /// Indica si la asistencia fue modificada después del registro inicial
        /// </summary>
        public bool FueModificada { get; set; }

        /// <summary>
        /// Fecha de la última modificación
        /// </summary>
        public DateTime? FechaUltimaModificacion { get; set; }

        /// <summary>
        /// Motivo de la modificación
        /// </summary>
        public string MotivoModificacion { get; set; }

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
        /// Indica si el alumno estuvo presente
        /// </summary>
        public bool EstaPresente => Estatus == EstadoAsistencia.Presente;

        /// <summary>
        /// Indica si el alumno faltó
        /// </summary>
        public bool Falto => Estatus == EstadoAsistencia.Falta;

        /// <summary>
        /// Indica si el alumno llegó tarde
        /// </summary>
        public bool LlegoTarde => Estatus == EstadoAsistencia.Retardo;

        /// <summary>
        /// Indica si es una falta justificada
        /// </summary>
        public bool EsFaltaJustificada => Estatus == EstadoAsistencia.Justificada;

        /// <summary>
        /// Indica si tiene permiso autorizado
        /// </summary>
        public bool TienePermiso => Estatus == EstadoAsistencia.Permiso;

        /// <summary>
        /// Indica si afecta negativamente (falta sin justificar o retardo)
        /// </summary>
        public bool AfectaNegat => Estatus == EstadoAsistencia.Falta ||
                                              Estatus == EstadoAsistencia.Retardo;

        /// <summary>
        /// Indica si requiere seguimiento (falta no justificada)
        /// </summary>
        public bool RequiereSeguim => Estatus == EstadoAsistencia.Falta && !Justificado;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Asistencia()
        {
            Fecha = DateTime.Today;
            FechaRegistro = DateTime.Now;
            Justificado = false;
            PadresNotificados = false;
            FueModificada = false;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Registra al alumno como presente
        /// </summary>
        public void MarcarComoPresente(TimeSpan? horaEntrada, int? usuarioId = null)
        {
            Estatus = EstadoAsistencia.Presente;
            HoraEntrada = horaEntrada;
            MinutosRetardo = null;

            if (usuarioId.HasValue)
                RegistradoPor = usuarioId;

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Registra al alumno con retardo
        /// </summary>
        public void MarcarComoRetardo(TimeSpan horaEntrada, TimeSpan horaEsperada, int? usuarioId = null)
        {
            Estatus = EstadoAsistencia.Retardo;
            HoraEntrada = horaEntrada;

            // Calcular minutos de retardo
            var diferencia = horaEntrada - horaEsperada;
            MinutosRetardo = (int)diferencia.TotalMinutes;

            if (usuarioId.HasValue)
                RegistradoPor = usuarioId;

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Registra la falta del alumno
        /// </summary>
        public void MarcarComoFalta(int? usuarioId = null)
        {
            Estatus = EstadoAsistencia.Falta;
            HoraEntrada = null;
            HoraSalida = null;
            MinutosRetardo = null;

            if (usuarioId.HasValue)
                RegistradoPor = usuarioId;

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Marca la asistencia como permiso autorizado
        /// </summary>
        public void MarcarComoPermiso(string motivo, int? usuarioId = null)
        {
            Estatus = EstadoAsistencia.Permiso;
            Motivo = motivo;
            Justificado = true;
            HoraEntrada = null;
            HoraSalida = null;

            if (usuarioId.HasValue)
                RegistradoPor = usuarioId;

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Justifica una falta o retardo
        /// </summary>
        public void Justificar(string motivo, string justificanteUrl, int usuarioAprobo)
        {
            if (Estatus == EstadoAsistencia.Presente)
                throw new InvalidOperationException("No se puede justificar una asistencia de un alumno presente");

            if (Estatus == EstadoAsistencia.Falta)
            {
                Estatus = EstadoAsistencia.Justificada;
            }

            Justificado = true;
            Motivo = motivo;
            JustificanteUrl = justificanteUrl;
            FechaJustificacion = DateTime.Now;
            AproboJustificanteId = usuarioAprobo;

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Rechaza o elimina la justificación
        /// </summary>
        public void RechazarJustificacion(int usuarioId)
        {
            if (Estatus == EstadoAsistencia.Justificada)
            {
                Estatus = EstadoAsistencia.Falta;
            }

            Justificado = false;
            JustificanteUrl = null;
            FechaJustificacion = null;
            AproboJustificanteId = null;

            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Registra la hora de salida del alumno
        /// </summary>
        public void RegistrarSalida(TimeSpan horaSalida)
        {
            HoraSalida = horaSalida;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Modifica el estado de asistencia con motivo
        /// </summary>
        public void Modificar(EstadoAsistencia nuevoEstatus, string motivo, int usuarioId)
        {
            Estatus = nuevoEstatus;
            MotivoModificacion = motivo;
            FueModificada = true;
            FechaUltimaModificacion = DateTime.Now;

            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Agrega observaciones a la asistencia
        /// </summary>
        public void AgregarObservaciones(string observaciones, int usuarioId)
        {
            Observaciones = observaciones;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Marca que se notificó a los padres
        /// </summary>
        public void MarcarPadresNotificados()
        {
            PadresNotificados = true;
            FechaNotificacionPadres = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Verifica si es una falta recurrente (más de 3 faltas en el mes)
        /// Esto requeriría acceso al repositorio para consultar otras asistencias
        /// </summary>
        public bool EsFaltaRecurrente(int totalFaltasEnMes)
        {
            return Falto && totalFaltasEnMes > 3;
        }

        /// <summary>
        /// Verifica si el retardo es significativo (más de 15 minutos)
        /// </summary>
        public bool EsRetardoSignificativo()
        {
            return LlegoTarde && MinutosRetardo.HasValue && MinutosRetardo.Value > 15;
        }

        /// <summary>
        /// Calcula las horas de permanencia en la escuela
        /// </summary>
        public TimeSpan? HorasPermanencia()
        {
            if (!HoraEntrada.HasValue || !HoraSalida.HasValue)
                return null;

            return HoraSalida.Value - HoraEntrada.Value;
        }

        /// <summary>
        /// Verifica si la asistencia corresponde a un día hábil
        /// </summary>
        public bool EsDiaHabil()
        {
            // Lunes a Viernes
            return Fecha.DayOfWeek != DayOfWeek.Saturday &&
                   Fecha.DayOfWeek != DayOfWeek.Sunday;
        }

        /// <summary>
        /// Verifica si el registro de asistencia está dentro del período permitido
        /// </summary>
        public bool RegistroEnTiempo(int diasMaximosRetroactivos = 3)
        {
            var diasDiferencia = (DateTime.Today - Fecha.Date).Days;
            return diasDiferencia <= diasMaximosRetroactivos;
        }

        /// <summary>
        /// Valida que la asistencia sea consistente
        /// </summary>
        public bool EsValida(out string mensajeError)
        {
            mensajeError = string.Empty;

            if (AlumnoId <= 0)
            {
                mensajeError = "El alumno es requerido";
                return false;
            }

            if (GrupoId <= 0)
            {
                mensajeError = "El grupo es requerido";
                return false;
            }

            if (Fecha > DateTime.Today)
            {
                mensajeError = "No se puede registrar asistencia de fechas futuras";
                return false;
            }

            if (HoraEntrada.HasValue && HoraSalida.HasValue && HoraEntrada.Value >= HoraSalida.Value)
            {
                mensajeError = "La hora de entrada debe ser anterior a la hora de salida";
                return false;
            }

            if (MinutosRetardo.HasValue && MinutosRetardo.Value < 0)
            {
                mensajeError = "Los minutos de retardo no pueden ser negativos";
                return false;
            }

            if (Estatus == EstadoAsistencia.Presente && !HoraEntrada.HasValue)
            {
                mensajeError = "Si el alumno está presente, debe tener hora de entrada";
                return false;
            }

            return true;
        }

        #endregion
    }
}