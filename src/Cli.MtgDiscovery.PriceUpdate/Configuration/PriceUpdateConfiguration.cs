namespace Cli.MtgDiscovery.PriceUpdate.Configuration;

internal sealed class PriceUpdateConfiguration
{
    public string ManaPoolApiUrl { get; init; } = "https://manapool.com/api/v1/prices/singles";
    public string ErrorOutputPath { get; init; } = "./logs/price-update-errors.csv";
    public string PriceChangeLogPath { get; init; } = "./logs/price-changes.csv";
    public int MaxConcurrentOperations { get; init; } = 10;
    public int MaxRuPerSecond { get; init; } = 5000;
    public int RetryCount { get; init; } = 3;
    public int RetryDelayMs { get; init; } = 1000;
    public bool SkipUnchangedPrices { get; init; } = true;
    public bool TestMode { get; init; }
    public int TestCardLimit { get; init; } = 100;
}
