using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Calificacion;
using SchoolSystem.Application.DTOs.Calificaciones;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Academico;
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
        public async Task<int> CreateMasivoAsync(CreateCalificacionMasivaDto dto)
        {

            // ---------------------------------------------------------
            // PASO 1: Validación y Filtrado en Memoria (0 Queries)
            // ---------------------------------------------------------

            // Obtenemos los IDs de los alumnos que queremos calificar
            var alumnosIdsEnDto = dto.Calificaciones.Select(c => c.AlumnoId).Distinct().ToList();

            // Verificamos en UNA SOLA consulta si ya existen calificaciones para estos alumnos
            // en esta materia/periodo para evitar duplicados.
            var calificacionesExistentes = await _unitOfWork.Calificaciones.FindAsync(c =>
                c.MateriaId == dto.MateriaId &&
                c.PeriodoId == dto.PeriodoId &&
                alumnosIdsEnDto.Contains(c.AlumnoId.Value) && // Uso de IN en SQL
                !c.IsDeleted);

            var idsConCalificacion = calificacionesExistentes.Select(c => c.AlumnoId.Value).ToHashSet();

            // ---------------------------------------------------------
            // PASO 2: Preparación de Entidades (En Memoria)
            // ---------------------------------------------------------

            var nuevasCalificaciones = new List<Calificacion>();

            foreach (var item in dto.Calificaciones)
            {
                // Si ya tiene calificación, lo saltamos
                if (idsConCalificacion.Contains(item.AlumnoId))
                    continue;

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
                    TipoEvaluacion = "Ordinaria" // O pasarlo en el DTO
                };

                // Usar método de dominio
                calificacion.EstablecerCalificacion(item.CalificacionNumerica, 6.0m);

                nuevasCalificaciones.Add(calificacion);
            }

            if (nuevasCalificaciones.Count == 0)
                return 0;

            // ---------------------------------------------------------
            // PASO 3: Inserción Masiva (1 Query)
            // ---------------------------------------------------------

            // AddRange es mucho más rápido que Add en un bucle
            await _unitOfWork.Calificaciones.AddRangeAsync(nuevasCalificaciones);
            await _unitOfWork.SaveChangesAsync(); // Se generan los IDs aquí

            // ---------------------------------------------------------
            // PASO 4: Recálculo Masivo de Promedios (2 Queries)
            // ---------------------------------------------------------

            // A. Traer TODAS las inscripciones afectadas de un solo golpe
            var inscripcionesAfectadas = await _unitOfWork.Inscripciones.FindAsync(i =>
                i.GrupoId == dto.GrupoId &&
                alumnosIdsEnDto.Contains(i.AlumnoId) && // Solo los alumnos que tocamos
                i.Estatus == Domain.Enums.Academico.EstatusInscripcion.Inscrito
            );

            // B. Traer TODAS las calificaciones de estos alumnos en este grupo de un solo golpe
            // (Necesitamos todas para promediar, no solo las nuevas)
            var todasLasCalificacionesDelGrupo = await _unitOfWork.Calificaciones.FindAsync(c =>
                c.GrupoId == dto.GrupoId &&
                alumnosIdsEnDto.Contains(c.AlumnoId.Value) &&
                !c.IsDeleted
            );

            // C. Procesamiento en Memoria (Rapidísimo)
            var inscripcionesParaActualizar = new List<Inscripcion>();

            foreach (var inscripcion in inscripcionesAfectadas)
            {
                // Filtramos calificaciones de este alumno
                var notasAlumno = todasLasCalificacionesDelGrupo
                    .Where(c => c.AlumnoId == inscripcion.AlumnoId)
                    .ToList();

                if (notasAlumno.Any())
                {
                    // USAMOS EL HELPER AQUÍ:
                    decimal promedio = CalcularPromedioInteligente(notasAlumno);

                    // Actualizamos la entidad
                    inscripcion.ActualizarPromedioAcumulado(promedio);

                    inscripcionesParaActualizar.Add(inscripcion);
                }
            }

            // ---------------------------------------------------------
            // PASO 5: Actualización Masiva (1 Query)
            // ---------------------------------------------------------

            if (inscripcionesParaActualizar.Any())
            {
                // UpdateRange es más eficiente
                await _unitOfWork.Inscripciones.UpdateRangeAsync(inscripcionesParaActualizar);
                await _unitOfWork.SaveChangesAsync();
            }

            return nuevasCalificaciones.Count;
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
    }
}
