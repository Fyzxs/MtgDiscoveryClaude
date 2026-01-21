namespace Lib.Adapter.UserSetCards.Apis;

/// <summary>
/// Main adapter service interface for user set cards operations.
///
/// Design Decision: Composite interface pattern for unified service access
/// This interface inherits from all specialized adapter interfaces, providing a single
/// public contract that encompasses all user set cards adapter functionality.
///
/// Pattern Consistency:
/// Follows the same pattern as ISetAdapterService, ICardAdapterService, etc.
///
/// Specialized Interfaces:
/// - IUserSetCardQueryAdapter: Query operations (Get)
/// - IUserSetCardCommandAdapter: Command operations (Upsert)
///
/// Entity Mapping Approach:
/// This interface accepts complete XfrEntity objects following the layered architecture pattern.
/// The adapter implementation handles internal primitive extraction when needed for external system calls.
/// </summary>
public interface IUserSetCardsAdapterService : IUserSetCardsQueryAdapter, IUserSetCardsCommandAdapter
{
    // All method signatures inherited from IUserSetCardCommandAdapter
    // Future specialized interfaces (IUserSetCardQueryAdapter, etc.)
    // will be added to the inheritance list above
}
