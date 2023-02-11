using Challenge.Trinca.Domain.Common.ValueObjects;

namespace Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.ValueObjects.Enums;

public sealed class BbqStatus : Enumeration<BbqStatus>
{
    private BbqStatus(int value, string name) : base(value, name)
    {
    }

    public static readonly BbqStatus New = new(1, nameof(New));
    public static readonly BbqStatus PendingConfirmations = new(2, nameof(PendingConfirmations));
    public static readonly BbqStatus Confirmed = new(3, nameof(Confirmed));
    public static readonly BbqStatus ItsNotGonnaHappen = new(4, nameof(ItsNotGonnaHappen));

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
