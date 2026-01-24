using Microsoft.VisualBasic;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Auditoria;
using SchoolSystem.Domain.Entities.Comunicacion;
using SchoolSystem.Domain.Entities.Conducta;
using SchoolSystem.Domain.Entities.Configuracion;
using SchoolSystem.Domain.Entities.Escuelas;
using SchoolSystem.Domain.Entities.Evaluacion;
using SchoolSystem.Domain.Entities.Finanzas;
using SchoolSystem.Domain.Entities.Medico;
using SchoolSystem.Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Repositorios Específicos (o genéricos expuestos)
        IRepository<Alumno> Alumnos { get; }
        IRepository<Asistencia> Asistencias { get; }
        IRepository<Calificacion> Calificaciones { get; }
        IRepository<Escuela> Escuelas { get; }
        IRepository<Grado> Grados { get; }
        IRepository<Grupo> Grupos { get; }
        IRepository<Inscripcion> Inscripciones { get; }
        IRepository<Maestro> Maestros { get; }
        IRepository<Materia> Materias { get; } 
        IRepository<Padre> Padres { get; } 
        IRepository<Usuario> Usuarios { get; }
        IRepository<RegistroConducta> RegistroConductas { get; }
        IRepository<LogAuditoria> LogAuditorias { get; }
        IRepository<ConfiguracionEscuela> ConfiguracionEscuelas { get; }
        IRepository<Cargo> Cargos { get; }
        IRepository<ConceptoPago> ConceptoPagos { get; }
        IRepository<EstadoCuenta> EstadoCuentas { get; }
        IRepository<Pago> Pagos { get; }
        IRepository<ExpedienteMedico> ExpedienteMedicos { get; }
        IRepository<Notificacion> Notificaciones { get; }
        IRepository<AlumnoPadre> AlumnoPadres { get; }
        IRepository<CicloEscolar> CicloEscolares { get; }
        IRepository<PeriodoEvaluacion> PeriodoEvaluaciones { get; }


            // Método para guardar todos los cambios
            Task<int> SaveChangesAsync();

            // Métodos para transacciones explícitas
            Task BeginTransactionAsync();
            Task CommitTransactionAsync();
            Task RollbackTransactionAsync();
        }
}
