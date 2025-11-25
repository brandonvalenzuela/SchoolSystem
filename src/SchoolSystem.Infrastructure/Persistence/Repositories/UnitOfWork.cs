using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Escuelas;
using SchoolSystem.Domain.Entities.Evaluacion;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Interfaces;
using SchoolSystem.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SchoolSystemDbContext _context;

        // Backing fields para los repositorios
        private IRepository<Alumno> _alumnos;
        private IRepository<Asistencia> _asistencias;
        private IRepository<Calificacion> _calificaciones;
        private IRepository<Escuela> _escuelas;
        private IRepository<Grado> _grados;
        private IRepository<Grupo> _grupos;
        private IRepository<Inscripcion> _inscripciones;
        private IRepository<Maestro> _maestros;
        private IRepository<Materia> _materias;
        private IRepository<Padre> _padres;
        private IRepository<Usuario> _usuarios;

        public UnitOfWork(SchoolSystemDbContext context)
        {
            _context = context;
        }

        // Inicialización Lazy (solo se crea si se usa)
        public IRepository<Alumno> Alumnos => _alumnos ??= new Repository<Alumno>(_context);
        public IRepository<Asistencia> Asistencias => _asistencias ??= new Repository<Asistencia>(_context);
        public IRepository<Calificacion> Calificaciones => _calificaciones ??= new Repository<Calificacion>(_context);
        public IRepository<Escuela> Escuelas => _escuelas ??= new Repository<Escuela>(_context);
        public IRepository<Grado> Grados => _grados ??= new Repository<Grado>(_context);
        public IRepository<Grupo> Grupos => _grupos ??= new Repository<Grupo>(_context);
        public IRepository<Inscripcion> Inscripciones => _inscripciones ??= new Repository<Inscripcion>(_context);
        public IRepository<Maestro> Maestros => _maestros ??= new Repository<Maestro>(_context);
        public IRepository<Materia> Materias => _materias ??= new Repository<Materia>(_context);
        public IRepository<Padre> Padres => _padres ??= new Repository<Padre>(_context);
        public IRepository<Usuario> Usuarios => _usuarios ??= new Repository<Usuario>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
