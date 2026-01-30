using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.Collections.Apis;

public interface ICollectionQueryAdapter
{
    Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(string ownerId);
    Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(string ownerId);
    Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(string collectionId, string ownerId);
    Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(string userId);
    Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(string userId);
}
