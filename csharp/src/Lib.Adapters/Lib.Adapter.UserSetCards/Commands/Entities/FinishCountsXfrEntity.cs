using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;

namespace Lib.Adapter.UserSetCards.Commands.Entities;

internal sealed class FinishCountsXfrEntity : IFinishCountsXfrEntity
{
    public required int Total { get; init; }
    public required int NonFoil { get; init; }
    public required int Foil { get; init; }
    public required int Etched { get; init; }
    public string CacheKey => $"finish_counts:{Foil}:{NonFoil}";
}
