---
paths:
  - "csharp/src/**/Enrichments/**"
---

# Enrichment Pattern

## Purpose

Enrichments **add supplementary data to query results** after the primary query completes. They enrich OutEntities in-place with data from secondary domain services (e.g., adding user collection ownership data to card query results).

## Key Characteristics

- Run **after** OufToOut mapping, on OutEntity lists
- **Fail silently** — enrichment failure does not fail the parent query
- **Mutate in-place** — modify target list items directly
- **Check preconditions** — skip enrichment when context data is absent
- Use **Integrators** to merge enrichment data into the target list

## Naming Convention

| Type | Pattern | Example |
|------|---------|---------|
| Composite interface | `I{Concern}Enrichment` | `IUserCardEnrichment` |
| Composite implementation | `{Concern}Enrichment` | `UserCardEnrichment` |
| Specialized interface | `I{Concern}By{Strategy}Enrichment` | `IUserCardByIdsEnrichment` |
| Specialized implementation | `{Concern}By{Strategy}Enrichment` | `UserCardByIdsEnrichment` |

## Specialized Enrichment

Each specialized enrichment handles a single data fetch strategy:

### Interface

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Queries/Actions/Enrichments/UserCardByIdsEnrichment.cs`

**Key points:**
- Return type is `Task` (void) — enrichment modifies `target` in-place
- Parameters: target list to enrich, context with lookup keys, CancellationToken

### Implementation

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Queries/Actions/Enrichments/UserCardByIdsEnrichment.cs`

### Execute Flow

1. **Check preconditions** — return early if context doesn't have required data
2. **Map** target list + context to ItrEntity for secondary domain query
3. **Call** secondary domain service
4. **Check failure** — return silently on failure (do not propagate error)
5. **Integrate** enrichment data into target list via Integrator

### Standard Dependencies (3)

1. Domain service (secondary data source)
2. Mapper (target + context → ItrEntity)
3. Integrator (merge enrichment data into target)

## Composite Enrichment

When a concern has multiple strategies, a composite delegates to specialized enrichments:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Queries/Actions/Enrichments/UserCardEnrichment.cs`

## Entry-Layer Integrator

Enrichments use **Entry-layer Integrators** to merge data. These are distinct from Adapter-layer Integrators:

| | Entry Integrator | Adapter Integrator |
|---|---|---|
| Purpose | Merge query enrichment data into OutEntities | Merge delta into state for persistence |
| Input | `List<OutEntity>` + `IEnumerable<OufEntity>` | `ExtEntity` + `XfrEntity` |
| Output | Modified `List<OutEntity>` | Modified `ExtEntity` |
| Location | `Queries/Actions/Integrators/` | `{Adapter}/Commands/Integrators/` |

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Queries/Actions/Integrators/UserCardCollectionIntegrator.cs`

## Location

All enrichment files live in `Queries/Actions/Enrichments/`.

## Existing Implementations

| Enrichment | Enriches | With |
|------------|----------|------|
| `UserCardByIdsEnrichment` | Card list | User collection data (by card IDs) |
| `UserCardBySetEnrichment` | Card list | User collection data (by set) |
| `UserCardByArtistEnrichment` | Card list | User collection data (by artist) |
| `UserCardByNameEnrichment` | Card list | User collection data (by name) |
| `UserWishlistCardByIdsEnrichment` | Card list | User wishlist data |
| `UserSetEnrichment` | Set list | User set tracking data |
| `UserSealedProductEnrichment` | Product list | User sealed product data |

## Related Patterns

- **Integrator** (Entry): Merges enrichment data into target — this file
- **Integrator** (Adapter): Merges delta into state — see `actions/integrators.md`
- **Mapper**: Transforms entities at boundaries — see `actions/mappers.md`
