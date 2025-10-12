using System.Collections.Generic;

namespace Lib.Aggregator.UserSetCards.Entities;

internal sealed class UserSetCardFinishGroupOufEntity : IUserSetCardFinishGroupOufEntity
{
    public IReadOnlyCollection<string> Cards { get; init; }
}
