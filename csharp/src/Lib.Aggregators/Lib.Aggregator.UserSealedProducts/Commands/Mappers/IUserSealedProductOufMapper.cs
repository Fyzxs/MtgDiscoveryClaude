using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;

namespace Lib.Aggregator.UserSealedProducts.Commands.Mappers;

internal interface IUserSealedProductOufMapper : ICreateMapper<UserSealedProductExtEntity, IUserSealedProductOufEntity>;
