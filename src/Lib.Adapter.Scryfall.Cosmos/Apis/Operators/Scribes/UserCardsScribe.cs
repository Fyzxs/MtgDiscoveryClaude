using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;

/// <summary>
/// Cosmos scribe operator for user card collection write operations.
/// Handles persistence of user card collection data to UserCards Cosmos container.
///
/// Design Pattern: Scribe operator following established Cosmos infrastructure patterns
/// Inherits from CosmosScribe to leverage common write operation functionality
/// and consistent error handling across all Cosmos write operations.
///
/// Container Integration:
/// Uses UserCardsCosmosContainer for all data persistence operations,
/// ensuring proper partitioning and document management for user card collections.
/// </summary>
public sealed class UserCardsScribe : CosmosScribe, IUserCardsScribe
{
    public UserCardsScribe(ILogger logger)
        : base(new UserCardsCosmosContainer(logger))
    { }
}
