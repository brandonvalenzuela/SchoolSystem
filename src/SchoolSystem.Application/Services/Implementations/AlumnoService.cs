using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Alumnos;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    public class AlumnoService : IAlumnoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AlumnoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene un alumno por su ID.
        /// </summary>
        public async Task<AlumnoDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Alumnos.GetByIdAsync(id);

            // Si es null, el controlador puede devolver NotFound, 
            // o podemos lanzar una KeyNotFoundException aquí.
            return _mapper.Map<AlumnoDto>(entity);
        }

        /// <summary>
        /// Obtiene una lista paginada de alumnos.
        /// </summary>
        public async Task<PagedResult<AlumnoDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            // Nota: En un entorno de producción con muchos datos, 
            // lo ideal es que el repositorio soporte IQueryable para paginar en base de datos.
            // Aquí usamos la implementación genérica básica.
            var allItems = await _unitOfWork.Alumnos.GetAllAsync();

            // Filtramos lógica básica (ej: solo mostrar activos por defecto si se requiere)
            // Como la entidad tiene SoftDelete, el repositorio o el QueryFilter global 
            // ya deberían estar filtrando los eliminados.

            var total = allItems.Count();

            var items = allItems
                .OrderBy(a => a.ApellidoPaterno) // Ordenar alfabéticamente por defecto
                .ThenBy(a => a.Nombre)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return new PagedResult<AlumnoDto>
            {
                Items = _mapper.Map<IEnumerable<AlumnoDto>>(items),
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        /// <summary>
        /// Crea un nuevo alumno.
        /// </summary>
        public async Task<int> CreateAsync(CreateAlumnoDto dto)
        {
            var entity = _mapper.Map<Alumno>(dto);

            // Lógica de negocio adicional al crear
            entity.FechaIngreso = DateTime.Now;
            entity.Estatus = SchoolSystem.Domain.Enums.Academico.EstatusAlumno.Activo;

            await _unitOfWork.Alumnos.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.Id;
        }

        /// <summary>
        /// Actualiza un alumno existente.
        /// </summary>
        public async Task UpdateAsync(int id, UpdateAlumnoDto dto)
        {
            var entity = await _unitOfWork.Alumnos.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Alumno con ID {id} no encontrado.");

            // AutoMapper actualiza las propiedades de 'entity' con los valores de 'dto'
            _mapper.Map(dto, entity);

            await _unitOfWork.Alumnos.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Elimina un alumno (Soft Delete según la entidad).
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Alumnos.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Alumno con ID {id} no encontrado.");

            // Si tu Repositorio Genérico maneja SoftDelete automáticamente al llamar DeleteAsync:
            await _unitOfWork.Alumnos.DeleteAsync(entity);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
