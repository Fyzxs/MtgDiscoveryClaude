using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.MtgDiscovery.Entry.Entities.Collections;

public interface ITransferCollectionOwnershipArgsEntity
{
    IAuthUserArgEntity AuthUser { get; }
    ITransferCollectionOwnershipArgEntity TransferOwnership { get; }
}
