using Challenge.Trinca.Application.Common.Repositories;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot;
using Challenge.Trinca.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Challenge.Trinca.Persistence.Repositories;

public sealed class PeopleRepository : IPeopleRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly DbSet<People> peoplesDbSet;

    public PeopleRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        peoplesDbSet = _appDbContext.Peoples;
    }

    public Task<List<People>> GetAllCoOwnersAsync(CancellationToken ct)
    {
        return peoplesDbSet.AsNoTracking()
            .Where(x => x.IsCoOwner)
            .ToListAsync(ct);
    }

    public Task<List<People>> GetAllAsync(CancellationToken ct)
    {
        return peoplesDbSet.AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<People?> GetByIdAsync(Guid peopleId, CancellationToken ct)
    {
        return await peoplesDbSet.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(peopleId), ct);
    }

    public Task UpdateAsync(People people)
    {
        return Task.FromResult(peoplesDbSet.Update(people));
    }
}
