using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects;
using Challenge.Trinca.Persistence.Configurations.Peoples.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Challenge.Trinca.Persistence.Configurations.Peoples;

public sealed class PeopleConfiguration : IEntityTypeConfiguration<People>
{
    public void Configure(EntityTypeBuilder<People> builder)
    {
        builder.ToContainer("People")
            .HasNoDiscriminator();

        builder.Property(b => b.Id);

        builder.OwnsMany<Invite>(nameof(People.Invites), x =>
        {
            x.Property(b => b.Id);

            x.Property(b => b.Status)
                .HasConversion(new InviteStatusValueConverter());
        });

        builder.HasData(new List<People>()
        {
            People.CreateWithId("171f9858-ddb1-4adf-886b-2ea36e0f0644", "Marcos Oliveira", true),
            People.CreateWithId("3f74e6bd-11b2-4d48-a294-239a7a2ce7d5", "Gustavo Sanfoninha", true),
            People.CreateWithId("e5c7c990-7d75-4445-b5a2-700df354a6a0", "João da Silva", false),
            People.CreateWithId("795fc8f2-1473-4f19-b33e-ade1a42ed123", "Alexandre Morales", false),
            People.CreateWithId("addd0967-6e16-4328-bab1-eec63bf31968", "Leandro Espera", false),
            People.CreateWithId("405ab31c-e14a-4854-95d4-627596ca3655", "Augusto Almeida", false),
            People.CreateWithId("6a5e49e8-731e-49f7-9c5d-08a8f4d06df3", "John Doe", false),
        });
    }
}