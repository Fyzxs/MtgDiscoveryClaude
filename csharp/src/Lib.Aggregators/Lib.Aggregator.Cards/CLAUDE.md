# Lib.Aggregator.Cards CLAUDE.md

## Purpose
Card data aggregation layer orchestrating data retrieval from adapter implementations and coordinating responses for domain layer consumption.

## Narrative Summary
This library implements the aggregation layer for card data operations, serving as the orchestrator between domain business logic and adapter implementations. It coordinates data retrieval through the card adapter interface, aggregates responses from storage operations, and handles operation response wrapping for consistent error handling. The aggregator is storage-agnostic, delegating all storage-specific operations to the adapter layer while focusing on orchestration, aggregation, and response formatting for the domain layer.

## Key Files
### Core Aggregator Service
- `Apis/CardAggregatorService.cs` - Main card aggregation service
- `Apis/ICardAggregatorService.cs:7-13` - Card aggregation interface

### Entity Implementations
- `Entities/CardNameItrEntity.cs` - Card name entity implementation
- `Entities/CardNameSearchResultCollectionItrEntity.cs` - Search result collection
- `Entities/CardNameSearchResultItrEntity.cs` - Individual search result entity

### Query Operations
- `Queries/QueryCardAggregatorService.cs` - Specialized query aggregation using adapter
- `Queries/Mappers/QueryCardsIdsToReadPointItemsMapper.cs` - ID mapping utilities

### Exception Handling
- `Exceptions/CardAggregatorOperationException.cs` - Aggregator-specific exceptions

## Aggregation Operations
### Card Retrieval
- `CardsByIdsAsync:9` - Aggregate cards by ID collection
- `CardsBySetCodeAsync:10` - Aggregate cards for specific set
- `CardsByNameAsync:11` - Aggregate cards by exact name
- `CardNameSearchAsync:12` - Aggregate name search results

### Data Orchestration
- Adapter operation coordination
- Collection aggregation and result processing
- Operation response wrapping
- Error handling and exception management

## Key Components
### Aggregator Service
- `ICardAggregatorService` - Interface for card data aggregation
- `CardAggregatorService` - Main aggregation service implementation
- Delegates to QueryCardAggregatorService
- Maintains consistent interface for domain layer

### Query Service
- `QueryCardAggregatorService` - Adapter orchestration implementation
- Uses `ICardAdapter` for all storage operations
- Transforms adapter responses to operation responses
- Handles error scenarios and exception wrapping

## Data Flow Architecture
### Aggregation Pipeline
1. Receives domain requests through ITR entities
2. Passes complete ITR entities directly to adapter (preserving MicroObjects principles)
3. Delegates to adapter for storage operations and primitive extraction
4. Receives IOperationResponse from adapter
5. Wraps results with consistent error handling and collection formatting

### Entity Mapping Approach
Following MicroObjects principles, this aggregator preserves value objects at layer boundaries:
- **Input**: ITR entity interfaces from domain layer
- **Processing**: Direct delegation to adapter without primitive extraction
- **Adapter Interface**: Complete ITR entities passed to maintain value object integrity
- **Output**: OperationResponse with ITR entity collections

This approach eliminates primitive extraction at the aggregator layer, delegating that responsibility to the adapter layer where external system interfaces require primitive values.

## Dependencies
- Card Adapter: `Lib.Adapter.Cards` - Storage abstraction interface
- Shared Models: `Lib.Shared.DataModels` - Entity interfaces
- Response Patterns: `Lib.Shared.Invocation` - Operation responses
- Aggregator Shared: `Lib.Aggregator.Scryfall.Shared` - Common entities

## Adapter Integration
### Storage Abstraction
- All storage operations delegated to ICardAdapter
- Storage-agnostic orchestration logic
- Consistent interface regardless of storage implementation
- Adapter injection through dependency injection

### Response Transformation
- OpResponse to OperationResponse mapping
- Success and failure scenario handling
- Exception wrapping with context
- Consistent error messaging

## Orchestration Patterns
### Request Processing
- Receives complete ITR entities from domain layer
- Passes ITR entities directly to adapter (no primitive extraction)
- Delegates to appropriate adapter method with value object preservation
- Handles adapter response appropriately with collection formatting

### Result Aggregation
- Collection assembly from adapter results
- Response wrapper creation
- Error context enrichment
- Success response formatting

## Integration Points
### Consumes
- Domain Layer: Card domain service requests
- Adapter Layer: Card adapter operations
- Shared Utilities: Entity definitions and response patterns

### Provides
- Card data aggregation to domain layer
- Adapter orchestration and coordination
- Operation response wrapping
- Consistent error handling patterns

## Exception Management
### Custom Exceptions
- `CardAggregatorOperationException` - Aggregation-specific errors
- Storage error wrapping and context preservation
- Detailed error information for debugging
- Consistent exception handling patterns

### Error Scenarios
- Storage connectivity issues
- Query parsing and execution errors
- Data transformation failures
- Resource exhaustion and timeout handling

## Architectural Benefits
### Separation of Concerns
- Storage details isolated in adapter layer
- Aggregator focuses on orchestration only
- Clean dependency flow through layers
- Testable through adapter interface mocking

### Flexibility
- Can switch storage implementations via adapter
- Storage changes don't affect aggregator logic
- Multiple adapters can be used if needed
- Easy to add caching or other adapters

## Configuration
Aggregator services configured through dependency injection:
- Card adapter instance for storage operations
- Logger instances for operation tracking
- Query service delegation pattern
- Mapper utilities for request processing

## Key Patterns
- Aggregator pattern for adapter orchestration
- Delegation pattern for query service
- Operation Response pattern for consistent error handling
- Adapter pattern for storage abstraction
- Response transformation pattern

## Related Documentation
- `../Lib.Domain.Cards/CLAUDE.md` - Domain business logic consumer
- `../Lib.Adapter.Cards/CLAUDE.md` - Card adapter implementation
- `../Lib.Aggregator.Scryfall.Shared/CLAUDE.md` - Shared aggregation entities
- `../Lib.Shared.DataModels/CLAUDE.md` - Entity interface definitions
- `../Architecture.md` - Overall aggregator layer architecture