using System.Data;
using Studweb.Application.Persistance;
using Studweb.Infrastructure.Utilities;
using Studweb.Infrastructure.Utils;

namespace Studweb.Infrastructure.Persistance;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _dbContext;
    private bool _disposed;
    private bool _transactionStarted;

    public UnitOfWork(DbContext dbContext)
    {
        _dbContext = dbContext;
        _disposed = false;
        _transactionStarted = false;
    }
    
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

        if (_transactionStarted)
        {
            throw new InvalidOperationException("A transaction has already been started.");
        }
        
        _dbContext.Transaction = _dbContext.Connection.BeginTransaction();
        _transactionStarted = true;
    }


    public void Commit()
    {
        if (!_transactionStarted)
        {
            throw new InvalidOperationException("No transaction has been started to commit.");
        }

        try
        {
            _dbContext.Transaction?.Commit();
            _transactionStarted = false;
        }
        catch (Exception ex)
        {
            _dbContext.Transaction?.Rollback();
            throw new InvalidOperationException("An error occurred during transaction commit. Transaction has been rolled back.", ex);
        }
        finally
        {
            DisposeTransactionAndSetNull();
        }
    }

    public void CommitAndCloseConnection()
    {
        Commit();
        
        if (_dbContext.Connection == null) return;
        
        _dbContext.Connection?.Close();
        _dbContext.Connection?.Dispose();
    }

    public void Rollback()
    {
        if (!_transactionStarted)
        {
            throw new InvalidOperationException("No transaction has been started to rollback.");
        }

        try
        {
            _dbContext.Transaction?.Rollback();
        }
        finally
        {
            _transactionStarted = false;
            DisposeTransactionAndSetNull();
        }
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