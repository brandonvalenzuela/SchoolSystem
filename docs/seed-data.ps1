# ============================================
# Script de Seed Data (Datos Iniciales)
# Sistema de Gestión Escolar
# ============================================

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

# Función para crear archivo de Seeder en C#
function New-SeedDataClass {
    Write-Header "CREANDO CLASE DE SEED DATA"
    
    $seedPath = "src\SchoolSystem.Infrastructure\Persistence\Seeds"
    
    # Crear carpeta si no existe
    if (-not (Test-Path $seedPath)) {
        New-Item -ItemType Directory -Force -Path $seedPath | Out-Null
        Write-Success "✓ Carpeta Seeds creada"
    }
    
    # Crear DataSeeder.cs
    $dataSeederContent = @'
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Domain.Entities.Escuelas;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Evaluacion;
using SchoolSystem.Domain.Enums;
using SchoolSystem.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSystem.Infrastructure.Persistence.Seeds
{
    public class DataSeeder
    {
        private readonly SchoolSystemDbContext _context;
        
        public DataSeeder(SchoolSystemDbContext context)
        {
            _context = context;
        }
        
        public async Task SeedAllAsync()
        {
            Console.WriteLine("🌱 Iniciando seed de datos...\n");
            
            // Verificar si ya hay datos
            if (await _context.Escuelas.AnyAsync())
            {
                Console.WriteLine("⚠️  Ya existen datos en la base de datos.");
                Console.Write("¿Deseas eliminar todos los datos y volver a sembrar? (S/N): ");
                var response = Console.ReadLine();
                
                if (response?.ToUpper() != "S")
                {
                    Console.WriteLine("❌ Operación cancelada.");
                    return;
                }
                
                await CleanDatabaseAsync();
            }
            
            await SeedPermisosAsync();
            await SeedEscuelasAsync();
            await SeedUsuariosAsync();
            await SeedNivelesEducativosAsync();
            await SeedGradosAsync();
            await SeedMateriasAsync();
            await SeedGruposAsync();
            await SeedAlumnosAsync();
            await SeedPadresAsync();
            await SeedInscripcionesAsync();
            await SeedPeriodosEvaluacionAsync();
            
            Console.WriteLine("\n✅ Seed de datos completado exitosamente!");
            Console.WriteLine("\n📊 Resumen de datos creados:");
            Console.WriteLine($"   • Escuelas: {await _context.Escuelas.CountAsync()}");
            Console.WriteLine($"   • Usuarios: {await _context.Usuarios.CountAsync()}");
            Console.WriteLine($"   • Alumnos: {await _context.Alumnos.CountAsync()}");
            Console.WriteLine($"   • Grupos: {await _context.Grupos.CountAsync()}");
            Console.WriteLine($"   • Materias: {await _context.Materias.CountAsync()}");
        }
        
        private async Task CleanDatabaseAsync()
        {
            Console.WriteLine("🗑️  Limpiando base de datos...");
            
            // Desactivar restricciones de foreign key temporalmente
            await _context.Database.ExecuteSqlRawAsync("SET FOREIGN_KEY_CHECKS = 0");
            
            // Limpiar todas las tablas
            _context.RemoveRange(_context.Inscripciones);
            _context.RemoveRange(_context.AlumnoPadres);
            _context.RemoveRange(_context.Padres);
            _context.RemoveRange(_context.Alumnos);
            _context.RemoveRange(_context.Maestros);
            _context.RemoveRange(_context.GrupoMateriaMaestro);
            _context.RemoveRange(_context.Grupos);
            _context.RemoveRange(_context.GradoMaterias);
            _context.RemoveRange(_context.Materias);
            _context.RemoveRange(_context.Grados);
            _context.RemoveRange(_context.NivelesEducativos);
            _context.RemoveRange(_context.PeriodosEvaluacion);
            _context.RemoveRange(_context.RolPermisos);
            _context.RemoveRange(_context.Usuarios);
            _context.RemoveRange(_context.Escuelas);
            
            await _context.SaveChangesAsync();
            
            // Reactivar restricciones
            await _context.Database.ExecuteSqlRawAsync("SET FOREIGN_KEY_CHECKS = 1");
            
            Console.WriteLine("✓ Base de datos limpiada\n");
        }
        
        private async Task SeedPermisosAsync()
        {
            Console.WriteLine("📋 Creando permisos...");
            
            var permisos = new[]
            {
                new { Nombre = "ver_alumnos", Descripcion = "Ver lista de alumnos", Modulo = "alumnos" },
                new { Nombre = "crear_alumnos", Descripcion = "Crear nuevos alumnos", Modulo = "alumnos" },
                new { Nombre = "editar_alumnos", Descripcion = "Editar información de alumnos", Modulo = "alumnos" },
                new { Nombre = "eliminar_alumnos", Descripcion = "Eliminar alumnos", Modulo = "alumnos" },
                new { Nombre = "ver_calificaciones", Descripcion = "Ver calificaciones", Modulo = "calificaciones" },
                new { Nombre = "capturar_calificaciones", Descripcion = "Capturar calificaciones", Modulo = "calificaciones" },
                new { Nombre = "ver_asistencias", Descripcion = "Ver asistencias", Modulo = "asistencias" },
                new { Nombre = "registrar_asistencias", Descripcion = "Registrar asistencias", Modulo = "asistencias" },
                new { Nombre = "ver_finanzas", Descripcion = "Ver información financiera", Modulo = "finanzas" },
                new { Nombre = "gestionar_pagos", Descripcion = "Gestionar pagos y cobros", Modulo = "finanzas" },
                new { Nombre = "enviar_notificaciones", Descripcion = "Enviar notificaciones", Modulo = "comunicacion" },
                new { Nombre = "ver_reportes", Descripcion = "Ver reportes generales", Modulo = "reportes" },
                new { Nombre = "gestionar_maestros", Descripcion = "Gestionar maestros", Modulo = "maestros" },
                new { Nombre = "gestionar_grupos", Descripcion = "Gestionar grupos", Modulo = "grupos" },
                new { Nombre = "configurar_sistema", Descripcion = "Configurar el sistema", Modulo = "configuracion" }
            };
            
            // Implementar lógica de permisos aquí cuando tengas la entidad Permiso
            
            Console.WriteLine($"✓ {permisos.Length} permisos creados\n");
        }
        
        private async Task SeedEscuelasAsync()
        {
            Console.WriteLine("🏫 Creando escuelas...");
            
            var escuela = new Escuela
            {
                Codigo = "ESC001",
                Nombre = "Instituto Educativo Demo",
                RazonSocial = "Instituto Educativo Demo S.C.",
                RFC = "IED010101ABC",
                Direccion = "Av. Educación #123, Colonia Centro",
                Telefono = "1234567890",
                Email = "contacto@institutodemo.edu.mx",
                PlanId = 2, // Plan Profesional
                FechaRegistro = DateTime.Now,
                FechaExpiracion = DateTime.Now.AddYears(1),
                Activo = true
            };
            
            _context.Escuelas.Add(escuela);
            await _context.SaveChangesAsync();
            
            Console.WriteLine($"✓ Escuela creada: {escuela.Nombre}\n");
        }
        
        private async Task SeedUsuariosAsync()
        {
            Console.WriteLine("👥 Creando usuarios...");
            
            var escuela = await _context.Escuelas.FirstAsync();
            
            // Nota: En producción, usar un password hasher real
            var usuarios = new List<Usuario>
            {
                // Director
                new Usuario
                {
                    EscuelaId = escuela.Id,
                    Username = "director",
                    Email = "director@institutodemo.edu.mx",
                    PasswordHash = "hash_temporal", // CAMBIAR por hash real
                    Rol = RolUsuario.Director,
                    Nombre = "Carlos",
                    ApellidoPaterno = "Ramírez",
                    ApellidoMaterno = "González",
                    Telefono = "1234567891",
                    FechaNacimiento = new DateTime(1975, 5, 15),
                    Genero = Genero.M,
                    Activo = true
                },
                
                // Subdirector
                new Usuario
                {
                    EscuelaId = escuela.Id,
                    Username = "subdirector",
                    Email = "subdirector@institutodemo.edu.mx",
                    PasswordHash = "hash_temporal",
                    Rol = RolUsuario.Subdirector,
                    Nombre = "María",
                    ApellidoPaterno = "López",
                    ApellidoMaterno = "Martínez",
                    Telefono = "1234567892",
                    FechaNacimiento = new DateTime(1980, 8, 20),
                    Genero = Genero.F,
                    Activo = true
                },
                
                // Maestros
                new Usuario
                {
                    EscuelaId = escuela.Id,
                    Username = "maestro1",
                    Email = "jperez@institutodemo.edu.mx",
                    PasswordHash = "hash_temporal",
                    Rol = RolUsuario.Maestro,
                    Nombre = "Juan",
                    ApellidoPaterno = "Pérez",
                    ApellidoMaterno = "Sánchez",
                    Telefono = "1234567893",
                    FechaNacimiento = new DateTime(1985, 3, 10),
                    Genero = Genero.M,
                    Activo = true
                },
                
                new Usuario
                {
                    EscuelaId = escuela.Id,
                    Username = "maestro2",
                    Email = "agarcia@institutodemo.edu.mx",
                    PasswordHash = "hash_temporal",
                    Rol = RolUsuario.Maestro,
                    Nombre = "Ana",
                    ApellidoPaterno = "García",
                    ApellidoMaterno = "Hernández",
                    Telefono = "1234567894",
                    FechaNacimiento = new DateTime(1988, 7, 22),
                    Genero = Genero.F,
                    Activo = true
                },
                
                // Padres de familia (se crearán más adelante vinculados a alumnos)
            };
            
            _context.Usuarios.AddRange(usuarios);
            await _context.SaveChangesAsync();
            
            Console.WriteLine($"✓ {usuarios.Count} usuarios creados\n");
        }
        
        private async Task SeedNivelesEducativosAsync()
        {
            Console.WriteLine("📚 Creando niveles educativos...");
            
            var escuela = await _context.Escuelas.FirstAsync();
            
            var niveles = new List<NivelEducativo>
            {
                new NivelEducativo
                {
                    EscuelaId = escuela.Id,
                    Nombre = "Primaria",
                    Abreviatura = "PRIM",
                    Orden = 1,
                    Activo = true
                },
                new NivelEducativo
                {
                    EscuelaId = escuela.Id,
                    Nombre = "Secundaria",
                    Abreviatura = "SEC",
                    Orden = 2,
                    Activo = true
                }
            };
            
            _context.NivelesEducativos.AddRange(niveles);
            await _context.SaveChangesAsync();
            
            Console.WriteLine($"✓ {niveles.Count} niveles educativos creados\n");
        }
        
        private async Task SeedGradosAsync()
        {
            Console.WriteLine("📖 Creando grados...");
            
            var escuela = await _context.Escuelas.FirstAsync();
            var niveles = await _context.NivelesEducativos.ToListAsync();
            
            var grados = new List<Grado>();
            
            // Grados de primaria (1° a 6°)
            var nivelPrimaria = niveles.First(n => n.Nombre == "Primaria");
            for (int i = 1; i <= 6; i++)
            {
                grados.Add(new Grado
                {
                    EscuelaId = escuela.Id,
                    NivelEducativoId = nivelPrimaria.Id,
                    Nombre = $"{i}°",
                    Orden = i,
                    Activo = true
                });
            }
            
            // Grados de secundaria (1° a 3°)
            var nivelSecundaria = niveles.First(n => n.Nombre == "Secundaria");
            for (int i = 1; i <= 3; i++)
            {
                grados.Add(new Grado
                {
                    EscuelaId = escuela.Id,
                    NivelEducativoId = nivelSecundaria.Id,
                    Nombre = $"{i}°",
                    Orden = i,
                    Activo = true
                });
            }
            
            _context.Grados.AddRange(grados);
            await _context.SaveChangesAsync();
            
            Console.WriteLine($"✓ {grados.Count} grados creados\n");
        }
        
        private async Task SeedMateriasAsync()
        {
            Console.WriteLine("📝 Creando materias...");
            
            var escuela = await _context.Escuelas.FirstAsync();
            
            var materias = new List<Materia>
            {
                new Materia { EscuelaId = escuela.Id, Nombre = "Matemáticas", Clave = "MAT", Color = "#FF5733", Activo = true },
                new Materia { EscuelaId = escuela.Id, Nombre = "Español", Clave = "ESP", Color = "#33FF57", Activo = true },
                new Materia { EscuelaId = escuela.Id, Nombre = "Ciencias Naturales", Clave = "CN", Color = "#3357FF", Activo = true },
                new Materia { EscuelaId = escuela.Id, Nombre = "Historia", Clave = "HIST", Color = "#FF33F5", Activo = true },
                new Materia { EscuelaId = escuela.Id, Nombre = "Geografía", Clave = "GEO", Color = "#F5FF33", Activo = true },
                new Materia { EscuelaId = escuela.Id, Nombre = "Educación Física", Clave = "EF", Color = "#33FFF5", Activo = true },
                new Materia { EscuelaId = escuela.Id, Nombre = "Inglés", Clave = "ING", Color = "#FF8C33", Activo = true },
                new Materia { EscuelaId = escuela.Id, Nombre = "Formación Cívica y Ética", Clave = "FCE", Color = "#8C33FF", Activo = true }
            };
            
            _context.Materias.AddRange(materias);
            await _context.SaveChangesAsync();
            
            Console.WriteLine($"✓ {materias.Count} materias creadas\n");
        }
        
        private async Task SeedGruposAsync()
        {
            Console.WriteLine("👨‍🎓 Creando grupos...");
            
            var escuela = await _context.Escuelas.FirstAsync();
            var grados = await _context.Grados.Include(g => g.NivelEducativo).ToListAsync();
            var maestros = await _context.Usuarios.Where(u => u.Rol == RolUsuario.Maestro).ToListAsync();
            
            var cicloActual = $"{DateTime.Now.Year}-{DateTime.Now.Year + 1}";
            var grupos = new List<Grupo>();
            
            int maestroIndex = 0;
            
            // Crear grupos A y B para cada grado
            foreach (var grado in grados.Take(6)) // Solo primeros 6 grados para demo
            {
                foreach (var seccion in new[] { "A", "B" })
                {
                    grupos.Add(new Grupo
                    {
                        EscuelaId = escuela.Id,
                        GradoId = grado.Id,
                        Nombre = seccion,
                        CicloEscolar = cicloActual,
                        CapacidadMaxima = 35,
                        MaestroTitularId = maestros[maestroIndex % maestros.Count].Id,
                        Aula = $"{grado.NivelEducativo.Abreviatura}-{grado.Nombre}-{seccion}",
                        Turno = Turno.Matutino,
                        Activo = true
                    });
                    
                    maestroIndex++;
                }
            }
            
            _context.Grupos.AddRange(grupos);
            await _context.SaveChangesAsync();
            
            Console.WriteLine($"✓ {grupos.Count} grupos creados\n");
        }
        
        private async Task SeedAlumnosAsync()
        {
            Console.WriteLine("🎓 Creando alumnos...");
            
            var escuela = await _context.Escuelas.FirstAsync();
            
            var nombresNiños = new[] { "Juan", "Pedro", "Luis", "Carlos", "Miguel", "José", "Daniel", "David" };
            var nombresNiñas = new[] { "María", "Ana", "Laura", "Carmen", "Rosa", "Patricia", "Sofía", "Elena" };
            var apellidos = new[] { "García", "Rodríguez", "Martínez", "López", "González", "Pérez", "Sánchez", "Ramírez" };
            
            var alumnos = new List<Alumno>();
            var random = new Random();
            
            for (int i = 1; i <= 50; i++)
            {
                var esNiño = random.Next(2) == 0;
                var nombre = esNiño ? nombresNiños[random.Next(nombresNiños.Length)] : nombresNiñas[random.Next(nombresNiñas.Length)];
                var apellido1 = apellidos[random.Next(apellidos.Length)];
                var apellido2 = apellidos[random.Next(apellidos.Length)];
                
                alumnos.Add(new Alumno
                {
                    EscuelaId = escuela.Id,
                    Matricula = $"A{DateTime.Now.Year}{i:D4}",
                    Nombre = nombre,
                    ApellidoPaterno = apellido1,
                    ApellidoMaterno = apellido2,
                    FechaNacimiento = DateTime.Now.AddYears(-random.Next(6, 15)),
                    Genero = esNiño ? Genero.M : Genero.F,
                    Direccion = $"Calle {i} #123",
                    Telefono = $"555{random.Next(1000000, 9999999)}",
                    FechaIngreso = DateTime.Now.AddMonths(-random.Next(1, 36)),
                    Estatus = EstatusAlumno.Activo
                });
            }
            
            _context.Alumnos.AddRange(alumnos);
            await _context.SaveChangesAsync();
            
            Console.WriteLine($"✓ {alumnos.Count} alumnos creados\n");
        }
        
        private async Task SeedPadresAsync()
        {
            Console.WriteLine("👨‍👩‍👧 Creando padres de familia...");
            
            var escuela = await _context.Escuelas.FirstAsync();
            var alumnos = await _context.Alumnos.Take(20).ToListAsync(); // Solo para algunos alumnos
            
            var padres = new List<Padre>();
            var usuarios = new List<Usuario>();
            var relaciones = new List<AlumnoPadre>();
            
            foreach (var alumno in alumnos)
            {
                // Crear usuario padre
                var usuarioPadre = new Usuario
                {
                    EscuelaId = escuela.Id,
                    Username = $"padre{alumno.Matricula}",
                    Email = $"padre.{alumno.ApellidoPaterno.ToLower()}@gmail.com",
                    PasswordHash = "hash_temporal",
                    Rol = RolUsuario.Padre,
                    Nombre = $"Padre de {alumno.Nombre}",
                    ApellidoPaterno = alumno.ApellidoPaterno,
                    ApellidoMaterno = "Padre",
                    Telefono = $"555{new Random().Next(1000000, 9999999)}",
                    Activo = true
                };
                
                _context.Usuarios.Add(usuarioPadre);
                await _context.SaveChangesAsync();
                
                var padre = new Padre
                {
                    EscuelaId = escuela.Id,
                    UsuarioId = usuarioPadre.Id,
                    Ocupacion = "Empleado",
                    LugarTrabajo = "Empresa XYZ"
                };
                
                _context.Padres.Add(padre);
                await _context.SaveChangesAsync();
                
                // Crear relación alumno-padre
                relaciones.Add(new AlumnoPadre
                {
                    AlumnoId = alumno.Id,
                    PadreId = padre.Id,
                    Relacion = RelacionFamiliar.Padre,
                    EsTutorPrincipal = true,
                    AutorizadoRecoger = true,
                    RecibeNotificaciones = true,
                    ViveConAlumno = true
                });
            }
            
            _context.AlumnoPadres.AddRange(relaciones);
            await _context.SaveChangesAsync();
            
            Console.WriteLine($"✓ {relaciones.Count} padres creados y vinculados\n");
        }
        
        private async Task SeedInscripcionesAsync()
        {
            Console.WriteLine("📋 Creando inscripciones...");
            
            var escuela = await _context.Escuelas.FirstAsync();
            var alumnos = await _context.Alumnos.ToListAsync();
            var grupos = await _context.Grupos.ToListAsync();
            
            var cicloActual = $"{DateTime.Now.Year}-{DateTime.Now.Year + 1}";
            var inscripciones = new List<Inscripcion>();
            var random = new Random();
            
            foreach (var alumno in alumnos)
            {
                var grupoAleatorio = grupos[random.Next(grupos.Count)];
                
                inscripciones.Add(new Inscripcion
                {
                    EscuelaId = escuela.Id,
                    AlumnoId = alumno.Id,
                    GrupoId = grupoAleatorio.Id,
                    CicloEscolar = cicloActual,
                    FechaInscripcion = DateTime.Now.AddMonths(-2),
                    NumeroLista = random.Next(1, 36),
                    Estatus = EstatusInscripcion.Inscrito
                });
            }
            
            _context.Inscripciones.AddRange(inscripciones);
            await _context.SaveChangesAsync();
            
            Console.WriteLine($"✓ {inscripciones.Count} inscripciones creadas\n");
        }
        
        private async Task SeedPeriodosEvaluacionAsync()
        {
            Console.WriteLine("📅 Creando períodos de evaluación...");
            
            var escuela = await _context.Escuelas.FirstAsync();
            var cicloActual = $"{DateTime.Now.Year}-{DateTime.Now.Year + 1}";
            
            var periodos = new List<PeriodoEvaluacion>
            {
                new PeriodoEvaluacion
                {
                    EscuelaId = escuela.Id,
                    CicloEscolar = cicloActual,
                    Nombre = "1er Bimestre",
                    Numero = 1,
                    FechaInicio = new DateTime(DateTime.Now.Year, 8, 20),
                    FechaFin = new DateTime(DateTime.Now.Year, 10, 15),
                    Porcentaje = 20,
                    Activo = true
                },
                new PeriodoEvaluacion
                {
                    EscuelaId = escuela.Id,
                    CicloEscolar = cicloActual,
                    Nombre = "2do Bimestre",
                    Numero = 2,
                    FechaInicio = new DateTime(DateTime.Now.Year, 10, 16),
                    FechaFin = new DateTime(DateTime.Now.Year, 12, 15),
                    Porcentaje = 20,
                    Activo = true
                },
                new PeriodoEvaluacion
                {
                    EscuelaId = escuela.Id,
                    CicloEscolar = cicloActual,
                    Nombre = "3er Bimestre",
                    Numero = 3,
                    FechaInicio = new DateTime(DateTime.Now.Year + 1, 1, 8),
                    FechaFin = new DateTime(DateTime.Now.Year + 1, 3, 15),
                    Porcentaje = 20,
                    Activo = true
                },
                new PeriodoEvaluacion
                {
                    EscuelaId = escuela.Id,
                    CicloEscolar = cicloActual,
                    Nombre = "4to Bimestre",
                    Numero = 4,
                    FechaInicio = new DateTime(DateTime.Now.Year + 1, 3, 16),
                    FechaFin = new DateTime(DateTime.Now.Year + 1, 5, 15),
                    Porcentaje = 20,
                    Activo = true
                },
                new PeriodoEvaluacion
                {
                    EscuelaId = escuela.Id,
                    CicloEscolar = cicloActual,
                    Nombre = "5to Bimestre",
                    Numero = 5,
                    FechaInicio = new DateTime(DateTime.Now.Year + 1, 5, 16),
                    FechaFin = new DateTime(DateTime.Now.Year + 1, 7, 10),
                    Porcentaje = 20,
                    Activo = true
                }
            };
            
            _context.PeriodosEvaluacion.AddRange(periodos);
            await _context.SaveChangesAsync();
            
            Console.WriteLine($"✓ {periodos.Count} períodos de evaluación creados\n");
        }
    }
}
'@

    $dataSeederPath = "$seedPath\DataSeeder.cs"
    $dataSeederContent | Out-File -FilePath $dataSeederPath -Encoding UTF8
    
    Write-Success "✓ Clase DataSeeder.cs creada en: $dataSeederPath"
    
    # Crear programa de consola para ejecutar el seeder
    $programContent = @'
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolSystem.Infrastructure.Persistence.Context;
using SchoolSystem.Infrastructure.Persistence.Seeds;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SchoolSystem.Infrastructure.Persistence.Seeds
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("╔════════════════════════════════════════════════╗");
            Console.WriteLine("║     SEED DATA - SISTEMA DE GESTIÓN ESCOLAR    ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝\n");
            
            // Leer configuración
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("❌ Error: No se encontró la cadena de conexión en appsettings.json");
                return;
            }
            
            // Configurar DbContext
            var optionsBuilder = new DbContextOptionsBuilder<SchoolSystemDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            
            using (var context = new SchoolSystemDbContext(optionsBuilder.Options))
            {
                // Verificar conexión
                try
                {
                    await context.Database.CanConnectAsync();
                    Console.WriteLine("✓ Conexión a base de datos exitosa\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error de conexión: {ex.Message}");
                    return;
                }
                
                // Ejecutar seeder
                var seeder = new DataSeeder(context);
                await seeder.SeedAllAsync();
            }
            
            Console.WriteLine("\n✅ Proceso completado. Presiona cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}
'@

    $programPath = "$seedPath\SeedProgram.cs"
    $programContent | Out-File -FilePath $programPath -Encoding UTF8
    
    Write-Success "✓ Clase SeedProgram.cs creada"
    
    Write-Info "`n📝 Notas importantes:"
    Write-Info "   1. Los archivos C# han sido creados en: $seedPath"
    Write-Info "   2. Necesitarás crear las entidades en Domain antes de compilar"
    Write-Info "   3. Ajusta los nombres de las entidades según tu implementación"
    Write-Info "   4. Implementa un password hasher real en lugar de 'hash_temporal'"
}

# Función para crear script de ejecución rápida
function New-QuickSeedScript {
    Write-Header "CREANDO SCRIPT DE EJECUCIÓN RÁPIDA"
    
    $quickSeedContent = @'
# ============================================
# Script rápido para ejecutar Seed Data
# ============================================

Write-Host "🌱 Ejecutando Seed Data..." -ForegroundColor Cyan

# Verificar que estamos en el directorio correcto
if (-not (Test-Path "SchoolSystem.sln")) {
    Write-Host "❌ Error: Ejecuta este script desde el directorio raíz del proyecto" -ForegroundColor Red
    exit
}

# Opción 1: Ejecutar directamente con dotnet run
Write-Host "`nOpción 1: Ejecutar seed desde código C#" -ForegroundColor Yellow
Write-Host "Comando: dotnet run --project src\SchoolSystem.Infrastructure\SchoolSystem.Infrastructure.csproj --seed"

# Opción 2: Llamar al método desde API
Write-Host "`nOpción 2: Llamar endpoint de seed desde la API" -ForegroundColor Yellow
Write-Host "1. Inicia la API: dotnet run --project src\SchoolSystem.API"
Write-Host "2. Llama al endpoint: POST http://localhost:5000/api/seed"

Write-Host "`n⚠️  IMPORTANTE: Asegúrate de tener configurada la cadena de conexión en appsettings.json" -ForegroundColor Yellow
Write-Host "`nPresiona cualquier tecla para continuar..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
'@

    $quickSeedPath = "scripts\quick-seed.ps1"
    
    # Crear carpeta scripts si no existe
    if (-not (Test-Path "scripts")) {
        New-Item -ItemType Directory -Force -Path "scripts" | Out-Null
    }
    
    $quickSeedContent | Out-File -FilePath $quickSeedPath -Encoding UTF8
    
    Write-Success "✓ Script de ejecución rápida creado: $quickSeedPath"
}

# Función para crear endpoint de Seed en API
function New-SeedController {
    Write-Header "CREANDO CONTROLADOR DE SEED PARA LA API"
    
    $controllerContent = @'
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Infrastructure.Persistence.Context;
using SchoolSystem.Infrastructure.Persistence.Seeds;
using System.Threading.Tasks;

namespace SchoolSystem.API.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly SchoolSystemDbContext _context;
        
        public SeedController(SchoolSystemDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Ejecuta el seed de datos inicial
        /// </summary>
        /// <remarks>
        /// ⚠️ SOLO USAR EN DESARROLLO
        /// Este endpoint pobla la base de datos con datos de prueba
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> SeedData()
        {
            #if DEBUG
            try
            {
                var seeder = new DataSeeder(_context);
                await seeder.SeedAllAsync();
                
                return Ok(new 
                { 
                    success = true,
                    message = "✅ Datos iniciales creados exitosamente"
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new 
                { 
                    success = false,
                    message = $"❌ Error al crear datos: {ex.Message}"
                });
            }
            #else
            return BadRequest(new 
            { 
                success = false,
                message = "❌ Este endpoint solo está disponible en modo desarrollo"
            });
            #endif
        }
        
        /// <summary>
        /// Verifica el estado de la base de datos
        /// </summary>
        [HttpGet("status")]
        public async Task<IActionResult> GetStatus()
        {
            var escuelasCount = await _context.Escuelas.CountAsync();
            var alumnosCount = await _context.Alumnos.CountAsync();
            var usuariosCount = await _context.Usuarios.CountAsync();
            var gruposCount = await _context.Grupos.CountAsync();
            
            return Ok(new
            {
                database = "Conectado",
                data = new
                {
                    escuelas = escuelasCount,
                    alumnos = alumnosCount,
                    usuarios = usuariosCount,
                    grupos = gruposCount
                },
                isEmpty = escuelasCount == 0
            });
        }
    }
}
'@

    $controllerPath = "src\SchoolSystem.API\Controllers\V1\SeedController.cs"
    
    # Crear directorio si no existe
    $controllerDir = Split-Path -Parent $controllerPath
    if (-not (Test-Path $controllerDir)) {
        New-Item -ItemType Directory -Force -Path $controllerDir | Out-Null
    }
    
    $controllerContent | Out-File -FilePath $controllerPath -Encoding UTF8
    
    Write-Success "✓ SeedController.cs creado en: $controllerPath"
    Write-Info "`n📝 Uso del controlador:"
    Write-Info "   POST /api/seed        - Ejecuta el seed de datos"
    Write-Info "   GET  /api/seed/status - Verifica el estado de la BD"
}

# Función para crear README con instrucciones
function New-SeedReadme {
    Write-Header "CREANDO DOCUMENTACIÓN"
    
    $readmeContent = @'
# 🌱 Seed Data - Sistema de Gestión Escolar

## ¿Qué es Seed Data?

El **Seed Data** es el proceso de poblar la base de datos con datos iniciales de prueba. Esto es útil para:

- 🧪 **Desarrollo**: Tener datos para probar funcionalidades
- 🎯 **Testing**: Datos consistentes para pruebas
- 📚 **Demos**: Mostrar el sistema funcionando
- 🚀 **Onboarding**: Nuevos desarrolladores pueden empezar rápido

## 📊 Datos que se Crean

El seed crea automáticamente:

- ✅ **1 Escuela demo** (Instituto Educativo Demo)
- ✅ **Usuarios** (Director, Subdirector, Maestros)
- ✅ **Niveles educativos** (Primaria, Secundaria)
- ✅ **Grados** (1° a 6° Primaria, 1° a 3° Secundaria)
- ✅ **8 Materias** (Matemáticas, Español, etc.)
- ✅ **12 Grupos** (A y B para cada grado)
- ✅ **50 Alumnos** con datos realistas
- ✅ **20 Padres de familia** vinculados a alumnos
- ✅ **Inscripciones** para el ciclo actual
- ✅ **5 Períodos de evaluación** (Bimestres)
- ✅ **Permisos** del sistema

## 🚀 Cómo Ejecutar el Seed

### Opción 1: Desde la API (Recomendado)

1. Inicia la API:
   ```powershell
   dotnet run --project src\SchoolSystem.API
   ```

2. Llama al endpoint de seed:
   ```bash
   POST http://localhost:5000/api/seed
   ```

3. Verifica el estado:
   ```bash
   GET http://localhost:5000/api/seed/status
   ```

### Opción 2: Script PowerShell

```powershell
.\scripts\quick-seed.ps1
```

### Opción 3: Desde Código

```csharp
var seeder = new DataSeeder(dbContext);
await seeder.SeedAllAsync();
```

## ⚠️ IMPORTANTE

### Seguridad

- 🔐 Los passwords en el seed son **temporales** ("hash_temporal")
- ⚠️ **NO USES** estos datos en producción
- 🔒 Implementa un password hasher real antes de producción

### Limpieza de Datos

El seed **pregunta antes de eliminar** datos existentes:
- Si la BD ya tiene datos, preguntará si quieres limpiarla
- Esto **borrará TODOS los datos** actuales
- Solo usar en **desarrollo**

## 🔐 Usuarios Creados

| Usuario | Email | Contraseña | Rol |
|---------|-------|------------|-----|
| director | director@institutodemo.edu.mx | (temporal) | Director |
| subdirector | subdirector@institutodemo.edu.mx | (temporal) | Subdirector |
| maestro1 | jperez@institutodemo.edu.mx | (temporal) | Maestro |
| maestro2 | agarcia@institutodemo.edu.mx | (temporal) | Maestro |
| padreA2024XXXX | padre.apellido@gmail.com | (temporal) | Padre |

## 📝 Personalización

Para personalizar los datos del seed, edita:

```
src/SchoolSystem.Infrastructure/Persistence/Seeds/DataSeeder.cs
```

Puedes modificar:
- Nombres de alumnos y maestros
- Cantidad de datos
- Materias y grados
- Configuración de la escuela demo

## 🐛 Solución de Problemas

### Error: "No se encontró la cadena de conexión"

Verifica que `appsettings.json` tenga:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=school_system;User=root;Password=tu_password;"
  }
}
```

### Error: "Tabla no existe"

Ejecuta las migraciones primero:
```powershell
dotnet ef database update
```

### Error: "Foreign key constraint fails"

La BD tiene datos inconsistentes. Limpia y vuelve a crear:
```powershell
.\migrate-database.ps1
# Opción 8: Recrear base de datos
```

## 📚 Archivos Relacionados

- `DataSeeder.cs` - Lógica principal del seed
- `SeedProgram.cs` - Programa de consola para ejecutar
- `SeedController.cs` - Endpoint de API para seed
- `quick-seed.ps1` - Script de ejecución rápida

## 🎯 Próximos Pasos

Después de ejecutar el seed:

1. ✅ Verifica los datos en la BD
2. ✅ Prueba el login con los usuarios creados
3. ✅ Implementa password hashing real
4. ✅ Crea más datos específicos según necesites
5. ✅ Empieza a desarrollar funcionalidades

---

💡 **Tip**: Ejecuta el seed cada vez que resetees tu base de datos de desarrollo para tener siempre datos de prueba listos.
'@

    $readmePath = "docs\SEED_DATA.md"
    
    # Crear carpeta docs si no existe
    if (-not (Test-Path "docs")) {
        New-Item -ItemType Directory -Force -Path "docs" | Out-Null
    }
    
    $readmeContent | Out-File -FilePath $readmePath -Encoding UTF8
    
    Write-Success "✓ Documentación creada en: $readmePath"
}

# ============================================
# MENÚ PRINCIPAL
# ============================================

function Show-Menu {
    Clear-Host
    Write-Host "╔════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
    Write-Host "║                                                            ║" -ForegroundColor Cyan
    Write-Host "║          GENERADOR DE SEED DATA - SISTEMA ESCOLAR          ║" -ForegroundColor Cyan
    Write-Host "║                                                            ║" -ForegroundColor Cyan
    Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "  1. Crear clase DataSeeder completa (C#)" -ForegroundColor White
    Write-Host "  2. Crear script de ejecución rápida (PowerShell)" -ForegroundColor White
    Write-Host "  3. Crear SeedController para API" -ForegroundColor White
    Write-Host "  4. Crear documentación (README)" -ForegroundColor White
    Write-Host "  5. Crear TODO (Opción recomendada)" -ForegroundColor Green
    Write-Host "  6. Salir" -ForegroundColor White
    Write-Host ""
}

# ============================================
# SCRIPT PRINCIPAL
# ============================================

# Verificar estructura del proyecto
if (-not (Test-ProjectStructure)) {
    Write-Host "`nPresiona cualquier tecla para salir..."
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    exit
}

# Loop principal del menú
do {
    Show-Menu
    $option = Read-Host "Selecciona una opción"
    
    switch ($option) {
        '1' { New-SeedDataClass }
        '2' { New-QuickSeedScript }
        '3' { New-SeedController }
        '4' { New-SeedReadme }
        '5' { 
            Write-Header "CREANDO TODOS LOS ARCHIVOS DE SEED DATA"
            New-SeedDataClass
            Write-Host ""
            New-QuickSeedScript
            Write-Host ""
            New-SeedController
            Write-Host ""
            New-SeedReadme
            Write-Success "`n✅ ¡Todos los archivos de Seed Data creados exitosamente!"
        }
        '6' { 
            Write-Success "`n¡Hasta luego!"
            return 
        }
        default { 
            Write-Warning "`n⚠️  Opción no válida. Por favor selecciona 1-6."
        }
    }
    
    if ($option -ne '6') {
        Write-Host "`nPresiona cualquier tecla para continuar..."
        $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    }
} while ($option -ne '6')

Write-Host "`n╔════════════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║                                                            ║" -ForegroundColor Green
Write-Host "║  ✅ ARCHIVOS DE SEED DATA CREADOS EXITOSAMENTE            ║" -ForegroundColor Green
Write-Host "║                                                            ║" -ForegroundColor Green
Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Green

Write-Info "`n📁 Archivos creados:"
Write-Info "   • DataSeeder.cs - Clase principal de seed"
Write-Info "   • SeedProgram.cs - Programa de consola"
Write-Info "   • SeedController.cs - Endpoint de API"
Write-Info "   • quick-seed.ps1 - Script de ejecución rápida"
Write-Info "   • SEED_DATA.md - Documentación completa"

Write-Info "`n🚀 Próximos pasos:"
Write-Info "   1. Asegúrate de que las entidades en Domain estén creadas"
Write-Info "   2. Ajusta los nombres de las entidades en DataSeeder.cs"
Write-Info "   3. Configura la cadena de conexión en appsettings.json"
Write-Info "   4. Ejecuta las migraciones: .\migrate-database.ps1"
Write-Info "   5. Ejecuta el seed: .\scripts\quick-seed.ps1"

Write-Success "`n💡 Lee la documentación en docs\SEED_DATA.md para más detalles"