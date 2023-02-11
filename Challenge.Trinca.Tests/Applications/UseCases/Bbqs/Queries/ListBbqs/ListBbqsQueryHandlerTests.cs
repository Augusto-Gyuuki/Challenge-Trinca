using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.UseCases.Bbqs.Common.Searchable;
using Challenge.Trinca.Application.UseCases.Bbqs.Queries.ListBbqs;
using Moq;

namespace Challenge.Trinca.Tests.Unit.Applications.UseCases.Bbqs.Queries.ListBbqs;

public sealed class ListBbqsQueryHandlerTests
{
    private readonly Mock<IBbqRepository> _bbqRepository = new();
    private readonly ListBbqsQueryHandler _sut;

    public ListBbqsQueryHandlerTests()
    {
        _sut = new ListBbqsQueryHandler(_bbqRepository.Object);
    }

    [Fact(DisplayName = "Handle() should return a list of bbqs")]
    [Trait("Application", "ListBbqsQuery - Handler")]
    public async Task Handle_ShouldReturnValidResult()
    {
        // Arrange
        var listBbqsQuery = ListBbqsQueryFixture.GetListBbqsQuery();
        var searchableOutput = ListBbqsQueryFixture.GetSearchableOutput(listBbqsQuery.Page, listBbqsQuery.PerPage);

        _bbqRepository.Setup(x => x.SearchAsync(It.IsAny<BbqsSearchInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(searchableOutput);

        // Act
        var queryResult = await _sut.Handle(listBbqsQuery, default);

        // Assert 
        _bbqRepository.Verify(
            x => x.SearchAsync(It.IsAny<BbqsSearchInput>(), It.IsAny<CancellationToken>()),
            Times.Once);

        queryResult.IsError.Should().BeFalse();
        queryResult.Value.Should().NotBeNull();
        queryResult.Value.CurrentPage.Should().Be(listBbqsQuery.Page);
        queryResult.Value.PerPage.Should().Be(listBbqsQuery.PerPage);
        queryResult.Value.Items.Count.Should().Be(listBbqsQuery.PerPage);
        queryResult.Value.Total.Should().Be(searchableOutput.Total);

        foreach (var (bbqModelResult, bbqModelResultindex) in queryResult.Value.Items.Select((value, index) => (value, index)))
        {
            var searchableOutputItems = searchableOutput.Items[bbqModelResultindex];

            bbqModelResult.Id.Should().Be(searchableOutputItems.Id);
            bbqModelResult.Reason.Should().Be(searchableOutputItems.Reason);
            bbqModelResult.Date.Should().Be(searchableOutputItems.Date);
            bbqModelResult.IsTrincaPaying.Should().Be(searchableOutputItems.IsTrincaPaying);
            bbqModelResult.Status.Should().Be(searchableOutputItems.Status.Name);
            bbqModelResult.UpdatedDateTime.Should().Be(searchableOutputItems.UpdatedDateTime);
            bbqModelResult.CreatedDateTime.Should().Be(searchableOutputItems.CreatedDateTime);
        }
    }
}
