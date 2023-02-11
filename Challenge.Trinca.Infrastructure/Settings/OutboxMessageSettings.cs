namespace Challenge.Trinca.Infrastructure.Settings;

public sealed class OutboxMessageSettings
{
    public int RetryCount { get; init; }

    public int RetryWaitTimeInSeconds { get; init; }

    public int BackgroundIntevalInSeconds { get; init; }

    public int MessagesTakeCount { get; set; }
}
