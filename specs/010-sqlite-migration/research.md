# Research: SQLite Migration & Scryfall-Level Search

**Phase 0 Output** | Generated: 2026-01-26

## R1: Microsoft.Data.Sqlite Connection Management

### Decision
Use `Microsoft.Data.Sqlite` 9.0.x NuGet package with `immutable=1` URI filename for maximum read performance. One `SqliteConnection` per concurrent operation with pooling enabled (default).

### Rationale
- `Microsoft.Data.Sqlite` is the Microsoft-maintained ADO.NET provider aligned with .NET versioning
- `immutable=1` tells SQLite the file will never change, skipping all locking and change detection
- Connection pooling (default since v6.0) reuses native connections efficiently
- One connection per thread/async operation avoids all thread-safety issues

### Configuration

**Connection string**: `Data Source=file:/path/to/mtg-data.db?immutable=1;Mode=ReadOnly;Pooling=True`

**Pragmas (set after opening each connection)**:

| PRAGMA | Value | Reason |
|--------|-------|--------|
| `journal_mode` | `OFF` | No journal needed for read-only |
| `synchronous` | `OFF` | No fsync needed (no writes) |
| `mmap_size` | `536870912` (512MB) | Memory-mapped I/O for fast reads |
| `temp_store` | `MEMORY` | Temp tables in memory |
| `query_only` | `ON` | Safety net preventing writes |
| `cache_size` | `-65536` (64MB) | Increased page cache |

### Alternatives Rejected
- **WAL mode**: Requires write access for `-shm` file; inappropriate for read-only databases
- **`System.Data.SQLite`**: Older, not Microsoft-maintained
- **`Cache=Shared`**: Microsoft docs recommend against it; pooling handles connection reuse
- **Single shared connection**: Would serialize all reads, defeating concurrency

---

## R2: FTS5 Availability

### Decision
FTS5 is included by default in `SQLitePCLRaw.bundle_e_sqlite3` (shipped with `Microsoft.Data.Sqlite`). No additional packages needed.

### Rationale
- The default bundle is compiled with `SQLITE_ENABLE_FTS5` since SQLitePCLRaw updated to SQLite 3.22.0 (2018)
- Verify at runtime with `PRAGMA compile_options;` checking for `ENABLE_FTS5`

### Alternatives Rejected
- **`SQLitePCLRaw.bundle_e_sqlcipher`**: Adds encryption overhead not needed here
- **Custom SQLite build**: Unnecessary complexity; default bundle includes FTS5

---

## R3: REGEXP Function Registration

### Decision
Register a custom `regexp` function via `connection.CreateFunction(...)` per connection. Cache compiled `Regex` objects for performance.

### Rationale
- SQLite has no built-in `regexp` implementation; the function must be registered
- `CreateFunction` binds to the native connection handle; with pooling, it persists on the pooled native connection
- `isDeterministic: true` enables SQLite query optimizer to apply additional optimizations
- Cached `ConcurrentDictionary<string, Regex>` avoids re-compiling patterns on every call

### Key Detail
Register functions before any concurrent queries begin. Calling `CreateFunction` while another connection is executing can cause `SqliteException: SQLite Error 21`.

---

## R4: Thread Safety for Concurrent Reads

### Decision
One `SqliteConnection` per thread/async operation. Connection pooling handles efficiency. Do NOT share a single connection across threads.

### Rationale
- ADO.NET connections are not thread-safe by design
- SQLite supports concurrent readers natively (compiled with `SQLITE_THREADSAFE=1`)
- `immutable=1` eliminates all locking contention for concurrent readers
- Pooling ensures opening a new connection reuses a pooled native connection (near-zero cost)

---

## R5: SQLite File Generation During Ingestion

### Decision
Use a single wrapping transaction with aggressive PRAGMA settings. Create indexes and FTS5 tables after data insertion. Write to temp file, rename on completion.

### Rationale
- Single transaction reduces disk syncs from 300K (one per row) to 1 (at commit)
- `journal_mode=OFF` and `synchronous=OFF` eliminate all journal I/O during writes
- Building indexes after data insertion is faster than maintaining indexes during inserts
- FTS5 `rebuild` command builds the full-text index in one pass after content tables are populated

### Write-Time Pragmas

| PRAGMA | Value | Reason |
|--------|-------|--------|
| `journal_mode` | `OFF` | File is disposable if ingestion fails |
| `synchronous` | `OFF` | No fsync needed during generation |
| `page_size` | `4096` | Aligns with OS/filesystem page size |
| `cache_size` | `-65536` (64MB) | Reduces disk reads during index creation |
| `temp_store` | `MEMORY` | Temp tables in memory |
| `locking_mode` | `EXCLUSIVE` | Single writer, no lock management overhead |
| `mmap_size` | `268435456` (256MB) | Memory-mapped I/O speeds writes on Linux |

### Post-Ingestion Steps
1. `INSERT INTO fts_table(fts_table) VALUES('optimize')` for each FTS5 table
2. `ANALYZE` to generate query planner statistics
3. `VACUUM` to defragment and compact the file
4. Rename `mtg-data.db.tmp` -> `mtg-data.db`

---

## R6: Azure Blob Storage for SQLite File Hosting

### Decision
Use `Azure.Storage.Blobs` SDK with `BlobClient.DownloadToAsync` for download on container startup. Upload generated file with `BlobClient.UploadAsync`.

### Rationale
- `BlobClient.DownloadToAsync(localPath)` streams directly to a file without loading into memory
- 300MB download on Azure-internal networking: 5-15 seconds
- `BlobClient.UploadAsync` auto-chunks large files (default 4MB blocks)
- Managed Identity via `Azure.Identity` for production; connection string for dev

### NuGet Packages
- `Azure.Storage.Blobs` 12.x
- `Azure.Identity` 1.x (for managed identity authentication)

### Alternatives Rejected
- **Azure Files (SMB mount)**: SQLite over network shares is discouraged; local file is faster and more reliable
- **Embedding in container image**: Requires container rebuild for every data update
- **CDN**: Unnecessary; single consumer (container app)

---

## R7: Hot-Swap vs Container Restart

### Decision
Container restart via Azure Container Apps revision management (blue/green deployment). Hot-swap is a future optimization if needed.

### Rationale
- Very low complexity: new container starts, downloads latest blob, opens SQLite, serves traffic
- Near-zero downtime: ACA routes traffic to new revision once healthy; old revision drains
- Clean slate: no leaked file handles, memory-mapped regions, or stale caches
- Trivial rollback: reactivate previous revision

### Update Trigger
After ingestion uploads new blob, trigger new revision deployment via Azure CLI or update an environment variable (e.g., `SQLITE_BLOB_VERSION`) to force a new revision.

### Alternatives Rejected
- **Hot-swap**: High complexity (concurrent file access, atomic pool swap, connection draining); warranted only if updates more frequent than every 30 minutes
- **Sidecar pattern**: Adds complexity with container-to-container coordination

---

## R8: SQLite File Size Estimation

### Decision
Budget 400MB as upper bound. Expected 250-380MB after VACUUM.

### Breakdown

| Component | Low Estimate | High Estimate |
|-----------|-------------|---------------|
| Cards table (~300K rows, 180+ cols) | 200MB | 270MB |
| Junction tables (card_colors, card_keywords, etc.) | 30MB | 60MB |
| Regular indexes | 20MB | 40MB |
| FTS5 indexes (external content) | 30MB | 60MB |
| SQLite overhead | 10MB | 20MB |
| **Total (before VACUUM)** | **290MB** | **450MB** |
| **After VACUUM** | **~250MB** | **~380MB** |

### Size Minimization
- `VACUUM` after all data/indexes: 10-20% reduction
- External content FTS5 tables (`content=base_table`): avoids text duplication
- `WITHOUT ROWID` on junction tables with composite PKs: ~20-30% space savings per table
- Gzip for blob transfer: ~100-150MB compressed

---

## R9: Query Parser Architecture

### Decision
Hand-written recursive descent parser with separate tokenizer. Sealed class hierarchy AST with Visitor pattern for SQL translation.

### Rationale
- Grammar is small (~10-15 production rules) -- does not warrant a parser library
- Zero external dependencies aligns with `Lib.Search.QueryParser` as a standalone library
- Full control over error messages with position tracking
- MicroObjects alignment: interface-per-class, sealed classes, composition over inheritance
- Parser combinator libraries (Superpower, Pidgin, Sprache) impose their own compositional model that conflicts with MicroObjects patterns

### Architecture

**Phase 1 - Tokenizer**: Raw string -> flat list of tokens
- Token kinds as sealed class hierarchy (not enums, per MicroObjects)
- Each token carries kind, text value, and character offset position
- Handles: field prefixes, comparison operators, quoted strings, boolean keywords, parens, negation prefix, bare words

**Phase 2 - Parser**: Token stream -> AST via recursive descent

Grammar (informal BNF):
```
Query       ::= OrExpr
OrExpr      ::= AndExpr ( 'OR' AndExpr )*
AndExpr     ::= UnaryExpr ( UnaryExpr | 'AND' UnaryExpr )*
UnaryExpr   ::= 'NOT' UnaryExpr | '-' UnaryExpr | PrimaryExpr
PrimaryExpr ::= '(' OrExpr ')' | FieldExpr | TextExpr
FieldExpr   ::= FIELD_NAME OPERATOR VALUE
TextExpr    ::= QUOTED_STRING | BARE_WORD
```

Implicit AND between adjacent terms (no explicit keyword required).

**Phase 3 - SQL Translator**: AST -> SQL WHERE clause + parameters via Visitor pattern

### AST Node Types

| Node Type | Properties | SQL Translation |
|-----------|-----------|----------------|
| `IAndNode` | `IReadOnlyList<ISearchNode> Children` | `(child1) AND (child2) ...` |
| `IOrNode` | `IReadOnlyList<ISearchNode> Children` | `(child1) OR (child2) ...` |
| `INotNode` | `ISearchNode Child` | `NOT (child)` |
| `IFieldComparisonNode` | `ISearchField Field`, `IComparisonOperator Op`, `string Value` | `column >= @p0` |
| `ITextSearchNode` | `string SearchText` | `cards_fts MATCH @p0` |
| `IFieldTextNode` | `ISearchField Field`, `string SearchText` | Field-scoped FTS5 MATCH or LIKE |

### Alternatives Rejected
- **Superpower**: External dependency; combinator style conflicts with MicroObjects
- **Pidgin**: Fastest combinator lib but still an external dependency
- **Sprache**: Older, less maintained
- **ANTLR**: Overkill; generated code harder to integrate with MicroObjects

---

## R10: SQL Injection Prevention

### Decision
Two-layer defense: parameterized SQL (`@p0`, `@p1`) for all user values + FTS5 double-quote wrapping for MATCH expressions.

### Layer 1: Parameterized SQL
- All user-provided values passed via `SqliteParameter` objects
- Parameter names auto-generated (`@p0`, `@p1`) during AST traversal
- User input never appears directly in SQL text

### Layer 2: FTS5 Expression Sanitization
- Replace all `"` in user text with `""` (SQL-style escaping)
- Wrap each term in double quotes to force verbatim literal matching
- This prevents FTS5 boolean injection (`AND`, `OR`, `NOT`, `*`, `NEAR`)

---

## R11: Parser Testing Strategy

### Decision
Three-level isolated testing (lexer, parser, translator) plus targeted property-based tests for safety invariants.

### Level 1: Lexer Tests
- Input: raw string
- Output: token list
- Test categories: single token recognition, quoted strings, whitespace, negation, edge cases, position tracking

### Level 2: Parser Tests
- Input: pre-constructed token list (isolates from lexer)
- Output: AST
- Test categories: field expressions, boolean operators, implicit AND, negation, grouping, precedence, errors

### Level 3: Translator Tests
- Input: pre-constructed AST (isolates from parser)
- Output: SQL WHERE clause + parameter list
- Test categories: field comparisons, text search, boolean composition, negation, parameter numbering, FTS5 sanitization

### Integration Tests
- Input: raw query string -> full pipeline -> SQL + params
- Small set covering key query patterns from the Scryfall syntax spec

### Property-Based Tests (FsCheck)
- Round-trip: `parse(tokenize(input))` never throws unhandled exception
- Parameter safety: translator output never contains user text in SQL string
- Parenthesization: generated SQL has balanced parentheses

---

## R12: Parser Error Handling

### Decision
Position-tracked structured errors with graceful degradation and panic-mode recovery.

### Error Design
Each error contains: position (char offset), error kind, expected token, found token, optional context.

### Graceful Degradation Rules
- Partial queries work: trailing incomplete field expressions are ignored
- ~~Unknown fields fall back to text search (matches Scryfall behavior)~~ **Overridden**: Per clarification session 2026-01-26, unsupported fields are rejected with structured errors (FR-029). Client pre-validates before submission (FR-030). Info box lists supported/unsupported syntax (FR-031).
- Unmatched parentheses auto-close at end of input
- Panic-mode recovery: on error, skip to next synchronization point (whitespace/paren) and resume

### Return Type
Follows the project's `IOperationResponse<T>` pattern: success returns AST, failure returns structured errors, partial parse returns both AST and warnings.

---

## Technology Summary

| Component | Package | Version |
|-----------|---------|---------|
| SQLite ADO.NET Provider | `Microsoft.Data.Sqlite` | 9.0.x |
| Native SQLite (includes FTS5) | `SQLitePCLRaw.bundle_e_sqlite3` | (transitive) |
| Azure Blob Storage | `Azure.Storage.Blobs` | 12.x |
| Azure Managed Identity | `Azure.Identity` | 1.x |
| Query Parser | Hand-written (no dependency) | N/A |
| Testing | MSTest + AwesomeAssertions (existing) | (existing) |
| Property Testing | FsCheck (optional) | 3.x |
