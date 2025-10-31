using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Domain.Enums.Escuelas
{
    /// <summary>
    /// Roles de usuario en el sistema
    /// </summary>
    public enum RolUsuario
    {
        /// <summary>
        /// Super administrador del sistema (acceso total)
        /// </summary>
        SuperAdmin = 1,

        /// <summary>
        /// Director de la escuela
        /// </summary>
        Director = 2,

        /// <summary>
        /// Subdirector de la escuela
        /// </summary>
        Subdirector = 3,

        /// <summary>
        /// Personal administrativo
        /// </summary>
        Administrativo = 4,

        /// <summary>
        /// Maestro/Profesor
        /// </summary>
        Maestro = 5,

        /// <summary>
        /// Padre de familia o tutor
        /// </summary>
        Padre = 6,

        /// <summary>
        /// Alumno (si tiene acceso al sistema)
        /// </summary>
        Alumno = 7
    }
}
