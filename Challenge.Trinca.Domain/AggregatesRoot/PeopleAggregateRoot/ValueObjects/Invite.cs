using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects.Enums;
using Challenge.Trinca.Domain.Common.Models;

namespace Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects;

public sealed class Invite : ValueObject
{
    public Guid Id { get; private set; }

    public Guid BbqId { get; private set; }

    public InviteStatus Status { get; private set; }

    public DateTime Date { get; private set; }

    private Invite() { }

    private Invite(
        Guid inviteId,
        Guid bbqId,
        DateTime date,
        InviteStatus status)
    {
        Id = inviteId;
        BbqId = bbqId;
        Date = date;
        Status = status;
    }

    public void AcceptInvite()
    {
        Status = InviteStatus.Accepted;
    }

    public void DeclineInvite()
    {
        Status = InviteStatus.Declined;
    }

    public static Invite Create(Guid bbqId)
    {
        return new(
            Guid.NewGuid(),
            bbqId,
            DateTime.UtcNow,
            InviteStatus.Pending);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }
}
