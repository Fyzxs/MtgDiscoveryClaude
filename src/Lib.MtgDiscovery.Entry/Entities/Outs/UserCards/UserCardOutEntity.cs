using System.Collections.Generic;

namespace Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;

public sealed class UserCardOutEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public ICollection<CollectedItemOutEntity> CollectedList { get; init; }
}
