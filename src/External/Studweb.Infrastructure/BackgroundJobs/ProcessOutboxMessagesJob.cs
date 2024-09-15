using MediatR;
using Newtonsoft.Json;
using Quartz;
using Studweb.Domain.Primitives;
using Studweb.Infrastructure.Outbox;
using Studweb.Infrastructure.Utils;

namespace Studweb.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
    private readonly IOutboxMessageRepository _outboxMessageRepository;
    private readonly IPublisher _publisher;

    public ProcessOutboxMessagesJob(IPublisher publisher, IOutboxMessageRepository outboxMessageRepository)
    {
        _publisher = publisher;
        _outboxMessageRepository = outboxMessageRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _outboxMessageRepository.GetUnProcessedDomainEvents();

        foreach (var message in messages)
        {
            var domainEvent = JsonConvert
                .DeserializeObject<IDomainEvent>(
                    message.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

            if (domainEvent is null)
            {
                continue;
            }

            await _publisher.Publish(domainEvent, context.CancellationToken);
            
            
        }
    }
}