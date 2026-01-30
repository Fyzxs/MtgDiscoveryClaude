using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.MtgDiscovery.Entry.Entities.Collections;

public sealed class RenameCollectionArgsEntity : IRenameCollectionArgsEntity
{
    public IAuthUserArgEntity AuthUser { get; init; }
    public IRenameCollectionArgEntity RenameCollection { get; init; }
}
