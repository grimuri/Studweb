using Dapper;
using Studweb.Application.Persistance;
using Studweb.Domain.Entities;
using Studweb.Infrastructure.Utils;
using Studweb.Infrastructure.Utils.TempClasses;

namespace Studweb.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly DbContext _dbContext;

    public RoleRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var connection = _dbContext.Create();
        
        const string sql = "SELECT Id, Name FROM Roles";

        var roles = await connection.QueryAsync<Role>(sql);
        
        return roles;
    }

    public async Task<Role?> GetByIdAsync(int roleId, CancellationToken cancellationToken = default)
    {
        var connection = _dbContext.Create();

        const string sql = @"SELECT Id, Name FROM Roles WHERE Id = @Id";

        var result = await connection.QueryFirstOrDefaultAsync<RoleTemp>(sql, new { Id = roleId });

        var role = result?.Convert();
        
        return role;
    }

    public async Task<Role?> GetByNameAsync(string roleName, CancellationToken cancellationToken = default)
    {
        var connection = _dbContext.Create();

        const string sql = @"SELECT Id, Name FROM Roles WHERE Name = @Name";

        var result = await connection.QueryFirstOrDefaultAsync<RoleTemp>(sql, new { Name = roleName });

        var role = result?.Convert();
        
        return role;
    }

    public async Task<int> AddAsync(Role role, CancellationToken cancellationToken = default)
    {
        using var connection = _dbContext.Create();

        const string sql = @"INSERT INTO Roles (Name) OUTPUT Inserted.Id VALUES (@Name)";

        var id = await connection.ExecuteScalarAsync<int>(sql, new { Name = role.Name });

        return id;
    }

    public async Task<Role> EditAsync(int id, string name, CancellationToken cancellationToken = default)
    {
        using var connection = _dbContext.Create();

        const string sql = @"
                            UPDATE Roles SET Name = @Name WHERE Id = @Id;
                            SELECT Id, Name FROM Roles WHERE Id = @Id";

        var role = await connection.QueryFirstOrDefaultAsync<Role>(sql, new { Id = id, Name = name });

        return role;
    }

    public async Task<int> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        using var connection = _dbContext.Create();

        const string sql = @"DELETE FROM Roles WHERE Id = @Id";

        var affectedRow = await connection.ExecuteAsync(sql, new { Id = id });

        return affectedRow;
    }
}