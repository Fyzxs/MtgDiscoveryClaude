using Lib.Shared.DataModels.Entities.Args.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Services;

namespace Lib.MtgDiscovery.Entry.Commands.UserSealedProducts;

/// <summary>
/// Entry service interface for adding a user sealed product to the collection.
/// Handles request validation and coordination with domain services.
/// </summary>
internal interface IAddUserSealedProductEntryService
    : IOperationResponseService<IAddUserSealedProductArgEntity, IUserSealedProductOufEntity>;
