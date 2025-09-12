using System.Threading.Tasks;
using Lib.Adapter.User.Persistence;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.User.Apis;

/// <summary>
/// Main user adapter service implementation following the passthrough pattern.
/// 
/// This service coordinates all user-related adapter operations by delegating
/// to specialized adapters. Currently delegates to IUserPersistenceAdapter for all
/// operations, but provides the architectural structure for future expansion.
/// 
/// Future Expansion Examples:
///   - IUserCacheAdapter for caching layer
///   - IUserFallbackAdapter for redundancy
///   - IUserMetricsAdapter for telemetry
/// 
/// Pattern Consistency:
/// Matches EntryService, DomainService, and AggregatorService patterns
/// to maintain predictable architecture across all layers.
/// </summary>
public sealed class UserAdapterService : IUserAdapterService
{
    private readonly IUserPersistenceAdapter _userPersistenceAdapter;

    public UserAdapterService(ILogger logger) : this(new UserCosmosPersistenceAdapter(logger))
    { }

    private UserAdapterService(IUserPersistenceAdapter userPersistenceAdapter)
    {
        _userPersistenceAdapter = userPersistenceAdapter;
    }

    public async Task<IOperationResponse<IUserInfoItrEntity>> RegisterUserAsync(IUserInfoItrEntity userInfo)
    {
        return await _userPersistenceAdapter.RegisterUserAsync(userInfo).ConfigureAwait(false);
    }
}
