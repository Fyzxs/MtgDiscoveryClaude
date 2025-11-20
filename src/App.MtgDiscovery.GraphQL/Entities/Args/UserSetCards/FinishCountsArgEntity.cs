using Lib.Shared.DataModels.Entities.Args.UserSetCards;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserSetCards;

public sealed class FinishCountsArgEntity : IFinishCountsArgEntity
{
    public int Total { get; init; }
    public int NonFoil { get; init; }
    public int Foil { get; init; }
    public int Etched { get; init; }
}
