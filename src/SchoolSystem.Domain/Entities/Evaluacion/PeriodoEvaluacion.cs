using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Common;
using System;
using System.Collections.Generic;

namespace SchoolSystem.Domain.Entities.Evaluacion
{
    /// <summary>
    /// Entidad PeriodoEvaluacion - Representa un período de evaluación en el ciclo escolar
    /// Ejemplos: Bimestre, Trimestre, Parcial, Semestre
    /// </summary>
    public class PeriodoEvaluacion : BaseEntity, IAuditableEntity
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

        #region Propiedades Básicas

        /// <summary>
        /// Ciclo escolar al que pertenece el período
        /// Ejemplo: "2024-2025"
        /// </summary>
        public int CicloEscolarId { get; set; }
        public virtual CicloEscolar? Ciclo { get; set; }


        /// <summary>
        /// Nombre del período de evaluación
        /// Ejemplos: "1er Bimestre", "2do Trimestre", "Parcial 1"
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Número secuencial del período
        /// Ejemplo: 1, 2, 3, 4, 5
        /// </summary>
        public int Numero { get; set; }

        /// <summary>
        /// Descripción del período (opcional)
        /// </summary>
        public string? Descripcion { get; set; }

        /// <summary>
        /// Indica si el período está activo
        /// </summary>
        public bool Activo { get; set; }

        #endregion

        #region Fechas

        /// <summary>
        /// Fecha de inicio del período
        /// </summary>
        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// Fecha de fin del período
        /// </summary>
        public DateTime FechaFin { get; set; }

        /// <summary>
        /// Fecha límite para captura de calificaciones
        /// </summary>
        public DateTime? FechaLimiteCaptura { get; set; }

        /// <summary>
        /// Fecha de publicación de calificaciones a padres/alumnos
        /// </summary>
        public DateTime? FechaPublicacion { get; set; }

        #endregion

        #region Configuración de Evaluación

        /// <summary>
        /// Porcentaje de peso en la calificación final
        /// Ejemplo: 20% para 5 bimestres, 33.33% para 3 trimestres
        /// </summary>
        public decimal Porcentaje { get; set; }

        /// <summary>
        /// Tipo de período
        /// Ejemplo: "Bimestre", "Trimestre", "Parcial", "Semestre"
        /// </summary>
        public string TipoPeriodo { get; set; }

        /// <summary>
        /// Indica si las calificaciones de este período son definitivas
        /// </summary>
        public bool CalificacionesDefinitivas { get; set; }

        /// <summary>
        /// Indica si permite recalificaciones o exámenes extraordinarios
        /// </summary>
        public bool PermiteRecalificacion { get; set; }

        /// <summary>
        /// Fecha límite para recalificaciones (si aplica)
        /// </summary>
        public DateTime? FechaLimiteRecalificacion { get; set; }

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

        #region Navigation Properties (Relaciones)

        /// <summary>
        /// Calificaciones capturadas en este período
        /// </summary>
        public virtual ICollection<Calificacion> Calificaciones { get; set; }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Nombre completo del período (incluye ciclo escolar)
        /// Ejemplo: "1er Bimestre 2024-2025"
        /// </summary>
        public string NombreCompleto => $"{Nombre} {Ciclo.Nombre} {Ciclo.Clave}";

        /// <summary>
        /// Duración en días del período
        /// </summary>
        public int DuracionDias => (FechaFin - FechaInicio).Days + 1;

        /// <summary>
        /// Indica si el período está en curso
        /// </summary>
        public bool EstaEnCurso
        {
            get
            {
                var hoy = DateTime.Today;
                return hoy >= FechaInicio && hoy <= FechaFin;
            }
        }

        /// <summary>
        /// Indica si el período ya finalizó
        /// </summary>
        public bool HaFinalizado => DateTime.Today > FechaFin;

        /// <summary>
        /// Indica si el período aún no ha comenzado
        /// </summary>
        public bool NoHaIniciado => DateTime.Today < FechaInicio;

        /// <summary>
        /// Indica si está dentro del plazo para captura de calificaciones
        /// </summary>
        public bool DentroDePlazoCap => !FechaLimiteCaptura.HasValue || DateTime.Now <= FechaLimiteCaptura.Value;

        /// <summary>
        /// Días restantes del período
        /// </summary>
        public int? DiasRestantes
        {
            get
            {
                if (HaFinalizado)
                    return 0;

                if (NoHaIniciado)
                    return null;

                return (FechaFin - DateTime.Today).Days;
            }
        }

        /// <summary>
        /// Porcentaje de avance del período
        /// </summary>
        public decimal PorcentajeAvance
        {
            get
            {
                if (NoHaIniciado)
                    return 0;

                if (HaFinalizado)
                    return 100;

                var diasTranscurridos = (DateTime.Today - FechaInicio).Days;
                var duracionTotal = DuracionDias;

                if (duracionTotal == 0)
                    return 0;

                return Math.Round((decimal)diasTranscurridos / duracionTotal * 100, 2);
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public PeriodoEvaluacion()
        {
            Activo = true;
            CalificacionesDefinitivas = false;
            PermiteRecalificacion = false;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            // Inicializar colecciones
            Calificaciones = new HashSet<Calificacion>();
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Verifica si el período está activo
        /// </summary>
        public bool EstaActivo()
        {
            return Activo;
        }

        /// <summary>
        /// Activa el período
        /// </summary>
        public void Activar(int usuarioId)
        {
            Activo = true;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Desactiva el período
        /// </summary>
        public void Desactivar(int usuarioId)
        {
            Activo = false;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Verifica si se pueden capturar calificaciones
        /// </summary>
        public bool PuedeCapturarCalificaciones()
        {
            if (!Activo)
                return false;

            if (CalificacionesDefinitivas)
                return false;

            if (!DentroDePlazoCap)
                return false;

            return true;
        }

        /// <summary>
        /// Verifica si se pueden modificar calificaciones
        /// </summary>
        public bool PuedeModificarCalificaciones()
        {
            return PuedeCapturarCalificaciones();
        }

        /// <summary>
        /// Marca las calificaciones como definitivas
        /// </summary>
        public void MarcarCalificacionesComoDefinitivas(int usuarioId)
        {
            CalificacionesDefinitivas = true;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Reabre las calificaciones para edición
        /// </summary>
        public void ReabrirCalificaciones(int usuarioId)
        {
            CalificacionesDefinitivas = false;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Habilita recalificaciones
        /// </summary>
        public void HabilitarRecalificaciones(DateTime fechaLimite, int usuarioId)
        {
            PermiteRecalificacion = true;
            FechaLimiteRecalificacion = fechaLimite;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Deshabilita recalificaciones
        /// </summary>
        public void DeshabilitarRecalificaciones(int usuarioId)
        {
            PermiteRecalificacion = false;
            FechaLimiteRecalificacion = null;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Verifica si está en período de recalificación
        /// </summary>
        public bool EstaEnPeriodoRecalificacion()
        {
            if (!PermiteRecalificacion)
                return false;

            if (!FechaLimiteRecalificacion.HasValue)
                return false;

            return DateTime.Now <= FechaLimiteRecalificacion.Value;
        }

        /// <summary>
        /// Establece la fecha de publicación de calificaciones
        /// </summary>
        public void EstablecerFechaPublicacion(DateTime fecha, int usuarioId)
        {
            FechaPublicacion = fecha;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Verifica si las calificaciones ya fueron publicadas
        /// </summary>
        public bool CalificacionesPublicadas()
        {
            return FechaPublicacion.HasValue && DateTime.Now >= FechaPublicacion.Value;
        }

        /// <summary>
        /// Verifica si el período se superpone con otro
        /// </summary>
        public bool SeSuperponeCon(PeriodoEvaluacion otroPeriodo)
        {
            if (otroPeriodo == null)
                return false;

            return !(FechaFin < otroPeriodo.FechaInicio || FechaInicio > otroPeriodo.FechaFin);
        }

        /// <summary>
        /// Extiende la fecha de fin del período
        /// </summary>
        public void ExtenderPeriodo(DateTime nuevaFechaFin, int usuarioId)
        {
            if (nuevaFechaFin <= FechaInicio)
                throw new InvalidOperationException("La nueva fecha de fin debe ser posterior a la fecha de inicio");

            FechaFin = nuevaFechaFin;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Extiende el plazo de captura de calificaciones
        /// </summary>
        public void ExtenderPlazoCap(DateTime nuevaFechaLimite, int usuarioId)
        {
            FechaLimiteCaptura = nuevaFechaLimite;
            UpdatedAt = DateTime.Now;
            UpdatedBy = usuarioId;
        }

        /// <summary>
        /// Valida que la configuración del período sea consistente
        /// </summary>
        public bool EsValido(out string mensajeError)
        {
            mensajeError = string.Empty;

            if (Ciclo == null)
            {
                mensajeError = "El ciclo escolar es requerido";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Nombre))
            {
                mensajeError = "El nombre del período es requerido";
                return false;
            }

            if (Numero <= 0)
            {
                mensajeError = "El número del período debe ser mayor a 0";
                return false;
            }

            if (FechaInicio >= FechaFin)
            {
                mensajeError = "La fecha de inicio debe ser anterior a la fecha de fin";
                return false;
            }

            if (Porcentaje < 0 || Porcentaje > 100)
            {
                mensajeError = "El porcentaje debe estar entre 0 y 100";
                return false;
            }

            if (FechaLimiteCaptura.HasValue && FechaLimiteCaptura.Value < FechaFin)
            {
                mensajeError = "La fecha límite de captura debe ser posterior o igual a la fecha de fin del período";
                return false;
            }

            if (FechaPublicacion.HasValue && FechaPublicacion.Value < FechaFin)
            {
                mensajeError = "La fecha de publicación debe ser posterior o igual a la fecha de fin del período";
                return false;
            }

            return true;
        }

        #endregion
    }
}