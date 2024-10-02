using Dapper;
using Studweb.Application.Persistance;
using Studweb.Domain.Common.Interfaces;
using Studweb.Infrastructure.Utilities;
using Studweb.Infrastructure.Utils;

namespace Studweb.Infrastructure.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly DbContext _dbContext;
    private readonly IUserRepository _userRepository;

    public TokenRepository(DbContext dbContext, IUserRepository userRepository)
    {
        _dbContext = dbContext;
        _userRepository = userRepository;
    }

    public async Task<int> AddTokenAsync(IToken token, CancellationToken cancellationToken = default)
    {
        var connection = _dbContext.Connection;


        
        // TODO: Adding token to database
        const string sql = @"INSERT INTO Tokens (Value, CreatedOnUtc, ExpiresOnUtc, Type) 
                            OUTPUT Inserted.Id
                            VALUES (@Value, @CreatedOnUtc, @ExpiresOnUtc, @Type)";

        var parameters = new
        {
            Value = token.Value,
            CreatedOnUtc = token.CreatedOnUtc,
            ExpiresOnUtc = token.ExpiresOnUtc,
            Type = token.Type,
        };
        
        var id = await connection.ExecuteScalarAsync<int>(sql, token);

        return id;
    }

    public Task<int> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}