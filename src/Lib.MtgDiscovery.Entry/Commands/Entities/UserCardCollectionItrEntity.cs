using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Commands.Entities;

internal sealed class UserCardCollectionOufEntity : IUserCardOufEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public ICollection<IUserCardDetailsOufEntity> CollectedList { get; init; }
}
