using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Challenge.Trinca.Application;

[ExcludeFromCodeCoverage]
public static class DependecyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        service.AddMediatR(typeof(DependecyInjection).Assembly);

        return service;
    }
}
