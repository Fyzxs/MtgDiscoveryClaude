using System.Collections.Generic;

namespace App.MtgDiscovery.GraphQL.Entities.Outs.UserCards;

public sealed class UserCardCollectionOutEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public ICollection<CollectedItemOutEntity> CollectedList { get; init; }
}
