namespace SchoolSystem.Web.Models
{
    public class RecalificarDecision
    {
        public bool PermitirRecalificarExistentes { get; set; }

        /// <summary>
        /// Motivo de recalificación. Requerido si PermitirRecalificarExistentes es true.
        /// Mínimo 10 caracteres para cumplir validación de backend.
        /// </summary>
        public string? MotivoRecalificacion { get; set; }

        /// <summary>
        /// Alias para MotivoRecalificacion. Se usa internamente en el DTO.
        /// </summary>
        public string? MotivoModificacion
        {
            get => MotivoRecalificacion;
            set => MotivoRecalificacion = value;
        }
    }
}
