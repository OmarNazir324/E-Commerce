using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace InfraStructure.Repositories.Generic;

public interface IGenericRepository<T>
{
     Task<IEnumerable<T>> GetALLAsync();

    Task<IEnumerable<T>> GetSelectedFields<T>(String sql, IDbTransaction? transaction = null) where T : class;
    Task<IEnumerable<dynamic>> GetSelectedFields(string sql, IDbTransaction? transaction = null);
    IQueryable GetQueryable();
    DbConnection GetConnection { get; }
    Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
    Task<(IEnumerable<T> Data, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        params Expression<Func<T, object>>[] includes);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    public Task<T?> GetByIdAsync(int id);
    public Task<T?> AddAsync(T entity);
    public Task UpdateAsync(T t);
    public Task Delete(T t);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);


}
