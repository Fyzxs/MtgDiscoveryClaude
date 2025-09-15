using Lib.Shared.DataModels.Entities;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserCards;

public sealed class CollectedItemArgEntity : ICollectedItemArgEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
}