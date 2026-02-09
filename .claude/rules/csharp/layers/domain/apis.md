---
paths:
  - "csharp/src/Lib.Domains/**/Apis/**"
---

# Domain APIs Folder

The `Apis/` folder is the **public contract** for a domain project. Everything here MUST be `public` scoped. Internal operation classes do NOT belong here.

## Files in Apis/

| File | Purpose |
|------|---------|
| `I{Domain}DomainService.cs` | Composite interface — inherits both command and query interfaces |
| `{Domain}DomainService.cs` | Passthrough facade — delegates to command/query routers |
| `I{Domain}CommandDomainService.cs` | Command interface — defines all command operations |
| `I{Domain}QueryDomainService.cs` | Query interface — defines all query operations |

For single-CQRS projects (query-only or command-only), the composite interface inherits only the relevant CQRS interface, and the facade has a single dependency.

## Composite Interface

The composite interface inherits from the CQRS-specific interfaces and defines NO methods itself — pure composition.

> **See:** `csharp/src/Lib.Domains/Lib.Domain.Collections/Apis/ICollectionsDomainService.cs`

For query-only projects:

> **See:** `csharp/src/Lib.Domains/Lib.Domain.Artists/Apis/ArtistDomainService.cs` (interface in same directory)

## Passthrough Facade Pattern

The `{Domain}DomainService` in `Apis/` is a **pure passthrough facade**. It constructs command/query router classes and delegates every method call directly. It MUST NOT contain any logic.

### Constructor Pattern

- **Both command + query**: 2 dependencies (command router + query router)
- **Query-only or command-only**: 1 dependency (single router)

### Implementation (Both CQRS)

> **See:** `csharp/src/Lib.Domains/Lib.Domain.Collections/Apis/CollectionsDomainService.cs`

### Implementation (Single CQRS)

> **See:** `csharp/src/Lib.Domains/Lib.Domain.Artists/Apis/ArtistDomainService.cs`

### Key Rules

1. Every method is a single `await` delegation with `.ConfigureAwait(false)`
2. No conditional logic, no mapping, no error handling — pure passthrough
3. Method signatures accept a single `IItrEntity` param plus `CancellationToken`
4. Return type is always `Task<IOperationResponse<IOufEntity>>`

## Method Contracts

All methods on the public `I{Domain}DomainService` MUST:
- Accept a single `IItrEntity` parameter plus `CancellationToken`
- Return `Task<IOperationResponse<IOufEntity>>`

Any specialized operation interfaces do not belong here.

## Reference Implementation

`Lib.Domain.Collections/Apis/` follows this pattern exactly and is the canonical reference.
