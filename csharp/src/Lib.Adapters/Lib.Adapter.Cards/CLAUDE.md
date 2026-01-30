# Lib.Adapter.Cards CLAUDE.md

## Purpose
Card data adapter layer providing abstraction over external storage systems and handling all storage-specific operations for card data retrieval.

## Narrative Summary
This library implements the adapter layer for card data operations, serving as the interface between the aggregation layer and external storage systems. It encapsulates all Cosmos DB-specific logic including query construction, storage entity handling, and data transformation. The adapter translates domain requests into storage operations, executes queries against Cosmos DB, and transforms storage-specific entities back into domain-compatible interfaces. This separation ensures the aggregator layer remains storage-agnostic and focused solely on orchestration.

## Key Files
### Core Adapter Interface
- `Apis/ICardAdapter.cs:7-13` - Card adapter interface defining storage operations
- `Apis/CardCosmosAdapter.cs` - Cosmos DB implementation of card adapter

### Internal Implementations
- `Internals/CardNameSearchResultItrEntity.cs` - Search result entity implementation
- `Internals/SuccessOpResponse.cs` - Successful operation response wrapper
- `Internals/FailureOpResponse.cs` - Failed operation response wrapper

### Exception Handling
- `Exceptions/CardAdapterException.cs` - Adapter-specific exception type

## Adapter Operations
### Storage Operations
- `GetCardsByIdsAsync:9` - Retrieve cards by ID collection from storage
- `GetCardsBySetCodeAsync:10` - Retrieve cards for specific set from storage
- `GetCardsByNameAsync:11` - Retrieve cards by exact name from storage
- `SearchCardNamesAsync:12` - Execute name search against storage indexes

### Data Transformation
- Maps ITR entity requests to storage query formats
- Transforms storage entities (EXT) back to ITR entities
- Handles storage-specific data structures and indexes
- Manages partition keys and document IDs

## Key Components
### CardCosmosAdapter
- Main adapter implementation for Cosmos DB
- Encapsulates all Cosmos-specific operations
- Uses Gophers for read operations
- Uses Inquisitors for query operations
- Handles GUID generation for partition keys
- Manages trigram processing for search

### Storage Integration
- Direct usage of Cosmos SDK components
- Query construction with QueryDefinition
- Partition key management
- Index optimization for search operations

## Data Flow Architecture
### Adapter Pipeline
1. Receives requests from aggregator as complete ItrEntity objects
2. Extracts primitives internally for external system interfaces
3. Constructs storage-specific queries (QueryDefinition)
4. Determines partition keys and document IDs
5. Executes operations through Gophers/Inquisitors
6. Transforms storage entities to ITR entities
7. Returns IOperationResponse with results or errors

### Entity Mapping Approach
Following MicroObjects principles, this adapter preserves value objects until the last moment:
- **Input**: Complete ItrEntity objects (ICardIdsItrEntity, ISetCodeItrEntity, etc.)
- **Internal Processing**: Primitive extraction only when interfacing with external systems
- **Storage Operations**: Uses extracted primitives for Cosmos DB queries and operations
- **Output**: ITR entity collections wrapped in IOperationResponse

This approach eliminates primitive obsession at layer boundaries and keeps value objects intact until external system interfaces require primitive values.

## Dependencies
- Cosmos Integration: `Lib.Adapter.Scryfall.Cosmos` - Cosmos operators
- Shared Mappers: `Lib.Aggregator.Scryfall.Shared` - Entity mappers
- Data Models: `Lib.Shared.DataModels` - Entity interfaces
- Shared Invocation: `Lib.Shared.Invocation` - IOperationResponse and base operations
- Identifiers: `Lib.Shared.Abstractions` - GUID generators
- Core Cosmos: `Lib.Cosmos` - Cosmos DB operations (Note: Use IOperationResponse from Lib.Shared.Invocation, not OpResponse from Cosmos)

## Storage Operations
### Cosmos DB Integration
- Card container point reads through Gophers
- Set index lookups for set-based queries
- Name-based queries using GUID partition keys
- Trigram-based search for partial name matching

### Query Optimization
- Batch operations for multiple card IDs
- Index utilization for set and name queries
- Server-side filtering for search operations
- Parallel task execution for bulk retrievals

## Search Implementation
### Trigram Search
- Normalizes search terms to lowercase letters
- Generates trigrams for search matching
- Queries trigram indexes with server-side filtering
- Ranks results by match count

### Name Resolution
- GUID generation from card names for partitioning
- Direct queries against name-partitioned containers
- Exact name matching with optimized indexes

## Integration Points
### Consumes
- Storage Layer: Cosmos DB operators and entities
- Shared Utilities: Entity mappers and transformers

### Provides
- Storage abstraction to aggregator layer
- Consistent IOperationResponse pattern for all operations (from Lib.Shared.Invocation)
- Storage-agnostic interface for card operations

## Exception Management
### Custom Exceptions
- `CardAdapterException` - Adapter-specific errors (extends OperationException)
- Wraps storage exceptions with context
- Provides detailed error messages
- Maintains exception chain for debugging

### Error Handling
- Cosmos connectivity issues wrapped appropriately
- Query execution errors with context
- Data transformation failures
- Not found scenarios handled gracefully

## Performance Optimization
### Query Efficiency
- Direct point reads for ID-based lookups
- Index usage for all query operations
- Batch processing with Task.WhenAll
- Connection pooling through Cosmos SDK

### Resource Management
- Reuses Cosmos client connections
- Efficient memory usage with streaming
- Minimal object allocation
- Optimized entity transformation

## Configuration
Adapter configured through dependency injection:
- Logger instance for operation tracking
- Cosmos Gophers and Inquisitors injected
- Entity mappers for transformation
- Connection managed by Cosmos operators

## Key Patterns
- Adapter pattern for external system abstraction
- Repository pattern for data access operations
- Mapper pattern for entity transformation
- Task-based asynchronous pattern for I/O
- Operation Response pattern using IOperationResponse from Lib.Shared.Invocation

## Architectural Benefits
- **Separation of Concerns**: Storage logic isolated from business logic
- **MicroObjects Compliance**: Preserves value objects and eliminates primitive obsession at layer boundaries
- **Testability**: Interface allows easy mocking for aggregator tests
- **Flexibility**: Can add new storage adapters without changing aggregator
- **Maintainability**: Storage changes don't affect aggregator or primitive extraction logic
- **Performance**: Optimized storage operations with minimal primitive extraction
- **Type Safety**: Complete ItrEntity objects provide compile-time safety until external system interface

## Related Documentation
- `../Lib.Aggregator.Cards/CLAUDE.md` - Aggregator layer consumer
- `../Lib.Adapter.Scryfall.Cosmos/CLAUDE.md` - Cosmos operators provider
- `../Lib.Cosmos/CLAUDE.md` - Core Cosmos infrastructure
- `../Architecture.md` - Overall adapter layer architecture