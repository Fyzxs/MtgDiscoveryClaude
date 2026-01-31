using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.Collections.Entities;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Mappers;

internal sealed class TransferCollectionOwnershipArgToItrMapper : ITransferCollectionOwnershipArgToItrMapper
{
    public Task<ITransferCollectionOwnershipItrEntity> Map(ITransferCollectionOwnershipArgEntity source1, string userId)
    {
        ITransferCollectionOwnershipItrEntity result = new TransferCollectionOwnershipItrEntity
        {
            CollectionId = source1.CollectionId,
            CurrentOwnerId = userId,
            TargetUserId = source1.TargetUserId
        };

        return Task.FromResult(result);
    }
}
