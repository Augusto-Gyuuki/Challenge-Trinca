using Challenge.Trinca.Application.UseCases.Peoples.Commands.DeclineInvite;
using Challenge.Trinca.Presentation.Common.Endpoint;
using Challenge.Trinca.Presentation.Endpoints.Common.Extensions;
using Challenge.Trinca.Presentation.Endpoints.Peoples.Common;
using Challenge.Trinca.Presentation.Endpoints.Peoples.Common.Requests;
using MediatR;
using IMapper = MapsterMapper.IMapper;

namespace Challenge.Trinca.Presentation.Endpoints.Peoples.DeclineInvite;

public sealed class DeclineInviteEndpoint : ApiEndpoint<InviteRequest>
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public DeclineInviteEndpoint(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Put(PeopleEndpointConfiguration.DeclineInvite);
        AllowAnonymous();
    }

    public override async Task HandleAsync(InviteRequest request, CancellationToken ct)
    {
        var personIdKvp = HttpContext.Request.Headers
            .FirstOrDefault(x => x.Key.Equals(PeopleEndpointConfiguration.PeopleIdHeaderName));

        var personId = personIdKvp.Value.FirstOrDefault();
        var inviteId = Route<string>(PeopleEndpointConfiguration.InviteIdParam);

        var declineInviteCommand = _mapper.Map<DeclineInviteCommand>((personId, inviteId, request));

        var declineInviteResult = await _mediator.Send(declineInviteCommand, ct);

        await declineInviteResult.Match(
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