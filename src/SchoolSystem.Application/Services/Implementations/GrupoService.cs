using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Calificaciones;
using SchoolSystem.Application.DTOs.Grupos;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Enums.Academico;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    public class GrupoService : IGrupoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GrupoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GrupoDto> GetByIdAsync(int id)
        {
            var allItems = await _unitOfWork.Grupos.GetAllIncludingAsync(
                g => g.Grado,
                g => g.MaestroTitular,
                g => g.MaestroTitular.Usuario
            );
            var entity = allItems.FirstOrDefault(g => g.Id == id);

            return _mapper.Map<GrupoDto>(entity);
        }

        public async Task<PagedResult<GrupoDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var allItems = await _unitOfWork.Grupos.GetAllIncludingAsync(
                g => g.Grado,
                g => g.Grado.NivelEducativo,
                g => g.MaestroTitular,
                g => g.MaestroTitular.Usuario
            );

            var total = allItems.Count();
            var items = allItems
                .OrderBy(g => g.Grado.NivelEducativo.Id)
                .ThenBy(g => g.Grado.Orden)
                .ThenBy(g => g.Nombre)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return new PagedResult<GrupoDto>
            {
                Items = _mapper.Map<IEnumerable<GrupoDto>>(items),
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> CreateAsync(CreateGrupoDto dto)
        {
            var entity = _mapper.Map<Grupo>(dto);
            await _unitOfWork.Grupos.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(int id, UpdateGrupoDto dto)
        {
            var grupo = await _unitOfWork.Grupos.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Grupo con ID {id} no encontrado");


            // REGLA: Validar Capacidad
            // Necesitamos contar los inscritos activos. 
            // Asumimos que tienes un método o usas FindAsync en el repo de inscripciones.
            var alumnosInscritos = (await _unitOfWork.Inscripciones
                .FindAsync(i => i.GrupoId == id && i.Estatus == EstatusInscripcion.Inscrito))
                .Count();

            // Ahora (Usando la entidad):
            // Asumimos que la entidad tiene cargada la colección de inscripciones
            // Nota: Aquí mapeamos primero para actualizar los valores propuestos
            _mapper.Map(dto, grupo);

            // Usamos el método de validación de la entidad
            if (!grupo.ConfiguracionEsValida(out string errorMsg))
            {
                throw new InvalidOperationException($"Configuración inválida: {errorMsg}");
            }


            await _unitOfWork.Grupos.UpdateAsync(grupo);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Grupos.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Grupo con ID {id} no encontrado");

            // REGLA: No eliminar si hay alumnos
            var tieneAlumnos = (await _unitOfWork.Inscripciones
                .FindAsync(i => i.GrupoId == id && !i.IsDeleted))
                .Any();

            if (tieneAlumnos)
            {
                throw new InvalidOperationException("No se puede eliminar el grupo porque tiene alumnos inscritos o historial académico. Considere desactivarlo.");
            }

            await _unitOfWork.Grupos.DeleteAsync(entity);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
