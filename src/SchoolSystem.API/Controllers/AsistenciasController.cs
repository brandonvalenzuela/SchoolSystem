using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Asistencias;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Constants;
using System.Threading.Tasks;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AsistenciasController : ControllerBase
    {
        private readonly IAsistenciaService _service;

        public AsistenciasController(IAsistenciaService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene una lista paginada de registros de asistencia.
        /// </summary>
        /// <param name="page">Número de página (default: 1)</param>
        /// <param name="size">Tamaño de página (default: 10)</param>
        /// <returns>Lista paginada de asistencias envuelta en ApiResponse</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<AsistenciaDto>>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetPagedAsync(page, size);
            var response = new ApiResponse<PagedResult<AsistenciaDto>>(result, "Listado de asistencias obtenido exitosamente.");
            return Ok(response);
        }

        /// <summary>
        /// Obtiene un registro de asistencia por su ID.
        /// </summary>
        /// <param name="id">ID de la asistencia</param>
        /// <returns>AsistenciaDto envuelto en ApiResponse</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AsistenciaDto>>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound(new ApiResponse<AsistenciaDto>("Registro de asistencia no encontrado."));

            return Ok(new ApiResponse<AsistenciaDto>(result, "Asistencia encontrada exitosamente."));
        }

        /// <summary>
        /// Registra una nueva asistencia.
        /// </summary>
        /// <param name="dto">Datos de la asistencia</param>
        /// <returns>ID de la asistencia creada envuelto en ApiResponse</returns>
        [HttpPost]
        [Authorize(Roles = Roles.Staff)]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateAsistenciaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos de asistencia inválidos."));

            var id = await _service.CreateAsync(dto);
            var response = new ApiResponse<int>(id, "Asistencia registrada exitosamente.");

            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        /// <summary>
        /// Actualiza un registro de asistencia existente (ej. para justificar una falta).
        /// </summary>
        /// <param name="id">ID de la asistencia a actualizar</param>
        /// <param name="dto">Datos actualizados</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Staff)]
        public async Task<ActionResult<ApiResponse<int>>> Update(int id, [FromBody] UpdateAsistenciaDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new ApiResponse<int>("El ID de la URL no coincide con el ID del cuerpo de la solicitud."));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos inválidos."));

            await _service.UpdateAsync(id, dto);

            // Se usa Ok en lugar de NoContent para devolver el mensaje de éxito
            return Ok(new ApiResponse<int>(id, "Registro de asistencia actualizado exitosamente."));
        }

        /// <summary>
        /// Elimina (lógicamente) un registro de asistencia.
        /// </summary>
        /// <param name="id">ID de la asistencia</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ApiResponse<int>>> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new ApiResponse<int>(id, "Registro de asistencia eliminado exitosamente."));
        }

        /// <summary>
        /// Registra la asistencia masiva para un grupo completo.
        /// </summary>
        /// <param name="dto">Datos de la asistencia grupal.</param>
        /// <returns>Cantidad de registros creados.</returns>
        [HttpPost("masivo")]
        [Authorize(Roles = Roles.Staff)] // Maestros y Admin
        public async Task<ActionResult<ApiResponse<int>>> CreateMasivo([FromBody] CreateAsistenciaMasivaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos de asistencia inválidos."));

            try
            {
                var count = await _service.CreateMasivoAsync(dto);
                return Ok(new ApiResponse<int>(count, $"{count} asistencias registradas exitosamente."));
            }
            catch (InvalidOperationException ex)
            {
                // Capturamos la validación de duplicados
                return BadRequest(new ApiResponse<int>(ex.Message));
            }
        }


        /// <summary>
        /// Obtiene la lista de asistencia de un grupo para una fecha específica.
        /// Útil para verificar si ya se tomó lista o para editarla.
        /// </summary>
        [HttpGet("grupo/{grupoId}/fecha/{fecha}")]
        public async Task<ActionResult<ApiResponse<List<AsistenciaDto>>>> GetByGrupoFecha(int grupoId, DateTime fecha)
        {
            var result = await _service.GetByGrupoAndFechaAsync(grupoId, fecha);

            if (result == null || !result.Any())
                return NotFound(new ApiResponse<List<AsistenciaDto>>("No se encontraron registros para esa fecha."));

            return Ok(new ApiResponse<List<AsistenciaDto>>(result, "Asistencias encontradas."));
        }

        // B. HISTORIAL DE UN ALUMNO
        /// <summary>
        /// Obtiene el historial completo de asistencias de un alumno.
        /// </summary>
        [HttpGet("alumno/{alumnoId}")]
        [Authorize(Roles = Roles.Staff + "," + Roles.Padre + "," + Roles.Alumno)] // Padres y Alumnos también pueden ver esto
        public async Task<ActionResult<ApiResponse<List<AsistenciaDto>>>> GetHistorialAlumno(int alumnoId)
        {
            // TODO: Aquí deberías validar que si el usuario es Padre, el alumno sea su hijo.
            var historial = await _service.GetHistorialByAlumnoAsync(alumnoId);
            return Ok(new ApiResponse<List<AsistenciaDto>>(historial, "Historial obtenido correctamente."));
        }

        // C. JUSTIFICAR FALTA
        /// <summary>
        /// Justifica una falta o retardo existente.
        /// </summary>
        [HttpPost("{id}/justificar")]
        [Authorize(Roles = Roles.Staff)] // Solo maestros o admin pueden justificar
        public async Task<ActionResult<ApiResponse<bool>>> JustificarFalta(int id, [FromBody] JustificarFaltaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<bool>("Datos inválidos."));

            // Obtener ID del usuario actual (suponiendo que tienes el servicio CurrentUser)
            // var userId = _currentUserService.UserId;
            var userId = 1; // Temporal

            try
            {
                await _service.JustificarFaltaAsync(id, dto, userId);
                return Ok(new ApiResponse<bool>(true, "Falta justificada exitosamente."));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<bool>(ex.Message));
            }
        }

        // D. REPORTE MENSUAL
        /// <summary>
        /// Obtiene el reporte mensual de asistencia (Sábana) para un grupo.
        /// </summary>
        [HttpGet("reporte/mensual")]
        [Authorize(Roles = Roles.Staff)] // Directores y Maestros
        public async Task<ActionResult<ApiResponse<List<ReporteMensualDto>>>> GetReporteMensual(
            [FromQuery] int grupoId,
            [FromQuery] int mes,
            [FromQuery] int anio)
        {
            if (grupoId <= 0 || mes < 1 || mes > 12 || anio < 2000)
                return BadRequest(new ApiResponse<List<ReporteMensualDto>>("Parámetros de reporte inválidos."));

            var reporte = await _service.GetReporteMensualAsync(grupoId, mes, anio);
            return Ok(new ApiResponse<List<ReporteMensualDto>>(reporte, $"Reporte generado para {mes}/{anio}."));
        }
    }
}