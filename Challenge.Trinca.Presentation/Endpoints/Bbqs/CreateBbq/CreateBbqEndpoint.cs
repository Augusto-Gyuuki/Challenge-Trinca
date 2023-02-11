using Challenge.Trinca.Application.UseCases.Bbqs.Commands.CreateBbq;
using Challenge.Trinca.Presentation.Common.Endpoint;
using Challenge.Trinca.Presentation.Endpoints.Bbqs.Common;
using Challenge.Trinca.Presentation.Endpoints.Common.Extensions;
using MediatR;
using IMapper = MapsterMapper.IMapper;

namespace Challenge.Trinca.Presentation.Endpoints.Bbqs.CreateBbq;

public sealed class CreateBbqEndpoint : ApiEndpoint<CreateBbqRequest>
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public CreateBbqEndpoint(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Post(BbqEndpointConfiguration.CreateRoute);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateBbqRequest req, CancellationToken ct)
    {
        var createBbqCommand = _mapper.Map<CreateBbqCommand>(req);

        var createBbqResult = await _mediator.Send(createBbqCommand, ct);

        await createBbqResult.Match(
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
