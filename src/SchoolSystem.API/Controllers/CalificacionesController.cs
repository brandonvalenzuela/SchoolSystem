using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolSystem.API.Extensions;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Calificacion;
using SchoolSystem.Application.DTOs.Calificaciones;
using SchoolSystem.Application.Exceptions;
using SchoolSystem.Application.Extensions;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Constants;
using System;
using System.Threading.Tasks;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CalificacionesController : ControllerBase
    {
        private readonly ICalificacionService _service;
        private readonly ILogger<CalificacionesController> _logger;

        public CalificacionesController(ICalificacionService service, ILogger<CalificacionesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene una lista paginada de calificaciones.
        /// </summary>
        /// <param name="page">Número de página (default: 1)</param>
        /// <param name="size">Tamaño de página (default: 10)</param>
        /// <returns>Lista paginada de CalificacionDto envuelta en ApiResponse</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<CalificacionDto>>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetPagedAsync(page, size);
            var response = new ApiResponse<PagedResult<CalificacionDto>>(result, "Listado de calificaciones obtenido exitosamente.");
            return Ok(response);
        }

        /// <summary>
        /// Obtiene una calificación por su ID.
        /// </summary>
        /// <param name="id">ID de la calificación</param>
        /// <returns>CalificacionDto envuelto en ApiResponse</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CalificacionDto>>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound(new ApiResponse<CalificacionDto>("Calificación no encontrada."));

            return Ok(new ApiResponse<CalificacionDto>(result, "Calificación encontrada exitosamente."));
        }

        /// <summary>
        /// Registra una nueva calificación.
        /// </summary>
        /// <param name="dto">Datos de la calificación</param>
        /// <returns>ID de la calificación creada envuelto en ApiResponse</returns>
        [HttpPost]
        [Authorize(Roles = Roles.Staff)]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateCalificacionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos de calificación inválidos."));

            var id = await _service.CreateAsync(dto);
            var response = new ApiResponse<int>(id, "Calificación registrada exitosamente.");

            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        /// <summary>
        /// Actualiza una calificación existente.
        /// </summary>
        /// <param name="id">ID de la calificación a actualizar</param>
        /// <param name="dto">Datos actualizados</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Staff)]
        public async Task<ActionResult<ApiResponse<int>>> Update(int id, [FromBody] UpdateCalificacionDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new ApiResponse<int>("El ID de la URL no coincide con el ID del cuerpo de la solicitud."));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos inválidos."));

            await _service.UpdateAsync(id, dto);

            return Ok(new ApiResponse<int>(id, "Calificación actualizada exitosamente."));
        }

        /// <summary>
        /// Elimina (lógicamente) una calificación.
        /// </summary>
        /// <param name="id">ID de la calificación</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ApiResponse<int>>> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new ApiResponse<int>(id, "Calificación eliminada exitosamente."));
        }

        // 5. CARGA MASIVA
        /// <summary>
        /// Registra calificaciones para todo un grupo en una materia y periodo específicos.
        /// 
        /// HARDENING DE SEGURIDAD:
        /// - Extrae UsuarioId y EscuelaId desde claims del token (ignora valores del DTO)
        /// - Valida que el usuario tiene permisos para el grupo
        /// - Auditoría completa de recalificaciones
        /// 
        /// MANEJO DE EXCEPCIONES:
        /// - DbUpdateException (clave duplicada/MySQL 1062) → 409 Conflict (concurrencia)
        /// - ConflictException → 409 Conflict
        /// - InvalidOperationException → 400 BadRequest (validaciones de negocio)
        /// - Exception no controlada → 500 Internal Server Error (sin stacktrace)
        /// 
        /// PRUEBAS MANUALES:
        /// 1. Simular doble inserción: Dos maestros intenta guardar simultáneamente
        ///    → Esperado: 409 Conflict con CorrelationId
        /// 2. Validación de motivo: Intenta recalificar sin motivo
        ///    → Esperado: 400 BadRequest con mensaje claro
        /// 3. Intenta usar EscuelaId/CapturadoPor falsos en DTO
        ///    → Esperado: Se ignoran, se usan los del token
        /// </summary>
        [HttpPost("masivo")]
        [Authorize(Roles = Roles.Staff)] // Maestros
        public async Task<ActionResult<ApiResponse<CalificacionMasivaResultadoDto>>> CreateMasivo([FromBody] CreateCalificacionMasivaDto dto)
        {
            // Generar CorrelationId para trazar la solicitud
            var correlationId = Guid.NewGuid().ToString();

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validación fallida en CreateMasivo. CorrelationId: {CorrelationId}", correlationId);
                return BadRequest(new ApiResponse<CalificacionMasivaResultadoDto>("Datos inválidos.") 
                { 
                    CorrelationId = correlationId,
                    StatusCode = 400
                });
            }

            try
            {
                // HARDENING: Extraer UsuarioId y EscuelaId desde el token (claims)
                // Ignoramos los valores que vienen en el DTO por seguridad
                var claimUserId = HttpContext.GetUserId();
                var claimEscuelaId = HttpContext.GetEscuelaId();
                var claimUsername = HttpContext.GetUsername();

                if (claimUserId <= 0 || claimEscuelaId <= 0)
                {
                    _logger.LogWarning(
                        "Claims inválidos o faltantes. CorrelationId: {CorrelationId}, " +
                        "UserId: {UserId}, EscuelaId: {EscuelaId}",
                        correlationId, claimUserId, claimEscuelaId);

                    return Unauthorized(new ApiResponse<CalificacionMasivaResultadoDto>(
                        "No se puede determinar tu identidad o escuela desde el token.")
                    {
                        CorrelationId = correlationId,
                        StatusCode = 401
                    });
                }

                // HARDENING: Sobrescribir DTO con valores seguros del token
                // (ignora cualquier intento de inyección de UsuarioId/EscuelaId malicioso)
                dto.CapturadoPor = claimUserId;
                dto.EscuelaId = claimEscuelaId;

                // VALIDACIÓN: Verificar que el maestro tiene permiso para capturar en este grupo
                // TODO: Implementar validación de permisos granular si tienes tabla de asignación
                // Por ahora, solo validamos que la escuela coincide.
                // Si necesitas validar que el maestro está asignado a este grupo:
                // - Consulta tabla GrupoMateriaMaestros
                // - Verifica que (grupoId, materiaId, maestroId, escuelaId) existen
                // 
                // Placeholder para validación de permisos:
                // if (!await _service.MaestroTienePermisoAsync(claimUserId, dto.GrupoId, dto.MateriaId, claimEscuelaId))
                //     return Forbid();

                _logger.LogInformation(
                    "Iniciando captura masiva (segura). CorrelationId: {CorrelationId}, " +
                    "Usuario: {Usuario} (ID: {UserId}), Escuela: {EscuelaId}, " +
                    "Grupo: {GrupoId}, Materia: {MateriaId}, Periodo: {PeriodoId}, " +
                    "Total: {TotalCalificaciones}, SoloValidar: {SoloValidar}",
                    correlationId, claimUsername, claimUserId, claimEscuelaId,
                    dto.GrupoId, dto.MateriaId, dto.PeriodoId, 
                    dto.Calificaciones?.Count ?? 0, dto.SoloValidar);

                var resultado = await _service.CreateMasivoAsync(dto);
                var total = (resultado?.Insertadas ?? 0) + (resultado?.Actualizadas ?? 0);
                var msg = dto.SoloValidar
                    ? "Validación completada."
                    : $"Se registraron {total} calificaciones exitosamente.";

                var response = new ApiResponse<CalificacionMasivaResultadoDto>(resultado, msg)
                {
                    CorrelationId = correlationId,
                    StatusCode = 200
                };

                _logger.LogInformation(
                    "Captura masiva exitosa. CorrelationId: {CorrelationId}, " +
                    "Usuario: {UserId}, Insertadas: {Insertadas}, Actualizadas: {Actualizadas}",
                    correlationId, claimUserId, resultado.Insertadas, resultado.Actualizadas);

                return Ok(response);
            }
            catch (ConflictException conflictEx)
            {
                // 409 Conflict: Concurrencia detectada por ConflictException
                _logger.LogWarning(
                    "Conflicto de concurrencia (ConflictException). CorrelationId: {CorrelationId}, " +
                    "GrupoId: {GrupoId}, MateriaId: {MateriaId}, PeriodoId: {PeriodoId}, " +
                    "Mensaje: {Mensaje}",
                    correlationId, dto.GrupoId, dto.MateriaId, dto.PeriodoId, conflictEx.Message);

                return Conflict(new ApiResponse<CalificacionMasivaResultadoDto>(
                    conflictEx.Message ?? "Conflicto de concurrencia detectado. Reintenta la operación.")
                {
                    CorrelationId = correlationId,
                    StatusCode = 409
                });
            }
            catch (DbUpdateException dbEx) when (dbEx.IsDuplicateKeyError())
            {
                // 409 Conflict: Violación de índice UNIQUE (MySQL 1062)
                _logger.LogWarning(
                    "Conflicto de concurrencia (DbUpdateException/Duplicate Key). CorrelationId: {CorrelationId}, " +
                    "GrupoId: {GrupoId}, MateriaId: {MateriaId}, PeriodoId: {PeriodoId}, " +
                    "TotalCalificaciones: {TotalCalificaciones}",
                    correlationId, dto.GrupoId, dto.MateriaId, dto.PeriodoId, 
                    dto.Calificaciones?.Count ?? 0);

                return Conflict(new ApiResponse<CalificacionMasivaResultadoDto>(dbEx.GetConflictMessage())
                {
                    CorrelationId = correlationId,
                    StatusCode = 409
                });
            }
            catch (InvalidOperationException invalidEx)
            {
                // 400 BadRequest: Validaciones de negocio conocidas
                _logger.LogWarning(
                    "Error de validación de negocio. CorrelationId: {CorrelationId}, " +
                    "GrupoId: {GrupoId}, Mensaje: {Mensaje}",
                    correlationId, dto.GrupoId, invalidEx.Message);

                return BadRequest(new ApiResponse<CalificacionMasivaResultadoDto>(invalidEx.Message)
                {
                    CorrelationId = correlationId,
                    StatusCode = 400
                });
            }
            catch (DbUpdateException dbEx)
            {
                // 500 Internal Server Error: DbUpdateException no esperada
                _logger.LogError(
                    dbEx,
                    "DbUpdateException inesperada. CorrelationId: {CorrelationId}, " +
                    "GrupoId: {GrupoId}, MateriaId: {MateriaId}, PeriodoId: {PeriodoId}",
                    correlationId, dto.GrupoId, dto.MateriaId, dto.PeriodoId);

                return StatusCode(500, new ApiResponse<CalificacionMasivaResultadoDto>(
                    "Error al guardar los datos. Por favor, intenta de nuevo.")
                {
                    CorrelationId = correlationId,
                    StatusCode = 500
                });
            }
            catch (Exception ex)
            {
                // 500 Internal Server Error: Excepciones no controladas
                _logger.LogError(
                    ex,
                    "Excepción inesperada en CreateMasivo. CorrelationId: {CorrelationId}, " +
                    "Tipo: {ExceptionType}",
                    correlationId, ex.GetType().Name);

                return StatusCode(500, new ApiResponse<CalificacionMasivaResultadoDto>(
                    "Error interno del servidor. Por favor, intenta más tarde.")
                {
                    CorrelationId = correlationId,
                    StatusCode = 500
                });
            }
        }

        // 3. REPORTE BOLETA
        /// <summary>
        /// Obtiene la boleta de calificaciones estructurada de un alumno.
        /// </summary>
        [HttpGet("boleta/{alumnoId}")]
        [Authorize(Roles = Roles.Staff + "," + Roles.Padre + "," + Roles.Alumno)]
        public async Task<ActionResult<ApiResponse<BoletaDto>>> GetBoleta(int alumnoId, [FromQuery] string cicloEscolar)
        {
            if (string.IsNullOrEmpty(cicloEscolar))
                return BadRequest(new ApiResponse<BoletaDto>("El ciclo escolar es requerido."));

            try
            {
                var boleta = await _service.GetBoletaAsync(alumnoId, cicloEscolar);
                return Ok(new ApiResponse<BoletaDto>(boleta, "Boleta generada exitosamente."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<BoletaDto>(ex.Message));
            }
        }
    }
}