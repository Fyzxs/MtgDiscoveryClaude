using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Commands.Entities;

internal sealed class UserCardCollectionItrEntity : IUserCardCollectionItrEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public ICollection<ICollectedItemItrEntity> CollectedList { get; init; }
}
