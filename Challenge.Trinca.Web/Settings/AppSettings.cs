using Challenge.Trinca.Infrastructure.Settings;
using Challenge.Trinca.Persistence.Settings;

namespace Challenge.Trinca.Web.Settings;

public sealed class AppSettings
{
    public CosmosDbSettings? CosmosDbSettings { get; init; }

    public OutboxMessageSettings? OutboxMessageSettings { get; init; }
}
