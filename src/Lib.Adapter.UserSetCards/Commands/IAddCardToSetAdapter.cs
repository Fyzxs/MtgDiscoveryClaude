using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.UserSetCards.Commands;

/// <summary>
/// Adapter for adding or removing a card from a user's set collection.
/// Implements atomic read-modify-write pattern.
/// </summary>
internal interface IAddCardToSetAdapter
    : IOperationResponseService<IAddCardToSetXfrEntity, UserSetCardExtEntity>;
