using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Enums;
using SchoolSystem.Domain.Enums.Academico;
using System;

namespace SchoolSystem.Domain.Entities.Academico
{
    /// <summary>
    /// Entidad Inscripcion - Representa la inscripción de un alumno a un grupo en un ciclo escolar
    /// Mantiene el historial académico del alumno por ciclo
    /// </summary>
    public class Inscripcion : BaseEntity, IAuditableEntity
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
        /// ID del alumno inscrito
        /// </summary>
        public int AlumnoId { get; set; }

        /// <summary>
        /// Alumno (Navigation Property)
        /// </summary>
        public virtual Alumno Alumno { get; set; }

        /// <summary>
        /// ID del grupo al que se inscribe
        /// </summary>
        public int GrupoId { get; set; }

        /// <summary>
        /// Grupo (Navigation Property)
        /// </summary>
        public virtual Grupo Grupo { get; set; }

        #endregion

        #region Datos de la Inscripción

        /// <summary>
        /// Ciclo escolar de la inscripción
        /// Ejemplo: "2024-2025"
        /// </summary>
        public string CicloEscolar { get; set; }

        /// <summary>
        /// Fecha en que se realizó la inscripción
        /// </summary>
        public DateTime FechaInscripcion { get; set; }

        /// <summary>
        /// Fecha de inicio del ciclo para el alumno (puede diferir de la inscripción)
        /// </summary>
        public DateTime? FechaInicio { get; set; }

        /// <summary>
        /// Fecha de fin del ciclo escolar
        /// </summary>
        public DateTime? FechaFin { get; set; }

        /// <summary>
        /// Número de lista del alumno dentro del grupo
        /// </summary>
        public int? NumeroLista { get; set; }

        /// <summary>
        /// Estatus de la inscripción
        /// </summary>
        public EstatusInscripcion Estatus { get; set; }

        #endregion

        #region Calificaciones y Evaluación

        /// <summary>
        /// Promedio final del alumno en el ciclo escolar
        /// Se calcula al finalizar el ciclo
        /// </summary>
        public decimal? PromedioFinal { get; set; }

        /// <summary>
        /// Promedio acumulado hasta el momento
        /// Se actualiza conforme se capturan calificaciones
        /// </summary>
        public decimal? PromedioAcumulado { get; set; }

        /// <summary>
        /// Indica si el alumno aprobó el ciclo escolar
        /// </summary>
        public bool? Aprobado { get; set; }

        /// <summary>
        /// Cantidad de materias reprobadas
        /// </summary>
        public int? MateriasReprobadas { get; set; }

        /// <summary>
        /// Posición del alumno en el ranking del grupo
        /// </summary>
        public int? LugarEnGrupo { get; set; }

        #endregion

        #region Asistencias

        /// <summary>
        /// Total de días de clase en el ciclo
        /// </summary>
        public int? TotalDiasClase { get; set; }

        /// <summary>
        /// Días en que el alumno asistió
        /// </summary>
        public int? DiasAsistidos { get; set; }

        /// <summary>
        /// Días en que el alumno faltó
        /// </summary>
        public int? DiasFaltados { get; set; }

        /// <summary>
        /// Días de retardo
        /// </summary>
        public int? DiasRetardo { get; set; }

        /// <summary>
        /// Porcentaje de asistencia
        /// </summary>
        public decimal? PorcentajeAsistencia { get; set; }

        #endregion

        #region Bajas y Cambios

        /// <summary>
        /// Fecha de baja temporal o definitiva (si aplica)
        /// </summary>
        public DateTime? FechaBaja { get; set; }

        /// <summary>
        /// Motivo de la baja
        /// </summary>
        public string MotivoBaja { get; set; }

        /// <summary>
        /// ID del grupo anterior (si hubo cambio de grupo)
        /// </summary>
        public int? GrupoAnteriorId { get; set; }

        /// <summary>
        /// Fecha de cambio de grupo (si aplica)
        /// </summary>
        public DateTime? FechaCambioGrupo { get; set; }

        /// <summary>
        /// Motivo del cambio de grupo
        /// </summary>
        public string MotivoCambioGrupo { get; set; }

        #endregion

        #region Información Adicional

        /// <summary>
        /// Observaciones generales sobre la inscripción
        /// </summary>
        public string Observaciones { get; set; }

        /// <summary>
        /// Indica si el alumno fue becado
        /// </summary>
        public bool Becado { get; set; }

        /// <summary>
        /// Tipo de beca (si aplica)
        /// </summary>
        public string TipoBeca { get; set; }

        /// <summary>
        /// Porcentaje de beca
        /// </summary>
        public decimal? PorcentajeBeca { get; set; }

        /// <summary>
        /// Indica si es alumno repetidor
        /// </summary>
        public bool Repetidor { get; set; }

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
        /// Indica si la inscripción está activa
        /// </summary>
        public bool EstaActiva => Estatus == EstatusInscripcion.Inscrito;

        /// <summary>
        /// Indica si el ciclo escolar ha finalizado
        /// </summary>
        public bool CicloFinalizado => Estatus == EstatusInscripcion.Finalizado;

        /// <summary>
        /// Indica si el alumno está dado de baja
        /// </summary>
        public bool EstaDeBaja => Estatus == EstatusInscripcion.BajaTemporal ||
                                  Estatus == EstatusInscripcion.BajaDefinitiva;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Inscripcion()
        {
            FechaInscripcion = DateTime.Now;
            Estatus = EstatusInscripcion.Inscrito;
            Becado = false;
            Repetidor = false;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Da de baja temporal al alumno
        /// </summary>
        public void DarDeBajaTemporal(string motivo, int usuarioId)
        {
            Estatus = EstatusInscripcion.BajaTemporal;
            FechaBaja = DateTime.Now;
            MotivoBaja = motivo;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Da de baja definitiva al alumno
        /// </summary>
        public void DarDeBajaDefinitiva(string motivo, int usuarioId)
        {
            Estatus = EstatusInscripcion.BajaDefinitiva;
            FechaBaja = DateTime.Now;
            MotivoBaja = motivo;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Reactiva la inscripción del alumno
        /// </summary>
        public void Reactivar(int usuarioId)
        {
            Estatus = EstatusInscripcion.Inscrito;
            FechaBaja = null;
            MotivoBaja = null;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Finaliza el ciclo escolar para el alumno
        /// </summary>
        public void FinalizarCiclo(int usuarioId)
        {
            Estatus = EstatusInscripcion.Finalizado;
            FechaFin = DateTime.Now;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Cambia al alumno a otro grupo
        /// </summary>
        public void CambiarGrupo(int nuevoGrupoId, string motivo, int usuarioId)
        {
            GrupoAnteriorId = GrupoId;
            GrupoId = nuevoGrupoId;
            FechaCambioGrupo = DateTime.Now;
            MotivoCambioGrupo = motivo;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Asigna un número de lista al alumno
        /// </summary>
        public void AsignarNumeroLista(int numero, int usuarioId)
        {
            NumeroLista = numero;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Actualiza el promedio acumulado
        /// </summary>
        public void ActualizarPromedioAcumulado(decimal promedio)
        {
            PromedioAcumulado = Math.Round(promedio, 2);
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Establece el promedio final y determina si aprobó
        /// </summary>
        public void EstablecerPromedioFinal(decimal promedio, decimal calificacionMinima, int materiasReprobadas)
        {
            PromedioFinal = Math.Round(promedio, 2);
            MateriasReprobadas = materiasReprobadas;
            Aprobado = promedio >= calificacionMinima && materiasReprobadas == 0;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Actualiza las estadísticas de asistencia
        /// </summary>
        public void ActualizarEstadisticasAsistencia(int totalDias, int diasAsistidos, int diasFaltados, int diasRetardo)
        {
            TotalDiasClase = totalDias;
            DiasAsistidos = diasAsistidos;
            DiasFaltados = diasFaltados;
            DiasRetardo = diasRetardo;

            if (totalDias > 0)
            {
                PorcentajeAsistencia = Math.Round((decimal)diasAsistidos / totalDias * 100, 2);
            }

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Asigna una beca al alumno
        /// </summary>
        public void AsignarBeca(string tipoBeca, decimal porcentaje, int usuarioId)
        {
            Becado = true;
            TipoBeca = tipoBeca;
            PorcentajeBeca = porcentaje;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Remueve la beca del alumno
        /// </summary>
        public void RemoverBeca(int usuarioId)
        {
            Becado = false;
            TipoBeca = null;
            PorcentajeBeca = null;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Marca al alumno como repetidor
        /// </summary>
        public void MarcarComoRepetidor(int usuarioId)
        {
            Repetidor = true;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Verifica si el alumno tiene buena asistencia (>= 80%)
        /// </summary>
        public bool TieneBuenaAsistencia()
        {
            return PorcentajeAsistencia.HasValue && PorcentajeAsistencia.Value >= 80;
        }

        /// <summary>
        /// Verifica si el alumno está en riesgo académico
        /// </summary>
        public bool EstaEnRiesgoAcademico(decimal calificacionMinima)
        {
            if (!PromedioAcumulado.HasValue)
                return false;

            return PromedioAcumulado.Value < calificacionMinima ||
                   (MateriasReprobadas.HasValue && MateriasReprobadas.Value > 2);
        }

        /// <summary>
        /// Verifica si hubo cambio de grupo durante el ciclo
        /// </summary>
        public bool TuvoCambioDeGrupo()
        {
            return GrupoAnteriorId.HasValue;
        }

        /// <summary>
        /// Calcula los días restantes del ciclo escolar
        /// </summary>
        public int? DiasRestantesCiclo()
        {
            if (!FechaFin.HasValue)
                return null;

            var diasRestantes = (FechaFin.Value - DateTime.Today).Days;
            return diasRestantes > 0 ? diasRestantes : 0;
        }

        /// <summary>
        /// Valida que la inscripción sea consistente
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

            if (string.IsNullOrWhiteSpace(CicloEscolar))
            {
                mensajeError = "El ciclo escolar es requerido";
                return false;
            }

            if (FechaInicio.HasValue && FechaFin.HasValue && FechaInicio.Value > FechaFin.Value)
            {
                mensajeError = "La fecha de inicio no puede ser posterior a la fecha de fin";
                return false;
            }

            if (PorcentajeBeca.HasValue && (PorcentajeBeca.Value < 0 || PorcentajeBeca.Value > 100))
            {
                mensajeError = "El porcentaje de beca debe estar entre 0 y 100";
                return false;
            }

            return true;
        }

        #endregion
    }
}