using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Calificacion;
using SchoolSystem.Application.DTOs.Calificaciones;
using SchoolSystem.Application.Exceptions;
using SchoolSystem.Application.Extensions;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Auditoria;
using SchoolSystem.Domain.Entities.Evaluacion;
using SchoolSystem.Domain.Enums.Auditoria;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    public class CalificacionService : ICalificacionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CalificacionService> _logger;

        public CalificacionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CalificacionService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CalificacionDto> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.Calificaciones.FindAsync(
                c => c.Id == id,
                c => c.Alumno,
                c => c.Materia,
                c => c.Grupo,
                c => c.Periodo,
                c => c.MaestroCaptura.Usuario
            );

            var entity = result.FirstOrDefault();

            if (entity == null)
                return null;

            return _mapper.Map<CalificacionDto>(entity);
        }

        public async Task<PagedResult<CalificacionDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var allItems = await _unitOfWork.Calificaciones.GetAllIncludingAsync(
                c => c.Alumno,
                c => c.Materia,
                c => c.Grupo,
                c => c.Periodo,
                c => c.MaestroCaptura.Usuario
            );

            var total = allItems.Count();

            var items = allItems.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<CalificacionDto>
            {
                Items = _mapper.Map<IEnumerable<CalificacionDto>>(items),
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> CreateAsync(CreateCalificacionDto dto)
        {
            if (dto.CalificacionNumerica < 0 || dto.CalificacionNumerica > 10)
                throw new ArgumentException("La calificación debe estar entre 0 y 10.");

            // Verificamos si ya existe una calificación para ese Alumno + Materia + Periodo
            var calificacionesExistentes = (await _unitOfWork.Calificaciones
                .FindAsync(c =>
                           c.AlumnoId == dto.AlumnoId &&
                           c.MateriaId == dto.MateriaId &&
                           c.PeriodoId == dto.PeriodoId &&
                           c.TipoEvaluacion == dto.TipoEvaluacion &&
                           !c.IsDeleted))
                .Any();

            if (calificacionesExistentes)
                // Lanzamos una excepción que el Middleware convertirá en un 400 Bad Request
                throw new InvalidOperationException($"El alumno ya tiene una calificación registrada para la materia {dto.MateriaId} en el periodo {dto.PeriodoId}. Utilice la opción de Actualizar.");

            // Mapeo inicial
            var entity = _mapper.Map<Calificacion>(dto);

            // USO DE MÉTODO DE DOMINIO
            // Definimos la calificación mínima aprobatoria (podría venir de ConfiguraciónEscuela)
            decimal calificacionMinima = 6.0m;

            // El método de la entidad se encarga de:
            // - Redondear decimales
            // - Calcular bool Aprobado
            // - Asignar la Letra (A, B, C...)
            // - Asignar quién capturó
            entity.EstablecerCalificacion(dto.CalificacionNumerica, calificacionMinima, dto.CapturadoPor);

            // Agregar info adicional si viene
            if (!string.IsNullOrEmpty(dto.Observaciones))
            {
                entity.Observaciones = dto.Observaciones;
            }

            if (!string.IsNullOrEmpty(dto.Fortalezas) || !string.IsNullOrEmpty(dto.AreasOportunidad))
            {
                entity.AgregarRetroalimentacion(dto.Fortalezas, dto.AreasOportunidad, dto.Recomendaciones, dto.CapturadoPor);
            }

            // Persistencia
            await _unitOfWork.Calificaciones.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Calificaciones.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Calificación con ID {id} no encontrada");

            // Validar si se puede eliminar
            if (entity.Bloqueada)
                throw new InvalidOperationException("No se puede eliminar una calificación que ha sido bloqueada/cerrada.");

            await _unitOfWork.Calificaciones.DeleteAsync(entity);

            await _unitOfWork.SaveChangesAsync();
        }

        // ============================================================
        // 5. CAPTURA MASIVA (BULK INSERT)
        // ============================================================
        public async Task<CalificacionMasivaResultadoDto> CreateMasivoAsync(CreateCalificacionMasivaDto dto)
        {
            var stopwatch = Stopwatch.StartNew();

            // ---------------------------------------------------------
            // PASO 0: Validaciones de Consistencia
            // ---------------------------------------------------------
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            if (dto.Calificaciones == null || dto.Calificaciones.Count == 0)
                throw new Exception("Debe enviar al menos una calificación.");

            // ✅ LOGGING: Inicio de operación con contexto completo
            _logger.LogInformation(
                "📋 CalificacionesMasivo_Start: Iniciando captura masiva. " +
                "EscuelaId: {EscuelaId}, GrupoId: {GrupoId}, MateriaId: {MateriaId}, PeriodoId: {PeriodoId}, " +
                "TotalEnviadas: {TotalEnviadas}, SoloValidar: {SoloValidar}, PermitirRecalificar: {PermitirRecalificar}, " +
                "CapturadoPor: {CapturadoPor}",
                dto.EscuelaId, dto.GrupoId, dto.MateriaId, dto.PeriodoId,
                dto.Calificaciones.Count, dto.SoloValidar, dto.PermitirRecalificarExistentes,
                dto.CapturadoPor);

            // Normalizar: si viene el mismo alumno repetido, nos quedamos con la última captura
            var califsNormalizadas = dto.Calificaciones
                .GroupBy(x => x.AlumnoId)
                .Select(g => g.Last())
                .ToList();

            var alumnosIdsEnDto = califsNormalizadas
                .Select(c => c.AlumnoId)
                .Distinct()
                .ToList();

            var grupo = await _unitOfWork.Grupos.FirstOrDefaultAsync(g =>
                g.Id == dto.GrupoId && g.EscuelaId == dto.EscuelaId)
                ?? throw new Exception("Grupo no encontrado o no pertenece a la escuela.");

            var periodo = await _unitOfWork.PeriodoEvaluaciones.FirstOrDefaultAsync(p =>
                p.Id == dto.PeriodoId && p.EscuelaId == dto.EscuelaId)
                ?? throw new Exception("Periodo de evaluación no encontrado o no pertenece a la escuela.");

            if (periodo.CicloEscolarId != grupo.CicloEscolarId)
                throw new Exception("El periodo seleccionado no pertenece al ciclo escolar del grupo.");

            // ---------------------------------------------------------
            // Validación: Verificar que la materia está asignada al grupo
            // ---------------------------------------------------------
            var materiaAsignadaAlGrupo = await _unitOfWork.GrupoMateriaMaestros.FirstOrDefaultAsync(gmm =>
                gmm.GrupoId == dto.GrupoId &&
                gmm.MateriaId == dto.MateriaId &&
                gmm.EscuelaId == dto.EscuelaId)
                ?? throw new Exception("La materia no está asignada a este grupo.");

            // Regla de periodo
            var permiteInsercion = periodo.PuedeCapturarCalificaciones();
            var permiteRecalificacion = periodo.PuedeModificarCalificaciones() || periodo.EstaEnPeriodoRecalificacion();

            // ✅ CAMBIO IMPORTANTE: NO lanzar excepciones globales en modo COMMIT
            // Las reglas de período se aplican POR ALUMNO en el loop de abajo
            // Esto permite batch mixto: algunos inserts (permitidos), algunos updates (rechazados si período no permite)

            if (dto.SoloValidar)
            {
                // En modo validar, sí podemos verificar restricciones globales para información
                if (!dto.PermitirRecalificarExistentes && !permiteInsercion)
                {
                    // Solo informar, no bloquear todo - el precheck dirá qué alumnos tienen problemas
                }

                if (dto.PermitirRecalificarExistentes && !permiteRecalificacion)
                {
                    // Solo informar - mismo comportamiento
                }
            }

            // ---------------------------------------------------------
            // PASO 0.1: Validación enterprise de alumnos vs inscripciones (seguridad + consistencia)
            // ---------------------------------------------------------
            var inscripcionesValidas = await _unitOfWork.Inscripciones.FindAsync(i =>
                i.GrupoId == dto.GrupoId &&
                i.CicloEscolarId == grupo.CicloEscolarId &&
                i.EscuelaId == dto.EscuelaId &&
                alumnosIdsEnDto.Contains(i.AlumnoId) &&
                i.Estatus == Domain.Enums.Academico.EstatusInscripcion.Inscrito &&
                !i.IsDeleted
            );

            var idsAlumnosValidos = inscripcionesValidas
                .Select(i => i.AlumnoId)
                .ToHashSet();

            // ---------------------------------------------------------
            // PASO 1: Preparación (existentes + resumen)
            // ---------------------------------------------------------
            var resultado = new CalificacionMasivaResultadoDto
            {
                TotalEnviadas = califsNormalizadas.Count,
                PermiteRecalificar = permiteRecalificacion,
                MotivoNoPermiteRecalificar = permiteRecalificacion ? null : "El periodo está cerrado o es definitivo."
            };

            // Traer calificaciones existentes (por grupo/materia/periodo)
            var existentes = await _unitOfWork.Calificaciones.FindAsync(c =>
                c.GrupoId == dto.GrupoId &&
                c.MateriaId == dto.MateriaId &&
                c.PeriodoId == dto.PeriodoId &&
                c.AlumnoId.HasValue &&
                alumnosIdsEnDto.Contains(c.AlumnoId.Value) &&
                !c.IsDeleted
            );

            var existentesPorAlumno = existentes.ToDictionary(x => x.AlumnoId!.Value);

            foreach (var kvp in existentesPorAlumno)
            {
                var cal = kvp.Value;

                resultado.Existentes.Add(new CalificacionMasivaExistenteDto
                {
                    AlumnoId = kvp.Key,
                    CalificacionActual = cal.CalificacionNumerica, // ajusta si tu propiedad se llama diferente
                    ObservacionesActuales = cal.Observaciones
                });
            }

            // Si solo valida, llenar preview y retornar sin persistir
            if (dto.SoloValidar)
            {
                // PASO 14: Llenar PreviewPorAlumno con el estado que tendría cada alumno
                foreach (var item in califsNormalizadas)
                {
                    var preview = new CalificacionMasivaPreviewAlumnoDto
                    {
                        AlumnoId = item.AlumnoId
                    };

                    // Validación 1: ¿Está inscrito?
                    if (!idsAlumnosValidos.Contains(item.AlumnoId))
                    {
                        preview.Estado = "Error";
                        preview.Motivo = "El alumno no está inscrito en este grupo/ciclo o no es válido.";
                        resultado.PreviewPorAlumno.Add(preview);
                        resultado.TotalErrores++;
                        continue;
                    }

                    // Validación 2: ¿Existe calificación?
                    if (existentesPorAlumno.TryGetValue(item.AlumnoId, out var existente))
                    {
                        preview.CalificacionActual = existente.CalificacionNumerica;
                        preview.ObservacionesActuales = existente.Observaciones;

                        if (!dto.PermitirRecalificarExistentes)
                        {
                            preview.Estado = "OmitirExistente";
                            preview.Motivo = "Ya existe calificación para este alumno.";
                            resultado.TotalOmitirias++;
                            resultado.PreviewPorAlumno.Add(preview);
                            continue;
                        }

                        // ¿Permite recalificar?
                        if (!permiteRecalificacion)
                        {
                            preview.Estado = "Error";
                            preview.Motivo = "El período no permite recalificación/modificación.";
                            resultado.TotalErrores++;
                            resultado.PreviewPorAlumno.Add(preview);
                            continue;
                        }

                        // ¿Está bloqueada?
                        if (existente.Bloqueada)
                        {
                            preview.Estado = "Error";
                            preview.Motivo = "La calificación está bloqueada y no puede ser modificada.";
                            resultado.TotalErrores++;
                            resultado.PreviewPorAlumno.Add(preview);
                            continue;
                        }

                        preview.Estado = "Actualizar";
                        resultado.TotalActualizarias++;
                        resultado.PreviewPorAlumno.Add(preview);
                        continue;
                    }

                    // No existe: ¿Permite insertar?
                    if (!permiteInsercion)
                    {
                        preview.Estado = "Error";
                        preview.Motivo = "Este período no permite captura (inactivo, definitivo o fuera de plazo).";
                        resultado.TotalErrores++;
                        resultado.PreviewPorAlumno.Add(preview);
                        continue;
                    }

                            preview.Estado = "Insertar";
                            resultado.TotalInsertarias++;
                            resultado.PreviewPorAlumno.Add(preview);
                        }

                            // AUDITORÍA: Setear flag si se requiere motivo de recalificación
                            // (cuando hay calificaciones a recalificar y el período lo permite)
                            resultado.RequiereMotivoRecalificacion = resultado.TotalActualizarias > 0 && permiteRecalificacion;

                            // ✅ LOGGING: Resumen del precheck
                            stopwatch.Stop();
                            _logger.LogInformation(
                                "CalificacionesMasivo_Precheck: Validación previa completada. " +
                                "EscuelaId: {EscuelaId}, GrupoId: {GrupoId}, MateriaId: {MateriaId}, PeriodoId: {PeriodoId}, " +
                                "AlumnosValidos: {AlumnosValidos}, Existentes: {Existentes}, PermiteInsercion: {PermiteInsercion}, " +
                                "PermiteRecalificacion: {PermiteRecalificacion}, Preview[Insertar: {TotalInsertarias}, " +
                                "Actualizar: {TotalActualizarias}, Omitir: {TotalOmitirias}, Errores: {TotalErrores}], " +
                                "DuracionMs: {DuracionMs}",
                                dto.EscuelaId, dto.GrupoId, dto.MateriaId, dto.PeriodoId,
                                idsAlumnosValidos.Count, existentesPorAlumno.Count, permiteInsercion, permiteRecalificacion,
                                resultado.TotalInsertarias, resultado.TotalActualizarias, resultado.TotalOmitirias,
                                resultado.TotalErrores, stopwatch.ElapsedMilliseconds);

                            return resultado;
                        }

            // ---------------------------------------------------------
            // PASO 2: Construir inserts y updates (con validaciones POR ALUMNO)
            // ---------------------------------------------------------
            var nuevasCalificaciones = new List<Calificacion>();
            var calificacionesActualizar = new List<Calificacion>();

            foreach (var item in califsNormalizadas)
            {
                // Validación enterprise: alumno debe estar inscrito en el grupo/ciclo/escuela
                if (!idsAlumnosValidos.Contains(item.AlumnoId))
                {
                    resultado.Errores.Add(new CalificacionMasivaErrorDto
                    {
                        AlumnoId = item.AlumnoId,
                        Motivo = "El alumno no está inscrito en este grupo/ciclo o no es válido."
                    });
                    continue;
                }

                // Existe calificación
                if (existentesPorAlumno.TryGetValue(item.AlumnoId, out var existente))
                {
                    // Si no se permite recalificar, lo omitimos (sin error, es decisión de negocio)
                    if (!dto.PermitirRecalificarExistentes)
                        continue;

                    // ✅ CAMBIO: Validación por período POR ALUMNO (no global)
                    if (!permiteRecalificacion)
                    {
                        resultado.Errores.Add(new CalificacionMasivaErrorDto
                        {
                            AlumnoId = item.AlumnoId,
                            Motivo = "El período no permite recalificación/modificación para este alumno."
                        });
                        continue;
                    }

                    // Validación: Si se va a recalificar, el motivo es obligatorio y debe tener mínimo 10 caracteres
                    if (string.IsNullOrWhiteSpace(dto.MotivoModificacion))
                        throw new InvalidOperationException("Debe indicar un motivo de recalificación.");

                    if (dto.MotivoModificacion.Length < 10)
                        throw new InvalidOperationException("El motivo de recalificación debe tener mínimo 10 caracteres.");

                    // Validación: No se puede recalificar una calificación bloqueada
                    if (existente.Bloqueada)
                    {
                        resultado.Errores.Add(new CalificacionMasivaErrorDto
                        {
                            AlumnoId = item.AlumnoId,
                            Motivo = "La calificación está bloqueada y no puede ser modificada."
                        });
                        continue;
                    }

                    // AUDITORÍA DE RECALIFICACIÓN
                    // Capturar la calificación original ANTES de cambiarla
                    if (!existente.EsRecalificacion)
                    {
                        existente.CalificacionOriginal = existente.CalificacionNumerica;
                    }

                    // Actualizar datos de la calificación
                    existente.Observaciones = item.Observaciones;
                    existente.CapturadoPor = dto.CapturadoPor;
                    existente.FechaCaptura = DateTime.Now;
                    existente.TipoEvaluacion = "Ordinaria";
                    existente.EstablecerCalificacion(item.CalificacionNumerica, 6.0m);

                    // Registrar auditoría de la recalificación
                    existente.EsRecalificacion = true;
                    existente.FechaRecalificacion = DateTime.Now;
                    existente.TipoRecalificacion = "Masiva";
                    existente.FueModificada = true;
                    existente.FechaUltimaModificacion = DateTime.Now;
                    existente.ModificadoPor = dto.CapturadoPor;
                    existente.MotivoModificacion = dto.MotivoModificacion;

                        calificacionesActualizar.Add(existente);
                        continue;
                    }

                    // No existe: crear nueva
                    // ✅ CAMBIO: Validación por período POR ALUMNO (inserts)
                    if (!permiteInsercion)
                    {
                        resultado.Errores.Add(new CalificacionMasivaErrorDto
                        {
                            AlumnoId = item.AlumnoId,
                            Motivo = "El período no permite captura de nuevas calificaciones para este alumno."
                        });
                        continue;
                    }

                    var calificacion = new Calificacion
                    {
                        EscuelaId = dto.EscuelaId,
                        GrupoId = dto.GrupoId,
                        MateriaId = dto.MateriaId,
                        PeriodoId = dto.PeriodoId,
                        CapturadoPor = dto.CapturadoPor,
                        AlumnoId = item.AlumnoId,
                        Observaciones = item.Observaciones,
                        FechaCaptura = DateTime.Now,
                        TipoEvaluacion = "Ordinaria"
                    };

                    calificacion.EstablecerCalificacion(item.CalificacionNumerica, 6.0m);
                    nuevasCalificaciones.Add(calificacion);
            }

            // ---------------------------------------------------------
            // PASO 3: Persistencia (bulk) + PASO 4: Recalculo de promedios
            // Envuelta en transacción explícita para garantizar consistencia
            // ---------------------------------------------------------
            try
            {
                // BEGIN TRANSACTION
                await _unitOfWork.BeginTransactionAsync();

                // PASO 3: Persistencia de calificaciones (inserts y updates)
                if (nuevasCalificaciones.Any())
                    await _unitOfWork.Calificaciones.AddRangeAsync(nuevasCalificaciones);

                if (calificacionesActualizar.Any())
                    await _unitOfWork.Calificaciones.UpdateRangeAsync(calificacionesActualizar);

                // AUDITORÍA: Crear logs de recalificación
                var auditLogs = new List<CalificacionAuditLog>();

                foreach (var calificacionActualizada in calificacionesActualizar)
                {
                    // Buscar el item en DTO para obtener valores nuevos
                    var itemEnDto = califsNormalizadas.FirstOrDefault(x => x.AlumnoId == calificacionActualizada.AlumnoId!.Value);

                    if (itemEnDto != null)
                    {
                        var auditLog = new CalificacionAuditLog
                        {
                            EscuelaId = dto.EscuelaId,
                            CalificacionId = calificacionActualizada.Id,
                            AlumnoId = calificacionActualizada.AlumnoId!.Value,
                            GrupoId = dto.GrupoId,
                            MateriaId = dto.MateriaId,
                            PeriodoId = dto.PeriodoId,
                            CalificacionAnterior = calificacionActualizada.CalificacionOriginal ?? calificacionActualizada.CalificacionNumerica,
                            CalificacionNueva = itemEnDto.CalificacionNumerica,
                            ObservacionesAnteriores = calificacionActualizada.Observaciones,
                            ObservacionesNuevas = itemEnDto.Observaciones,
                            Motivo = dto.MotivoModificacion ?? "Recalificación masiva",
                            RecalificadoPor = dto.CapturadoPor,
                            RecalificadoAtUtc = DateTime.UtcNow,
                            Origen = "Web",
                            CorrelationId = null,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        auditLogs.Add(auditLog);
                    }
                }

                // Persistir audit logs en la misma transacción
                if (auditLogs.Any())
                    await _unitOfWork.CalificacionesAuditLog.AddRangeAsync(auditLogs);

                if (!nuevasCalificaciones.Any() && !calificacionesActualizar.Any())
                {
                    // Si no hay cambios, rollback y retornamos resultado
                    await _unitOfWork.RollbackTransactionAsync();

                    stopwatch.Stop();

                    // ✅ LOGGING: Sin cambios (batch vacío o todos rechazados)
                    _logger.LogWarning(
                        "⚠️ CalificacionesMasivo_NoChanges: Ninguna calificación pudo ser procesada (todos rechazados). " +
                        "EscuelaId: {EscuelaId}, GrupoId: {GrupoId}, MateriaId: {MateriaId}, PeriodoId: {PeriodoId}, " +
                        "TotalEnviadas: {TotalEnviadas}, Errores: {Errores}, DuracionMs: {DuracionMs}",
                        dto.EscuelaId, dto.GrupoId, dto.MateriaId, dto.PeriodoId,
                        califsNormalizadas.Count, resultado.Errores.Count, stopwatch.ElapsedMilliseconds);

                    return resultado;
                }

                // Primer SaveChanges dentro de la transacción (calificaciones + auditLogs)
                await _unitOfWork.SaveChangesAsync();

                resultado.Insertadas = nuevasCalificaciones.Count;
                resultado.Actualizadas = calificacionesActualizar.Count;
                resultado.AlumnoIdsInsertados = nuevasCalificaciones.Select(x => x.AlumnoId!.Value).ToList();
                resultado.AlumnoIdsActualizados = calificacionesActualizar.Select(x => x.AlumnoId!.Value).ToList();

                // PASO 4: Recalcular promedios (por ciclo)
                var inscripcionesAfectadas = await _unitOfWork.Inscripciones.FindAsync(i =>
                    i.GrupoId == dto.GrupoId &&
                    i.CicloEscolarId == grupo.CicloEscolarId &&
                    idsAlumnosValidos.Contains(i.AlumnoId) &&
                    i.Estatus == Domain.Enums.Academico.EstatusInscripcion.Inscrito &&
                    !i.IsDeleted
                );

                var todasLasCalificacionesDelGrupo = await _unitOfWork.Calificaciones.FindAsync(c =>
                    c.GrupoId == dto.GrupoId &&
                    c.AlumnoId.HasValue &&
                    idsAlumnosValidos.Contains(c.AlumnoId.Value) &&
                    !c.IsDeleted
                );

                var inscripcionesParaActualizar = new List<Inscripcion>();

                foreach (var inscripcion in inscripcionesAfectadas)
                {
                    var notasAlumno = todasLasCalificacionesDelGrupo
                        .Where(c => c.AlumnoId == inscripcion.AlumnoId)
                        .ToList();

                    if (notasAlumno.Any())
                    {
                        decimal promedio = CalcularPromedioInteligente(notasAlumno);
                        inscripcion.ActualizarPromedioAcumulado(promedio);
                        inscripcionesParaActualizar.Add(inscripcion);
                    }
                }

                if (inscripcionesParaActualizar.Any())
                {
                    await _unitOfWork.Inscripciones.UpdateRangeAsync(inscripcionesParaActualizar);
                    // Segundo SaveChanges dentro de la transacción (promedios)
                    await _unitOfWork.SaveChangesAsync();
                }

                    // COMMIT TRANSACTION
                    await _unitOfWork.CommitTransactionAsync();

                    stopwatch.Stop();

                    // ✅ LOGGING: Fin exitoso (con distintos niveles según errores)
                    if (resultado.TotalErrores > 0)
                    {
                        // LogWarning si hay errores (batch parcial)
                        _logger.LogWarning(
                            "⚠️ CalificacionesMasivo_End_Partial: Captura masiva completada parcialmente (con errores). " +
                            "EscuelaId: {EscuelaId}, GrupoId: {GrupoId}, MateriaId: {MateriaId}, PeriodoId: {PeriodoId}, " +
                            "TotalEnviadas: {TotalEnviadas}, Insertadas: {Insertadas}, Actualizadas: {Actualizadas}, " +
                            "TotalProcesadas: {TotalProcesadas}, Errores: {Errores}, DuracionMs: {DuracionMs}",
                            dto.EscuelaId, dto.GrupoId, dto.MateriaId, dto.PeriodoId,
                            califsNormalizadas.Count, resultado.Insertadas, resultado.Actualizadas,
                            resultado.Insertadas + resultado.Actualizadas, resultado.Errores.Count,
                            stopwatch.ElapsedMilliseconds);
                    }
                    else
                    {
                        // LogInformation si fue todo exitoso
                        _logger.LogInformation(
                            "✅ CalificacionesMasivo_End: Captura masiva completada exitosamente. " +
                            "EscuelaId: {EscuelaId}, GrupoId: {GrupoId}, MateriaId: {MateriaId}, PeriodoId: {PeriodoId}, " +
                            "Insertadas: {Insertadas}, Actualizadas: {Actualizadas}, TotalProcesadas: {TotalProcesadas}, " +
                            "DuracionMs: {DuracionMs}",
                            dto.EscuelaId, dto.GrupoId, dto.MateriaId, dto.PeriodoId,
                            resultado.Insertadas, resultado.Actualizadas,
                            resultado.Insertadas + resultado.Actualizadas,
                            stopwatch.ElapsedMilliseconds);
                    }

                    return resultado;
                            }
                            catch (DbUpdateException dbEx) when (IsDuplicateKey(dbEx))
                            {
                                stopwatch.Stop();

                                // ROLLBACK on duplicate key violation (MySQL error 1062)
                                await _unitOfWork.RollbackTransactionAsync();

                                // LOGGING: Warning con CorrelationId y contexto
                                _logger.LogWarning(
                                    "❌ CalificacionesMasivo_ConcurrencyDuplicate: Conflicto de concurrencia detectado (UNIQUE violation). " +
                                    "EscuelaId: {EscuelaId}, GrupoId: {GrupoId}, MateriaId: {MateriaId}, PeriodoId: {PeriodoId}, " +
                                    "TotalEnviadas: {TotalEnviadas}, Insertadas: {Insertadas}, Actualizadas: {Actualizadas}, " +
                                    "Errores: {Errores}, DuracionMs: {DuracionMs}, InnerException: {InnerException}",
                                    dto.EscuelaId, dto.GrupoId, dto.MateriaId, dto.PeriodoId,
                                    califsNormalizadas.Count, resultado.Insertadas, resultado.Actualizadas,
                                    resultado.Errores.Count, stopwatch.ElapsedMilliseconds,
                                    dbEx.InnerException?.Message ?? "No details");

                                // Lanzar ConcurrencyConflictException con contexto
                                throw new ConcurrencyConflictException(
                                    "Se detectó una captura simultánea. Recarga y vuelve a intentar.",
                                    contextData: new Dictionary<string, object>
                                    {
                                        { "EscuelaId", dto.EscuelaId },
                                        { "GrupoId", dto.GrupoId },
                                        { "MateriaId", dto.MateriaId },
                                        { "PeriodoId", dto.PeriodoId },
                                        { "CapturadoPor", dto.CapturadoPor },
                                        { "InnerException", dbEx.InnerException?.Message ?? "N/A" }
                                    }
                                );
                            }
                            catch (Exception ex)
                            {
                                stopwatch.Stop();

                                // ROLLBACK on any other exception
                                await _unitOfWork.RollbackTransactionAsync();

                                // ✅ LOGGING: Error inesperado con stacktrace y contexto
                                _logger.LogError(
                                    ex,
                                    "🔥 CalificacionesMasivo_Error: Excepción inesperada durante captura masiva. " +
                                    "EscuelaId: {EscuelaId}, GrupoId: {GrupoId}, MateriaId: {MateriaId}, PeriodoId: {PeriodoId}, " +
                                    "TotalEnviadas: {TotalEnviadas}, Insertadas: {Insertadas}, Actualizadas: {Actualizadas}, " +
                                    "Errores: {Errores}, DuracionMs: {DuracionMs}, " +
                                    "ExceptionType: {ExceptionType}, ExceptionMessage: {ExceptionMessage}",
                                    dto.EscuelaId, dto.GrupoId, dto.MateriaId, dto.PeriodoId,
                                    califsNormalizadas.Count, resultado.Insertadas, resultado.Actualizadas,
                                    resultado.Errores.Count, stopwatch.ElapsedMilliseconds,
                                    ex.GetType().Name, ex.Message);

                                throw;
                                }
                    }


        // ============================================================
        // 4. AUDITORÍA EN UPDATE (Lógica mejorada)
        // ============================================================
        public async Task UpdateAsync(int id, UpdateCalificacionDto dto)
        {
            var entity = await _unitOfWork.Calificaciones.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Calificación con ID {id} no encontrada");

            decimal? valorAnterior = entity.CalificacionNumerica;
            bool cambioNota = false;

            // USO DE MÉTODO DE DOMINIO: 'Modificar'
            // Este método es genial porque internamente:
            // 1. Revisa si 'Bloqueada' es true y lanza excepción si es así (Protección).
            // 2. Recalcula Aprobado y Letra con el nuevo valor.
            // 3. Registra la auditoría de modificación (Motivo, Fecha, Usuario).
            try
            {

                if (dto.CalificacionNumerica.HasValue && dto.CalificacionNumerica.Value != entity.CalificacionNumerica)
                {
                    entity.Modificar(dto.CalificacionNumerica.Value, dto.MotivoModificacion, dto.ModificadoPor);
                    cambioNota = true;
                }

                // Mapear resto de campos (Observaciones, etc)
                _mapper.Map(dto, entity);

                await _unitOfWork.Calificaciones.UpdateAsync(entity);

                // REGISTRAR AUDITORÍA SI HUBO CAMBIO DE NOTA
                if (cambioNota)
                {
                    LogAuditoria log = new()
                    {
                        EscuelaId = entity.EscuelaId,
                        UsuarioId = dto.ModificadoPor,
                        TipoAccion = TipoAccion.Actualizar,
                        EntidadAfectada = "Calificacion",
                        EntidadAfectadaId = entity.Id,
                        Descripcion = $"Cambio de calificación: {valorAnterior} -> {entity.CalificacionNumerica}",
                        FechaHora = DateTime.Now,
                        Exitoso = true
                    };

                    // Si tienes implementado repositorio de Logs en UnitOfWork:
                    await _unitOfWork.LogAuditorias.AddAsync(log);
                    // Si no, al menos queda en el historial de la entidad (entity.MotivoModificacion)
                }

                await _unitOfWork.SaveChangesAsync();

                // 2. RECALCULAR PROMEDIO (Disparador)
                if (cambioNota)
                {
                    await RecalcularPromedioInscripcion(entity.AlumnoId.Value, entity.GrupoId);
                }
            }
            catch (Exception ex) {
                // Capturamos la excepción de negocio (ej: "La calificación está bloqueada")
                // y la relanzamos para que el controlador la maneje (generalmente 400 Bad Request)
                throw new InvalidOperationException(ex.Message);
            }
        }

        // ============================================================
        // 3. REPORTE BOLETA
        // ============================================================

        public async Task<BoletaDto> GetBoletaAsync(int alumnoId, string cicloEscolar)
        {
            // 1. Obtener Alumno e Inscripción
            var inscripcion = (await _unitOfWork.Inscripciones.FindAsync(i =>
                i.AlumnoId == alumnoId && i.Ciclo != null && i.Ciclo.Clave == cicloEscolar,
                i => i.Alumno, i => i.Grupo, i => i.Ciclo
            )).FirstOrDefault();

            if (inscripcion == null)
                throw new KeyNotFoundException("El alumno no está inscrito en este ciclo.");

            // 2. Obtener todas las calificaciones del alumno en ese ciclo
            // Necesitamos incluir Materia y Periodo
            var calificaciones = await _unitOfWork.Calificaciones.FindAsync(c =>
                c.AlumnoId == alumnoId && c.Grupo.Ciclo.Clave == cicloEscolar,
                c => c.Materia, c => c.Periodo
            );

            // 3. Obtener todos los periodos del ciclo para armar columnas (ordenados)
            // Esto asume que tienes un repositorio de periodos o los sacas de las calificaciones
            var periodos = calificaciones.Select(c => c.Periodo.Nombre).Distinct().OrderBy(x => x).ToList();

            // 4. Armar DTO
            var boleta = new BoletaDto
            {
                AlumnoId = alumnoId,
                NombreAlumno = inscripcion.Alumno.NombreCompleto,
                Matricula = inscripcion.Alumno.Matricula,
                Grupo = inscripcion.Grupo.Nombre,
                CicloEscolarId = inscripcion.Grupo.CicloEscolarId,
                CicloEscolarClave = inscripcion.Grupo.Ciclo?.Clave,
                Materias = new List<MateriaBoletaDto>()
            };

            // Agrupar por materia
            var gruposMaterias = calificaciones.GroupBy(c => c.Materia.Nombre);

            decimal sumaPromediosMaterias = 0;

            foreach (var gMateria in gruposMaterias)
            {
                var materiaDto = new MateriaBoletaDto
                {
                    NombreMateria = gMateria.Key,
                    CalificacionesPorPeriodo = new Dictionary<string, decimal?>()
                };

                decimal sumaCalif = 0;
                int countCalif = 0;

                foreach (var calif in gMateria)
                {
                    materiaDto.CalificacionesPorPeriodo.Add(calif.Periodo.Nombre, calif.CalificacionNumerica);
                    sumaCalif += calif.CalificacionNumerica;
                    countCalif++;
                }

                materiaDto.PromedioMateria = countCalif > 0 ? Math.Round(sumaCalif / countCalif, 2) : 0;
                sumaPromediosMaterias += materiaDto.PromedioMateria;

                boleta.Materias.Add(materiaDto);
            }

            // Promedio General
            if (boleta.Materias.Any())
            {
                boleta.PromedioGeneral = Math.Round(sumaPromediosMaterias / boleta.Materias.Count, 2);
            }

            return boleta;
        }

        // ============================================================
        // MÉTODO PRIVADO: RECALCULAR PROMEDIOS
        // ============================================================
        private async Task RecalcularPromedioInscripcion(int alumnoId, int grupoId)
        {
            // 1. Buscar la inscripción
            var inscripcion = (await _unitOfWork.Inscripciones.FindAsync(i =>
                i.AlumnoId == alumnoId && i.GrupoId == grupoId)).FirstOrDefault();

            if (inscripcion == null)
                return;

            // 2. Traer todas las calificaciones del alumno en ese grupo
            var calificaciones = (await _unitOfWork.Calificaciones.FindAsync(c =>
                c.AlumnoId == alumnoId && c.GrupoId == grupoId && !c.IsDeleted)).ToList();

            if (!calificaciones.Any())
                // Si borró todas las notas, el promedio es 0
                inscripcion.ActualizarPromedioAcumulado(0);
            else
            {
                // 3. USAMOS EL HELPER AQUÍ TAMBIÉN:
                decimal promedio = CalcularPromedioInteligente(calificaciones);
                inscripcion.ActualizarPromedioAcumulado(promedio);
            }

            // Si fuera fin de ciclo, podríamos llamar a EstablecerPromedioFinal
            // inscripcion.EstablecerPromedioFinal(promedio, 6.0m, reprobadas);
            await _unitOfWork.Inscripciones.UpdateAsync(inscripcion);
            await _unitOfWork.SaveChangesAsync();
        }

        // Método auxiliar para centralizar la lógica matemática
        private decimal CalcularPromedioInteligente(List<Calificacion> calificaciones)
        {
            if (calificaciones == null || !calificaciones.Any())
                return 0;

            // 1. Calcular la suma total de los pesos definidos
            // El operador '?? 0' convierte los nulos a cero.
            decimal sumaPesos = calificaciones.Sum(c => c.Peso ?? 0);

            decimal promedioFinal;

            // 2. Decidir qué fórmula usar
            if (sumaPesos > 0)
            {
                // --- ESTRATEGIA PONDERADA ---
                // Fórmula: Suma(Nota * Peso) / Suma(Pesos)
                // Esto funciona incluso si los pesos no suman 100 todavía (promedio parcial proporcional).

                decimal sumaPonderada = calificaciones.Sum(c => c.CalificacionNumerica * (c.Peso ?? 0));
                promedioFinal = sumaPonderada / sumaPesos;
            }
            else
            {
                // --- ESTRATEGIA SIMPLE ---
                // Fórmula: Suma(Notas) / Cantidad
                promedioFinal = calificaciones.Average(c => c.CalificacionNumerica);
            }

                        // Retornamos redondeado a 2 decimales (estándar escolar)
                        return Math.Round(promedioFinal, 2);
                    }

                    // ============================================================
                    // HELPER PRIVADO: DETECCIÓN DE VIOLACIÓN DE CLAVE ÚNICA
                    // ============================================================
                    /// <summary>
                    /// Detecta si una excepción DbUpdateException es causada por
                    /// una violación de clave única (MySQL error 1062).
                    /// 
                    /// Delegado a la extensión DbUpdateExceptionExtensions.IsDuplicateKeyError()
                    /// para reutilizar lógica y mantener consistencia.
                    /// 
                    /// Esto es usado en CreateMasivoAsync para manejar race conditions
                    /// cuando múltiples usuarios capturan calificaciones simultáneamente.
                    /// </summary>
                    private bool IsDuplicateKey(DbUpdateException ex)
                    {
                        // Usar la extensión que ya tiene lógica robusta
                        // Compatible con Pomelo MySQL y MySqlConnector
                        return ex.IsDuplicateKeyError();
                    }
                }
            }
