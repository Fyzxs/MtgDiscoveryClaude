# Core Infrastructure Libraries

## Purpose
Foundational infrastructure providing database abstraction, configuration management, HTTP client, caching, and service location for the entire platform. Everything else depends on these libraries.

## Projects

### Lib.Cosmos
Azure Cosmos DB abstraction layer with type-safe operations, configuration management, and structured response handling.

**Public API** (Lib.Cosmos/Apis/):
- `ICosmosContainerAdapter` - Main interface combining all CRUD operators (Read, Upsert, Delete, Query)
- `OpResponse<T>` - Standardized operation response wrapping results with HTTP status codes
- `ICosmosConfiguration` - Configuration loading from appsettings.json (CerberusCosmosConfig key)
- `ICosmosContainerDefinition` - Per-container configuration (partition key, throughput, etc.)
- Point items: `ReadPointItem`, `DeletePointItem` (type-safe ID/partition key carriers)
- Base entities: `CosmosEntity`, `CosmosItem` (JSON-serializable with id, partition, metadata)

See: `Lib.Cosmos/Apis/Adapters/ICosmosContainerAdapter.cs`, `Lib.Cosmos/Apis/Operators/`

**Key Patterns**:
- **Operator Pattern**: Separate interfaces for each CRUD operation combined into adapter
- **Response Wrapping**: All operations return `OpResponse<T>` capturing status, value, and exceptions (not exceptions thrown)
- **Configuration as Objects**: Immutable domain objects from appsettings, not scattered strings
- **Genesis Pattern**: Database/container creation via `ICosmosGenesisClientAdapter`
- **Authentication Strategy**: `IGenesisDevice` abstracts auth modes (SAS via connection string, Entra ID)
- **MonoState Singleton**: Single static HttpClient instance across app lifetime

See: `Lib.Cosmos/Adapters/` for Genesis/Device implementations, `Lib.Cosmos/Apis/Configurations/` for config classes

**Used by**: All Adapter projects, CLI applications, examples, Lib.Scryfall.Ingestion

### Lib.Universal
Cross-platform utilities and primitives used everywhere. Foundation that Lib.Cosmos depends on.

**Public APIs**:
- `IServiceLocator` / `ServiceLocator` - Manual service location (factories, not DI framework)
- `IConfig` / `MonoStateConfig` - Global configuration access (IConfiguration wrapper, set-once semantics)
- `IHttpClient` / `MonoStateHttpClient` - HTTP client abstraction (singleton with 2-minute connection pooling)
- `IMemoryCache` / `MonoStateMemoryCache` - Caching abstraction (singleton wrapper)
- `ToSystemType<T>` - Base class for domain primitives with implicit conversion
- Primitives: `Url`, `ProvidedUrl`

See: `Lib.Universal/Inversion/IServiceLocator.cs`, `Lib.Universal/Configurations/MonoStateConfig.cs`, `Lib.Universal/Http/MonoStateHttpClient.cs`, `Lib.Universal/Primitives/ToSystemType.cs`

**Key Patterns**:
- **MonoState Pattern**: Everything singleton (ServiceLocator, Config, HttpClient, MemoryCache) accessed via static properties
- **No Null Returns**: Services throw if accessed before initialization
- **Thread-Safe Singletons**: Use static constructors and Semaphore for safe initialization
- **Configuration Holder**: `IConfiguration` set once globally at startup via `MonoStateConfig.SetConfiguration()`
- **Connection Pooling**: 2-minute lifetime for HTTP connections
- **Domain Primitives**: Wrap primitives with implicit conversion back (e.g., `Url` → `string`)

See: `Lib.Universal/` folders (Configurations, Inversion, Http, Caching, Primitives)

**Used by**: Everything (Lib.Cosmos, all Adapters, Aggregators, Domains, Common libraries, CLI apps)

## Established Patterns

### Configuration as Immutable Objects (Lib.Cosmos)
Not strings scattered throughout code. Configuration expressed through immutable domain objects created from appsettings.json. Type-safe DI. See: `Lib.Cosmos/Apis/Configurations/`

### MonoState Singleton Pattern (Lib.Universal & Lib.Cosmos)
Static field holds single instance accessible via property. Used for: ServiceLocator, Config, HttpClient, MemoryCache, CosmosClient. Provides application-wide state without DI framework.

### Domain Primitives with Implicit Conversion (Lib.Universal)
Domain objects wrap primitives (string, Uri) with type safety. Implicit operators enable seamless conversion. See: `Lib.Universal/Primitives/ToSystemType.cs`, `Lib.Universal/Primitives/Url.cs`

### Manual Service Location (Lib.Universal)
No DI framework. ServiceLocator holds factories, calls them once, caches result. See: `Lib.Universal/Inversion/ServiceLocator.cs`

## Critical Design Decisions

1. **No Dependency Injection Framework** - Uses MonoState + ServiceLocator pattern
2. **No Nulls in Responses** - Operations return response objects, not null values
3. **Singleton Everything** - Global state via MonoState pattern (acceptable for core infrastructure)
4. **Configuration as Objects** - Type-safe, immutable, from appsettings
6. **One-Time Config Init** - Semaphore prevents double-setting configuration
