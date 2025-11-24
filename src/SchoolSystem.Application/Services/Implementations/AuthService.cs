using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SchoolSystem.Application.DTOs.Auth;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SchoolSystem.Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<Usuario> _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IRepository<Usuario> userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            // 1. Buscar usuario
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Username == loginDto.Username);

            // 2. Validar usuario y contraseña (¡Aquí deberías comparar HASHES, no texto plano!)
            // Por ahora, para el ejemplo, comparamos directo.
            if (user == null || user.PasswordHash != loginDto.Password)
            {
                throw new UnauthorizedAccessException("Credenciales inválidas.");
            }

            if (!user.Activo)
                throw new UnauthorizedAccessException("El usuario está inactivo.");

            // 3. Generar Token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Rol.ToString()), // Importante para roles
                new Claim("EscuelaId", user.EscuelaId.ToString()) // Custom claim
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:DurationInMinutes"])),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
