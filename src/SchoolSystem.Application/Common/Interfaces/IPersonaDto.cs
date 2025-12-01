using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Common.Interfaces
{
    public interface IPersonaDto
    {
        string Nombre { get; set; }
        string ApellidoPaterno { get; set; }
        string ApellidoMaterno { get; set; }
        string Email { get; set; }
        string Telefono { get; set; }
        DateTime? FechaNacimiento { get; set; } // o DateTime? si así lo tienes
    }
}
