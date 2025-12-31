using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Finanzas
{
    public class PagoDto
    {
        public int Id { get; set; }
        public int CargoId { get; set; }
        public string DescripcionCargo { get; set; } // Ej: "Colegiatura Octubre"
        public int AlumnoId { get; set; }
        public string NombreAlumno { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; }
        public DateTime FechaPago { get; set; }
        public string FolioRecibo { get; set; }
        public bool Cancelado { get; set; }
        public string Observaciones { get; set; }
    }
}
