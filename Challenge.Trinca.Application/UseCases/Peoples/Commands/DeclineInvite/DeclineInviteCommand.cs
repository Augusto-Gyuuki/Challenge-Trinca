using Challenge.Trinca.Application.UseCases.Peoples.Common.Results;
using ErrorOr;
using MediatR;

namespace Challenge.Trinca.Application.UseCases.Peoples.Commands.DeclineInvite;

public sealed record DeclineInviteCommand : IRequest<ErrorOr<InviteModelResult>>
{
    public string PeopleId { get; init; } = string.Empty;

    public string InviteId { get; init; } = string.Empty;

    public bool IsVeg { get; init; }
}
