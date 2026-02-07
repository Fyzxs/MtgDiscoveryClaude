using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;

namespace Lib.Aggregator.UserSealedProducts.Commands.Mappers;

internal interface IUserSealedProductExtToSealedProductOufMapper : ICreateMapper<UserSealedProductExtEntity, ISealedProductOufEntity>;
