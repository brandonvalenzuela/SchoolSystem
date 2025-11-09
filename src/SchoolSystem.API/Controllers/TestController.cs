using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Infrastructure.Persistence;
using SchoolSystem.Infrastructure.Persistence.Context;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly SchoolSystemDbContext _context;

    public TestController(SchoolSystemDbContext context)
    {
        _context = context;
    }

    [HttpGet("health")]
    public async Task<IActionResult> Health()
    {
        try
        {
            // Verifica conexión
            await _context.Database.CanConnectAsync();

            // Cuenta registros
            var stats = new
            {
                Escuelas = await _context.Escuelas.CountAsync(),
                Usuarios = await _context.Usuarios.CountAsync(),
                Alumnos = await _context.Alumnos.CountAsync(),
                Maestros = await _context.Maestros.CountAsync(),
                Padres = await _context.Padres.CountAsync()
            };

            return Ok(new
            {
                Status = "Healthy",
                Database = "Connected",
                Stats = stats
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Status = "Unhealthy",
                Error = ex.Message
            });
        }
    }
}