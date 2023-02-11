using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.UseCases.Peoples.Queries.GetPeopleInvites;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.Errors;
using Challenge.Trinca.Tests.Unit.BaseFixtures;
using Moq;

namespace Challenge.Trinca.Tests.Unit.Applications.UseCases.Peoples.Queries.GetPeopleInvites;

public sealed class GetPeopleInvitesQueryHandlerTests
{
    private readonly GetPeopleInvitesQueryHandler _sut;
    private readonly Mock<IPeopleRepository> _peopleRepository = new();
    private readonly Mock<IBbqRepository> _bbqRepository = new();

    public GetPeopleInvitesQueryHandlerTests()
    {
        _sut = new GetPeopleInvitesQueryHandler(_peopleRepository.Object, _bbqRepository.Object);
    }

    [Fact(DisplayName = "Handle() should return people when valid query is given")]
    [Trait("Application", "GetPeopleInvites - Handler")]
    public async Task Handle_ShouldReturnValidResult()
    {
        // Arrange
        var getPeopleInvitesQuery = GetPeopleInvitesQueryFixture.GetValidGetPeopleInvitesQuery();
        var peopleExample = CommonPeopleFixture.GetPeople();
        var inviteExample = CommonPeopleFixture.GetInvite();
        var bbqExample = CommonBbqFixture.GetBbq();
        peopleExample.Invite(inviteExample);

        _peopleRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(peopleExample);

        _bbqRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(bbqExample);

        // Act
        var queryResult = await _sut.Handle(getPeopleInvitesQuery, default);

        // Assert 
        _peopleRepository.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _bbqRepository.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);

        queryResult.IsError.Should().BeFalse();
        queryResult.Value.Invites.Should().HaveCount(peopleExample.Invites.Count);
        queryResult.Value.Invites.First().BbqId.Should().Be(inviteExample.BbqId);
        queryResult.Value.Invites.First().Id.Should().Be(inviteExample.Id);
        queryResult.Value.Invites.First().Status.Should().Be(inviteExample.Status.Name);
        queryResult.Value.Invites.First().Date.Should().Be(inviteExample.Date);
    }

    [Fact(DisplayName = "Handle() should return PeopleNotFoundError")]
    [Trait("Application", "GetPeopleInvites - Handler")]
    public async Task Handle_ShouldReturnPeopleNotFoundError()
    {
        // Arrange
        var getPeopleInvitesQuery = GetPeopleInvitesQueryFixture.GetValidGetPeopleInvitesQuery();
        var peopleExample = CommonPeopleFixture.GetPeople();

        // Act
        var queryResult = await _sut.Handle(getPeopleInvitesQuery, default);

        // Assert 
        _peopleRepository.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);

        queryResult.IsError.Should().BeTrue();
        queryResult.FirstError.Should().Be(PeopleErrors.PeopleNotFound);
    }
}
