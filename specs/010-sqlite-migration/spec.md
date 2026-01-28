# Feature Specification: SQLite Migration & Scryfall-Level Search

**Feature Branch**: `010-sqlite-migration`
**Created**: 2026-01-26
**Status**: Draft
**Input**: User description: "Use .docs/010-sqlite-migration-and-search-design.md as the basis of an architectural update"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Existing Card and Set Browsing Remains Identical (Priority: P1)

As a user browsing the MTG Discovery platform, I can view cards by set code, look up cards by name, view all sets, look up sets by code, browse artists, and view sealed products exactly as I do today. The data source change is invisible to me. All existing pages, queries, and interactions continue to work without any change in behavior or data accuracy.

**Why this priority**: This is the foundational requirement. If migrating the data source breaks any existing user-facing functionality, the entire feature fails. Every other story depends on this one succeeding. Zero regression is non-negotiable.

**Independent Test**: Can be fully tested by navigating every existing page and query in the application and verifying that all results match the current production behavior exactly.

**Acceptance Scenarios**:

1. **Given** the platform is running with the new data source, **When** a user searches for a card by name (e.g., "Lightning Bolt"), **Then** the same card results are returned as with the previous data source, with identical field values.
2. **Given** the platform is running with the new data source, **When** a user browses cards by set code (e.g., "mh2"), **Then** all cards for that set are returned with the same data, ordering, and completeness as before.
3. **Given** the platform is running with the new data source, **When** a user views all sets, **Then** every set is listed with the same information (name, code, card count, release date, icons) as before.
4. **Given** the platform is running with the new data source, **When** a user performs a card name substring search (e.g., typing "bolt"), **Then** results include substring matches (e.g., "Thunderbolt") just as the current trigram search does.
5. **Given** the platform is running with the new data source, **When** a user searches for an artist by name, **Then** the same artist results and associated cards are returned as before.
6. **Given** the platform is running with the new data source, **When** a user views sealed products for a set, **Then** the same sealed product data is returned as before.

---

### User Story 2 - Data Stays Current Through Ingestion Updates (Priority: P1)

As a platform operator, I can run the existing data ingestion pipeline and have the static data source automatically rebuilt with the latest Scryfall data. The updated data becomes available to users after a container restart. User-specific data (collections, wishlists, tracked sets) remains unaffected by the ingestion process.

**Why this priority**: Data freshness is critical for a card database platform. If the new data source cannot be rebuilt from the ingestion pipeline, the data will go stale and the platform loses value. This is tied with Story 1 as a baseline requirement.

**Independent Test**: Can be fully tested by running the ingestion pipeline, verifying the data file is generated, restarting the application, and confirming new card data (e.g., a recently released set) appears correctly.

**Acceptance Scenarios**:

1. **Given** a new Scryfall bulk data release is available, **When** the ingestion pipeline runs, **Then** a new data file is generated containing all cards, sets, artists, rulings, and sealed products from the latest release.
2. **Given** a new data file has been generated, **When** the application container restarts, **Then** the application serves data from the newly generated file.
3. **Given** the ingestion pipeline is running, **When** it completes the data generation step, **Then** user-specific data (collections, wishlists, set tracking) is completely unaffected.
4. **Given** the ingestion pipeline encounters a failure during data generation, **When** the failure occurs, **Then** the previously working data file remains intact and the application continues serving from it.

---

### User Story 3 - Infrastructure Cost Reduction for Static Data (Priority: P2)

As a platform operator, after migrating static data reads to the new data source, I can reduce the provisioned capacity on the 14 static data containers that no longer serve production read traffic. The static containers are retained for manual investigation via the cloud portal but no longer require production-level throughput, resulting in significant cost savings.

**Why this priority**: Cost reduction is one of the two primary goals of this migration. Once Story 1 confirms data parity, reducing infrastructure costs delivers immediate, ongoing business value.

**Independent Test**: Can be fully tested by confirming that all production read traffic for static data uses the new data source, then reducing provisioned throughput on static containers and verifying the application continues to function correctly.

**Acceptance Scenarios**:

1. **Given** all static data reads are served by the new data source, **When** the provisioned throughput on static data containers is reduced to minimum, **Then** the application continues to serve all user requests without errors or degraded performance.
2. **Given** the static data containers are at minimum throughput, **When** a platform operator accesses the data containers via the cloud portal for investigation, **Then** the data is still accessible for manual queries.
3. **Given** the application is running with the new data source, **When** monitoring is reviewed, **Then** zero production read requests are being made to the static data containers.

---

### User Story 4 - Configurable Data Source with Operator Investigatory Access (Priority: P2)

As a platform operator, I can configure the application to read static data from either the original source, the new source, or both. The "both" mode exists primarily for ingestion — it keeps the original data store populated so I can inspect data via the cloud portal. At runtime, only one source should serve reads. If both are technically active, the adapter layer merges results transparently so that no layer above the adapters is aware of the data source configuration. I can also roll back quickly to the original source if issues are discovered.

**Why this priority**: Migration safety and operator access are essential. Keeping the original data store populated during migration preserves investigatory access via the cloud portal. The ability to revert to the original source reduces deployment risk.

**Independent Test**: Can be fully tested by changing the data source configuration, restarting the application, and verifying that the expected data source serves the requests. For "both" mode, verify that ingestion writes to both stores and the adapter layer merges read results transparently.

**Acceptance Scenarios**:

1. **Given** the application is configured to use the original data source only, **When** the application starts, **Then** all static data reads come from the original source and the new source is not accessed.
2. **Given** the application is configured to use the new data source only, **When** the application starts, **Then** all static data reads come from the new source and the original source is not accessed for reads.
3. **Given** the application is configured to use both data sources, **When** the ingestion pipeline runs, **Then** both data stores are populated with the latest data, preserving the original store for operator investigation via the cloud portal.
4. **Given** the application is configured to use both data sources at runtime, **When** a user performs a query, **Then** the adapter layer merges results from both sources and returns them transparently — no layer above the adapters is aware of the dual-source configuration.
5. **Given** the application is running with the new data source, **When** an issue is discovered, **Then** an operator can change configuration back to the original source and restart to restore previous behavior.

---

### User Story 5 - Advanced Card Search Using Scryfall-Style Syntax (Priority: P3)

As a user who is familiar with Scryfall's search syntax, I can search for cards using field-specific queries such as `t:creature c:red cmc>=3` (creatures that are red and cost 3 or more mana) or `o:"draw a card" -t:creature` (non-creatures with "draw a card" in their rules text). The search supports field prefixes for name, oracle text, type line, colors, mana cost, power, toughness, set, rarity, format legality, and more. Results are paginated and can be sorted.

**Why this priority**: Advanced search is the second primary goal of this migration — enabled at zero additional cost by the new data source. However, it depends on Stories 1-2 being complete (data must be migrated and accurate before search can work). It delivers significant user value by matching the industry-standard search experience that MTG players expect.

**Independent Test**: Can be fully tested by entering Scryfall-style queries into the search interface and verifying the returned cards match the expected criteria.

**Acceptance Scenarios**:

1. **Given** the search system is available, **When** a user enters `name:bolt`, **Then** all cards with "bolt" as a substring of their name are returned (e.g., "Lightning Bolt", "Thunderbolt").
2. **Given** the search system is available, **When** a user enters `t:creature c:red cmc>=3`, **Then** only red creature cards with converted mana cost 3 or greater are returned.
3. **Given** the search system is available, **When** a user enters `o:"draw a card" -t:creature`, **Then** only non-creature cards whose rules text contains the phrase "draw a card" are returned.
4. **Given** the search system is available, **When** a user enters `(t:goblin or t:elf) c:R`, **Then** only red cards that are either goblins or elves are returned.
5. **Given** the search system is available, **When** a user enters `f:modern r>=rare`, **Then** only cards that are legal in Modern format and are rare or higher rarity are returned.
6. **Given** the search returns more results than fit on one page, **When** the user requests the next page, **Then** the next batch of results is returned without duplicates or gaps.
7. **Given** the search system is available, **When** a user enters a malformed query (e.g., unmatched parentheses), **Then** the system returns a helpful error message indicating where the syntax error occurred, rather than crashing or returning no results silently.

---

### User Story 6 - Advanced Search Boolean Logic and Modifiers (Priority: P4)

As a power user, I can combine search terms with boolean operators (AND, OR, NOT), group them with parentheses, negate individual terms with a `-` prefix, and use special modifiers like `is:foil`, `not:reprint`, `is:reserved`, `is:fullart`. I can sort results by name, mana cost, price, rarity, release date, or EDHREC rank. I can filter by year, date, and price ranges.

**Why this priority**: These are extensions to the core search functionality (Story 5). They complete the Scryfall-like experience but are not required for the search to be useful. Core field searches are more commonly used than complex boolean expressions.

**Independent Test**: Can be fully tested by entering complex boolean queries and verifying correct results, and by applying sort/filter options and verifying ordering.

**Acceptance Scenarios**:

1. **Given** the search system supports boolean logic, **When** a user enters `t:goblin or t:elf`, **Then** cards that are either goblins or elves (or both) are returned.
2. **Given** the search system supports negation, **When** a user enters `-is:digital is:foil`, **Then** only physical foil cards are returned.
3. **Given** the search system supports sorting, **When** a user enters `c:blue order:cmc`, **Then** blue cards are returned sorted by mana cost.
4. **Given** the search system supports date filtering, **When** a user enters `year>=2023 t:creature`, **Then** only creatures from sets released in 2023 or later are returned.
5. **Given** the search system supports exact name match, **When** a user enters `!"Lightning Bolt"`, **Then** only the card with that exact name is returned.
6. **Given** the search system supports regex, **When** a user enters `o:/\{T\}:.*draw/`, **Then** cards whose rules text matches the regex pattern are returned.

---

### User Story 7 - Frontend Search Interface (Priority: P5)

As a user, I can access a dedicated search page with a search bar that accepts Scryfall-style syntax. The page provides syntax help or autocomplete suggestions as I type. A sidebar offers faceted filters (color, rarity, set, format) that can be combined with or replace text queries. An alternative visual query builder allows users unfamiliar with text syntax to construct searches through dropdowns and form fields.

**Why this priority**: The frontend interface is the last phase — it requires all backend search functionality (Stories 5-6) to be complete. It enhances discoverability but the search is functional without it (queries can be submitted through the existing query interface).

**Independent Test**: Can be fully tested by navigating to the search page, using the search bar and faceted filters, and verifying that the UI correctly submits queries and displays results.

**Acceptance Scenarios**:

1. **Given** the search page is loaded, **When** a user types a query and submits, **Then** matching card results are displayed with card images, names, and key details.
2. **Given** the search page is loaded, **When** a user selects filters from the faceted sidebar (e.g., color: blue, rarity: rare), **Then** results are filtered accordingly and the active filters are visually indicated.
3. **Given** the search page is loaded, **When** a user types in the search bar, **Then** syntax help or suggestions are displayed to guide query construction.
4. **Given** a search returns many results, **When** the user scrolls or clicks to load more, **Then** additional results are loaded without losing current results or scroll position.
5. **Given** the search page is loaded on a mobile device, **When** the user interacts with search and filters, **Then** the interface is responsive and usable on smaller screens.
6. **Given** the search page is loaded, **When** the user views the search area, **Then** an informational element is visible listing supported syntax and noting unsupported Scryfall fields (e.g., `art:`, `cube:`, `function:`, `lang:`).
7. **Given** the search page is loaded, **When** a user types a query containing an unsupported field prefix (e.g., `art:landscape`), **Then** the client displays an inline error identifying the unsupported field before the query is submitted to the server.

---

### Edge Cases

- What happens when a user searches for a card that exists in the original data source but has not yet been ingested into the new data source? The system should return the result from whichever source is configured as active; if only the new source is active and the data is missing, no result is returned.
- How does the system handle extremely large result sets (e.g., a query matching 100,000+ cards)? Results are paginated with a maximum page size, and the total count is returned so the user knows the scope.
- What happens when the data file is corrupted or missing on application startup? The application should fail to start with a clear error message rather than serving partial or incorrect data.
- How does the system handle concurrent users performing searches at the same time? Multiple users can search simultaneously without blocking each other or experiencing degraded performance.
- What happens when a search query contains special characters or injection attempts? The system sanitizes all user input before processing and never passes raw user text into queries.
- What happens when the ingestion pipeline runs while users are actively using the application? The currently running application continues serving from the existing data file. Only after a restart does the new data file become active.
- How does the system handle Scryfall fields that have no equivalent in the local data (e.g., community tags, cube data)? The server rejects the query with a structured error identifying the unsupported field. The client pre-validates queries before submission to catch unsupported fields early and inform the user inline.

## Clarifications

### Session 2026-01-26

- Q: Does advanced card search require user authentication? → A: Public (no authentication) — any visitor can use advanced search, consistent with existing public card/set/artist queries.
- Q: How should the system handle unsupported Scryfall search fields (e.g., `art:`, `cube:`)? → A: Hard error — reject the query. The client MUST pre-validate queries for unsupported fields and inform the user before submission. The search UI MUST display an info box listing supported syntax and explicitly noting unsupported Scryfall fields.
- Q: What is the purpose of the "both" data source mode? → A: The "both" mode keeps Cosmos populated during ingestion so the operator can inspect data via the cloud portal. It is not for read-time data parity validation. At runtime, only one source should serve reads. If both are technically active, the adapter layer merges results so nothing above the adapter layer is aware of the data source configuration.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST serve all static card data reads from the configured data source without any change to the data returned to users.
- **FR-002**: System MUST support looking up cards by ID, by set code, by name, and by name substring search, returning identical results to the current system.
- **FR-003**: System MUST support looking up sets by ID, by code, and listing all sets, returning identical results to the current system.
- **FR-004**: System MUST support artist search by name substring and retrieving cards by artist, returning identical results to the current system.
- **FR-005**: System MUST support looking up sealed products by set code, returning identical results to the current system.
- **FR-006**: System MUST continue to read and write user-specific data (collections, wishlists, set tracking) from the existing user data store, unaffected by the static data migration.
- **FR-007**: System MUST provide a configuration option that controls which data source is active, with three modes: original only, new only, and both. In "both" mode, ingestion writes to both stores (preserving operator investigatory access to the original), and reads merge results at the adapter layer so no layer above the adapters is aware of the configuration.
- **FR-008**: System MUST rebuild the static data file during the ingestion pipeline, incorporating all cards, sets, artists, rulings, and sealed products from the latest Scryfall bulk data.
- **FR-009**: System MUST ensure that a failed data generation does not corrupt or replace the existing working data file.
- **FR-010**: System MUST support an advanced card search endpoint that accepts Scryfall-style query syntax and returns paginated results. This endpoint is public and does not require authentication.
- **FR-011**: System MUST support field-specific search prefixes for: card name, oracle text, type line, flavor text, artist name, keyword, colors, color identity, mana cost, power, toughness, loyalty, set code, set type, rarity, format legality, and price.
- **FR-012**: System MUST support comparison operators (equals, greater than, less than, greater-or-equal, less-or-equal) for numeric fields like mana cost, power, toughness, and price.
- **FR-013**: System MUST support boolean operators (AND, OR, NOT) and parenthesized grouping in search queries, with implicit AND between adjacent terms.
- **FR-014**: System MUST support negation via `-` prefix on any search term or group.
- **FR-015**: System MUST support `is:` and `not:` modifiers for boolean card properties (foil, reprint, reserved, digital, fullart, promo, and card layout types).
- **FR-016**: System MUST support sorting search results by name, mana cost, price, rarity, release date, and EDHREC rank, with configurable sort direction.
- **FR-017**: System MUST return structured, position-aware error messages for malformed search queries, indicating where the syntax error occurred and what was expected.
- **FR-018**: System MUST support regex pattern matching in search queries for oracle text fields.
- **FR-019**: System MUST support exact card name matching (e.g., `!"Lightning Bolt"` returns only the exact match).
- **FR-020**: System MUST support date and year filtering for card release dates.
- **FR-021**: System MUST paginate search results with cursor-based pagination, returning total count and page information.
- **FR-022**: System MUST handle color search semantics including: includes color, exactly these colors, superset of colors, subset of colors, and colorless.
- **FR-023**: System MUST sanitize all user-provided search input to prevent injection of unintended queries.
- **FR-024**: System MUST support multiple concurrent users performing searches without blocking or performance degradation.
- **FR-025**: System MUST provide a frontend search page with a search bar accepting Scryfall-style syntax.
- **FR-026**: System MUST provide a faceted filter sidebar on the search page allowing users to filter by color, rarity, set, and format.
- **FR-027**: System MUST provide syntax help or autocomplete in the search bar to guide users.
- **FR-028**: System MUST provide a responsive search interface that works on mobile devices.
- **FR-029**: System MUST reject search queries containing unsupported field prefixes with a structured error identifying the unsupported field by name and position.
- **FR-030**: The search UI MUST pre-validate queries on the client side for unsupported Scryfall fields and display an inline error before submitting to the server.
- **FR-031**: The search UI MUST display an informational element listing supported Scryfall search syntax and explicitly noting which Scryfall fields are not supported (e.g., `art:`, `cube:`, `function:`, `lang:`).

### Key Entities

- **Card**: A Magic: The Gathering card with identification, name, rules text, type, mana cost, combat stats, set information, art details, prices, format legality, and various boolean flags. Cards can have multiple faces, colors, keywords, finishes, and related parts.
- **Set**: A published collection of Magic cards with a code, name, release date, type, card count, and visual icon. Sets can have parent-child relationships.
- **Artist**: A person who illustrated one or more Magic cards, with a name and counts of cards and sets they contributed to.
- **Ruling**: An official rules clarification tied to a card's oracle identity, with a publication date, source, and explanatory comment.
- **Sealed Product**: A purchasable product (booster box, pack, bundle) associated with a set, with pricing and purchase links.
- **Card Face**: A distinct face of a multi-faced card (double-faced, split, flip), with its own name, mana cost, type, rules text, and art.
- **Search Query**: A user-entered text string following Scryfall-style syntax that is parsed into structured search criteria, validated, and executed against the card data.
- **Search Result**: A paginated collection of cards matching a search query, with total count and cursor-based navigation.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: 100% of existing card, set, artist, and sealed product queries return identical data after migration — zero data regression across all fields.
- **SC-002**: Static data queries respond with equal or better latency compared to the current system under normal load.
- **SC-003**: The system supports at least 100 concurrent users performing searches and data lookups simultaneously without errors or noticeable performance degradation.
- **SC-004**: Infrastructure cost for static data serving is reduced by at least 70% compared to the pre-migration baseline.
- **SC-005**: The data ingestion pipeline successfully rebuilds the complete static data file, including all ~300,000 cards, within a reasonable timeframe relative to the current ingestion process.
- **SC-006**: Advanced search queries covering all supported field types (text, numeric, color, boolean, date) return accurate results that match the expected Scryfall-equivalent behavior.
- **SC-007**: Malformed search queries return structured error messages with position information in 100% of cases, with no unhandled exceptions.
- **SC-008**: All existing automated tests continue to pass after migration with zero test failures.
- **SC-009**: The search interface is usable on both desktop and mobile screen sizes, with all interactive elements accessible and functional.
- **SC-010**: User-specific data operations (collections, wishlists, set tracking) are completely unaffected by the migration, with zero changes to their read/write behavior.

## Assumptions

- The Scryfall bulk data API continues to provide the same data format and fields used by the current ingestion pipeline.
- The ~300,000 card dataset fits within reasonable memory and storage constraints for the target deployment environment.
- Users familiar with Scryfall search syntax expect similar (not necessarily identical) behavior from the platform's search feature.
- English-language data only; multi-language search is out of scope.
- Community-curated data (Scryfall tags like `art:`, `function:`, cube data) is not available in the Scryfall API and is out of scope for search.
- The platform operator has access to modify the data source configuration and restart the application for migration cutover.
- The existing automated test suite provides sufficient coverage to verify data parity after migration.
