using AutoMapper;
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
        private readonly IRepository<Alumno> _alumnoRepository;
        private readonly IMapper _mapper;

        public AlumnoService(IRepository<Alumno> alumnoRepository, IMapper mapper)
        {
            _alumnoRepository = alumnoRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(CreateAlumnoDto dto)
        {
            var alumno = _mapper.Map<Alumno>(dto);
            await _alumnoRepository.AddAsync(alumno);
            await _alumnoRepository.SaveChangesAsync();
            return alumno.Id;
        }

        public async Task<AlumnoDto> GetByIdAsync(int id)
        {
            var alumno = await _alumnoRepository.GetByIdAsync(id);
            return _mapper.Map<AlumnoDto>(alumno);
        }

        public async Task UpdateAsync(UpdateAlumnoDto dto)
        {
            var alumno = await _alumnoRepository.GetByIdAsync(dto.Id);
            if (alumno == null)
                throw new KeyNotFoundException("Alumno no encontrado");

            _mapper.Map(dto, alumno); // actualiza propiedades
            await _alumnoRepository.UpdateAsync(alumno);
            await _alumnoRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var alumno = await _alumnoRepository.GetByIdAsync(id);
            if (alumno == null)
                throw new KeyNotFoundException("Alumno no encontrado");
            await _alumnoRepository.DeleteAsync(alumno);
            await _alumnoRepository.SaveChangesAsync();
        }

    }
}
