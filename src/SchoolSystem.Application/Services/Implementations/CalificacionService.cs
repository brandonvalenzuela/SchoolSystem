using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Calificacion;
using SchoolSystem.Application.DTOs.Calificaciones;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Auditoria;
using SchoolSystem.Domain.Entities.Evaluacion;
using SchoolSystem.Domain.Enums.Auditoria;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    public class CalificacionService : ICalificacionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CalificacionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            // Verificamos si ya existe una calificación para ese Alumno + Materia + Periodo
            var calificacionesExistentes = await _unitOfWork.Calificaciones.FindAsync(c =>
                c.AlumnoId == dto.AlumnoId &&
                c.MateriaId == dto.MateriaId &&
                c.PeriodoId == dto.PeriodoId 
            );

            if (calificacionesExistentes.Any())
            {
                // Lanzamos una excepción que el Middleware convertirá en un 400 Bad Request
                throw new InvalidOperationException($"El alumno ya tiene una calificación registrada para la materia {dto.MateriaId} en el periodo {dto.PeriodoId}. Utilice la opción de Actualizar.");
            }

            var entity = _mapper.Map<Calificacion>(dto);
            await _unitOfWork.Calificaciones.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Calificaciones.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Calificación con ID {id} no encontrada");

            await _unitOfWork.Calificaciones.DeleteAsync(entity);

            await _unitOfWork.SaveChangesAsync();
        }

        // ============================================================
        // 5. CAPTURA MASIVA (BULK INSERT)
        // ============================================================
        public async Task<int> CreateMasivoAsync(CreateCalificacionMasivaDto dto)
        {
            int guardados = 0;

            // Validar que no existan calificaciones previas para evitar duplicados masivos
            // (Opcional: Podrías decidir actualizar si ya existen, aquí asumimos solo inserción nueva)
            var existentes = await _unitOfWork.Calificaciones.FindAsync(c =>
                c.GrupoId == dto.GrupoId &&
                c.MateriaId == dto.MateriaId &&
                c.PeriodoId == dto.PeriodoId);

            var idsAlumnosExistentes = existentes.Select(c => c.AlumnoId).ToList();

            foreach (var item in dto.Calificaciones)
            {
                if (idsAlumnosExistentes.Contains(item.AlumnoId))
                    continue; // Saltar si ya tiene nota

                var calificacion = new Calificacion
                {
                    EscuelaId = dto.EscuelaId,
                    GrupoId = dto.GrupoId,
                    MateriaId = dto.MateriaId,
                    PeriodoId = dto.PeriodoId,
                    CapturadoPor = dto.CapturadoPor,
                    AlumnoId = item.AlumnoId,
                    Observaciones = item.Observaciones,
                    FechaCaptura = DateTime.Now
                };

                // Usar método de dominio para establecer nota y estatus
                // Asumimos 6.0 como mínima aprobatoria hardcodeada o la traes de config
                calificacion.EstablecerCalificacion(item.CalificacionNumerica, 6.0m);

                await _unitOfWork.Calificaciones.AddAsync(calificacion);
                guardados++;
            }

            await _unitOfWork.SaveChangesAsync();

            // 2. RECALCULAR PROMEDIOS (Disparador)
            // Recorremos los alumnos afectados para actualizar su inscripción
            foreach (var item in dto.Calificaciones)
            {
                await RecalcularPromedioInscripcion(item.AlumnoId, dto.GrupoId);
            }

            return guardados;
        }

        // ============================================================
        // 4. AUDITORÍA EN UPDATE (Lógica mejorada)
        // ============================================================
        public async Task UpdateAsync(int id, UpdateCalificacionDto dto)
        {
            var entity = await _unitOfWork.Calificaciones.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Calificación con ID {id} no encontrada");

            decimal? valorAnterior = entity.CalificacionNumerica;
            bool cambioNota = false;

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

        // ============================================================
        // 3. REPORTE BOLETA
        // ============================================================

        public async Task<BoletaDto> GetBoletaAsync(int alumnoId, string cicloEscolar)
        {
            // 1. Obtener Alumno e Inscripción
            var inscripcion = (await _unitOfWork.Inscripciones.FindAsync(i =>
                i.AlumnoId == alumnoId && i.CicloEscolar == cicloEscolar,
                i => i.Alumno, i => i.Grupo
            )).FirstOrDefault();

            if (inscripcion == null)
                throw new KeyNotFoundException("El alumno no está inscrito en este ciclo.");

            // 2. Obtener todas las calificaciones del alumno en ese ciclo
            // Necesitamos incluir Materia y Periodo
            var calificaciones = await _unitOfWork.Calificaciones.FindAsync(c =>
                c.AlumnoId == alumnoId && c.Grupo.CicloEscolar == cicloEscolar,
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
                CicloEscolar = cicloEscolar,
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
            var calificaciones = await _unitOfWork.Calificaciones.FindAsync(c =>
                c.AlumnoId == alumnoId && c.GrupoId == grupoId);

            if (!calificaciones.Any())
                return;

            // 3. Calcular promedio simple
            decimal promedio = calificaciones.Average(c => c.CalificacionNumerica);

            // 4. Contar reprobadas (asumiendo < 6 es reprobatoria)
            int reprobadas = calificaciones.Count(c => c.CalificacionNumerica < 6.0m);

            // 5. Actualizar entidad Inscripción
            inscripcion.ActualizarPromedioAcumulado(promedio);

            // Si fuera fin de ciclo, podríamos llamar a EstablecerPromedioFinal
            // inscripcion.EstablecerPromedioFinal(promedio, 6.0m, reprobadas);

            await _unitOfWork.Inscripciones.UpdateAsync(inscripcion);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
