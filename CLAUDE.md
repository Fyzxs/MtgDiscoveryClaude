# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Development Commands

### Build
```bash
# Build entire solution
dotnet build src/MtgDiscoveryVibe.sln

# Build specific project
dotnet build src/Lib.Cosmos/Lib.Cosmos.csproj
```

### Test
```bash
# Run all tests
dotnet test src/MtgDiscoveryVibe.sln

# Run specific test project
dotnet test src/Lib.Cosmos.Tests/Lib.Cosmos.Tests.csproj

# Run with coverage
dotnet test src/MtgDiscoveryVibe.sln --collect:"XPlat Code Coverage"

# Run single test
dotnet test src/Lib.Cosmos.Tests/Lib.Cosmos.Tests.csproj --filter "FullyQualifiedName~MethodName"
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
- Do not use System.Text.Json; use only Newtonsoft.

## Architecture Overview

This is a .NET 9.0 solution following the **MicroObjects** architectural pattern - an extreme OOP approach where every concept is explicitly represented as an object.

### Project Structure
- **Lib.Universal**: Core utilities including configuration, service locator, and base primitives
- **Lib.Cosmos**: Azure Cosmos DB integration library with full MicroObjects patterns
- **Lib.BlobStorage**: Azure Blob Storage integration library
- **TestConvenience.Core**: Testing utilities including fakes, type wrappers, and reflection helpers
- **Example.Core**: Example implementation showing patterns

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
- Methods expose behavior, not data (no getters/setters)

#### Configuration Pattern
Configuration classes follow a specific structure:
- Root configs use `MonoStateConfig` singleton
- Nested configs use parent key + colon separator
- Config classes prefixed with "Config" (e.g., `ConfigCosmosConfiguration`)
- Marker classes use semicolon syntax: `public abstract class Name : Base;`

#### Testing Architecture
- MSTest framework with AwesomeAssertions
- Self-contained tests (no test class variables)
- TypeWrapper pattern for testing classes with private constructors
- Fakes in `Fakes` folder with "Fake" suffix
- No mocks - use simple fake implementations

#### Scope Rules
- Public scope: Only in `Apis` folder
- Internal scope: Everything outside `Apis` folder
- Test projects have `InternalsVisibleTo` access

## Important Files to Reference

1. **CODING_CRITERIA.md**: Specific patterns for this codebase
2. **TESTING_GUIDELINES.md**: Unit testing conventions and patterns
3. **microobjects_coding_guidelines.md**: Complete MicroObjects philosophy and implementation guide

## Code Style Requirements

- File-scoped namespaces
- No greater than operators (use `<` only)
- No boolean negation (`!`) - use `is false` or explicit inverse methods
- `ConfigureAwait(false)` on all async calls
- `init` setters for DTO-style classes
- No comments unless explicitly requested
- Use `replace_all: true` when using Edit tool
- If statement bodies MUST be block bodies, or on a single line. If bodies should never not have braces and not be on the same line as the `if`

## Testing Requirements

When writing tests:
- Each test completely self-contained
- Use Arrange-Act-Assert pattern
- Return value named `actual` or use `_` discard
- Test naming: `MethodName_Scenario_ExpectedBehavior`
- Always verify fake invocation counts
- Use `ConfigureAwait(false)` on async calls

## Critical Patterns to Follow

1. **Never modify production code for test scenarios**
2. **Always check existing patterns in neighboring files before implementing**
3. **Create interfaces before implementations**
4. **Wrap all primitives in domain objects**
5. **Use marker classes for type safety without implementation**
6. **Follow the 3-build-failure limit before stopping**