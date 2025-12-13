using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Escuelas
{
    public class ResumenEscuelaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string PlanActual { get; set; }
        public string EstadoSuscripcion { get; set; } // "Activa", "Vencida"

        // Estadísticas
        public int TotalAlumnos { get; set; }
        public int TotalMaestros { get; set; }
        public int TotalGrupos { get; set; }

        // Capacidad (Opcional, para barras de progreso en el frontend)
        public int? CapacidadAlumnos { get; set; }
        public decimal PorcentajeOcupacionAlumnos { get; set; }
    }
}
