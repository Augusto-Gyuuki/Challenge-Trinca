using Challenge.Trinca.Application.UseCases.Bbqs.Queries.ListBbqs;
using Challenge.Trinca.Presentation.Common.Endpoint;
using Challenge.Trinca.Presentation.Endpoints.Bbqs.Common;
using Challenge.Trinca.Presentation.Endpoints.Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using IMapper = MapsterMapper.IMapper;

namespace Challenge.Trinca.Presentation.Endpoints.Bbqs.ListBbqs;

public sealed class ListBbqsEndpoint : ApiEndpoint<ListBbqsRequest>
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public ListBbqsEndpoint(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get(BbqEndpointConfiguration.ListRoute);
        AllowAnonymous();
    }

    public override async Task HandleAsync([FromQuery] ListBbqsRequest req, CancellationToken ct)
    {
        var listBbqsQuery = _mapper.Map<ListBbqsQuery>(req);

        var listBbqsResult = await _mediator.Send(listBbqsQuery, ct);

        await listBbqsResult.Match(
            async (result) =>
            {
                var response = result.ToApiResponseList();
                await SendOkAsync(response, ct);
            },
            async (errors) =>
            {
                var response = errors.ToErrorResponse();
                await SendAsync(response, response.StatusCode, ct);
            }
        );
    }
}