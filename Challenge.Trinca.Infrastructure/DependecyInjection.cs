using Challenge.Trinca.Infrastructure.BackgroundJobs;
using Challenge.Trinca.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.Diagnostics.CodeAnalysis;

namespace Challenge.Trinca.Infrastructure;

[ExcludeFromCodeCoverage]
public static class DependecyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service, OutboxMessageSettings outboxMessageSettings)
    {
        service
            .AddBackgroundJobs(outboxMessageSettings)
            .AddSettings(outboxMessageSettings);

        return service;
    }

    public static IServiceCollection AddBackgroundJobs(this IServiceCollection service, OutboxMessageSettings outboxMessageSettings)
    {
        service.AddQuartz(config =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessageJob));

            config.AddJob<ProcessOutboxMessageJob>(jobKey)
                  .AddTrigger(trigger =>
                  {
                      trigger.ForJob(jobKey)
                             .WithSimpleSchedule(schedule =>
                             {
                                 schedule.WithIntervalInSeconds(outboxMessageSettings.BackgroundIntevalInSeconds)
                                         .RepeatForever();
                             });
                  });

            config.UseMicrosoftDependencyInjectionJobFactory();
        });

        service.AddQuartzHostedService();

        return service;
    }

    public static IServiceCollection AddSettings(this IServiceCollection service, OutboxMessageSettings outboxMessageSettings)
    {
        service.AddSingleton(outboxMessageSettings);

        return service;
    }
}
