using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.UserCards.Queries;

/// <summary>
/// Adapter for retrieving all user cards by a specific artist.
/// </summary>
internal interface IUserCardsByArtistAdapter
    : IOperationResponseService<IUserCardsArtistXfrEntity, IEnumerable<UserCardExtEntity>>;
