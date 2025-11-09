using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Common;
using System;

namespace SchoolSystem.Domain.Entities.Evaluacion
{
    /// <summary>
    /// Entidad Calificacion - Representa la calificación de un alumno en una materia
    /// durante un período de evaluación específico
    /// </summary>
    public class Calificacion : BaseEntity, IAuditableEntity
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
        /// ID del alumno evaluado
        /// </summary>
        public int? AlumnoId { get; set; }

        /// <summary>
        /// Alumno (Navigation Property)
        /// </summary>
        public virtual Alumno? Alumno { get; set; }

        /// <summary>
        /// ID de la materia evaluada
        /// </summary>
        public int MateriaId { get; set; }

        /// <summary>
        /// Materia (Navigation Property)
        /// </summary>
        public virtual Academico.Materia Materia { get; set; }

        /// <summary>
        /// ID del grupo al que pertenece el alumno
        /// </summary>
        public int GrupoId { get; set; }

        /// <summary>
        /// Grupo (Navigation Property)
        /// </summary>
        public virtual Grupo Grupo { get; set; }

        /// <summary>
        /// ID del período de evaluación
        /// </summary>
        public int PeriodoId { get; set; }

        /// <summary>
        /// Período de evaluación (Navigation Property)
        /// </summary>
        public virtual PeriodoEvaluacion Periodo { get; set; }

        #endregion

        #region Calificación

        /// <summary>
        /// Calificación numérica del alumno
        /// Ejemplo: 8.5, 9.0, 7.2
        /// </summary>
        public decimal CalificacionNumerica { get; set; }

        /// <summary>
        /// Calificación en formato de letra (opcional)
        /// Ejemplos: "A", "B", "C" o "MB" (Muy Bien), "B" (Bien), "S" (Suficiente), "I" (Insuficiente)
        /// </summary>
        public string CalificacionLetra { get; set; }

        /// <summary>
        /// Indica si el alumno aprobó la materia en este período
        /// </summary>
        public bool Aprobado { get; set; }

        /// <summary>
        /// Calificación mínima requerida para aprobar (referencia)
        /// </summary>
        public decimal? CalificacionMinima { get; set; }

        #endregion

        #region Detalles de la Evaluación

        /// <summary>
        /// Tipo de evaluación
        /// Ejemplo: "Examen", "Tareas", "Proyecto", "Participación"
        /// </summary>
        public string TipoEvaluacion { get; set; }

        /// <summary>
        /// Peso o porcentaje de esta calificación en el total del período
        /// Ejemplo: Examen 70%, Tareas 30%
        /// </summary>
        public decimal? Peso { get; set; }

        /// <summary>
        /// Observaciones del maestro sobre el desempeño del alumno
        /// </summary>
        public string Observaciones { get; set; }

        /// <summary>
        /// Fortalezas identificadas del alumno
        /// </summary>
        public string Fortalezas { get; set; }

        /// <summary>
        /// Áreas de oportunidad o mejora
        /// </summary>
        public string AreasOportunidad { get; set; }

        /// <summary>
        /// Recomendaciones para el alumno
        /// </summary>
        public string Recomendaciones { get; set; }

        #endregion

        #region Control de Captura

        /// <summary>
        /// Fecha y hora en que se capturó la calificación
        /// </summary>
        public DateTime FechaCaptura { get; set; }

        /// <summary>
        /// ID del maestro que capturó la calificación
        /// </summary>
        public int? CapturadoPor { get; set; }

        /// <summary>
        /// Maestro que capturó (Navigation Property)
        /// </summary>
        public virtual Maestro MaestroCaptura { get; set; }

        /// <summary>
        /// Indica si la calificación fue modificada después de la captura inicial
        /// </summary>
        public bool FueModificada { get; set; }

        /// <summary>
        /// Fecha de la última modificación
        /// </summary>
        public DateTime? FechaUltimaModificacion { get; set; }

        /// <summary>
        /// ID del usuario que modificó por última vez
        /// </summary>
        public int? ModificadoPor { get; set; }

        /// <summary>
        /// Motivo de la modificación
        /// </summary>
        public string MotivoModificacion { get; set; }

        #endregion

        #region Recalificación y Regularización

        /// <summary>
        /// Indica si es una recalificación (examen extraordinario)
        /// </summary>
        public bool EsRecalificacion { get; set; }

        /// <summary>
        /// Calificación original (antes de recalificación)
        /// </summary>
        public decimal? CalificacionOriginal { get; set; }

        /// <summary>
        /// Fecha de la recalificación
        /// </summary>
        public DateTime? FechaRecalificacion { get; set; }

        /// <summary>
        /// Tipo de recalificación
        /// Ejemplo: "Extraordinario", "Regularización", "Segunda oportunidad"
        /// </summary>
        public string TipoRecalificacion { get; set; }

        #endregion

        #region Estado

        /// <summary>
        /// Indica si la calificación está bloqueada para edición
        /// </summary>
        public bool Bloqueada { get; set; }

        /// <summary>
        /// Fecha en que se bloqueó la calificación
        /// </summary>
        public DateTime? FechaBloqueo { get; set; }

        /// <summary>
        /// Indica si la calificación está visible para padres/alumnos
        /// </summary>
        public bool VisibleParaPadres { get; set; }

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
        /// Indica si la calificación es reprobatoria
        /// </summary>
        public bool EsReprobatoria
        {
            get
            {
                if (CalificacionMinima.HasValue)
                    return CalificacionNumerica < CalificacionMinima.Value;

                return CalificacionNumerica < 6; // Valor por defecto
            }
        }

        /// <summary>
        /// Indica si la calificación es excelente (>= 9.0)
        /// </summary>
        public bool EsExcelente => CalificacionNumerica >= 9.0m;

        /// <summary>
        /// Indica si la calificación puede ser modificada
        /// </summary>
        public bool PuedeSerModificada => !Bloqueada;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Calificacion()
        {
            FechaCaptura = DateTime.Now;
            FueModificada = false;
            EsRecalificacion = false;
            Bloqueada = false;
            VisibleParaPadres = true;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Establece la calificación y determina si aprobó
        /// </summary>
        public void EstablecerCalificacion(decimal calificacion, decimal calificacionMinima, int? maestroId = null)
        {
            CalificacionNumerica = Math.Round(calificacion, 2);
            CalificacionMinima = calificacionMinima;
            Aprobado = calificacion >= calificacionMinima;

            if (maestroId.HasValue)
                CapturadoPor = maestroId;

            // Asignar letra automáticamente según el número
            CalificacionLetra = ObtenerLetraPorNumero(calificacion);

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Convierte una calificación numérica a letra
        /// </summary>
        private string ObtenerLetraPorNumero(decimal calificacion)
        {
            if (calificacion >= 9.0m)
                return "A";
            else if (calificacion >= 8.0m)
                return "B";
            else if (calificacion >= 7.0m)
                return "C";
            else if (calificacion >= 6.0m)
                return "D";
            else
                return "F";
        }

        /// <summary>
        /// Modifica la calificación con motivo
        /// </summary>
        public void Modificar(decimal nuevaCalificacion, string motivo, int usuarioId)
        {
            if (Bloqueada)
                throw new InvalidOperationException("La calificación está bloqueada y no puede ser modificada");

            CalificacionNumerica = Math.Round(nuevaCalificacion, 2);
            Aprobado = CalificacionMinima.HasValue ?
                       nuevaCalificacion >= CalificacionMinima.Value :
                       nuevaCalificacion >= 6.0m;

            FueModificada = true;
            FechaUltimaModificacion = DateTime.Now;
            ModificadoPor = usuarioId;
            MotivoModificacion = motivo;

            CalificacionLetra = ObtenerLetraPorNumero(nuevaCalificacion);

            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Registra una recalificación
        /// </summary>
        public void Recalificar(decimal nuevaCalificacion, string tipoRecalificacion, int usuarioId)
        {
            CalificacionOriginal = CalificacionNumerica;
            CalificacionNumerica = Math.Round(nuevaCalificacion, 2);
            Aprobado = CalificacionMinima.HasValue ?
                       nuevaCalificacion >= CalificacionMinima.Value :
                       nuevaCalificacion >= 6.0m;

            EsRecalificacion = true;
            FechaRecalificacion = DateTime.Now;
            TipoRecalificacion = tipoRecalificacion;

            CalificacionLetra = ObtenerLetraPorNumero(nuevaCalificacion);

            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Bloquea la calificación para evitar modificaciones
        /// </summary>
        public void Bloquear(int usuarioId)
        {
            Bloqueada = true;
            FechaBloqueo = DateTime.Now;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Desbloquea la calificación para permitir modificaciones
        /// </summary>
        public void Desbloquear(int usuarioId)
        {
            Bloqueada = false;
            FechaBloqueo = null;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Hace la calificación visible para padres y alumnos
        /// </summary>
        public void HacerVisible(int usuarioId)
        {
            VisibleParaPadres = true;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Oculta la calificación para padres y alumnos
        /// </summary>
        public void Ocultar(int usuarioId)
        {
            VisibleParaPadres = false;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Agrega observaciones del maestro
        /// </summary>
        public void AgregarObservaciones(string observaciones, int usuarioId)
        {
            Observaciones = observaciones;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Agrega retroalimentación completa
        /// </summary>
        public void AgregarRetroalimentacion(string fortalezas, string areasOportunidad, string recomendaciones, int usuarioId)
        {
            Fortalezas = fortalezas;
            AreasOportunidad = areasOportunidad;
            Recomendaciones = recomendaciones;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Verifica si la calificación amerita atención especial
        /// </summary>
        public bool AmerlAtenci()
        {
            return EsReprobatoria || CalificacionNumerica < 7.0m;
        }

        /// <summary>
        /// Verifica si el alumno mejoró en recalificación
        /// </summary>
        public bool MejoroEnRecalificacion()
        {
            if (!EsRecalificacion || !CalificacionOriginal.HasValue)
                return false;

            return CalificacionNumerica > CalificacionOriginal.Value;
        }

        /// <summary>
        /// Calcula la diferencia con la calificación original
        /// </summary>
        public decimal? DiferenciaConOriginal()
        {
            if (!CalificacionOriginal.HasValue)
                return null;

            return CalificacionNumerica - CalificacionOriginal.Value;
        }

        /// <summary>
        /// Valida que la calificación sea consistente
        /// </summary>
        public bool EsValida(out string mensajeError)
        {
            mensajeError = string.Empty;

            if (AlumnoId <= 0)
            {
                mensajeError = "El alumno es requerido";
                return false;
            }

            if (MateriaId <= 0)
            {
                mensajeError = "La materia es requerida";
                return false;
            }

            if (GrupoId <= 0)
            {
                mensajeError = "El grupo es requerido";
                return false;
            }

            if (PeriodoId <= 0)
            {
                mensajeError = "El período es requerido";
                return false;
            }

            if (CalificacionNumerica < 0 || CalificacionNumerica > 10)
            {
                mensajeError = "La calificación debe estar entre 0 y 10";
                return false;
            }

            if (Peso.HasValue && (Peso.Value < 0 || Peso.Value > 100))
            {
                mensajeError = "El peso debe estar entre 0 y 100";
                return false;
            }

            return true;
        }

        #endregion
    }
}