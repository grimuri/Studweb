using System.Data;
using Studweb.Application.Persistance;
using Studweb.Infrastructure.Utils;

namespace Studweb.Infrastructure.Persistance;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _dbContext;
    
    private bool _disposed;

    public UnitOfWork(DbContext dbContext)
    {
        _dbContext = dbContext;
        _disposed = false;
    }

    // public void BeginTransaction()
    // {
    //     _dbContext.Connection?.Open();
    //     _dbContext.Transaction = _dbContext.Connection?.BeginTransaction();
    // }
    
    public void BeginTransaction()
    {
        if (_dbContext.Connection == null)
        {
            throw new InvalidOperationException("The database connection has not been initialized.");
        }

        if (_dbContext.Connection.State != ConnectionState.Open)
        {
            _dbContext.Connection.Open();
        }

        _dbContext.Transaction = _dbContext.Connection.BeginTransaction();
    }


    public void Commit()
    {
        _dbContext.Transaction?.Commit();
        DisposeTransactionAndSetNull();
    }

    public void CommitAndCloseConnection()
    {
        Commit();
        _dbContext.Connection?.Close();
        _dbContext.Connection?.Dispose();
    }

    public void Rollback()
    {
        if (_dbContext.Transaction == null)
        {
            throw new InvalidOperationException("No transaction has been started.");
        }
        
        _dbContext.Transaction?.Rollback();
        DisposeTransactionAndSetNull();
    }
    
    public void Dispose()
    {
        if (!_disposed)
        {
            _dbContext.Connection?.Dispose();
            DisposeTransactionAndSetNull();
            _disposed = true;
        }
    }

    private void DisposeTransactionAndSetNull()
    {
        if (_dbContext.Transaction != null)
        {
            _dbContext.Transaction.Dispose();
            _dbContext.Transaction = null;
        }
    }

}