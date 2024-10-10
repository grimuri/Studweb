using System.Data;
using Microsoft.Data.SqlClient;
using Studweb.Infrastructure.Persistance;

namespace Studweb.Infrastructure.Utilities;

public class DbContext : IDbContext
{
    private readonly string _connectionString;
    
    private IDbConnection? _connection;

    public IDbConnection? Connection
    {
        get
        {
            if (_connection is null || _connection.State != ConnectionState.Open)
            {
                _connection = Create();
            }

            return _connection;
        } 
    }

    public IDbTransaction? Transaction { get; set; }

    public DbContext(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("The connection string cannot be null or empty", nameof(connectionString));
        }
        
        _connectionString = connectionString;
    }

    public SqlConnection Create()
    {
        return new SqlConnection(_connectionString);
    }

    public SqlConnection GetSqlConnection() => (SqlConnection)Connection;

    public SqlTransaction GetSqlTransaction() => (SqlTransaction)Transaction;

}