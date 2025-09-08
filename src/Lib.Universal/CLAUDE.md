# Lib.Universal CLAUDE.md

## Purpose
Core utilities library providing configuration, HTTP client, service locator, and primitive value objects for the entire MTG Discovery platform.

## Narrative Summary
This foundational library implements the universal infrastructure components required across all layers of the application. It provides the MonoState configuration system, HTTP client abstractions, dependency injection utilities, and MicroObjects primitive wrappers. The library follows strict MicroObjects principles with no public statics, immutable objects, and comprehensive value object wrapping of primitives.

## Key Files
### Configuration System
- `Configurations/MonoStateConfig.cs:7-37` - Singleton configuration provider
- `Configurations/IConfig.cs` - Configuration interface abstraction

### HTTP Infrastructure
- `Http/MonoStateHttpClient.cs` - HTTP client singleton implementation
- `Http/IHttpClient.cs` - HTTP client interface abstraction

### Service Location
- `Inversion/ServiceLocator.cs` - Dependency injection service locator

### Primitive Wrappers
- `Primitives/Url.cs` - URL value object wrapper
- `Primitives/ProvidedUrl.cs` - Provided URL implementation
- `Primitives/ToSystemType.cs` - Type conversion abstractions
- `Primitives/StringEqualityToSystemType.cs` - String equality comparisons

### Extensions
- `Extensions/LinqExtensions.cs` - LINQ utility extensions
- `Extensions/StringExtensions.cs` - String manipulation extensions
- `Extensions/ExceptionExtensions.cs` - Exception handling extensions

### Utilities
- `Utilities/GuidUtility.cs` - GUID generation and manipulation
- `Caching/MonoStateMemoryCache.cs` - Memory caching singleton

## Architecture Patterns
### MonoState Pattern
- Single configuration instance across application
- Thread-safe initialization with semaphore protection
- Prevents multiple configuration scenarios

### Value Object Pattern
- All primitives wrapped in domain objects
- Immutable value objects with behavior
- Type safety through explicit wrappers

### Service Locator Pattern
- Centralized dependency resolution
- Interface-based service registration
- Supports constructor injection patterns

## Configuration System
### MonoState Configuration
- `MonoStateConfig.SetConfiguration:12-24` - One-time configuration setup
- Thread-safe with semaphore protection
- Throws on duplicate configuration attempts
- Indexer access to configuration values

### Configuration Hierarchy
- Root configs use MonoStateConfig directly
- Nested configs use parent key + colon separator
- Config classes prefixed with "Config"

## Dependencies
- External: Microsoft.Extensions.Configuration, Microsoft.Extensions.Caching, Newtonsoft.Json
- Internal: No internal project dependencies (foundation library)

## Integration Points
### Provides
- Configuration system to all layers
- HTTP client abstractions
- Service location infrastructure
- Primitive value object wrappers
- Extension method utilities
- Caching infrastructure

### Configuration Usage Pattern
```csharp
// Root configuration
MonoStateConfig.SetConfiguration(configuration);

// Nested configuration
config["ParentKey:ChildKey"]
```

## Key Patterns
- MonoState pattern for singleton infrastructure
- Value Object pattern for primitive wrapping
- Interface Segregation for service abstractions
- Extension method pattern for utility functions
- Thread-safe initialization patterns

## MicroObjects Implementation
- No public static members (except MonoState infrastructure)
- All primitives wrapped in value objects
- Immutable objects with readonly fields
- Interface for every abstraction
- Constructor injection only
- No enums, no reflection usage

## Related Documentation
- All project CLAUDE.md files reference this foundation
- `../Lib.Cosmos/CLAUDE.md` - Configuration consumer
- Architecture.md - Overall system configuration patterns