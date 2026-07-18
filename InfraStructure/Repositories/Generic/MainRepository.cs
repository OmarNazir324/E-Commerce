using Dapper;
using InfraStructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

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
        public AppdbContext GetCurrentContext => _context;
        public DbConnection GetConnection => _context.Database.GetDbConnection();
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

            return await query.AsNoTracking().ToListAsync();
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
            var data = await query.AsNoTracking()
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
            return await query.AsNoTracking()
                .Where(predicate)
                .ToListAsync();
        }
        public virtual async Task<T?> GetByID(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<IEnumerable<T>> GetSelectedFields<T>(string sql, IDbTransaction? transaction = null)
            where T : class
        {
            var connection = _context.Database.GetDbConnection();
            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync();
            }

            var result = await connection.QueryAsync<T>(sql, transaction: transaction);

            return result.ToList();
        }
        public async Task<IEnumerable<dynamic>> GetSelectedFields(
            string sql,
            IDbTransaction? transaction = null)
        {
            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            return await connection.QueryAsync(sql, transaction: transaction);
        }
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }
        #region CRUD

        public virtual async Task Create(T entity)
        {
            _dbSet.Add(entity);
        }
        public virtual async Task Update(T t)
        {
            _dbSet.Update(t);
        }
        public async Task Delete(T t)
        {
            _dbSet.Remove(t);
        }
        #endregion
    }
}
