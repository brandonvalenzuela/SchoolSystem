using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Academicos
{
    public class CreateAlumnoPadreDto
    {
        [Required] public int AlumnoId { get; set; }
        [Required] public int PadreId { get; set; }
        [Required] public string Relacion { get; set; } // Padre, Madre, Tutor
        public bool EsTutorPrincipal { get; set; }
        public bool ViveConAlumno { get; set; }
        public bool AutorizadoRecoger { get; set; }
        public bool RecibeNotificaciones { get; set; }
    }
}
