using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Interfaces
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        int? EscuelaId { get; } // <--- Agrega esto
        bool IsInRole(string role); // <--- Agrega esto para facilitar la validación
    }
}
