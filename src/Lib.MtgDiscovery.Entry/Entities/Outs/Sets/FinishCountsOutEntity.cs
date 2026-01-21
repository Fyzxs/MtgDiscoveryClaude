namespace Lib.MtgDiscovery.Entry.Entities.Outs.Sets;

public sealed class FinishCountsOutEntity
{
    public int Total { get; init; }
    public int NonFoil { get; init; }
    public int Foil { get; init; }
    public int Etched { get; init; }
}
