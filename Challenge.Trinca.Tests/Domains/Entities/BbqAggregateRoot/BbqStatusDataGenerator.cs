using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.ValueObjects.Enums;

namespace Challenge.Trinca.Tests.Unit.Domains.Entities.BbqAggregateRoot;

public sealed class BbqStatusDataGenerator
{
    public static TheoryData<int, BbqStatus> GetBbqStatusValues()
    {
        return new TheoryData<int, BbqStatus>
        {
            { 1, BbqStatus.New},
            { 2, BbqStatus.PendingConfirmations},
            { 3, BbqStatus.Confirmed},
            { 4, BbqStatus.ItsNotGonnaHappen}
        };
    }

    public static TheoryData<string, BbqStatus> GetBbqStatusName()
    {
        return new TheoryData<string, BbqStatus>
        {
            { nameof(BbqStatus.New), BbqStatus.New},
            { nameof(BbqStatus.PendingConfirmations), BbqStatus.PendingConfirmations},
            { nameof(BbqStatus.Confirmed), BbqStatus.Confirmed},
            { nameof(BbqStatus.ItsNotGonnaHappen), BbqStatus.ItsNotGonnaHappen}
        };
    }

    public static TheoryData<BbqStatus, int> GetBbqStatusAndExpectedValues()
    {
        return new TheoryData<BbqStatus, int>
        {
            { BbqStatus.New, 1 },
            { BbqStatus.PendingConfirmations, 2 },
            { BbqStatus.Confirmed, 3 },
            { BbqStatus.ItsNotGonnaHappen, 4 }
        };
    }
}
