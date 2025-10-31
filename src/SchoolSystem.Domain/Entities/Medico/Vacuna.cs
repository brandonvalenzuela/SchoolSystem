using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolSystem.Domain.Entities.Common;

namespace SchoolSystem.Domain.Entities.Medico
{
    /// <summary>
    /// Registro de vacunas aplicadas a alumnos
    /// </summary>
    [Table("Vacunas")]
    public class Vacuna : BaseEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// Expediente médico relacionado
        /// </summary>
        [Required]
        public int ExpedienteMedicoId { get; set; }

        /// <summary>
        /// Nombre de la vacuna
        /// </summary>
        [Required]
        [StringLength(100)]
        public string NombreVacuna { get; set; }

        #endregion

        #region Información de Aplicación

        /// <summary>
        /// Dosis aplicada (primera, segunda, refuerzo, etc.)
        /// </summary>
        [StringLength(50)]
        public string Dosis { get; set; }

        /// <summary>
        /// Fecha de aplicación
        /// </summary>
        [Required]
        public DateTime FechaAplicacion { get; set; }

        /// <summary>
        /// Fecha programada para la próxima dosis
        /// </summary>
        public DateTime? FechaProximaDosis { get; set; }

        #endregion

        #region Detalles del Producto

        /// <summary>
        /// Lote de la vacuna
        /// </summary>
        [StringLength(50)]
        public string Lote { get; set; }

        /// <summary>
        /// Marca o laboratorio
        /// </summary>
        [StringLength(100)]
        public string Marca { get; set; }

        /// <summary>
        /// Fecha de caducidad
        /// </summary>
        public DateTime? FechaCaducidad { get; set; }

        #endregion

        #region Lugar de Aplicación

        /// <summary>
        /// Institución donde se aplicó
        /// </summary>
        [StringLength(200)]
        public string InstitucionAplicacion { get; set; }

        /// <summary>
        /// Médico o enfermero que aplicó
        /// </summary>
        [StringLength(200)]
        public string PersonalAplicacion { get; set; }

        /// <summary>
        /// Lugar anatómico de aplicación (brazo izquierdo, etc.)
        /// </summary>
        [StringLength(100)]
        public string LugarAnatomico { get; set; }

        #endregion

        #region Reacciones

        /// <summary>
        /// Indica si hubo reacciones adversas
        /// </summary>
        public bool TuvoReacciones { get; set; }

        /// <summary>
        /// Descripción de las reacciones adversas
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string DescripcionReacciones { get; set; }

        #endregion

        #region Documentación

        /// <summary>
        /// URL del comprobante o certificado
        /// </summary>
        [StringLength(500)]
        public string ComprobanteUrl { get; set; }

        /// <summary>
        /// Observaciones adicionales
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Observaciones { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Expediente médico relacionado
        /// </summary>
        [ForeignKey("ExpedienteMedicoId")]
        public virtual ExpedienteMedico ExpedienteMedico { get; set; }

        #endregion

        #region Constructor

        public Vacuna()
        {
            FechaAplicacion = DateTime.Now;
            TuvoReacciones = false;
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si tiene próxima dosis programada
        /// </summary>
        public bool TieneProximaDosis => FechaProximaDosis.HasValue;

        /// <summary>
        /// Indica si la próxima dosis está vencida
        /// </summary>
        public bool ProximaDosisVencida => FechaProximaDosis.HasValue && DateTime.Now > FechaProximaDosis.Value;

        /// <summary>
        /// Días hasta la próxima dosis
        /// </summary>
        public int? DiasHastaProximaDosis
        {
            get
            {
                if (!FechaProximaDosis.HasValue) return null;
                return (FechaProximaDosis.Value.Date - DateTime.Now.Date).Days;
            }
        }

        /// <summary>
        /// Indica si la próxima dosis está próxima (menos de 30 días)
        /// </summary>
        public bool ProximaDosisProxima
        {
            get
            {
                var dias = DiasHastaProximaDosis;
                return dias.HasValue && dias.Value > 0 && dias.Value <= 30;
            }
        }

        /// <summary>
        /// Años desde la aplicación
        /// </summary>
        public int AniosDesdeAplicacion => DateTime.Now.Year - FechaAplicacion.Year;

        /// <summary>
        /// Indica si tiene comprobante
        /// </summary>
        public bool TieneComprobante => !string.IsNullOrWhiteSpace(ComprobanteUrl);

        /// <summary>
        /// Indica si el lote está caducado
        /// </summary>
        public bool LoteCaducado => FechaCaducidad.HasValue && DateTime.Now > FechaCaducidad.Value;

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Programa la próxima dosis
        /// </summary>
        public void ProgramarProximaDosis(DateTime fecha)
        {
            if (fecha <= FechaAplicacion)
                throw new ArgumentException("La fecha de la próxima dosis debe ser posterior a la fecha de aplicación");

            FechaProximaDosis = fecha;
        }

        /// <summary>
        /// Registra reacciones adversas
        /// </summary>
        public void RegistrarReacciones(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("La descripción de las reacciones es requerida");

            TuvoReacciones = true;
            DescripcionReacciones = descripcion;
        }

        /// <summary>
        /// Adjunta comprobante
        /// </summary>
        public void AdjuntarComprobante(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("La URL del comprobante es requerida");

            ComprobanteUrl = url;
        }

        /// <summary>
        /// Valida que la vacuna sea correcta
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(NombreVacuna))
                errores.Add("El nombre de la vacuna es requerido");

            if (FechaAplicacion > DateTime.Now)
                errores.Add("La fecha de aplicación no puede ser futura");

            if (FechaProximaDosis.HasValue && FechaProximaDosis.Value <= FechaAplicacion)
                errores.Add("La fecha de la próxima dosis debe ser posterior a la fecha de aplicación");

            if (FechaCaducidad.HasValue && FechaCaducidad.Value < FechaAplicacion)
                errores.Add("La fecha de caducidad no puede ser anterior a la fecha de aplicación");

            if (TuvoReacciones && string.IsNullOrWhiteSpace(DescripcionReacciones))
                errores.Add("Debe especificar la descripción de las reacciones adversas");

            return errores;
        }

        #endregion
    }
}