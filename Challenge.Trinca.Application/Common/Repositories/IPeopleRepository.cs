using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot;

namespace Challenge.Trinca.Application.Common.Repositories;

public interface IPeopleRepository
{
    Task<People?> GetByIdAsync(Guid peopleId, CancellationToken ct);

    Task UpdateAsync(People people);

    Task<List<People>> GetAllCoOwnersAsync(CancellationToken ct);

    Task<List<People>> GetAllAsync(CancellationToken ct);
}
