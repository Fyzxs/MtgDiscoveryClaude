using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Entities;

internal sealed class UserSetCardFinishGroupOufEntity : IUserSetCardFinishGroupOufEntity
{
    public IReadOnlyCollection<string> Cards { get; init; }
}
