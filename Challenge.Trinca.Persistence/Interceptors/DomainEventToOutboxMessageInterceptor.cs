using Challenge.Trinca.Domain.Common.Models;
using Challenge.Trinca.Persistence.Outbox;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace Challenge.Trinca.Persistence.Interceptors;

public sealed class DomainEventToOutboxMessageInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var outboxMessageList = dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(aggregateRoot => aggregateRoot.Entity)
            .SelectMany(aggregateRoot =>
            {
                var domainEvents = aggregateRoot.GetDomainEvents();
                aggregateRoot.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredAt = DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                    })
            })
            .ToList();

        dbContext.Set<OutboxMessage>().AddRange(outboxMessageList);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
