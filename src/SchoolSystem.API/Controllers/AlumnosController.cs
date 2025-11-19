using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.DTOs.Alumnos;
using SchoolSystem.Application.Services.Interfaces;

namespace SchoolSystem.API.Controllers
{
    public class AlumnosController : ControllerBase
    {
        private readonly IAlumnoService _alumnoService;
        private readonly IMapper _mapper;

        public AlumnosController(IAlumnoService alumnoService, IMapper mapper)
        {
            _alumnoService = alumnoService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlumnoDto>> Get(int id)
        {
            var alumno = await _alumnoService.GetByIdAsync(id);
            return Ok(_mapper.Map<AlumnoDto>(alumno));
        }
    }
}
