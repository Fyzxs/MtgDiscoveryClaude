namespace Cli.MtgDiscovery.PriceUpdate.PriceChangeLogging;

internal sealed class PriceChangeRecord
{
    public string ScryfallId { get; init; } = string.Empty;
    public string Container { get; init; } = string.Empty;
    public string CardName { get; init; } = string.Empty;
    public string SetCode { get; init; } = string.Empty;
    public string OldUsd { get; init; } = string.Empty;
    public string OldUsdFoil { get; init; } = string.Empty;
    public string OldUsdEtched { get; init; } = string.Empty;
    public string NewUsd { get; init; } = string.Empty;
    public string NewUsdFoil { get; init; } = string.Empty;
    public string NewUsdEtched { get; init; } = string.Empty;
}
