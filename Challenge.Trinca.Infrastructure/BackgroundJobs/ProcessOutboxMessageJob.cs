using Challenge.Trinca.Domain.Common.Models;
using Challenge.Trinca.Infrastructure.Settings;
using Challenge.Trinca.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Polly;
using Quartz;

namespace Challenge.Trinca.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public sealed class ProcessOutboxMessageJob : IJob
{
    private readonly AppDbContext _appDbContext;
    private readonly IPublisher _publisher;
    private readonly OutboxMessageSettings _outboxMessageSettings;

    public ProcessOutboxMessageJob(
        AppDbContext appDbContext,
        IPublisher publisher,
        OutboxMessageSettings outboxMessageSettings)
    {
        _appDbContext = appDbContext;
        _publisher = publisher;
        _outboxMessageSettings = outboxMessageSettings;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var outboxMessageList = await _appDbContext.OutboxMessages
            .Where(x => x.ProcessedAt == null)
            .Take(_outboxMessageSettings.MessagesTakeCount)
            .ToListAsync(context.CancellationToken);

        foreach (var outboxMessage in outboxMessageList)
        {
            IDomainEvent? domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                outboxMessage.Content,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                });

            if (domainEvent is null)
            {
                continue;
            }

            var policy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(
                    _outboxMessageSettings.RetryCount,
                    attempt => TimeSpan.FromSeconds(attempt * _outboxMessageSettings.RetryWaitTimeInSeconds)
                );

            var policyResult = await policy.ExecuteAndCaptureAsync(() =>
            {
                return _publisher.Publish(domainEvent, context.CancellationToken);
            });

            outboxMessage.Error = policyResult.FinalException?.ToString();
            outboxMessage.ProcessedAt = DateTime.UtcNow;
        }

        await _appDbContext.SaveChangesAsync(context.CancellationToken);
    }
}
