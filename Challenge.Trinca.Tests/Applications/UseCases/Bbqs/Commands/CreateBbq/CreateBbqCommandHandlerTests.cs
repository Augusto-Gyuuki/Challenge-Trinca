using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.UseCases.Bbqs.Commands.CreateBbq;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.ValueObjects.Enums;
using Challenge.Trinca.Domain.Repositories;
using Moq;

namespace Challenge.Trinca.Tests.Unit.Applications.UseCases.Bbqs.Commands.CreateBbq;

public sealed class CreateBbqCommandHandlerTests
{
    private readonly CreateBbqCommandHandler _sut;
    private readonly Mock<IBbqRepository> _bbqRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    public CreateBbqCommandHandlerTests()
    {
        _sut = new CreateBbqCommandHandler(_bbqRepository.Object, _unitOfWork.Object);
    }

    [Fact(DisplayName = "Handle() should create new bbq")]
    [Trait("Application", "CreateBbq - Handler")]
    public async Task Handle_ShouldReturnValidResult()
    {
        // Arrange
        var createBbqCommand = CreateBbqCommandFixture.GetCreateBbqCommand();

        // Act
        var commandResult = await _sut.Handle(createBbqCommand, default);

        // Assert 
        _bbqRepository.Verify(
            x => x.AddAsync(It.IsAny<Bbq>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        commandResult.IsError.Should().BeFalse();
        commandResult.Value.Id.Should().NotBeEmpty();
        commandResult.Value.Status.Should().Be(BbqStatus.New.Name);
        commandResult.Value.Date.Should().Be(createBbqCommand.Date);
    }
}
