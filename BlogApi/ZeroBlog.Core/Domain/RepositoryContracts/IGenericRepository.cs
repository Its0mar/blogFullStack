using System.Linq.Expressions;

namespace ZeroBlog.Core.Domain.RepositoryContracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetWithIncludeAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteAsync(Guid id);
    }

}

