using Challenge.Trinca.Application.UseCases.Peoples.Queries.GetPeopleInvites;
using Challenge.Trinca.Presentation.Common.Endpoint;
using Challenge.Trinca.Presentation.Endpoints.Common.Extensions;
using Challenge.Trinca.Presentation.Endpoints.Peoples.Common;
using MediatR;
using IMapper = MapsterMapper.IMapper;


namespace Challenge.Trinca.Presentation.Endpoints.Peoples.GetPeopleInvites;

public sealed class GetPeopleInvitesEndpoint : ApiEndpoint
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public GetPeopleInvitesEndpoint(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get(PeopleEndpointConfiguration.GetPeopleInvites);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var peopleIdKvp = HttpContext.Request.Headers
            .FirstOrDefault(x => x.Key.Equals(PeopleEndpointConfiguration.PeopleIdHeaderName));

        var peopleId = peopleIdKvp.Value.FirstOrDefault();

        var getPeopleInvitesQuery = new GetPeopleInvitesQuery()
        {
            PeopleId = peopleId
        };

        var getPeopleInvitesResult = await _mediator.Send(getPeopleInvitesQuery, ct);

        await getPeopleInvitesResult.Match(
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