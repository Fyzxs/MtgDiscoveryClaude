# CLAUDE.md - MtgDiscovery

## Overview
This is a Magic: The Gathering collection management and tracking platform.

## Tech Stack
- .NET 10, ASP.NET Core GraphQL APIs
- CosmosDB
- CQRS Pattern
- Custom Validation for request validation
- MsTest + AwesomeAssertions for testing

## Full-Stack Architecture

The platform consists of:
- **Backend**: .NET 10.0 GraphQL API with layered MicroObjects architecture
- **Frontend**: React 19 client application with Material-UI components

### Backend Layered Architecture

The .NET solution implements a layered architecture following the intended data flow:

**Data Flow (Request → Response):**
1. **App Layer** (`App.MtgDiscovery.GraphQL`): Translate request into ArgEntity
2. **Entry Layer** (`Lib.MtgDiscovery.Entry`): Validates ArgEntity and maps to ItrEntity (In-Flow Internal)  
3. **Shared Layer** (`Lib.Shared.*`): Applies rules on the data (validation, filtering, transformation)
4. **Domain Layer** (`Lib.Domain.*`): Applies ALWAYS rules on the data (business logic)
5. **Aggregator Layer** (`Lib.Aggregator.*`): Knows what adapters to talk to, orchestrates data retrieval
6. **Adapter Layer** (`Lib.Adapter.*`): Maps ItrEntity to ExtEntity, calls external world, maps ExtEntity back to OufEntity (Out-Flow Internal)
7. **Infrastructure Layer** (`Lib.Cosmos`, `Lib.Universal`): Core infrastructure components

**Return Flow (Response ← Request):**
- Aggregator aggregates adapter responses
- Domain applies always rules
- Shared applies rules
- Entry maps OufEntity to OutEntity
- App translates OutEntity to response


**Entity Types by Layer:**
- **ArgEntity**: Argument entities from GraphQL/external input (App → Entry)
- **ItrEntity**: Internal transfer entities between layers (Entry ↔ Shared ↔ Domain ↔ Aggregator)
- **XfrEntity**: Transfer entities within adapter layer operations (used by adapter services)
- **ExtEntity**: External system entities from Cosmos DB (Cosmos documents)
- **OutEntity**: Output entities returned to GraphQL layer (Entry → App)
- **OufEntity**: Output from domain/aggregator layers (internal layer outputs before final mapping)

**Layer Details:**
- **App Layer**: GraphQL API endpoints using HotChocolate with JWT authentication (Auth0)
- **Entry Layer**: Service entry point, request validation, response formatting
- **Shared Layer**: Cross-cutting action patterns (filtering, validation, enrichment), operation responses, entity interfaces
- **Domain Layer**: Business logic and domain operations for Cards, Sets, Artists, User, UserCards, UserSetCards
- **Aggregator Layer**: Data aggregation and transformation, coordinates data retrieval from adapters
- **Adapter Layer**: External system integration (Cosmos DB via `Lib.Adapter.Scryfall.Cosmos`, Scryfall API ingestion)
- **Infrastructure Layer**: Core infrastructure components and utilities

### Key Architectural Patterns

#### MicroObjects Philosophy
The codebase follows MicroObjects principles with pragmatic DTO usage:
- Every concept has explicit representation through interfaces and classes
- Primitives wrapped in domain objects where appropriate, strings used in DTOs for simplicity
- No nulls - use Null Object pattern (except validators at boundaries which check for null)
- Immutable objects with `private readonly` fields
- Interface for every class (1:1 mapping)
- Constructor injection only (no logic in constructors)
- No public statics (except MonoState pattern, LoggerMessage attributes, framework requirements)
- No enums, no reflection at runtime
- Composition over inheritance
- Methods expose behavior, not data (no getters/setters except DTOs)

#### Frontend Patterns
The React application follows these architectural patterns:
- **Atomic Design**: Components organized by complexity and domain (atoms → molecules → organisms)
- **Material-UI System**: Primary UI framework using sx props for styling (Tailwind being phased out)
- **Component Composition**: Reusable components with clear prop interfaces
- **Context-Aware Display**: Components adapt based on CardContext (isOnSetPage, showCollectorInfo)
- **GraphQL Integration**: Apollo Client with generated types and hooks from codegen
- **Theme-Based Styling**: Custom MTG theme extending Material-UI with rarity colors and MTG-specific shadows
- **Responsive Design**: Mobile-first approach with adaptive layouts
- **Authentication**: Auth0 integration with JWT token management

#### Scope Rules
- **Public scope**: Only in `Apis` folders
- **Internal scope**: Everything outside `Apis` folders
- **Test projects**: Have `InternalsVisibleTo` access to source projects


## Code Style Requirements

### Backend (.NET)
- File-scoped namespaces
- No greater than operators (use `<` only)
- No boolean negation (`!`) - use `is false` or explicit inverse methods
- `ConfigureAwait(false)` on all async calls
- `init` setters for DTO-style classes
- No comments unless explicitly requested
- If statement bodies MUST be block bodies, or on a single line with braces
- Classes must be `sealed` or `abstract` (very few exceptions)
- Explicit types REQUIRED over `var` for readability

## Critical Patterns to Follow

2. **Always check existing patterns in neighboring files before implementing**
3. **Create interfaces before implementations**
4. **Balance MicroObjects with DTOs** - Wrap primitives in domain objects, but use strings in DTOs for simplicity
5. **Use marker classes for type safety without implementation**
7. **Service dependencies flow downward through layers** (Entry → Domain → Aggregator → Adapter)
8. **Use `IEntryService` for GraphQL to service layer communication**
9. **Authentication requires JWT claims principal injection** in GraphQL mutations (Auth0)
11. **Adapters must return `IOperationResponse<T>` from `Lib.Shared.Invocation`**, NOT `OpResponse<T>` from `Lib.Cosmos`
12. **Adapter exceptions should extend `OperationException`** from `Lib.Shared.Invocation.Exceptions`
13. **Use XfrEntity for adapter layer internal transfers**, not ItrEntity at adapter service boundaries
14. **Validators check for null at boundaries** - this is correct and necessary, not a violation of No Nulls principle