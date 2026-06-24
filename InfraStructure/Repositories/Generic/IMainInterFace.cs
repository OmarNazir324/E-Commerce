using System.Data;
using System.Linq.Expressions;

namespace InfraStructure.Repositories.Generic;

public interface IMainInterFace<T>
{
    public Task<IEnumerable<T>> GetALL();

    Task<IEnumerable<T>> GetSelectedFields<T>(String sql, IDbTransaction? transaction = null) where T : class;
    Task<IEnumerable<dynamic>> GetSelectedFields(string sql, IDbTransaction? transaction = null);
    Task<IQueryable> GetQueryable();
    Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
    Task<(IEnumerable<T> Data, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        params Expression<Func<T, object>>[] includes);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    public Task<T?> GetByID(int id);
    public Task Create(T entity);
    public Task Update(T t);
    public Task Delete(T t);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);


}
