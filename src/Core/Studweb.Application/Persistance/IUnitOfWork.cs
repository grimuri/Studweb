namespace Studweb.Application.Persistance;

public interface IUnitOfWork : IDisposable
{
    void BeginTransaction();

    void Commit();

    void CommitAndCloseConnection();

    void Rollback();
}