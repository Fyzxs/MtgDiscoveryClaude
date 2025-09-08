# Lib.Shared.Abstractions CLAUDE.md

## Purpose
Core abstractions and interfaces for action patterns, filtering, validation, caching, mapping, and identifier generation used throughout the platform.

## Narrative Summary
This library defines the fundamental abstractions that enable consistent patterns across all layers of the MTG Discovery platform. It provides interface definitions for action-based processing patterns including filtering, validation, transformation, and enrichment. The library also includes identifier generation utilities, caching abstractions, and mapper interfaces that support the MicroObjects architecture while maintaining loose coupling between components.

## Key Files
### Action Pattern Interfaces
- `Actions/IFilterAction.cs:5-11` - Generic filtering action interface
- `Actions/IValidatorAction.cs` - Generic validation action interface
- `Actions/ITransformationAction.cs` - Data transformation action interface
- `Actions/IEnrichmentAction.cs` - Data enrichment action interface

### Action Result Interfaces
- `Actions/IFilterActionResult.cs` - Filter action result contract
- `Actions/IValidatorActionResult.cs` - Validator action result contract
- `Actions/FilterActionResult.cs` - Concrete filter result implementation
- `Actions/ValidatorActionResult.cs` - Concrete validator result implementation

### Action Container Framework
- `Actions/FilterActionContainer.cs` - Filter action composition
- `Actions/ValidatorActionContainer.cs` - Validator action composition
- `Actions/TransformationActionContainer.cs` - Transformation action composition
- `Actions/EnrichmentActionContainer.cs` - Enrichment action composition

### Container Abstractions
- `Actions/FilterContainer.cs` - Filter container implementation
- `Actions/ValidatorContainer.cs` - Validator container implementation

### Identifier Generation
- `Identifiers/ICardNameGuidGenerator.cs` - Card name GUID generation interface
- `Identifiers/CardNameGuidGenerator.cs` - Card name GUID generation implementation
- `Identifiers/CardNameGuid.cs` - Card name GUID value object

### Mapping Abstractions
- `Mappers/IMapper.cs` - Generic mapping interface
- `Mappers/ICreateMapper.cs` - Creation mapping interface

### Caching Abstractions
- `Cache/ICacheableEntity.cs` - Cacheable entity interface

## Action Pattern Framework
### Filter Actions
- `IFilterAction<TItem, TFailureStatus>:5-11` - Generic item filtering
- Constants for filter results: `ValueFilteredOut`, `ValueNotFilteredOut`
- Async filtering with result status tracking
- Composable filtering through container pattern

### Validation Actions
- Generic validation interface for any item type
- Validation result tracking with failure status
- Composable validation chains
- Async validation with proper error handling

### Transformation Actions
- Generic data transformation between types
- Chainable transformation operations
- Result tracking and error handling
- Composable transformation pipelines

### Enrichment Actions
- Data enrichment and augmentation patterns
- Async enrichment operations
- Composable enrichment chains
- Error handling and rollback support

## Container Pattern Implementation
### Action Composition
- Container classes compose multiple actions of the same type
- Sequential execution with short-circuit failure handling
- Result aggregation and status tracking
- Dependency injection friendly composition

### Container Benefits
- Reusable action composition
- Consistent execution patterns
- Simplified dependency management
- Testable action pipelines

## Identifier Generation System
### Card Name GUID Generation
- `CardNameGuidGenerator` - Deterministic GUID generation for card names
- `CardNameGuid` - Value object wrapper for generated GUIDs
- Consistent identifier generation across system
- Collision-resistant identifier creation

### Generation Patterns
- Deterministic generation for consistent results
- Namespace-based GUID generation
- Value object wrapping for type safety
- Interface abstraction for testability

## Mapping Abstractions
### Generic Mapping
- `IMapper<TSource, TDestination>` - Generic type mapping
- `ICreateMapper<TSource, TDestination>` - Creation-specific mapping
- Consistent mapping patterns across layers
- Dependency injection friendly interfaces

### Mapping Benefits
- Layer-to-layer data transformation
- Testable mapping logic
- Reusable mapping components
- Type-safe transformation contracts

## Caching Abstractions
### Cacheable Entity Pattern
- `ICacheableEntity` - Interface for cacheable data entities
- Consistent caching behavior across entity types
- Cache key generation and management
- Cache invalidation patterns

## Dependencies
- External: Standard .NET async/threading libraries
- Internal: No internal project dependencies (foundation library)
- Base Types: Generic interfaces and abstract classes only

## Integration Points
### Provides
- Action pattern interfaces to all layers
- Identifier generation utilities for domain objects
- Mapping abstractions for data transformation
- Caching interfaces for performance optimization
- Container patterns for composable operations

### Usage Patterns
- **Domain Layer**: Business rule validation and transformation
- **Data Layer**: Filtering and enrichment of data results
- **Adapter Layer**: Entity mapping and transformation
- **Service Layer**: Request validation and response filtering

## Key Patterns
- Action pattern for pluggable behavior
- Container pattern for action composition
- Interface Segregation for focused contracts
- Value Object pattern for identifiers
- Factory pattern for identifier generation
- Strategy pattern through action interfaces

## MicroObjects Implementation
### Interface Design
- Every abstraction has a corresponding interface
- Generic interfaces for reusable patterns
- Value objects for primitive wrapping
- Immutable action results
- Constructor injection support

### Action Result Consistency
- Consistent result interfaces across action types
- Boolean and status-based result patterns
- Error information preservation
- Async operation support throughout

## Async Action Support
### Async Patterns
- All action interfaces use `Task<>` return types
- Proper async/await patterns throughout
- ConfigureAwait(false) for library code
- Cancellation token support where appropriate

## Related Documentation
- All layer CLAUDE.md files reference these abstractions
- `../Lib.Universal/CLAUDE.md` - Base primitive types
- `../Lib.Shared.Invocation/CLAUDE.md` - Operation response patterns
- Architecture.md - Overall abstraction and pattern usage