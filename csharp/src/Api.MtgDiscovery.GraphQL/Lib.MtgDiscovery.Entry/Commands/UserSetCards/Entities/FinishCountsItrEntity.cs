using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards.Entities;

internal sealed class FinishCountsItrEntity : IFinishCountsItrEntity
{
    public required int Total { get; init; }
    public required int NonFoil { get; init; }
    public required int Foil { get; init; }
    public required int Etched { get; init; }
}
