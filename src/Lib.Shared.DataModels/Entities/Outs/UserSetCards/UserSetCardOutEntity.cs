using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Outs.UserSetCards;

public sealed class UserSetCardOutEntity
{
    public string UserId { get; init; }
    public string SetId { get; init; }
    public int TotalCards { get; init; }
    public int UniqueCards { get; init; }
    public IReadOnlyDictionary<string, UserSetCardGroupOutEntity> Groups { get; init; }
}
