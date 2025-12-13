# ============================================
# Script Generador de Entidades
# Sistema de Gestión Escolar
# ============================================

param(
    [Parameter(Mandatory=$false)]
    [string]$EntityName,
    
    [Parameter(Mandatory=$false)]
    [ValidateSet("Escuelas", "Usuarios", "Academico", "Evaluacion", "Conducta", "Comunicacion", "Finanzas", "Biblioteca", "Eventos", "Otros")]
    [string]$Module,
    
    [Parameter(Mandatory=$false)]
    [switch]$SkipRepository,
    
    [Parameter(Mandatory=$false)]
    [switch]$SkipService,
    
    [Parameter(Mandatory=$false)]
    [switch]$SkipDTO
)

# Colores para output
function Write-Success {
    param([string]$Message)
    Write-Host $Message -ForegroundColor Green
}

function Write-Info {
    param([string]$Message)
    Write-Host $Message -ForegroundColor Cyan
}

function Write-Warning {
    param([string]$Message)
    Write-Host $Message -ForegroundColor Yellow
}

function Write-Error {
    param([string]$Message)
    Write-Host $Message -ForegroundColor Red
}

function Write-Header {
    param([string]$Message)
    Write-Host "`n╔════════════════════════════════════════════════╗" -ForegroundColor Magenta
    Write-Host "║  $Message" -ForegroundColor Magenta
    Write-Host "╚════════════════════════════════════════════════╝`n" -ForegroundColor Magenta
}

# Verificar estructura del proyecto
function Test-ProjectStructure {
    if (-not (Test-Path "SchoolSystem.sln")) {
        Write-Error "❌ Error: No se encontró SchoolSystem.sln"
        Write-Warning "Por favor, ejecuta este script desde el directorio raíz del proyecto."
        return $false
    }
    return $true
}

# Convertir nombre a plural (simple)
function Get-PluralName {
    param([string]$Name)
    
    if ($Name.EndsWith("s") -or $Name.EndsWith("x") -or $Name.EndsWith("z")) {
        return $Name + "es"
    }
    elseif ($Name.EndsWith("y")) {
        return $Name.Substring(0, $Name.Length - 1) + "ies"
    }
    else {
        return $Name + "s"
    }
}

# Solicitar información si no se proporcionó
function Get-EntityInformation {
    if ([string]::IsNullOrWhiteSpace($script:EntityName)) {
        Write-Header "INFORMACIÓN DE LA ENTIDAD"
        Write-Info "Ingresa el nombre de la entidad (PascalCase, singular):"
        Write-Info "Ejemplos: Tarea, Evento, Comunicado, Permiso"
        $script:EntityName = Read-Host "Nombre de la entidad"
        
        if ([string]::IsNullOrWhiteSpace($script:EntityName)) {
            Write-Error "❌ El nombre de la entidad es obligatorio"
            return $false
        }
        
        # Validar formato PascalCase
        if ($script:EntityName -notmatch '^[A-Z][a-zA-Z0-9]*$') {
            Write-Warning "⚠️  Se recomienda usar PascalCase (ej: MiEntidad)"
        }
    }
    
    if ([string]::IsNullOrWhiteSpace($script:Module)) {
        Write-Info "`nSelecciona el módulo al que pertenece:"
        Write-Host "  1. Escuelas"
        Write-Host "  2. Usuarios"
        Write-Host "  3. Academico"
        Write-Host "  4. Evaluacion"
        Write-Host "  5. Conducta"
        Write-Host "  6. Comunicacion"
        Write-Host "  7. Finanzas"
        Write-Host "  8. Biblioteca"
        Write-Host "  9. Eventos"
        Write-Host "  10. Otros"
        
        $moduleOption = Read-Host "`nOpción"
        
        $script:Module = switch ($moduleOption) {
            '1' { "Escuelas" }
            '2' { "Usuarios" }
            '3' { "Academico" }
            '4' { "Evaluacion" }
            '5' { "Conducta" }
            '6' { "Comunicacion" }
            '7' { "Finanzas" }
            '8' { "Biblioteca" }
            '9' { "Eventos" }
            '10' { "Otros" }
            default { "Otros" }
        }
    }
    
    return $true
}

# Crear entidad en Domain
function New-DomainEntity {
    Write-Header "CREANDO ENTIDAD EN DOMAIN"
    
    $entityPath = "src\SchoolSystem.Domain\Entities\$Module"
    
    # Crear directorio si no existe
    if (-not (Test-Path $entityPath)) {
        New-Item -ItemType Directory -Force -Path $entityPath | Out-Null
    }
    
    $entityContent = @"
using SchoolSystem.Domain.Entities.Common;
using System;
using System.Collections.Generic;

namespace SchoolSystem.Domain.Entities.$Module
{
    /// <summary>
    /// Entidad $EntityName
    /// </summary>
    public class $EntityName : BaseEntity
    {
        // ID heredado de BaseEntity
        
        /// <summary>
        /// ID de la escuela (Multi-tenant)
        /// </summary>
        public int EscuelaId { get; set; }
        
        // TODO: Agregar propiedades específicas de $EntityName
        // Ejemplo:
        // public string Nombre { get; set; }
        // public string Descripcion { get; set; }
        // public bool Activo { get; set; }
        
        #region Propiedades de Auditoría
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        #endregion
        
        #region Relaciones (Navigation Properties)
        
        // TODO: Definir relaciones con otras entidades
        // Ejemplo:
        // public virtual Escuela Escuela { get; set; }
        // public virtual ICollection<OtraEntidad> OtrasEntidades { get; set; }
        
        #endregion
        
        #region Métodos de Negocio
        
        // TODO: Agregar métodos de lógica de negocio si es necesario
        // Ejemplo:
        // public void Activar()
        // {
        //     Activo = true;
        //     UpdatedAt = DateTime.Now;
        // }
        
        #endregion
    }
}
"@

    $entityFile = "$entityPath\$EntityName.cs"
    $entityContent | Out-File -FilePath $entityFile -Encoding UTF8
    
    Write-Success "✓ Entidad creada: $entityFile"
}

# Crear BaseEntity si no existe
function New-BaseEntity {
    $basePath = "src\SchoolSystem.Domain\Entities\Common"
    $baseFile = "$basePath\BaseEntity.cs"
    
    if (Test-Path $baseFile) {
        return
    }
    
    Write-Info "Creando BaseEntity..."
    
    if (-not (Test-Path $basePath)) {
        New-Item -ItemType Directory -Force -Path $basePath | Out-Null
    }
    
    $baseContent = @"
namespace SchoolSystem.Domain.Entities.Common
{
    /// <summary>
    /// Clase base para todas las entidades
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Identificador único de la entidad
        /// </summary>
        public int Id { get; set; }
    }
}
"@

    $baseContent | Out-File -FilePath $baseFile -Encoding UTF8
    Write-Success "✓ BaseEntity creada"
}

# Crear DTOs
function New-DTOs {
    if ($SkipDTO) {
        Write-Info "⏭️  Omitiendo creación de DTOs"
        return
    }
    
    Write-Header "CREANDO DTOs"
    
    $pluralName = Get-PluralName -Name $EntityName
    $dtoPath = "src\SchoolSystem.Application\DTOs\$pluralName"
    
    # Crear directorio si no existe
    if (-not (Test-Path $dtoPath)) {
        New-Item -ItemType Directory -Force -Path $dtoPath | Out-Null
    }
    
    # DTO de lectura
    $dtoContent = @"
using System;

namespace SchoolSystem.Application.DTOs.$pluralName
{
    /// <summary>
    /// DTO para lectura de $EntityName
    /// </summary>
    public class ${EntityName}Dto
    {
        public int Id { get; set; }
        public int EscuelaId { get; set; }
        
        // TODO: Agregar propiedades que se mostrarán al cliente
        // public string Nombre { get; set; }
        // public string Descripcion { get; set; }
        // public bool Activo { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
"@

    $dtoFile = "$dtoPath\${EntityName}Dto.cs"
    $dtoContent | Out-File -FilePath $dtoFile -Encoding UTF8
    Write-Success "✓ DTO de lectura creado: $dtoFile"
    
    # DTO de creación
    $createDtoContent = @"
using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Application.DTOs.$pluralName
{
    /// <summary>
    /// DTO para crear $EntityName
    /// </summary>
    public class Create${EntityName}Dto
    {
        // TODO: Agregar propiedades necesarias para crear
        // con sus validaciones correspondientes
        
        // [Required(ErrorMessage = "El nombre es requerido")]
        // [MaxLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        // public string Nombre { get; set; }
        
        // [MaxLength(500)]
        // public string Descripcion { get; set; }
    }
}
"@

    $createDtoFile = "$dtoPath\Create${EntityName}Dto.cs"
    $createDtoContent | Out-File -FilePath $createDtoFile -Encoding UTF8
    Write-Success "✓ DTO de creación creado: $createDtoFile"
    
    # DTO de actualización
    $updateDtoContent = @"
using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Application.DTOs.$pluralName
{
    /// <summary>
    /// DTO para actualizar $EntityName
    /// </summary>
    public class Update${EntityName}Dto
    {
        // TODO: Agregar propiedades que se pueden actualizar
        // con sus validaciones correspondientes
        
        // [Required(ErrorMessage = "El nombre es requerido")]
        // [MaxLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        // public string Nombre { get; set; }
        
        // [MaxLength(500)]
        // public string Descripcion { get; set; }
    }
}
"@

    $updateDtoFile = "$dtoPath\Update${EntityName}Dto.cs"
    $updateDtoContent | Out-File -FilePath $updateDtoFile -Encoding UTF8
    Write-Success "✓ DTO de actualización creado: $updateDtoFile"
}

# Crear interfaz de repositorio
function New-RepositoryInterface {
    if ($SkipRepository) {
        Write-Info "⏭️  Omitiendo creación de Repositorio"
        return
    }
    
    Write-Header "CREANDO INTERFAZ DE REPOSITORIO"
    
    $interfacePath = "src\SchoolSystem.Domain\Interfaces"
    
    if (-not (Test-Path $interfacePath)) {
        New-Item -ItemType Directory -Force -Path $interfacePath | Out-Null
    }
    
    $interfaceContent = @"
using SchoolSystem.Domain.Entities.$Module;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolSystem.Domain.Interfaces
{
    /// <summary>
    /// Interfaz de repositorio para $EntityName
    /// </summary>
    public interface I${EntityName}Repository : IRepository<$EntityName>
    {
        // Métodos específicos de $EntityName (además de los heredados de IRepository)
        
        /// <summary>
        /// Obtiene todos los ${EntityName}s activos de una escuela
        /// </summary>
        Task<IEnumerable<$EntityName>> GetByEscuelaIdAsync(int escuelaId);
        
        // TODO: Agregar métodos específicos según necesidades
        // Ejemplo:
        // Task<$EntityName> GetByNombreAsync(string nombre);
        // Task<IEnumerable<$EntityName>> GetActivosAsync(int escuelaId);
    }
}
"@

    $interfaceFile = "$interfacePath\I${EntityName}Repository.cs"
    $interfaceContent | Out-File -FilePath $interfaceFile -Encoding UTF8
    
    Write-Success "✓ Interfaz de repositorio creada: $interfaceFile"
}

# Crear implementación de repositorio
function New-RepositoryImplementation {
    if ($SkipRepository) {
        return
    }
    
    Write-Header "CREANDO IMPLEMENTACIÓN DE REPOSITORIO"
    
    $repoPath = "src\SchoolSystem.Infrastructure\Persistence\Repositories"
    
    if (-not (Test-Path $repoPath)) {
        New-Item -ItemType Directory -Force -Path $repoPath | Out-Null
    }
    
    $repoContent = @"
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Domain.Entities.$Module;
using SchoolSystem.Domain.Interfaces;
using SchoolSystem.Infrastructure.Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSystem.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Implementación del repositorio de $EntityName
    /// </summary>
    public class ${EntityName}Repository : Repository<$EntityName>, I${EntityName}Repository
    {
        public ${EntityName}Repository(SchoolSystemDbContext context) : base(context)
        {
        }
        
        /// <summary>
        /// Obtiene todos los ${EntityName}s de una escuela
        /// </summary>
        public async Task<IEnumerable<$EntityName>> GetByEscuelaIdAsync(int escuelaId)
        {
            return await _context.Set<$EntityName>()
                .Where(x => x.EscuelaId == escuelaId)
                .ToListAsync();
        }
        
        // TODO: Implementar métodos específicos adicionales
        // Ejemplo:
        // public async Task<$EntityName> GetByNombreAsync(string nombre)
        // {
        //     return await _context.Set<$EntityName>()
        //         .FirstOrDefaultAsync(x => x.Nombre == nombre);
        // }
    }
}
"@

    $repoFile = "$repoPath\${EntityName}Repository.cs"
    $repoContent | Out-File -FilePath $repoFile -Encoding UTF8
    
    Write-Success "✓ Implementación de repositorio creada: $repoFile"
}

# Crear interfaz de servicio
function New-ServiceInterface {
    if ($SkipService) {
        Write-Info "⏭️  Omitiendo creación de Servicio"
        return
    }
    
    Write-Header "CREANDO INTERFAZ DE SERVICIO"
    
    $pluralName = Get-PluralName -Name $EntityName
    $interfacePath = "src\SchoolSystem.Application\Services\Interfaces"
    
    if (-not (Test-Path $interfacePath)) {
        New-Item -ItemType Directory -Force -Path $interfacePath | Out-Null
    }
    
    $interfaceContent = @"
using SchoolSystem.Application.DTOs.$pluralName;
using SchoolSystem.Application.DTOs.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Interfaces
{
    /// <summary>
    /// Interfaz de servicio para $EntityName
    /// </summary>
    public interface I${EntityName}Service
    {
        /// <summary>
        /// Obtiene un $EntityName por ID
        /// </summary>
        Task<ApiResponse<${EntityName}Dto>> GetByIdAsync(int id);
        
        /// <summary>
        /// Obtiene todos los ${EntityName}s de una escuela
        /// </summary>
        Task<ApiResponse<IEnumerable<${EntityName}Dto>>> GetAllByEscuelaAsync(int escuelaId);
        
        /// <summary>
        /// Obtiene ${EntityName}s paginados
        /// </summary>
        Task<ApiResponse<PagedResult<${EntityName}Dto>>> GetPagedAsync(int escuelaId, int pageNumber, int pageSize);
        
        /// <summary>
        /// Crea un nuevo $EntityName
        /// </summary>
        Task<ApiResponse<${EntityName}Dto>> CreateAsync(Create${EntityName}Dto dto, int escuelaId);
        
        /// <summary>
        /// Actualiza un $EntityName existente
        /// </summary>
        Task<ApiResponse<${EntityName}Dto>> UpdateAsync(int id, Update${EntityName}Dto dto);
        
        /// <summary>
        /// Elimina un $EntityName
        /// </summary>
        Task<ApiResponse<bool>> DeleteAsync(int id);
        
        // TODO: Agregar métodos adicionales según necesidades de negocio
    }
}
"@

    $interfaceFile = "$interfacePath\I${EntityName}Service.cs"
    $interfaceContent | Out-File -FilePath $interfaceFile -Encoding UTF8
    
    Write-Success "✓ Interfaz de servicio creada: $interfaceFile"
}

# Crear implementación de servicio
function New-ServiceImplementation {
    if ($SkipService) {
        return
    }
    
    Write-Header "CREANDO IMPLEMENTACIÓN DE SERVICIO"
    
    $pluralName = Get-PluralName -Name $EntityName
    $servicePath = "src\SchoolSystem.Application\Services\Implementations"
    
    if (-not (Test-Path $servicePath)) {
        New-Item -ItemType Directory -Force -Path $servicePath | Out-Null
    }
    
    $serviceContent = @"
using AutoMapper;
using SchoolSystem.Application.DTOs.$pluralName;
using SchoolSystem.Application.DTOs.Common;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.$Module;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio de $EntityName
    /// </summary>
    public class ${EntityName}Service : I${EntityName}Service
    {
        private readonly I${EntityName}Repository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public ${EntityName}Service(
            I${EntityName}Repository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public async Task<ApiResponse<${EntityName}Dto>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                
                if (entity == null)
                {
                    return ApiResponse<${EntityName}Dto>.ErrorResponse("$EntityName no encontrado");
                }
                
                var dto = _mapper.Map<${EntityName}Dto>(entity);
                return ApiResponse<${EntityName}Dto>.SuccessResponse(dto);
            }
            catch (Exception ex)
            {
                // Nota: Escapar el $ de la interpolación de C# con `
                return ApiResponse<${EntityName}Dto>.ErrorResponse(`$"Error al obtener $EntityName: {ex.Message}");
            }
        }
        
        public async Task<ApiResponse<IEnumerable<${EntityName}Dto>>> GetAllByEscuelaAsync(int escuelaId)
        {
            try
            {
                var entities = await _repository.GetByEscuelaIdAsync(escuelaId);
                var dtos = _mapper.Map<IEnumerable<${EntityName}Dto>>(entities);
                
                return ApiResponse<IEnumerable<${EntityName}Dto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<${EntityName}Dto>>.ErrorResponse(`$"Error al obtener ${EntityName}s: {ex.Message}");
            }
        }
        
        public async Task<ApiResponse<PagedResult<${EntityName}Dto>>> GetPagedAsync(int escuelaId, int pageNumber, int pageSize)
        {
            try
            {
                var entities = await _repository.GetByEscuelaIdAsync(escuelaId);
                var totalCount = entities.Count();
                
                var pagedEntities = entities
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                
                var dtos = _mapper.Map<IEnumerable<${EntityName}Dto>>(pagedEntities);
                
                var pagedResult = new PagedResult<${EntityName}Dto>
                {
                    Items = dtos,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                };
                
                return ApiResponse<PagedResult<${EntityName}Dto>>.SuccessResponse(pagedResult);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<${EntityName}Dto>>.ErrorResponse(`$"Error al obtener ${EntityName}s paginados: {ex.Message}");
            }
        }
        
        public async Task<ApiResponse<${EntityName}Dto>> CreateAsync(Create${EntityName}Dto dto, int escuelaId)
        {
            try
            {
                var entity = _mapper.Map<$EntityName>(dto);
                entity.EscuelaId = escuelaId;
                entity.CreatedAt = DateTime.Now;
                entity.UpdatedAt = DateTime.Now;
                
                await _repository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                
                var resultDto = _mapper.Map<${EntityName}Dto>(entity);
                return ApiResponse<${EntityName}Dto>.SuccessResponse(resultDto, "$EntityName creado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<${EntityName}Dto>.ErrorResponse(`$"Error al crear $EntityName: {ex.Message}");
            }
        }
        
        public async Task<ApiResponse<${EntityName}Dto>> UpdateAsync(int id, Update${EntityName}Dto dto)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                
                if (entity == null)
                {
                    return ApiResponse<${EntityName}Dto>.ErrorResponse("$EntityName no encontrado");
                }
                
                _mapper.Map(dto, entity);
                entity.UpdatedAt = DateTime.Now;
                
                _repository.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                
                var resultDto = _mapper.Map<${EntityName}Dto>(entity);
                return ApiResponse<${EntityName}Dto>.SuccessResponse(resultDto, "$EntityName actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<${EntityName}Dto>.ErrorResponse(`$"Error al actualizar $EntityName: {ex.Message}");
            }
        }
        
        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                
                if (entity == null)
                {
                    return ApiResponse<bool>.ErrorResponse("$EntityName no encontrado");
                }
                
                _repository.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
                
                return ApiResponse<bool>.SuccessResponse(true, "$EntityName eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse(`$"Error al eliminar $EntityName: {ex.Message}");
            }
        }
    }
}
"@

    $serviceFile = "$servicePath\${EntityName}Service.cs"
    $serviceContent | Out-File -FilePath $serviceFile -Encoding UTF8
    
    Write-Success "✓ Implementación de servicio creada: $serviceFile"
}

# Crear AutoMapper Profile
function New-MapperProfile {
    if ($SkipDTO -or $SkipService) {
        return
    }
    
    Write-Header "CREANDO AUTOMAPPER PROFILE"
    
    $pluralName = Get-PluralName -Name $EntityName
    $profilePath = "src\SchoolSystem.Application\Mappings"
    
    if (-not (Test-Path $profilePath)) {
        New-Item -ItemType Directory -Force -Path $profilePath | Out-Null
    }
    
    $profileContent = @"
using AutoMapper;
using SchoolSystem.Application.DTOs.$pluralName;
using SchoolSystem.Domain.Entities.$Module;

namespace SchoolSystem.Application.Mappings
{
    /// <summary>
    /// Profile de AutoMapper para $EntityName
    /// </summary>
    public class ${EntityName}Profile : Profile
    {
        public ${EntityName}Profile()
        {
            // Entity -> DTO
            CreateMap<$EntityName, ${EntityName}Dto>();
            
            // Create DTO -> Entity
            CreateMap<Create${EntityName}Dto, $EntityName>();
            
            // Update DTO -> Entity
            CreateMap<Update${EntityName}Dto, $EntityName>();
            
            // TODO: Agregar mapeos personalizados si es necesario
            // Ejemplo:
            // CreateMap<$EntityName, ${EntityName}Dto>()
            //     .ForMember(dest => dest.NombreCompleto, 
            //         opt => opt.MapFrom(src => `$"{src.Nombre} {src.Apellido}"`));
        }
    }
}
"@

    $profileFile = "$profilePath\${EntityName}Profile.cs"
    $profileContent | Out-File -FilePath $profileFile -Encoding UTF8
    
    Write-Success "✓ AutoMapper Profile creado: $profileFile"
}

# Crear Controller
function New-Controller {
    Write-Header "CREANDO CONTROLLER DE API"
    
    $pluralName = Get-PluralName -Name $EntityName
    $controllerPath = "src\SchoolSystem.API\Controllers\V1"
    
    if (-not (Test-Path $controllerPath)) {
        New-Item -ItemType Directory -Force -Path $controllerPath | Out-Null
    }
    
    $controllerContent = @"
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.DTOs.$pluralName;
using SchoolSystem.Application.Services.Interfaces;
using System.Threading.Tasks;

namespace SchoolSystem.API.Controllers.V1
{
    /// <summary>
    /// Controller para gestión de $pluralName
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class ${pluralName}Controller : ControllerBase
    {
        private readonly I${EntityName}Service _service;
        
        public ${pluralName}Controller(I${EntityName}Service service)
        {
            _service = service;
        }
        
        /// <summary>
        /// Obtiene un $EntityName por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            
            if (!result.Success)
                return NotFound(result);
            
            return Ok(result);
        }
        
        /// <summary>
        /// Obtiene todos los $pluralName de la escuela
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int escuelaId)
        {
            var result = await _service.GetAllByEscuelaAsync(escuelaId);
            return Ok(result);
        }
        
        /// <summary>
        /// Obtiene $pluralName paginados
        /// </summary>
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int escuelaId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetPagedAsync(escuelaId, pageNumber, pageSize);
            return Ok(result);
        }
        
        /// <summary>
        /// Crea un nuevo $EntityName
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Create${EntityName}Dto dto, [FromQuery] int escuelaId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _service.CreateAsync(dto, escuelaId);
            
            if (!result.Success)
                return BadRequest(result);
            
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }
        
        /// <summary>
        /// Actualiza un $EntityName existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Update${EntityName}Dto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _service.UpdateAsync(id, dto);
            
            if (!result.Success)
                return BadRequest(result);
            
            return Ok(result);
        }
        
        /// <summary>
        /// Elimina un $EntityName
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            
            if (!result.Success)
                return BadRequest(result);
            
            return Ok(result);
        }
    }
}
"@

    $controllerFile = "$controllerPath\${pluralName}Controller.cs"
    $controllerContent | Out-File -FilePath $controllerFile -Encoding UTF8
    
    Write-Success "✓ Controller creado: $controllerFile"
}

# Crear EF Core Configuration
function New-EntityConfiguration {
    Write-Header "CREANDO CONFIGURACIÓN DE EF CORE"
    
    $configPath = "src\SchoolSystem.Infrastructure\Persistence\Configurations"
    
    if (-not (Test-Path $configPath)) {
        New-Item -ItemType Directory -Force -Path $configPath | Out-Null
    }
    
    $configContent = @"
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.$Module;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para $EntityName
    /// </summary>
    public class ${EntityName}Configuration : IEntityTypeConfiguration<$EntityName>
    {
        public void Configure(EntityTypeBuilder<$EntityName> builder)
        {
            // Nombre de la tabla
            builder.ToTable("$(($EntityName).ToLower())s");
            
            // Clave primaria
            builder.HasKey(x => x.Id);
            
            // Propiedades
            builder.Property(x => x.EscuelaId)
                .IsRequired();
            
            // TODO: Configurar propiedades específicas
            // Ejemplo:
            // builder.Property(x => x.Nombre)
            //     .IsRequired()
            //     .HasMaxLength(200);
            
            // builder.Property(x => x.Descripcion)
            //     .HasMaxLength(500);
            
            // Propiedades de auditoría
            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            builder.Property(x => x.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
            
            // Índices
            builder.HasIndex(x => x.EscuelaId)
                .HasDatabaseName("idx_$(($EntityName).ToLower())_escuela_id");
            
            // TODO: Agregar más índices si es necesario
            // builder.HasIndex(x => x.Nombre);
            
            // Relaciones
            // TODO: Configurar relaciones con otras entidades
            // Ejemplo:
            // builder.HasOne(x => x.Escuela)
            //     .WithMany()
            //     .HasForeignKey(x => x.EscuelaId)
            //     .OnDelete(DeleteBehavior.Cascade);
            
            // Query Filter para Multi-tenancy
            // builder.HasQueryFilter(x => x.EscuelaId == CurrentTenantId);
        }
    }
}
"@

    $configFile = "$configPath\${EntityName}Configuration.cs"
    $configContent | Out-File -FilePath $configFile -Encoding UTF8
    
    Write-Success "✓ Configuración de EF Core creada: $configFile"
}

# Crear archivo de resumen
function New-GenerationSummary {
    $pluralName = Get-PluralName -Name $EntityName
    
    Write-Header "RESUMEN DE ARCHIVOS GENERADOS"
    
    Write-Success "`n✅ Entidad $EntityName generada exitosamente"
    Write-Info "`n📁 Archivos creados:"
    
    Write-Host "   Domain Layer:" -ForegroundColor Yellow
    Write-Host "   ✓ Entidad: Domain\Entities\$Module\$EntityName.cs"
    if (-not $SkipRepository) {
        Write-Host "   ✓ Interfaz Repositorio: Domain\Interfaces\I${EntityName}Repository.cs"
    }
    
    if (-not $SkipDTO) {
        Write-Host "`n   Application Layer:" -ForegroundColor Yellow
        Write-Host "   ✓ DTO Lectura: Application\DTOs\$pluralName\${EntityName}Dto.cs"
        Write-Host "   ✓ DTO Creación: Application\DTOs\$pluralName\Create${EntityName}Dto.cs"
        Write-Host "   ✓ DTO Actualización: Application\DTOs\$pluralName\Update${EntityName}Dto.cs"
    }
    
    if (-not $SkipService) {
        Write-Host "   ✓ Interfaz Servicio: Application\Services\Interfaces\I${EntityName}Service.cs"
        Write-Host "   ✓ Implementación Servicio: Application\Services\Implementations\${EntityName}Service.cs"
    }
    
    if (-not $SkipDTO -and -not $SkipService) {
        Write-Host "   ✓ AutoMapper Profile: Application\Mappings\${EntityName}Profile.cs"
    }
    
    if (-not $SkipRepository) {
        Write-Host "`n   Infrastructure Layer:" -ForegroundColor Yellow
        Write-Host "   ✓ Implementación Repositorio: Infrastructure\Repositories\${EntityName}Repository.cs"
        Write-Host "   ✓ Configuración EF Core: Infrastructure\Configurations\${EntityName}Configuration.cs"
    }
    
    Write-Host "`n   API Layer:" -ForegroundColor Yellow
    Write-Host "   ✓ Controller: API\Controllers\V1\${pluralName}Controller.cs"
    
    Write-Info "`n📝 Próximos pasos:"
    Write-Host "   1. ✏️  Completa las propiedades en la entidad $EntityName.cs"
    Write-Host "   2. ✏️  Agrega validaciones en los DTOs"
    Write-Host "   3. ✏️  Configura las relaciones en ${EntityName}Configuration.cs"
    Write-Host "   4. 🔧 Registra el repositorio y servicio en DependencyInjection"
    Write-Host "   5. 📦 Crea una migración: .\migrate-database.ps1"
    Write-Host "   6. 🧪 Prueba los endpoints en Swagger"
    
    Write-Warning "`n⚠️  Recuerda registrar las dependencias en Program.cs o Startup.cs:"
    Write-Info @"
    
    // En ConfigureServices
    services.AddScoped<I${EntityName}Repository, ${EntityName}Repository>();
    services.AddScoped<I${EntityName}Service, ${EntityName}Service>();
"@
}

# ============================================
# SCRIPT PRINCIPAL
# ============================================

Clear-Host
Write-Host "╔════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║                                                            ║" -ForegroundColor Cyan
Write-Host "║          GENERADOR DE ENTIDADES - SISTEMA ESCOLAR          ║" -ForegroundColor Cyan
Write-Host "║                                                            ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

# Verificar estructura del proyecto
if (-not (Test-ProjectStructure)) {
    Write-Host "`nPresiona cualquier tecla para salir..."
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    exit
}

# Obtener información de la entidad
if (-not (Get-EntityInformation)) {
    Write-Host "`nPresiona cualquier tecla para salir..."
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    exit
}

# Mostrar resumen de lo que se va a generar
Write-Header "CONFIGURACIÓN"
Write-Info "Entidad: $EntityName"
Write-Info "Módulo: $Module"
$repoStatus = if ($SkipRepository) { "Si" } else { "No" }
Write-Info "Omitir Repositorio: $repoStaturvice) { 'Si' } else { 'No' })"
Write-Info "Omitir DTOs: $(if ($SkipDTO) { 'Si' } else { 'No' })"

Write-Host "`n¿Deseas continuar? (S/N): " -NoNewline
$confirm = Read-Host

if ($confirm -ne "S" -and $confirm -ne "s") {
    Write-Info "Operación cancelada."
    exit
}

# Generar archivos
try {
    New-BaseEntity
    New-DomainEntity
    New-DTOs
    New-RepositoryInterface
    New-RepositoryImplementation
    New-ServiceInterface
    New-ServiceImplementation
    New-MapperProfile
    New-EntityConfiguration
    New-Controller
    New-GenerationSummary
    
    Write-Success "`n🎉 ¡Generación completada exitosamente!"
}
catch {
    Write-Error "`n❌ Error durante la generación: $($_.Exception.Message)"
    Write-Error $_.ScriptStackTrace
}

Write-Host "`nPresiona cualquier tecla para salir..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

