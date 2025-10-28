using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.UserSetCards.Queries;

/// <summary>
/// Adapter for retrieving user set card data from storage.
/// </summary>
internal interface IGetUserSetCardAdapter
    : IOperationResponseService<IUserSetCardGetXfrEntity, UserSetCardExtEntity>;
