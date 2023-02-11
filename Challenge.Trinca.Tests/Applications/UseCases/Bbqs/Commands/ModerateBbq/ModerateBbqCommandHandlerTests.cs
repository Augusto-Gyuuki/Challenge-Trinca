using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.UseCases.Bbqs.Commands.ModerateBbq;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.ValueObjects.Enums;
using Challenge.Trinca.Domain.Repositories;
using Challenge.Trinca.Tests.Unit.BaseFixtures;
using Moq;

namespace Challenge.Trinca.Tests.Unit.Applications.UseCases.Bbqs.Commands.ModerateBbq;

public sealed class ModerateBbqCommandHandlerTests
{
    private readonly ModerateBbqCommandHandler _sut;
    private readonly Mock<IBbqRepository> _bbqRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    public ModerateBbqCommandHandlerTests()
    {
        _sut = new ModerateBbqCommandHandler(_bbqRepository.Object, _unitOfWork.Object);
    }

    [Fact(DisplayName = "Handle() should update the status of the bbq to PendingConfirmations and IsTrincaPaying to true")]
    [Trait("Application", "ModerateBbq - Handler")]
    public async Task Handle_UpdateToPendingConfirmationsAndIsTrincaPayingToTrue()
    {
        // Arrange
        var bbqExample = CommonBbqFixture.GetBbq();

        var moderateBbqCommand = new ModerateBbqCommand
        {
            BbqId = bbqExample.Id.ToString(),
            TrincaWillPay = true,
            GonnaHappen = true,
        };

        _bbqRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(bbqExample);

        // Act
        var commandResult = await _sut.Handle(moderateBbqCommand, default);

        // Assert 
        _bbqRepository.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        commandResult.IsError.Should().BeFalse();
        commandResult.Value.Id.Should().NotBeEmpty();
        commandResult.Value.Status.Should().Be(BbqStatus.PendingConfirmations.Name);
        commandResult.Value.Date.Should().Be(bbqExample.Date);
        commandResult.Value.IsTrincaPaying.Should().BeTrue();
    }

    [Fact(DisplayName = "Handle() should update the status of the bbq to PendingConfirmations and IsTrincaPaying to false")]
    [Trait("Application", "ModerateBbq - Handler")]
    public async Task Handle_UpdateToPendingConfirmationsAndIsTrincaPayingToFalse()
    {
        // Arrange
        var bbqExample = CommonBbqFixture.GetBbq();

        var moderateBbqCommand = new ModerateBbqCommand
        {
            BbqId = bbqExample.Id.ToString(),
            TrincaWillPay = false,
            GonnaHappen = true,
        };

        _bbqRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(bbqExample);

        // Act
        var commandResult = await _sut.Handle(moderateBbqCommand, default);

        // Assert 
        _bbqRepository.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        commandResult.IsError.Should().BeFalse();
        commandResult.Value.Id.Should().NotBeEmpty();
        commandResult.Value.Status.Should().Be(BbqStatus.PendingConfirmations.Name);
        commandResult.Value.Date.Should().Be(bbqExample.Date);
        commandResult.Value.IsTrincaPaying.Should().BeFalse();
    }

    [Fact(DisplayName = "Handle() should update the status of the bbq to ItsNotGonnaHappen and IsTrincaPaying to false")]
    [Trait("Application", "ModerateBbq - Handler")]
    public async Task Handle_UpdateToItsNotGonnaHappenAndIsTrincaPayingToFalse()
    {
        // Arrange
        var bbqExample = CommonBbqFixture.GetBbq();

        var moderateBbqCommand = new ModerateBbqCommand
        {
            BbqId = bbqExample.Id.ToString(),
            TrincaWillPay = false,
            GonnaHappen = false,
        };

        _bbqRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(bbqExample);

        // Act
        var commandResult = await _sut.Handle(moderateBbqCommand, default);

        // Assert 
        _bbqRepository.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        commandResult.IsError.Should().BeFalse();
        commandResult.Value.Id.Should().NotBeEmpty();
        commandResult.Value.Status.Should().Be(BbqStatus.ItsNotGonnaHappen.Name);
        commandResult.Value.Date.Should().Be(bbqExample.Date);
        commandResult.Value.IsTrincaPaying.Should().BeFalse();
    }

    [Fact(DisplayName = "Handle() should return BbqNotFound error")]
    [Trait("Application", "ModerateBbq - Handler")]
    public async Task Handle_ShouldReturnBbqNotFoundError()
    {
        // Arrange
        var bbqExample = CommonBbqFixture.GetBbq();

        var moderateBbqCommand = new ModerateBbqCommand
        {
            BbqId = bbqExample.Id.ToString(),
        };

        // Act
        var commandResult = await _sut.Handle(moderateBbqCommand, default);

        // Assert 
        _bbqRepository.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);

        commandResult.IsError.Should().BeTrue();
        commandResult.FirstError.Should().Be(BbqErrors.BbqNotFound);
    }
}
