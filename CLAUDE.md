# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Development Commands

### Build
```bash
# Build entire solution
dotnet build src/MtgDiscoveryVibe.sln

# Build specific project
dotnet build src/Lib.Cosmos/Lib.Cosmos.csproj

# Build GraphQL API
dotnet build src/App.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL.csproj
```

### Test
```bash
# Run all tests
dotnet test src/MtgDiscoveryVibe.sln

# Run specific test project
dotnet test src/Lib.Cosmos.Tests/Lib.Cosmos.Tests.csproj

# Run with coverage
dotnet test src/MtgDiscoveryVibe.sln --collect:"XPlat Code Coverage"

# Run single test by method name
dotnet test src/Lib.Cosmos.Tests/Lib.Cosmos.Tests.csproj --filter "FullyQualifiedName~MethodName"

# Run tests for a specific class
dotnet test --filter "ClassName"

# Run tests with detailed output showing individual test results (from src directory)
dotnet vstest ProjectName.Tests/bin/Debug/net9.0/ProjectName.Tests.dll --logger:"console;verbosity=normal"
```

### Run Applications
```bash
# Run GraphQL API
dotnet run --project src/App.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL.csproj

# Run example applications
dotnet run --project src/Example.Core/Example.Core.csproj
dotnet run --project src/Example.Scryfall.CosmosIngestion/Example.Scryfall.CosmosIngestion.csproj
```

### Clean
```bash
dotnet clean src/MtgDiscoveryVibe.sln
```

### Update NuGet Packages
```powershell
# From src directory
./slnNugetUpdater.ps1
```

## Dependency Management

- Use dotnet commands to add nuget packages, not editing of files
- Do not use System.Text.Json; use only Newtonsoft.Json
- When adding project references: `dotnet add reference ../ProjectName/ProjectName.csproj`

## Architecture Overview

This is a Magic: The Gathering collection management platform built on .NET 9.0 following the **MicroObjects** architectural pattern - an extreme OOP approach where every concept is explicitly represented as an object.

### Layered Architecture

The solution implements a layered architecture with clear separation of concerns:

1. **Entry Layer** (`Lib.MtgDiscovery.Entry`): Service entry point, request validation, response formatting
2. **Domain Layer** (`Lib.Domain.Cards`): Business logic and domain operations  
3. **Data Layer** (`Lib.MtgDiscovery.Data`): Data access coordination
4. **Aggregator Layer** (`Lib.Aggregator.Cards`): Data aggregation and transformation
5. **Adapter Layer** (`Lib.Adapter.Scryfall.*`): External system integration (Cosmos DB, Blob Storage)
6. **Infrastructure Layer** (`Lib.Cosmos`, `Lib.BlobStorage`): Core infrastructure components
7. **API Layer** (`App.MtgDiscovery.GraphQL`): GraphQL API endpoints

### Project Structure

- **App.MtgDiscovery.GraphQL**: GraphQL API using HotChocolate
- **Lib.Universal**: Core utilities including configuration, service locator, and base primitives
- **Lib.Cosmos**: Azure Cosmos DB integration library with full MicroObjects patterns
- **Lib.BlobStorage**: Azure Blob Storage integration library
- **Lib.Adapter.Scryfall.Cosmos**: Scryfall data models for Cosmos DB
- **Lib.Adapter.Scryfall.BlobStorage**: Scryfall image storage adapters
- **Lib.Scryfall.Ingestion**: Scryfall API client and data ingestion
- **Lib.MtgDiscovery.Entry**: Entry service layer with validation
- **Lib.Domain.Cards**: Card domain logic
- **Lib.MtgDiscovery.Data**: Data service layer
- **Lib.Aggregator.Cards**: Card data aggregation
- **Lib.Shared.***: Shared abstractions and data models
- **TestConvenience.Core**: Testing utilities including fakes, type wrappers, and reflection helpers
- **Example.***: Example applications demonstrating specific functionality

### Key Architectural Patterns

#### MicroObjects Philosophy
The codebase follows strict MicroObjects principles:
- Every concept has explicit representation through interfaces and classes
- No primitives - everything wrapped in domain objects
- No nulls - use Null Object pattern
- Immutable objects with `private readonly` fields
- Interface for every class (1:1 mapping)
- Constructor injection only (no logic in constructors)
- No public statics, no enums, no reflection
- Composition over inheritance
- Methods expose behavior, not data (no getters/setters except DTOs)

#### Configuration Pattern
Configuration classes follow a specific structure:
- Root configs use `MonoStateConfig` singleton
- Nested configs use parent key + colon separator
- Config classes prefixed with "Config" (e.g., `ConfigCosmosConfiguration`)
- Marker classes use semicolon syntax: `public abstract class Name : Base;`
- Configuration hierarchy: Root → Account → Container/Database

#### Testing Architecture
- MSTest framework with AwesomeAssertions
- Self-contained tests (no test class variables)
- TypeWrapper pattern for testing classes with private constructors
- Fakes in `Fakes` folder with "Fake" suffix
- No mocks - use simple fake implementations
- Test naming: `MethodName_Scenario_ExpectedBehavior`

#### GraphQL Schema Patterns
- Union types for polymorphic responses (success/failure)
- Separate ObjectType classes for each entity type
- Schema extensions in `Schemas` folder
- Query methods in separate classes with `[ExtendObjectType]` attribute
- Response models extend `ResponseModel` base class

#### Scope Rules
- Public scope: Only in `Apis` folder
- Internal scope: Everything outside `Apis` folder
- Test projects have `InternalsVisibleTo` access

## Important Files to Reference

1. **CODING_CRITERIA.md**: Specific patterns for this codebase
2. **TESTING_GUIDELINES.md**: Unit testing conventions and patterns
3. **microobjects_coding_guidelines.md**: Complete MicroObjects philosophy and implementation guide
4. **PRD.md**: Product requirements and feature specifications
5. **Architecture.md**: Detailed system architecture documentation

## Code Style Requirements

- File-scoped namespaces
- No greater than operators (use `<` only)
- No boolean negation (`!`) - use `is false` or explicit inverse methods
- `ConfigureAwait(false)` on all async calls
- `init` setters for DTO-style classes
- No comments unless explicitly requested
- Use `replace_all: true` when using Edit tool
- If statement bodies MUST be block bodies, or on a single line with braces

### Pragma Directives
- **Avoid #pragma directives** - they accumulate as technical debt
- When reviewing code, check if existing #pragma directives are still necessary
- Remove any #pragma that suppresses warnings that no longer occur
- If you must add a #pragma:
  - Include a comment explaining why it's necessary
  - Scope it as narrowly as possible (line-specific, not file-wide)
  - Plan to remove it in the future
- Common unnecessary pragmas to remove:
  - Formatting suppressions (IDE0055) - fix the formatting instead
  - Warnings for code that's been refactored
  - Overly broad suppressions that hide real issues

## Testing Requirements

When writing tests:
- Each test completely self-contained
- Use Arrange-Act-Assert pattern
- Return value named `actual` or use `_` discard
- Test naming: `MethodName_Scenario_ExpectedBehavior`
- Always verify fake invocation counts
- Use `ConfigureAwait(false)` on async calls
- Create `InstanceWrapper : TypeWrapper<ClassUnderTest>` for testing classes with private constructors

## GraphQL Development

### Query Structure
- Queries return union types for different response scenarios
- Use `... on TypeName` fragments to access type-specific fields
- Always include `__typename` for type discrimination
- Response types: `SuccessDataResponseModel<T>`, `FailureResponseModel`

### Adding New Queries
1. Create query method in `Queries` folder with `[ExtendObjectType]`
2. Define ObjectType classes for response entities in `Entities/Types`
3. Register types in schema extensions
4. Create mappers for data transformation

## Critical Patterns to Follow

1. **Never modify production code for test scenarios**
2. **Always check existing patterns in neighboring files before implementing**
3. **Create interfaces before implementations**
4. **Wrap all primitives in domain objects**
5. **Use marker classes for type safety without implementation**
6. **Follow the 3-build-failure limit before stopping**
7. **Service dependencies flow downward through layers (Entry → Domain → Data → Aggregator → Adapter)**
8. **Use `IEntryService` for GraphQL to service layer communication**
## Sessions System Behaviors

@CLAUDE.sessions.md
