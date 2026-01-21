using System.Collections.Generic;
using System.Linq;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.UserCards.Tests.Fakes;

internal sealed class FakeUserCardCollectionItrEntity : IUserCardCollectionItrEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public ICollection<ICollectedItemItrEntity> CollectedList { get; init; }
}
