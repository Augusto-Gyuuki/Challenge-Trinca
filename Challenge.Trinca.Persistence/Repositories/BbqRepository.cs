using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Application.Common.Searchable;
using Challenge.Trinca.Application.UseCases.Bbqs.Common.Searchable;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.ValueObjects.Enums;
using Challenge.Trinca.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Challenge.Trinca.Persistence.Repositories;

public sealed class BbqRepository : IBbqRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly DbSet<Bbq> bbqDbSet;

    public BbqRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        bbqDbSet = _appDbContext.Bbqs;
    }

    public async Task AddAsync(Bbq bbq, CancellationToken ct)
    {
        await bbqDbSet.AddAsync(bbq, ct);
    }

    public async Task<Bbq?> GetByIdAsync(Guid bbqid, CancellationToken ct)
    {
        return await bbqDbSet.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(bbqid), ct);
    }

    public async Task<SearchableOutput<Bbq>> SearchAsync(BbqsSearchInput baseSearchableInput, CancellationToken cancellationToken)
    {
        var toSkip = (baseSearchableInput.Page - 1) * baseSearchableInput.PerPage;
        var query = bbqDbSet
            .AsNoTracking()
            .Where(x => !x.Status.Equals(BbqStatus.ItsNotGonnaHappen))
            .Where(x => x.Date > DateTime.UtcNow);

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip(toSkip)
            .Take(baseSearchableInput.PerPage)
            .ToListAsync(cancellationToken);

        return new SearchableOutput<Bbq>()
        {
            CurrentPage = baseSearchableInput.Page,
            PerPage = baseSearchableInput.PerPage,
            Total = total,
            Items = items
        };
    }

    public Task UpdateAsync(Bbq bbq)
    {
        return Task.FromResult(bbqDbSet.Update(bbq));
    }
}

