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

## Layered Architecture

### Layer 1: App.[Client].Entry
**Purpose**: The primary entry point for client requests

**Responsibilities**:
- HTTP endpoints or Azure Function triggers
- Minimal logic - primarily routing and exception handling
- Construct argument containers from incoming requests
- Isolate external frameworks from business logic

**Pattern**:
```csharp
IResult EntryMethod(...) {
    try {
        var args = new SpecificCallRequestArgs(requestData);
        return DI.ClientDataService().MethodCall(..., args );
    } catch(Exception ex) {
        log.LogError(ex, "Something went wrong. Please look into it.");
    }
}
```

### Layer 2: Lib.[Client].Entry
**Purpose**: Support the App entry layer and keep it thin

**Responsibilities**:
- Additional class declarations that support the App layer
- Logic that would otherwise bloat the App layer
- Maintains separation between external frameworks and business logic

### Layer 3: Lib.[Client].Data
**Purpose**: Map between Service Domain Model and Client Response Model

**Responsibilities**:
- Transform Service Domain Model to Client-specific response format
- Call the Client Logic layer
- Handle client-specific data transformations

### Layer 4: Lib.[Client].Logic
**Purpose**: Apply client-specific business logic

**Responsibilities**:
- Client-specific business rules and logic
- Call appropriate shared logic layers
- Handle client-specific processing requirements

### Layer 5: Lib.Logic.[SharedScope]
**Purpose**: Shared logic across multiple clients

**Responsibilities**:
- Business logic common to multiple clients
- Call appropriate domain logic layers
- Reusable processing components

### Layer 6: Lib.Domain.Logic.[Domain]
**Purpose**: Apply universal business logic to domain data

**Responsibilities**:
- Business rules that apply to all consumers
- Receive Service Domain Model from Aggregator layer
- Universal data validation and processing

### Layer 7: Lib.Aggregator.[Domain]
**Purpose**: Construct Service Domain Model from multiple sources

**Responsibilities**:
- Orchestrate data retrieval from multiple adapters
- Map external models to internal Service Domain Model
- Handle data aggregation and composition
- Only handle queries (data flowing OUT)

**Note**: Commands go directly from Domain Logic to Adapter layers

### Layer 8: Lib.Adapter.[External].[Domain]
**Purpose**: Interface with external systems

**Responsibilities**:
- Communicate with external services, databases, files
- Isolate external system specifics
- Provide consistent internal interface
- Handle external system protocols and data formats

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