# Architecture Guide — Quick Reference

## 7-Layer Architecture

```
Request → App → Entry → Shared → Domain → Aggregator → Adapter → Infrastructure
                                                       ↓
                                            External Systems (DB, APIs)
```

### Layer 1: App (`App.MtgDiscovery.GraphQL`)
**Role**: GraphQL HTTP entry point only

- Translate GraphQL input → **ArgEntity**
- Call **IEntryService**
- Map response → **ResponseModel** (success|failure union)
- Never contain: validation, mapping, business logic

**See**: Query example `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Queries/CardQueryMethods.cs:42-46`

### Layer 2: Entry (`Lib.MtgDiscovery.Entry`)
**Role**: Validation, mapping, delegation

- Validate **ArgEntity** (many small validator classes)
- Map: **ArgEntity** → **ItrEntity** (internal)
- Call domain/aggregator service
- Map: **OufEntity** → **OutEntity** (response)

**See**: Entry service pattern `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Queries/UserEntryService.cs`

### Layer 3: Shared (`Lib.Shared.*`)
**Role**: Cross-cutting abstractions

- Action patterns: **IValidatorAction**, **IFilterAction**, **IEnrichmentAction**
- **IOperationResponse<T>** for success/failure
- Entity interfaces: **I*ItrEntity**, **I*ArgEntity**, **I*OutEntity**

### Layer 4: Domain (`Lib.Domain.*`)
**Role**: ALWAYS rules (apply to all consumers)

- Business logic for Cards, Sets, Artists, Users
- Passthrough to Aggregator (ready for logic insertion)
- Universal constraints and invariants

### Layer 5: Aggregator (`Lib.Aggregator.*`)
**Role**: Know which adapters to call

- Orchestrate multiple adapter calls
- Aggregate responses
- Build collection entities
- No business logic, pure coordination

### Layer 6: Adapter (`Lib.Adapter.*`)
**Role**: External system integration

- Map **ItrEntity** → **ExtEntity** (external format)
- Call external API/DB
- Map **ExtEntity** → **OufEntity** (internal response)
- Handle external exceptions & variations

**Key Patterns**:
- **Inquisition Pattern** for parameterized Cosmos queries (strongly typed parameters)
- **Gopher** for read operations, **Scribe** for writes, **Inquisitor** for query execution
- Explicit mappers: `ScryfallCardItemToCardItemItrEntityMapper`

### Layer 7: Infrastructure (`Lib.Cosmos`, `Lib.Universal`)
**Role**: Low-level utilities

- Database/storage operations
- Configuration (MonoState pattern)
- Logging, telemetry
- Core abstractions: **ICosmosGopher<T>**, **ICosmosInquisitor**

---

## Entity Transformation Pipeline

```
ArgEntity  →  [Entry validates]  →  ItrEntity
(App input)   [Mappers]             (Internal)

                                  →  OufEntity  →  OutEntity
                                     (Adapter)      (App output)
```

### Naming Conventions

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

---

## Key Architectural Patterns

### Constructor Chain (DI without containers)
**See**: `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Queries/ArtistQueryMethods.cs:24-29` or `CLAUDE.md` (Dependency Inversion section)

### Validator Container (Many Small Classes)
**See**: Complete pattern in `Architecture_Layers_Patterns.md:212-262` (Validator Pattern Implementation)

### Explicit Mappers (No AutoMapper)
Every layer-crossing transformation gets a dedicated mapper class implementing `ICreateMapper<TSource, TDestination>`.

### Inquisition Pattern (Parameterized Queries)
**See**: Complete pattern in `Architecture_Layers_Patterns.md:746-874` (Inquisition Pattern)

---

## Avoiding Primitive Obsession & Nulls

Use `ToSystemType<T>` wrappers for domain concepts

For tightly-coupled data, use interface-based DTOs 

**No Nulls**: Use Null Object pattern instead of null checks. 

---

## Hard Rules

1. **Layer isolation**: Dependencies flow inward only
2. **No framework DI**: Use constructor chains
3. **No AutoMapper**: Explicit mapper classes only
4. **Async always**: Use `ConfigureAwait(false)`
5. **No statics**: Instance methods only (except Null Object pattern)
6. **Interface-first**: Every class has an interface in its hierarchy
7. **No GetSet**: Expose behavior, not data
8. **Sealed by default**: Only abstract for inheritance
9. **Immutable**: Use `init` setters, `private readonly` fields
10. **No nulls**: Use Null Object pattern or validation

---

## Testing & Configuration

### Configuration Management
Hierarchical, singleton-based configuration with colon separators.

---

## Error Handling

All operations return `IOperationResponse<T>` with proper error context. Errors propagate through layers maintaining context.

---

## Quick Lookup by Task

| Task | Guide |
|------|-------|
| **Adding a GraphQL query** | `.claude/rules/graphql-conventions.md` |
| **Adding validation** | Entry layer: `*ArgEntityValidator` classes composed in `*ArgEntityValidatorContainer` |
| **Adding a Cosmos query** | Use Inquisition pattern: `*ExtArgs`, `*Inquisition`, `*QueryDefinition` |
| **Adding a mapper** | Dedicated class: `SourceToDestinationMapper` implementing `ICreateMapper<>` |
| **Adding domain logic** | Domain layer service (currently passthrough, ready for logic) |
| **Adding a new external system** | Adapter layer with mapper: `ExternalItemToItrEntityMapper` |
| **Adding tests** | `.claude/rules/architecture-guide.md#testing--configuration` |
| **Handling errors** | Use `IOperationResponse<T>` with proper exception context |
| **Configuring settings** | Hierarchical MonoState config classes |
