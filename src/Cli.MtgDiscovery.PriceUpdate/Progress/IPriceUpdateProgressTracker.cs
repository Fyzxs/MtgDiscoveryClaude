namespace Cli.MtgDiscovery.PriceUpdate.Progress;

internal interface IPriceUpdateProgressTracker
{
    void Initialize(int totalRecords);
    void IncrementProgress();
    void Complete();
}
