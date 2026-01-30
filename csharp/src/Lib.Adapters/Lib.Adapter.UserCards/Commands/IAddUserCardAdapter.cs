using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.UserCards.Commands;

/// <summary>
/// Adapter for adding or updating a card in a user's collection.
/// </summary>
internal interface IAddUserCardAdapter
    : IOperationResponseService<IAddUserCardXfrEntity, UserCardExtEntity>;
