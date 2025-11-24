using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Entities;

internal sealed class FinishCountsOufEntity : IFinishCountsOufEntity
{
    public int Total { get; init; }
    public int NonFoil { get; init; }
    public int Foil { get; init; }
    public int Etched { get; init; }
}
