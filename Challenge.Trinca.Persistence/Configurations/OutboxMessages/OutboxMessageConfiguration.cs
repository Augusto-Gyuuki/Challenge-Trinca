using Challenge.Trinca.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Challenge.Trinca.Persistence.Configurations.OutboxMessages;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToContainer("OutboxMessages")
            .HasNoDiscriminator();

        builder.Property(b => b.Id);
    }
}
