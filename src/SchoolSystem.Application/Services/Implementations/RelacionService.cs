using AutoMapper;
using SchoolSystem.Application.DTOs.Academicos;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    public class RelacionService : IRelacionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RelacionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> VincularAsync(CreateAlumnoPadreDto dto)
        {
            // Validar que no exista ya la relación
            var existe = (await _unitOfWork.AlumnoPadres.FindAsync(ap =>
                ap.AlumnoId == dto.AlumnoId && ap.PadreId == dto.PadreId)).Any();

            if (existe)
                throw new System.Exception("La relación ya existe.");

            var entity = _mapper.Map<AlumnoPadre>(dto);
            await _unitOfWork.AlumnoPadres.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task DesvincularAsync(int id)
        {
            var entity = await _unitOfWork.AlumnoPadres.GetByIdAsync(id);
            if (entity == null)
                throw new System.Collections.Generic.KeyNotFoundException("Relación no encontrada");

            // Hard delete porque es una tabla de relación pura
            await _unitOfWork.AlumnoPadres.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<AlumnoPadreDto>> GetPorPadreAsync(int padreId)
        {
            // Necesitamos incluir Alumno y Padre.Usuario
            var relaciones = await _unitOfWork.AlumnoPadres.GetAllIncludingAsync(
                ap => ap.Alumno,
                ap => ap.Padre,
                ap => ap.Padre.Usuario
            );

            var filtradas = relaciones.Where(ap => ap.PadreId == padreId).ToList();
            return _mapper.Map<List<AlumnoPadreDto>>(filtradas);
        }

        public async Task<List<AlumnoPadreDto>> GetPorAlumnoAsync(int alumnoId)
        {
            var relaciones = await _unitOfWork.AlumnoPadres.GetAllIncludingAsync(
                ap => ap.Alumno,
                ap => ap.Padre,
                ap => ap.Padre.Usuario
            );

            var filtradas = relaciones.Where(ap => ap.AlumnoId == alumnoId).ToList();
            return _mapper.Map<List<AlumnoPadreDto>>(filtradas);
        }
    }
}
