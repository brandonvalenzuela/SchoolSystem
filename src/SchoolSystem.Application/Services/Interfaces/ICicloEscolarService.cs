using SchoolSystem.Application.DTOs.Ciclos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Interfaces
{
    public interface ICicloEscolarService
    {
        Task<CicloEscolarActualDto?> GetActualAsync(int escuelaId);
    }
}
