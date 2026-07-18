using System.Data;

namespace InfraStructure.Persistence.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync();

    Task BeginTransactionAsync();

    Task CommitTransactionAsync();
    Task CommitTransactionAndSaveChangesAsync();

    Task RollbackTransactionAsync();
    IDbTransaction? CurrentTransaction { get; }
}
