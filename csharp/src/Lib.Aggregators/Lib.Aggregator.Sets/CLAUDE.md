# Lib.Aggregator.Sets CLAUDE.md

## Purpose
Set data aggregation layer coordinating data retrieval from Cosmos DB storage and transforming storage entities into domain-compatible set data models.

## Narrative Summary
This library implements the aggregation layer for set data operations, serving as the bridge between set domain business logic and storage adapter implementations. It coordinates data retrieval from Cosmos DB through the Scryfall adapter layer, transforms storage-specific entities into domain-compatible interfaces, and handles complex query operations including set collection retrieval and filtering. The aggregator manages data mapping, query optimization, and result transformation while maintaining consistent operation response patterns.

## Key Files
### Core Aggregator Service
- `Apis/SetAggregatorService.cs` - Main set aggregation service
- `Apis/ISetAggregatorService.cs:7-12` - Set aggregation interface

### Entity Implementations
- `Entities/SetIdsItrEntity.cs` - Set ID collection entity
- `Entities/SetItemCollectionItrEntity.cs` - Set result collection
- `Models/SetItemItrEntity.cs` - Individual set entity implementation
- `Models/SetGroupingItrEntity.cs` - Set grouping and organization

### Query Operations
- `Queries/QuerySetAggregatorService.cs` - Specialized query aggregation
- `Queries/Mappers/QuerySetsIdsToReadPointItemsMapper.cs` - ID to storage mapping
- `Queries/Mappers/QuerySetsCodesToReadPointItemsMapper.cs` - Code to storage mapping
- `Queries/Mappers/ScryfallSetItemToSetItemItrEntityMapper.cs` - Storage to domain mapping

### Set Models
- `Models/CollectorNumberRangeItrEntity.cs` - Collector number range tracking
- `Models/GroupingFiltersItrEntity.cs` - Set grouping and filtering

### Exception Handling
- `Exceptions/AggregatorOperationException.cs` - Aggregator-specific exceptions

## Aggregation Operations
### Set Retrieval
- `SetsAsync:9` - Aggregate sets by ID collection
- `SetsByCodeAsync:10` - Aggregate sets by code collection
- `AllSetsAsync:11` - Aggregate all available sets

### Data Transformation
- Storage entity to ITR entity mapping
- Set collection aggregation and result processing
- Set grouping and organization
- Error handling and exception wrapping

## Key Components
### Aggregator Service
- `ISetAggregatorService` - Interface for set data aggregation
- `SetAggregatorService` - Main aggregation service implementation
- Coordinates multiple storage operations
- Transforms and maps set entity data

### Query Service
- `QuerySetAggregatorService` - Specialized query aggregation
- Complex set query coordination and optimization
- Result processing and transformation
- Performance optimization for large set collections

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
- Set container operations through adapter layer
- Query optimization for set retrieval performance
- Result pagination and streaming for large catalogs
- Connection pooling and resource management

### Query Mapping
- Domain query parameters to Cosmos SQL queries
- Set code normalization and lookup optimization
- Result filtering and sorting optimization
- Error handling for storage operations

## Set Data Processing
### Set Collection Management
- Complete set catalog retrieval and processing
- Set filtering by various criteria (format, block, etc.)
- Set grouping and organizational structures
- Performance optimization for set browsing

### Set Information Aggregation
- Set metadata processing and validation
- Release date and block information aggregation
- Format legality tracking and processing
- Card count and completion status tracking

## Integration Points
### Consumes
- Domain Layer: Set domain service requests
- Storage Layer: Cosmos DB adapter operations
- Shared Utilities: Aggregation helper services

### Provides
- Set data aggregation to domain layer
- Storage abstraction and entity transformation
- Query optimization and result processing
- Consistent error handling and operation responses

## Exception Management
### Custom Exceptions
- `AggregatorOperationException` - Aggregation-specific errors
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
- Index utilization for set lookups
- Batch processing for bulk operations
- Result streaming for large set catalogs
- Query plan optimization through Cosmos SDK

### Caching Strategies
- Set catalog caching for common operations
- Entity transformation caching
- Connection pooling and reuse
- Memory management for large result sets

## Set Grouping and Organization
### Grouping Features
- Block-based set grouping
- Format-based organization
- Release date chronological ordering
- Set type classification

### Filtering Capabilities
- Format legality filtering
- Release date range filtering
- Set type filtering
- Block and expansion filtering

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
- `../Lib.Domain.Sets/CLAUDE.md` - Domain business logic consumer
- `../Lib.Adapter.Scryfall.Cosmos/CLAUDE.md` - Storage adapter integration
- `../Lib.Aggregator.Scryfall.Shared/CLAUDE.md` - Shared aggregation utilities
- `../Lib.Shared.DataModels/CLAUDE.md` - Entity interface definitions