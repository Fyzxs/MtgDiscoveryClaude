using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.Shared.DataModels.Entities.Args.SealedProducts;

public interface ISealedProductsBySetCodeArgEntity : IUserIdArgEntity
{
    string SetCode { get; }

    /// <summary>
    /// Optional collection identifier for enriching results with collection-specific data.
    /// When provided, the backend validates that the authenticated user (from JWT) has permission to access this collection.
    /// </summary>
    string CollectionId { get; }

    /// <summary>
    /// Indicates whether a CollectionId has been provided.
    /// </summary>
    bool HasCollectionId => string.IsNullOrEmpty(CollectionId) is false;
}
