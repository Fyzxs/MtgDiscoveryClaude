using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserCards.Queries;

/// <summary>
/// Adapter interface for retrieving user cards by multiple artists for convention signing planning.
/// </summary>
internal interface IUserCardsForSigningAdapter
{
    /// <summary>
    /// Retrieves all user cards that match any of the specified artist IDs.
    /// </summary>
    /// <param name="input">The user cards for signing entity containing userId and artistIds</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>Collection of user card collection information wrapped in an operation response</returns>
    Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> Execute(
        IUserCardsForSigningXfrEntity input,
        CancellationToken cancellationToken);
}
