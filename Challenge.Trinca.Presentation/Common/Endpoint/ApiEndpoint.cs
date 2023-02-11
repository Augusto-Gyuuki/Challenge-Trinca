using FastEndpoints;

namespace Challenge.Trinca.Presentation.Common.Endpoint;

public abstract class ApiEndpoint : EndpointWithoutRequest
{
    public abstract override void Configure();

    public abstract override Task HandleAsync(CancellationToken ct);
}

public abstract class ApiEndpoint<TRequest> : Endpoint<TRequest>
    where TRequest : notnull, new()
{
    public abstract override void Configure();

    public abstract override Task HandleAsync(TRequest req, CancellationToken ct);
}