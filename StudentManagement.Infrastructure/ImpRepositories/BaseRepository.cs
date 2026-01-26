using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.IBaseRepositories;
using StudentManagmentSystemApi.Data;
using StudentManagmentSystemApi.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace StudentManagement.Infrastructure.ImpRepositories
{

    public class BaseRepository<T>  (ApplicationDbContext _context) : IBaseRepository<T> where T : class
    {
        public async Task<IEnumerable<T>> GetAllAsync(
                                          Expression<Func<T, bool>>? predicate = null,
                                          params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();

            // Apply includes
            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            // Apply predicate if provided
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            var result = await query.ToListAsync();
            return result ?? Enumerable.Empty<T>();
        }



        public async Task<IEnumerable<TResult>> GetAllAsync<T, TResult>(
                                  Expression<Func<T, bool>> predicate,
                                  Expression<Func<T, TResult>> selector,
                                  params Expression<Func<T, object>>[] includes
  ) where T : class
        {
            var query = _context.Set<T>().AsQueryable();

            // Apply includes
            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            // Apply predicate (filtering) to the query
            query = query.Where(predicate);

            // Apply the selector (projection) to shape the result
            return await query.Select(selector).ToListAsync();
        }


        public async Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }
        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(predicate);
        }


        public async Task<T> GetByIdAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(string id)
        {
            var entity = await GetByIdAsync(id);

            if (entity is null)
                return 0;
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsync(
      Expression<Func<T, bool>>? predicate = null,
      Expression<Func<T, object>>? includeProperty = null)
        {
            var query = _context.Set<T>().AsQueryable();

            // Apply Include if provided
            if (includeProperty != null)
            {
                query = query.Include(includeProperty);
            }

            // Apply Predicate if provided
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.CountAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }
        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Any(predicate);
        }

        public async Task<IEnumerable<T>> OrderByAsync(
                    Expression<Func<T, object>> orderBy,
                    Expression<Func<T, bool>> filter = null,
                    bool descending = true,
                    int? take = null)
        {
            var query = _context.Set<T>().AsQueryable();

            // Apply filter if specified
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Apply ordering
            query = descending
                ? query.OrderByDescending(orderBy)
                : query.OrderBy(orderBy);

            // If takeFirst is true, return the first result; otherwise, return the full query
            if (take is not null)
            {
                return await query.Take(take.Value).ToListAsync(); // Return the first result as a list to comply with IQueryable
            }

            // Return the full query
            return query;
        }


        public IEnumerable<T> OrderBy(
                   Expression<Func<T, object>> orderBy,
                   Expression<Func<T, bool>> filter = null,
                   bool descending = true,
                    int? take = null)
        {
            var query = _context.Set<T>().AsQueryable();

            // Apply filter if specified
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Apply ordering
            query = descending
                ? query.OrderByDescending(orderBy)
                : query.OrderBy(orderBy);

            // If take is specified, limit the number of results
            if (take is not null)
            {
                return query.Take(take.Value).ToList(); // Use ToList() for synchronous operation
            }

            // Return the full query
            return query.ToList(); // Also use ToList() here to fetch all results synchronously
        }

    }
}
