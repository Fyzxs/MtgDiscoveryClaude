using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserCards;

public sealed class AddCardToCollectionArgEntity : IAddCardToCollectionArgEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public ICollection<ICollectedItemArgEntity> CollectedList { get; init; }
}

public sealed class CollectedItemArgEntity : ICollectedItemArgEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
}
