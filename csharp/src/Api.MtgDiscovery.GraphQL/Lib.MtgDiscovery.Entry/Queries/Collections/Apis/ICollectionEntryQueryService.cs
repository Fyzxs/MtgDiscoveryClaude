using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Collections;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Collections.Apis;

public interface ICollectionEntryQueryService
{
    Task<IOperationResponse<List<CollectionOutEntity>>> MyCollectionsAsync(IUserIdArgEntity args);
    Task<IOperationResponse<CollectionOutEntity>> GetCollectionByIdAsync(ICollectionIdArgEntity args);
    Task<IOperationResponse<List<CollectionOutEntity>>> SharedCollectionsAsync(IUserIdArgEntity args);
    Task<IOperationResponse<List<CollectionOutEntity>>> AccessibleCollectionsAsync(IUserIdArgEntity args);
}
