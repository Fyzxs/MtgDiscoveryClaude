using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.MtgDiscovery.Entry.Entities.Collections;

public sealed class TransferCollectionOwnershipArgsEntity : ITransferCollectionOwnershipArgsEntity
{
    public IAuthUserArgEntity AuthUser { get; init; }
    public ITransferCollectionOwnershipArgEntity TransferOwnership { get; init; }
}
