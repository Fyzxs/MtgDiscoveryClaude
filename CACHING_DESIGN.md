# Adapter Layer In-Memory Cache Design

## Executive Summary

This document describes the design for implementing in-memory caching at the adapter layer to reduce Cosmos DB Request Unit (RU) consumption and improve response times. The design follows MicroObjects principles with self-describing entities that define their own cache keys.

**Primary Goals:**
- Reduce Cosmos DB Request Unit (RU) consumption by 70-90%
- Improve response times for all data access operations
- Maintain cache consistency through write-through invalidation
- Follow MicroObjects patterns (interfaces, self-describing behavior, dependency injection)

**Design Philosophy:**
- **Single caching policy**: Cache until explicitly invalidated (no time-based expiration)
- **Self-describing entities**: Transfer entities define their own cache keys
- **Collaborator pattern**: Adapters coordinate between cache and Cosmos operators
- **Type safety**: Marker interfaces for each entity type enforce cacheability
- **Single instance**: In-memory cache for single-instance deployment

**Key Metrics:**
- Target: 80%+ cache hit rate for all data types
- Expected: 70-90% reduction in Cosmos RU consumption
- Expected: 90%+ improvement in response times for cached data

---

## Architecture Overview

### Core Principles

1. **Cache at Adapter Layer**: Implement caching in adapter classes, not Cosmos operators
2. **Collaborator Pattern**: Adapters have both Cosmos operator and cache service as dependencies
3. **Self-Describing Entities**: XfrEntity interfaces define their own cache keys via `ICacheableEntity`
4. **Single Cache Policy**: "Cache until invalidated" for all data types
5. **Write-Through Invalidation**: Command adapters invalidate cache on successful writes
6. **Single Instance**: In-memory cache (MemoryCache), no distributed cache concerns
7. **MicroObjects Compliant**: Interface-based, constructor injection, objects expose behavior

### Layer Integration

```
┌─────────────────────────────────────────────────────────────┐
│ Aggregator Layer                                            │
│ (Calls Adapter interfaces - no cache awareness)            │
└─────────────────────────────────────────────────────────────┘
                           ↓
┌─────────────────────────────────────────────────────────────┐
│ Adapter Layer (Lib.Adapter.Cards, Lib.Adapter.Sets, etc.)  │
│                                                             │
│  ┌──────────────────────────────────────────────────────┐  │
│  │ CardQueryAdapter : ICardQueryAdapter                 │  │
│  │  ├─→ ICacheService _cache                            │  │
│  │  ├─→ ICardGopher _gopher                             │  │
│  │  │                                                    │  │
│  │  └─→ GetCardByIdAsync(id)                            │  │
│  │       ├─→ Check cache using entity.CacheKey          │  │
│  │       ├─→ Cache Hit: Return cached result            │  │
│  │       └─→ Cache Miss: Fetch from Gopher, cache it    │  │
│  └──────────────────────────────────────────────────────┘  │
│                                                             │
│  ┌──────────────────────────────────────────────────────┐  │
│  │ UserCardsCommandAdapter : IUserCardsCommandAdapter   │  │
│  │  ├─→ ICacheService _cache                            │  │
│  │  ├─→ IUserCardsScribe _scribe                        │  │
│  │  │                                                    │  │
│  │  └─→ UpsertUserCardAsync(entity)                     │  │
│  │       ├─→ Call Scribe to write                       │  │
│  │       ├─→ On success: Invalidate user partition      │  │
│  │       └─→ Return result                              │  │
│  └──────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                           ↓
┌─────────────────────────────────────────────────────────────┐
│ Cosmos Implementation (Lib.Adapter.Scryfall.Cosmos)        │
│ (Gophers, Scribes, Inquisitors - no cache awareness)       │
└─────────────────────────────────────────────────────────────┘
                           ↓
┌─────────────────────────────────────────────────────────────┐
│ Cache Infrastructure (Lib.Shared.Caching)                  │
│  ┌─────────────────┐                                       │
│  │ ICacheService   │  (Simple interface)                   │
│  │ MemoryCacheService                                      │
│  └─────────────────┘                                       │
└─────────────────────────────────────────────────────────────┘
```

---

## Comprehensive Marker Interface Pattern

### Establishing Marker Interfaces for All Entity Types

**Important:** While this document focuses on caching, we are using this opportunity to establish **marker interfaces for ALL entity types across ALL entity layers**. This is a comprehensive architectural pattern adoption, not just for caching.

### Why Marker Interfaces for Everything?

**MicroObjects Principle:**
- Every concept should have an explicit representation through interfaces
- Each entity type gets its own marker interface (1:1 mapping)
- Type system enforces entity type distinctions
- Interface-level behaviors can be defined once for all implementations

**Benefits:**
1. **Type Safety**: Prevents mixing entity types at compile time
2. **Single Source of Truth**: Behaviors defined at interface level
3. **Future Extensibility**: Easy to add cross-cutting concerns (validation, serialization, auditing)
4. **Clear Documentation**: Type system documents all entity types
5. **Consistency**: Same pattern across all layers

### Entity Layers and Marker Interfaces

The system has multiple entity layers following the data flow pattern. **Each entity type in each layer needs its own marker interface:**

#### ArgEntity Layer (App → Entry)
Argument entities from external input (GraphQL, REST, etc.)

```csharp
namespace Lib.Shared.DataModels.Entities.Args;

public interface ICardArgEntity : IArgEntity { }
public interface ISetArgEntity : IArgEntity { }
public interface IArtistArgEntity : IArgEntity { }
public interface IUserCardArgEntity : IArgEntity { }
public interface IUserSetCardArgEntity : IArgEntity { }
public interface IUserInfoArgEntity : IArgEntity { }
```

#### ItrEntity Layer (Entry ↔ Domain ↔ Aggregator)
Internal transfer entities between service layers

```csharp
namespace Lib.Shared.DataModels.Entities.Itrs;

public interface ICardItrEntity : IItrEntity { }
public interface ISetItrEntity : IItrEntity { }
public interface IArtistItrEntity : IItrEntity { }
public interface IUserCardItrEntity : IItrEntity { }
public interface IUserSetCardItrEntity : IItrEntity { }
public interface IUserInfoItrEntity : IItrEntity { }
```

#### XfrEntity Layer (Adapter Operations)
Transfer entities used within adapter layer operations

```csharp
namespace Lib.Shared.DataModels.Entities.Xfrs;

// These also implement ICacheableEntity for caching
public interface ICardXfrEntity : IXfrEntity { }
public interface ISetXfrEntity : IXfrEntity { }
public interface IArtistXfrEntity : IXfrEntity { }
public interface IUserCardXfrEntity : IXfrEntity { }
public interface IUserSetCardXfrEntity : IXfrEntity { }
public interface IUserInfoXfrEntity : IXfrEntity { }
```

#### ExtEntity Layer (Cosmos Documents)
External system entities (Cosmos DB documents)

```csharp
namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

// Note: These may not need marker interfaces since they're implementation-specific
// But for consistency, could add:
public interface ICardExtEntity : IExtEntity { }
public interface ISetExtEntity : IExtEntity { }
public interface IArtistExtEntity : IExtEntity { }
```

#### OufEntity Layer (Domain/Aggregator → Entry)
Output from domain/aggregator layers (internal layer outputs)

```csharp
namespace Lib.Shared.DataModels.Entities.Oufs;

public interface ICardOufEntity : IOufEntity { }
public interface ISetOufEntity : IOufEntity { }
public interface IArtistOufEntity : IOufEntity { }
public interface IUserCardOufEntity : IOufEntity { }
public interface IUserSetCardOufEntity : IOufEntity { }
```

#### OutEntity Layer (Entry → App)
Output entities returned to external layer (GraphQL responses)

```csharp
namespace Lib.Shared.DataModels.Entities.Outs;

public interface ICardOutEntity : IOutEntity { }
public interface ISetOutEntity : IOutEntity { }
public interface IArtistOutEntity : IOutEntity { }
public interface IUserCardOutEntity : IOutEntity { }
public interface IUserSetCardOutEntity : IOutEntity { }
```

### Complete Entity Type Matrix

For **each entity concept**, create marker interface in **each relevant layer**:

| Entity Concept | ArgEntity | ItrEntity | XfrEntity | OufEntity | OutEntity |
|---------------|-----------|-----------|-----------|-----------|-----------|
| Card | ICardArgEntity | ICardItrEntity | ICardXfrEntity | ICardOufEntity | ICardOutEntity |
| Set | ISetArgEntity | ISetItrEntity | ISetXfrEntity | ISetOufEntity | ISetOutEntity |
| Artist | IArtistArgEntity | IArtistItrEntity | IArtistXfrEntity | IArtistOufEntity | IArtistOutEntity |
| UserCard | IUserCardArgEntity | IUserCardItrEntity | IUserCardXfrEntity | IUserCardOufEntity | IUserCardOutEntity |
| UserSetCard | IUserSetCardArgEntity | IUserSetCardItrEntity | IUserSetCardXfrEntity | IUserSetCardOufEntity | IUserSetCardOutEntity |
| UserInfo | IUserInfoArgEntity | IUserInfoItrEntity | IUserInfoXfrEntity | IUserInfoOufEntity | IUserInfoOutEntity |

### Implementation Scope

**Phase 1 (This Caching Implementation):**
- Create ALL marker interfaces across ALL layers
- Implement cache-specific behavior ONLY in XfrEntity layer
- Update existing concrete entity classes to implement their marker interfaces
- No behavior changes to non-XfrEntity layers (just add interfaces)

**Benefits of Doing This Now:**
1. **One-time cost**: Establish pattern comprehensively
2. **Avoid partial adoption**: No mixing of old and new patterns
3. **Clear architecture**: Type system reflects all entity types
4. **Easy future additions**: Pattern is established for new entity types

---

## Self-Describing Entity Pattern (Caching-Specific)

### ICacheableEntity Interface

**XfrEntity layer only:** Transfer entities in the adapter layer implement `ICacheableEntity`, which defines how they generate their cache key.

```csharp
namespace Lib.Shared.DataModels.Abstractions;

/// <summary>
/// Marker interface for entities that can be cached.
/// Entities must provide their own cache key.
/// </summary>
public interface ICacheableEntity
{
    /// <summary>
    /// Unique cache key for this entity instance.
    /// Format determined by entity type (e.g., "card:{id}", "user:{userId}:card:{cardId}")
    /// </summary>
    string CacheKey { get; }
}
```

### IXfrEntity Base Interface

All transfer entities inherit from `IXfrEntity`, which enforces cacheability:

```csharp
namespace Lib.Shared.DataModels.Abstractions;

/// <summary>
/// Base interface for all transfer entities in the adapter layer.
/// All XfrEntities are cacheable by design.
/// </summary>
public interface IXfrEntity : ICacheableEntity
{
    // Marker interface - no additional members
}
```

### Entity Type Marker Interfaces

Each entity type has its own marker interface that defines the cache key format using default interface implementation:

```csharp
namespace Lib.Shared.DataModels.Entities.Xfrs;

/// <summary>
/// Card transfer entity interface.
/// Cache key format: "card:{CardId}"
/// </summary>
public interface ICardXfrEntity : IXfrEntity
{
    string CardId { get; }

    // Default implementation - all Card entities use this format
    string ICacheableEntity.CacheKey => $"card:{CardId}";
}

/// <summary>
/// Set transfer entity interface.
/// Cache key format: "set:{SetCode}"
/// </summary>
public interface ISetXfrEntity : IXfrEntity
{
    string SetCode { get; }

    string ICacheableEntity.CacheKey => $"set:{SetCode}";
}

/// <summary>
/// User card transfer entity interface (user-partitioned).
/// Cache key format: "user:{UserId}:card:{CardId}"
/// </summary>
public interface IUserCardXfrEntity : IXfrEntity
{
    string UserId { get; }
    string CardId { get; }

    string ICacheableEntity.CacheKey => $"user:{UserId}:card:{CardId}";
}

/// <summary>
/// Artist transfer entity interface.
/// Cache key format: "artist:{ArtistId}"
/// </summary>
public interface IArtistXfrEntity : IXfrEntity
{
    string ArtistId { get; }

    string ICacheableEntity.CacheKey => $"artist:{ArtistId}";
}
```

### Concrete Entity Implementation

Concrete entity classes inherit the cache key implementation automatically:

```csharp
namespace Lib.Shared.DataModels.Entities.Xfrs;

public sealed class CardXfrEntity : ICardXfrEntity
{
    public string CardId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    // ... other properties

    // CacheKey implementation inherited from ICardXfrEntity interface
    // No need to implement it here
}

public sealed class UserCardXfrEntity : IUserCardXfrEntity
{
    public string UserId { get; init; } = string.Empty;
    public string CardId { get; init; } = string.Empty;
    public int Quantity { get; init; }
    // ... other properties

    // CacheKey implementation inherited from IUserCardXfrEntity interface
}
```

**Benefits of This Approach:**
- **Consistency**: All implementations of `ICardXfrEntity` have identical cache key format
- **Type Safety**: Impossible to create a Card entity that doesn't have a cache key
- **Single Source of Truth**: Cache key logic defined once at the type level
- **Query/Command Coordination**: Both read and write operations use the same entity interface, guaranteeing cache key consistency

---

## Cache Service Interface

### ICacheService

Simple interface with four core operations:

```csharp
namespace Lib.Shared.Caching.Apis.Abstractions;

/// <summary>
/// Cache service for storing and retrieving entities.
/// Uses entity-provided cache keys.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Gets cached value or creates it using factory function.
    /// Cache key comes from entity.CacheKey after factory executes.
    /// </summary>
    Task<IOperationResponse<TEntity>> GetOrCreateAsync<TEntity>(
        string cacheKey,
        Func<Task<IOperationResponse<TEntity>>> factory)
        where TEntity : class, ICacheableEntity;

    /// <summary>
    /// Invalidates a specific cache entry by key.
    /// </summary>
    void Invalidate(string cacheKey);

    /// <summary>
    /// Invalidates all cache entries for a user partition.
    /// Removes all entries with keys starting with "user:{userId}:"
    /// </summary>
    void InvalidateUserPartition(string userId);

    /// <summary>
    /// Clears entire cache (nuclear option for bulk reimport scenarios).
    /// </summary>
    void Clear();
}
```

**Design Notes:**
- Uses string cache keys (no ICacheKey interface needed)
- Factory returns `IOperationResponse<T>` (caches both success and failure responses)
- User partition invalidation uses prefix matching
- No policy parameters (single policy: cache forever)

---

## Adapter Collaborator Pattern

### Query Adapter Example

Query adapters check cache before calling Cosmos operators:

```csharp
namespace Lib.Adapter.Cards;

internal sealed class CardQueryAdapter : ICardQueryAdapter
{
    private readonly ICardGopher _gopher;
    private readonly ICacheService _cache;

    public CardQueryAdapter(
        ICardGopher gopher,
        ICacheService cache)
    {
        _gopher = gopher;
        _cache = cache;
    }

    public async Task<IOperationResponse<ICardXfrEntity>> GetCardByIdAsync(string cardId)
    {
        string cacheKey = $"card:{cardId}";

        IOperationResponse<ICardXfrEntity> response = await _cache
            .GetOrCreateAsync(
                cacheKey,
                async () =>
                {
                    // Cache miss - fetch from Cosmos
                    IOperationResponse<CardExtEntity> cosmosResponse = await _gopher
                        .GetByIdAsync(cardId)
                        .ConfigureAwait(false);

                    // Map ExtEntity to XfrEntity
                    return MapToXfrEntity(cosmosResponse);
                })
            .ConfigureAwait(false);

        return response;
    }

    private IOperationResponse<ICardXfrEntity> MapToXfrEntity(
        IOperationResponse<CardExtEntity> cosmosResponse)
    {
        // Mapping logic...
    }
}
```

**Key Points:**
- Adapter explicitly coordinates cache and Cosmos operator
- Cache key matches entity's cache key format
- Caches the entire `IOperationResponse<T>` (including failures)
- Factory function only executes on cache miss

### Command Adapter Example

Command adapters invalidate cache after successful writes:

```csharp
namespace Lib.Adapter.UserCards;

internal sealed class UserCardsCommandAdapter : IUserCardsCommandAdapter
{
    private readonly IUserCardsScribe _scribe;
    private readonly ICacheService _cache;

    public UserCardsCommandAdapter(
        IUserCardsScribe scribe,
        ICacheService cache)
    {
        _scribe = scribe;
        _cache = cache;
    }

    public async Task<IOperationResponse> UpsertUserCardAsync(IUserCardXfrEntity entity)
    {
        // Map XfrEntity to ExtEntity
        UserCardsExtEntity extEntity = MapToExtEntity(entity);

        // Write to Cosmos
        IOperationResponse result = await _scribe
            .UpsertAsync(extEntity)
            .ConfigureAwait(false);

        // Invalidate cache on success
        if (result.IsSuccess)
        {
            // Invalidate entire user partition (aggressive but safe)
            _cache.InvalidateUserPartition(entity.UserId);

            // Alternative: Invalidate only specific entity
            // _cache.Invalidate(entity.CacheKey);
        }

        return result;
    }

    private UserCardsExtEntity MapToExtEntity(IUserCardXfrEntity xfrEntity)
    {
        // Mapping logic...
    }
}
```

**Key Points:**
- Write operations don't check cache (always write through to Cosmos)
- Invalidation happens only on successful write
- User partition invalidation removes all cached data for that user
- Could use specific entity invalidation for more precision

### Inquisitor (Query) Adapter Example

Query operations also benefit from caching:

```csharp
namespace Lib.Adapter.Cards;

internal sealed class CardQueryAdapter : ICardQueryAdapter
{
    private readonly ICardInquisitor _inquisitor;
    private readonly ICacheService _cache;

    // ... constructor

    public async Task<IOperationResponse<IReadOnlyList<ICardXfrEntity>>> GetCardsByArtistAsync(
        string artistId,
        int page,
        int pageSize)
    {
        // Generate cache key from query parameters
        string cacheKey = $"query:cardsByArtist:{artistId}:p{page}:s{pageSize}";

        IOperationResponse<IReadOnlyList<ICardXfrEntity>> response = await _cache
            .GetOrCreateAsync(
                cacheKey,
                async () =>
                {
                    // Cache miss - execute query
                    IOperationResponse<IReadOnlyList<CardExtEntity>> queryResponse =
                        await _inquisitor
                            .QueryCardsByArtistAsync(artistId, page, pageSize)
                            .ConfigureAwait(false);

                    // Map results to XfrEntity
                    return MapQueryResults(queryResponse);
                })
            .ConfigureAwait(false);

        return response;
    }
}
```

**Query Cache Keys:**
- Format: `query:{queryType}:{param1}:{param2}:...`
- Include all parameters that affect results
- Static data queries: Never invalidated (artist data doesn't change)
- User data queries: Include userId in key, invalidate with user partition

---

## Cache Key Patterns

### Static Entity Keys

Static data from Scryfall (cards, sets, artists) never changes:

```
Format:    "{entityType}:{id}"

Examples:
  card:550c74d4-1fcb-406a-b02a-639a760a4380
  set:lea
  set:m21
  artist:560
  artist:731

Invalidation: Never (except manual bulk reimport)
```

### User-Partitioned Entity Keys

User data is partitioned by userId:

```
Format:    "user:{userId}:{entityType}:{id}"

Examples:
  user:auth0|123456:card:550c74d4-1fcb-406a-b02a-639a760a4380
  user:auth0|123456:setcard:lea:550c74d4-1fcb-406a-b02a-639a760a4380
  user:auth0|789012:info:auth0|789012

Invalidation: On user write operations (partition-based)
```

### Query Result Keys

Query results include parameters in the key:

```
Format:    "query:{queryType}:{param1}:{param2}:..."

Examples:
  query:cardsByArtist:560:p1:s20
  query:setsByYear:1993
  query:userCollection:auth0|123456:includeSetInfo

Invalidation:
  - Static queries: Never (artist/set data doesn't change)
  - User queries: With user partition invalidation
```

---

## Invalidation Strategies

### 1. User Partition Invalidation (Aggressive)

**Scenario:** User updates their collection (add/remove cards)

**Strategy:**
```csharp
_cache.InvalidateUserPartition(userId);
```

**Effect:**
- Removes all cache entries with prefix `user:{userId}:`
- Ensures user sees changes immediately
- Simple and safe (can't miss an entry)
- Trades cache efficiency for consistency

**When to Use:**
- Default approach for all user write operations
- Simple to implement and reason about
- Acceptable for single-instance deployment

---

### 2. Selective Entity Invalidation (Precise)

**Scenario:** Update a specific entity, want to preserve other cached data

**Strategy:**
```csharp
_cache.Invalidate(entity.CacheKey);
```

**Effect:**
- Only removes the specific cache entry
- More efficient (preserves other cached data)
- Requires careful analysis of what else might be affected
- Risk of missing related cache entries (e.g., query results)

**When to Use:**
- When cache efficiency is critical
- For high-frequency updates to specific entities
- When you're confident about cache dependencies

---

### 3. Bulk Invalidation (Nuclear)

**Scenario:** Scryfall bulk data reimport or cache corruption

**Strategy:**
```csharp
_cache.Clear();
```

**Effect:**
- Clears entire cache
- All subsequent reads will repopulate cache
- Simple but heavy-handed

**When to Use:**
- Bulk data reimport from Scryfall
- Cache corruption suspected
- Major system changes
- Manual administrative action

---

### 4. No Time-Based Expiration

**Design Decision:** Cache entries never expire based on time

**Rationale:**
- Data only changes when we change it (via write operations)
- We control all writes through adapters
- Write-through invalidation ensures consistency
- Single-instance deployment (no multi-instance staleness concerns)
- Eventual consistency not needed when we invalidate explicitly

**Safety Net:**
- Could add very long TTL (24+ hours) as defensive measure
- Primarily relies on explicit invalidation
- Clear() operation available for emergency use

---

## Cache Service Implementation

### MemoryCacheService

Concrete implementation using Microsoft.Extensions.Caching.Memory:

```csharp
namespace Lib.Shared.Caching.Implementations;

internal sealed class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<MemoryCacheService> _logger;
    private static readonly MemoryCacheEntryOptions _cacheOptions = new()
    {
        // No expiration - cache forever
        AbsoluteExpiration = null,
        SlidingExpiration = null,
        Priority = CacheItemPriority.Normal
    };

    public MemoryCacheService(
        IMemoryCache cache,
        ILogger<MemoryCacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<IOperationResponse<TEntity>> GetOrCreateAsync<TEntity>(
        string cacheKey,
        Func<Task<IOperationResponse<TEntity>>> factory)
        where TEntity : class, ICacheableEntity
    {
        // Try to get from cache
        if (_cache.TryGetValue(cacheKey, out IOperationResponse<TEntity>? cached))
        {
            LogCacheHit(cacheKey);
            return cached!;
        }

        // Cache miss - execute factory
        LogCacheMiss(cacheKey);
        IOperationResponse<TEntity> result = await factory().ConfigureAwait(false);

        // Cache the result (both success and failure)
        _cache.Set(cacheKey, result, _cacheOptions);

        return result;
    }

    public void Invalidate(string cacheKey)
    {
        _cache.Remove(cacheKey);
        LogInvalidation(cacheKey);
    }

    public void InvalidateUserPartition(string userId)
    {
        // MemoryCache doesn't support prefix-based removal
        // Options:
        // 1. Track all keys in a concurrent dictionary
        // 2. Use IMemoryCache.Compact() with custom logic
        // 3. Store user keys in a set per user

        // For now: Track keys and remove matching ones
        // (Implementation details depend on key tracking strategy)

        LogPartitionInvalidation(userId);
    }

    public void Clear()
    {
        // MemoryCache doesn't have a Clear() method
        // Options:
        // 1. Dispose and recreate (requires singleton lifetime management)
        // 2. Compact with percentage = 1.0 (removes everything)
        // 3. Track all keys and remove individually

        if (_cache is MemoryCache memCache)
        {
            memCache.Compact(1.0); // Remove everything
        }

        LogCacheClear();
    }

    // LoggerMessage attributes for performance
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Cache hit: {CacheKey}")]
    private partial void LogCacheHit(string cacheKey);

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Cache miss: {CacheKey}")]
    private partial void LogCacheMiss(string cacheKey);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Cache invalidated: {CacheKey}")]
    private partial void LogInvalidation(string cacheKey);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "User partition invalidated: user:{UserId}:*")]
    private partial void LogPartitionInvalidation(string userId);

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Cache cleared (all entries)")]
    private partial void LogCacheClear();
}
```

**Implementation Notes:**
- Caches both success and failure responses
- No expiration (cache forever)
- Partition invalidation requires key tracking (see below)
- Clear() operation uses Compact or custom key tracking

### Key Tracking for Partition Invalidation

To support user partition invalidation, need to track which keys belong to which user:

```csharp
private readonly ConcurrentDictionary<string, HashSet<string>> _userKeyIndex = new();
private readonly object _indexLock = new();

public void InvalidateUserPartition(string userId)
{
    if (_userKeyIndex.TryGetValue(userId, out HashSet<string>? keys))
    {
        lock (_indexLock)
        {
            foreach (string key in keys)
            {
                _cache.Remove(key);
            }
            _userKeyIndex.TryRemove(userId, out _);
        }
    }

    LogPartitionInvalidation(userId);
}

// When caching, track user keys
private void TrackUserKey(string cacheKey)
{
    if (cacheKey.StartsWith("user:"))
    {
        string userId = ExtractUserId(cacheKey);
        lock (_indexLock)
        {
            if (_userKeyIndex.TryGetValue(userId, out HashSet<string>? keys) is false)
            {
                keys = new HashSet<string>();
                _userKeyIndex[userId] = keys;
            }
            keys.Add(cacheKey);
        }
    }
}
```

---

## Dependency Injection Setup

### Service Registration

In `Lib.Shared.Caching` or dedicated caching library:

```csharp
namespace Lib.Shared.Caching;

public static class CachingServiceRegistration
{
    public static IServiceCollection AddEntityCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register MemoryCache infrastructure
        services.AddMemoryCache(options =>
        {
            options.SizeLimit = configuration.GetValue<long?>("Caching:SizeLimit");
            options.CompactionPercentage =
                configuration.GetValue<double>("Caching:CompactionPercentage", 0.25);
        });

        // Register cache service
        services.AddSingleton<ICacheService, MemoryCacheService>();

        return services;
    }
}
```

### Adapter Layer Registration

In each adapter project (e.g., `Lib.Adapter.Cards`):

```csharp
namespace Lib.Adapter.Cards;

public static class CardAdapterServiceRegistration
{
    public static IServiceCollection AddCardAdapters(
        this IServiceCollection services)
    {
        // Query adapter with cache
        services.AddScoped<ICardQueryAdapter, CardQueryAdapter>();

        // Command adapter with cache (if it exists)
        // services.AddScoped<ICardCommandAdapter, CardCommandAdapter>();

        return services;
    }
}
```

**Note:** Adapters receive `ICacheService` via constructor injection, no special registration needed.

### Startup Integration

In `App.MtgDiscovery.GraphQL/Startup.cs`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Register caching infrastructure
    services.AddEntityCaching(Configuration);

    // Register Cosmos operators (Gophers, Scribes, Inquisitors)
    services.AddCosmosOperators(Configuration);

    // Register adapters (which depend on both cache and operators)
    services.AddCardAdapters();
    services.AddSetAdapters();
    services.AddArtistAdapters();
    services.AddUserAdapters();
    services.AddUserCardsAdapters();

    // ... rest of services
}
```

---

## Configuration

### appsettings.json

```json
{
  "Caching": {
    "Enabled": true,
    "SizeLimit": 10000,
    "CompactionPercentage": 0.25
  }
}
```

**Configuration Properties:**
- `Enabled`: Master switch for caching (feature flag)
- `SizeLimit`: Maximum number of cache entries
- `CompactionPercentage`: When size limit reached, percentage of entries to evict

### appsettings.Development.json

```json
{
  "Caching": {
    "Enabled": true,
    "SizeLimit": 1000
  }
}
```

**Note:** Much simpler configuration than original design - no policies, no TTLs, no per-entity settings.

---

## Testing Strategy

### Unit Tests

**Test Project:** `Lib.Shared.Caching.Tests`

**Key Test Classes:**

1. **MemoryCacheServiceTests.cs**
   - `GetOrCreateAsync_CacheMiss_ExecutesFactory`
   - `GetOrCreateAsync_CacheHit_ReturnsWithoutCallingFactory`
   - `Invalidate_ExistingKey_RemovesFromCache`
   - `InvalidateUserPartition_RemovesAllUserKeys`
   - `Clear_RemovesAllCacheEntries`

2. **CacheableEntityTests.cs**
   - `CardXfrEntity_CacheKey_HasCorrectFormat`
   - `UserCardXfrEntity_CacheKey_IncludesUserId`
   - `TwoCardsWithSameId_CacheKey_AreEqual`

### Integration Tests

**Test Adapters with Real Cache:**

```csharp
[TestMethod]
public async Task CardQueryAdapter_SecondCall_ReturnsCachedValue()
{
    // Arrange
    IMemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
    ICacheService cacheService = new MemoryCacheService(memoryCache, logger);
    FakeCardGopher fakeGopher = new FakeCardGopher();
    CardQueryAdapter adapter = new CardQueryAdapter(fakeGopher, cacheService);

    string cardId = "test-card-id";

    // Act - First call (cache miss)
    IOperationResponse<ICardXfrEntity> firstResult = await adapter
        .GetCardByIdAsync(cardId)
        .ConfigureAwait(false);

    // Act - Second call (cache hit)
    IOperationResponse<ICardXfrEntity> secondResult = await adapter
        .GetCardByIdAsync(cardId)
        .ConfigureAwait(false);

    // Assert
    fakeGopher.GetByIdAsyncCallCount.Should().Be(1); // Gopher called only once
    secondResult.Should().BeSameAs(firstResult); // Same instance returned
}
```

**Test Cache Invalidation:**

```csharp
[TestMethod]
public async Task UserCardsCommandAdapter_Upsert_InvalidatesUserPartition()
{
    // Arrange
    IMemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
    ICacheService cacheService = new MemoryCacheService(memoryCache, logger);

    // Pre-populate cache with user data
    string userId = "auth0|123";
    await PrePopulateUserCache(cacheService, userId);

    FakeUserCardsScribe fakeScribe = new FakeUserCardsScribe();
    UserCardsCommandAdapter adapter = new UserCardsCommandAdapter(fakeScribe, cacheService);

    IUserCardXfrEntity entity = new UserCardXfrEntity
    {
        UserId = userId,
        CardId = "card-123",
        Quantity = 5
    };

    // Act - Write triggers invalidation
    await adapter.UpsertUserCardAsync(entity).ConfigureAwait(false);

    // Assert - Cache should be empty for this user
    bool cacheHit = memoryCache.TryGetValue($"user:{userId}:card:any-card", out _);
    cacheHit.Should().BeFalse(); // User partition was invalidated
}
```

---

## Performance Targets

### Cache Hit Rates (Expected)

| Entity Type | Target Hit Rate | Rationale |
|-------------|----------------|-----------|
| Cards | 85-95% | Frequently browsed, never changes |
| Sets | 80-90% | High reuse, never changes |
| Artists | 75-85% | Moderate reuse, never changes |
| User Cards | 60-80% | Varies by user activity, invalidated on writes |
| Query Results (static) | 70-85% | Artist/set queries, never changes |
| Query Results (user) | 50-70% | Invalidated with user writes |

### RU Consumption Reduction (Expected)

| Scenario | Current RUs | Cached RUs | Savings |
|----------|------------|------------|---------|
| Card page view | 10 RU | 0 RU | 100% |
| Set page view | 50 RU | 0 RU | 100% |
| User collection load | 100 RU | 20 RU | 80% |
| Artist search | 75 RU | 0 RU | 100% |

**Overall Expected Savings:** 70-90% reduction in Cosmos RU consumption

### Response Time Improvement (Expected)

| Operation | Current | Cached | Improvement |
|-----------|---------|--------|-------------|
| Get card by ID | 50-100ms | 1-5ms | 95% faster |
| Get set by code | 40-80ms | 1-5ms | 94% faster |
| User card list | 150-300ms | 30-60ms | 75% faster |

---

## Comparison with Original Design

### What Changed

| Aspect | Original Design | New Design |
|--------|----------------|------------|
| **Location** | Cosmos layer (Gophers/Scribes) | Adapter layer |
| **Pattern** | Decorator | Collaborator |
| **Policies** | 3 policies (static, user, query) | 1 policy (cache forever) |
| **Cache Keys** | External generator | Self-describing entities |
| **Interfaces** | 7+ interfaces | 2 interfaces |
| **Expiration** | Time-based (5min, 1min) | Invalidation-based only |
| **Complexity** | High | Low |

### Why This Is Better

**Architectural Clarity:**
- Cache at the right boundary (adapter abstraction layer, not implementation)
- Decoupled from Cosmos-specific concepts
- Could swap Cosmos for different backend without changing cache

**Simplicity:**
- One caching policy for everything
- Two core operations: GetOrCreate, Invalidate
- No complex policy factory or configuration

**Self-Describing Entities:**
- Objects define their own behavior (cache key)
- Type system enforces consistency
- Impossible to have mismatched keys between query and command

**Maintainability:**
- Less code to write and maintain
- Fewer interfaces and abstractions
- Clear responsibilities (adapters coordinate, entities describe themselves)

**Performance:**
- No time-based expiration overhead
- Simple cache lookup (no policy evaluation)
- Aggressive caching (everything cached forever)

---

## Implementation Phases

### Phase 1: Core Infrastructure

**Deliverables:**
1. Create `Lib.Shared.Caching` project (or add to existing Lib.Shared)
2. Implement `ICacheableEntity` and `IXfrEntity` interfaces
3. Implement `ICacheService` interface
4. Implement `MemoryCacheService` with key tracking
5. Configuration support

**Testing:**
- Unit tests for MemoryCacheService
- Cache key generation tests
- Partition invalidation tests

**Success Criteria:**
- All cache operations work correctly
- Partition invalidation removes all user keys
- Clear() removes all entries

### Phase 2: Static Entity Caching (Cards, Sets, Artists)

**Deliverables:**
1. Add `ICacheableEntity` to existing XfrEntity interfaces
2. Update Card/Set/Artist query adapters to use cache
3. Define cache key formats in entity interfaces
4. Integration tests

**Testing:**
- Cache hit/miss scenarios
- Performance benchmarks (RU reduction)
- Concurrent access tests

**Success Criteria:**
- 80%+ cache hit rate for repeated reads
- 90%+ reduction in RU consumption
- No data corruption

### Phase 3: User Data Caching & Invalidation

**Deliverables:**
1. Add cache support to User/UserCards/UserSetCards adapters
2. Implement partition invalidation in command adapters
3. Test invalidation behavior
4. Monitor cache hit rates

**Testing:**
- Write operation invalidates cache
- User A's write doesn't affect User B
- Concurrent read/write scenarios

**Success Criteria:**
- User sees changes immediately after write
- No cross-user cache pollution
- 60%+ cache hit rate for user data

### Phase 4: Query Result Caching

**Deliverables:**
1. Add caching to Inquisitor operations
2. Define query cache key patterns
3. Coordinate query invalidation with entity writes
4. Performance tuning

**Testing:**
- Complex query caching
- Query invalidation scenarios
- Cache key collision prevention

**Success Criteria:**
- Reduced query latency
- Correct invalidation behavior
- No stale results

### Phase 5: Monitoring & Optimization

**Deliverables:**
1. Add cache metrics (hit/miss counts)
2. Logging for cache operations
3. Performance profiling
4. Documentation

**Testing:**
- Load testing
- Memory pressure testing
- Performance regression testing

**Success Criteria:**
- Metrics available for monitoring
- Performance targets met
- Production-ready

---

## Risk Assessment & Mitigations

### Risk 1: Memory Pressure

**Scenario:** Cache grows too large, causes OOM

**Likelihood:** Medium
**Impact:** High (application crash)

**Mitigation:**
- Configure `SizeLimit` in MemoryCache
- Set `CompactionPercentage` for automatic eviction
- Monitor cache size in production
- Emergency Clear() operation available

### Risk 2: Invalidation Bugs

**Scenario:** Stale data shown to users after writes

**Likelihood:** Low
**Impact:** High (user frustration)

**Mitigation:**
- Aggressive partition invalidation (whole user vs specific entity)
- Comprehensive integration tests
- Logging of all invalidation operations
- Emergency Clear() endpoint for support team

### Risk 3: Key Tracking Memory Overhead

**Scenario:** User key index grows unbounded

**Likelihood:** Low
**Impact:** Medium (memory leak)

**Mitigation:**
- Track only active users (evict inactive user indexes)
- Periodically cleanup stale user indexes
- Monitor index size
- Alternative: Prefix-based cache implementation

### Risk 4: Single Instance Limitation

**Scenario:** Need to scale to multiple instances

**Likelihood:** Low (stated requirement: single instance)
**Impact:** Medium (requires refactoring)

**Mitigation:**
- Abstracted behind `ICacheService` interface
- Can swap to Redis/distributed cache
- Same adapter code would work
- Document limitation clearly

---

## Future Enhancements

### 1. Distributed Cache (Redis)

**When:** Multi-instance scale-out required

**Implementation:**
- Create `RedisCacheService : ICacheService`
- Use Redis prefix patterns for partition invalidation
- Serialize/deserialize IOperationResponse
- Same adapter code, just swap registration

### 2. Cache Metrics & Monitoring

**When:** Production deployment

**Implementation:**
- `ICacheMetrics` interface
- Track hit/miss by entity type
- Export to Application Insights
- Dashboard for cache health

### 3. Selective Invalidation

**When:** Cache efficiency matters more

**Implementation:**
- Track query dependencies
- Invalidate only affected queries
- More complex but more efficient
- Document tradeoffs

### 4. Defensive TTL

**When:** Extra safety desired

**Implementation:**
- Add very long TTL (24 hours) as backup
- Primarily rely on explicit invalidation
- Safety net for edge cases
- Configurable per environment

---

## Deployment Checklist

### Pre-Deployment

- [ ] All tests passing
- [ ] Performance benchmarks meet targets
- [ ] Configuration validated
- [ ] Logging verified
- [ ] Runbook reviewed

### Deployment Steps

1. [ ] Deploy with caching **disabled** (`Caching:Enabled = false`)
2. [ ] Verify application health (baseline)
3. [ ] Enable caching (`Caching:Enabled = true`)
4. [ ] Monitor for 24 hours
5. [ ] Validate RU reduction and hit rates
6. [ ] Final approval

### Rollback Plan

- [ ] Set `Caching:Enabled = false`
- [ ] Restart application
- [ ] Verify return to baseline

---

## Conclusion

This simplified caching design provides:

**Advantages:**
- Single, simple caching policy (cache until invalidated)
- Self-describing entities (objects know their own cache keys)
- Clear architectural boundary (adapter layer, not implementation)
- Technology-agnostic (could swap Cosmos for anything)
- Less code, fewer interfaces, easier to maintain
- Aggressive caching (maximum performance)

**Trade-offs:**
- Single instance only (no distributed cache coordination)
- Aggressive invalidation (user partition, not selective)
- No time-based expiration (relies on explicit invalidation)

**Best For:**
- Single-instance deployments
- High read:write ratio
- Static or infrequently-changing data
- Clear write operation boundaries

**Next Steps:**
1. Review and approve design
2. Implement Phase 1 (core infrastructure)
3. Test with static entities first
4. Expand to user data and queries
5. Monitor and optimize

---

**Document Version:** 2.0
**Last Updated:** 2025-01-23
**Author:** Design Review - Simplified Approach
**Status:** Ready for Implementation
