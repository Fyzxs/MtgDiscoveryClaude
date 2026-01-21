using Lib.Shared.DataModels.Entities;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserCards;

public sealed class AddCardToCollectionArgEntity : IAddCardToCollectionArgEntity
{
    public string CardId { get; init; }
    public string SetId { get; init; }

    // GraphQL needs concrete type for input, not interface
    public CollectedItemArgEntity CollectedItem { get; init; }

    // Implement interface property
    ICollectedItemArgEntity IAddCardToCollectionArgEntity.CollectedItem => CollectedItem;
}
