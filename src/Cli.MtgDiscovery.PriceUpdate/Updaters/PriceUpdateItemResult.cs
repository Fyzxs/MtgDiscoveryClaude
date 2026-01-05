namespace Cli.MtgDiscovery.PriceUpdate.Updaters;

internal sealed class PriceUpdateItemResult
{
    public string ScryfallId { get; init; } = string.Empty;
    public string Container { get; init; } = string.Empty;
    public string CardName { get; init; } = string.Empty;
    public string SetCode { get; init; } = string.Empty;
    public bool WasUpdated { get; init; }
    public bool WasSkipped { get; init; }
    public bool HasError { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;
    public double RuConsumed { get; init; }
    public string OldUsd { get; init; } = string.Empty;
    public string OldUsdFoil { get; init; } = string.Empty;
    public string OldUsdEtched { get; init; } = string.Empty;
    public string NewUsd { get; init; } = string.Empty;
    public string NewUsdFoil { get; init; } = string.Empty;
    public string NewUsdEtched { get; init; } = string.Empty;
}
