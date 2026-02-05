using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserInfo;
using Lib.Adapter.User.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.User.Apis;

/// <summary>
/// Specialized adapter interface for user persistence operations.
///
/// This interface represents the persistence-specific adapter functionality,
/// separate from the main IUserAdapterService which coordinates all adapters.
///
/// Pattern: Main service inherits from specialized interfaces
///   IUserAdapterService : IUserPersistenceAdapter, IUserCacheAdapter, IUserMetricsAdapter
///
/// Design Decision: Public specialized interface
/// While concrete implementations are internal, the specialized interfaces are public
/// to allow the main service interface to inherit from them and provide a unified API.
///
/// Entity Mapping Approach:
/// - Input: IXfrEntity (transfer from aggregator)
/// - Output: ExtEntity (external representation)
/// - Aggregator layer handles mapping from ExtEntity to OufEntity
/// </summary>
public interface IUserCommandAdapter
{
    Task<IOperationResponse<UserInfoExtEntity>> RegisterUserAsync(IUserInfoXfrEntity userInfo, CancellationToken cancellationToken);
}
