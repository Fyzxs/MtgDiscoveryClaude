using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.MtgDiscovery.Entry.Entities.Collections;

public sealed class RevokeCollectionAccessArgsEntity : IRevokeCollectionAccessArgsEntity
{
    public IAuthUserArgEntity AuthUser { get; init; }
    public IRevokeCollectionAccessArgEntity RevokeAccess { get; init; }
}
