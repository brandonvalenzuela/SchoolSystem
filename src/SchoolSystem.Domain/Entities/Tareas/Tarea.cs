using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Enums.Academico;

namespace SchoolSystem.Domain.Entities.Tareas
{
    /// <summary>
    /// Tareas y actividades asignadas por maestros a grupos
    /// </summary>
    [Table("Tareas")]
    public class Tarea : BaseEntity, IAuditableEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// Grupo al que se asigna la tarea
        /// </summary>
        [Required]
        public int GrupoId { get; set; }

        /// <summary>
        /// Materia de la tarea
        /// </summary>
        [Required]
        public int MateriaId { get; set; }

        /// <summary>
        /// Maestro que asigna la tarea
        /// </summary>
        [Required]
        public int MaestroId { get; set; }

        /// <summary>
        /// Título de la tarea
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        /// <summary>
        /// Descripción detallada de la tarea
        /// </summary>
        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? Descripcion { get; set; }

        #endregion

        #region Fechas

        /// <summary>
        /// Fecha y hora de asignación de la tarea
        /// </summary>
        [Required]
        public DateTime FechaAsignacion { get; set; }

        /// <summary>
        /// Fecha y hora límite de entrega
        /// </summary>
        [Required]
        public DateTime FechaEntrega { get; set; }

        /// <summary>
        /// Fecha límite para entregas tardías (opcional)
        /// </summary>
        public DateTime? FechaLimiteTardia { get; set; }

        #endregion

        #region Configuración de Evaluación

        /// <summary>
        /// Tipo de tarea
        /// </summary>
        [Required]
        public TipoTarea Tipo { get; set; }

        /// <summary>
        /// Valor en puntos de la tarea
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal ValorPuntos { get; set; }

        /// <summary>
        /// Permite entregas tardías
        /// </summary>
        [Required]
        public bool PermiteEntregaTardia { get; set; }

        /// <summary>
        /// Porcentaje de penalización por entrega tardía (ej: 20 = 20%)
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal? PenalizacionTardia { get; set; }

        #endregion

        #region Archivos y Recursos

        /// <summary>
        /// URL del archivo adjunto (documento, PDF, etc.)
        /// </summary>
        [StringLength(500)]
        public string ArchivoAdjuntoUrl { get; set; }

        /// <summary>
        /// Nombre original del archivo adjunto
        /// </summary>
        [StringLength(200)]
        public string ArchivoAdjuntoNombre { get; set; }

        /// <summary>
        /// Tamaño del archivo en bytes
        /// </summary>
        public long? ArchivoAdjuntoTamano { get; set; }

        #endregion

        #region Estado y Control

        /// <summary>
        /// Indica si la tarea está activa
        /// </summary>
        [Required]
        public bool Activo { get; set; }

        /// <summary>
        /// Observaciones adicionales
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Observaciones { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Grupo relacionado
        /// </summary>
        [ForeignKey("GrupoId")]
        public virtual Grupo Grupo { get; set; }

        /// <summary>
        /// Materia relacionada
        /// </summary>
        [ForeignKey("MateriaId")]
        public virtual Materia Materia { get; set; }

        /// <summary>
        /// Maestro que asignó la tarea
        /// </summary>
        [ForeignKey("MaestroId")]
        public virtual Maestro Maestro { get; set; }

        /// <summary>
        /// Entregas de la tarea por alumnos
        /// </summary>
        public virtual ICollection<TareaEntrega> Entregas { get; set; }

        #endregion

        #region Auditoría (IAuditableEntity)

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        #endregion

        #region Constructor

        public Tarea()
        {
            FechaAsignacion = DateTime.Now;
            Activo = true;
            PermiteEntregaTardia = false;
            Tipo = TipoTarea.Tarea;
            ValorPuntos = 10;
            Entregas = new HashSet<TareaEntrega>();
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si la tarea ya venció
        /// </summary>
        public bool EstaVencida => DateTime.Now > FechaEntrega;

        /// <summary>
        /// Indica si aún está en el período de entrega tardía
        /// </summary>
        public bool EnPeriodoTardio => EstaVencida &&
                                       PermiteEntregaTardia &&
                                       FechaLimiteTardia.HasValue &&
                                       DateTime.Now <= FechaLimiteTardia.Value;

        /// <summary>
        /// Indica si ya no se puede entregar (ni siquiera tarde)
        /// </summary>
        public bool FueraDeTiempo
        {
            get
            {
                if (!EstaVencida) return false;
                if (!PermiteEntregaTardia) return true;
                if (!FechaLimiteTardia.HasValue) return true;
                return DateTime.Now > FechaLimiteTardia.Value;
            }
        }

        /// <summary>
        /// Días que faltan para la entrega (negativo si ya venció)
        /// </summary>
        public int DiasParaEntrega
        {
            get
            {
                var dias = (FechaEntrega.Date - DateTime.Now.Date).Days;
                return dias;
            }
        }

        /// <summary>
        /// Horas que faltan para la entrega
        /// </summary>
        public double HorasParaEntrega => (FechaEntrega - DateTime.Now).TotalHours;

        /// <summary>
        /// Total de entregas realizadas
        /// </summary>
        public int TotalEntregas => Entregas?.Count ?? 0;

        /// <summary>
        /// Entregas pendientes (asumiendo que hay X alumnos en el grupo)
        /// </summary>
        public int EntregasPendientes(int totalAlumnosGrupo)
        {
            return totalAlumnosGrupo - TotalEntregas;
        }

        /// <summary>
        /// Porcentaje de entregas realizadas
        /// </summary>
        public decimal PorcentajeEntregas(int totalAlumnosGrupo)
        {
            if (totalAlumnosGrupo == 0) return 0;
            return (decimal)TotalEntregas / totalAlumnosGrupo * 100;
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Verifica si un alumno puede entregar la tarea en este momento
        /// </summary>
        /// <returns>Tupla con (puede_entregar, es_tardia, mensaje)</returns>
        public (bool puedeEntregar, bool esTardia, string mensaje) VerificarPuedeEntregar()
        {
            if (!Activo)
                return (false, false, "La tarea está inactiva");

            if (!EstaVencida)
                return (true, false, "Dentro del plazo de entrega");

            if (PermiteEntregaTardia && EnPeriodoTardio)
            {
                var penalizacion = PenalizacionTardia.HasValue ? $" (Penalización: {PenalizacionTardia}%)" : "";
                return (true, true, $"Entrega tardía permitida{penalizacion}");
            }

            return (false, false, "Fuera del plazo de entrega");
        }

        /// <summary>
        /// Calcula el puntaje con penalización si es tardía
        /// </summary>
        /// <param name="calificacionOriginal">Calificación sin penalización</param>
        /// <param name="esTardia">Si la entrega es tardía</param>
        /// <returns>Calificación final con penalización aplicada</returns>
        public decimal CalcularPuntajeConPenalizacion(decimal calificacionOriginal, bool esTardia)
        {
            if (!esTardia || !PenalizacionTardia.HasValue)
                return calificacionOriginal;

            var reduccion = calificacionOriginal * (PenalizacionTardia.Value / 100);
            var calificacionFinal = calificacionOriginal - reduccion;

            return calificacionFinal < 0 ? 0 : calificacionFinal;
        }

        /// <summary>
        /// Extiende la fecha límite de entrega
        /// </summary>
        /// <param name="nuevaFecha">Nueva fecha límite</param>
        /// <param name="extenderTambienTardia">Si también se extiende la fecha tardía</param>
        public void ExtenderFechaEntrega(DateTime nuevaFecha, bool extenderTambienTardia = false)
        {
            if (nuevaFecha <= FechaEntrega)
                throw new InvalidOperationException("La nueva fecha debe ser posterior a la actual");

            var diferencia = nuevaFecha - FechaEntrega;
            FechaEntrega = nuevaFecha;

            if (extenderTambienTardia && FechaLimiteTardia.HasValue)
            {
                FechaLimiteTardia = FechaLimiteTardia.Value.Add(diferencia);
            }

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Desactiva la tarea
        /// </summary>
        public void Desactivar()
        {
            Activo = false;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Activa la tarea
        /// </summary>
        public void Activar()
        {
            Activo = true;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Adjunta un archivo a la tarea
        /// </summary>
        public void AdjuntarArchivo(string url, string nombreArchivo, long tamano)
        {
            ArchivoAdjuntoUrl = url;
            ArchivoAdjuntoNombre = nombreArchivo;
            ArchivoAdjuntoTamano = tamano;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Remueve el archivo adjunto
        /// </summary>
        public void RemoverArchivo()
        {
            ArchivoAdjuntoUrl = null;
            ArchivoAdjuntoNombre = null;
            ArchivoAdjuntoTamano = null;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Valida que la tarea sea correcta antes de guardar
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Titulo))
                errores.Add("El título es requerido");

            if (string.IsNullOrWhiteSpace(Descripcion))
                errores.Add("La descripción es requerida");

            if (FechaEntrega <= FechaAsignacion)
                errores.Add("La fecha de entrega debe ser posterior a la fecha de asignación");

            if (PermiteEntregaTardia && FechaLimiteTardia.HasValue && FechaLimiteTardia.Value <= FechaEntrega)
                errores.Add("La fecha límite tardía debe ser posterior a la fecha de entrega");

            if (ValorPuntos <= 0)
                errores.Add("El valor en puntos debe ser mayor a cero");

            if (PermiteEntregaTardia && PenalizacionTardia.HasValue && (PenalizacionTardia < 0 || PenalizacionTardia > 100))
                errores.Add("La penalización debe estar entre 0 y 100");

            return errores;
        }

        #endregion
    }
}