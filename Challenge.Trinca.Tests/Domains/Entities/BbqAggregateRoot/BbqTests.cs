using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.ValueObjects.Enums;
using Challenge.Trinca.Domain.Common.Exceptions;
using Challenge.Trinca.Tests.Unit.BaseFixtures;

namespace Challenge.Trinca.Tests.Unit.Domains.Entities.BbqAggregateRoot;

public sealed class BbqTests
{
    [Fact(DisplayName = "Bbq.Create() should return new bbq when valid parameters is given")]
    [Trait("Domain", "Bbq - Create")]
    public void Create_ReturnValidBbq()
    {
        // Arrange
        var exampleBbq = CommonBbqFixture.GetBbq();

        // Act
        var bbq = Bbq.Create(exampleBbq.Reason, exampleBbq.Date, exampleBbq.IsTrincaPaying);

        // Assert 
        bbq.Should().NotBeNull();
        bbq.Reason.Should().Be(exampleBbq.Reason);
        bbq.Status.Should().Be(exampleBbq.Status);
        bbq.Date.Should().Be(exampleBbq.Date);
        bbq.Id.Should().NotBeEmpty();
        bbq.IsTrincaPaying.Should().Be(exampleBbq.IsTrincaPaying);
        bbq.Guests.Should().BeEmpty();
        bbq.GetDomainEvents().Should().ContainSingle();
        bbq.UpdatedDateTime.Should().BeCloseTo(exampleBbq.UpdatedDateTime, TimeSpan.FromSeconds(2));
        bbq.CreatedDateTime.Should().BeCloseTo(exampleBbq.CreatedDateTime, TimeSpan.FromSeconds(2));
    }

    [Fact(DisplayName = "Bbq.Create() should throw ReasonNullOrWhiteSpace error when \"reason\" is null")]
    [Trait("Domain", "Bbq - Create")]
    public void Create_ThrowReasonNullOrWhiteSpaceErrorWhenCreating()
    {
        // Arrange
        var bbqInvalidReason = CommonBbqFixture.GetNullOrWhiteSpaceReason();
        var bbqInvalidDate = CommonBbqFixture.GetInvalidDate();

        // Act
        var action = () => Bbq.Create(bbqInvalidReason, bbqInvalidDate);

        // Assert 
        action.Should()
            .ThrowExactly<EntityValidationException>()
            .WithMessage(BbqErrors.ReasonNullOrWhiteSpace.Description);
    }

    [Fact(DisplayName = "Bbq.Create() should throw ReasonMaxLength error when \"reason\" is null")]
    [Trait("Domain", "Bbq - Create")]
    public void Create_ThrowReasonMaxLengthErrorWhenCreating()
    {
        // Arrange
        var bbqInvalidReason = CommonBbqFixture.GetInvalidMaxLengthReason();
        var bbqInvalidDate = CommonBbqFixture.GetInvalidDate();

        // Act
        var action = () => Bbq.Create(bbqInvalidReason, bbqInvalidDate);

        // Assert 
        action.Should()
            .ThrowExactly<EntityValidationException>()
            .WithMessage(BbqErrors.ReasonMaxLength.Description);
    }

    [Fact(DisplayName = "Bbq.Create() should throw DateInvalid error when \"reason\" is null")]
    [Trait("Domain", "Bbq - Create")]
    public void Create_ThrowDateInvalidErrorWhenCreating()
    {
        // Arrange
        var bbqValidReason = CommonBbqFixture.GetValidReason();
        var bbqInvalidDate = CommonBbqFixture.GetInvalidDate();

        // Act
        var action = () => Bbq.Create(bbqValidReason, bbqInvalidDate);

        // Assert 
        action.Should()
            .ThrowExactly<EntityValidationException>()
            .WithMessage(BbqErrors.DateInvalid.Description);
    }

    [Fact(DisplayName = "Bbq.DenyBbq() should update the status to BbqStatus.ItsNotGonnaHappen")]
    [Trait("Domain", "Bbq - DenyBbq")]
    public void DenyBbq_ShouldUpdateStatusToItsNotGonnaHappen()
    {
        // Arrange
        var exampleBbq = CommonBbqFixture.GetBbq();

        // Act
        exampleBbq.DenyBbq();

        // Assert 
        exampleBbq.Status.Should().Be(BbqStatus.ItsNotGonnaHappen);
        exampleBbq.GetDomainEvents().Should().HaveCount(2);
        exampleBbq.UpdatedDateTime.Should().BeAfter(exampleBbq.CreatedDateTime);
        exampleBbq.UpdatedDateTime.Should().BeCloseTo(exampleBbq.UpdatedDateTime, TimeSpan.FromSeconds(2));
    }

    [Fact(DisplayName = "Bbq.ConfirmBbq() should update the status to BbqStatus.Confirmed")]
    [Trait("Domain", "Bbq - ConfirmBbq")]
    public void ConfirmBbq_ShouldUpdateStatusToConfirmed()
    {
        // Arrange
        var exampleBbq = CommonBbqFixture.GetBbq();

        // Act
        exampleBbq.ChangeStatusToPendingConfirmations();

        // Assert 
        exampleBbq.Status.Should().Be(BbqStatus.PendingConfirmations);
        exampleBbq.UpdatedDateTime.Should().BeAfter(exampleBbq.CreatedDateTime);
        exampleBbq.UpdatedDateTime.Should().BeCloseTo(exampleBbq.UpdatedDateTime, TimeSpan.FromSeconds(2));
    }

    [Fact(DisplayName = "Bbq.Update() should update bbq date and reason")]
    [Trait("Domain", "Bbq - Update")]
    public void Update_ShouldUpdateReasonAndDate()
    {
        // Arrange
        var exampleBbq = CommonBbqFixture.GetBbq();
        var bbqValidReason = CommonBbqFixture.GetValidReason();
        var bbqValidDate = CommonBbqFixture.GetValidDate();

        // Act
        exampleBbq.Update(bbqValidReason, bbqValidDate);

        // Assert 
        exampleBbq.Should().NotBeNull();
        exampleBbq.Reason.Should().Be(bbqValidReason);
        exampleBbq.Date.Should().Be(bbqValidDate);
        exampleBbq.UpdatedDateTime.Should().BeAfter(exampleBbq.CreatedDateTime);
        exampleBbq.UpdatedDateTime.Should().BeCloseTo(exampleBbq.UpdatedDateTime, TimeSpan.FromSeconds(2));
    }

    [Fact(DisplayName = "Bbq.Update() should update bbq date and maintain reason")]
    [Trait("Domain", "Bbq - Update")]
    public void Update_ShouldUpdateDate()
    {
        // Arrange
        var exampleBbq = CommonBbqFixture.GetBbq();
        var bbqValidReason = CommonBbqFixture.GetValidReason();

        var previousDate = exampleBbq.Date;

        // Act
        exampleBbq.Update(bbqValidReason);

        // Assert 
        exampleBbq.Should().NotBeNull();
        exampleBbq.Reason.Should().Be(bbqValidReason);
        exampleBbq.Date.Should().Be(previousDate);
        exampleBbq.UpdatedDateTime.Should().BeAfter(exampleBbq.CreatedDateTime);
        exampleBbq.UpdatedDateTime.Should().BeCloseTo(exampleBbq.UpdatedDateTime, TimeSpan.FromSeconds(2));
    }

    [Fact(DisplayName = "Bbq.Update() should update bbq reason and maintain date")]
    [Trait("Domain", "Bbq - Update")]
    public void Update_ShouldUpdateReason()
    {
        // Arrange
        var exampleBbq = CommonBbqFixture.GetBbq();
        var bbqValidDate = CommonBbqFixture.GetValidDate();

        var previousReason = exampleBbq.Reason;

        // Act
        exampleBbq.Update(previousReason, bbqValidDate);

        // Assert 
        exampleBbq.Should().NotBeNull();
        exampleBbq.Reason.Should().Be(previousReason);
        exampleBbq.Date.Should().Be(bbqValidDate);
        exampleBbq.UpdatedDateTime.Should().BeAfter(exampleBbq.CreatedDateTime);
        exampleBbq.UpdatedDateTime.Should().BeCloseTo(exampleBbq.UpdatedDateTime, TimeSpan.FromSeconds(2));
    }

    [Fact(DisplayName = "Bbq.Update() should throw ReasonNullOrWhiteSpace error when \"reason\" is null")]
    [Trait("Domain", "Bbq - Update")]
    public void Update_ThrowReasonNullOrWhiteSpaceError()
    {
        // Arrange
        var exampleBbq = CommonBbqFixture.GetBbq();
        var bbqInvalidReason = CommonBbqFixture.GetNullOrWhiteSpaceReason();

        // Act
        var action = () => exampleBbq.Update(bbqInvalidReason);

        // Assert 
        action.Should()
            .ThrowExactly<EntityValidationException>()
            .WithMessage(BbqErrors.ReasonNullOrWhiteSpace.Description);
    }

    [Fact(DisplayName = "Bbq.Update() should throw ReasonMaxLength error when \"reason\" is null")]
    [Trait("Domain", "Bbq - Update")]
    public void Update_ThrowReasonMaxLengthError()
    {
        // Arrange
        var exampleBbq = CommonBbqFixture.GetBbq();
        var bbqInvalidReason = CommonBbqFixture.GetInvalidMaxLengthReason();

        // Act
        var action = () => exampleBbq.Update(bbqInvalidReason);

        // Assert 
        action.Should()
            .ThrowExactly<EntityValidationException>()
            .WithMessage(BbqErrors.ReasonMaxLength.Description);
    }

    [Fact(DisplayName = "Bbq.Update() should throw DateInvalid error when \"reason\" is null")]
    [Trait("Domain", "Bbq - Update")]
    public void Update_ThrowDateInvalidError()
    {
        // Arrange
        var exampleBbq = CommonBbqFixture.GetBbq();
        var bbqInvalidDate = CommonBbqFixture.GetInvalidDate();

        // Act
        var action = () => exampleBbq.Update(exampleBbq.Reason, bbqInvalidDate);

        // Assert 
        action.Should()
            .ThrowExactly<EntityValidationException>()
            .WithMessage(BbqErrors.DateInvalid.Description);
    }

    [Fact(DisplayName = "Bbq.TrincaWillPay() should update the value of the property IsTrincaPaying to true")]
    [Trait("Domain", "Bbq - TrincaWillPay")]
    public void TrincaWillPay_ShouldUpdateTheValueToFalse()
    {
        // Arrange
        var exampleBbq = CommonBbqFixture.GetBbq();
        exampleBbq.ClearDomainEvents();

        // Act
        exampleBbq.TrincaWillPay();

        // Assert 
        exampleBbq.Should().NotBeNull();
        exampleBbq.GetDomainEvents().Should().BeEmpty();
        exampleBbq.IsTrincaPaying.Should().BeTrue();
        exampleBbq.UpdatedDateTime.Should().BeAfter(exampleBbq.CreatedDateTime);
        exampleBbq.UpdatedDateTime.Should().BeCloseTo(exampleBbq.UpdatedDateTime, TimeSpan.FromSeconds(2));
    }

    [Fact(DisplayName = "Bbq.TrincaWillNotPay() should update the value of the property IsTrincaPaying to false")]
    [Trait("Domain", "Bbq - TrincaWillNotPay")]
    public void TrincaWillNotPay_ShouldUpdateTheValueToFalse()
    {
        // Arrange
        var exampleBbq = CommonBbqFixture.GetBbq();
        exampleBbq.ClearDomainEvents();
        exampleBbq.TrincaWillPay();

        // Act
        exampleBbq.TrincaWillNotPay();

        // Assert 
        exampleBbq.Should().NotBeNull();
        exampleBbq.GetDomainEvents().Should().BeEmpty();
        exampleBbq.IsTrincaPaying.Should().BeFalse();
        exampleBbq.UpdatedDateTime.Should().BeAfter(exampleBbq.CreatedDateTime);
        exampleBbq.UpdatedDateTime.Should().BeCloseTo(exampleBbq.UpdatedDateTime, TimeSpan.FromSeconds(2));
    }

    [Fact(DisplayName = "Bbq.ChangeStatusToConfirmed() should update the bbq status to BbqStatus.Confirmed")]
    [Trait("Domain", "Bbq - ChangeStatusToConfirmed")]
    public void ChangeStatusToConfirmed_ShouldUpdateBbqStatus()
    {
        // Arrange
        var exampleBbq = CommonBbqFixture.GetBbq();

        // Act
        exampleBbq.ChangeStatusToConfirmed();

        // Assert 
        exampleBbq.Status.Should().Be(BbqStatus.Confirmed);
        exampleBbq.GetDomainEvents().Should().ContainSingle();
        exampleBbq.UpdatedDateTime.Should().BeAfter(exampleBbq.CreatedDateTime);
        exampleBbq.UpdatedDateTime.Should().BeCloseTo(exampleBbq.UpdatedDateTime, TimeSpan.FromSeconds(2));
    }

    [Fact(DisplayName = "Bbq.DeclineGuest() should update the guest IsAttending property to false")]
    [Trait("Domain", "Bbq - DeclineGuest")]
    public void DeclineGuest_ShouldDeclineGuest()
    {
        // Arrange
        var exampleBbq = CommonBbqFixture.GetBbq();
        var exampleGuest = CommonBbqFixture.GetGuest();
        exampleBbq.AddGuest(exampleGuest);

        // Act
        exampleBbq.DeclineGuest(exampleGuest);

        // Assert 
        var guest = exampleBbq.Guests.FirstOrDefault(x => x.PeopleId.Equals(exampleGuest.PeopleId));

        guest.Should().NotBeNull();
        guest.IsAttending.HasValue.Should().BeTrue();
        guest.IsAttending.Should().BeFalse();
        exampleBbq.GetDomainEvents().Should().HaveCount(2);
    }

    [Fact(DisplayName = "Bbq.ConfirmGuest() should update the guest IsAttending property to true")]
    [Trait("Domain", "Bbq - ConfirmGuest")]
    public void ConfirmGuest_ShouldConfirmGuest()
    {
        // Arrange
        var exampleBbq = CommonBbqFixture.GetBbq();
        var exampleGuest = CommonBbqFixture.GetGuest();
        exampleBbq.AddGuest(exampleGuest);

        // Act
        exampleBbq.ConfirmGuest(exampleGuest);

        // Assert 
        var guest = exampleBbq.Guests.FirstOrDefault(x => x.PeopleId.Equals(exampleGuest.PeopleId));

        guest.Should().NotBeNull();
        guest.IsAttending.HasValue.Should().BeTrue();
        guest.IsAttending.Should().BeTrue();
        exampleBbq.GetDomainEvents().Should().HaveCount(2);
    }
}