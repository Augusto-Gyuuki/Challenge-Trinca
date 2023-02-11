using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.UseCases.Peoples.Commands.ConfirmInvite;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects.Enums;
using Challenge.Trinca.Domain.Repositories;
using Challenge.Trinca.Tests.Unit.BaseFixtures;
using Moq;

namespace Challenge.Trinca.Tests.Unit.Applications.UseCases.Peoples.Commands.ConfirmInvite;

public sealed class ConfirmInviteCommandHandlerTests
{
    private readonly ConfirmInviteCommandHandler _sut;
    private readonly Mock<IPeopleRepository> _peopleRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    public ConfirmInviteCommandHandlerTests()
    {
        _sut = new ConfirmInviteCommandHandler(_peopleRepository.Object, _unitOfWork.Object);
    }

    [Fact(DisplayName = "Handle() should accept people invite")]
    [Trait("Application", "ConfirmInvite - Handler")]
    public async Task Handle_ShouldReturnValidResult()
    {
        // Arrange
        var peopleExample = CommonPeopleFixture.GetPeople();
        var inviteExample = CommonPeopleFixture.GetInvite();
        peopleExample.Invite(inviteExample);

        var confirmInviteCommand = new ConfirmInviteCommand
        {
            PersonId = peopleExample.Id.ToString(),
            InviteId = inviteExample.Id.ToString(),
            IsVeg = CommonPeopleFixture.GetRandomBool()
        };

        _peopleRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(peopleExample);

        // Act
        var commandResult = await _sut.Handle(confirmInviteCommand, default);

        // Assert 
        _peopleRepository.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        commandResult.IsError.Should().BeFalse();
        commandResult.Value.BbqId.Should().Be(inviteExample.BbqId);
        commandResult.Value.Id.Should().Be(inviteExample.Id);
        commandResult.Value.Status.Should().Be(InviteStatus.Accepted.Name);
        commandResult.Value.Date.Should().Be(inviteExample.Date);
    }

    [Fact(DisplayName = "Handle() should return PeopleNotFoundError")]
    [Trait("Application", "ConfirmInvite - Handler")]
    public async Task Handle_ShouldReturnPeopleNotFoundError()
    {
        // Arrange
        var declineInviteCommand = ConfirmInviteCommandFixture.GetValidConfirmInviteCommand();
        var peopleExample = CommonPeopleFixture.GetPeople();

        // Act
        var commandResult = await _sut.Handle(declineInviteCommand, default);

        // Assert 
        _peopleRepository.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);

        commandResult.IsError.Should().BeTrue();
        commandResult.FirstError.Should().Be(PeopleErrors.PeopleNotFound);
    }

    [Fact(DisplayName = "Handle() should return InviteNotFoundError")]
    [Trait("Application", "ConfirmInvite - Handler")]
    public async Task Handle_ShouldReturnInviteNotFoundError()
    {
        // Arrange
        var peopleExample = CommonPeopleFixture.GetPeople();
        var inviteExample = CommonPeopleFixture.GetInvite();
        peopleExample.Invite(inviteExample);

        var declineInviteCommand = new ConfirmInviteCommand
        {
            PersonId = peopleExample.Id.ToString(),
            InviteId = CommonPeopleFixture.GetPeopleId().ToString(),
            IsVeg = CommonPeopleFixture.GetRandomBool()
        };

        _peopleRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(peopleExample);

        // Act
        var commandResult = await _sut.Handle(declineInviteCommand, default);

        // Assert 
        _peopleRepository.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);

        commandResult.IsError.Should().BeTrue();
        commandResult.FirstError.Should().Be(PeopleErrors.InviteNotFound);
    }
}
