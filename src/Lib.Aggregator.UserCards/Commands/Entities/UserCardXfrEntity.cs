using System.Collections.Generic;
using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Aggregator.UserCards.Commands.Entities;

internal sealed class AddUserCardXfrEntity : IAddUserCardXfrEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public ICollection<IUserCardDetailsXfrEntity> CollectedList { get; init; }
}
