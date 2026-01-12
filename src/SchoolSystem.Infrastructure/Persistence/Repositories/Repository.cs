using Microsoft.EntityFrameworkCore;
using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Interfaces;
using SchoolSystem.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Infrastructure.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly SchoolSystemDbContext _context;

        public Repository(SchoolSystemDbContext context) => _context = context;

        public async Task<T> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            // Aplicar relaciones si existen
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.Where(predicate).ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            //await SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            //await SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync() =>
            await _context.SaveChangesAsync();

        public async Task<IEnumerable<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();

            // Aplica cada include (ej: x => x.Alumno)
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Agrega múltiples registros (Optimizado para carga masiva)
        /// </summary>
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.AddRangeAsync(entities);
            // No hacemos SaveChanges aquí para permitir que el servicio controle la transacción
        }

        public Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _context.UpdateRange(entities);
            return Task.CompletedTask;
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().CountAsync(predicate);
        }

        public IQueryable<T> GetQueryable()
        {
            // Retornamos el DbSet como IQueryable para encadenar Where, OrderBy, etc.
            return _context.Set<T>().AsQueryable();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetAllDeletedAsync()
        {
            // IgnoreQueryFilters permite ver los Soft Deleted
            return await _context.Set<T>().IgnoreQueryFilters()
                               .Where(x => EF.Property<bool>(x, "IsDeleted"))
                               .ToListAsync();
        }

        public async Task RestaurarAsync(int id)
        {
            var entity = await _context.Set<T>().IgnoreQueryFilters()
                                     .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);

            if (entity != null)
            {
                // Usamos reflexión o acceso directo si la entidad implementa ISoftDeletable
                var softDeletable = entity as ISoftDeletable;
                if (softDeletable != null)
                {
                    softDeletable.IsDeleted = false;
                    softDeletable.DeletedAt = null;
                    softDeletable.DeletedBy = null;

                    _context.Set<T>().Update(entity);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
