using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Alumnos;
using SchoolSystem.Application.DTOs.Filtros;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Constants;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Enums.Academico;
using SchoolSystem.Domain.Interfaces;

namespace SchoolSystem.Application.Services.Implementations
{
    public class AlumnoService : IAlumnoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser; // <--- Inyectamos esto


        public AlumnoService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        /// <summary>
        /// Obtiene un alumno por su ID.
        /// </summary>
        public async Task<AlumnoDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Alumnos.GetByIdAsync(id);

            // Si es null, el controlador puede devolver NotFound, 
            // o podemos lanzar una KeyNotFoundException aquí.
            return _mapper.Map<AlumnoDto>(entity);
        }

        /// <summary>
        /// Obtiene una lista paginada de alumnos.
        /// </summary>
        public async Task<PagedResult<AlumnoDto>> GetPagedAsync(AlumnoFilterDto filter)
        {
            // 1. Empezamos con la consulta base (IQueryable para no traer datos aún)
            // Necesitamos un método en el repo que devuelva IQueryable, o usamos el contexto directo si UnitOfWork lo permite.
            // Asumiremos que Repository.GetQueryable() existe o lo agregamos.
            var query = _unitOfWork.Alumnos.GetQueryable();

            // 2. OBTENER CONTEXTO DEL USUARIO
            var userId = _currentUser.UserId;
            var rol = _currentUser.Role; // Necesitas exponer el Rol en ICurrentUserService
            var escuelaId = _currentUser.EscuelaId; // Necesitas exponer la Escuela en ICurrentUserService

            // 3. APLICAR FILTROS DE SEGURIDAD (DATA SCOPING)

            // Regla 1: Multi-tenant (Siempre filtrar por escuela)
            query = query.Where(a => a.EscuelaId == escuelaId);

            // Regla 2: Filtros por Rol
            if (rol == Roles.Maestro)
            {
                // El maestro solo ve alumnos que están inscritos en SUS grupos.
                // Esto requiere un JOIN complejo.
                // Lógica: Alumnos -> Inscripciones -> Grupo -> (MaestroTitular == Yo O MateriaImpartidaPor == Yo)

                // Primero buscamos el ID del Maestro ligado a este Usuario
                var maestro = await _unitOfWork.Maestros.FirstOrDefaultAsync(m => m.UsuarioId == userId);
                if (maestro != null)
                {
                    var maestroId = maestro.Id;

                    query = query.Where(a => a.Inscripciones.Any(i =>
                        i.Estatus == EstatusInscripcion.Inscrito &&
                        (i.Grupo.MaestroTitularId == maestroId ||
                         i.Grupo.GrupoMateriaMaestros.Any(gm => gm.MaestroId == maestroId))
                    ));
                }
            }
            else if (rol == Roles.Padre)
            {
                // El padre solo ve a sus hijos
                // Lógica: Alumno -> AlumnoPadres -> Padre -> UsuarioId == Yo
                query = query.Where(a => a.AlumnoPadres.Any(ap => ap.Padre.UsuarioId == userId));
            }
            // Si es Director o Admin, no aplicamos filtro extra (ven todo lo de la escuela).

            // 4. APLICAR FILTROS DE BÚSQUEDA DEL USUARIO (Lo que el usuario escribió)
            if (!string.IsNullOrEmpty(filter.TerminoBusqueda))
            {
                var term = filter.TerminoBusqueda.ToLower().Trim();
                // Concatenamos todas las partes del nombre y la matrícula en una sola cadena larga
                // y verificamos si lo que escribió el usuario existe dentro de esa cadena completa.
                // El (a.ApellidoMaterno ?? "") evita errores si el alumno no tiene segundo apellido.
                query = query.Where(a =>
                    (a.Nombre + " " + a.ApellidoPaterno + " " + (a.ApellidoMaterno ?? "") + " " + a.Matricula)
                    .ToLower()
                    .Contains(term)
                );
            }

            // Filtro por Grupo específico (para directores)
            if (!string.IsNullOrEmpty(filter.GrupoId) && int.TryParse(filter.GrupoId, out int grupoId) && (rol == Roles.Director || rol == Roles.SuperAdmin))
            {
                // Permitir al director filtrar por grupo específico si quiere
                query = query.Where(a => a.Inscripciones.Any(i => i.GrupoId == int.Parse(filter.GrupoId)));
            }

            // Filtro por Estatus (si viene en el filtro)
            if (!string.IsNullOrEmpty(filter.Estatus) && Enum.TryParse<EstatusAlumno>(filter.Estatus, true, out var estatusEnum))
            {
                query = query.Where(a => a.Estatus == estatusEnum);
            }

            // 5. EJECUTAR CONSULTA PAGINADA
            var total = await query.CountAsync();

            var items = await query
                .OrderBy(a => a.ApellidoPaterno)
                .ThenBy(a => a.Nombre)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PagedResult<AlumnoDto>
            {
                Items = _mapper.Map<IEnumerable<AlumnoDto>>(items),
                TotalItems = total,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        /// <summary>
        /// Crea un nuevo alumno.
        /// </summary>
        public async Task<int> CreateAsync(CreateAlumnoDto dto)
        {
            var entity = _mapper.Map<Alumno>(dto);

            // Lógica de negocio adicional al crear
            entity.FechaIngreso = DateTime.Now;
            entity.Estatus = SchoolSystem.Domain.Enums.Academico.EstatusAlumno.Activo;

            await _unitOfWork.Alumnos.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.Id;
        }

        /// <summary>
        /// Actualiza un alumno existente.
        /// </summary>
        public async Task UpdateAsync(int id, UpdateAlumnoDto dto)
        {
            var entity = await _unitOfWork.Alumnos.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Alumno con ID {id} no encontrado.");

            // AutoMapper actualiza las propiedades de 'entity' con los valores de 'dto'
            _mapper.Map(dto, entity);

            await _unitOfWork.Alumnos.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Elimina un alumno (Soft Delete según la entidad).
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Alumnos.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Alumno con ID {id} no encontrado.");

            // Si tu Repositorio Genérico maneja SoftDelete automáticamente al llamar DeleteAsync:
            await _unitOfWork.Alumnos.DeleteAsync(entity);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
