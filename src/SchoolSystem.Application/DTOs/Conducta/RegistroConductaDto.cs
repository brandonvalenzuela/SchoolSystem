using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Conducta
{
    public class RegistroConductaDto
    {
        public int Id { get; set; }
        public int EscuelaId { get; set; }

        public int AlumnoId { get; set; }
        public string NombreAlumno { get; set; }

        public int MaestroId { get; set; }
        public string NombreMaestro { get; set; }

        public string Tipo { get; set; } // Positiva / Negativa
        public string Categoria { get; set; } // Académica, Disciplina, etc.
        public string Gravedad { get; set; } // Leve, Grave...

        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int Puntos { get; set; } // Ej: -5 o +10

        public DateTime FechaHoraIncidente { get; set; }
        public string Estado { get; set; } // Activo, Resuelto
        public bool PadresNotificados { get; set; }
    }
}