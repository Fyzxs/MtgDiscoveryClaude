using System.Threading.Tasks;
using Lib.Adapter.UserCards.Commands;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserCards.Apis;

/// <summary>
/// Main user cards adapter service implementation following the passthrough pattern.
///
/// This service coordinates all user card collection-related adapter operations by delegating
/// to specialized adapters. Currently delegates to IUserCardsCommandAdapter for command
/// operations, but provides the architectural structure for future expansion.
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
internal sealed class UserCardsAdapterService : IUserCardsAdapterService
{
    private readonly IUserCardsCommandAdapter _userCardsCommandAdapter;

    public UserCardsAdapterService(ILogger logger) : this(new UserCardsCommandAdapter(logger))
    { }

    private UserCardsAdapterService(IUserCardsCommandAdapter userCardsCommandAdapter)
    {
        _userCardsCommandAdapter = userCardsCommandAdapter;
    }

    public async Task<IOperationResponse<IUserCardCollectionItrEntity>> AddUserCardAsync(IUserCardCollectionItrEntity userCard)
    {
        return await _userCardsCommandAdapter.AddUserCardAsync(userCard).ConfigureAwait(false);
    }
}
