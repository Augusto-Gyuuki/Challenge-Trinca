using Challenge.Trinca.Application.UseCases.Peoples.Common.Results;
using ErrorOr;
using MediatR;

namespace Challenge.Trinca.Application.UseCases.Peoples.Commands.ConfirmInvite;

public sealed record ConfirmInviteCommand : IRequest<ErrorOr<InviteModelResult>>
{
    public string PeopleId { get; init; } = string.Empty;

    public string InviteId { get; init; } = string.Empty;

    public bool IsVeg { get; init; }
}
