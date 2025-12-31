using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Asistencias;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Evaluacion;
using SchoolSystem.Domain.Enums.Academico;
using SchoolSystem.Domain.Enums.Asistencia;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    public class AsistenciaService : IAsistenciaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AsistenciaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ==========================================
        // CRUD BÁSICO
        // ==========================================
        public async Task<AsistenciaDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Asistencias.GetByIdAsync(id);
            if (entity == null)
                return null;
            return _mapper.Map<AsistenciaDto>(entity);
        }

        public async Task<PagedResult<AsistenciaDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var allItems = await _unitOfWork.Asistencias.GetAllIncludingAsync(a => a.Alumno, a => a.Grupo);
            var total = allItems.Count();
            var items = allItems
                .OrderByDescending(a => a.Fecha)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return new PagedResult<AsistenciaDto>
            {
                Items = _mapper.Map<IEnumerable<AsistenciaDto>>(items),
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> CreateAsync(CreateAsistenciaDto dto)
        {
            var existe = (await _unitOfWork.Asistencias.FindAsync(a =>
               a.AlumnoId == dto.AlumnoId &&
               a.Fecha.Date == dto.Fecha.Date &&
               !a.IsDeleted)).Any();

            if (existe)
                throw new InvalidOperationException($"El alumno ya tiene asistencia registrada para la fecha {dto.Fecha:dd/MM/yyyy}.");

            var entity = _mapper.Map<Asistencia>(dto);
            entity.FechaRegistro = DateTime.Now;

            await _unitOfWork.Asistencias.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(int id, UpdateAsistenciaDto dto)
        {
            var entity = await _unitOfWork.Asistencias.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Asistencia con ID {id} no encontrada");

            // Si cambia el estatus, usamos el método de dominio Modificar
            if (dto.Estatus != entity.Estatus)
                entity.Modificar(dto.Estatus, "Corrección manual", dto.UsuarioModificoId);

            _mapper.Map(dto, entity);
            await _unitOfWork.Asistencias.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Asistencias.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Asistencia con ID {id} no encontrada");

            await _unitOfWork.Asistencias.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        // ==========================================
        // A. REGISTRO MASIVO (PASE DE LISTA)
        // ==========================================
        public async Task<int> CreateMasivoAsync(CreateAsistenciaMasivaDto dto)
        {
            // 1.VALIDACIÓN DE NEGOCIO: Evitar duplicados
            // Verificamos si ya existen registros para este grupo en esta fecha exacta.
            var asistenciasExistentes = await _unitOfWork.Asistencias.FindAsync(a =>
                a.GrupoId == dto.GrupoId &&
                a.Fecha.Date == dto.Fecha.Date &&
                !a.IsDeleted
            );

            if (asistenciasExistentes.Any())
                throw new InvalidOperationException($"Ya existe un registro de asistencia para el Grupo ID {dto.GrupoId} en la fecha {dto.Fecha:dd/MM/yyyy}. Debe usar la edición individual si desea corregir.");

            // 2. PROCESAMIENTO
            var nuevasAsistencias = new List<Asistencia>();

            foreach (var item in dto.Asistencias)
            {
                // Mapeo manual o híbrido para combinar datos de cabecera con datos de detalle
                var asistencia = new Asistencia
                {
                    // Datos de cabecera (CreateAsistenciaMasivaDto)
                    EscuelaId = dto.EscuelaId,
                    GrupoId = dto.GrupoId,
                    Fecha = dto.Fecha,
                    RegistradoPor = dto.RegistradoPor,
                    FechaRegistro = DateTime.Now,

                    // Datos del alumno (AsistenciaAlumnoDto)
                    AlumnoId = item.AlumnoId,
                    Estatus = item.Estatus,
                    HoraEntrada = item.HoraEntrada,
                    MinutosRetardo = item.MinutosRetardo,
                    Observaciones = item.Observaciones,

                    // Valores por defecto
                    CreatedAt = DateTime.Now
                };

                // Validar regla de negocio de la entidad (si falta hora de entrada y está presente)
                if (asistencia.Estatus == EstadoAsistencia.Presente && !asistencia.HoraEntrada.HasValue)
                {
                    // Si falta hora de entrada, ponemos una por defecto o lanzamos error.
                    // Por ahora ponemos una hora laboral default para no romper el proceso masivo
                    asistencia.HoraEntrada = new TimeSpan(8, 0, 0);
                }

                // Agregar a la lista temporal (o directo al repo si no tienes AddRange)
                nuevasAsistencias.Add(asistencia);
            }

            // 3. PERSISTENCIA MASIVA
            await _unitOfWork.Asistencias.AddRangeAsync(nuevasAsistencias);
            await _unitOfWork.SaveChangesAsync();

            return nuevasAsistencias.Count;
        }

        public async Task<List<AsistenciaDto>> GetByGrupoAndFechaAsync(int grupoId, DateTime fecha)
        {
            // Usamos FindAsync del repositorio genérico
            var entidades = await _unitOfWork.Asistencias.FindAsync(
                a => a.GrupoId == grupoId && a.Fecha.Date == fecha.Date && !a.IsDeleted,
                a => a.Alumno
            );

            return _mapper.Map<List<AsistenciaDto>>(entidades.OrderBy(x => x.Alumno.ApellidoPaterno));
        }

        // ==========================================
        // B. HISTORIAL DE ALUMNO
        // ==========================================
        public async Task<List<AsistenciaDto>> GetHistorialByAlumnoAsync(int alumnoId)
        {
            // 1. Obtener asistencias filtradas por alumno
            // Usamos tu repositorio actualizado que acepta includes
            var entidades = await _unitOfWork.Asistencias.FindAsync(
                a => a.AlumnoId == alumnoId && !a.IsDeleted,
                a => a.Grupo,   // Incluir Grupo para mostrar el nombre
                a => a.Alumno   // Incluir Alumno para el nombre
            );

            // 2. Ordenar por fecha descendente (lo más reciente primero)
            return _mapper.Map<List<AsistenciaDto>>(entidades.OrderByDescending(x => x.Fecha).ToList());
        }

        // ==========================================
        // C. JUSTIFICAR FALTA
        // ==========================================
        public async Task JustificarFaltaAsync(int asistenciaId, JustificarFaltaDto dto, int usuarioId)
        {
            var asistencia = await _unitOfWork.Asistencias.GetByIdAsync(asistenciaId);

            if (asistencia == null)
                throw new KeyNotFoundException("Registro de asistencia no encontrado.");

            // Regla de negocio: Solo se pueden justificar Faltas o Retardos
            if (asistencia.Estatus == EstadoAsistencia.Presente)
                throw new InvalidOperationException("No se puede justificar una asistencia donde el alumno estuvo presente.");

            // Usamos el método de dominio que ya tienes en tu entidad
            asistencia.Justificar(dto.Motivo, dto.JustificanteUrl, usuarioAprobo: usuarioId);
            // Cambiamos el estatus visualmente a Justificada
            asistencia.Estatus = EstadoAsistencia.Justificada;

            await _unitOfWork.Asistencias.UpdateAsync(asistencia);
            await _unitOfWork.SaveChangesAsync();
        }

        // ==========================================
        // D. REPORTE MENSUAL (SÁBANA)
        // ==========================================
        public async Task<List<ReporteMensualDto>> GetReporteMensualAsync(int grupoId, int mes, int anio)
        {
            // 1. Definir rango de fechas
            var fechaInicio = new DateTime(anio, mes, 1);
            var diasEnMes = DateTime.DaysInMonth(anio, mes);
            var fechaFin = new DateTime(anio, mes, diasEnMes);

            // 2. Obtener TODOS los alumnos del grupo (incluidos los que no tengan asistencia)
            // Necesitamos el repositorio de Inscripciones para saber quiénes pertenecen al grupo
            // Nota: Asumo que tienes _inscripcionRepository o accedes via unitOfWork
            var inscripciones = await _unitOfWork.Inscripciones.FindAsync(
                i => i.GrupoId == grupoId && i.Estatus == EstatusInscripcion.Inscrito && !i.IsDeleted,
                i => i.Alumno
            );

            // 3. Obtener las asistencias del mes para ese grupo
            var asistencias = await _unitOfWork.Asistencias.FindAsync(
                a => a.GrupoId == grupoId && a.Fecha >= fechaInicio && a.Fecha <= fechaFin && !a.IsDeleted
            );

            var reporte = new List<ReporteMensualDto>();

            // 4. Construir la matriz
            foreach (var ins in inscripciones.OrderBy(x => x.Alumno.ApellidoPaterno))
            {
                var fila = new ReporteMensualDto
                {
                    AlumnoId = ins.AlumnoId,
                    NombreAlumno = ins.Alumno.NombreCompleto,
                    Matricula = ins.Alumno.Matricula,
                    Dias = new List<DiaAsistenciaDto>()
                };

                // Filtrar asistencias de este alumno
                var asistenciasAlumno = asistencias.Where(a => a.AlumnoId == ins.AlumnoId).ToList();
                int contadorAsistencias = 0;

                // Iterar todos los días del mes
                // Llenar los días del mes
                for (int dia = 1; dia <= diasEnMes; dia++)
                {
                    var fechaDia = new DateTime(anio, mes, dia);
                    // Buscar si hay registro ese día
                    var asistenciaDia = asistenciasAlumno.FirstOrDefault(a => a.Fecha.Date == fechaDia);

                    string letraEstatus = "-"; // Por defecto: Sin registro (fin de semana o no hubo clase)
                    string nombreEstatus = "Sin registro";

                    if (asistenciaDia != null)
                    {
                        // Convertir Enum a letra para la tabla compacta
                        switch (asistenciaDia.Estatus)
                        {
                            case EstadoAsistencia.Presente:
                                letraEstatus = "P";
                                contadorAsistencias++;
                                break;
                            case EstadoAsistencia.Falta:
                                letraEstatus = "F";
                                fila.Faltas++;
                                break;
                            case EstadoAsistencia.Retardo:
                                letraEstatus = "R";
                                fila.Retardos++;
                                // Dependiendo de la regla, un retardo puede contar como asistencia o media falta
                                contadorAsistencias++;
                                break;
                            case EstadoAsistencia.Justificada:
                                letraEstatus = "J";
                                // Justificada no cuenta como falta, ¿cuenta como asistencia? Depende de la escuela.
                                break;
                            case EstadoAsistencia.Permiso:
                                letraEstatus = "PER";
                                break;
                        }
                        nombreEstatus = asistenciaDia.Estatus.ToString();
                    }
                    else if (fechaDia.DayOfWeek == DayOfWeek.Saturday || fechaDia.DayOfWeek == DayOfWeek.Sunday)
                    {
                        letraEstatus = "S/D"; // Fin de semana
                    }

                    fila.Dias.Add(new DiaAsistenciaDto
                    {
                        Dia = dia,
                        Fecha = fechaDia,
                        Estatus = letraEstatus,
                        EstatusCompleto = nombreEstatus
                    });

                    fila.Asistencias = contadorAsistencias;

                    // Cálculo simple de porcentaje (sobre días registrados, no sobre días hábiles totales del mes para no castigar días futuros)
                    var diasRegistrados = asistenciasAlumno.Count;
                    fila.Porcentaje = diasRegistrados > 0 ? Math.Round((decimal)contadorAsistencias / diasRegistrados * 100, 2) : 100;
                }
                reporte.Add(fila);
            }
            return reporte;
        }
    }
}