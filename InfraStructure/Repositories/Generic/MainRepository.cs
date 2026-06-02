using InfraStructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace InfraStructure.Repositories.Generic
{
    public class MainRepository<T> : IMainInterFace<T> where T : class
    {

        private readonly AppdbContext _context;
        private readonly Microsoft.EntityFrameworkCore.DbSet<T> _dbSet;
       

        public MainRepository(AppdbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();

        }
        public async Task<IEnumerable<T>> GetALL()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync(
    params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync();
        }
        public async Task<(IEnumerable<T> Data, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            // Includes
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            // Filter
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Total Count
            int totalCount = await query.CountAsync();

            // Order By
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Pagination
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalCount);
        }
        public async Task<IQueryable> GetQueryable()
        {
            return _dbSet.AsNoTracking().AsQueryable();
        }
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            // Includes
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query
                .Where(predicate)
                .ToListAsync();
        }
        public async Task<T> GetByID(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public virtual async Task Create(T t)
        {
            _dbSet.Add(t);
            await _context.SaveChangesAsync();
        }
        public virtual async Task Update(T t)
        {
            _dbSet.Update(t);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(T t)
        {
            _dbSet.Remove(t);
            await _context.SaveChangesAsync();
        }
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }
    }
}
