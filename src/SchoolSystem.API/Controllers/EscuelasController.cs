using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.ConfiguracionEscuela;
using SchoolSystem.Application.DTOs.Escuelas;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Constants;
using System.Threading.Tasks;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EscuelasController : ControllerBase
    {
        private readonly IEscuelaService _service;
        private readonly ICurrentUserService _currentUser;

        public EscuelasController(IEscuelaService service, ICurrentUserService currentUser)
        {
            _service = service;
            _currentUser = currentUser;
        }

        /// <summary>
        /// Obtiene una lista paginada de escuelas.
        /// </summary>
        /// <param name="page">Número de página (default: 1)</param>
        /// <param name="size">Tamaño de página (default: 10)</param>
        /// <returns>Lista paginada de EscuelaDto envuelta en ApiResponse</returns>
        [HttpGet]
        [Authorize(Roles = Roles.SuperAdmin)]
        public async Task<ActionResult<ApiResponse<PagedResult<EscuelaDto>>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetPagedAsync(page, size);
            var response = new ApiResponse<PagedResult<EscuelaDto>>(result, "Listado de escuelas obtenido exitosamente.");
            return Ok(response);
        }

        /// <summary>
        /// Obtiene una escuela por su ID.
        /// </summary>
        /// <param name="id">ID de la escuela</param>
        /// <returns>EscuelaDto envuelto en ApiResponse</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ApiResponse<EscuelaDto>>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound(new ApiResponse<EscuelaDto>("Escuela no encontrada."));

            return Ok(new ApiResponse<EscuelaDto>(result, "Escuela encontrada exitosamente."));
        }

        /// <summary>
        /// Registra una nueva escuela.
        /// </summary>
        /// <param name="dto">Datos de la escuela</param>
        /// <returns>ID de la escuela creada envuelto en ApiResponse</returns>
        [HttpPost]
        [Authorize(Roles = Roles.SuperAdmin)]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateEscuelaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos de la escuela inválidos."));

            var id = await _service.CreateAsync(dto);
            var response = new ApiResponse<int>(id, "Escuela registrada exitosamente.");

            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        /// <summary>
        /// Actualiza una escuela existente.
        /// </summary>
        /// <param name="id">ID de la escuela a actualizar</param>
        /// <param name="dto">Datos actualizados</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ApiResponse<int>>> Update(int id, [FromBody] UpdateEscuelaDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new ApiResponse<int>("El ID de la URL no coincide con el ID del cuerpo de la solicitud."));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos inválidos."));

            await _service.UpdateAsync(id, dto);

            return Ok(new ApiResponse<int>(id, "Escuela actualizada exitosamente."));
        }

        /// <summary>
        /// Elimina (lógicamente) una escuela.
        /// </summary>
        /// <param name="id">ID de la escuela</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.SuperAdmin)]
        public async Task<ActionResult<ApiResponse<int>>> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new ApiResponse<int>(id, "Escuela eliminada exitosamente."));
        }

        /// <summary>
        /// Obtiene un resumen estadístico de la escuela (Dashboard).
        /// </summary>
        /// <param name="id">ID de la escuela</param>
        /// <returns>Resumen con contadores y estado de licencia</returns>
        [HttpGet("{id}/resumen")]
        [Authorize(Roles = Roles.Admin)] // Solo Directores y SuperAdmin pueden ver esto
        public async Task<ActionResult<ApiResponse<ResumenEscuelaDto>>> GetResumen(int id)
        {
            // --- VALIDACIÓN DE SEGURIDAD HORIZONTAL ---
            // Si NO es SuperAdmin Y el ID solicitado es diferente al ID de su escuela:
            if (!_currentUser.IsInRole(Roles.SuperAdmin) && _currentUser.EscuelaId != id)
            {
                // Retornamos 403 Forbidden (Entendí quién eres, pero no tienes permiso para ver ESTO)
                return Forbid();

                // O si prefieres usar tu wrapper estándar:
                // return StatusCode(403, new ApiResponse<ResumenEscuelaDto>("No tiene permisos para acceder a esta escuela."));
            }

            var resumen = await _service.GetResumenAsync(id);

            if (resumen == null)
                return NotFound(new ApiResponse<ResumenEscuelaDto>("Escuela no encontrada."));

            return Ok(new ApiResponse<ResumenEscuelaDto>(resumen, "Resumen generado exitosamente."));
        }

        [HttpGet("{id}/configuracion")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ApiResponse<ConfiguracionEscuelaDto>>> GetConfig(int id)
        {
            if (!_currentUser.IsInRole(Roles.SuperAdmin) && _currentUser.EscuelaId != id)
            {
                return Forbid();
            }

            // Validar seguridad horizontal (mismo código que en resumen)
            var config = await _service.GetConfiguracionAsync(id);
            return Ok(new ApiResponse<ConfiguracionEscuelaDto>(config));
        }

        [HttpPut("{id}/configuracion")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UpdateConfig(int id, [FromBody] UpdateConfiguracionEscuelaDto dto)
        {
            if (!_currentUser.IsInRole(Roles.SuperAdmin) && _currentUser.EscuelaId != id)
            {
                return Forbid();
            }

            await _service.UpdateConfiguracionAsync(id, dto);
            return Ok(new ApiResponse<bool>(true, "Configuración actualizada."));
        }

    }
}