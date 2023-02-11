using Challenge.Trinca.Application.UseCases.Peoples.Common.Results;
using ErrorOr;
using MediatR;

namespace Challenge.Trinca.Application.UseCases.Peoples.Commands.DeclineInvite;

public sealed class DeclineInviteCommand : IRequest<ErrorOr<InviteModelResult>>
{
    public string PersonId { get; init; } = string.Empty;

    public string InviteId { get; init; } = string.Empty;

    public bool IsVeg { get; init; }
}
