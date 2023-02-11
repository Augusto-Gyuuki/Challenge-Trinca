using Challenge.Trinca.Application;
using Challenge.Trinca.Infrastructure;
using Challenge.Trinca.Persistence;
using Challenge.Trinca.Presentation;
using Challenge.Trinca.Web;
using Challenge.Trinca.Web.Settings;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);
{
    var appSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

    builder.Services
        .AddWeb()
        .AddPersistance(appSettings.CosmosDbSettings)
        .AddApplication()
        .AddPresentation()
        .AddInfrastructure(appSettings.OutboxMessageSettings);
}

var app = builder.Build();
{
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });

    app.UseFastEndpoints(c =>
    {
        c.Endpoints.RoutePrefix = "api";
        //c.Endpoints.Configurator = ep =>
        //{
        //    ep.PostProcessors(Order.After, new ExceptionHandlingPostProcessor());
        //};
    });

    app.UseOpenApi();
    app.UseSwaggerUi3(c => c.ConfigureDefaults());

    app.UseHttpsRedirection();
    app.UseDefaultExceptionHandler();
    app.Run();
}