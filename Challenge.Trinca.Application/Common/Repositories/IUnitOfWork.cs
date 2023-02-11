namespace Challenge.Trinca.Domain.Repositories;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
