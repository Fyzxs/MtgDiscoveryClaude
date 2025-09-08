# Lib.Aggregator.Cards CLAUDE.md

## Purpose
Card data aggregation layer coordinating data retrieval from Cosmos DB storage and transforming storage entities into domain-compatible data models.

## Narrative Summary
This library implements the aggregation layer for card data operations, serving as the bridge between domain business logic and storage adapter implementations. It coordinates data retrieval from Cosmos DB through the Scryfall adapter layer, transforms storage-specific entities into domain-compatible interfaces, and handles complex query operations including card search and collection retrieval. The aggregator manages data mapping, query optimization, and result transformation while maintaining consistent operation response patterns.

## Key Files
### Core Aggregator Service
- `Apis/CardAggregatorService.cs` - Main card aggregation service
- `Apis/ICardAggregatorService.cs:7-13` - Card aggregation interface

### Entity Implementations
- `Entities/CardNameItrEntity.cs` - Card name entity implementation
- `Entities/CardNameSearchResultCollectionItrEntity.cs` - Search result collection
- `Entities/CardNameSearchResultItrEntity.cs` - Individual search result entity

### Query Operations
- `Queries/QueryCardAggregatorService.cs` - Specialized query aggregation
- `Queries/Mappers/QueryCardsIdsToReadPointItemsMapper.cs` - ID to storage mapping

### Exception Handling
- `Exceptions/CardAggregatorOperationException.cs` - Aggregator-specific exceptions

## Aggregation Operations
### Card Retrieval
- `CardsByIdsAsync:9` - Aggregate cards by ID collection
- `CardsBySetCodeAsync:10` - Aggregate cards for specific set
- `CardsByNameAsync:11` - Aggregate cards by exact name
- `CardNameSearchAsync:12` - Aggregate name search results

### Data Transformation
- Storage entity to ITR entity mapping
- Collection aggregation and result processing
- Search result ranking and filtering
- Error handling and exception wrapping

## Key Components
### Aggregator Service
- `ICardAggregatorService` - Interface for card data aggregation
- `CardAggregatorService` - Main aggregation service implementation
- Coordinates multiple storage operations
- Transforms and maps entity data

### Query Service
- `QueryCardAggregatorService` - Specialized query aggregation
- Complex query coordination and optimization
- Result processing and transformation
- Performance optimization for large datasets

## Data Flow Architecture
### Aggregation Pipeline
1. Receives domain requests through ITR entities
2. Maps request parameters to storage query formats
3. Coordinates data retrieval from Cosmos adapter layer
4. Transforms storage entities to domain entities
5. Aggregates results into collection entities
6. Handles errors and wraps in operation responses

### Entity Mapping
- **Input**: ITR entity interfaces from domain layer
- **Processing**: Storage entity retrieval and transformation
- **Output**: ITR entity collections for domain consumption

## Dependencies
- Storage Adapter: `Lib.Adapter.Scryfall.Cosmos` - Cosmos DB operations
- Shared Models: `Lib.Shared.DataModels` - Entity interfaces
- Response Patterns: `Lib.Shared.Invocation` - Operation responses
- Aggregator Shared: `Lib.Aggregator.Scryfall.Shared` - Common utilities

## Storage Integration
### Cosmos DB Coordination
- Card container operations through adapter layer
- Query optimization for performance
- Result pagination and streaming
- Connection pooling and resource management

### Query Mapping
- Domain query parameters to Cosmos SQL queries
- Search term processing and index utilization
- Result filtering and sorting optimization
- Error handling for storage operations

## Search Implementation
### Name Search Processing
- Search term normalization and preprocessing
- Fuzzy matching and relevance scoring
- Result ranking and pagination
- Performance optimization for partial matches

### Collection Retrieval
- Bulk card retrieval optimization
- Set-based filtering and aggregation
- Memory-efficient result streaming
- Concurrent query execution where appropriate

## Integration Points
### Consumes
- Domain Layer: Card domain service requests
- Storage Layer: Cosmos DB adapter operations
- Shared Utilities: Aggregation helper services

### Provides
- Card data aggregation to domain layer
- Storage abstraction and entity transformation
- Query optimization and result processing
- Consistent error handling and operation responses

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

## Performance Optimization
### Query Efficiency
- Index utilization for card lookups
- Batch processing for bulk operations
- Result streaming for large collections
- Query plan optimization through Cosmos SDK

### Caching Strategies
- Query result caching for common searches
- Entity transformation caching
- Connection pooling and reuse
- Memory management for large result sets

## Configuration
Aggregator services configured through dependency injection:
- Cosmos adapter instances for storage operations
- Logger instances for aggregation tracking
- Query service instances for specialized operations
- Shared utility services for common operations

## Key Patterns
- Aggregator pattern for data coordination
- Mapper pattern for entity transformation
- Operation Response pattern for consistent error handling
- Query Object pattern for complex operations
- Factory pattern for entity creation

## Related Documentation
- `../Lib.Domain.Cards/CLAUDE.md` - Domain business logic consumer
- `../Lib.Adapter.Scryfall.Cosmos/CLAUDE.md` - Storage adapter integration
- `../Lib.Aggregator.Scryfall.Shared/CLAUDE.md` - Shared aggregation utilities
- `../Lib.Shared.DataModels/CLAUDE.md` - Entity interface definitions