namespace Cli.MtgDiscovery.PriceUpdate.Orchestration;

internal sealed class PriceUpdateResult
{
    public int TotalCards { get; init; }
    public int UpdatedCards { get; init; }
    public int SkippedCards { get; init; }
    public int Errors { get; init; }
    public double TotalRuConsumed { get; init; }
}
