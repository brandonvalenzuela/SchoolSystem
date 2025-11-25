using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Auth;
using SchoolSystem.Application.Services.Interfaces;
using System.Threading.Tasks;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Inicia sesión en el sistema y obtiene un token JWT.
        /// </summary>
        /// <param name="loginDto">Credenciales de acceso (usuario y contraseña).</param>
        /// <returns>Token JWT envuelto en ApiResponse.</returns>
        /// <remarks>
        /// Si las credenciales son incorrectas, el servicio lanzará una excepción que será 
        /// capturada por el middleware global, devolviendo un código 401 Unauthorized.
        /// </remarks>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<string>>> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>("Datos de inicio de sesión inválidos."));

            var token = await _authService.LoginAsync(loginDto);

            return Ok(new ApiResponse<string>(token, "Autenticación exitosa."));
        }
    }
}