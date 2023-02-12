using Challenge.Trinca.Application.UseCases.Bbqs.Queries.GetBbq;
using Challenge.Trinca.Presentation.Common.Endpoint;
using Challenge.Trinca.Presentation.Endpoints.Bbqs.Common;
using Challenge.Trinca.Presentation.Endpoints.Common.Extensions;
using Challenge.Trinca.Presentation.Endpoints.Peoples.Common;
using MediatR;
using IMapper = MapsterMapper.IMapper;

namespace Challenge.Trinca.Presentation.Endpoints.Bbqs.GetBbq;

public sealed class GetBbqEndpoint : ApiEndpoint
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public GetBbqEndpoint(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get(BbqEndpointConfiguration.ShoppingListRoute);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var personIdKvp = HttpContext.Request.Headers
            .FirstOrDefault(x => x.Key.Equals(PeopleEndpointConfiguration.PeopleIdHeaderName));

        var personId = personIdKvp.Value.FirstOrDefault();
        var bbqId = Route<string>(BbqEndpointConfiguration.BbqIdParam);

        var getBbqQuery = _mapper.Map<GetBbqQuery>((personId, bbqId));

        var getBbqResult = await _mediator.Send(getBbqQuery, ct);

        await getBbqResult.Match(
            async (result) =>
            {
                var response = result.ToApiResponse();
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