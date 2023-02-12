using ErrorOr;
using MediatR;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Queries.GetBbq;

public sealed record GetBbqQuery : IRequest<ErrorOr<GetBbqQueryResult>>
{
    public string PeopleId { get; init; } = string.Empty;

    public string BbqId { get; init; } = string.Empty;
}
