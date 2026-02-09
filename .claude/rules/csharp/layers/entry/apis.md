---
paths:
  - "csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Apis/**"
---

# Entry APIs Folder

The `Apis/` folder is the **public contract** for the Entry layer. Everything here MUST be `public` scoped. Internal operation classes do NOT belong here.

## Files in Apis/

| File | Purpose |
|------|---------|
| `IEntryService.cs` | Composite interface — inherits all domain-specific sub-service interfaces |
| `EntryService.cs` | Passthrough facade — delegates to routers and sub-services |
| `I{Domain}EntryService.cs` | Domain-specific interfaces (e.g., `ICardEntryService`) |
| `{Domain}EntryService.cs` | Simple sub-services when no CQRS split is needed |
| `I{Arg}ArgEntity.cs` | Arg entity interfaces shared across domains |

## Composite Interface

The composite interface inherits from all domain-specific sub-service interfaces and defines NO methods itself — pure composition.

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Apis/IEntryService.cs`

## Passthrough Facade Pattern

`EntryService` is a **pure passthrough facade**. It constructs all sub-services and delegates every method call directly. It MUST NOT contain any logic.

### Constructor Pattern

The facade constructs all sub-services via constructor chaining:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Apis/EntryService.cs`

### Key Rules

1. Every method is a single `await` delegation with `.ConfigureAwait(false)`
2. No conditional logic, no mapping, no error handling — pure passthrough
3. Method signatures accept arg entity interfaces plus `CancellationToken`
4. Return type is always `Task<IOperationResponse<TOutEntity>>`

## Domain Sub-Service Interfaces

Each domain has a dedicated interface defining its operations:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Apis/` (domain-specific sub-service interfaces)

For domains with CQRS split, separate command and query interfaces exist:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Apis/` (command/query split interfaces)

## Simple Sub-Services

When a domain needs no CQRS split and has few operations, the sub-service can live directly in `Apis/`:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Apis/SealedProductsEntryService.cs`

## Method Contracts

All methods on public Entry interfaces MUST:
- Accept arg entity interface(s) plus `CancellationToken`
- Return `Task<IOperationResponse<TOutEntity>>`

## Reference Implementation

`Lib.MtgDiscovery.Entry/Apis/EntryService.cs` is the canonical reference for the facade pattern.
