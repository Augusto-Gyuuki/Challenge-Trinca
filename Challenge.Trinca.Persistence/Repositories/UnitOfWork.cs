using Challenge.Trinca.Domain.Repositories;
using Challenge.Trinca.Persistence.Data;

namespace Challenge.Trinca.Persistence.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _appDbContext;

    public UnitOfWork(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }
}
