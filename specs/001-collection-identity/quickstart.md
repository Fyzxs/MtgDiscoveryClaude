# Quickstart: Collection Identity Architecture

**Phase**: 1 - Quickstart | **Date**: 2026-01-27

## Prerequisites

- .NET 9.0 SDK installed
- Node.js 18+ with npm
- Azure Cosmos DB Emulator or test account configured
- Auth0 development tenant configured
- Solution builds successfully: `dotnet build src/MtgDiscoveryVibe.sln`

## Implementation Order

The feature is organized into three epics that must be implemented sequentially. Within each epic, features should be implemented in order.

### Epic 1: Core Infrastructure (Backend)

**Start here.** All backend infrastructure before any frontend work.

```
1.1 Collection Entity Model (Cosmos + Shared interfaces + Entity classes)
1.2 Default Collection Creation (Hook into user registration)
1.3 Collection Entry Services (Validation + entry layer)
1.4 Collection Domain Services (Business logic)
1.5 Collection Aggregator Services (Data orchestration)
1.6 Collection Adapter Services (Cosmos persistence)
1.7 Collection Authorization (Role checking service)
1.8 Migrate UserCards Schema (Add collection_id)
1.9 GraphQL Schema (Types, queries, mutations)
1.10 DI Registration (Wire everything up)
1.11 Backend Testing (Unit + integration tests)
1.12 Collection Deletion (Delete flow across layers)
```

### Epic 2: Collection Selection UI (Frontend)

**After Epic 1 is complete and backend is stable.**

```
2.1 Frontend Collection State Management (CollectionManagementContext)
2.2 Collection GraphQL Queries (Frontend .graphql files + codegen)
2.3 Collection Selector Component (Header dropdown)
2.4 Create Collection Dialog
2.5 Collections Management Page
2.6 Collection Persistence (localStorage)
2.7 Update Card Operations (Use active collection)
```

### Epic 3: Collection Sharing (Full Stack)

**After Epic 2 is complete.**

```
3.1 Backend Sharing (Grant/revoke services)
3.2 Backend Sharing Queries (Access list, shared collections)
3.3 GraphQL Sharing Schema
3.4 Frontend Sharing Mutations
3.5 Grant Access UI
3.6 Access List UI
3.7 Shared Collections Display
3.8 Sharing Notifications
3.9 User ID Discovery (placeholder)
3.10 Ownership Transfer
```

## Key Patterns to Follow

### Creating a New Backend Project (e.g., Lib.Domain.Collections)

```bash
# From src/ directory
dotnet new classlib -n Lib.Domain.Collections -f net9.0
dotnet sln MtgDiscoveryVibe.sln add Lib.Domain.Collections/Lib.Domain.Collections.csproj

# Add project references
cd Lib.Domain.Collections
dotnet add reference ../Lib.Shared.DataModels/Lib.Shared.DataModels.csproj
dotnet add reference ../Lib.Shared.Invocation/Lib.Shared.Invocation.csproj
dotnet add reference ../Lib.Shared.Abstractions/Lib.Shared.Abstractions.csproj

# Add InternalsVisibleTo for test project
# In .csproj: <InternalsVisibleTo Include="Lib.Domain.Collections.Tests" />
```

### Creating a New Cosmos Container

Reference: `UserCardsCosmosContainer.cs` and `UserCardsCosmosContainerDefinition.cs`

1. Create container name class in `Cosmos/Containers/Names/`
2. Create container definition class in `Cosmos/Containers/Definitions/`
3. Create container class in `Cosmos/Containers/`
4. Create Gopher, Scribe, Inquisitor in `Apis/Operators/`

### Creating Entity Interfaces

Reference: `Lib.Shared.DataModels/Entities/`

1. Create interface in appropriate subfolder (Args/, Itrs/, Oufs/, Outs/)
2. Properties use `string` type (DTO pattern, not wrapped primitives)
3. Use `{ get; }` for interfaces, `{ get; init; }` for implementations

### Creating Validators

Reference: `CardIdsArgEntityValidatorContainer.cs`

1. Create validator container class that composes individual validators
2. Each validator is a separate sealed class
3. Container extends `ValidatorActionContainer<TInput, TOutput>`
4. Each validator implements typed behavior with typed error message

### Creating GraphQL Types

Reference: Constitution Section "GraphQL Development Standards"

1. Create union type class extending `UnionType`
2. Create success response type class
3. Create entity type class(es)
4. Register ALL types in schema extensions via `AddType<T>()`

## Build and Test

```bash
# Build
dotnet build src/MtgDiscoveryVibe.sln

# Run all tests
dotnet test src/MtgDiscoveryVibe.sln

# Run specific test project
dotnet test src/Lib.Domain.Collections.Tests/Lib.Domain.Collections.Tests.csproj

# Frontend codegen (after backend schema changes)
cd client && npm run codegen

# Frontend dev server
cd client && npm run dev
```

## Critical Reminders

- **Interfaces before implementations**: Always create the interface in Lib.Shared.DataModels first
- **ConfigureAwait(false)**: On every async call
- **Sealed classes**: All implementation classes must be sealed
- **Internal scope**: Everything outside Apis/ folders is internal
- **No enums**: Roles and types are strings with validation, not C# enums
- **Newtonsoft.Json only**: Never use System.Text.Json
- **Fakes over mocks**: Create fakes in Fakes/ folder for testing
- **3-build-failure limit**: Stop and report after 3 consecutive build failures
