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
ArgEntity  →  [Entry validates]  →  ItrEntity
(App input)   [Mappers]             (Internal)

              →  OufEntity  →  OutEntity
                 (Adapter)      (App output)
```

### Naming Conventions

Use these patterns consistently across layers:

- **App layer**: `*ArgEntity` (input), `*OutEntity` (output)
- **Internal**: `*ItrEntity`
- **Adapter**: `*ExtEntity`, `*ExtArgs`, `*Item`
- **Services**: `*EntryService`, `*DomainService`, `*AggregatorService`
- **Validators**: `*ArgEntityValidator`, `*ArgEntityValidatorContainer`
- **Mappers**: `*To*Mapper`
- **Responses**: `*ResponseModel` (union: Success|Failure)

See: `CLAUDE.md` (Naming Conventions section) for full table.

---

## Key Patterns

- **Constructor Chain (DI)** — See: `ArtistQueryMethods.cs:24-29`
- **Validator Container** — Many small validator classes composed together
- **Explicit Mappers** — Every layer crossing gets a dedicated `ICreateMapper<TSource, TDestination>`
- **Inquisition Pattern** — Parameterized Cosmos queries with strongly typed parameters
- **Null Object Pattern** — Use instead of null checks; trust objects to handle absence

**Explore the codebase** to see these patterns in action. Real examples are better teachers than descriptions.

---

## What to Read Next

- **Adding a GraphQL query?** → `.claude/rules/graphql-conventions.md`
- **Writing tests?** → `.claude/rules/testing-guide.md`
- **C# design principles?** → `.claude/rules/microobjects-philosophy.md`
