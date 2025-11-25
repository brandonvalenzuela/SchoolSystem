using SchoolSystem.Domain.Enums.Escuelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Domain.Constants
{
    public static class Roles
    {
        // Roles individuales basados en el Enum
        public const string SuperAdmin = nameof(RolUsuario.SuperAdmin);
        public const string Director = nameof(RolUsuario.Director);
        public const string Subdirector = nameof(RolUsuario.Subdirector);
        public const string Maestro = nameof(RolUsuario.Maestro);
        public const string Padre = nameof(RolUsuario.Padre);
        public const string Alumno = nameof(RolUsuario.Alumno);

        // --- COMBINACIONES ÚTILES ---

        // Admin: Cualquiera con poder administrativo
        public const string Admin = SuperAdmin + "," + Director + "," + Subdirector;

        // Staff: Administrativos + Maestros (para ver listas, asistencias, etc.)
        public const string Staff = Admin + "," + Maestro;

        // ReadOnly: Usuarios que generalmente solo consultan
        public const string Consultores = Padre + "," + Alumno;
    }
}
