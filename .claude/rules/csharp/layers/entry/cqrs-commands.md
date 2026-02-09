---
paths:
  - "csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Commands/**"
---

# Entry Command Services

## Router Class: `{Domain}EntryService`

The router implements the domain-specific command interface from `Apis/` and delegates each method to the appropriate operation service's `Execute()`.

### Pattern

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Commands/UserCardsEntryService.cs`

Each method delegates to the corresponding operation's `Execute(input, cancellationToken)` — no logic in the router.

## Operation Services

These are targeted classes following single responsibility — each implements a single command behavior.

### Interface Pattern

All operation interfaces MUST inherit from `IOperationResponseService<TInput, TOutput>` — never define `Execute` manually.

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Commands/UserCards/` (operation service interfaces)

### Standard Execute Flow

All command operation services follow this sequence:

1. **Validate** input via injected validator container
2. **Map** ArgEntity/ArgsEntity to ItrEntity via injected mapper
3. **Call** domain service with ItrEntity + CancellationToken
4. Check `response.IsFailure` — return `FailureOperationResponse` if failed
5. **Map** OufEntity to OutEntity via injected mapper
6. Return `SuccessOperationResponse<TOutEntity>`

### Standard Dependencies

Command operation services typically inject:

1. Domain service
2. Validator container
3. ArgToItr mapper
4. OufToOut mapper

## Combined ArgsEntity Pattern

Mutations that require authenticated user context combine `ClaimsPrincipal`-derived auth data with the GraphQL input into a single combined `IArgsEntity`:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Entities/IAddCardToCollectionArgsEntity.cs`

The GraphQL layer creates the combined entity via a dedicated mapper:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Actions/Mappers/AddCardToCollectionArgsMapper.cs`

The command operation service then validates the combined args and extracts what it needs:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Commands/Collections/CollectionEntryCommandService.cs`

## Cross-Domain Coordination

Some commands coordinate across multiple domain services (e.g., fetching card details before updating user cards). When a command needs data from another domain:

1. Call the secondary domain service
2. Extract needed metadata
3. Map to an enriched ItrEntity via a dedicated mapper
4. Call the primary domain service with the enriched entity

All mapping to ItrEntities MUST go through dedicated mapper classes — no inline entity construction.

## Collection Command Service Pattern

For domains with many command operations, a dedicated command service class handles all operations with their validators and mappers:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Commands/Collections/CollectionEntryCommandService.cs`

## Common Rules

1. **Constructor Chain**: Public `ILogger` → private dependencies
2. **ConfigureAwait(false)**: All async calls
3. **ArgsEntity in, IOperationResponse<OutEntity> out**: Validate → Map → Domain → Map → Return
4. **No exceptions**: Domain failures pass through as `IOperationResponse`
5. **All operation interfaces inherit IOperationResponseService**: Never manually define `Execute`
6. **All ItrEntity creation via mappers**: No inline entity construction in service methods

## Reference Implementations

- **Router**: `Commands/UserCardsEntryService.cs`
- **Operation service**: `Commands/UserCards/AddCardToCollectionEntryService.cs`
- **Collection commands**: `Commands/Collections/CollectionEntryCommandService.cs`
- **Default resource creation**: `Commands/Collections/DefaultCollectionCreator.cs`
