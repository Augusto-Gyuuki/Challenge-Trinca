using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Challenge.Trinca.Persistence.Configurations.Peoples.ValueConverters;

public sealed class InviteStatusValueConverter : ValueConverter<InviteStatus, string>
{
    public InviteStatusValueConverter(ConverterMappingHints mappingHints = null)
        : base(id => id.Name, value => InviteStatus.FromName(value.ToString()), mappingHints)
    { }
}
