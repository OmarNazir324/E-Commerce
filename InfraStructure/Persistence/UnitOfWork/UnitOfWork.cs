using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace InfraStructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppdbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(AppdbContext context)
        {
            _context = context;
        }
        public IDbTransaction? CurrentTransaction =>
        _transaction?.GetDbTransaction();
        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {

                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
        }
        public async Task CommitTransactionAndSaveChangesAsync()
        {
            try
            {

                if (_transaction != null)
                {
                    await SaveChangesAsync();
                    await _transaction.CommitAsync();
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

    }
}
