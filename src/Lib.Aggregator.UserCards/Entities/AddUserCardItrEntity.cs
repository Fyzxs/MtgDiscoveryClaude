using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Entities;

internal sealed class UserCardOufEntity : IUserCardOufEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public ICollection<IUserCardDetailsItrEntity> CollectedList { get; init; }
}
