using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Asistencias;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Evaluacion;
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

        public async Task<AsistenciaDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Asistencias.GetByIdAsync(id);
            return _mapper.Map<AsistenciaDto>(entity);
        }

        public async Task<PagedResult<AsistenciaDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var allItems = await _unitOfWork.Asistencias.GetAllAsync();
            var total = allItems.Count();
            var items = allItems.Skip((pageNumber - 1) * pageSize).Take(pageSize);

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
            var entity = _mapper.Map<Asistencia>(dto);
            await _unitOfWork.Asistencias.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(int id, UpdateAsistenciaDto dto)
        {
            var entity = await _unitOfWork.Asistencias.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Asistencia con ID {id} no encontrada");

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


        public async Task<int> CreateMasivoAsync(CreateAsistenciaMasivaDto dto)
        {
            // 1. VALIDACIÓN DE NEGOCIO: Evitar duplicados
            // Verificamos si ya existen registros para este grupo en esta fecha exacta.
            // Usamos el método FindAsync del repositorio genérico.
            var asistenciasExistentes = await _unitOfWork.Asistencias.FindAsync(a =>
                a.GrupoId == dto.GrupoId &&
                a.Fecha.Date == dto.Fecha.Date
            );

            if (asistenciasExistentes.Any())
            {
                throw new InvalidOperationException($"Ya existe un registro de asistencia para el Grupo ID {dto.GrupoId} en la fecha {dto.Fecha:dd/MM/yyyy}. Debe usar la edición individual si desea corregir.");
            }

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

                // Validar regla de negocio de la entidad
                if (asistencia.Estatus == Domain.Enums.Asistencia.EstadoAsistencia.Presente && !asistencia.HoraEntrada.HasValue)
                {
                    // Si falta hora de entrada, ponemos una por defecto o lanzamos error.
                    // Por ahora ponemos una hora laboral default para no romper el proceso masivo
                    asistencia.HoraEntrada = new TimeSpan(8, 0, 0);
                }

                // Agregar a la lista temporal (o directo al repo si no tienes AddRange)
                nuevasAsistencias.Add(asistencia);
            }

            // 3. PERSISTENCIA MASIVA
            // Nota: Si tu repositorio genérico no tiene AddRangeAsync, deberás agregarlo o hacer un bucle.
            // Suponiendo que tienes AddRangeAsync:
            foreach (var asistencia in nuevasAsistencias)
            {
                await _unitOfWork.Asistencias.AddAsync(asistencia);
            }

            await _unitOfWork.SaveChangesAsync();

            return nuevasAsistencias.Count;
        }
    }
}
