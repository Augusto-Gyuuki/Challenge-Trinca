using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Challenge.Trinca.Presentation;

[ExcludeFromCodeCoverage]
public static class DependecyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection service)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());
        service.AddSingleton(config);
        service.AddScoped<IMapper, ServiceMapper>();

        return service;
    }
}