---
paths:
  - "csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Queries/**"
---

# Entry Query Services

## Router Class: `{Domain}EntryService`

The router implements the domain-specific query interface from `Apis/` and delegates each method to the appropriate operation service's `Execute()`.

### Pattern

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Queries/CardEntryService.cs`

Each method delegates to the corresponding operation's `Execute(input, cancellationToken)` — no logic in the router.

## Operation Services

These are targeted classes following single responsibility — each implements a single query behavior.

### Interface Pattern

All operation interfaces MUST inherit from `IOperationResponseService<TInput, TOutput>` — never define `Execute` manually.

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Queries/Cards/` (operation service interfaces)

### Standard Execute Flow

All query operation services follow this sequence:

1. **Validate** input via injected validator container
2. **Map** ArgEntity to ItrEntity via injected mapper
3. **Call** domain service with ItrEntity + CancellationToken
4. Check `response.IsFailure` — return `FailureOperationResponse` if failed
5. **Map** OufEntity to OutEntity via injected mapper
6. Optionally **enrich** OutEntity with additional data
7. Return `SuccessOperationResponse<TOutEntity>`

### Implementation

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Queries/Cards/CardsByIdsEntryService.cs`

### Standard Dependencies

Query operation services typically inject:

1. Domain service
2. Validator container
3. ArgToItr mapper
4. OufToOut mapper
5. Enrichment(s) — when user-specific data is needed

### Enrichment Step

Some queries enrich the core response with user-specific data (collection ownership, wishlist status). Enrichments:

- Run **after** the OufToOut mapping
- Are **optional** — only present when user context exists
- **Fail silently** — enrichment failure does not fail the query
- See `enrichments.md` for full pattern details

## Common Rules

1. **Constructor Chain**: Public `ILogger` → private dependencies
2. **ConfigureAwait(false)**: All async calls
3. **ArgEntity in, IOperationResponse<OutEntity> out**: Validate → Map → Domain → Map → Return
4. **No exceptions**: Domain failures pass through as `IOperationResponse`
5. **All operation interfaces inherit IOperationResponseService**: Never manually define `Execute`

## Reference Implementations

- **Router**: `Queries/CardEntryService.cs`
- **Operation with enrichment**: `Queries/Cards/CardsByIdsEntryService.cs`
- **Operation without enrichment**: `Queries/Sets/SetsByIdsEntryService.cs`
- **Collection queries**: `Queries/Collections/CollectionEntryQueryService.cs`
