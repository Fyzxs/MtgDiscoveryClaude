# Quickstart: SQLite Migration Development

## Prerequisites

- .NET 9.0 SDK
- Existing solution builds: `dotnet build src/MtgDiscoveryVibe.sln`
- Azure Blob Storage access (for production; local file for development)

## New NuGet Packages

```bash
# Infrastructure project (Lib.Sqlite)
dotnet add src/Lib.Sqlite/Lib.Sqlite.csproj package Microsoft.Data.Sqlite --version 9.0.9

# Blob storage (ingestion pipeline)
dotnet add src/Lib.Scryfall.Ingestion/Lib.Scryfall.Ingestion.csproj package Azure.Storage.Blobs --version 12.23.0
dotnet add src/Lib.Scryfall.Ingestion/Lib.Scryfall.Ingestion.csproj package Azure.Identity --version 1.13.2
```

## Configuration

### appsettings.json

Add to `App.MtgDiscovery.GraphQL/appsettings.json`:

```json
{
  "StaticDataSource": "source_cosmos",
  "SqliteConfig": {
    "DataFilePath": "/app/data/mtg-data.db",
    "BlobContainerName": "sqlite-data",
    "BlobFileName": "mtg-data.db"
  }
}
```

### Development: Local SQLite File

For local development, generate a SQLite file and point the config to it:

```json
{
  "StaticDataSource": "source_sqlite",
  "SqliteConfig": {
    "DataFilePath": "./data/mtg-data.db"
  }
}
```

### Migration: Both Sources Active

For verification during migration:

```json
{
  "StaticDataSource": "source_both"
}
```

## Project Setup

### Create New Projects

```bash
cd src

# Infrastructure
dotnet new classlib -n Lib.Sqlite --framework net9.0
dotnet sln MtgDiscoveryVibe.sln add Lib.Sqlite/Lib.Sqlite.csproj

# Shared static adapter interfaces
dotnet new classlib -n Lib.Adapter.StaticSource --framework net9.0
dotnet sln MtgDiscoveryVibe.sln add Lib.Adapter.StaticSource/Lib.Adapter.StaticSource.csproj

# SQLite adapters
dotnet new classlib -n Lib.Adapter.Cards.Sqlite --framework net9.0
dotnet new classlib -n Lib.Adapter.Sets.Sqlite --framework net9.0
dotnet new classlib -n Lib.Adapter.Artists.Sqlite --framework net9.0
dotnet new classlib -n Lib.Adapter.SealedProducts.Sqlite --framework net9.0
dotnet sln MtgDiscoveryVibe.sln add Lib.Adapter.Cards.Sqlite/Lib.Adapter.Cards.Sqlite.csproj
dotnet sln MtgDiscoveryVibe.sln add Lib.Adapter.Sets.Sqlite/Lib.Adapter.Sets.Sqlite.csproj
dotnet sln MtgDiscoveryVibe.sln add Lib.Adapter.Artists.Sqlite/Lib.Adapter.Artists.Sqlite.csproj
dotnet sln MtgDiscoveryVibe.sln add Lib.Adapter.SealedProducts.Sqlite/Lib.Adapter.SealedProducts.Sqlite.csproj

# Query parser (pure logic, no infrastructure dependencies)
dotnet new classlib -n Lib.Search.QueryParser --framework net9.0
dotnet sln MtgDiscoveryVibe.sln add Lib.Search.QueryParser/Lib.Search.QueryParser.csproj

# Test projects
dotnet new mstest -n Lib.Sqlite.Tests --framework net9.0
dotnet new mstest -n Lib.Adapter.Cards.Sqlite.Tests --framework net9.0
dotnet new mstest -n Lib.Search.QueryParser.Tests --framework net9.0
dotnet sln MtgDiscoveryVibe.sln add Lib.Sqlite.Tests/Lib.Sqlite.Tests.csproj
dotnet sln MtgDiscoveryVibe.sln add Lib.Adapter.Cards.Sqlite.Tests/Lib.Adapter.Cards.Sqlite.Tests.csproj
dotnet sln MtgDiscoveryVibe.sln add Lib.Search.QueryParser.Tests/Lib.Search.QueryParser.Tests.csproj
```

### Add Project References

```bash
# Lib.Sqlite references
dotnet add src/Lib.Sqlite/Lib.Sqlite.csproj reference src/Lib.Universal/Lib.Universal.csproj

# Lib.Adapter.StaticSource references
dotnet add src/Lib.Adapter.StaticSource/Lib.Adapter.StaticSource.csproj reference src/Lib.Shared.DataModels/Lib.Shared.DataModels.csproj
dotnet add src/Lib.Adapter.StaticSource/Lib.Adapter.StaticSource.csproj reference src/Lib.Shared.Invocation/Lib.Shared.Invocation.csproj

# SQLite adapter references (reference StaticSource for interfaces, not .Cosmos)
dotnet add src/Lib.Adapter.Cards.Sqlite/Lib.Adapter.Cards.Sqlite.csproj reference src/Lib.Adapter.StaticSource/Lib.Adapter.StaticSource.csproj
dotnet add src/Lib.Adapter.Cards.Sqlite/Lib.Adapter.Cards.Sqlite.csproj reference src/Lib.Sqlite/Lib.Sqlite.csproj
dotnet add src/Lib.Adapter.Cards.Sqlite/Lib.Adapter.Cards.Sqlite.csproj reference src/Lib.Shared.Invocation/Lib.Shared.Invocation.csproj

# (Repeat pattern for Sets.Sqlite, Artists.Sqlite, SealedProducts.Sqlite)

# Cosmos adapter references to StaticSource (after interface extraction)
dotnet add src/Lib.Adapter.Cards.Cosmos/Lib.Adapter.Cards.Cosmos.csproj reference src/Lib.Adapter.StaticSource/Lib.Adapter.StaticSource.csproj
# (Repeat for Sets.Cosmos, Artists.Cosmos, SealedProducts.Cosmos)

# Aggregator references to StaticSource (for interface types) and SQLite adapters (for DI)
dotnet add src/Lib.Aggregator.Cards/Lib.Aggregator.Cards.csproj reference src/Lib.Adapter.StaticSource/Lib.Adapter.StaticSource.csproj
dotnet add src/Lib.Aggregator.Cards/Lib.Aggregator.Cards.csproj reference src/Lib.Adapter.Cards.Sqlite/Lib.Adapter.Cards.Sqlite.csproj
dotnet add src/Lib.Aggregator.Sets/Lib.Aggregator.Sets.csproj reference src/Lib.Adapter.Sets.Sqlite/Lib.Adapter.Sets.Sqlite.csproj
dotnet add src/Lib.Aggregator.Artists/Lib.Aggregator.Artists.csproj reference src/Lib.Adapter.Artists.Sqlite/Lib.Adapter.Artists.Sqlite.csproj
```

## Adapter Rename (Phase 0)

Rename existing adapter projects to include `.Cosmos` suffix:

```bash
# In src/ directory, rename folders
mv Lib.Adapter.Cards Lib.Adapter.Cards.Cosmos
mv Lib.Adapter.Sets Lib.Adapter.Sets.Cosmos
mv Lib.Adapter.Artists Lib.Adapter.Artists.Cosmos
mv Lib.Adapter.User Lib.Adapter.User.Cosmos
mv Lib.Adapter.UserCards Lib.Adapter.UserCards.Cosmos
mv Lib.Adapter.UserSetCards Lib.Adapter.UserSetCards.Cosmos

# Rename .csproj files inside each
mv Lib.Adapter.Cards.Cosmos/Lib.Adapter.Cards.csproj Lib.Adapter.Cards.Cosmos/Lib.Adapter.Cards.Cosmos.csproj
# ... repeat for each project

# Update all ProjectReference paths in:
# - Aggregator .csproj files
# - Test .csproj files
# - App.MtgDiscovery.GraphQL .csproj
# - Solution file (.sln)
# - All namespace declarations in .cs files
```

## Build Verification

After each phase, verify:

```bash
# Build entire solution
dotnet build src/MtgDiscoveryVibe.sln

# Run all tests
dotnet test src/MtgDiscoveryVibe.sln

# Run specific adapter tests
dotnet test src/Lib.Adapter.Cards.Sqlite.Tests/Lib.Adapter.Cards.Sqlite.Tests.csproj
```

## SQLite File Generation (Development)

To generate a local SQLite database for development testing:

1. Configure ingestion to output SQLite: set `StaticDataSource` to `source_both`
2. Run ingestion pipeline: `dotnet run --project src/Example.Scryfall.BulkIngestion/Example.Scryfall.BulkIngestion.csproj`
3. The SQLite file will be generated at the configured `DataFilePath`
4. Switch `StaticDataSource` to `source_sqlite` for the GraphQL API

## Connection String Reference

### Read-Only (GraphQL API Runtime)
```
Data Source=file:/app/data/mtg-data.db?immutable=1;Mode=ReadOnly;Pooling=True
```

### Write (Ingestion Pipeline)
```
Data Source=/app/data/mtg-data.db.tmp;Mode=ReadWriteCreate;Pooling=False
```

## Key PRAGMA Settings

### Read-Only Connection (set after opening)
```sql
PRAGMA journal_mode=OFF;
PRAGMA synchronous=OFF;
PRAGMA mmap_size=536870912;
PRAGMA temp_store=MEMORY;
PRAGMA query_only=ON;
PRAGMA cache_size=-65536;
```

### Write Connection (set during ingestion)
```sql
PRAGMA journal_mode=OFF;
PRAGMA synchronous=OFF;
PRAGMA page_size=4096;
PRAGMA cache_size=-65536;
PRAGMA temp_store=MEMORY;
PRAGMA locking_mode=EXCLUSIVE;
PRAGMA mmap_size=268435456;
```

## Implementation Order

1. **Phase 0**: Rename adapters to `.Cosmos`, add `StaticDataSource` config, verify unchanged behavior
2. **Phase 1**: Create `Lib.Sqlite`, create `Lib.Adapter.Cards.Sqlite`, generate SQLite cards data
3. **Phase 2**: Add sets, artists, sealed products SQLite adapters and data
4. **Phase 3**: Add FTS5 virtual tables and FTS5-backed search adapters
5. **Phase 4**: Create `Lib.Search.QueryParser` with lexer, parser, SQL translator
6. **Phase 5**: Add advanced search syntax (OR, parens, is:/not:, color expansion, regex, sorting)
7. **Phase 6**: Frontend search UI with syntax help and faceted filters
