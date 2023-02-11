using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects;

namespace Challenge.Trinca.Application.UseCases.Peoples.Common.Results;

public sealed record PeopleModelResult
{
    private PeopleModelResult(
        Guid id,
        string name,
        bool isCoOwner,
        IReadOnlyList<InviteModelResult> invites)
    {
        Id = id;
        Name = name;
        IsCoOwner = isCoOwner;
        Invites = invites;
    }

    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public bool IsCoOwner { get; init; }

    public IReadOnlyList<InviteModelResult> Invites { get; init; }

    public static PeopleModelResult FromPeople(People people, List<Invite> invites)
    {
        return new(
            people.Id,
            people.Name,
            people.IsCoOwner,
            invites.Select(InviteModelResult.FromInvite)
                .ToList());
    }

}