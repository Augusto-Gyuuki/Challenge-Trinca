using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot;
using Challenge.Trinca.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Challenge.Trinca.Persistence.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultContainer("Bbqs");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public DbSet<Bbq> Bbqs { get; set; }

    public DbSet<People> Peoples { get; set; }

    public DbSet<OutboxMessage> OutboxMessages { get; set; }
}
