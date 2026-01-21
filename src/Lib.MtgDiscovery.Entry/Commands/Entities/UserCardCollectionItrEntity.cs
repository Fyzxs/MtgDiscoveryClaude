using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Commands.Entities;

internal sealed class UserCardCollectionItrEntity : IUserCardItrEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public ICollection<IUserCardDetailsItrEntity> CollectedList { get; init; }
}
