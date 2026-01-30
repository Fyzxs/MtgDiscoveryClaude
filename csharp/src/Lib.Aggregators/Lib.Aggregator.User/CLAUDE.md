# Lib.Aggregator.User CLAUDE.md

## Purpose
User data aggregation layer coordinating user data operations and transformations between domain and adapter layers.

## Narrative Summary
This library implements the aggregator pattern for user data operations, serving as the coordination layer between the domain business logic and the adapter infrastructure. It handles user data aggregation, transformation, and persistence coordination. The aggregator layer receives user operation requests from the domain layer and orchestrates the appropriate adapter operations for data storage and retrieval.

## Key Files
- `Apis/IUserAggregatorService.cs` - Aggregator service interface
- `Apis/UserAggregatorService.cs` - Aggregator service implementation

## Aggregation Operations
### User Data Coordination
- Coordinates user data persistence through Cosmos DB adapters
- Handles user information aggregation and transformation
- Manages user registration data flow
- Provides user data retrieval operations

## Key Components
### Aggregator Service
- `IUserAggregatorService` - Interface for user data aggregation
- `UserAggregatorService` - Implementation of aggregation logic
- User data transformation between domain and storage models
- Adapter coordination for persistence operations

## Dependencies
- Adapter Layer: `Lib.Adapter.Scryfall.Cosmos` - Data persistence
- Shared Models: `Lib.Shared.DataModels` - Entity interfaces  
- User Info: `Lib.Shared.UserInfo` - User value objects
- Response Patterns: `Lib.Shared.Invocation` - Operation responses

## Data Flow
### User Registration Aggregation
1. Receives user registration request from domain layer
2. Transforms domain entities to storage entities
3. Coordinates with Cosmos DB adapter for persistence
4. Handles storage success and failure responses
5. Returns aggregated response to domain layer

### Data Transformation
- Maps domain user entities to Cosmos DB storage models
- Preserves data integrity during transformation
- Maintains MicroObjects patterns for value objects

## Integration Points
### Consumes
- Domain Layer: `Lib.Domain.User` - Business logic requests
- Adapter Layer: Cosmos DB operations for user data

### Provides
- User data aggregation to domain layer
- Storage coordination and management
- Data transformation services

## Storage Coordination
- Coordinates with `UserInfoScribe` for user data persistence
- Manages `UserInfoItem` entity storage in Cosmos DB
- Handles user data retrieval and query operations

## Configuration
Aggregator services configured through dependency injection:
- Adapter service instances for storage operations
- Logger instances for aggregation tracking
- Storage container and connection management

## Key Patterns
- Aggregator pattern for data coordination
- Transformation pattern for entity mapping
- Operation response pattern for consistent error handling
- Dependency injection for adapter service composition
- MicroObjects pattern for value object preservation

## Related Documentation
- `../Lib.Domain.User/CLAUDE.md` - User domain logic
- `../Lib.Adapter.Scryfall.Cosmos/CLAUDE.md` - Storage adapter
- `../Lib.MtgDiscovery.Data/CLAUDE.md` - Data layer coordination