# Lib.Domain.Cards CLAUDE.md

## Purpose
Card domain logic layer implementing business rules and operations for MTG card management, search, and retrieval.

## Narrative Summary
This library contains the core business logic for card operations in the MTG Discovery platform. It implements card search workflows, business rule validation, and coordinates card data operations. The domain layer processes card-related requests from higher layers and orchestrates data persistence through the data coordination layer, ensuring all business rules and constraints for card operations are properly enforced.

## Key Files
### Domain Service Interface
- `Apis/ICardDomainService.cs:7-13` - Card domain operations contract
- `Apis/CardDomainService.cs` - Card domain service implementation

### Query Operations
- `Queries/QueryCardDomainService.cs` - Specialized query operations
- `QueryCardsIds.cs` - Card ID query utilities

## Domain Operations
### Card Retrieval Operations
- `CardsByIdsAsync:9` - Retrieve cards by ID collection
- `CardsBySetCodeAsync:10` - Retrieve cards for specific set
- `CardsByNameAsync:11` - Retrieve cards by exact name match
- `CardNameSearchAsync:12` - Search cards by name substring

### Business Logic Areas
- Card collection management
- Search term processing and validation
- Result filtering and sorting
- Card relationship management

## Key Components
### Domain Service
- `ICardDomainService` - Interface defining card domain operations
- `CardDomainService` - Implementation of card business logic
- Query delegation to specialized query services
- Business rule enforcement for card operations

### Query Service
- `QueryCardDomainService` - Specialized card query operations
- Search optimization and result processing
- Card filtering and aggregation logic

## Dependencies
- Data Layer: `Lib.MtgDiscovery.Data` - Data coordination service
- Shared Models: `Lib.Shared.DataModels` - Card entity interfaces
- Response Patterns: `Lib.Shared.Invocation` - Operation responses
- Logging: `Microsoft.Extensions.Logging` - Domain operation tracking

## Business Logic
### Card Search Process
1. Receives search criteria from entry layer
2. Validates search terms and parameters
3. Applies business rules for search scope
4. Coordinates data retrieval through data layer
5. Processes and filters results based on business rules
6. Generates search response with card collections

### Business Rules
- Card name validation and normalization
- Set code format validation
- Search term length and format constraints
- Result set size limitations
- Card access permissions

## Search Operations
### Card Retrieval Patterns
- **By IDs**: Bulk retrieval for specific card identifiers
- **By Set Code**: All cards within a specific MTG set
- **By Name**: Exact name matching for card lookup
- **Name Search**: Substring search with relevance ranking

### Search Optimization
- Query efficiency through proper indexing strategies
- Result caching for common search patterns
- Search term preprocessing and normalization
- Relevance scoring for search results

## Integration Points
### Consumes
- Entry Layer: `Lib.MtgDiscovery.Entry` - Request coordination
- Data Layer: Card data persistence and retrieval operations

### Provides
- Card domain operations to entry layer
- Business rule enforcement for card operations
- Card search and retrieval workflows
- Query optimization and result processing

## Entity Handling
### Card Entity Operations
- `ICardItemCollectionItrEntity` - Card result collections
- `ICardNameSearchResultCollectionItrEntity` - Search result collections
- `ICardIdsItrEntity` - Card ID argument collections
- `ICardNameItrEntity` - Card name argument entities

### Input Validation
- Card ID format validation
- Set code format verification
- Search term length and character constraints
- Collection size limitations

## Configuration
Domain services configured through dependency injection:
- Data service instances for persistence operations
- Logger instances for domain operation tracking
- Query service instances for specialized operations

## Key Patterns
- Domain service pattern for business logic encapsulation
- Operation response pattern for consistent error handling
- Query object pattern for complex search operations
- MicroObjects pattern for value object wrapping
- Dependency injection for service composition

## Performance Considerations
- Efficient card lookup through proper indexing
- Result set pagination for large collections
- Search result caching strategies
- Query optimization through data layer

## Related Documentation
- `../Lib.MtgDiscovery.Entry/CLAUDE.md` - Entry layer coordination
- `../Lib.MtgDiscovery.Data/CLAUDE.md` - Data layer integration
- `../Lib.Aggregator.Cards/CLAUDE.md` - Card data aggregation
- `../Lib.Shared.DataModels/CLAUDE.md` - Card entity interfaces