namespace Challenge.Trinca.Persistence.Settings;

public sealed class CosmosDbSettings
{
    public string AccountEndpoint { get; init; } = string.Empty;

    public string AccountKey { get; init; } = string.Empty;

    public string DatabaseName { get; init; } = string.Empty;
}
