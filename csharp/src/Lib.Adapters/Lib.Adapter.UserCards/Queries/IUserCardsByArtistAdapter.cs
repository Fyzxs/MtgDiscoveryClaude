using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserCards.Queries;

/// <summary>
/// Adapter for retrieving all user cards by a specific artist.
/// </summary>
internal interface IUserCardsByArtistAdapter
{
    Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> Execute(
        IUserCardsArtistXfrEntity input,
        CancellationToken cancellationToken);
}
