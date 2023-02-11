using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects.Enums;

namespace Challenge.Trinca.Tests.Unit.Domains.Entities.PeopleAggregateRoot;

public sealed class InviteStatusTests
{
    [Theory(DisplayName = "InviteStatus.FromValue() should return expected status when getting from value")]
    [Trait("Domain", "InviteStatus - FromValue")]
    [MemberData(nameof(InviteStatusDataGenerator.GetInviteStatusByValues), MemberType = typeof(InviteStatusDataGenerator))]
    public void FromValue_ReturnExpectedStatusFromValue(int enumValue, InviteStatus expectedInviteStatus)
    {
        // Act
        var inviteStatus = InviteStatus.FromValue(enumValue);

        // Assert 
        inviteStatus.Should().BeEquivalentTo(expectedInviteStatus);
    }

    [Theory(DisplayName = "InviteStatus.FromName() should return expected status when getting from name")]
    [Trait("Domain", "InviteStatus - FromName")]
    [MemberData(nameof(InviteStatusDataGenerator.GetInviteStatusByName), MemberType = typeof(InviteStatusDataGenerator))]
    public void FromName_ReturnExpectedStatusFromName(string enumName, InviteStatus expectedInviteStatus)
    {
        // Act
        var inviteStatus = InviteStatus.FromName(enumName);

        // Assert 
        inviteStatus.Should().BeEquivalentTo(expectedInviteStatus);
    }

    [Theory(DisplayName = "InviteStatus.GetEqualityComponents() should return expected value")]
    [Trait("Domain", "InviteStatus - GetEqualityComponents")]
    [MemberData(nameof(InviteStatusDataGenerator.GetInviteStatusAndExpectedValues), MemberType = typeof(InviteStatusDataGenerator))]
    public void GetEqualityComponents_ReturnExpectedValue(InviteStatus inviteStatus, int enumValue)
    {
        // Act
        var value = inviteStatus.GetEqualityComponents();

        // Assert 
        value.First().Should().Be(enumValue);
    }
}
