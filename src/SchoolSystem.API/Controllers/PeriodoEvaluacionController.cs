using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Evaluacion;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Constants;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PeriodoEvaluacionController : Controller
    {
        private readonly IPeriodoEvaluacionService _service;

        public PeriodoEvaluacionController(IPeriodoEvaluacionService service)
        {
            _service = service;
        }

        [HttpGet("por-grupo/{grupoId}")]
        [Authorize(Roles = Roles.Staff)]
        public async Task<ActionResult<ApiResponse<List<PeriodoEvaluacionDto>>>> GetPorGrupo(int grupoId, [FromQuery] bool soloActivos = true)
        {
            var list = await _service.GetPorGrupoAsync(grupoId, soloActivos);
            return Ok(new ApiResponse<List<PeriodoEvaluacionDto>>(list, "Periodos obtenidos."));
        }
    }
}
