using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Domain.Repositories;
using Challenge.Trinca.Persistence.Data;
using Challenge.Trinca.Persistence.Interceptors;
using Challenge.Trinca.Persistence.Repositories;
using Challenge.Trinca.Persistence.Settings;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Challenge.Trinca.Persistence;

[ExcludeFromCodeCoverage]
public static class DependecyInjection
{
    public static IServiceCollection AddPersistance(this IServiceCollection service, CosmosDbSettings cosmosDbSettings)
    {
        service
            .AddDatabase(cosmosDbSettings)
            .AddRepositories();

        return service;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection service)
    {
        service.AddScoped<IUnitOfWork, UnitOfWork>();
        service.AddScoped<IBbqRepository, BbqRepository>();
        service.AddScoped<IPeopleRepository, PeopleRepository>();

        return service;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection service, CosmosDbSettings cosmosDbSettings)
    {
        service.AddSingleton<DomainEventToOutboxMessageInterceptor>();

        service.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var interceptor = serviceProvider.GetService<DomainEventToOutboxMessageInterceptor>();
            //var connectionString = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==;";

            //options.UseCosmos(
            //    connectionString,
            //    cosmosDbSettings.DatabaseName);

            options.UseCosmos(
                cosmosDbSettings.AccountEndpoint,
                cosmosDbSettings.AccountKey,
                cosmosDbSettings.DatabaseName, (options) =>
                {
                    options.ConnectionMode(ConnectionMode.Gateway);
                    options.HttpClientFactory(() =>
                    {
                        HttpMessageHandler httpMessageHandler = new HttpClientHandler()
                        {
                            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                        };

                        return new HttpClient(httpMessageHandler);
                    });
                })
                .AddInterceptors(interceptor);
        });

        var serviceProvider = service.BuildServiceProvider();

        var appDbContext = serviceProvider.GetService<AppDbContext>();
        appDbContext?.Database.EnsureCreatedAsync().Wait();

        return service;
    }
}