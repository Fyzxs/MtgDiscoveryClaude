using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Adapter.UserCards.Commands;
using Lib.Adapter.UserCards.Queries;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserCards.Apis;

/// <summary>
/// Main user cards adapter service implementation following the passthrough pattern.
///
/// This service coordinates all user card collection-related adapter operations by delegating
/// to specialized adapters. Currently delegates to IUserCardsCommandAdapter for command
/// operations and IUserCardsQueryAdapter for query operations.
///
/// Future Expansion Examples:
///   - IUserCardsCacheAdapter for caching layer
///   - IUserCardsFallbackAdapter for redundancy
///   - IUserCardsMetricsAdapter for telemetry
///
/// Pattern Consistency:
/// Matches EntryService, DomainService, and AggregatorService patterns
/// to maintain predictable architecture across all layers.
/// </summary>
public sealed class UserCardsAdapterService : IUserCardsAdapterService
{
    private readonly IUserCardsCommandAdapter _userCardsCommandAdapter;
    private readonly IUserCardsQueryAdapter _userCardsQueryAdapter;

    public UserCardsAdapterService(ILogger logger) : this(
        new UserCardsCommandAdapter(logger),
        new UserCardsQueryAdapter(logger))
    { }

    private UserCardsAdapterService(
        IUserCardsCommandAdapter userCardsCommandAdapter,
        IUserCardsQueryAdapter userCardsQueryAdapter)
    {
        _userCardsCommandAdapter = userCardsCommandAdapter;
        _userCardsQueryAdapter = userCardsQueryAdapter;
    }

    public async Task<IOperationResponse<UserCardExtEntity>> AddUserCardAsync(
        IAddUserCardXfrEntity addUserCard,
        CancellationToken cancellationToken)
        => await _userCardsCommandAdapter.AddUserCardAsync(addUserCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsBySetAsync(
        IUserCardsSetXfrEntity userCardsSet,
        CancellationToken cancellationToken)
        => await _userCardsQueryAdapter.UserCardsBySetAsync(userCardsSet, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardAsync(
        IUserCardXfrEntity userCard,
        CancellationToken cancellationToken)
        => await _userCardsQueryAdapter.UserCardAsync(userCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsByIdsAsync(
        IUserCardsByIdsXfrEntity userCards,
        CancellationToken cancellationToken)
        => await _userCardsQueryAdapter.UserCardsByIdsAsync(userCards, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsByArtistAsync(
        IUserCardsArtistXfrEntity userCardsArtist,
        CancellationToken cancellationToken)
        => await _userCardsQueryAdapter.UserCardsByArtistAsync(userCardsArtist, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsByNameAsync(
        IUserCardsNameXfrEntity userCardsName,
        CancellationToken cancellationToken)
        => await _userCardsQueryAdapter.UserCardsByNameAsync(userCardsName, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsForSigningAsync(
        IUserCardsForSigningXfrEntity userCardsForSigning,
        CancellationToken cancellationToken)
        => await _userCardsQueryAdapter.UserCardsForSigningAsync(userCardsForSigning, cancellationToken).ConfigureAwait(false);
}
