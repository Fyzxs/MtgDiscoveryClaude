using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Args;

public sealed class UserCardsByIdsArgEntity : IUserCardsByIdsArgEntity
{
    public string UserId { get; init; }
    public ICollection<string> CardIds { get; init; }
}
