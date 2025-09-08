# Lib.Domain.Sets CLAUDE.md

## Purpose
Set domain logic layer implementing business rules and operations for MTG set management, retrieval, and collection operations.

## Narrative Summary
This library contains the core business logic for set operations in the MTG Discovery platform. It implements set retrieval workflows, business rule validation, and coordinates set data operations. The domain layer processes set-related requests from higher layers and orchestrates data persistence through the aggregator layer, ensuring all business rules and constraints for set operations are properly enforced.

## Key Files
### Domain Service Interface
- `Apis/ISetDomainService.cs:7-12` - Set domain operations contract
- `Apis/SetDomainService.cs` - Set domain service implementation

### Query Operations
- `Queries/QuerySetDomainService.cs` - Specialized set query operations

## Domain Operations
### Set Retrieval Operations
- `SetsAsync:9` - Retrieve sets by ID collection
- `SetsByCodeAsync:10` - Retrieve sets by code collection
- `AllSetsAsync:11` - Retrieve all available sets

### Business Logic Areas
- Set collection management
- Set code validation and normalization
- Set hierarchy and relationships
- Release date and format validation

## Key Components
### Domain Service
- `ISetDomainService` - Interface defining set domain operations
- `SetDomainService` - Implementation of set business logic
- Query delegation to specialized query services
- Business rule enforcement for set operations

### Query Service
- `QuerySetDomainService` - Specialized set query operations
- Set filtering and collection processing
- Set relationship management and validation

## Dependencies
- Aggregator Layer: `Lib.Aggregator.Sets` - Set data aggregation
- Shared Models: `Lib.Shared.DataModels` - Set entity interfaces
- Response Patterns: `Lib.Shared.Invocation` - Operation responses
- Logging: `Microsoft.Extensions.Logging` - Domain operation tracking

## Business Logic
### Set Retrieval Process
1. Receives set criteria from entry layer
2. Validates set identifiers and parameters
3. Applies business rules for set access
4. Coordinates data retrieval through aggregator layer
5. Processes and filters results based on business rules
6. Generates response with set collections

### Business Rules
- Set code format validation (3-letter codes, special formats)
- Set ID uniqueness verification
- Release date validation and constraints
- Format legality and availability rules
- Set type classification and validation

## Set Operations
### Retrieval Patterns
- **By IDs**: Bulk retrieval for specific set identifiers
- **By Codes**: Retrieval using MTG set codes (e.g., "RNA", "GRN")
- **All Sets**: Complete set catalog retrieval

### Set Validation
- Set code format verification (length, characters)
- Release date validation and chronological ordering
- Format classification (Standard, Modern, Legacy, etc.)
- Set type validation (Core, Expansion, Masters, etc.)

## Integration Points
### Consumes
- Entry Layer: `Lib.MtgDiscovery.Entry` - Request coordination
- Aggregator Layer: Set data aggregation and retrieval operations

### Provides
- Set domain operations to entry layer
- Business rule enforcement for set operations
- Set retrieval and collection workflows
- Set validation and normalization

## Entity Handling
### Set Entity Operations
- `ISetItemCollectionItrEntity` - Set result collections
- `ISetIdsItrEntity` - Set ID argument collections
- `ISetCodesItrEntity` - Set code argument collections

### Input Validation
- Set ID format validation
- Set code format verification (standard 3-letter codes)
- Collection size limitations
- Parameter null and empty checks

## Configuration
Domain services configured through dependency injection:
- Aggregator service instances for data operations
- Logger instances for domain operation tracking
- Query service instances for specialized operations

## Key Patterns
- Domain service pattern for business logic encapsulation
- Operation response pattern for consistent error handling
- Query object pattern for complex set operations
- MicroObjects pattern for value object wrapping
- Dependency injection for service composition

## Set Data Management
### Set Information Processing
- Set metadata validation and processing
- Release date and block information
- Format legality tracking
- Card count and completion tracking

### Set Relationships
- Block and set grouping relationships
- Format inclusion and rotation tracking
- Set dependency management
- Historical set data integrity

## Related Documentation
- `../Lib.MtgDiscovery.Entry/CLAUDE.md` - Entry layer coordination
- `../Lib.MtgDiscovery.Data/CLAUDE.md` - Data layer integration
- `../Lib.Aggregator.Sets/CLAUDE.md` - Set data aggregation
- `../Lib.Shared.DataModels/CLAUDE.md` - Set entity interfaces