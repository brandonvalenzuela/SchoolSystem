using SchoolSystem.Domain.Enums.Conducta;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Conducta
{
    public class CreateRegistroConductaDto
    {
        [Required]
        public int EscuelaId { get; set; }

        [Required(ErrorMessage = "El alumno es obligatorio.")]
        public int AlumnoId { get; set; }

        [Required(ErrorMessage = "El maestro es obligatorio.")]
        public int MaestroId { get; set; }

        public int? GrupoId { get; set; }

        [Required]
        public TipoConducta Tipo { get; set; }

        [Required]
        public CategoriaConducta Categoria { get; set; }

        public GravedadIncidente? Gravedad { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(2000)]
        public string Descripcion { get; set; }

        [Required]
        public int Puntos { get; set; } // El frontend debe enviar positivo o negativo según corresponda

        public DateTime? FechaHoraIncidente { get; set; } // Si es null, el servicio pondrá DateTime.Now
    }
}
