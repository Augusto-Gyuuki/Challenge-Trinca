using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.UseCases.Peoples.Events;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.ValueObjects;
using Challenge.Trinca.Domain.DomainEvents.Bbqs;
using Challenge.Trinca.Domain.DomainEvents.Peoples;
using Challenge.Trinca.Domain.Repositories;
using Challenge.Trinca.Tests.Unit.BaseFixtures;
using Moq;

namespace Challenge.Trinca.Tests.Unit.Applications.UseCases.Peoples.Events.InviteAccepted;

public sealed class InviteAcceptedDomainEventHandlerTests
{
    private readonly InviteAcceptedDomainEventHandler _sut;
    private readonly Mock<IBbqRepository> _bbqRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    public InviteAcceptedDomainEventHandlerTests()
    {
        _sut = new InviteAcceptedDomainEventHandler(_unitOfWork.Object, _bbqRepository.Object);
    }

    [Theory(DisplayName = "Handle() should update the value of the guest IsAttending prop to true")]
    [Trait("Application - Event", "InviteAccepted - Handler")]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Handle_UpdateValueIsAttendingToTrue(bool isGuestVegetarian)
    {
        // Arrange
        var peopleExample = CommonPeopleFixture.GetPeople();
        var bbqExample = CommonBbqFixture.GetBbq();
        var guestExample = Guest.Create(peopleExample.Id, !isGuestVegetarian);
        bbqExample.AddGuest(guestExample);
        bbqExample.ClearDomainEvents();

        var inviteAcceptedDomainEvent = new InviteAcceptedDomainEvent(
            isGuestVegetarian,
            peopleExample.Id,
            bbqExample.Id);

        _bbqRepository.Setup(x => x.GetByIdAsync(bbqExample.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(bbqExample);

        // Act
        await _sut.Handle(inviteAcceptedDomainEvent, default);

        // Assert 
        _bbqRepository.Verify(
            x => x.GetByIdAsync(bbqExample.Id, It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        bbqExample.GetDomainEvents().Should().ContainSingle();
        bbqExample.Guests.FirstOrDefault(x => x.PeopleId.Equals(peopleExample.Id)).IsAttending.Should().BeTrue();
        bbqExample.Guests.FirstOrDefault(x => x.PeopleId.Equals(peopleExample.Id)).IsVegetarian.Should().Be(isGuestVegetarian);
        bbqExample.GetDomainEvents().First().Should().BeOfType<GuestUpdatedDomainEvent>();
    }

    [Fact(DisplayName = "Handle() should throw exception when bbq was not found")]
    [Trait("Application - Event", "InviteAccepted - Handler")]
    public async Task Handle_ThrowArgumentNullExceptionWhenBbqWasNotFound()
    {
        // Arrange
        var peopleExample = CommonPeopleFixture.GetPeople();
        var bbqExample = CommonBbqFixture.GetBbq();

        var inviteAcceptedDomainEvent = new InviteAcceptedDomainEvent(
            true,
            peopleExample.Id,
            bbqExample.Id);

        _bbqRepository.Setup(x => x.GetByIdAsync(bbqExample.Id, It.IsAny<CancellationToken>()));

        // Act
        var action = async () => await _sut.Handle(inviteAcceptedDomainEvent, default);

        // Assert 
        await action.Should().ThrowAsync<ArgumentNullException>()
                .WithParameterName(nameof(Bbq), BbqErrors.BbqNotFound.Description);
    }

    [Fact(DisplayName = "Handle() should throw exception when guest was not found")]
    [Trait("Application - Event", "InviteAccepted - Handler")]
    public async Task Handle_ThrowArgumentNullExceptionWhenGuestWasNotFound()
    {
        // Arrange
        var peopleExample = CommonPeopleFixture.GetPeople();
        var bbqExample = CommonBbqFixture.GetBbq();

        var inviteAcceptedDomainEvent = new InviteAcceptedDomainEvent(
            true,
            peopleExample.Id,
            bbqExample.Id);

        _bbqRepository.Setup(x => x.GetByIdAsync(bbqExample.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(bbqExample);

        // Act
        var action = async () => await _sut.Handle(inviteAcceptedDomainEvent, default);

        // Assert 
        await action.Should().ThrowAsync<ArgumentNullException>()
                .WithParameterName(nameof(Guest), BbqErrors.GuestNotFound.Description);
    }
}
