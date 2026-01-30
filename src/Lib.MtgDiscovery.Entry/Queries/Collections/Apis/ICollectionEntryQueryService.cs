using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Collections.Apis;

public interface ICollectionEntryQueryService
{
    Task<IOperationResponse<List<CollectionOutEntity>>> MyCollectionsAsync(string userId);
    Task<IOperationResponse<CollectionOutEntity>> GetCollectionByIdAsync(string collectionId, string userId);
    Task<IOperationResponse<List<CollectionOutEntity>>> SharedCollectionsAsync(string userId);
    Task<IOperationResponse<List<CollectionOutEntity>>> AccessibleCollectionsAsync(string userId);
}
