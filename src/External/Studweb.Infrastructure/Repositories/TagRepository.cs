using Dapper;
using Studweb.Application.Persistance;
using Studweb.Domain.Aggregates.Notes.ValueObjects;
using Studweb.Infrastructure.Persistance;
using Studweb.Infrastructure.Utils.TempClasses;

namespace Studweb.Infrastructure.Repositories;

public sealed class TagRepository : ITagRepository
{
    private readonly IDbContext _dbContext;

    public TagRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<IEnumerable<Tag>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<int?> GetIdTagByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var connection = _dbContext.Connection;

        const string sql = @"SELECT Id FROM Tags WHERE Name = @Name";

        var tagId = await connection.QueryFirstOrDefaultAsync<int?>(sql, new { Name = name });

        return tagId;
    }

    public Task<Tag?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Tag?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<int> CreateAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        var connection = _dbContext.Connection;

        const string sql = @"INSERT INTO Tags (Name) OUTPUT Inserted.Id VALUES (@Name)";

        var parameters = new
        {
            Name = tag.Value
        };

        var tagId = await connection.ExecuteScalarAsync<int>(sql, parameters);

        return tagId;
    }
}