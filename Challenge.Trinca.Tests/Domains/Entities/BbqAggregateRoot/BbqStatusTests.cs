using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.ValueObjects.Enums;

namespace Challenge.Trinca.Tests.Unit.Domains.Entities.BbqAggregateRoot;

public sealed class BbqStatusTests
{
    [Theory(DisplayName = "BbqStatus.FromValue() should return expected status when getting from value")]
    [Trait("Domain", "BbqStatus - FromValue")]
    [MemberData(nameof(BbqStatusDataGenerator.GetBbqStatusValues), MemberType = typeof(BbqStatusDataGenerator))]
    public void FromValue_ReturnExpectedStatusFromValue(int enumValue, BbqStatus expectedBbqStatus)
    {
        // Act
        var bbqStatus = BbqStatus.FromValue(enumValue);

        // Assert 
        bbqStatus.Should().BeEquivalentTo(expectedBbqStatus);
    }

    [Theory(DisplayName = "BbqStatus.FromName() should return expected status when getting from name")]
    [Trait("Domain", "BbqStatus - FromName")]
    [MemberData(nameof(BbqStatusDataGenerator.GetBbqStatusName), MemberType = typeof(BbqStatusDataGenerator))]
    public void FromName_ReturnExpectedStatusFromName(string enumName, BbqStatus expectedBbqStatus)
    {
        // Act
        var bbqStatus = BbqStatus.FromName(enumName);

        // Assert 
        bbqStatus.Should().BeEquivalentTo(expectedBbqStatus);
    }

    [Theory(DisplayName = "BbqStatus.GetEqualityComponents() should return expected value")]
    [Trait("Domain", "BbqStatus - GetEqualityComponents")]
    [MemberData(nameof(BbqStatusDataGenerator.GetBbqStatusAndExpectedValues), MemberType = typeof(BbqStatusDataGenerator))]
    public void GetEqualityComponents_ReturnExpectedValue(BbqStatus bbqStatus, int enumValue)
    {
        // Act
        var value = bbqStatus.GetEqualityComponents();

        // Assert 
        value.First().Should().Be(enumValue);
    }
}
