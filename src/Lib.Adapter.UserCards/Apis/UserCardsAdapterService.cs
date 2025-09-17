using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.UserCards.Commands;
using Lib.Adapter.UserCards.Queries;
using Lib.Shared.DataModels.Entities;
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

    public async Task<IOperationResponse<IUserCardItrEntity>> AddUserCardAsync(IUserCardItrEntity userCard)
    {
        return await _userCardsCommandAdapter.AddUserCardAsync(userCard).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<IUserCardItrEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet)
    {
        return await _userCardsQueryAdapter.UserCardsBySetAsync(userCardsSet).ConfigureAwait(false);
    }
}
