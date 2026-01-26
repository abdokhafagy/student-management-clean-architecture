
using System.Linq.Expressions;

namespace StudentManagement.Domain.IBaseRepositories;

public interface IBaseRepository <T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync( Expression<Func<T, bool>>? predicate = null,
        params Expression<Func<T, object>>[] includes);
    Task<IEnumerable<TResult>> GetAllAsync<T, TResult>(
                 Expression<Func<T, bool>> predicate,
                 Expression<Func<T, TResult>> selector,
                 params Expression<Func<T, object>>[] includes)where T : class;
    Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate);
    Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<T> GetByIdAsync(string id);
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task<int> DeleteAsync(string id);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>? includeProperty = null);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    bool Exists(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> OrderByAsync(
        Expression<Func<T, object>> orderBy,
        Expression<Func<T, bool>> filter = null,
        bool descending = true,
        int? take = null);
    public IEnumerable<T> OrderBy(
                  Expression<Func<T, object>> orderBy,
                  Expression<Func<T, bool>> filter = null!,
                  bool descending = true,
                  int? take = null);
}
