using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.ValueObjects;
using Challenge.Trinca.Persistence.Configurations.Bbqs.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Challenge.Trinca.Persistence.Configurations.Bbqs;

public sealed class BbqConfiguration : IEntityTypeConfiguration<Bbq>
{
    public void Configure(EntityTypeBuilder<Bbq> builder)
    {
        builder.ToContainer("Bbqs")
            .HasNoDiscriminator();

        builder.Property(b => b.Id);

        builder.Property(b => b.Status)
            .HasConversion(new BbqStatusValueConverter());

        builder.OwnsMany<Guest>(nameof(Bbq.Guests), x =>
        {
            x.HasKey(b => b.PeopleId);
        });
    }
}
