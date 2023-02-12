using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.UseCases.Bbqs.Events;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects.Enums;
using Challenge.Trinca.Domain.DomainEvents.Bbqs;
using Challenge.Trinca.Domain.Repositories;
using Challenge.Trinca.Tests.Unit.BaseFixtures;
using Moq;
using Serilog;

namespace Challenge.Trinca.Tests.Unit.Applications.UseCases.Bbqs.Events.BbqCreated;

public sealed class BbqCreatedDomainEventHandlerTests
{
    private readonly BbqCreatedDomainEventHandler _sut;
    private readonly Mock<IPeopleRepository> _peopleRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<ILogger> _logger = new();

    public BbqCreatedDomainEventHandlerTests()
    {
        _sut = new BbqCreatedDomainEventHandler(_peopleRepository.Object, _unitOfWork.Object, _logger.Object);
    }

    [Fact(DisplayName = "Handle() should add invite to all co-owners")]
    [Trait("Application - Event", "BbqCreated - Handler")]
    public async Task Handle_ShouldAddInviteToAllCoOwners()
    {
        // Arrange
        var coOwnerPeopleList = Enumerable.Range(1, 20)
            .Select(x => CommonPeopleFixture.GetCoOwnerPeople())
            .ToList();

        var bbqExample = CommonBbqFixture.GetBbq();

        var bbqCreatedDomainEvent = new BbqCreatedDomainEvent(bbqExample.Id);

        _peopleRepository.Setup(x => x.GetAllCoOwnersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(coOwnerPeopleList);

        // Act
        await _sut.Handle(bbqCreatedDomainEvent, default);

        // Assert 
        _peopleRepository.Verify(
            x => x.GetAllCoOwnersAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        _peopleRepository.Verify(
            x => x.UpdateAsync(It.IsAny<People>()),
            Times.Exactly(coOwnerPeopleList.Count));

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        coOwnerPeopleList.ToList().ForEach(people =>
        {
            people.Invites.Should().ContainSingle();
            people.Invites.First().BbqId.Should().Be(bbqExample.Id);
            people.Invites.First().Status.Should().Be(InviteStatus.Pending);
        });
    }
}
