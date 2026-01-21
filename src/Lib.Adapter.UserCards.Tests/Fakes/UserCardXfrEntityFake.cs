using System.Collections.Generic;
using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Adapter.UserCards.Tests.Fakes;

internal sealed class UserCardXfrEntityFake : IUserCardXfrEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public ICollection<IUserCardDetailsXfrEntity> CollectedList { get; init; }
}