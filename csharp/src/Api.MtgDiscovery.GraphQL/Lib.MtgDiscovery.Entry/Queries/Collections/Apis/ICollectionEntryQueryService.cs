using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Collections;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Collections.Apis;

public interface ICollectionEntryQueryService
{
    Task<IOperationResponse<List<CollectionOutEntity>>> MyCollectionsAsync(IUserIdArgEntity args, CancellationToken cancellationToken);
    Task<IOperationResponse<CollectionOutEntity>> GetCollectionByIdAsync(ICollectionIdArgEntity args, CancellationToken cancellationToken);
    Task<IOperationResponse<List<CollectionOutEntity>>> SharedCollectionsAsync(IUserIdArgEntity args, CancellationToken cancellationToken);
    Task<IOperationResponse<List<CollectionOutEntity>>> AccessibleCollectionsAsync(IUserIdArgEntity args, CancellationToken cancellationToken);
}
