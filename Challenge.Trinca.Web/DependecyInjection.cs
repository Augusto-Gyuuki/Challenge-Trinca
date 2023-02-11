using FastEndpoints;
using FastEndpoints.Swagger;
using System.Diagnostics.CodeAnalysis;

namespace Challenge.Trinca.Web;

[ExcludeFromCodeCoverage]
public static class DependecyInjection
{
    public static IServiceCollection AddWeb(this IServiceCollection service)
    {
        service.AddFastEndpoints();

        service.AddSwaggerDoc();

        return service;
    }
}
