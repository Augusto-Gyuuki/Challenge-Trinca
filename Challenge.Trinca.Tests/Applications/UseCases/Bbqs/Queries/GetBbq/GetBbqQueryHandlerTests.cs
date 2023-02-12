using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.UseCases.Bbqs.Queries.GetBbq;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.Errors;
using Challenge.Trinca.Tests.Unit.BaseFixtures;
using Moq;
using Serilog;

namespace Challenge.Trinca.Tests.Unit.Applications.UseCases.Bbqs.Queries.GetBbq;

public sealed class GetBbqQueryHandlerTests
{
    private readonly GetBbqQueryHandler _sut;
    private readonly Mock<IBbqRepository> _bbqRepository = new();
    private readonly Mock<IPeopleRepository> _peopleRepository = new();
    private readonly Mock<ILogger> _logger = new();

    public GetBbqQueryHandlerTests()
    {
        _sut = new GetBbqQueryHandler(_bbqRepository.Object, _peopleRepository.Object, _logger.Object);
    }

    [Fact(DisplayName = "Handle() should return bbq when people is co-owner")]
    [Trait("Application", "GetBbq - Handler")]
    public async Task Handle_ReturnBbqWhenPeopleIsCoOwner()
    {
        // Arrange
        var bbqExample = CommonBbqFixture.GetBbq();
        var peopleExample = CommonPeopleFixture.GetCoOwnerPeople();
        var vegetarianGuest = CommonBbqFixture.GetGuest();
        bbqExample.AddGuest(vegetarianGuest);
        vegetarianGuest.SetIsVegetarian(true);
        bbqExample.ConfirmGuest(vegetarianGuest);

        var nonVegetarianGuest = CommonBbqFixture.GetGuest();
        bbqExample.AddGuest(nonVegetarianGuest);
        nonVegetarianGuest.SetIsVegetarian(false);
        bbqExample.ConfirmGuest(nonVegetarianGuest);

        var getBbqQuery = new GetBbqQuery
        {
            BbqId = bbqExample.Id.ToString(),
            PeopleId = peopleExample.Id.ToString(),
        };

        _bbqRepository.Setup(x => x.GetByIdAsync(bbqExample.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(bbqExample);

        _peopleRepository.Setup(x => x.GetByIdAsync(peopleExample.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(peopleExample);

        // Act
        var queryResult = await _sut.Handle(getBbqQuery, default);

        // Assert 
        _bbqRepository.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _peopleRepository.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);

        queryResult.IsError.Should().BeFalse();
        queryResult.Value.Id.Should().Be(bbqExample.Id);
        queryResult.Value.VegetableAmountInKilograms.Should().BeGreaterThan(default);
        queryResult.Value.MeatAmountInKilograms.Should().BeGreaterThan(default);
    }

    [Fact(DisplayName = "Handle() should return BbqNotFound error when bbq was not found")]
    [Trait("Application", "GetBbq - Handler")]
    public async Task Handle_ReturnBbqNotFoundError()
    {
        // Arrange
        var bbqExample = CommonBbqFixture.GetBbq();
        var peopleExample = CommonPeopleFixture.GetCoOwnerPeople();

        var getBbqQuery = new GetBbqQuery
        {
            BbqId = bbqExample.Id.ToString(),
            PeopleId = peopleExample.Id.ToString(),
        };

        _bbqRepository.Setup(x => x.GetByIdAsync(bbqExample.Id, It.IsAny<CancellationToken>()));

        _peopleRepository.Setup(x => x.GetByIdAsync(peopleExample.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(peopleExample);

        // Act
        var queryResult = await _sut.Handle(getBbqQuery, default);

        // Assert 
        _bbqRepository.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _peopleRepository.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);

        queryResult.IsError.Should().BeTrue();
        queryResult.FirstError.Should().Be(BbqErrors.BbqNotFound);
    }

    [Fact(DisplayName = "Handle() should return NotAuthorized error when people is not co owner")]
    [Trait("Application", "GetBbq - Handler")]
    public async Task Handle_ReturnNotAuthorizedError()
    {
        // Arrange
        var bbqExample = CommonBbqFixture.GetBbq();
        var peopleExample = CommonPeopleFixture.GetPeople();

        var getBbqQuery = new GetBbqQuery
        {
            BbqId = bbqExample.Id.ToString(),
            PeopleId = peopleExample.Id.ToString(),
        };

        _peopleRepository.Setup(x => x.GetByIdAsync(peopleExample.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(peopleExample);

        // Act
        var queryResult = await _sut.Handle(getBbqQuery, default);

        // Assert 
        _peopleRepository.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);

        queryResult.IsError.Should().BeTrue();
        queryResult.FirstError.Should().Be(PeopleErrors.NotAuthorized);
    }

    [Fact(DisplayName = "Handle() should return PeopleNotFound error when people was not found")]
    [Trait("Application", "GetBbq - Handler")]
    public async Task Handle_ReturnPeopleNotFoundError()
    {
        // Arrange
        var peopleExample = CommonPeopleFixture.GetCoOwnerPeople();

        var getBbqQuery = new GetBbqQuery
        {
            BbqId = Guid.NewGuid().ToString(),
            PeopleId = peopleExample.Id.ToString(),
        };

        _peopleRepository.Setup(x => x.GetByIdAsync(peopleExample.Id, It.IsAny<CancellationToken>()));

        // Act
        var queryResult = await _sut.Handle(getBbqQuery, default);

        // Assert 
        _peopleRepository.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);

        queryResult.IsError.Should().BeTrue();
        queryResult.FirstError.Should().Be(PeopleErrors.PeopleNotFound);
    }
}
