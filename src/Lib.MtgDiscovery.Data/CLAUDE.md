# Lib.MtgDiscovery.Data CLAUDE.md

## Purpose
Data coordination layer orchestrating data operations across multiple aggregator services for cards, sets, artists, and users.

## Narrative Summary
This library serves as the coordination point between domain logic and data aggregation layers. It implements the composite pattern to unify multiple data service interfaces into a single data service contract. The layer routes data requests to appropriate specialized aggregator services while maintaining consistent operation response patterns and error handling across all data operations.

## Key Files
### Core Data Service
- `Apis/DataService.cs:10-40` - Main composite data service implementation
- `Apis/IDataService.cs:3-7` - Unified data service interface

### Specialized Service Interfaces
- `Apis/ICardDataService.cs` - Card data operations interface
- `Apis/ISetDataService.cs` - Set data operations interface
- `Apis/IArtistDataService.cs` - Artist data operations interface
- `Apis/IUserDataService.cs` - User data operations interface

### Service Implementations
- `Queries/CardDataService.cs` - Card data operations
- `Queries/SetDataService.cs` - Set data operations
- `Queries/ArtistDataService.cs` - Artist data operations
- `Commands/UserDataService.cs` - User data operations

## Architecture Pattern
### Composite Service Pattern
- Single `IDataService` interface aggregating multiple specialized interfaces
- Each specialized service handles domain-specific data operations
- Consistent operation response patterns across all services
- Constructor injection of specialized service implementations

### Service Composition
- `DataService:17-26` - Composes four specialized data services
- Delegation pattern routing calls to appropriate services
- Uniform async/await patterns with ConfigureAwait(false)
- Consistent logging injection across all services

## Data Operations
### Card Operations
- `CardsByIdsAsync` - Retrieve cards by ID collection
- `CardsBySetCodeAsync` - Retrieve cards for specific set
- `CardsByNameAsync` - Retrieve cards by name
- `CardNameSearchAsync` - Search card names by term

### Set Operations
- `SetsAsync` - Retrieve sets by ID collection
- `SetsByCodeAsync` - Retrieve sets by code collection
- `AllSetsAsync` - Retrieve all available sets

### Artist Operations
- Artist data retrieval and search operations
- Cards by artist relationship queries

### User Operations
- User registration and information management
- User data persistence and retrieval

## Service Organization
### Query Services (Read Operations)
- Located in `Queries/` directory
- Handle data retrieval operations
- Implement read-only data access patterns

### Command Services (Write Operations)
- Located in `Commands/` directory
- Handle data modification operations
- Implement write and update patterns

## Dependencies
- Domain Services: Card, Set, Artist, and User domain layers
- Aggregator Services: Specialized data aggregation layers
- Shared Models: `Lib.Shared.DataModels` - Entity interfaces
- Response Patterns: `Lib.Shared.Invocation` - Operation response types
- Logging: `Microsoft.Extensions.Logging` - Service logging

## Integration Points
### Consumes
- Domain Layer: Business logic services for each entity type
- Aggregator Layer: Data aggregation services
- Shared Contracts: ITR entity interfaces for data flow

### Provides
- Unified data service interface to entry layer
- Consistent operation response patterns
- Coordinated data operations across entity types
- Error handling and logging coordination

## Operation Response Pattern
### Consistent Response Types
- All operations return `IOperationResponse<T>`
- Success/failure handling through response objects
- Standardized error propagation across services
- Async/await patterns with proper ConfigureAwait usage

### Entity Collection Patterns
- Collection entities for bulk operations
- Search result entities for query operations
- Argument entities for request parameters

## Configuration
Data services configured through dependency injection:
- Logger instances for operation tracking
- Specialized service instances for delegation
- Domain service dependencies for business logic

## Key Patterns
- Composite pattern for service aggregation
- Delegation pattern for operation routing
- Operation Response pattern for consistent error handling
- Interface Segregation for specialized service contracts
- Dependency Injection for service composition

## Layer Responsibilities
- Coordinates data operations across multiple domains
- Routes requests to appropriate specialized services
- Maintains consistent response patterns
- Provides single entry point for all data operations
- Handles service composition and dependency management

## Related Documentation
- `../Lib.MtgDiscovery.Entry/CLAUDE.md` - Entry layer consumer
- `../Lib.Domain.Cards/CLAUDE.md` - Card domain operations
- `../Lib.Domain.User/CLAUDE.md` - User domain operations
- `../Lib.Aggregator.Cards/CLAUDE.md` - Card data aggregation
- `../Lib.Aggregator.User/CLAUDE.md` - User data aggregation