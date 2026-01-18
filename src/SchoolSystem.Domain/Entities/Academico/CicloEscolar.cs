using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Escuelas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SchoolSystem.Domain.Entities.Academico
{
    /// <summary>
    /// Tabla: CiclosEscolares
    /// Representa un ciclo escolar por escuela (multi-tenant).
    /// </summary>
    [Table("CiclosEscolares")]
    public class CicloEscolar : BaseEntity, IAuditableEntity
    {

        /// <summary>
        /// FK a Escuelas.Id (multi-tenant). NO NULL.
        /// </summary>
        public int EscuelaId { get; set; }

        /// <summary>
        /// Navigation a Escuela.
        /// </summary>
        public virtual Escuela Escuela { get; set; }

        /// <summary>
        /// Clave del ciclo escolar (ej: "2024-2025").
        /// NOT NULL, varchar(20).
        /// Única por escuela.
        /// </summary>
        public string Clave { get; set; } = "";

        /// <summary>
        /// Nombre amigable (ej: "Ciclo 2024-2025").
        /// NULL permitido, varchar(100).
        /// </summary>
        public string? Nombre { get; set; }

        /// <summary>
        /// Fecha de inicio del ciclo.
        /// DATE (sin hora). NULL permitido.
        /// </summary>
        public DateTime? FechaInicio { get; set; }

        /// <summary>
        /// Fecha de fin del ciclo.
        /// DATE (sin hora). NULL permitido.
        /// </summary>
        public DateTime? FechaFin { get; set; }

        /// <summary>
        /// Indica si es el ciclo actual de la escuela.
        /// tinyint(1) NOT NULL DEFAULT 0
        /// </summary>
        public bool EsActual { get; set; } = false;

        /// <summary>
        /// Auditoría: fecha/hora de creación.
        /// datetime(6) NOT NULL
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Auditoría: fecha/hora de última actualización.
        /// datetime(6) NOT NULL
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Auditoría: usuario que creó.
        /// INT NULL
        /// </summary>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Auditoría: usuario que actualizó por última vez.
        /// INT NULL
        /// </summary>
        public int? UpdatedBy { get; set; }

        #region Propiedades calculadas (opcionales)

        [NotMapped]
        public bool TieneRangoFechas => FechaInicio.HasValue && FechaFin.HasValue;

        [NotMapped]
        public bool RangoValido => !TieneRangoFechas || FechaInicio!.Value.Date <= FechaFin!.Value.Date;

        [NotMapped]
        public bool EstaVigentePorFecha
        {
            get
            {
                if (!TieneRangoFechas)
                    return false;
                var hoy = DateTime.Today;
                return FechaInicio!.Value.Date <= hoy && FechaFin!.Value.Date >= hoy;
            }
        }

        #endregion

        #region Métodos de negocio (opcionales)

        public void MarcarComoActual(int usuarioId)
        {
            if (!RangoValido)
                throw new InvalidOperationException("El rango de fechas del ciclo escolar no es válido.");

            EsActual = true;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = usuarioId;
        }

        public void DesmarcarComoActual(int usuarioId)
        {
            EsActual = false;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = usuarioId;
        }

        public void ActualizarNombre(string? nombre, int usuarioId)
        {
            Nombre = string.IsNullOrWhiteSpace(nombre) ? null : nombre.Trim();
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = usuarioId;
        }

        public void ActualizarRango(DateTime? inicio, DateTime? fin, int usuarioId)
        {
            FechaInicio = inicio?.Date;
            FechaFin = fin?.Date;

            if (!RangoValido)
                throw new ArgumentException("FechaInicio no puede ser posterior a FechaFin.");

            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = usuarioId;
        }

        #endregion
    }
}
