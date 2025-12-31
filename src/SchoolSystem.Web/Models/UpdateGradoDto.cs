using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class UpdateGradoDto
    {
        public int Id { get; set; }

        [Required] public int NivelEducativoId { get; set; }
        [Required] public string Nombre { get; set; }
        [Required] public int Orden { get; set; }
        public bool Activo { get; set; }
        public int? EdadRecomendada { get; set; }
        public int? CapacidadMaximaPorGrupo { get; set; }
    }
}
