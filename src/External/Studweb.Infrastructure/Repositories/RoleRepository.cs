using Dapper;
using Studweb.Application.Persistance;
using Studweb.Domain.Entities;
using Studweb.Infrastructure.Utils;

namespace Studweb.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly SqlConnectionFactory _sqlConnectionFactory;

    public RoleRepository(SqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var connection = _sqlConnectionFactory.Create();
        
        const string sql = "SELECT Id, Name FROM Roles";

        var roles = await connection.QueryAsync<Role>(sql);
        
        return roles;
    }

    public async Task<Role?> GetByIdAsync(int roleId, CancellationToken cancellationToken = default)
    {
        using var connection = _sqlConnectionFactory.Create();

        const string sql = @"SELECT Id, Name FROM Roles WHERE Id = @Id";

        var role = await connection.QueryFirstOrDefaultAsync<Role>(sql, new { Id = roleId });
        
        return role;
    }

    public async Task<Role?> GetByNameAsync(string roleName, CancellationToken cancellationToken = default)
    {
        using var connection = _sqlConnectionFactory.Create();

        const string sql = @"SELECT Id, Name FROM Roles WHERE Name = @Name";

        var role = await connection.QueryFirstOrDefaultAsync<Role>(sql, new { Name = roleName });
        
        return role;
    }

    public async Task<int> AddAsync(Role role, CancellationToken cancellationToken = default)
    {
        using var connection = _sqlConnectionFactory.Create();

        const string sql = @"INSERT INTO Roles (Name) OUTPUT Inserted.Id VALUES (@Name)";

        var id = await connection.ExecuteScalarAsync<int>(sql, new { Name = role.Name });

        return id;
    }

    public async Task<Role> EditAsync(int id, string name, CancellationToken cancellationToken = default)
    {
        using var connection = _sqlConnectionFactory.Create();

        const string sql = @"
                            UPDATE Roles 
                            SET Name = @Name 
                            WHERE Id = @Id;
                            SELECT Id, Name
                            FROM Roles
                            WHERE Id = @Id";

        var role = await connection.QueryFirstOrDefaultAsync<Role>(sql, new { Id = id, Name = name });

        return role;
    }
}