---
paths:
  - "csharp/src/**"
---

# Architecture Guide — Quick Reference

## 7-Layer Architecture

```
Request → App → Entry → Shared → Domain → Aggregator → Adapter → Infrastructure
                                                       ↓
                                            External Systems (DB, APIs)
```

### Layer Roles

| Layer | Role | Key Example |
|-------|------|-------------|
| **App** | GraphQL HTTP entry point | `CardQueryMethods.cs:42-46` |
| **Entry** | Validation, mapping, delegation | `UserEntryService.cs` |
| **Shared** | Cross-cutting abstractions | `IOperationResponse<T>` |
| **Domain** | Business rules & invariants | Domain services (Cards, Sets, Artists) |
| **Aggregator** | Multi-adapter orchestration | Coordinate adapters, build results |
| **Adapter** | External system integration | Database, API calls, Scryfall |
| **Infrastructure** | Low-level utilities | Cosmos operations, configuration |

**Read the codebase examples** rather than detailed docs—the code is the source of truth.

---

## Entity Transformation Pipeline

```
ArgEntity  →  [Entry validates]  →  ItrEntity  →  XfrEntity  →  Adapter
(App input)   [Mappers]             (Internal)    (Transfer)      ↓
                                                              ExtEntity
                                                                  ↓
OutEntity  ←  OufEntity  ←  [Aggregator maps]  ←  ExtEntity (from Adapter)
(App output)  (Output)
```

### Naming Conventions

Use these patterns consistently across layers:

- **App layer**: `*ArgEntity` (input), `*OutEntity` (output)
- **Entry/Domain/Aggregator**: `*ItrEntity` (internal), `*XfrEntity` (transfer to adapter)
- **Adapter**: `*ExtEntity` (external system format)
- **Aggregator output**: `*OufEntity` (output from aggregator)
- **Services**: `*EntryService`, `*DomainService`, `*AggregatorService`
- **Validators**: `*ArgEntityValidator`, `*ArgEntityValidatorContainer`
- **Mappers**: `*To*Mapper`
- **Responses**: `*ResponseModel` (union: Success|Failure)

See: `CLAUDE.md` (Naming Conventions section) for full table.

---

## Key Patterns

- **Constructor Chain (DI)** — See: `ArtistQueryMethods.cs:24-29`
- **Validator Container** — Many small validator classes composed together — see `actions/validators.md`
- **Explicit Mappers** — Every layer crossing gets a dedicated `ICreateMapper<TSource, TDestination>` — see `actions/mappers.md`
- **Enrichments** — Post-query data enrichment at the Entry layer — see `actions/enrichments.md`
- **Inquisition Pattern** — Parameterized Cosmos queries with strongly typed parameters — see `cosmos/cosmos-inquisition.md`
- **Null Object Pattern** — Use instead of null checks; trust objects to handle absence

**Explore the codebase** to see these patterns in action. Real examples are better teachers than descriptions.

---

## What to Read Next

### By layer
- **App/GraphQL layer** → `layers/app/folder-structure.md`, `layers/app/response-models.md`, `layers/app/authentication.md`, `layers/app/schema-extensions.md`, `layers/app/input-types.md`, `layers/app/error-handling.md`, `layers/app/startup-configuration.md`, `graphql-conventions.md`
- **Entry layer** → `layers/entry/folder-structure.md`, `layers/entry/apis.md`, `layers/entry/cqrs-queries.md`, `layers/entry/cqrs-commands.md`, `layers/entry/entities.md`
- **Domain layer** → `layers/domain/`
- **Aggregator layer** → `layers/aggregator/`
- **Adapter layer** → `layers/adapters/`

### By concern
- **Adding a GraphQL query?** → `graphql-conventions.md`
- **Writing tests?** → `testing-guide.md`
- **C# design principles?** → `csharp-code-style.md`
- **Writing validators?** → `actions/validators.md`
- **Writing enrichments?** → `actions/enrichments.md`
- **Error handling?** → `layers/app/error-handling.md`
- **Schema registration?** → `layers/app/schema-extensions.md`
- **Startup/configuration?** → `layers/app/startup-configuration.md`

### Domain-specific
- **Signing** — Artist autograph tracking: organizes user cards by artist for signing sessions. Query: `UserCardsForSigning`. Entities: `SigningResultOutEntity`, `SigningSetGroupOutEntity`, `SigningArtistGroupOutEntity`, `SigningCardOutEntity`. See: `Queries/UserCardsQueryMethods.cs`, `Entities/Types/Signing/`
