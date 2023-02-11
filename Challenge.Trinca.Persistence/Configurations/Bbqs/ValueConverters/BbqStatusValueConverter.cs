using Challenge.Trinca.Domain.AggregatesRoot.BbqAggregateRoot.ValueObjects.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Challenge.Trinca.Persistence.Configurations.Bbqs.ValueConverters;

public sealed class BbqStatusValueConverter : ValueConverter<BbqStatus, string>
{
    public BbqStatusValueConverter(ConverterMappingHints mappingHints = null)
        : base(status => status.Name, value => BbqStatus.FromName(value), mappingHints)
    { }
}
