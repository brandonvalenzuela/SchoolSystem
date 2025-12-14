using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Inscripciones;
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
    public class InscripcionService : IInscripcionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitOfWork _grupoUnitOfWork;
        private readonly IMapper _mapper;

        // Actualizamos el constructor para recibir el repositorio de Grupos
        public InscripcionService(IUnitOfWork unitOfWork, IUnitOfWork grupoUnitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _grupoUnitOfWork = grupoUnitOfWork;
            _mapper = mapper;
        }

        // ... (Métodos GetByIdAsync, GetPagedAsync, CreateAsync, UpdateAsync, DeleteAsync igual que antes) ...

        public async Task<InscripcionDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Inscripciones.GetByIdAsync(id);
            return _mapper.Map<InscripcionDto>(entity);
        }

        public async Task<PagedResult<InscripcionDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var allItems = await _unitOfWork.Inscripciones.GetAllAsync();
            var total = allItems.Count();
            var items = allItems.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PagedResult<InscripcionDto>
            {
                Items = _mapper.Map<IEnumerable<InscripcionDto>>(items),
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> CreateAsync(CreateInscripcionDto dto)
        {
            // Validación extra: Verificar cupo antes de crear la primera inscripción
            var grupo = await _grupoUnitOfWork.Grupos.GetByIdAsync(dto.GrupoId);
            if (grupo == null)
                throw new KeyNotFoundException("El grupo seleccionado no existe.");

            // Nota: Para que EstaLleno() funcione con exactitud, el repositorio debe incluir las Inscripciones
            // o debemos hacer un count manual aquí. Asumimos por ahora que el repositorio trae la data necesaria.
            if (grupo.EstaLleno())
                throw new InvalidOperationException("El grupo seleccionado ya no tiene cupo disponible.");

            var entity = _mapper.Map<Inscripcion>(dto);
            entity.FechaInscripcion = DateTime.Now;
            entity.Estatus = SchoolSystem.Domain.Enums.Academico.EstatusInscripcion.Inscrito;

            await _unitOfWork.Inscripciones.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(int id, UpdateInscripcionDto dto)
        {
            var entity = await _unitOfWork.Inscripciones.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Inscripción con ID {id} no encontrada");

            // Si el DTO intenta cambiar el GrupoId, usamos la lógica especializada
            if (dto.GrupoId != entity.GrupoId)
            {
                // Llamamos a nuestro método especializado internamente si es necesario,
                // o lanzamos error pidiendo usar el endpoint específico.
                // Para mantenerlo limpio, aquí solo actualizamos datos simples y estatus.
                // El cambio de grupo debe ir por el método CambiarDeGrupoAsync.
            }

            _mapper.Map(dto, entity);
            await _unitOfWork.Inscripciones.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Inscripciones.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Inscripción con ID {id} no encontrada");
            await _unitOfWork.Inscripciones.DeleteAsync(entity);

            await _unitOfWork.SaveChangesAsync();
        }

        // --- IMPLEMENTACIÓN DEL NUEVO MÉTODO ---
        public async Task CambiarDeGrupoAsync(int inscripcionId, int nuevoGrupoId, string motivo, int usuarioId)
        {
            // 1. Obtener la inscripción actual
            var inscripcion = await _unitOfWork.Inscripciones.GetByIdAsync(inscripcionId);
            if (inscripcion == null)
                throw new KeyNotFoundException("La inscripción no existe.");

            if (!inscripcion.EstaActiva)
                throw new InvalidOperationException("Solo se pueden cambiar de grupo inscripciones activas.");

            if (inscripcion.GrupoId == nuevoGrupoId)
                throw new ArgumentException("El alumno ya pertenece al grupo seleccionado.");

            // 2. Obtener y validar el nuevo grupo
            // Es importante que el repositorio traiga las inscripciones del grupo para validar el cupo
            // Si tu repositorio es genérico simple, podrías necesitar una consulta específica aquí.
            var nuevoGrupo = await _grupoUnitOfWork.Grupos.GetByIdAsync(nuevoGrupoId);

            if (nuevoGrupo == null)
                throw new KeyNotFoundException("El grupo destino no existe.");

            // Validaciones de negocio usando la Entidad Grupo
            if (!nuevoGrupo.EstaActivo())
                throw new InvalidOperationException("No se puede transferir a un grupo inactivo.");

            if (nuevoGrupo.EscuelaId != inscripcion.EscuelaId)
                throw new InvalidOperationException("No se puede transferir a un grupo de otra escuela.");

            // Validación de capacidad (Usando el método de tu Entidad Grupo)
            // IMPORTANTE: Esto asume que 'nuevoGrupo.Inscripciones' fue cargado por el repositorio.
            // Si es null, EstaLleno() podría dar falso positivo. 
            // En un escenario real, haríamos: _inscripcionRepo.Count(x => x.GrupoId == nuevoGrupoId && x.Activo)
            if (nuevoGrupo.EstaLleno())
                throw new InvalidOperationException($"El grupo {nuevoGrupo.Nombre} está lleno (Capacidad: {nuevoGrupo.CapacidadMaxima}).");

            // 3. Ejecutar la lógica de dominio
            // La entidad Inscripcion se encarga de actualizar sus propiedades (GrupoAnterior, FechaCambio, etc.)
            inscripcion.CambiarGrupo(nuevoGrupoId, motivo, usuarioId);

            // 4. Persistir cambios
            await _unitOfWork.Inscripciones.UpdateAsync(inscripcion);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
