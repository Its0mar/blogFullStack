

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ZeroBlog.Core.Domain.Entities;
using ZeroBlog.Core.Domain.RepositoryContracts;
using ZeroBlog.Infrastructure.DBContext;

namespace ZeroBlog.Infrastructure
{
    public class BaseRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDBContext _context;
        private readonly DbSet<T> _dbSet;
        public BaseRepository(AppDBContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetWithIncludeAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.Where(predicate).ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
