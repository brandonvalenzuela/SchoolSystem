namespace SchoolSystem.Web.Models
{
    public class RecalificarDecision
    {
        public bool PermitirRecalificarExistentes { get; set; }

        /// <summary>
        /// Motivo de recalificación. Requerido si PermitirRecalificarExistentes es true.
        /// </summary>
        public string? MotivoRecalificacion { get; set; }
    }
}
