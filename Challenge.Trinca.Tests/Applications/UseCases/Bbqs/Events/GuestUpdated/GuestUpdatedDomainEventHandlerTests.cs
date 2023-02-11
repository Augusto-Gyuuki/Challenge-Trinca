using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.UseCases.Bbqs.Events;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.ValueObjects.Enums;
using Challenge.Trinca.Domain.DomainEvents.Bbqs;
using Challenge.Trinca.Domain.Repositories;
using Challenge.Trinca.Tests.Unit.BaseFixtures;
using Moq;

namespace Challenge.Trinca.Tests.Unit.Applications.UseCases.Bbqs.Events.GuestUpdated;

public sealed class GuestUpdatedDomainEventHandlerTests
{
    private readonly GuestUpdatedDomainEventHandler _sut;
    private readonly Mock<IBbqRepository> _bbqRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    public GuestUpdatedDomainEventHandlerTests()
    {
        _sut = new GuestUpdatedDomainEventHandler(_unitOfWork.Object, _bbqRepository.Object);
    }

    [Fact(DisplayName = "Handle() should update the bbqStatus to BbqStatus.Confirmed")]
    [Trait("Application - Event", "GuestUpdated - Handler")]
    public async Task Handle_ShouldUpdateBbqStatusToConfirmed()
    {
        // Arrange
        var guestList = Enumerable.Range(1, 20)
            .Select(x => CommonBbqFixture.GetGuest())
            .ToList();

        var bbqExample = CommonBbqFixture.GetBbq();
        guestList.ForEach(bbqExample.AddGuest);
        guestList.ForEach(bbqExample.ConfirmGuest);
        bbqExample.ChangeStatusToPendingConfirmations();

        var guestUpdatedDomainEvent = new GuestUpdatedDomainEvent(bbqExample.Id);

        _bbqRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(bbqExample);

        // Act
        await _sut.Handle(guestUpdatedDomainEvent, default);

        // Assert 
        _bbqRepository.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _bbqRepository.Verify(
            x => x.UpdateAsync(It.IsAny<Bbq>()),
            Times.Once);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        bbqExample.Status.Should().Be(BbqStatus.Confirmed);
    }

    [Fact(DisplayName = "Handle() should update the bbqStatus to BbqStatus.PendingConfirmations")]
    [Trait("Application - Event", "GuestUpdated - Handler")]
    public async Task Handle_ShouldUpdateBbqStatusToPendingConfirmations()
    {
        // Arrange
        var guestList = Enumerable.Range(1, 6)
            .Select(x => CommonBbqFixture.GetGuest())
            .ToList();

        var bbqExample = CommonBbqFixture.GetBbq();
        guestList.ForEach(bbqExample.AddGuest);
        guestList.ForEach(bbqExample.ConfirmGuest);
        bbqExample.ChangeStatusToConfirmed();

        var guestUpdatedDomainEvent = new GuestUpdatedDomainEvent(bbqExample.Id);

        _bbqRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(bbqExample);

        // Act
        await _sut.Handle(guestUpdatedDomainEvent, default);

        // Assert 
        _bbqRepository.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _bbqRepository.Verify(
            x => x.UpdateAsync(It.IsAny<Bbq>()),
            Times.Once);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        bbqExample.Status.Should().Be(BbqStatus.PendingConfirmations);
    }

    [Fact(DisplayName = "Handle() should throw exception when bbq was not found")]
    [Trait("Application - Event", "GuestUpdated - Handler")]
    public async Task Handle_ThrowArgumentNullExceptionWhenBbqWasNotFound()
    {
        // Arrange
        var bbqExample = CommonBbqFixture.GetBbq();

        var guestUpdatedDomainEvent = new GuestUpdatedDomainEvent(bbqExample.Id);

        _bbqRepository.Setup(x => x.GetByIdAsync(bbqExample.Id, It.IsAny<CancellationToken>()));

        // Act
        var action = async () => await _sut.Handle(guestUpdatedDomainEvent, default);

        // Assert 
        await action.Should().ThrowAsync<ArgumentNullException>()
                .WithParameterName(nameof(Bbq), BbqErrors.BbqNotFound.Description);
    }
}
