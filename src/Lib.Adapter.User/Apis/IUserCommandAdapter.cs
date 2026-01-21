using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities.Itrs;
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
/// - Input: Preserves ItrEntity parameters following MicroObjects principles
/// - Output: Returns ExtEntity types from storage systems
/// - Aggregator layer handles mapping from ExtEntity to ItrEntity
/// Primitive extraction happens in the concrete implementation when interfacing with external systems.
/// </summary>
public interface IUserCommandAdapter
{
    Task<IOperationResponse<UserInfoExtEntity>> RegisterUserAsync(IUserInfoItrEntity userInfo);
}
