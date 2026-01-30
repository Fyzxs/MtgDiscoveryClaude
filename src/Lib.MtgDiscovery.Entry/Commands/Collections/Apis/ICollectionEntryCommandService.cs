using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Collections;
using Lib.MtgDiscovery.Entry.Entities.Outs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Apis;

public interface ICollectionEntryCommandService
{
    Task<IOperationResponse<CollectionOutEntity>> CreateCollectionAsync(ICreateCollectionArgsEntity argsEntity);
    Task<IOperationResponse<CollectionOutEntity>> RenameCollectionAsync(IRenameCollectionArgsEntity argsEntity);
    Task<IOperationResponse<CollectionOutEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityArgsEntity argsEntity);
    Task<IOperationResponse<CollectionOutEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessArgsEntity argsEntity);
    Task<IOperationResponse<CollectionOutEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessArgsEntity argsEntity);
    Task<IOperationResponse<CollectionOutEntity>> DeleteCollectionAsync(IDeleteCollectionArgsEntity argsEntity);
    Task<IOperationResponse<CollectionOutEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipArgsEntity argsEntity);
    Task<IOperationResponse<IEnumerable<AuthorizedUserOutEntity>>> GetCollectionAccessListAsync(IGetCollectionAccessListArgsEntity argsEntity);
}
