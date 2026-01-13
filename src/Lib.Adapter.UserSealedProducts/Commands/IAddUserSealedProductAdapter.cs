using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.UserSealedProducts.Commands;

/// <summary>
/// Adapter for adding or updating a sealed product in a user's collection.
/// </summary>
public interface IAddUserSealedProductAdapter
    : IOperationResponseService<IUserSealedProductXfrEntity, IUserSealedProductOufEntity>;
