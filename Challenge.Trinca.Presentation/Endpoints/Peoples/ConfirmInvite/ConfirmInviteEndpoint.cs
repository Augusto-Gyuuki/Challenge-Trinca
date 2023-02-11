using Challenge.Trinca.Application.UseCases.Peoples.Commands.ConfirmInvite;
using Challenge.Trinca.Presentation.Common.Endpoint;
using Challenge.Trinca.Presentation.Endpoints.Common.Extensions;
using Challenge.Trinca.Presentation.Endpoints.Peoples.Common;
using Challenge.Trinca.Presentation.Endpoints.Peoples.Common.Requests;
using MediatR;
using IMapper = MapsterMapper.IMapper;

namespace Challenge.Trinca.Presentation.Endpoints.Peoples.ConfirmInvite;

public sealed class ConfirmInviteEndpoint : ApiEndpoint<InviteRequest>
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public ConfirmInviteEndpoint(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Put(PeopleEndpointConfiguration.ConfirmInvite);
        AllowAnonymous();
    }

    public override async Task HandleAsync(InviteRequest request, CancellationToken ct)
    {
        var personIdKvp = HttpContext.Request.Headers
            .FirstOrDefault(x => x.Key.Equals(PeopleEndpointConfiguration.PersonIdHeaderName));

        var personId = personIdKvp.Value.FirstOrDefault();
        var inviteId = Route<string>(PeopleEndpointConfiguration.InviteIdParam);

        var getPeopleInvitesQuery = _mapper.Map<ConfirmInviteCommand>((personId, inviteId, request));

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