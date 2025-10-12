using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Outs.UserSetCards;

public sealed class UserSetCardFinishGroupOutEntity
{
    public IReadOnlyCollection<string> Cards { get; init; }
}
