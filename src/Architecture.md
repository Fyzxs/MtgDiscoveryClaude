# CCS Architecture Reference Guide

## Overview

This document serves as a comprehensive architecture reference that can be applied to new projects to ensure consistent, maintainable, and scalable software design. The architecture is based on a layered service approach with strict separation of concerns and dependency inversion principles.

## Core Architectural Principles

### 1. Separation of Concerns
- Each layer has a single, well-defined responsibility
- Dependencies flow inward (toward the domain)
- External concerns (HTTP, databases, etc.) are isolated at the boundaries

### 2. Explicit Type Declaration
- Prefer explicit types over `var` for better code readability and maintainability
- All properties for (de)serialization MUST have JsonPropertyAttribute
- JSON contracts MUST use lower_snake_case naming

### 3. Class Design Standards
- Classes MUST be declared `sealed` or `abstract` with very few exceptions
- Only classes intended for external consumption should be `public`
- Everything else should be at least `internal`
- Public APIs should be placed in dedicated `Apis` namespaces with XML documentation

### 4. Dependency Management
- Use centralized package management via `Directory.Packages.props`
- Maintain NuGet sources in `nuget.config`
- Projects use `Directory.Build.props` for configuration
- Remove `PropertyGroup` tags from individual projects

## Project Structure and Naming Conventions

### Naming Standards
- Project names should be PascalCase
- No ALLCAPS in project names
- Use `.` separators for logical grouping

### Project Prefixes
- `App.` - Entry point projects (HTTP endpoints, Azure Functions, etc.)
- `Lib.` - Class library projects
- `TestConvenience.` - Projects containing test utilities and fakes
- `Monitor.` - Projects focused on monitoring and telemetry
- `.Tests` - Test projects (suffixed to source project name)
- `.Shared` - Common components

## MTG Discovery Platform Layered Architecture

The MTG Discovery platform implements a specific layered architecture following the data flow pattern below:

### Data Flow Pattern

**Request Flow (Inbound):**
1. App → translate request into ArgEntity
2. Entry.Entry → validates ArgEntity and maps to ItrEntity
3. Shared → applies rules on the data
4. Domain → applies ALWAYS rules on the data
5. Aggregator → knows what adapters to talk to
6. Adapter → maps from ItrEntity to ExtEntity, calls external world, maps ExtEntity back to ItrEntity

**Response Flow (Outbound):**
7. Aggregator → aggregates adapter responses
8. Domain → applies always rules
9. Shared → applies rules
10. Entry → maps ItrEntity to OutEntity
11. App → translates OutEntity to response

### Layer Definitions

### Layer 1: App Layer (`App.MtgDiscovery.GraphQL`)
**Purpose**: Primary entry point for GraphQL requests

**Responsibilities**:
- GraphQL API endpoints using HotChocolate
- Authentication and authorization
- Translate requests into ArgEntity
- Translate OutEntity to response format
- HTTP-specific concerns and error handling

**Pattern**:
```csharp
[Query]
public async Task<CardSearchResponse> SearchCards(string searchTerm, ClaimsPrincipal user)
{
    var argEntity = new CardSearchTermArgEntity(searchTerm);
    var response = await _entryService.CardNameSearchAsync(argEntity);
    return MapToGraphQLResponse(response);
}
```

### Layer 2: Entry Layer (`Lib.MtgDiscovery.Entry`)
**Purpose**: Service entry point with validation and transformation

**Responsibilities**:
- Validate ArgEntity from App layer
- Map ArgEntity to ItrEntity for internal processing
- Map ItrEntity to OutEntity for response
- Request validation and business rule enforcement
- Response formatting and error handling

### Layer 3: Shared Layer (`Lib.Shared.*`)
**Purpose**: Apply cross-cutting rules and provide common abstractions

**Responsibilities**:
- Action patterns: filtering, validation, transformation, enrichment
- Operation response patterns and error handling
- Entity interfaces (ArgEntity, ItrEntity, OutEntity patterns)
- Cross-cutting concerns and shared utilities

**Components**:
- `Lib.Shared.Abstractions`: Action patterns and core interfaces
- `Lib.Shared.DataModels`: Entity interfaces and data contracts
- `Lib.Shared.Invocation`: Operation response patterns

### Layer 4: Domain Layer (`Lib.Domain.*`)
**Purpose**: Apply universal business logic and ALWAYS rules

**Responsibilities**:
- Business rules that apply to all consumers
- Domain-specific logic for Cards, Sets, Artists, Users
- Universal data validation and business constraints
- Domain entity behavior and invariants

**Components**:
- `Lib.Domain.Cards`: Card-specific business logic
- `Lib.Domain.Sets`: Set-specific business logic
- `Lib.Domain.Artists`: Artist-specific business logic
- `Lib.Domain.User`: User-specific business logic

### Layer 5: Aggregator Layer (`Lib.Aggregator.*`)
**Purpose**: Orchestrate data retrieval and know which adapters to call

**Responsibilities**:
- Determine which adapters to call for specific operations
- Orchestrate data retrieval from multiple sources
- Aggregate responses from multiple adapters
- Handle data composition and coordination

**Components**:
- `Lib.Aggregator.Cards`: Card data aggregation
- `Lib.Aggregator.Sets`: Set data aggregation
- `Lib.Aggregator.Artists`: Artist data aggregation
- `Lib.Aggregator.User`: User data aggregation

### Layer 6: Adapter Layer (`Lib.Adapter.*`)
**Purpose**: Interface with external systems and handle data transformation

**Responsibilities**:
- Map ItrEntity to ExtEntity for external system calls
- Communicate with external services (Scryfall API, Cosmos DB, Blob Storage)
- Map ExtEntity responses back to ItrEntity
- Handle external system protocols and error conditions
- Isolate external system specifics from internal layers

**Components**:
- `Lib.Adapter.Scryfall.Cosmos`: Cosmos DB integration
- `Lib.Adapter.Scryfall.BlobStorage`: Blob storage integration
- `Lib.Scryfall.Ingestion`: Scryfall API integration

### Layer 7: Infrastructure Layer (`Lib.Cosmos`, `Lib.Universal`)
**Purpose**: Provide core infrastructure and utilities

**Responsibilities**:
- Database connectivity and operations
- Configuration management
- Logging and telemetry
- Core utilities and primitive types
- Service locator and dependency injection support
- Base abstractions and foundational patterns

## Implementation Patterns

### Public API Design
- Public classes for external consumption must be in `Apis` namespaces
- All public APIs require XML documentation
- Use interface segregation - create specific interfaces for different concerns
- Implement composite interfaces that inherit from specific ones

### Execution Context Pattern
Implement `IExecutionContext` for passing execution-specific information:

```csharp
public interface IExecutionContext
{
    ILogger Logger { get; }
    // Can be enhanced with correlation IDs, user context, etc.
}
```

Pass through constructors for scoped services or method parameters for singletons.

### Service Layer Pattern
Each layer should expose services through well-defined interfaces:

```csharp
// In Apis namespace
public sealed class DomainLogicService : IDomainLogicService
{
    private readonly ISpecificService _specificService;
    
    public DomainLogicService(IExecutionContext exCtx) : this(
        new SpecificService(exCtx))
    { }
    
    private DomainLogicService(ISpecificService specificService)
    {
        _specificService = specificService;
    }
    
    public async Task<OperationResult> ProcessAsync(IDomain domain) =>
        await _specificService.ProcessAsync(domain).NoSync();
}
```

## Project Configuration

### Directory.Build.props Standards
```xml
<Project>
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>disable</ImplicitUsings>
    <Nullable>warnings</Nullable>
    <LangVersion>latest</LangVersion>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
    <InvariantGlobalization>true</InvariantGlobalization>
  </PropertyGroup>
</Project>
```

### Central Package Management
Use `Directory.Packages.props` for version management:

```xml
<Project>
  <ItemGroup>
    <PackageVersion Include="PackageName" Version="X.Y.Z" />
  </ItemGroup>
</Project>
```

### Test Project Standards
- Test projects MUST NOT reference other test projects
- All test projects go in `~zTests` solution folder
- Use consistent testing frameworks across the solution
- Test projects automatically have internals visible from source projects

## Development Workflow Standards

### Code Quality Standards
- Use Code Cleanup profiles in Visual Studio
- Enable "Run code cleanup profile on save"
- Treat warnings as errors
- Enable all analyzers and code analysis

### Testing Standards
- Aim for confidence, not coverage percentages
- Never use `[ExcludeFromCodeCoverage]` - make code testable instead
- Test projects follow `.Tests` naming convention
- Create test convenience projects for shared testing utilities

### Class Naming Convention
Classes should be suffixed with their interface or base class name:

```csharp
sealed class CarVehicle : IVehicle
sealed class DatabaseUserRepository : IUserRepository
```

## Technology Standards

### JSON Serialization
- All JSON property names MUST be lower_snake_case
- All serializable properties MUST have JsonPropertyAttribute
- Incoming and outgoing contracts follow same naming conventions

### Logging and Telemetry
- Use structured logging with proper correlation
- Implement execution context for tracing requests
- Include telemetry and monitoring from the start

### Error Handling
- Use consistent error handling patterns across layers
- Log errors at appropriate levels
- Implement proper exception boundaries

## Migration Guide for Existing Projects

### Step 1: Solution Structure
1. Create solution folders following the reference structure
2. Move projects into appropriate solution folders
3. Rename projects to follow naming conventions

### Step 2: Layer Separation
1. Identify current mixing of concerns
2. Extract layers following the reference architecture
3. Ensure dependencies flow inward only

### Step 3: Configuration Standardization
1. Implement `Directory.Build.props`
2. Move to central package management
3. Update NuGet.Config for consistent package sources

### Step 4: Code Standards Implementation
1. Apply class design standards (sealed/abstract)
2. Move public APIs to `Apis` namespaces
3. Implement execution context pattern

### Step 5: Testing Structure
1. Move all tests to `~zTests` solution folder
2. Ensure test projects don't reference each other
3. Create test convenience projects for shared utilities

## Benefits of This Architecture

### Maintainability
- Clear separation of concerns makes code easier to understand and modify
- Consistent patterns reduce cognitive load for developers
- Well-defined layer boundaries prevent architectural drift

### Testability
- Each layer can be tested in isolation
- Dependency inversion makes mocking straightforward
- Clear interfaces make unit testing simple

### Scalability
- New clients can be added without affecting existing ones
- Shared logic layers promote reuse
- Domain-driven organization supports team scaling

### Flexibility
- External system changes are isolated to adapter layers
- Client-specific requirements are contained
- Business logic can evolve independently of technical concerns

---

*This architecture reference is based on the CCS (Clean Code Service) pattern and represents battle-tested practices for building maintainable, scalable services. Apply these patterns consistently across projects for maximum benefit.*