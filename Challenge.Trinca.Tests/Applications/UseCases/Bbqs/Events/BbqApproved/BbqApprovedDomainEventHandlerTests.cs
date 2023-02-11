using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.UseCases.Bbqs.Events;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects.Enums;
using Challenge.Trinca.Domain.DomainEvents.Bbqs;
using Challenge.Trinca.Domain.Repositories;
using Challenge.Trinca.Tests.Unit.BaseFixtures;
using Moq;

namespace Challenge.Trinca.Tests.Unit.Applications.UseCases.Bbqs.Events.BbqApproved;

public sealed class BbqApprovedDomainEventHandlerTests
{
    private readonly BbqApprovedDomainEventHandler _sut;
    private readonly Mock<IBbqRepository> _bbqRepository = new();
    private readonly Mock<IPeopleRepository> _peopleRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    public BbqApprovedDomainEventHandlerTests()
    {
        _sut = new BbqApprovedDomainEventHandler(_peopleRepository.Object, _unitOfWork.Object, _bbqRepository.Object);
    }

    [Fact(DisplayName = "Handle() should add invite to all workers")]
    [Trait("Application - Event", "BbqApproved - Handler")]
    public async Task Handle_ShouldAddInviteToAllWorkers()
    {
        // Arrange
        var peopleList = Enumerable.Range(1, 20)
            .SelectMany(x => CommonPeopleFixture.GetRandomPeopleList())
            .ToList();

        var bbqExample = CommonBbqFixture.GetBbq();

        var bbqApprovedDomainEvent = new BbqApprovedDomainEvent(bbqExample.Id);

        _bbqRepository.Setup(x => x.GetByIdAsync(bbqExample.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(bbqExample);

        _peopleRepository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(peopleList);

        // Act
        await _sut.Handle(bbqApprovedDomainEvent, default);

        // Assert 
        _peopleRepository.Verify(
            x => x.GetAllAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        _peopleRepository.Verify(
            x => x.UpdateAsync(It.IsAny<People>()),
            Times.Exactly(peopleList.Where(x => !x.IsCoOwner).Count()));

        _bbqRepository.Verify(
            x => x.GetByIdAsync(bbqExample.Id, It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        bbqExample.Guests.Count.Should().Be(peopleList.Count);

        peopleList.Where(x => !x.IsCoOwner).ToList().ForEach(people =>
        {
            people.Invites.Should().ContainSingle();
            people.Invites.First().BbqId.Should().Be(bbqExample.Id);
            people.Invites.First().Status.Should().Be(InviteStatus.Pending);
        });
    }

    [Fact(DisplayName = "Handle() should throw exception when bbq was not found")]
    [Trait("Application - Event", "BbqApproved - Handler")]
    public async Task Handle_ThrowArgumentNullExceptionWhenBbqWasNotFound()
    {
        // Arrange
        var peopleExample = CommonPeopleFixture.GetPeople();
        var bbqExample = CommonBbqFixture.GetBbq();

        var bbqApprovedDomainEvent = new BbqApprovedDomainEvent(bbqExample.Id);

        _bbqRepository.Setup(x => x.GetByIdAsync(bbqExample.Id, It.IsAny<CancellationToken>()));

        // Act
        var action = async () => await _sut.Handle(bbqApprovedDomainEvent, default);

        // Assert 
        await action.Should().ThrowAsync<ArgumentNullException>()
                .WithParameterName(nameof(Bbq), BbqErrors.BbqNotFound.Description);
    }
}

