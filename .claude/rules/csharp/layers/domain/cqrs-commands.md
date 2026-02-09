---
paths:
  - "csharp/src/Lib.Domains/**/Commands/*"
---

# Domain Command Services

## Router Class: `{Domain}CommandDomainService`

The `{Domain}CommandDomainService` implements the `I{Domain}CommandDomainService` interface defined in `Apis/`. It is always `internal sealed`.

### Naming

- Class: `{Domain}CommandDomainService` (internal)
- Implements: `I{Domain}CommandDomainService` (public interface from `Apis/`)

## Two Delegation Patterns

Domain command routers use one of two patterns depending on complexity.

### Pattern A: Direct Passthrough

The router injects a **single aggregator service** and delegates every method call directly to the corresponding aggregator method. No specialized operation services exist.

**Use when**: Each command is a simple 1:1 delegation to an aggregator method.

> **See:** `csharp/src/Lib.Domains/Lib.Domain.Collections/Commands/CollectionCommandDomainService.cs`

**Key characteristics:**
- Single dependency: the aggregator service
- Every method is `await _aggregatorService.{Method}(...).ConfigureAwait(false)`
- No specialized operation files in `Commands/`

### Pattern B: Specialized Operations

The router injects **multiple specialized single-method services** and delegates each method to the corresponding service's `Execute()`.

**Use when**: Commands need individual service isolation (separate aggregator dependencies per behavior, or multiple behaviors from different aggregators).

**Router:**

> **See:** `csharp/src/Lib.Domains/Lib.Domain.UserCards/Commands/UserCardsCommandDomainService.cs`

**Specialized operation:**

> **See:** `csharp/src/Lib.Domains/Lib.Domain.UserCards/Commands/AddUserCardDomain.cs`

**Key characteristics:**
- Router depends on multiple specialized operations (1 per behavior)
- Each specialized operation inherits `IOperationResponseService<TInput, TOutput>` — never define `Execute` manually
- Naming: `I{Behavior}Domain` / `{Behavior}Domain` (`Domain` suffix, not `DomainService`)
- Specialized operations inject the aggregator directly
- Router delegates via `_operation.Execute(input, cancellationToken)`

## Common Rules

Regardless of pattern:

1. **Constructor Chain**: Public `ILogger` → private dependencies
2. **Always Logic**: Any logic that must always be applied; is applied here. (no instances of yet)
3. **ConfigureAwait(false)**: All async calls
4. **ItrEntity in, IOperationResponse out**: No intermediate types
5. **No exceptions**: Aggregator failures pass through as `IOperationResponse`

## Reference Implementations

- **Direct passthrough**: `Lib.Domain.Collections/Commands/CollectionCommandDomainService.cs`
- **Specialized operations**: `Lib.Domain.UserCards/Commands/`
