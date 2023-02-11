using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects;
using Challenge.Trinca.Domain.Common.Exceptions;
using Challenge.Trinca.Domain.Common.Models;
using Challenge.Trinca.Domain.DomainEvents.Peoples;

namespace Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot;

public sealed class People : AggregateRoot
{
    public const int NAME_MAX_LENGTH = 256;

    private readonly List<Invite> _invites = new();

    public Guid Id { get; set; }

    public string Name { get; set; }

    public bool IsCoOwner { get; set; }

    public IReadOnlyList<Invite> Invites => _invites.AsReadOnly();

    private People() { }

    private People(
        Guid personId,
        string name,
        bool isCoOwner)
    {
        Id = personId;
        Name = name;
        IsCoOwner = isCoOwner;
        UpdatedDateTime = DateTime.UtcNow;
        CreatedDateTime = DateTime.UtcNow;
    }

    public void AcceptInvite(Invite invite, bool isVegetarian)
    {
        invite.AcceptInvite();
        RaiseDomainEvent(new InviteAcceptedDomainEvent(isVegetarian, Id, invite.BbqId));
    }

    public void DeclineInvite(Invite invite)
    {
        invite.DeclineInvite();
        RaiseDomainEvent(new InviteDeclinedDomainEvent(Id, invite.BbqId));
    }

    public static People Create(string name, bool isCoOwner)
    {
        var person = new People(
            Guid.NewGuid(),
            name,
            isCoOwner);

        person.Validate();

        return person;
    }

    public static People CreateWithId(string id, string name, bool isCoOwner)
    {
        var person = new People(
            Guid.Parse(id),
            name,
            isCoOwner);

        person.Validate();

        return person;
    }

    public void Invite(Invite invite)
    {
        _invites.Add(invite);

        Validate();
        UpdatedDateTime = DateTime.UtcNow;
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new EntityValidationException(PeopleErrors.NameNullOrWhiteSpace.Description);
        }

        if (Name.Length > NAME_MAX_LENGTH)
        {
            throw new EntityValidationException(PeopleErrors.NameMaxLength.Description);
        }
    }
}
