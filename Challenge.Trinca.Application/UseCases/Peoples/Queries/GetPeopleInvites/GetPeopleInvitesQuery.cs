using Challenge.Trinca.Application.UseCases.Peoples.Common.Results;
using ErrorOr;
using MediatR;

namespace Challenge.Trinca.Application.UseCases.Peoples.Queries.GetPeopleInvites;

public sealed record GetPeopleInvitesQuery : IRequest<ErrorOr<PeopleModelResult>>
{
    public string PeopleId { get; set; } = string.Empty;
}
