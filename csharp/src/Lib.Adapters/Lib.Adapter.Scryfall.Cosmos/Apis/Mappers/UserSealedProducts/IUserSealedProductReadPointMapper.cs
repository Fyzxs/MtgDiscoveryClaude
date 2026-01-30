using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Mappers.UserSealedProducts;

/// <summary>
/// Maps sealed product UUID and user ID to a ReadPointItem for Cosmos DB operations.
/// </summary>
public interface IUserSealedProductReadPointMapper : ICreateMapper<string, string, ReadPointItem>;
