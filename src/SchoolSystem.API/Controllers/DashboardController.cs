using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Dashboard;
using SchoolSystem.Application.Services.Interfaces;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _service;

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<DashboardDto>>> Get()
        {
            var stats = await _service.GetStatsAsync();
            return Ok(new ApiResponse<DashboardDto>(stats, "Estadísticas obtenidas."));
        }
    }
}
