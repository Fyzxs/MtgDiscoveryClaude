using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
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
/// - Output: Returns ITR entities for consistency with main service interface
/// - Internal mapping: Adapter implementations map from storage entities to ITR entities
/// Primitive extraction happens in the concrete implementation when interfacing with external systems.
/// </summary>
public interface IUserCommandAdapter
{
    Task<IOperationResponse<IUserInfoItrEntity>> RegisterUserAsync(IUserInfoItrEntity userInfo);
}
