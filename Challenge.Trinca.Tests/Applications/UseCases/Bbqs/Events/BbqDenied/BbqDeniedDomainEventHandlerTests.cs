using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.UseCases.Bbqs.Events;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects.Enums;
using Challenge.Trinca.Domain.DomainEvents.Bbqs;
using Challenge.Trinca.Domain.Repositories;
using Challenge.Trinca.Tests.Unit.BaseFixtures;
using Moq;
using Serilog;

namespace Challenge.Trinca.Tests.Unit.Applications.UseCases.Bbqs.Events.BbqDenied;

public sealed class BbqDeniedDomainEventHandlerTests
{
    private readonly BbqDeniedDomainEventHandler _sut;
    private readonly Mock<IPeopleRepository> _peopleRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<ILogger> _logger = new();

    public BbqDeniedDomainEventHandlerTests()
    {
        _sut = new BbqDeniedDomainEventHandler(_peopleRepository.Object, _unitOfWork.Object, _logger.Object);
    }

    [Fact(DisplayName = "Handle() should decline all remaining invitations")]
    [Trait("Application - Event", "BbqDenied - Handler")]
    public async Task Handle_ShouldDeclineAllRemainingInvitations()
    {
        // Arrange
        var peopleList = Enumerable.Range(1, 20)
            .SelectMany(x => CommonPeopleFixture.GetRandomPeopleList())
            .ToList();

        var bbqExample = CommonBbqFixture.GetBbq();

        peopleList.ForEach(x => x.Invite(Invite.Create(bbqExample.Id)));

        var bbqDeniedDomainEvent = new BbqDeniedDomainEvent(bbqExample.Id);

        _peopleRepository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(peopleList);

        // Act
        await _sut.Handle(bbqDeniedDomainEvent, default);

        // Assert 
        _peopleRepository.Verify(
            x => x.GetAllAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        _peopleRepository.Verify(
            x => x.UpdateAsync(It.IsAny<People>()),
            Times.Exactly(peopleList.Count));

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        peopleList.ForEach(people =>
        {
            people.Invites.Should().ContainSingle();
            people.Invites.First().Status.Should().Be(InviteStatus.Declined);
        });
    }

    [Fact(DisplayName = "Handle() shouldn't update people")]
    [Trait("Application - Event", "BbqDenied - Handler")]
    public async Task Handle_ShouldntUpdatePeople()
    {
        // Arrange
        var bbqExample = CommonBbqFixture.GetBbq();

        var peopleList = Enumerable.Range(1, 20)
            .SelectMany(x => CommonPeopleFixture.GetRandomPeopleList())
            .ToList();

        var bbqDeniedDomainEvent = new BbqDeniedDomainEvent(bbqExample.Id);

        _peopleRepository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(peopleList);

        // Act
        await _sut.Handle(bbqDeniedDomainEvent, default);

        // Assert 
        _peopleRepository.Verify(
            x => x.GetAllAsync(It.IsAny<CancellationToken>()),
            Times.Once());

        _peopleRepository.Verify(
            x => x.UpdateAsync(It.IsAny<People>()),
            Times.Never());

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
