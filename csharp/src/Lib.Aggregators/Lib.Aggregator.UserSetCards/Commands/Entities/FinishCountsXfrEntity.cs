using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Commands.Entities;

internal sealed class FinishCountsXfrEntity : IFinishCountsXfrEntity
{
    public int Total { get; init; }
    public int NonFoil { get; init; }
    public int Foil { get; init; }
    public int Etched { get; init; }
    public string CacheKey => $"finish_counts:{Foil}:{NonFoil}:{Etched}";
}
