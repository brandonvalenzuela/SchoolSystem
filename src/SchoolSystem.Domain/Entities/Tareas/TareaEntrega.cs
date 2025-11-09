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
    /// Entregas de tareas realizadas por los alumnos
    /// </summary>
    [Table("TareaEntregas")]
    public class TareaEntrega : BaseEntity, IAuditableEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// Tarea que se está entregando
        /// </summary>
        [Required]
        public int TareaId { get; set; }

        /// <summary>
        /// Alumno que entrega la tarea
        /// </summary>
        [Required]
        public int? AlumnoId { get; set; }

        #endregion

        #region Información de Entrega

        /// <summary>
        /// Fecha y hora en que se entregó la tarea
        /// </summary>
        public DateTime? FechaEntrega { get; set; }

        /// <summary>
        /// Comentarios del alumno al entregar
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ComentariosAlumno { get; set; }

        /// <summary>
        /// URL del archivo entregado
        /// </summary>
        [StringLength(500)]
        public string ArchivoUrl { get; set; }

        /// <summary>
        /// Nombre original del archivo
        /// </summary>
        [StringLength(200)]
        public string ArchivoNombre { get; set; }

        /// <summary>
        /// Tamaño del archivo en bytes
        /// </summary>
        public long? ArchivoTamano { get; set; }

        /// <summary>
        /// Estado de la entrega
        /// </summary>
        [Required]
        public EstatusEntrega Estatus { get; set; }

        /// <summary>
        /// Indica si la entrega es tardía
        /// </summary>
        [Required]
        public bool EsTardia { get; set; }

        /// <summary>
        /// Número de intentos de entrega (para permitir reenvíos)
        /// </summary>
        public int NumeroIntento { get; set; }

        #endregion

        #region Evaluación

        /// <summary>
        /// Calificación obtenida (0-100 o según escala de la escuela)
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Calificacion { get; set; }

        /// <summary>
        /// Calificación original antes de penalizaciones
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal? CalificacionOriginal { get; set; }

        /// <summary>
        /// Penalización aplicada por entrega tardía
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal? PenalizacionAplicada { get; set; }

        /// <summary>
        /// Retroalimentación del maestro
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Retroalimentacion { get; set; }

        /// <summary>
        /// Maestro que revisó la tarea
        /// </summary>
        public int? RevisadoPorId { get; set; }

        /// <summary>
        /// Fecha de revisión
        /// </summary>
        public DateTime? FechaRevision { get; set; }

        #endregion

        #region Archivos de Retroalimentación

        /// <summary>
        /// URL del archivo con retroalimentación (opcional)
        /// </summary>
        [StringLength(500)]
        public string ArchivoRetroalimentacionUrl { get; set; }

        /// <summary>
        /// Nombre del archivo de retroalimentación
        /// </summary>
        [StringLength(200)]
        public string ArchivoRetroalimentacionNombre { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Tarea relacionada
        /// </summary>
        [ForeignKey("TareaId")]
        public virtual Tarea Tarea { get; set; }

        /// <summary>
        /// Alumno que entregó
        /// </summary>
        [ForeignKey("AlumnoId")]
        public virtual Alumno? Alumno { get; set; }

        /// <summary>
        /// Maestro que revisó
        /// </summary>
        [ForeignKey("RevisadoPorId")]
        public virtual Maestro RevisadoPor { get; set; }

        #endregion

        #region Auditoría (IAuditableEntity)

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        #endregion

        #region Constructor

        public TareaEntrega()
        {
            Estatus = EstatusEntrega.Pendiente;
            EsTardia = false;
            NumeroIntento = 1;
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si la entrega ya fue calificada
        /// </summary>
        public bool EstaCalificada => Calificacion.HasValue;

        /// <summary>
        /// Indica si fue aprobada (calificación >= 60 o según configuración)
        /// </summary>
        public bool Aprobada => Calificacion.HasValue && Calificacion.Value >= 60;

        /// <summary>
        /// Días de retraso en la entrega
        /// </summary>
        public int? DiasRetraso
        {
            get
            {
                if (!FechaEntrega.HasValue || Tarea == null || !EsTardia) return null;
                return (FechaEntrega.Value.Date - Tarea.FechaEntrega.Date).Days;
            }
        }

        /// <summary>
        /// Indica si el alumno no entregó nada
        /// </summary>
        public bool NoEntregada => Estatus == EstatusEntrega.NoEntregada;

        /// <summary>
        /// Indica si está pendiente de entregar
        /// </summary>
        public bool Pendiente => Estatus == EstatusEntrega.Pendiente;

        /// <summary>
        /// Indica si ya fue entregada pero no revisada
        /// </summary>
        public bool PendienteRevision => Estatus == EstatusEntrega.Entregada;

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Registra la entrega de la tarea
        /// </summary>
        /// <param name="archivoUrl">URL del archivo entregado</param>
        /// <param name="nombreArchivo">Nombre del archivo</param>
        /// <param name="tamanoArchivo">Tamaño en bytes</param>
        /// <param name="comentarios">Comentarios del alumno</param>
        /// <param name="esTardia">Si la entrega es tardía</param>
        public void RegistrarEntrega(string archivoUrl, string nombreArchivo, long tamanoArchivo,
                                     string comentarios, bool esTardia)
        {
            FechaEntrega = DateTime.Now;
            ArchivoUrl = archivoUrl;
            ArchivoNombre = nombreArchivo;
            ArchivoTamano = tamanoArchivo;
            ComentariosAlumno = comentarios;
            EsTardia = esTardia;
            Estatus = EstatusEntrega.Entregada;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Permite reenviar la tarea (incrementa el número de intento)
        /// </summary>
        public void Reenviar(string archivoUrl, string nombreArchivo, long tamanoArchivo, string comentarios)
        {
            NumeroIntento++;
            FechaEntrega = DateTime.Now;
            ArchivoUrl = archivoUrl;
            ArchivoNombre = nombreArchivo;
            ArchivoTamano = tamanoArchivo;
            ComentariosAlumno = comentarios;
            Estatus = EstatusEntrega.Entregada;

            // Limpiar calificación anterior
            Calificacion = null;
            CalificacionOriginal = null;
            PenalizacionAplicada = null;
            Retroalimentacion = null;
            RevisadoPorId = null;
            FechaRevision = null;

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Califica la entrega
        /// </summary>
        /// <param name="calificacion">Calificación asignada</param>
        /// <param name="retroalimentacion">Comentarios del maestro</param>
        /// <param name="maestroId">ID del maestro que califica</param>
        /// <param name="aplicarPenalizacion">Si se debe aplicar penalización por tardía</param>
        public void Calificar(decimal calificacion, string retroalimentacion, int maestroId, bool aplicarPenalizacion = true)
        {
            CalificacionOriginal = calificacion;

            if (EsTardia && aplicarPenalizacion && Tarea?.PenalizacionTardia != null)
            {
                var penalizacion = calificacion * (Tarea.PenalizacionTardia.Value / 100);
                PenalizacionAplicada = penalizacion;
                Calificacion = calificacion - penalizacion;

                // No permitir calificaciones negativas
                if (Calificacion < 0) Calificacion = 0;
            }
            else
            {
                Calificacion = calificacion;
                PenalizacionAplicada = 0;
            }

            Retroalimentacion = retroalimentacion;
            RevisadoPorId = maestroId;
            FechaRevision = DateTime.Now;
            Estatus = EstatusEntrega.Revisada;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Marca como no entregada (cuando venció el plazo)
        /// </summary>
        public void MarcarComoNoEntregada()
        {
            Estatus = EstatusEntrega.NoEntregada;
            Calificacion = 0;
            CalificacionOriginal = 0;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Adjunta archivo de retroalimentación del maestro
        /// </summary>
        public void AdjuntarRetroalimentacion(string url, string nombreArchivo)
        {
            ArchivoRetroalimentacionUrl = url;
            ArchivoRetroalimentacionNombre = nombreArchivo;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Recalifica la entrega
        /// </summary>
        public void Recalificar(decimal nuevaCalificacion, string nuevaRetroalimentacion, int maestroId)
        {
            var calificacionAnterior = Calificacion;
            Calificar(nuevaCalificacion, nuevaRetroalimentacion, maestroId, EsTardia);

            // Agregar nota sobre recalificación
            Retroalimentacion = $"[RECALIFICACIÓN - Anterior: {calificacionAnterior}]\n{Retroalimentacion}";
        }

        /// <summary>
        /// Valida que la entrega sea correcta
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (Estatus == EstatusEntrega.Entregada || Estatus == EstatusEntrega.Revisada)
            {
                if (!FechaEntrega.HasValue)
                    errores.Add("La fecha de entrega es requerida");

                if (string.IsNullOrWhiteSpace(ArchivoUrl))
                    errores.Add("El archivo de entrega es requerido");
            }

            if (Estatus == EstatusEntrega.Revisada)
            {
                if (!Calificacion.HasValue)
                    errores.Add("La calificación es requerida para entregas revisadas");

                if (!RevisadoPorId.HasValue)
                    errores.Add("El maestro revisor es requerido");

                if (Calificacion.HasValue && (Calificacion < 0 || Calificacion > 100))
                    errores.Add("La calificación debe estar entre 0 y 100");
            }

            return errores;
        }

        #endregion
    }
}