using Challenge.Trinca.Application.UseCases.Bbqs.Common.Result;
using ErrorOr;
using MediatR;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Commands.CreateBbq;

public sealed record CreateBbqCommand : IRequest<ErrorOr<BbqModelResult>>
{
    public string Reason { get; init; } = string.Empty;

    public DateTime Date { get; init; }

    public bool IsTrincaPaying { get; init; }
}
