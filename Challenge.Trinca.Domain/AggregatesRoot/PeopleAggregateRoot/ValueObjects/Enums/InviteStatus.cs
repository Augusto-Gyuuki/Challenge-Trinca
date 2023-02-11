using Challenge.Trinca.Domain.Common.ValueObjects;

namespace Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects.Enums;

public sealed class InviteStatus : Enumeration<InviteStatus>
{
    private InviteStatus(int value, string name) : base(value, name)
    {
    }

    public static readonly InviteStatus Pending = new(1, nameof(Pending));
    public static readonly InviteStatus Accepted = new(2, nameof(Accepted));
    public static readonly InviteStatus Declined = new(3, nameof(Declined));

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
