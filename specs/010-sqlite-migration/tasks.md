# Tasks: SQLite Migration & Scryfall-Level Search

**Input**: Design documents from `specs/010-sqlite-migration/`
**Prerequisites**: plan.md (required), spec.md (required), research.md, data-model.md, contracts/

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story. Test tasks are inline per phase (constitution Principle III: Test-First Development).

**Phase Numbering**: Phases 0-8 align with plan.md phases (0-based).

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2)
- Include exact file paths in descriptions

---

## Phase 0: Setup (Project Renames, Interface Extraction & Scaffolding)

**Purpose**: Rename existing adapter projects to `.Cosmos` suffix, extract shared static adapter interfaces into `Lib.Adapter.StaticSource`, and create empty scaffolding for all new projects.

### Adapter Renames

- [ ] T001 Rename src/Lib.Adapter.Cards/ folder to src/Lib.Adapter.Cards.Cosmos/, rename .csproj file, update all namespace declarations in .cs files to Lib.Adapter.Cards.Cosmos
- [ ] T002 [P] Rename src/Lib.Adapter.Sets/ to src/Lib.Adapter.Sets.Cosmos/ with .csproj and namespace updates
- [ ] T003 [P] Rename src/Lib.Adapter.Artists/ to src/Lib.Adapter.Artists.Cosmos/ with .csproj and namespace updates
- [ ] T004 [P] Rename src/Lib.Adapter.SealedProducts/ to src/Lib.Adapter.SealedProducts.Cosmos/ with .csproj and namespace updates
- [ ] T005 [P] Rename src/Lib.Adapter.User/ to src/Lib.Adapter.User.Cosmos/ with .csproj and namespace updates
- [ ] T006 [P] Rename src/Lib.Adapter.UserCards/ to src/Lib.Adapter.UserCards.Cosmos/ with .csproj and namespace updates
- [ ] T007 [P] Rename src/Lib.Adapter.UserSetCards/ to src/Lib.Adapter.UserSetCards.Cosmos/ with .csproj and namespace updates
- [ ] T008 [P] Rename src/Lib.Adapter.UserSealedProducts/ to src/Lib.Adapter.UserSealedProducts.Cosmos/ with .csproj and namespace updates
- [ ] T009 [P] Rename src/Lib.Adapter.UserWishlistCards/ to src/Lib.Adapter.UserWishlistCards.Cosmos/ with .csproj and namespace updates

> **Note**: Lib.Adapter.Scryfall.Cosmos retains its current name (already has `.Cosmos` suffix). 9 remaining adapter projects are renamed.

- [ ] T010 Update all ProjectReference paths in aggregator .csproj files, test .csproj files, src/App.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL.csproj, and src/MtgDiscoveryVibe.sln to reflect renamed .Cosmos project paths
- [ ] T011 Verify dotnet build src/MtgDiscoveryVibe.sln succeeds and dotnet test src/MtgDiscoveryVibe.sln passes with all renames

### Interface Extraction

- [ ] T012 Create src/Lib.Adapter.StaticSource/ classlib project, add to src/MtgDiscoveryVibe.sln, add project references to Lib.Shared.DataModels and Lib.Shared.Invocation
- [ ] T013 Extract shared static adapter interfaces from src/Lib.Adapter.Cards.Cosmos/Apis/, src/Lib.Adapter.Sets.Cosmos/Apis/, src/Lib.Adapter.Artists.Cosmos/Apis/, src/Lib.Adapter.SealedProducts.Cosmos/Apis/ into src/Lib.Adapter.StaticSource/Apis/ — move query adapter service interfaces only (not implementations)
- [ ] T014 Update src/Lib.Adapter.Cards.Cosmos/, Sets.Cosmos/, Artists.Cosmos/, SealedProducts.Cosmos/ to reference Lib.Adapter.StaticSource for shared interfaces
- [ ] T015 Update aggregator projects (Lib.Aggregator.Cards, Sets, Artists, SealedProducts) to reference Lib.Adapter.StaticSource instead of the old adapter interface locations
- [ ] T016 Verify dotnet build src/MtgDiscoveryVibe.sln succeeds and dotnet test src/MtgDiscoveryVibe.sln passes after interface extraction

### New Project Scaffolding

- [ ] T017 [P] Create src/Lib.Sqlite/ classlib project (dotnet new classlib -n Lib.Sqlite --framework net9.0), add Microsoft.Data.Sqlite 9.0.x NuGet, add to src/MtgDiscoveryVibe.sln, add project reference to src/Lib.Universal/Lib.Universal.csproj
- [ ] T018 [P] Create src/Lib.Adapter.Cards.Sqlite/ classlib project, add to .sln, add project references to Lib.Adapter.StaticSource, Lib.Sqlite, and Lib.Shared.Invocation
- [ ] T019 [P] Create src/Lib.Adapter.Sets.Sqlite/ classlib project, add to .sln, add project references to Lib.Adapter.StaticSource, Lib.Sqlite, and Lib.Shared.Invocation
- [ ] T020 [P] Create src/Lib.Adapter.Artists.Sqlite/ classlib project, add to .sln, add project references to Lib.Adapter.StaticSource, Lib.Sqlite, and Lib.Shared.Invocation
- [ ] T021 [P] Create src/Lib.Adapter.SealedProducts.Sqlite/ classlib project, add to .sln, add project references to Lib.Adapter.StaticSource, Lib.Sqlite, and Lib.Shared.Invocation
- [ ] T022 [P] Create src/Lib.Search.QueryParser/ classlib project, add to .sln (no infrastructure dependencies — pure logic library)
- [ ] T023 Verify dotnet build src/MtgDiscoveryVibe.sln succeeds after all scaffolding

---

## Phase 1: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that MUST be complete before ANY user story can be implemented. Creates the SQLite connection layer, data source configuration, and Cosmos adapter guards.

**CRITICAL**: No user story work can begin until this phase is complete.

### Configuration

- [ ] T024 Create IConfigStaticDataSource interface with IsSourceSqlite() and IsSourceCosmos() methods in src/Lib.Sqlite/Apis/IConfigStaticDataSource.cs
- [ ] T025 Create ConfigStaticDataSource sealed class implementing IConfigStaticDataSource in src/Lib.Sqlite/Configurations/ConfigStaticDataSource.cs — reads "StaticDataSource" from appsettings.json, returns true/false per data-model.md mapping (source_sqlite, source_cosmos, source_both)
- [ ] T026 Add "StaticDataSource": "source_cosmos" and "SqliteConfig": { "DataFilePath": "./data/mtg-data.db" } to src/App.MtgDiscovery.GraphQL/appsettings.json
- [ ] T027 Register ConfigStaticDataSource in DI container in src/App.MtgDiscovery.GraphQL/Startup.cs

### SQLite Infrastructure

- [ ] T028 Create ISqliteConnectionFactory interface in src/Lib.Sqlite/Apis/ISqliteConnectionFactory.cs with CreateReadOnlyConnection() method
- [ ] T029 Create SqliteConnectionFactory sealed class in src/Lib.Sqlite/ implementing ISqliteConnectionFactory — opens read-only connection with immutable=1 URI, sets PRAGMAs per research.md R1, registers REGEXP function per research.md R3
- [ ] T030 Create ISqliteQueryExecutor interface in src/Lib.Sqlite/Apis/ISqliteQueryExecutor.cs with QueryAsync<T> and ExecuteScalarAsync methods
- [ ] T031 Create SqliteQueryExecutor sealed class in src/Lib.Sqlite/ implementing ISqliteQueryExecutor — parameterized query execution with SqliteParameter, ConfigureAwait(false)

### Adapter Guards

- [ ] T032 Inject IConfigStaticDataSource into each existing Cosmos adapter in src/Lib.Adapter.Cards.Cosmos/, src/Lib.Adapter.Sets.Cosmos/, src/Lib.Adapter.Artists.Cosmos/, src/Lib.Adapter.SealedProducts.Cosmos/ — each adapter self-governs via IsSourceCosmos(), returning null-object response when false

### Registration & Verification

- [ ] T033 Register Lib.Sqlite services (ISqliteConnectionFactory, ISqliteQueryExecutor, IConfigStaticDataSource) in src/App.MtgDiscovery.GraphQL/Startup.cs DI container
- [ ] T034 Verify dotnet build and dotnet test pass — existing behavior unchanged with source_cosmos config

### Tests

- [ ] T035 Create src/Lib.Sqlite.Tests/ mstest project, add to .sln, add project reference to Lib.Sqlite with InternalsVisibleTo
- [ ] T036 Write unit tests for ConfigStaticDataSource — verify IsSourceSqlite() and IsSourceCosmos() return correct values for each of the 3 config strings (source_sqlite, source_cosmos, source_both)
- [ ] T037 Write unit tests for SqliteConnectionFactory — verify connection opens with expected PRAGMAs, REGEXP function registered
- [ ] T038 Write unit tests for SqliteQueryExecutor — verify parameterized query execution, ConfigureAwait(false) on async calls

**Checkpoint**: Foundation ready — user story implementation can now begin

---

## Phase 2: User Story 1 + User Story 2 — Card/Set/Artist/SealedProduct Data Migration & Ingestion (Priority: P1) MVP

**Goal**: All existing card, set, artist, and sealed product queries return identical data from the new SQLite data source. The ingestion pipeline generates the SQLite database file containing all static data.

**Self-Governing Adapter Pattern**: Both Cosmos and SQLite adapters are always called by the aggregator. Each adapter self-governs via `IConfigStaticDataSource` — executes its operation or returns a null-object response. In `source_both` mode, SQLite results take precedence; Cosmos results are used only if SQLite returns empty.

**Independent Test**: Navigate every existing page and GraphQL query, verify all results match current production behavior exactly. Run ingestion pipeline and verify data file is generated and served after restart.

### SQLite Entity Models

- [ ] T039 [P] [US1] Create CardSqlEntity sealed class in src/Lib.Adapter.Cards.Sqlite/ with all properties from data-model.md CardSqlEntity table (180+ columns mapping to cards table)
- [ ] T040 [P] [US1] Create SetSqlEntity sealed class in src/Lib.Adapter.Sets.Sqlite/ with all properties from data-model.md SetSqlEntity table
- [ ] T041 [P] [US1] Create ArtistSqlEntity sealed class in src/Lib.Adapter.Artists.Sqlite/ with all properties from data-model.md ArtistSqlEntity table
- [ ] T042 [P] [US1] Create SealedProductSqlEntity sealed class in src/Lib.Adapter.SealedProducts.Sqlite/ with all properties from data-model.md SealedProductSqlEntity table
- [ ] T043 [P] [US1] Create RulingSqlEntity sealed class in src/Lib.Sqlite/ with all properties from data-model.md RulingSqlEntity table

### SQLite Database Generation (Ingestion Pipeline)

- [ ] T044 [US2] Create ISqliteDbGenerator interface in src/Lib.Sqlite/Apis/ISqliteDbGenerator.cs with GenerateAsync() method
- [ ] T045 [US2] Create ISqliteDbWriter interface in src/Lib.Sqlite/Apis/ISqliteDbWriter.cs with methods for creating tables, inserting data, creating indexes, and creating FTS5 virtual tables
- [ ] T046 [US2] Implement SqliteDbWriter sealed class in src/Lib.Sqlite/ — creates all tables from design doc schema (cards, sets, artists, rulings, sealed_products, all junction/type-keyed tables), sets write-time PRAGMAs per research.md R5
- [ ] T047 [US2] Implement cards table data insertion in SqliteDbWriter — bulk insert all card rows and all junction table rows (card_colors, card_keywords, card_artists, card_images, card_legalities, card_prices, card_uris, card_faces, card_parts, etc.)
- [ ] T048 [P] [US2] Implement sets, artists, rulings, sealed_products table data insertion in SqliteDbWriter
- [ ] T049 [US2] Implement index creation in SqliteDbWriter — all indexes from design doc (idx_cards_oracle_id, idx_cards_set_code, idx_card_artists_artist, etc.)
- [ ] T050 [US2] Implement FTS5 virtual table creation in SqliteDbWriter — card_names_fts (trigram tokenizer), artist_names_fts (trigram tokenizer), cards_fts (porter unicode61 tokenizer) as external content tables
- [ ] T051 [US2] Implement post-generation steps in SqliteDbWriter — FTS5 optimize, ANALYZE, VACUUM per research.md R5
- [ ] T052 [US2] Implement SqliteDbGenerator sealed class in src/Lib.Sqlite/ — orchestrates write to temp file (.db.tmp), calls SqliteDbWriter, renames temp to final path on success per research.md R5 (atomic rename ensures FR-009 failure safety)
- [ ] T053 [US2] Integrate SqliteDbGenerator into src/Lib.Scryfall.Ingestion/BulkIngestionOrchestrator.cs — always invoke the generator; the generator self-governs via IsSourceSqlite() (executes for source_sqlite and source_both, no-ops for source_cosmos)

### SQLite Adapter Implementations (Cards)

- [ ] T054 [US1] Create SqliteCardQueryAdapter sealed class in src/Lib.Adapter.Cards.Sqlite/Apis/ implementing the card query adapter interface from Lib.Adapter.StaticSource — inject ISqliteQueryExecutor and IConfigStaticDataSource, self-governs via IsSourceSqlite()
- [ ] T055 [US1] Implement GetCardsByIdsAsync in SqliteCardQueryAdapter — SELECT * FROM cards WHERE id IN (@p0, @p1, ...) → map CardSqlEntity to OufEntity
- [ ] T056 [US1] Implement GetCardsBySetCodeAsync in SqliteCardQueryAdapter — SELECT * FROM cards WHERE set_code = @p0 → map CardSqlEntity to OufEntity
- [ ] T057 [US1] Implement GetCardsByNameAsync in SqliteCardQueryAdapter — SELECT * FROM cards WHERE name_lower = @p0 → map CardSqlEntity to OufEntity
- [ ] T058 [US1] Implement SearchCardNamesAsync in SqliteCardQueryAdapter — SELECT * FROM card_names_fts WHERE card_names_fts MATCH @p0 (trigram substring) → map to name results

### SQLite Adapter Implementations (Sets, Artists, SealedProducts)

- [ ] T059 [P] [US1] Create SqliteSetQueryAdapter sealed class in src/Lib.Adapter.Sets.Sqlite/Apis/ — implement SetsByIdsAsync (WHERE id IN), SetsByCodesAsync (WHERE code IN), AllSetsAsync (SELECT *)
- [ ] T060 [P] [US1] Create SqliteArtistQueryAdapter sealed class in src/Lib.Adapter.Artists.Sqlite/Apis/ — implement SearchArtistsAsync (artist_names_fts MATCH), CardsByArtistIdAsync (JOIN card_artists), CardsByArtistNameAsync (FTS5 + JOIN)
- [ ] T061 [P] [US1] Create SqliteSealedProductQueryAdapter sealed class in src/Lib.Adapter.SealedProducts.Sqlite/Apis/ — implement SealedProductsBySetCodeAsync (WHERE set_code = @p0)

### SqlEntity-to-OufEntity Mappers

- [ ] T062 [P] [US1] Create CardSqlEntityMapper sealed class in src/Lib.Adapter.Cards.Sqlite/ — maps CardSqlEntity to existing card OufEntity interface (JSON array string → list deserialization for colors, keywords, etc.)
- [ ] T063 [P] [US1] Create SetSqlEntityMapper sealed class in src/Lib.Adapter.Sets.Sqlite/
- [ ] T064 [P] [US1] Create ArtistSqlEntityMapper sealed class in src/Lib.Adapter.Artists.Sqlite/
- [ ] T065 [P] [US1] Create SealedProductSqlEntityMapper sealed class in src/Lib.Adapter.SealedProducts.Sqlite/

### Aggregator Wiring

- [ ] T066 [US1] Add Lib.Adapter.Cards.Sqlite project reference to src/Lib.Aggregator.Cards/Lib.Aggregator.Cards.csproj, register SqliteCardQueryAdapter in DI alongside Cosmos adapter
- [ ] T067 [P] [US1] Add Lib.Adapter.Sets.Sqlite project reference to src/Lib.Aggregator.Sets/Lib.Aggregator.Sets.csproj, register SqliteSetQueryAdapter
- [ ] T068 [P] [US1] Add Lib.Adapter.Artists.Sqlite project reference to src/Lib.Aggregator.Artists/Lib.Aggregator.Artists.csproj, register SqliteArtistQueryAdapter
- [ ] T069 [P] [US1] Add Lib.Adapter.SealedProducts.Sqlite project reference to src/Lib.Aggregator.SealedProducts/Lib.Aggregator.SealedProducts.csproj, register SqliteSealedProductQueryAdapter

### Verification

- [ ] T070 [US1] Verify all existing GraphQL queries (cardsById, cardsBySetCode, cardsByName, cardNameSearch, setsById, setsByCode, allSets, artistSearch, cardsByArtist, sealedProductsBySetCode) return identical results with source_sqlite config
- [ ] T071 [US2] Verify ingestion pipeline generates SQLite file, restart application, confirm new data is served

### Tests

- [ ] T072 [P] Create src/Lib.Adapter.Cards.Sqlite.Tests/ mstest project, add to .sln with InternalsVisibleTo
- [ ] T073 [P] Create src/Lib.Adapter.Sets.Sqlite.Tests/, src/Lib.Adapter.Artists.Sqlite.Tests/, src/Lib.Adapter.SealedProducts.Sqlite.Tests/ mstest projects, add to .sln with InternalsVisibleTo
- [ ] T074 Write unit tests for SqlEntity-to-OufEntity mappers (CardSqlEntityMapper, SetSqlEntityMapper, ArtistSqlEntityMapper, SealedProductSqlEntityMapper) — verify correct JSON deserialization, null handling, property mapping
- [ ] T075 Write unit tests for SQLite adapter query methods — use in-memory SQLite database seeded with test data, verify correct SQL generation and result mapping
- [ ] T076 Write unit tests for SqliteDbWriter — verify table creation, index creation, FTS5 virtual table creation, data insertion using in-memory SQLite
- [ ] T077 Write unit tests for SqliteDbGenerator — verify temp file workflow, atomic rename on success

### Safety & Concurrency Verification

- [ ] T078 [US2] Verify ingestion failure leaves existing .db file intact — interrupt or fail generation mid-process, confirm .db.tmp is not promoted and existing .db continues serving (FR-009)
- [ ] T079 [US1] Verify concurrent SQLite read access — run multiple simultaneous GraphQL queries against SQLite, confirm no errors or blocking (FR-024)

**Checkpoint**: User Stories 1 & 2 fully functional — all static data served from SQLite, ingestion pipeline generates new data files

---

## Phase 3: User Story 3 — Infrastructure Cost Reduction (Priority: P2)

**Goal**: All production read traffic uses SQLite; static Cosmos containers can be reduced to minimum throughput.

**Independent Test**: Confirm zero read requests to static Cosmos containers via monitoring. Reduce throughput and verify application operates normally.

- [ ] T080 [US3] Verify with source_sqlite config that zero read requests are made to static Cosmos containers (CardItems, SetItems, ArtistItems, etc.) — check via monitoring or Cosmos metrics
- [ ] T081 [US3] Document operational steps for reducing Cosmos container throughput to minimum (400 RU/s or serverless) in specs/010-sqlite-migration/quickstart.md

**Checkpoint**: Cost reduction verified — static Cosmos containers serve investigation only

---

## Phase 4: User Story 4 — Configurable Data Source Verification (Priority: P2)

**Goal**: All three data source modes (original, new, both) function correctly. Operator can roll back to Cosmos at any time.

**Independent Test**: Switch between source_cosmos, source_sqlite, and source_both configurations, verify each mode behaves correctly.

- [ ] T082 [US4] Verify source_cosmos mode: all reads from Cosmos, SQLite adapters return null-object responses
- [ ] T083 [US4] Verify source_sqlite mode: all reads from SQLite, Cosmos adapters return null-object responses
- [ ] T084 [US4] Verify source_both mode: ingestion writes to both Cosmos and SQLite, both adapters fire, SQLite results take precedence per merge semantics
- [ ] T085 [US4] Verify rollback: switch source_sqlite → source_cosmos, restart, confirm Cosmos serves all reads

**Checkpoint**: All data source modes verified — migration safety confirmed

---

## Phase 5: User Story 5 — Core Advanced Card Search (Priority: P3)

**Goal**: Users can search for cards using Scryfall-style query syntax with core field prefixes, proper color search semantics, and the advancedCardSearch GraphQL endpoint returns paginated results.

**Independent Test**: Submit Scryfall-style queries (e.g., `name:bolt`, `t:creature c:red cmc>=3`, `o:"draw a card"`) via advancedCardSearch and verify correct, paginated results.

### Query Parser — Lexer

- [ ] T086 [P] [US5] Create ITokenKind sealed class hierarchy (FieldPrefix, ComparisonOperator, QuotedString, BareWord, BooleanOr, BooleanAnd, BooleanNot, NegationPrefix, OpenParen, CloseParen, ExactMatch) in src/Lib.Search.QueryParser/Lexer/
- [ ] T087 [P] [US5] Create IToken interface with Kind, Value, Position properties in src/Lib.Search.QueryParser/Lexer/IToken.cs
- [ ] T088 [US5] Create ILexer interface and Lexer sealed class in src/Lib.Search.QueryParser/Lexer/ — tokenize raw query string into IReadOnlyList<IToken> with position tracking

### Query Parser — AST & Parser

- [ ] T089 [P] [US5] Create ISearchField sealed class hierarchy (NameField, OracleTextField, TypeLineField, FlavorTextField, ArtistField, KeywordField, ColorField, ColorIdentityField, ManaCostField, PowerField, ToughnessField, LoyaltyField, CmcField, SetCodeField, SetTypeField, RarityField, FormatField, PriceField) in src/Lib.Search.QueryParser/Parser/
- [ ] T090 [P] [US5] Create IComparisonOperator sealed class hierarchy (Equals, GreaterThan, LessThan, GreaterOrEqual, LessOrEqual) in src/Lib.Search.QueryParser/Parser/
- [ ] T091 [P] [US5] Create ISearchNode sealed class hierarchy (IAndNode, IOrNode, INotNode, IFieldComparisonNode, ITextSearchNode, IFieldTextNode) with ISearchNodeVisitor<T> in src/Lib.Search.QueryParser/Parser/
- [ ] T092 [US5] Create IParser interface and Parser sealed class in src/Lib.Search.QueryParser/Parser/ — recursive descent parser: token stream → AST, implicit AND between adjacent terms, field expressions, text expressions per research.md R9 grammar

### Query Parser — SQL Translator

- [ ] T093 [US5] Create ISqlTranslationResult interface in src/Lib.Search.QueryParser/Translator/ with WhereClause (string) and Parameters (IReadOnlyList) properties
- [ ] T094 [US5] Create SqlTranslator sealed class implementing ISearchNodeVisitor<string> in src/Lib.Search.QueryParser/Translator/ — translates AST nodes to SQL WHERE clauses with parameterized values (@p0, @p1), FTS5 MATCH for text fields, direct comparison for numeric fields
- [ ] T095 [US5] Implement FTS5 expression sanitization in SqlTranslator — replace " with "" in user text, wrap terms in double quotes to prevent FTS5 boolean injection per research.md R10

### Color Search Semantics

- [ ] T096 [US5] Implement color search semantics in SqlTranslator — c:U (includes), c=UR (exactly), c>=UR (superset), c<=UR (subset), colorless per design doc color fields table. This is core to meaningful search and ships with Phase 5.

### Query Parser — Public API

- [ ] T097 [US5] Create IQueryParser interface in src/Lib.Search.QueryParser/Apis/IQueryParser.cs with Parse(string query) returning IOperationResponse containing SQL WHERE clause and parameters or structured parse errors
- [ ] T098 [US5] Create QueryParser sealed class in src/Lib.Search.QueryParser/ — orchestrates ILexer → IParser → ISqlTranslator pipeline

### Layer Wiring (Entry → Domain → Aggregator → Adapter)

- [ ] T099 [US5] Create IAdvancedCardSearchItrEntity interface in src/Lib.Shared.DataModels/ with Query (string), First (int), After (string) properties
- [ ] T100 [P] [US5] Create IAdvancedCardSearchResultOufEntity interface in src/Lib.Shared.DataModels/ with TotalCount, Cards (IReadOnlyList), HasNextPage, EndCursor properties
- [ ] T101 [US5] Implement advanced card search adapter method in src/Lib.Adapter.Cards.Sqlite/ — accepts parsed SQL WHERE clause + params, executes query with pagination (LIMIT/OFFSET via cursor), returns IAdvancedCardSearchResultOufEntity
- [ ] T102 [US5] Create IAdvancedCardSearchAggregatorService and implementation in src/Lib.Aggregator.Cards/ — calls query parser then adapter
- [ ] T103 [US5] Create IAdvancedCardSearchDomainService and implementation in src/Lib.Domain.Cards/ — delegates to aggregator
- [ ] T104 [US5] Create IAdvancedCardSearchEntryService and implementation in src/Lib.MtgDiscovery.Entry/ — validates input, maps ArgEntity to ItrEntity, calls domain, maps result to OutEntity

### GraphQL Endpoint

- [ ] T105 [US5] Create AdvancedCardSearchArgEntity in src/App.MtgDiscovery.GraphQL/ with Query, First, After properties
- [ ] T106 [US5] Create AdvancedSearchQueryMethods sealed class with [ExtendObjectType(typeof(Query))] in src/App.MtgDiscovery.GraphQL/Queries/AdvancedSearchQueryMethods.cs — public advancedCardSearch endpoint, no [Authorize]
- [ ] T107 [US5] Create GraphQL response types per contracts/advanced-search.graphql: AdvancedCardSearchResultUnionType, AdvancedCardSearchSuccessDataResponseModelType, AdvancedCardSearchErrorType, SearchParseErrorType, PageInfoType in src/App.MtgDiscovery.GraphQL/Entities/Types/
- [ ] T108 [US5] Register all new GraphQL types in schema extensions in src/App.MtgDiscovery.GraphQL/
- [ ] T109 [US5] Verify advancedCardSearch works for core fields: name:bolt, t:creature, c:red, c>=RG, cmc>=3, o:"draw a card", s:mh2, r:mythic, f:modern

### Tests

- [ ] T110 Create src/Lib.Search.QueryParser.Tests/ mstest project, add to .sln with InternalsVisibleTo
- [ ] T111 Write lexer unit tests — token recognition, quoted strings, whitespace handling, negation, position tracking, edge cases per research.md R11
- [ ] T112 Write parser unit tests — field expressions, implicit AND, negation, grouping, precedence, error recovery per research.md R11 (use pre-constructed token lists to isolate from lexer)
- [ ] T113 Write SQL translator unit tests — field comparisons, text search, boolean composition, negation, parameter numbering, FTS5 sanitization, color semantics per research.md R11 (use pre-constructed AST to isolate from parser)

**Checkpoint**: Core advanced search functional — users can search with field prefixes, color semantics, and get paginated results

---

## Phase 6: User Story 6 — Advanced Query Features (Priority: P4)

**Goal**: Full Scryfall-style boolean logic, sorting, regex, date/price filters, and unsupported field rejection.

**Independent Test**: Submit complex queries with OR, parentheses, is:/not: flags, sorting, regex, date/price filters; verify correct results. Submit unsupported fields; verify structured error.

- [ ] T114 [US6] Add OR keyword and parenthesized grouping support to Lexer and Parser in src/Lib.Search.QueryParser/
- [ ] T115 [US6] Add is: and not: boolean flag support to Lexer, Parser, and SqlTranslator — map is:foil → WHERE foil = 1, is:reprint → WHERE reprint = 1, is:transform → WHERE layout = 'transform', etc. per design doc boolean flags table
- [ ] T116 [P] [US6] Add color expansion to Parser — guild names (azorius → WU, dimir → UB, etc.), shard names (bant → GWU, etc.) in src/Lib.Search.QueryParser/Parser/
- [ ] T117 [P] [US6] Add sorting support to Parser and SQL generation — order:name, order:cmc, order:price, order:rarity, order:released, order:edhrec, direction:asc/desc in src/Lib.Search.QueryParser/
- [ ] T118 [US6] Add regex support — ensure REGEXP function registered in SqliteConnectionFactory, add regex syntax (/pattern/) to Lexer/Parser, translate to WHERE column REGEXP @p0 in SqlTranslator
- [ ] T119 [P] [US6] Add exact name match (! prefix) to Lexer/Parser/SqlTranslator — !"Lightning Bolt" → WHERE name_lower = 'lightning bolt'
- [ ] T120 [P] [US6] Add date/year filtering to Parser/SqlTranslator — year:2023 → WHERE released_at >= '2023-01-01' AND released_at < '2024-01-01', year>=2020, date:2023-06-15
- [ ] T121 [P] [US6] Add price filtering to Parser/SqlTranslator — usd>10 → WHERE id IN (SELECT card_id FROM card_prices WHERE currency = 'usd' AND amount > 10)
- [ ] T122 [US6] Add unsupported field rejection with structured errors in Parser — reject art:, cube:, function:, lang: with SearchParseError containing field name, position, and message per FR-029
- [ ] T123 [US6] Verify all advanced query patterns: (t:goblin or t:elf) c:R, -is:digital is:foil, order:cmc, year>=2023, !"Lightning Bolt", o:/\{T\}:.*draw/

### Tests

- [ ] T124 Write unit tests for advanced query features — OR/parentheses, is:/not: flags, color expansion, sorting, regex, exact match, date/year, price, unsupported field rejection

**Checkpoint**: Full Scryfall-style search complete — all syntax patterns supported

---

## Phase 7: User Story 7 — Frontend Search UI (Priority: P5)

**Goal**: Dedicated search page with Scryfall-style search bar, syntax help info box, faceted filters, client-side unsupported field pre-validation, and responsive mobile layout.

**Independent Test**: Navigate to search page, submit queries, use faceted filters, verify results displayed correctly. Verify syntax help visible. Enter unsupported field — verify inline error before submission. Test on mobile viewport.

- [ ] T125 [US7] Create advancedCardSearch.graphql query definition in client/src/ and run npm run codegen to generate TypeScript types and hooks
- [ ] T126 [US7] Create SearchBar atom component in client/src/components/atoms/Search/SearchBar.tsx — text input with submit, MUI sx props
- [ ] T127 [P] [US7] Create SyntaxHelpInfoBox molecule component in client/src/components/molecules/Search/SyntaxHelpInfoBox.tsx — lists supported Scryfall syntax fields and explicitly notes unsupported fields (art:, cube:, function:, lang:) per FR-031
- [ ] T128 [US7] Implement client-side pre-validation in SearchBar — check for unsupported field prefixes before submission, display inline MUI Alert error per FR-030
- [ ] T129 [P] [US7] Create FacetedFilterSidebar organism in client/src/components/organisms/Search/FacetedFilterSidebar.tsx — color, rarity, set, format filter chips/dropdowns with MUI sx props per FR-026
- [ ] T130 [P] [US7] Create SearchResultsGrid organism in client/src/components/organisms/Search/SearchResultsGrid.tsx — display matching cards using existing CardCompact or CardDisplayResponsive components
- [ ] T131 [US7] Create AdvancedSearchPage in client/src/pages/AdvancedSearchPage.tsx — compose SearchBar + SyntaxHelpInfoBox + FacetedFilterSidebar + SearchResultsGrid, wire to generated advancedCardSearch hook
- [ ] T132 [US7] Implement cursor-based pagination in AdvancedSearchPage — "Load More" or infinite scroll using PageInfo.hasNextPage and endCursor from API response
- [ ] T133 [US7] Add route for AdvancedSearchPage in client/src/App.tsx (e.g., /search)
- [ ] T134 [US7] Verify responsive layout on mobile viewports using MUI breakpoints — search bar, filters, and results usable on small screens per FR-028

> **Out of scope**: Autocomplete/type-ahead suggestions — the SyntaxHelpInfoBox satisfies FR-027 ("syntax help or autocomplete"). Visual query builder is deferred to a future feature.

**Checkpoint**: Frontend search UI complete — full user-facing search experience

---

## Phase 8: Polish & Cross-Cutting Concerns

**Purpose**: Final verification and documentation updates

- [ ] T135 Verify all existing tests pass with dotnet test src/MtgDiscoveryVibe.sln
- [ ] T136 [P] Run npm run build in client/ to verify frontend builds without errors
- [ ] T137 Run quickstart.md validation steps end-to-end
- [ ] T138 Update CLAUDE.md with new project references, SQLite patterns, query parser conventions, and Lib.Adapter.StaticSource documentation
- [ ] T139 Investigate orphan src/Lib.Adapter.Scryfall.BlobStorage.Tests/ project (test project exists with no corresponding source project) — determine if it should be removed or if a source project is missing

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 0)**: No dependencies — can start immediately
- **Foundational (Phase 1)**: Depends on Setup (Phase 0) completion — BLOCKS all user stories
- **US1+US2 (Phase 2)**: Depends on Foundational (Phase 1) — core data migration
- **US3 (Phase 3)**: Depends on US1+US2 (Phase 2) — verifies cost reduction after migration
- **US4 (Phase 4)**: Depends on US1+US2 (Phase 2) — verifies all config modes work with SQLite
- **US5 (Phase 5)**: Depends on US1+US2 (Phase 2) — search requires migrated data
- **US6 (Phase 6)**: Depends on US5 (Phase 5) — extends core search
- **US7 (Phase 7)**: Depends on US6 (Phase 6) — frontend for full search
- **Polish (Phase 8)**: Depends on all desired user stories being complete

### User Story Dependencies

- **US1+US2 (P1)**: Start after Foundational — no dependencies on other stories
- **US3 (P2)**: Depends on US1+US2 — can only verify cost reduction after migration
- **US4 (P2)**: Depends on US1+US2 — can only verify all modes after SQLite adapters exist
- **US5 (P3)**: Depends on US1+US2 — search queries execute against SQLite data
- **US6 (P4)**: Depends on US5 — extends the core parser
- **US7 (P5)**: Depends on US6 — frontend needs full search backend

### Within Each User Story

- Entity models before adapter implementations
- Adapter implementations before aggregator wiring
- Aggregator wiring before verification
- Ingestion pipeline (US2) before adapter testing (US1)

### Parallel Opportunities

- All adapter renames T001-T009 marked [P] can run in parallel
- All project scaffolding T017-T022 marked [P] can run in parallel
- All SqlEntity models T039-T043 marked [P] can run in parallel
- All non-Card SQLite adapters T059-T061 marked [P] can run in parallel
- All SqlEntity mappers T062-T065 marked [P] can run in parallel
- All non-Card aggregator wiring T067-T069 marked [P] can run in parallel
- Lexer and AST type hierarchies T086-T091 marked [P] can run in parallel
- US3 and US4 can run in parallel after US1+US2 complete
- Multiple frontend components T127, T129, T130 marked [P] can run in parallel

---

## Parallel Example: Phase 2 (US1+US2)

```bash
# Launch all entity models in parallel:
Task: "Create CardSqlEntity in src/Lib.Adapter.Cards.Sqlite/"
Task: "Create SetSqlEntity in src/Lib.Adapter.Sets.Sqlite/"
Task: "Create ArtistSqlEntity in src/Lib.Adapter.Artists.Sqlite/"
Task: "Create SealedProductSqlEntity in src/Lib.Adapter.SealedProducts.Sqlite/"
Task: "Create RulingSqlEntity in src/Lib.Sqlite/"

# After models complete, launch non-Card adapters in parallel:
Task: "Create SqliteSetQueryAdapter in src/Lib.Adapter.Sets.Sqlite/"
Task: "Create SqliteArtistQueryAdapter in src/Lib.Adapter.Artists.Sqlite/"
Task: "Create SqliteSealedProductQueryAdapter in src/Lib.Adapter.SealedProducts.Sqlite/"
```

## Parallel Example: Phase 5 (US5)

```bash
# Launch type hierarchies in parallel:
Task: "Create ITokenKind hierarchy in src/Lib.Search.QueryParser/Lexer/"
Task: "Create ISearchField hierarchy in src/Lib.Search.QueryParser/Parser/"
Task: "Create IComparisonOperator hierarchy in src/Lib.Search.QueryParser/Parser/"
Task: "Create ISearchNode hierarchy in src/Lib.Search.QueryParser/Parser/"
```

---

## Implementation Strategy

### MVP First (US1 + US2 Only)

1. Complete Phase 0: Setup (project renames, interface extraction, scaffolding)
2. Complete Phase 1: Foundational (config + SQLite infrastructure + tests)
3. Complete Phase 2: US1+US2 (data migration + ingestion + tests)
4. **STOP and VALIDATE**: Verify all existing queries return identical data from SQLite
5. Deploy with source_sqlite config

### Incremental Delivery

1. Setup + Foundational → Infrastructure ready
2. US1+US2 → All static data migrated → **MVP deployed**
3. US3+US4 → Cost reduction verified, config modes validated
4. US5 → Core advanced search with color semantics available
5. US6 → Full Scryfall-style search syntax
6. US7 → Frontend search UI → Full feature complete

### Suggested MVP Scope

**Phase 0 + Phase 1 + Phase 2** (Setup + Foundational + US1/US2) delivers the complete data migration with zero user-facing regression, ingestion pipeline support, and immediate cost savings. This is the recommended stopping point for the first deployment.

---

## Notes

- [P] tasks = different files, no dependencies on incomplete tasks
- [US*] label maps task to specific user story for traceability
- All new .NET classes must be `sealed` with file-scoped namespaces per constitution
- All async methods must use `ConfigureAwait(false)` per constitution
- Public scope only in `Apis` folders; all implementations are `internal` per constitution
- Use `IOperationResponse<T>` from Lib.Shared.Invocation for adapter return types, NOT `OpResponse<T>` from Lib.Cosmos
- Frontend components use MUI sx props, not Tailwind classes per constitution
- No comments in code unless explicitly requested per constitution
- Both adapters are always called by the aggregator; each self-governs via IConfigStaticDataSource
- In source_both mode, SQLite results take precedence over Cosmos results
