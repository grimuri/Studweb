using Studweb.Domain.Primitives;

namespace Studweb.Infrastructure.Outbox;

public interface IOutboxMessageRepository
{
    Task SaveDomainEvents(IDomainEvent domainEvent);
    
    Task<IEnumerable<OutboxMessage>> GetUnProcessedDomainEvents();

    Task ProcessDomainEvent(int id);
}