using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Collections;
using Lib.MtgDiscovery.Entry.Entities.Outs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Apis;

public interface ICollectionEntryCommandService
{
    Task<IOperationResponse<CollectionOutEntity>> CreateCollectionAsync(ICreateCollectionArgsEntity argsEntity, CancellationToken cancellationToken);
    Task<IOperationResponse<CollectionOutEntity>> RenameCollectionAsync(IRenameCollectionArgsEntity argsEntity, CancellationToken cancellationToken);
    Task<IOperationResponse<CollectionOutEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityArgsEntity argsEntity, CancellationToken cancellationToken);
    Task<IOperationResponse<CollectionOutEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessArgsEntity argsEntity, CancellationToken cancellationToken);
    Task<IOperationResponse<CollectionOutEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessArgsEntity argsEntity, CancellationToken cancellationToken);
    Task<IOperationResponse<CollectionOutEntity>> DeleteCollectionAsync(IDeleteCollectionArgsEntity argsEntity, CancellationToken cancellationToken);
    Task<IOperationResponse<CollectionOutEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipArgsEntity argsEntity, CancellationToken cancellationToken);
    Task<IOperationResponse<IEnumerable<AuthorizedUserOutEntity>>> GetCollectionAccessListAsync(IGetCollectionAccessListArgsEntity argsEntity, CancellationToken cancellationToken);
}
