namespace Cli.MtgDiscovery.PriceUpdate.ErrorTracking;

internal sealed class PriceUpdateError
{
    public string ScryfallId { get; init; } = string.Empty;
    public string Container { get; init; } = string.Empty;
    public string ErrorMessage { get; init; } = string.Empty;
}
