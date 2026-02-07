using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.UserCards.Queries;

/// <summary>
/// Adapter interface for retrieving user cards by multiple artists for convention signing planning.
/// </summary>
internal interface IUserCardsForSigningAdapter
    : IOperationResponseService<IUserCardsForSigningXfrEntity, IEnumerable<UserCardExtEntity>>;
