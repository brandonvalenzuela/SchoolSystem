using SchoolSystem.Domain.Enums.Conducta;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Conducta
{
    public class UpdateRegistroConductaDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(200)]
        public string Titulo { get; set; }

        [StringLength(2000)]
        public string Descripcion { get; set; }

        public int? Puntos { get; set; }

        public EstadoRegistroConducta? Estado { get; set; }

        public bool? PadresNotificados { get; set; }

        [StringLength(1000)]
        public string AccionesTomadas { get; set; }
    }
}
