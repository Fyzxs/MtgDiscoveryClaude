using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators;

/// <summary>
/// Specialized interface for UserCards Cosmos scribe operations.
/// Extends ICosmosScribe to provide type-safe access to user card collection persistence operations.
///
/// Design Pattern: Specialized operator interface
/// While the concrete implementation inherits from CosmosScribe base class,
/// this interface provides a specific contract for UserCards operations
/// that can be used for dependency injection and testing.
///
/// Architecture Consistency:
/// Follows the same pattern as other specialized operator interfaces
/// in the Cosmos adapter layer for type safety and explicit contracts.
/// </summary>
public interface IUserCardsScribe : ICosmosScribe
{
    // All methods inherited from ICosmosScribe
    // Specialized interface serves for type safety and dependency injection
}
