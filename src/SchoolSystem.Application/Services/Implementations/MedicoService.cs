using AutoMapper;
using SchoolSystem.Application.DTOs.Medico;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Medico;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    public class MedicoService : IMedicoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MedicoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ExpedienteMedicoDto> GetByAlumnoIdAsync(int alumnoId)
        {
            // Usamos FindAsync con Includes para traer Alergias y Vacunas
            var expedientes = await _unitOfWork.ExpedienteMedicos.FindAsync(
                e => e.AlumnoId == alumnoId,
                e => e.Alumno,
                e => e.AlergiasRegistradas,
                e => e.Vacunas
            );

            var expediente = expedientes.FirstOrDefault();
            return _mapper.Map<ExpedienteMedicoDto>(expediente);
        }

        public async Task<int> CreateAsync(CreateExpedienteDto dto)
        {
            // Verificar si ya existe
            var existe = (await _unitOfWork.ExpedienteMedicos.FindAsync(e => e.AlumnoId == dto.AlumnoId)).Any();
            if (existe)
                throw new System.Exception("El alumno ya tiene expediente.");

            var entity = _mapper.Map<ExpedienteMedico>(dto);
            entity.Activo = true;

            await _unitOfWork.ExpedienteMedicos.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(int id, UpdateExpedienteDto dto)
        {
            var entity = await _unitOfWork.ExpedienteMedicos.GetByIdAsync(id);
            if (entity == null)
                throw new System.Collections.Generic.KeyNotFoundException("Expediente no encontrado");

            _mapper.Map(dto, entity);
            await _unitOfWork.ExpedienteMedicos.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
