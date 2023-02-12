using Challenge.Trinca.Web.Settings;
using FastEndpoints;
using FastEndpoints.Swagger;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Diagnostics.CodeAnalysis;
using ILogger = Serilog.ILogger;

namespace Challenge.Trinca.Web;

[ExcludeFromCodeCoverage]
public static class DependecyInjection
{
    public static IServiceCollection AddWeb(this IServiceCollection service, AppSettings appSettings, ElasticConfiguration elasticConfiguration)
    {
        service.AddLogger(appSettings, elasticConfiguration);

        service.AddFastEndpoints();

        service.AddSwaggerDoc();

        return service;
    }

    private static IServiceCollection AddLogger(this IServiceCollection service, AppSettings appSettings, ElasticConfiguration elasticConfiguration)
    {
        service.AddSingleton<ILogger>(new LoggerConfiguration()
                .Enrich.WithMachineName()
                .WriteTo.Console()
                //.WriteTo.Seq("http://seq:5341")
                .WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(elasticConfiguration.Uri))
                    {
                        IndexFormat = $"{appSettings.ApplicationName}-logs-{appSettings.EnvironmentName.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyyy-MM}",
                        AutoRegisterTemplate = true,
                        NumberOfReplicas = 1,
                        NumberOfShards = 2
                    })
                .Enrich.WithProperty("Environment", appSettings.EnvironmentName)
                .Enrich.WithProperty(nameof(AppSettings.ApplicationName), appSettings.ApplicationName)
                .Enrich.FromLogContext()
                .CreateLogger());

        return service;
    }
}
