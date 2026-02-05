using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Aggregator.Collections.Commands.Entities;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.Aggregator.Collections.Commands.Mappers;

internal sealed class TransferCollectionOwnershipItrToXfrMapper : ITransferCollectionOwnershipItrToXfrMapper
{
    public Task<ITransferCollectionOwnershipXfrEntity> Map(ITransferCollectionOwnershipItrEntity source)
    {
        ITransferCollectionOwnershipXfrEntity result = new TransferCollectionOwnershipXfrEntity
        {
            CollectionId = source.CollectionId,
            CurrentOwnerId = source.CurrentOwnerId,
            TargetUserId = source.TargetUserId
        };

        return Task.FromResult(result);
    }
}
