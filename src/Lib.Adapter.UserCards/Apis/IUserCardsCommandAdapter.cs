using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserCards.Apis;

/// <summary>
/// Specialized adapter interface for user card collection command operations.
///
/// This interface represents the command-specific adapter functionality,
/// separate from the main IUserCardsAdapterService which coordinates all adapters.
///
/// Pattern: Main service inherits from specialized interfaces
///   IUserCardsAdapterService : IUserCardsCommandAdapter, IUserCardsCacheAdapter, IUserCardsMetricsAdapter
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
public interface IUserCardsCommandAdapter
{
    /// <summary>
    /// Adds a card to a user's collection with the specified collection details.
    /// </summary>
    /// <param name="userCard">The user card collection information to add</param>
    /// <returns>The added user card collection information wrapped in an operation response</returns>
    Task<IOperationResponse<IUserCardCollectionItrEntity>> AddUserCardAsync(IUserCardCollectionItrEntity userCard);
}
