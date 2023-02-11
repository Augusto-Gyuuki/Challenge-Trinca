using Challenge.Trinca.Application.Common.Searchable;
using Challenge.Trinca.Application.UseCases.Bbqs.Common.Searchable;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;

namespace Challenge.Trinca.Application.Common.Repositories;

public interface IBbqRepository
{
    Task AddAsync(Bbq bbq, CancellationToken ct);

    Task UpdateAsync(Bbq bbq);

    Task<Bbq?> GetByIdAsync(Guid bbqid, CancellationToken ct);

    Task<SearchableOutput<Bbq>> SearchAsync(BbqsSearchInput bbqsSearchInput, CancellationToken cancellationToken);
}
