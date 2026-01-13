using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.UserSealedProducts.Commands;

/// <summary>
/// Marker interface for adding a user sealed product to the collection.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface IAddUserSealedProductDomainService
    : IOperationResponseService<IAddUserSealedProductItrEntity, IUserSealedProductOufEntity>;
