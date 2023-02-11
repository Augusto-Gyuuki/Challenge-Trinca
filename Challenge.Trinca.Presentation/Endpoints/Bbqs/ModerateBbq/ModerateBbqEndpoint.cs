using Challenge.Trinca.Application.UseCases.Bbqs.Commands.ModerateBbq;
using Challenge.Trinca.Presentation.Common.Endpoint;
using Challenge.Trinca.Presentation.Endpoints.Bbqs.Common;
using Challenge.Trinca.Presentation.Endpoints.Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using IMapper = MapsterMapper.IMapper;

namespace Challenge.Trinca.Presentation.Endpoints.Bbqs.ModerateBbq;

public sealed class ModerateBbqEndpoint : ApiEndpoint<ModerateBbqRequest>
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public ModerateBbqEndpoint(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Put(BbqEndpointConfiguration.ModerateRoute);
        AllowAnonymous();
    }

    public override async Task HandleAsync([FromBody] ModerateBbqRequest req, CancellationToken ct)
    {
        var bbqId = Route<string>(BbqEndpointConfiguration.BbqIdParam);

        var moderateBbqCommand = _mapper.Map<ModerateBbqCommand>((req, bbqId));

        var moderateBbqResult = await _mediator.Send(moderateBbqCommand, ct);

        await moderateBbqResult.Match(
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