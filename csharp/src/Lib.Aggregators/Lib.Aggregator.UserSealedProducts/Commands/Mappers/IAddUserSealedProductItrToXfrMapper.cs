using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;

namespace Lib.Aggregator.UserSealedProducts.Commands.Mappers;

internal interface IAddUserSealedProductItrToXfrMapper : ICreateMapper<IAddUserSealedProductItrEntity, IUserSealedProductXfrEntity>;
