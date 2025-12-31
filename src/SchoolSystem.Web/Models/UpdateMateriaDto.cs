using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class UpdateMateriaDto
    {
        public int Id { get; set; }
        [Required] public string Nombre { get; set; }
        [Required] public string Clave { get; set; }
        public int Area { get; set; }
        public int Tipo { get; set; }
        public bool Activo { get; set; }
    }
}
