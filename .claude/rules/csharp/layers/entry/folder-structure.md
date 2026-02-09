---
paths:
  - "csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/**"
---

# Entry Layer Folder Structure

## Canonical Layout

```
Lib.MtgDiscovery.Entry/
├── Apis/                                        (public contract)
│   ├── IEntryService.cs                             (composite interface — inherits all sub-service interfaces)
│   ├── EntryService.cs                              (passthrough facade)
│   ├── I{Domain}EntryService.cs                     (domain-specific sub-service interfaces)
│   ├── {Domain}EntryService.cs                      (simple sub-service implementations — when no CQRS split)
│   └── I{Arg}ArgEntity.cs                           (arg entity interfaces shared across domains)
│
├── Commands/                                    (command implementations)
│   ├── Actions/                                     (shared command actions)
│   │   ├── Mappers/                                     (ArgToItr mappers shared across commands)
│   │   └── Validators/                                  (validator containers shared across commands)
│   │       └── {Domain}/                                (domain-specific validator subfolders)
│   ├── Entities/                                    (ItrEntities specific to commands)
│   ├── {Domain}EntryService.cs                      (router — delegates to operation services)
│   └── {Domain}/                                    (domain-specific command folders)
│       ├── Apis/                                        (domain command interfaces — optional)
│       ├── Entities/                                    (domain-specific ItrEntities and ArgsEntities)
│       ├── Mappers/                                     (domain-specific ArgToItr mappers)
│       ├── Validators/                                  (domain-specific validators)
│       ├── I{Behavior}EntryService.cs                   (operation interface — extends IOperationResponseService)
│       └── {Behavior}EntryService.cs                    (operation implementation)
│
├── Queries/                                     (query implementations)
│   ├── Actions/                                     (shared query actions)
│   │   ├── Enrichments/                                 (post-query data enrichment)
│   │   ├── Integrators/                                 (query result merging)
│   │   ├── Mappers/                                     (OufToOut + ArgToItr mappers)
│   │   │   └── {Feature}/                               (feature-specific mapper subfolders)
│   │   └── Validators/                                  (query validators)
│   │       └── {Domain}/                                (domain-specific validator subfolders)
│   ├── Entities/                                    (ItrEntities specific to queries)
│   ├── {Domain}EntryService.cs                      (router — delegates to operation services)
│   └── {Domain}/                                    (domain-specific query folders)
│       ├── Apis/                                        (domain query interfaces — optional)
│       ├── Mappers/                                     (domain-specific OufToOut mappers)
│       ├── I{Behavior}EntryService.cs                   (operation interface — extends IOperationResponseService)
│       └── {Behavior}EntryService.cs                    (operation implementation)
│
└── Entities/                                    (entity definitions)
    ├── Itrs/                                        (Entry-layer ItrEntities organized by domain)
    │   └── {Domain}/
    ├── Outs/                                        (OutEntities organized by domain)
    │   ├── Artists/
    │   ├── Cards/
    │   ├── Collections/
    │   ├── Sets/
    │   ├── Signing/
    │   ├── User/
    │   ├── UserCards/
    │   ├── UserSetCards/
    │   └── UserWishlistCards/
    ├── {Domain}/                                    (combined ArgsEntities for specific domains)
    └── I{Combined}ArgsEntity.cs                     (combined arg entity interfaces at root)
```

## Key Rules

### Naming

The Entry layer uses scope-based suffix distinction, matching the Domain and Aggregator patterns:

- **Public scope** (`Apis/` + routers): Uses `EntryService` suffix
- **Internal scope** (operation services): Uses `EntryService` suffix with behavior prefix

| Scope | Pattern | Example |
|-------|---------|---------|
| Facade (public) | `EntryService` | `EntryService` |
| Sub-service interface (public) | `I{Domain}EntryService` | `ICardEntryService` |
| Router (internal) | `{Domain}EntryService` | `CardEntryService` |
| Operation interface (internal) | `I{Behavior}EntryService` | `ICardsByIdsEntryService` |
| Operation implementation (internal) | `{Behavior}EntryService` | `CardsByIdsEntryService` |

### CQRS Split

Commands and Queries are separated at the folder level. Some domains need both, others only one:

- **Query-only**: Cards, Sets, Artists, SealedProducts
- **Command-only**: User (registration)
- **Both**: UserCards, UserSetCards, UserWishlistCards, Collections, UserSealedProducts

### Actions Folders

The `Actions/` folder under both `Commands/` and `Queries/` holds shared cross-cutting concerns:

| Subfolder | Purpose | Location |
|-----------|---------|----------|
| `Mappers/` | ArgToItr, OufToOut, ArgToItr mappers | Both Commands and Queries |
| `Validators/` | Validator containers and individual validators | Both Commands and Queries |
| `Enrichments/` | Post-query data enrichment | Queries only |
| `Integrators/` | Query result merging | Queries only |

### Entity Location

- **ItrEntities**: `Commands/Entities/`, `Queries/Entities/`, or `Entities/Itrs/{Domain}/`
- **OutEntities**: Always in `Entities/Outs/{Domain}/`
- **Combined ArgsEntities**: `Entities/` root or `Entities/{Domain}/`

### Structure Flexibility

- Simple sub-services that need no CQRS split can live directly in `Apis/` (e.g., `SealedProductsEntryService`)
- Domain-specific subfolders under `Commands/` or `Queries/` may include `Apis/`, `Entities/`, `Mappers/`, `Validators/` only when needed
- Do not create empty subfolders for future use

### When Sub-Services Live in Apis/

A sub-service belongs in `Apis/` (rather than `Commands/` or `Queries/`) when ALL of these are true:

1. **Pure passthrough** — delegates to a single operation service with no logic
2. **No CQRS split needed** — the domain has only queries OR only commands, not both
3. **Few operations** — typically 1-2 methods

Examples:
- `SealedProductsEntryService` (Apis/) — one query method, pure passthrough to `SealedProductsBySetCodeEntryService`

When a sub-service needs logic, mapping, or error handling, it belongs in the appropriate `Commands/` or `Queries/` folder instead, even if there is no CQRS split.

## Reference Implementation

- **Query router**: `Queries/CardEntryService.cs`
- **Query operation**: `Queries/Cards/CardsByIdsEntryService.cs`
- **Command router**: `Commands/UserCardsEntryService.cs`
- **Command operation**: `Commands/UserCards/AddCardToCollectionEntryService.cs`
- **Collections (full CQRS)**: `Commands/Collections/` + `Queries/Collections/`
