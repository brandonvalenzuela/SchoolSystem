using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.ConfiguracionEscuela;
using SchoolSystem.Application.DTOs.Escuelas;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Configuracion;
using SchoolSystem.Domain.Entities.Escuelas;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Enums.Escuelas;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    public class EscuelaService : IEscuelaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;

        public EscuelaService(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<EscuelaDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Escuelas.GetByIdAsync(id);
            return _mapper.Map<EscuelaDto>(entity);
        }

        public async Task<PagedResult<EscuelaDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var allItems = await _unitOfWork.Escuelas.GetAllAsync();

            var total = allItems.Count();
            var items = allItems
                .OrderBy(e => e.Nombre)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return new PagedResult<EscuelaDto>
            {
                Items = _mapper.Map<IEnumerable<EscuelaDto>>(items),
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        // =========================================================
        // ONBOARDING AUTOMÁTICO (CreateAsync)
        // =========================================================
        public async Task<int> CreateAsync(CreateEscuelaDto dto)
        {
            // 1. Validar que el código no exista (Regla de negocio crítica)
            var existeCodigo = await _unitOfWork.Escuelas.FindAsync(e => e.Codigo == dto.Codigo);
            if (existeCodigo.Any())
                throw new InvalidOperationException($"El código escolar '{dto.Codigo}' ya está en uso.");

            // 2. Crear la Escuela
            var escuela = _mapper.Map<Escuela>(dto);
            escuela.FechaRegistro = DateTime.Now;
            escuela.Activo = true;

            // Estrategia de transacción implícita con UnitOfWork
            await _unitOfWork.Escuelas.AddAsync(escuela);
            await _unitOfWork.SaveChangesAsync(); // Guardamos para obtener el ID de la escuela

            // 3. Crear Configuración por Defecto (Vital para que el sistema funcione)
            var config = new ConfiguracionEscuela
            {
                EscuelaId = escuela.Id,
                NombreInstitucion = escuela.Nombre,
                NombreCorto = escuela.Nombre.Length > 15 ? escuela.Nombre.Substring(0, 15) : escuela.Nombre,
                SistemaCalificacion = "Numerico",
                CalificacionMinimaAprobatoria = 6.0m,
                CalificacionMaxima = 10.0m,
                ZonaHoraria = "America/Mexico_City",
                IdiomaPredeterminado = "es-MX",
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.ConfiguracionEscuelas.AddAsync(config);

            // 4. Crear Usuario Administrador Inicial (Director)
            // Generamos un usuario basado en el código de la escuela o el email de contacto
            var adminUser = new Usuario
            {
                EscuelaId = escuela.Id,
                Username = $"admin.{escuela.Codigo.ToLower()}", // Ej: admin.ievalle
                Email = escuela.Email, // Usamos el email de contacto de la escuela
                Nombre = "Administrador",
                ApellidoPaterno = "Sistema",
                ApellidoMaterno = "",
                Rol = RolUsuario.Director,
                Activo = true,
                CreatedAt = DateTime.Now,
                // Contraseña temporal por defecto: "Admin123!" (Debería enviarse por email en un sistema real)
                PasswordHash = _passwordHasher.HashPassword("Admin123!"),
                Telefono = escuela.Telefono,
                TelefonoEmergencia = escuela.Telefono
            };
            await _unitOfWork.Usuarios.AddAsync(adminUser);

            // 5. Guardar todo lo restante
            await _unitOfWork.SaveChangesAsync();

            return escuela.Id;
        }

        public async Task UpdateAsync(int id, UpdateEscuelaDto dto)
        {
            var entity = await _unitOfWork.Escuelas.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Escuela con ID {id} no encontrada");

            _mapper.Map(dto, entity);

            await _unitOfWork.Escuelas.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Escuelas.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Escuela con ID {id} no encontrada");

            await _unitOfWork.Escuelas.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        // =========================================================
        // DASHBOARD / RESUMEN (GetResumenAsync)
        // =========================================================
        public async Task<ResumenEscuelaDto> GetResumenAsync(int id)
        {
            var escuela = await _unitOfWork.Escuelas.GetByIdAsync(id);
            if (escuela == null)
                return null;

            // Consultas optimizadas usando CountAsync (solo devuelve un número, no trae datos)
            // Nota: Asegúrate de agregar los repositorios al IUnitOfWork si no están
            var totalAlumnos = await _unitOfWork.Alumnos.CountAsync(a => a.EscuelaId == id && !a.IsDeleted);
            var totalMaestros = await _unitOfWork.Maestros.CountAsync(m => m.EscuelaId == id);
            var totalGrupos = await _unitOfWork.Grupos.CountAsync(g => g.EscuelaId == id && g.Activo);

            // Calcular ocupación
            decimal ocupacion = 0;
            if (escuela.MaxAlumnos.HasValue && escuela.MaxAlumnos.Value > 0)
            {
                ocupacion = Math.Round(((decimal)totalAlumnos / escuela.MaxAlumnos.Value) * 100, 1);
            }

            return new ResumenEscuelaDto
            {
                Id = escuela.Id,
                Nombre = escuela.Nombre,
                Codigo = escuela.Codigo,
                PlanActual = escuela.TipoPlan?.ToString() ?? "N/A",
                EstadoSuscripcion = escuela.TieneSuscripcionVigente() ? "Activa" : "Vencida",
                TotalAlumnos = totalAlumnos,
                TotalMaestros = totalMaestros,
                TotalGrupos = totalGrupos,
                CapacidadAlumnos = escuela.MaxAlumnos,
                PorcentajeOcupacionAlumnos = ocupacion
            };
        }

        public async Task<ConfiguracionEscuelaDto> GetConfiguracionAsync(int escuelaId)
        {
            // Necesitas agregar el repositorio al UnitOfWork si no está
            var config = (await _unitOfWork.ConfiguracionEscuelas.FindAsync(c => c.EscuelaId == escuelaId)).FirstOrDefault();

            if (config == null)
                return null;

            return _mapper.Map<ConfiguracionEscuelaDto>(config);
        }

        public async Task UpdateConfiguracionAsync(int escuelaId, UpdateConfiguracionEscuelaDto dto)
        {
            var config = (await _unitOfWork.ConfiguracionEscuelas.FindAsync(c => c.EscuelaId == escuelaId)).FirstOrDefault();

            if (config == null)
                throw new KeyNotFoundException("Configuración no encontrada.");

            _mapper.Map(dto, config);

            await _unitOfWork.ConfiguracionEscuelas.UpdateAsync(config);
            await _unitOfWork.SaveChangesAsync();
        }


    }
}
