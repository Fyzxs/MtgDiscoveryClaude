# C# Backend Guidelines - 7-Layer Architecture

This file provides guidance for work in `/csharp/src` (GraphQL backend, CLI tools, domain logic).

## 7-Layer Architecture Overview

```
Request → App → Entry → Shared → Domain → Aggregator → Adapter → Infrastructure
                                                      ↓
                                           External Systems (DB, APIs)
```

### Layer 1: App (App.MtgDiscovery.GraphQL)
**Role**: GraphQL HTTP entry point ONLY

Responsibilities:
- Translate GraphQL input → `ArgEntity`
- Call `IEntryService`
- Map response → `ResponseModel` (success|failure union)
- Never contain: validation, mapping, business logic

Example pattern: `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Queries/CardQueryMethods.cs:42-46`

### Layer 2: Entry (Lib.MtgDiscovery.Entry)
**Role**: Validation, mapping, delegation

Responsibilities:
- Validate `ArgEntity` (many small validator classes)
- Map: `ArgEntity` → `ItrEntity` (internal)
- Call domain/aggregator service
- Map: `OufEntity` → `OutEntity` (response)

Example pattern: `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Queries/UserEntryService.cs`

### Layer 3: Shared (Lib.Shared.*)
**Role**: Cross-cutting abstractions

Provides:
- Action patterns: `IValidatorAction`, `IFilterAction`, `IEnrichmentAction`
- `IOperationResponse<T>` for success/failure
- Entity interfaces: `I*ItrEntity`, `I*ArgEntity`, `I*OutEntity`

### Layer 4: Domain (Lib.Domain.*)
**Role**: ALWAYS rules (apply to all consumers)

Contains:
- Business logic for Cards, Sets, Artists, Users
- Passthrough to Aggregator (ready for logic insertion)
- Universal constraints and invariants

### Layer 5: Aggregator (Lib.Aggregator.*)
**Role**: Know which adapters to call

Responsibilities:
- Orchestrate multiple adapter calls
- Aggregate responses
- Build collection entities
- No business logic, pure coordination

### Layer 6: Adapter (Lib.Adapter.*)
**Role**: External system integration

Responsibilities:
- Map `ItrEntity` → `ExtEntity` (external format)
- Call external API/DB
- Map `ExtEntity` → `OufEntity` (internal response)
- Handle external exceptions & variations

Key Patterns:
- **Inquisition Pattern** for parameterized Cosmos queries (strongly typed parameters)
- **Gopher** for read operations
- **Scribe** for write operations
- **Inquisitor** for query execution
- Explicit mappers: e.g., `ScryfallCardItemToCardItemItrEntityMapper`

### Layer 7: Infrastructure (Lib.Cosmos, Lib.Universal)
**Role**: Low-level utilities

Provides:
- Database/storage operations
- Configuration (MonoState pattern)
- Logging, telemetry
- Core abstractions: `ICosmosGopher<T>`, `ICosmosInquisitor`

## Entity Transformation Pipeline

```
ArgEntity  →  [Entry validates]  →  ItrEntity
(App input)   [Mappers]             (Internal)

                                  →  OufEntity  →  OutEntity
                                     (Adapter)      (App output)
```

## Naming Conventions

| Layer | Pattern | Example |
|-------|---------|---------|
| App | `*ArgEntity` | `CardIdsArgEntity` |
| App | `*OutEntity` | `CardCollectionOutEntity` |
| Internal | `*ItrEntity` | `CardItemItrEntity` |
| Adapter | `*ExtEntity` / `*Item` | `ScryfallCardItem` |
| Adapter | `*ExtArgs` | `UserCardItemsBySetExtArgs` |
| Services | `*EntryService`, `*DomainService`, `*AggregatorService`, `*AdapterService` | `CardEntryService` |
| Validators | `*ArgEntityValidator`, `*ArgEntityValidatorContainer` | `CardIdsArgEntityValidatorContainer` |
| Mappers | `*To*Mapper` | `CardSearchTermArgToItrMapper` |
| Responses | `*ResponseModel` (union: Success\|Failure) | `CardResponseModel` |

## GraphQL Query/Mutation Implementation

### Request → Response Flow

1. GraphQL Input → `ArgEntity` (in Entities/Args/)
2. Query/Mutation Method → calls `IEntryService`
3. `IEntryService` → returns `IOperationResponse<OutEntity>`
4. Mapper → converts to `ResponseModel` (union: Success|Failure)
5. GraphQL Output → `ResponseModel` returned to client

### Layer 1: GraphQL Endpoint (Thin)
- `ArgEntity` example: `Entities/Args/CardIdsArgEntity.cs`
  - Pattern: Implement interface, public properties for input
- Query method example: `Queries/CardQueryMethods.cs:42-46`
  - Constructor pattern: `public (ILogger)` → `private` with `IEntryService` + mappers
  - Method signature: `async Task<ResponseModel> MethodName(ArgEntity arg)`
  - Decorators: `[GraphQLType]` + `[Authorize]` if needed
- Mutation method example: `Mutations/UserMutationMethods.cs:37-43`
  - Same pattern: inject services, call `_entryService`, map response

### Layer 2: Type Registration (Schemas)
- Reference file: `Schemas/ApiQueryExtensions.cs`
  - Pattern: `.AddTypeExtension<MethodClass>()`
  - Pattern: `.AddType<ArgEntityInputType>()` for each input
  - Pattern: `.AddType<ResponseModelUnionType>()` for each response

### Layer 3 & Below: Entry/Domain (Already Exists)
- Entry service interface: `Lib.MtgDiscovery.Entry/Apis/IEntryService.cs`
- Example entry service: `Lib.MtgDiscovery.Entry/Queries/UserEntryService.cs`
  - These handle validation, mapping, domain calls
  - GraphQL never touches these layers

### Key Rules for GraphQL
- GraphQL = Request/Response translation only
- No business logic, validation, or mapping in queries/mutations
- `ArgEntity` properties = GraphQL input fields (1:1)
- All errors come as `ResponseModel` (union type), never throw
- Always use `ConfigureAwait(false)` on async calls

### Real Examples to Copy From
- Complete query endpoint: `Queries/CardQueryMethods.cs` (lines 17-68)
- Complete mutation endpoint: `Mutations/UserMutationMethods.cs` (lines 17-45)
- Full type registration: `Schemas/ApiQueryExtensions.cs` (lines 17-72)
- Mapper usage: `Queries/CardQueryMethods.cs:45-46` or `Mutations/UserMutationMethods.cs:42-43`

## Dependency Inversion Pattern

Dependencies are constructed via constructor chains, never using framework DI containers.

Example: `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Queries/ArtistQueryMethods.cs:24-29`

## Validator Container Pattern

Many small validator classes composed in a container class:

Example: `Lib.MtgDiscovery.Entry.Tests/Commands/Collections/Validators/CreateCollectionArgEntityValidatorContainerTests.cs:13-34`

## Explicit Mappers

Every layer-crossing transformation gets a dedicated mapper class implementing `ICreateMapper<TSource, TDestination>`.

No AutoMapper allowed - all mappings are explicit and intentional.

## Inquisition Pattern for Cosmos Queries

Parameterized queries using strongly typed parameters:
- `*ExtArgs` - Query parameters
- `*Inquisition` - Query executor
- `*QueryDefinition` - Query definition

## Security
- No secrets in code
- Auth must be explicit on endpoints (no accidental public routes)
- CORS must be restrictive (no AllowAnyOrigin in prod configs)
