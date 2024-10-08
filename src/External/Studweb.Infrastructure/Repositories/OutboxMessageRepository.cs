using Dapper;
using Newtonsoft.Json;
using Studweb.Application.Persistance;
using Studweb.Domain.Primitives;
using Studweb.Infrastructure.Outbox;
using Studweb.Infrastructure.Persistance;
using Studweb.Infrastructure.Utilities;
using Studweb.Infrastructure.Utils;

namespace Studweb.Infrastructure.Repositories;

public class OutboxMessageRepository : IOutboxMessageRepository
{
    private readonly IDbContext _dbContext;

    public OutboxMessageRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveDomainEvents(IDomainEvent domainEvent)
    {
        var connection = _dbContext.Connection;
        var transaction = _dbContext.Transaction;

        const string sql = @"INSERT INTO OutboxMessage (Type, Content, OccuredOnUtc) 
                            VALUES (@Type, @Content, @OccuredOnUtc)";

        var parameters = new
        {
            Type = domainEvent.GetType().Name,
            Content = JsonConvert.SerializeObject(
                domainEvent,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                }),
            OccuredOnUtc = DateTime.UtcNow,
            
        };
        
        await connection.ExecuteScalarAsync(sql, parameters, transaction);
    }

    public async Task<IEnumerable<OutboxMessage>> GetUnProcessedDomainEvents()
    {
        var connection = _dbContext.Connection;
        var transaction = _dbContext.Transaction;

        const string sql = @"SELECT * FROM OutboxMessage WHERE ProcessedOnUtc IS NULL";

        var result = await connection.QueryAsync<OutboxMessage>(sql, transaction: transaction);

        return result.ToList();
    }

    public async Task ProcessDomainEvent(int id)
    {
        var connection = _dbContext.Connection;

        const string sql = @"UPDATE OutboxMessage 
                            SET ProcessedOnUtc = @DateNow 
                            WHERE Id = @Id";

        var parameters = new
        {
            DateNow = DateTime.UtcNow,
            Id = id
        };

        await connection.ExecuteScalarAsync(sql, parameters);
    }
}