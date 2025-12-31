using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class UpdateInscripcionDto
    {
        public int Id { get; set; }

        [Required]
        public int GrupoId { get; set; } // Por si queremos moverlo de grupo

        // 1=Inscrito, 2=BajaTemporal, 3=BajaDefinitiva, 4=Finalizado
        public int Estatus { get; set; }

        public int? NumeroLista { get; set; }
        public bool Becado { get; set; }
        public bool Repetidor { get; set; }
        public string? Observaciones { get; set; }
    }
}
