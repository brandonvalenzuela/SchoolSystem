using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Enums.Finanzas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Domain.Entities.Finanzas
{
    /// <summary>
    /// Conceptos de pago configurables por la escuela
    /// </summary>
    [Table("ConceptosPago")]
    public class ConceptoPago : BaseEntity, IAuditableEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// Nombre del concepto de pago
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        /// <summary>
        /// Descripción del concepto
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Monto base del concepto
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal MontoBase { get; set; }

        /// <summary>
        /// Tipo de concepto
        /// </summary>
        [Required]
        public TipoConceptoPago Tipo { get; set; }

        #endregion

        #region Recurrencia

        /// <summary>
        /// Indica si es un concepto recurrente
        /// </summary>
        [Required]
        public bool Recurrente { get; set; }

        /// <summary>
        /// Periodicidad del pago (si es recurrente)
        /// </summary>
        public PeriodicidadPago? Periodicidad { get; set; }

        #endregion

        #region Configuración

        /// <summary>
        /// Indica si el concepto está activo
        /// </summary>
        [Required]
        public bool Activo { get; set; }

        /// <summary>
        /// Código del concepto (para referencia)
        /// </summary>
        [StringLength(20)]
        public string Codigo { get; set; }

        /// <summary>
        /// Indica si aplica descuentos
        /// </summary>
        public bool AplicaDescuentos { get; set; }

        /// <summary>
        /// Porcentaje máximo de descuento permitido
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal? PorcentajeMaximoDescuento { get; set; }

        /// <summary>
        /// Indica si tiene mora por retraso
        /// </summary>
        public bool TieneMora { get; set; }

        /// <summary>
        /// Porcentaje de mora por retraso
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal? PorcentajeMora { get; set; }

        /// <summary>
        /// Días de gracia antes de aplicar mora
        /// </summary>
        public int? DiasGracia { get; set; }

        #endregion

        #region Aplicabilidad

        /// <summary>
        /// Nivel educativo al que aplica (null = todos)
        /// </summary>
        public int? NivelEducativoId { get; set; }

        /// <summary>
        /// Grado al que aplica (null = todos)
        /// </summary>
        public int? GradoId { get; set; }

        /// <summary>
        /// Ciclo escolar al que aplica
        /// </summary>
        [StringLength(20)]
        public string CicloEscolar { get; set; }

        #endregion

        #region Metadata

        /// <summary>
        /// Cuenta contable (para integración con sistemas contables)
        /// </summary>
        [StringLength(50)]
        public string CuentaContable { get; set; }

        /// <summary>
        /// Categoría fiscal (IVA, exento, etc.)
        /// </summary>
        [StringLength(50)]
        public string CategoriaFiscal { get; set; }

        /// <summary>
        /// Notas adicionales
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Notas { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Nivel educativo relacionado
        /// </summary>
        [ForeignKey("NivelEducativoId")]
        public virtual NivelEducativo NivelEducativo { get; set; }

        /// <summary>
        /// Grado relacionado
        /// </summary>
        [ForeignKey("GradoId")]
        public virtual Grado Grado { get; set; }

        /// <summary>
        /// Cargos generados con este concepto
        /// </summary>
        public virtual ICollection<Cargo> Cargos { get; set; }

        #endregion

        #region Auditoría (IAuditableEntity)

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        #endregion

        #region Constructor

        public ConceptoPago()
        {
            Activo = true;
            Recurrente = false;
            AplicaDescuentos = true;
            TieneMora = false;
            DiasGracia = 0;
            Cargos = new HashSet<Cargo>();
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si es colegiatura
        /// </summary>
        public bool EsColegiatura => Tipo == TipoConceptoPago.Colegiatura;

        /// <summary>
        /// Indica si es inscripción
        /// </summary>
        public bool EsInscripcion => Tipo == TipoConceptoPago.Inscripcion;

        /// <summary>
        /// Indica si aplica a todos los niveles
        /// </summary>
        public bool AplicaATodosLosNiveles => !NivelEducativoId.HasValue;

        /// <summary>
        /// Indica si aplica a todos los grados
        /// </summary>
        public bool AplicaATodosLosGrados => !GradoId.HasValue;

        /// <summary>
        /// Nombre completo con código
        /// </summary>
        public string NombreCompleto => string.IsNullOrWhiteSpace(Codigo) ? Nombre : $"{Codigo} - {Nombre}";

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Calcula el monto con descuento aplicado
        /// </summary>
        /// <param name="porcentajeDescuento">Porcentaje de descuento a aplicar</param>
        /// <returns>Monto con descuento</returns>
        public decimal CalcularMontoConDescuento(decimal porcentajeDescuento)
        {
            if (!AplicaDescuentos)
                return MontoBase;

            if (porcentajeDescuento < 0 || porcentajeDescuento > 100)
                throw new ArgumentException("El porcentaje de descuento debe estar entre 0 y 100");

            if (PorcentajeMaximoDescuento.HasValue && porcentajeDescuento > PorcentajeMaximoDescuento.Value)
                throw new InvalidOperationException($"El descuento no puede exceder el {PorcentajeMaximoDescuento}%");

            var descuento = MontoBase * (porcentajeDescuento / 100);
            return MontoBase - descuento;
        }

        /// <summary>
        /// Calcula la mora por días de retraso
        /// </summary>
        /// <param name="diasRetraso">Días de retraso en el pago</param>
        /// <returns>Monto de mora</returns>
        public decimal CalcularMora(int diasRetraso)
        {
            if (!TieneMora || !PorcentajeMora.HasValue)
                return 0;

            var diasGracia = DiasGracia ?? 0;
            if (diasRetraso <= diasGracia)
                return 0;

            var diasMora = diasRetraso - diasGracia;
            var mora = MontoBase * (PorcentajeMora.Value / 100) * diasMora;

            return Math.Round(mora, 2);
        }

        /// <summary>
        /// Calcula el monto total con mora
        /// </summary>
        /// <param name="diasRetraso">Días de retraso</param>
        /// <param name="montoOriginal">Monto original (puede ser con descuento)</param>
        /// <returns>Monto total con mora</returns>
        public decimal CalcularMontoConMora(int diasRetraso, decimal? montoOriginal = null)
        {
            var monto = montoOriginal ?? MontoBase;
            var mora = CalcularMora(diasRetraso);
            return monto + mora;
        }

        /// <summary>
        /// Activa el concepto de pago
        /// </summary>
        public void Activar()
        {
            if (!Activo)
            {
                Activo = true;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Desactiva el concepto de pago
        /// </summary>
        public void Desactivar()
        {
            if (Activo)
            {
                Activo = false;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Actualiza el monto base
        /// </summary>
        public void ActualizarMonto(decimal nuevoMonto)
        {
            if (nuevoMonto < 0)
                throw new ArgumentException("El monto no puede ser negativo");

            MontoBase = nuevoMonto;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Configura la recurrencia
        /// </summary>
        public void ConfigurarRecurrencia(bool esRecurrente, PeriodicidadPago? periodicidad = null)
        {
            Recurrente = esRecurrente;

            if (esRecurrente && !periodicidad.HasValue)
                throw new ArgumentException("Debe especificar la periodicidad si es recurrente");

            Periodicidad = esRecurrente ? periodicidad : null;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Configura los descuentos
        /// </summary>
        public void ConfigurarDescuentos(bool aplicaDescuentos, decimal? porcentajeMaximo = null)
        {
            AplicaDescuentos = aplicaDescuentos;

            if (aplicaDescuentos && porcentajeMaximo.HasValue)
            {
                if (porcentajeMaximo < 0 || porcentajeMaximo > 100)
                    throw new ArgumentException("El porcentaje máximo debe estar entre 0 y 100");

                PorcentajeMaximoDescuento = porcentajeMaximo;
            }
            else if (!aplicaDescuentos)
            {
                PorcentajeMaximoDescuento = null;
            }

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Configura la mora
        /// </summary>
        public void ConfigurarMora(bool tieneMora, decimal? porcentajeMora = null, int? diasGracia = null)
        {
            TieneMora = tieneMora;

            if (tieneMora)
            {
                if (!porcentajeMora.HasValue || porcentajeMora < 0)
                    throw new ArgumentException("Debe especificar un porcentaje de mora válido");

                PorcentajeMora = porcentajeMora;
                DiasGracia = diasGracia ?? 0;
            }
            else
            {
                PorcentajeMora = null;
                DiasGracia = null;
            }

            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Establece el alcance del concepto
        /// </summary>
        public void EstablecerAlcance(int? nivelEducativoId = null, int? gradoId = null, string cicloEscolar = null)
        {
            NivelEducativoId = nivelEducativoId;
            GradoId = gradoId;
            CicloEscolar = cicloEscolar;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Valida que el concepto sea correcto
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Nombre))
                errores.Add("El nombre es requerido");

            if (Nombre?.Length > 100)
                errores.Add("El nombre no puede exceder 100 caracteres");

            if (MontoBase < 0)
                errores.Add("El monto base no puede ser negativo");

            if (Recurrente && !Periodicidad.HasValue)
                errores.Add("Debe especificar la periodicidad si es recurrente");

            if (AplicaDescuentos && PorcentajeMaximoDescuento.HasValue)
            {
                if (PorcentajeMaximoDescuento < 0 || PorcentajeMaximoDescuento > 100)
                    errores.Add("El porcentaje máximo de descuento debe estar entre 0 y 100");
            }

            if (TieneMora)
            {
                if (!PorcentajeMora.HasValue)
                    errores.Add("Debe especificar el porcentaje de mora");
                else if (PorcentajeMora < 0)
                    errores.Add("El porcentaje de mora no puede ser negativo");

                if (DiasGracia.HasValue && DiasGracia < 0)
                    errores.Add("Los días de gracia no pueden ser negativos");
            }

            return errores;
        }

        #endregion
    }
}