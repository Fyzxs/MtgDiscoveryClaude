# Implementation Plan: SQLite Migration & Scryfall-Level Search

**Branch**: `010-sqlite-migration` | **Date**: 2026-01-27 | **Spec**: [spec.md](spec.md)
**Input**: Feature specification from `specs/010-sqlite-migration/spec.md`

## Summary

Migrate all production reads for static (non-user) data from Azure Cosmos DB to a single SQLite database file. The 14 static Cosmos containers are retained for ingestion writes and operator investigation but no longer serve production read traffic. This simultaneously achieves cost reduction (minimal RU/s on static containers) and enables Scryfall-level full-text search via FTS5 at zero additional cost. The adapter layer is the integration seam: layers above (Entry, Domain, Aggregator) remain unchanged.

## Technical Context

**Language/Version**: C# .NET 9.0 (backend), TypeScript/React 19 (frontend - Phase 6 only)
**Primary Dependencies**: Microsoft.Data.Sqlite 9.0.x (new), HotChocolate (existing GraphQL), Newtonsoft.Json (existing), Azure Cosmos DB SDK (existing), Azure.Storage.Blobs 12.x (new), Azure.Identity 1.x (new)
**Storage**: SQLite (new, static read-only data ~250-400MB) + Azure Cosmos DB (existing, user data + ingestion writes) + Azure Blob Storage (new, SQLite file hosting)
**Testing**: MSTest with AwesomeAssertions (existing pattern)
**Target Platform**: Azure Container Apps (Linux containers)
**Project Type**: Web application (existing layered .NET backend + React frontend)
**Performance Goals**: SQLite read latency <= Cosmos read latency for static data; concurrent reader support across all GraphQL query threads
**Constraints**: SQLite file ~200-400MB; container memory budget ~1GB; read-only at runtime; rebuilt during ingestion
**Scale/Scope**: ~300K card rows + junction/type-keyed tables; 14 static containers migrated; 5 user containers unchanged; 6+ new .NET projects + adapter renames; 61 projects currently in solution

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

### I. MicroObjects Architecture: PASS

- New SQLite adapter implementations will follow the existing interface-per-class pattern
- Config classes for `StaticDataSource` will be proper MicroObjects (no enums, typed behavior methods)
- Null Object pattern used for no-op adapter responses when data source is disabled
- `Lib.Search.QueryParser` is pure logic with no infrastructure dependencies (composition over inheritance)
- AST node types as sealed class hierarchy (not enums, per MicroObjects)

### II. Layered Architecture Flow: PASS

- Only the Adapter layer changes (new `.Sqlite` implementations alongside renamed `.Cosmos` ones)
- App, Entry, Domain, Aggregator layers are completely unchanged
- Entity type conventions preserved: XfrEntity at adapter boundaries, ExtEntity for external data
- Service dependencies continue to flow downward only
- Both adapters are always called by the aggregator; each self-governs via `IConfigStaticDataSource` (executes or returns null-object)
- In "both" mode, SQLite results take precedence; Cosmos results used only if SQLite returns empty
- No layer above the adapter is aware of data source configuration

### III. Test-First Development: PASS

- All new adapter implementations will have corresponding test projects
- `Lib.Search.QueryParser` is highly testable (pure functions: string in, SQL out)
- Existing test patterns (Fakes, TypeWrapper, self-contained tests) will be followed
- Lexer, Parser, SQL Translator each independently unit-testable
- Three-level isolated testing strategy: lexer tests, parser tests, translator tests

### IV. Null Boundary Guards: PASS

- SQLite adapters will validate input at boundaries (XfrEntity null checks)
- Query parser validates search input at entry point
- Interior code assumes non-null after boundary validation

### V. Scope and Access Control: PASS

- Public scope only in `Apis` folders of new projects
- Internal scope for all implementation classes
- Test projects get `InternalsVisibleTo` access

### VI. Code Style Consistency: PASS

- File-scoped namespaces, sealed/abstract classes, explicit types
- `ConfigureAwait(false)` on all async calls
- No comments, no enums, no boolean negation
- Frontend changes (Phase 6 only) will use MUI sx props

### VII. NoArgsEntity Pattern: PASS

- Operations like `AllSets` will continue to use `NoArgsEntity`/`IAllSetsXfrEntity` as input types
- No changes to existing parameter-less operation patterns

### Constitution Gate Result: ALL GATES PASS

**Complexity concerns noted (not violations):**
- 6+ new .NET projects is significant but maps 1:1 to the existing adapter pattern per domain
- `Lib.Search.QueryParser` is a new standalone library (no infrastructure dependencies) justified by Scryfall search syntax complexity
- Adapter renames are mechanical (folder + .csproj name changes) with no logic changes

## Project Structure

### Documentation (this feature)

```text
specs/010-sqlite-migration/
├── plan.md              # This file
├── spec.md              # Feature specification
├── research.md          # Phase 0: technology decisions and research
├── data-model.md        # Phase 1: SQLite schema entity mapping
├── quickstart.md        # Phase 1: developer setup guide
├── contracts/           # Phase 1: GraphQL schema additions
│   └── advanced-search.graphql
├── checklists/          # Quality checklists
│   └── requirements.md
└── tasks.md             # Phase 2: task breakdown (generated by /speckit.tasks)
```

### Source Code (repository root)

```text
src/
├── Lib.Sqlite/                          # NEW: SQLite infrastructure (connection, query, REGEXP)
│   ├── Apis/
│   │   ├── ISqliteConnection.cs
│   │   └── ISqliteQueryExecutor.cs
│   ├── Configurations/
│   │   └── ConfigStaticDataSource.cs     # StaticDataSource config (IsSourceSqlite/IsSourceCosmos)
│   └── Lib.Sqlite.csproj
│
├── Lib.Search.QueryParser/              # NEW: Scryfall syntax parser (pure logic)
│   ├── Apis/
│   │   └── IQueryParser.cs
│   ├── Lexer/
│   ├── Parser/
│   ├── Translator/
│   └── Lib.Search.QueryParser.csproj
│
├── Lib.Adapter.StaticSource/              # NEW: Shared adapter interfaces for static data domains
│   ├── Apis/                              # ICardQueryAdapter, ISetQueryAdapter, IArtistQueryAdapter, ISealedProductQueryAdapter
│   └── Lib.Adapter.StaticSource.csproj
│
├── Lib.Adapter.Cards.Cosmos/            # RENAMED from Lib.Adapter.Cards (implementation only; interfaces extracted to StaticSource)
│   └── (existing implementation files, unchanged logic)
├── Lib.Adapter.Cards.Sqlite/            # NEW: SQLite card query adapter (references StaticSource, not .Cosmos)
│   ├── Apis/
│   └── Lib.Adapter.Cards.Sqlite.csproj
│
├── Lib.Adapter.Sets.Cosmos/             # RENAMED from Lib.Adapter.Sets
├── Lib.Adapter.Sets.Sqlite/             # NEW: SQLite set query adapter
│
├── Lib.Adapter.Artists.Cosmos/          # RENAMED from Lib.Adapter.Artists
├── Lib.Adapter.Artists.Sqlite/          # NEW: SQLite artist query adapter
│
├── Lib.Adapter.SealedProducts.Cosmos/   # RENAMED from Lib.Adapter.SealedProducts
├── Lib.Adapter.SealedProducts.Sqlite/   # NEW: SQLite sealed product query adapter
│
├── Lib.Adapter.User.Cosmos/             # RENAMED from Lib.Adapter.User (stays Cosmos-only)
├── Lib.Adapter.UserCards.Cosmos/        # RENAMED from Lib.Adapter.UserCards (stays Cosmos-only)
├── Lib.Adapter.UserSetCards.Cosmos/     # RENAMED from Lib.Adapter.UserSetCards (stays Cosmos-only)
├── Lib.Adapter.UserSealedProducts.Cosmos/ # RENAMED from Lib.Adapter.UserSealedProducts (stays Cosmos-only)
├── Lib.Adapter.UserWishlistCards.Cosmos/ # RENAMED from Lib.Adapter.UserWishlistCards (stays Cosmos-only)
│
├── Lib.Aggregator.Cards/               # UPDATED: references both .Cosmos and .Sqlite adapters
├── Lib.Aggregator.Sets/                # UPDATED: references both .Cosmos and .Sqlite adapters
├── Lib.Aggregator.Artists/             # UPDATED: references both .Cosmos and .Sqlite adapters
├── Lib.Aggregator.SealedProducts/      # UPDATED: references both .Cosmos and .Sqlite adapters
│
├── Lib.Scryfall.Ingestion/             # UPDATED: adds SQLite generation step
│   └── BulkIngestion/
│       └── BulkIngestionOrchestrator.cs  # Modified to generate SQLite DB during ingestion
│
└── App.MtgDiscovery.GraphQL/           # UPDATED: new advancedCardSearch query + StaticDataSource config
    ├── Queries/
    │   └── AdvancedSearchQueryMethods.cs  # NEW
    └── appsettings.json                   # Add StaticDataSource + SqliteConfig keys
```

**Structure Decision**: Extends the existing layered .NET solution structure. New projects follow the established naming convention (`Lib.Adapter.{Domain}.{Provider}`). Shared static adapter interfaces are extracted into `Lib.Adapter.StaticSource` so that neither `.Cosmos` nor `.Sqlite` depends on the other — both reference `StaticSource` for interface contracts. The `Lib.Sqlite` infrastructure project mirrors `Lib.Cosmos` for SQLite. `Lib.Search.QueryParser` is a standalone library with no infrastructure dependencies. All 9 existing adapter projects (4 static + 5 user) are renamed with `.Cosmos` suffix for consistency.

## Implementation Phases

### Phase 0: Adapter Rename, Interface Extraction & Configuration
- Rename all 9 existing adapter projects: `Lib.Adapter.Cards` -> `Lib.Adapter.Cards.Cosmos`, etc.
- Create `Lib.Adapter.StaticSource` project and extract shared static adapter interfaces from the 4 static `.Cosmos` projects into it
- Update `.Cosmos` and `.Sqlite` adapter projects to reference `Lib.Adapter.StaticSource` (not each other)
- Update all project references in aggregators, tests, GraphQL app, and .sln
- Update all namespace declarations in .cs files
- Add `StaticDataSource` configuration to appsettings.json (initial value: `source_cosmos`)
- Create `ConfigStaticDataSource` with `IsSourceSqlite()` / `IsSourceCosmos()` methods
- Each adapter self-governs: Cosmos adapters check `IsSourceCosmos()`, SQLite adapters check `IsSourceSqlite()` — no-op (null-object response) when false
- Verify everything works unchanged with `source_cosmos` config
- **No behavior change** — this phase only renames, extracts interfaces, and adds config plumbing

### Phase 1: SQLite Infrastructure & Card Data Migration
- Create `Lib.Sqlite` project with connection management, query execution, REGEXP function registration
- Create `Lib.Adapter.Cards.Sqlite` project with config check (`IsSourceSqlite()`)
- Add SQLite generation step to ingestion pipeline (cards table + all junction/type-keyed tables + indexes)
- Implement SQLite card query adapters: `CardsByIds`, `CardsBySetCode`, `CardsByName`, `CardNameSearch`
- Add aggregator references to `Lib.Adapter.Cards.Sqlite`
- Verify existing GraphQL card queries return identical results

### Phase 2: Sets, Artists, Sealed Products, Remaining Static Data
- Add sets, artists, rulings, sealed_products tables to SQLite generation
- Create `Lib.Adapter.Sets.Sqlite`, `Lib.Adapter.Artists.Sqlite`, `Lib.Adapter.SealedProducts.Sqlite`
- Implement SQLite adapters for all remaining static data queries
- Add aggregator references to new `.Sqlite` projects
- **All static data reads now served by SQLite**

### Phase 3: FTS5 Search (replaces trigrams)
- Add FTS5 virtual tables to SQLite generation (card_names_fts trigram, artist_names_fts trigram, cards_fts porter)
- Implement FTS5-backed card name search and artist name search adapters
- Verify substring search behavior matches current trigram containers

### Phase 4: Query Parser - Core
- Create `Lib.Search.QueryParser` project
- Build lexer (tokenizer) for field prefixes, operators, quoted strings, negation
- Build parser (recursive descent) for AST construction
- Build SQL translator (Visitor pattern) for WHERE clause generation
- Support core fields: `name:`, `o:`, `t:`, `c:`, `id:`, `r:`, `s:`, `f:`, `pow:`, `tou:`, `cmc:`, `a:`, `kw:`
- Add new GraphQL query: `advancedCardSearch(query: String!)`
- Wire through Entry -> Domain -> Aggregator -> Adapter layers

### Phase 5: Query Parser - Advanced
- Add OR, parentheses, `is:`/`not:` boolean flags
- Add color expansion (guild names, shard names)
- Add sorting (`order:`, `direction:`)
- Add regex support (register REGEXP function)
- Add exact name match (`!`)
- Add date/year, price filters
- Reject unsupported fields with structured errors (FR-029)

### Phase 6: Frontend Search UI
- Advanced search bar with Scryfall-style syntax
- Syntax help info box listing supported/unsupported fields (FR-031)
- Client-side pre-validation for unsupported fields (FR-030)
- Faceted filter sidebar (color, rarity, set, format)
- Result pagination
- Visual query builder alternative
- Responsive mobile layout

## Complexity Tracking

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|--------------------------------------|
| 7+ new projects | Each domain needs a dedicated SQLite adapter + infrastructure + parser library + `Lib.Adapter.StaticSource` for shared interfaces | Combining adapters into one project would violate the 1:1 domain-adapter pattern; without StaticSource, `.Sqlite` depends on `.Cosmos` for interfaces |
| Dual adapter pattern (Cosmos + SQLite) | Migration requires running both data sources; "both" mode keeps Cosmos populated for operator investigation | Single-source cutover is too risky; self-governing config avoids composite adapter complexity |
| Lib.Search.QueryParser as standalone library | Scryfall search syntax is complex (lexer + parser + translator); pure logic benefits from isolation | Embedding parser in adapter project would couple search logic to infrastructure |
| 9 project renames | Consistency requires all adapter projects (including user data) to have `.Cosmos` suffix when static adapters get `.Cosmos`/`.Sqlite` variants | Renaming only static adapters would create inconsistent naming across the adapter layer |
