# Core Infrastructure

## Purpose
Foundational libraries: database abstraction (Cosmos), configuration, HTTP client, caching, service location. Everything depends on these.

## Lib.Cosmos

Azure Cosmos DB abstraction with type-safe operations and response handling.

**Main Types**:
- `ICosmosContainerAdapter` — CRUD operations (Read, Upsert, Delete, Query)
- `OpResponse<T>` — Operation response (status, value, exceptions as values)
- `ICosmosConfiguration` — Config from appsettings (CerberusCosmosConfig key)
- `ICosmosContainerDefinition` — Per-container configuration
- `ReadPointItem`, `DeletePointItem` — Type-safe ID/partition carriers
- `CosmosEntity`, `CosmosItem` — JSON-serializable base entities

**Key Patterns**:
- **Operator Pattern** — Separate interfaces for each CRUD operation
- **Response Wrapping** — All operations return `OpResponse<T>` (never throw exceptions)
- **Genesis Pattern** — Database/container creation via `ICosmosGenesisClientAdapter`
- **Authentication** — `IGenesisDevice` abstracts SAS vs. Entra ID

See: `Lib.Cosmos/Apis/Adapters/`, `Lib.Cosmos/Apis/Operators/`, `Lib.Cosmos/Apis/Configurations/`

## Lib.Universal

Cross-platform utilities: configuration, HTTP client, caching, service location, domain primitives.

**Main Types**:
- `MonoStateConfig` — Global configuration (set once at startup)
- `MonoStateHttpClient` — HTTP client singleton (2-min connection pooling)
- `MonoStateMemoryCache` — Caching abstraction
- `ServiceLocator` — Manual service location (factories)
- `ToSystemType<T>` — Base class for domain primitives with implicit conversion
- `Url`, `ProvidedUrl` — Domain primitives

**Key Patterns**:
- **MonoState Pattern** — Static fields for singletons (ServiceLocator, Config, HttpClient, MemoryCache, CosmosClient)
- **No Null Returns** — Services throw if accessed before initialization
- **Thread-Safe Init** — Semaphore prevents double-setting configuration
- **Domain Primitives** — Wrap primitives with implicit conversion (e.g., `Url` → `string`)

See: `Lib.Universal/Inversion/`, `Lib.Universal/Configurations/`, `Lib.Universal/Http/`, `Lib.Universal/Primitives/`

## Critical Design Decisions

- **No DI Framework** — MonoState + ServiceLocator only
- **Never Null in Responses** — Return response objects, never null
- **Singletons Are Acceptable** — Core infrastructure gets global state
- **Configuration as Objects** — Type-safe, immutable, from appsettings
- **One-Time Config Init** — Semaphore prevents reconfiguration
