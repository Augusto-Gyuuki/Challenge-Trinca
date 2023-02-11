using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects.Enums;

namespace Challenge.Trinca.Tests.Unit.Domains.Entities.PeopleAggregateRoot;

public sealed class InviteStatusDataGenerator
{
    public static TheoryData<int, InviteStatus> GetInviteStatusByValues()
    {
        return new TheoryData<int, InviteStatus>
        {
            { 1, InviteStatus.Pending},
            { 2, InviteStatus.Accepted},
            { 3, InviteStatus.Declined},
        };
    }

    public static TheoryData<string, InviteStatus> GetInviteStatusByName()
    {
        return new TheoryData<string, InviteStatus>
        {
            { nameof(InviteStatus.Pending), InviteStatus.Pending},
            { nameof(InviteStatus.Accepted), InviteStatus.Accepted},
            { nameof(InviteStatus.Declined), InviteStatus.Declined},
        };
    }

    public static TheoryData<InviteStatus, int> GetInviteStatusAndExpectedValues()
    {
        return new TheoryData<InviteStatus, int>
        {
            { InviteStatus.Pending, 1 },
            { InviteStatus.Accepted, 2 },
            { InviteStatus.Declined, 3 },
        };
    }
}
