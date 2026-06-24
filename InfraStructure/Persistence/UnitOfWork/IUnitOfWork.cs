using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace InfraStructure.Persistence.UnitOfWork
{
    public interface IUnitOfWork:IDisposable
    {
        Task<int> SaveChangesAsync();

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
        IDbTransaction? CurrentTransaction { get; }
    }
}
