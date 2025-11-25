using Moq;
using SchoolSystem.Application.DTOs.Alumnos;
using SchoolSystem.Application.Services.Implementations;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Interfaces;
using Xunit;

namespace SchoolSystem.UnitTests;

public class AlumnoServiceTests
{
   /* 
    private readonly Mock<IRepository<Alumno>> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly AlumnoService _service;

    public AlumnoServiceTests()
    {
        _mockRepo = new Mock<IRepository<Alumno>>();
        _mockMapper = new Mock<IMapper>();
        _service = new AlumnoService(_mockRepo.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetByIdAsync_DeberiaRetornarDto_CuandoExisteAlumno()
    {
        // Arrange (Preparar)
        var alumno = new Alumno { Id = 1, Nombre = "Juan" };
        var alumnoDto = new AlumnoDto { Id = 1, Nombre = "Juan" };

        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(alumno);
        _mockMapper.Setup(m => m.Map<AlumnoDto>(alumno)).Returns(alumnoDto);

        // Act (Actuar)
        var result = await _service.GetByIdAsync(1);

        // Assert (Verificar)
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Juan", result.Nombre);
    }
   */
}