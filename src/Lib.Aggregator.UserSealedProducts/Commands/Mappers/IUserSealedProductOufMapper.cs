using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Abstractions.Patterns.Creation;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;

namespace Lib.Aggregator.UserSealedProducts.Commands.Mappers;

internal interface IUserSealedProductOufMapper : ICreateMapper<UserSealedProductExtEntity, IUserSealedProductOufEntity>;
