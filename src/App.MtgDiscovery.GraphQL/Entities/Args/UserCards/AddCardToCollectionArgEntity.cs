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
