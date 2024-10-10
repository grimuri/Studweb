using System.Data;
using Microsoft.Data.SqlClient;

namespace Studweb.Infrastructure.Persistance;

public interface IDbContext
{
    IDbConnection Connection { get; }
    IDbTransaction Transaction { get; set; }
    public SqlConnection Create();
}