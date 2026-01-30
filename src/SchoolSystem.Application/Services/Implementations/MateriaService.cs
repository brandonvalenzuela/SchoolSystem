using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Materias;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    public class MateriaService : IMateriaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MateriaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MateriaDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Materias.GetByIdAsync(id);
            return _mapper.Map<MateriaDto>(entity);
        }

        public async Task<PagedResult<MateriaDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var allItems = await _unitOfWork.Materias.GetAllAsync();
            var total = allItems.Count();
            var items = allItems.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PagedResult<MateriaDto>
            {
                Items = _mapper.Map<IEnumerable<MateriaDto>>(items),
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> CreateAsync(CreateMateriaDto dto)
        {
            // HARDENING: Validar datos de entrada
            ValidateMateriaInput(dto);

            // REGLA: Clave única por escuela
            var existeClave = (await _unitOfWork.Materias
                .FindAsync(m => m.EscuelaId == dto.EscuelaId && m.Clave == dto.Clave && !m.IsDeleted))
                .Any();

            if (existeClave)
                throw new InvalidOperationException($"Ya existe una materia con la clave '{dto.Clave}'.");

            // HARDENING: Validar que no exista materia con el mismo nombre en la escuela
            var existeNombre = (await _unitOfWork.Materias
                .FindAsync(m => m.EscuelaId == dto.EscuelaId && m.Nombre == dto.Nombre && !m.IsDeleted))
                .Any();

            if (existeNombre)
                throw new InvalidOperationException($"Ya existe una materia con el nombre '{dto.Nombre}' en esta escuela.");

            var entity = _mapper.Map<Materia>(dto);
            await _unitOfWork.Materias.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(int id, UpdateMateriaDto dto)
        {
            var entity = await _unitOfWork.Materias.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Materia con ID {id} no encontrada");

            // HARDENING: Validar datos de entrada
            ValidateMateriaInput(dto);

            // HARDENING: Si el nombre cambió, validar que no exista otro con el mismo nombre
            if (entity.Nombre != dto.Nombre)
            {
                var existeNombre = (await _unitOfWork.Materias
                    .FindAsync(m => m.EscuelaId == entity.EscuelaId && 
                              m.Nombre == dto.Nombre && 
                              m.Id != id && 
                              !m.IsDeleted))
                    .Any();

                if (existeNombre)
                    throw new InvalidOperationException($"Ya existe una materia con el nombre '{dto.Nombre}' en esta escuela.");
            }

            _mapper.Map(dto, entity);
            await _unitOfWork.Materias.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var materia = await _unitOfWork.Materias.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Materia con ID {id} no encontrada.");

            // REGLA: Integridad Académica
            var tieneCalificaciones = (await _unitOfWork.Calificaciones
                .FindAsync(c => c.MateriaId == id && !c.IsDeleted))
                .Any();

            if (tieneCalificaciones)
            {
                throw new InvalidOperationException(
                    "No se puede eliminar la materia porque existen calificaciones asociadas. " +
                    "Esto dañaría el historial académico de los alumnos. Desactívela en su lugar.");
            }

                        await _unitOfWork.Materias.DeleteAsync(materia);

                        await _unitOfWork.SaveChangesAsync();
                    }

                    /// <summary>
                    /// Valida que los datos de una materia sean válidos antes de crear/actualizar
                    /// </summary>
                    private void ValidateMateriaInput(object dto)
                    {
                        // Validar que sea CreateMateriaDto o UpdateMateriaDto
                        if (dto is CreateMateriaDto createDto)
                        {
                            // Validar Nombre
                            if (string.IsNullOrWhiteSpace(createDto.Nombre))
                                throw new ArgumentException("El nombre de la materia es requerido.");

                            if (createDto.Nombre.Length > 200)
                                throw new ArgumentException("El nombre no puede exceder 200 caracteres.");

                            // HARDENING: Validar ColorHex si está presente
                            if (!string.IsNullOrWhiteSpace(createDto.ColorHex))
                            {
                                ValidateColorHex(createDto.ColorHex);
                            }
                        }
                        else if (dto is UpdateMateriaDto updateDto)
                        {
                            // Validar Nombre
                            if (string.IsNullOrWhiteSpace(updateDto.Nombre))
                                throw new ArgumentException("El nombre de la materia es requerido.");

                            if (updateDto.Nombre.Length > 200)
                                throw new ArgumentException("El nombre no puede exceder 200 caracteres.");

                            // HARDENING: Validar ColorHex si está presente
                            if (!string.IsNullOrWhiteSpace(updateDto.ColorHex))
                            {
                                ValidateColorHex(updateDto.ColorHex);
                            }
                        }
                    }

                    /// <summary>
                    /// Valida que el ColorHex tenga formato correcto (#RRGGBB)
                    /// </summary>
                    private void ValidateColorHex(string colorHex)
                    {
                        // Patrón: debe ser #RRGGBB (7 caracteres, # seguido de 6 hexadecimales)
                        var hexPattern = @"^#[0-9A-Fa-f]{6}$";
                        if (!Regex.IsMatch(colorHex, hexPattern))
                        {
                            throw new ArgumentException(
                                $"ColorHex inválido: '{colorHex}'. Debe estar en formato #RRGGBB (ej: #FF5733)."
                            );
                        }
                    }
                }
            }
