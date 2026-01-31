using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.User;
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
/// - Output: Returns IUserSyncOufEntity with isFirstLogin flag
/// - Aggregator layer handles mapping from OufEntity to ItrEntity
/// Primitive extraction happens in the concrete implementation when interfacing with external systems.
/// </summary>
public interface IUserCommandAdapter
{
    Task<IOperationResponse<IUserSyncOufEntity>> RegisterUserAsync(IUserInfoItrEntity userInfo);
}
