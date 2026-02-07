# CollectionQueryAdapter Refactoring Plan

**Created**: 2026-02-06
**Status**: Pending Review
**Scope**: `Lib.Adapter.Collections/Queries/CollectionQueryAdapter.cs`

---

## Executive Summary

`CollectionQueryAdapter.cs` violates multiple documented patterns. This document outlines the complete refactoring plan to bring it into compliance with the Inquisition pattern and single-responsibility principle.

---

## Part 1: Current Violations

### 1.1 Violation Summary

| Line | Violation | Pattern Expected |
|------|-----------|------------------|
| 21-166 | 5 operations in one class | Single responsibility - one adapter per behavior |
| 41-42, 66-67, 99-100, 125-127 | Inline `QueryDefinition` SQL | `InquiryDefinition` classes |
| 44-46, 69-71 | Direct `ICosmosInquisitor.QueryAsync()` | `ICosmosInquisition<TArgs>.QueryAsync()` |
| 84-88 | Inline `ReadPointItem` creation | `ICollectionIdXfrToReadPointMapper` |
| 103-104, 129-131 | Direct `CrossPartitionQueryAsync()` | Cross-partition `ICosmosInquisition` |
| 145 | Inline `new OwnerIdXfrEntity{}` | Mapper |
| 114 | Business logic (visibility check) | Domain/Aggregator layer |

### 1.2 Current File Structure

```
Lib.Adapter.Collections/
├── Apis/
│   ├── ICollectionAdapterService.cs
│   ├── CollectionAdapterService.cs
│   ├── ICollectionQueryAdapter.cs      ← 5 methods in interface
│   └── Entities/
├── Queries/
│   ├── CollectionQueryAdapter.cs       ← 166 lines, 5 operations
│   └── Entities/
└── Commands/
    └── ...
```

### 1.3 Problematic Code Examples

**Inline QueryDefinition (lines 41-42):**
```csharp
QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.owner_id = @ownerId AND c.is_default = true")
    .WithParameter("@ownerId", args.OwnerId);
```

**Inline ReadPointItem (lines 84-88):**
```csharp
ReadPointItem readItem = new()
{
    Id = new ProvidedCosmosItemId(args.CollectionId),
    Partition = new ProvidedPartitionKeyValue(args.OwnerId)
};
```

**Business logic in adapter (line 114):**
```csharp
if (collection.Visibility == "public")
{
    return new SuccessOperationResponse<CollectionExtEntity>(collection);
}
```

---

## Part 2: Infrastructure Layer Changes

**Location:** `Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitions/`

### 2.1 Query Definitions (3 files)

#### CollectionsByOwnerQueryDefinition.cs

> **Note:** Kept for partition-scoped queries. Used by `DefaultCollectionAdapter` which only needs
> the user's owned collections.

```csharp
using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;

public sealed class CollectionsByOwnerQueryDefinition : InquiryDefinition
{
    public override QueryDefinition AsSystemType() =>
        new("SELECT * FROM c WHERE c.owner_id = @ownerId");
}
```

#### CollectionByIdQueryDefinition.cs

```csharp
using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;

public sealed class CollectionByIdQueryDefinition : InquiryDefinition
{
    public override QueryDefinition AsSystemType() =>
        new("SELECT * FROM c WHERE c.id = @collectionId");
}
```

#### AccessibleCollectionsQueryDefinition.cs

> **Replaces:** `SharedCollectionsByUserQueryDefinition` — combines owned + shared in a single query.
>
> **Design Decision:** A single cross-partition query is more efficient than calling a partition-scoped
> query + cross-partition query separately when both are always needed together. Index on
> `/authorized_users/[]/user_id/?` optimizes the EXISTS clause.

```csharp
using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;

public sealed class AccessibleCollectionsQueryDefinition : InquiryDefinition
{
    public override QueryDefinition AsSystemType() =>
        new("SELECT * FROM c WHERE c.owner_id = @userId OR EXISTS (SELECT VALUE au FROM au IN c.authorized_users WHERE au.user_id = @userId)");
}
```

### 2.2 Inquisition Args Entities (2 files)

**Location:** `Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitions/Entities/`

> **Note:** `OwnerIdExtEntitys` is not needed — `CollectionsByOwnerInquisition` uses partition key
> directly, and `AccessibleCollectionsInquisition` uses `UserIdExtEntitys` for both owned and shared.

#### UserIdExtEntitys.cs

```csharp
namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;

public sealed class UserIdExtEntitys
{
    public string UserId { get; init; }
}
```

#### CollectionIdExtEntitys.cs

```csharp
namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;

public sealed class CollectionIdExtEntitys
{
    public string CollectionId { get; init; }
}
```

### 2.3 Inquisitions (3 files)

#### CollectionsByOwnerInquisition.cs (partitioned)

> **Note:** Partition-scoped query used by `DefaultCollectionAdapter`. Uses `UserIdExtEntitys` for consistency.

```csharp
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;

public sealed class CollectionsByOwnerInquisition : ICosmosInquisition<UserIdExtEntitys>
{
    private readonly ICosmosInquisitor _inquisitor;
    private readonly InquiryDefinition _inquiry;

    public CollectionsByOwnerInquisition(ILogger logger)
        : this(new CollectionsInquisitor(logger), new CollectionsByOwnerQueryDefinition())
    { }

    private CollectionsByOwnerInquisition(ICosmosInquisitor inquisitor, InquiryDefinition inquiry)
    {
        _inquisitor = inquisitor;
        _inquiry = inquiry;
    }

    public async Task<OpResponse<IEnumerable<T>>> QueryAsync<T>(
        [NotNull] UserIdExtEntitys args,
        CancellationToken cancellationToken = default)
    {
        QueryDefinition query = _inquiry.AsSystemType()
            .WithParameter("@ownerId", args.UserId);

        PartitionKey partitionKey = new(args.UserId);

        return await _inquisitor.QueryAsync<T>(query, partitionKey, cancellationToken)
            .ConfigureAwait(false);
    }
}
```

#### CollectionByIdInquisition.cs (cross-partition)

```csharp
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;

public sealed class CollectionByIdInquisition : ICosmosInquisition<CollectionIdExtEntitys>
{
    private readonly ICosmosInquisitor _inquisitor;
    private readonly InquiryDefinition _inquiry;

    public CollectionByIdInquisition(ILogger logger)
        : this(new CollectionsInquisitor(logger), new CollectionByIdQueryDefinition())
    { }

    private CollectionByIdInquisition(ICosmosInquisitor inquisitor, InquiryDefinition inquiry)
    {
        _inquisitor = inquisitor;
        _inquiry = inquiry;
    }

    public async Task<OpResponse<IEnumerable<T>>> QueryAsync<T>(
        [NotNull] CollectionIdExtEntitys args,
        CancellationToken cancellationToken = default)
    {
        QueryDefinition query = _inquiry.AsSystemType()
            .WithParameter("@collectionId", args.CollectionId);

        return await _inquisitor.CrossPartitionQueryAsync<T>(query, cancellationToken)
            .ConfigureAwait(false);
    }
}
```

#### AccessibleCollectionsInquisition.cs (cross-partition)

> **Replaces:** `SharedCollectionsByUserInquisition`
>
> **Design Decision:** Single cross-partition query returning both owned and shared collections.
> More efficient than running partition-scoped + cross-partition queries separately when both
> results are always combined.

```csharp
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;

public sealed class AccessibleCollectionsInquisition : ICosmosInquisition<UserIdExtEntitys>
{
    private readonly ICosmosInquisitor _inquisitor;
    private readonly InquiryDefinition _inquiry;

    public AccessibleCollectionsInquisition(ILogger logger)
        : this(new CollectionsInquisitor(logger), new AccessibleCollectionsQueryDefinition())
    { }

    private AccessibleCollectionsInquisition(ICosmosInquisitor inquisitor, InquiryDefinition inquiry)
    {
        _inquisitor = inquisitor;
        _inquiry = inquiry;
    }

    public async Task<OpResponse<IEnumerable<T>>> QueryAsync<T>(
        [NotNull] UserIdExtEntitys args,
        CancellationToken cancellationToken = default)
    {
        QueryDefinition query = _inquiry.AsSystemType()
            .WithParameter("@userId", args.UserId);

        return await _inquisitor.CrossPartitionQueryAsync<T>(query, cancellationToken)
            .ConfigureAwait(false);
    }
}
```

---

## Part 3: Adapter Layer Changes

**Location:** `Lib.Adapter.Collections/Queries/`

### 3.1 Target File Structure

> **Simplified:** Removed `SharedCollectionsAdapter` (replaced by `AccessibleCollectionsInquisition`)
> and `OwnerIdXfrToArgsMapper` (all queries now use `UserIdExtEntitys`).

```
Lib.Adapter.Collections/
├── Apis/
│   ├── ICollectionAdapterService.cs    ← Update to wire new adapters
│   ├── CollectionAdapterService.cs     ← Update to wire new adapters
│   ├── ICollectionQueryAdapter.cs      ← Update: delegate to specialized interfaces
│   └── Entities/
├── Queries/
│   ├── IDefaultCollectionAdapter.cs
│   ├── DefaultCollectionAdapter.cs
│   ├── ICollectionsByOwnerAdapter.cs
│   ├── CollectionsByOwnerAdapter.cs
│   ├── ICollectionByIdAdapter.cs
│   ├── CollectionByIdAdapter.cs
│   ├── IAccessibleCollectionsAdapter.cs
│   ├── AccessibleCollectionsAdapter.cs
│   ├── Entities/
│   └── Mappers/
│       ├── IUserIdXfrToArgsMapper.cs
│       ├── UserIdXfrToArgsMapper.cs
│       ├── ICollectionIdXfrToReadPointMapper.cs
│       └── CollectionIdXfrToReadPointMapper.cs
└── Commands/
    └── ...
```

### 3.2 Mappers (4 files)

> **Simplified:** `OwnerIdXfrToArgsMapper` removed — all inquisitions use `UserIdExtEntitys`.

#### IUserIdXfrToArgsMapper.cs

```csharp
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace Lib.Adapter.Collections.Queries.Mappers;

internal interface IUserIdXfrToArgsMapper
    : ICreateMapper<IUserIdXfrEntity, UserIdExtEntitys>;
```

#### UserIdXfrToArgsMapper.cs

```csharp
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;

namespace Lib.Adapter.Collections.Queries.Mappers;

internal sealed class UserIdXfrToArgsMapper : IUserIdXfrToArgsMapper
{
    public Task<UserIdExtEntitys> Map(IUserIdXfrEntity source)
    {
        UserIdExtEntitys args = new() { UserId = source.UserId };
        return Task.FromResult(args);
    }
}
```

#### ICollectionIdXfrToReadPointMapper.cs

```csharp
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace Lib.Adapter.Collections.Queries.Mappers;

internal interface ICollectionIdXfrToReadPointMapper
    : ICreateMapper<ICollectionIdXfrEntity, ReadPointItem>;
```

#### CollectionIdXfrToReadPointMapper.cs

```csharp
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.Collections.Queries.Mappers;

internal sealed class CollectionIdXfrToReadPointMapper : ICollectionIdXfrToReadPointMapper
{
    public Task<ReadPointItem> Map(ICollectionIdXfrEntity source)
    {
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(source.CollectionId),
            Partition = new ProvidedPartitionKeyValue(source.OwnerId)
        };
        return Task.FromResult(readPoint);
    }
}
```

### 3.3 Specialized Adapters (8 files)

> **Simplified:** `SharedCollectionsAdapter` removed — `AccessibleCollectionsAdapter` now uses
> `AccessibleCollectionsInquisition` directly instead of composing two adapters.

#### IDefaultCollectionAdapter.cs

```csharp
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.Collections.Queries;

internal interface IDefaultCollectionAdapter
    : IOperationResponseService<IUserIdXfrEntity, CollectionExtEntity>;
```

#### DefaultCollectionAdapter.cs

> **Note:** This adapter composes `CollectionsByOwnerAdapter` and filters in memory for the default collection.
> A user typically has few collections (10-50), so filtering in memory is negligible and avoids
> the overhead of a separate Inquisition and index.

```csharp
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Collections.Exceptions;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Collections.Queries;

internal sealed class DefaultCollectionAdapter : IDefaultCollectionAdapter
{
    private readonly ICollectionsByOwnerAdapter _collectionsAdapter;

    public DefaultCollectionAdapter(ILogger logger)
        : this(new CollectionsByOwnerAdapter(logger))
    { }

    private DefaultCollectionAdapter(ICollectionsByOwnerAdapter collectionsAdapter)
        => _collectionsAdapter = collectionsAdapter;

    public async Task<IOperationResponse<CollectionExtEntity>> Execute(
        IUserIdXfrEntity input,
        CancellationToken cancellationToken)
    {
        IOperationResponse<IEnumerable<CollectionExtEntity>> response = await _collectionsAdapter
            .Execute(input, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<CollectionExtEntity>(response.OuterException);
        }

        CollectionExtEntity defaultCollection = response.ResponseData.FirstOrDefault(c => c.IsDefault);

        if (defaultCollection is null)
        {
            return new FailureOperationResponse<CollectionExtEntity>(
                new CollectionAdapterException($"No default collection found for user {input.UserId}"));
        }

        return new SuccessOperationResponse<CollectionExtEntity>(defaultCollection);
    }
}
```

#### ICollectionsByOwnerAdapter.cs

> **Note:** Partition-scoped query for owner's collections only. Used by `DefaultCollectionAdapter`.

```csharp
using System.Collections.Generic;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.Collections.Queries;

internal interface ICollectionsByOwnerAdapter
    : IOperationResponseService<IUserIdXfrEntity, IEnumerable<CollectionExtEntity>>;
```

#### CollectionsByOwnerAdapter.cs

```csharp
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Collections.Exceptions;
using Lib.Adapter.Collections.Queries.Mappers;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Collections.Queries;

internal sealed class CollectionsByOwnerAdapter : ICollectionsByOwnerAdapter
{
    private readonly ICosmosInquisition<UserIdExtEntitys> _inquisition;
    private readonly IUserIdXfrToArgsMapper _mapper;

    public CollectionsByOwnerAdapter(ILogger logger)
        : this(new CollectionsByOwnerInquisition(logger), new UserIdXfrToArgsMapper())
    { }

    private CollectionsByOwnerAdapter(
        ICosmosInquisition<UserIdExtEntitys> inquisition,
        IUserIdXfrToArgsMapper mapper)
    {
        _inquisition = inquisition;
        _mapper = mapper;
    }

    public async Task<IOperationResponse<IEnumerable<CollectionExtEntity>>> Execute(
        IUserIdXfrEntity input,
        CancellationToken cancellationToken)
    {
        UserIdExtEntitys args = await _mapper.Map(input).ConfigureAwait(false);

        OpResponse<IEnumerable<CollectionExtEntity>> response = await _inquisition
            .QueryAsync<CollectionExtEntity>(args, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<CollectionExtEntity>>(
                new CollectionAdapterException($"Failed to query collections for user {input.UserId}"));
        }

        return new SuccessOperationResponse<IEnumerable<CollectionExtEntity>>(response.Value ?? []);
    }
}
```

#### ICollectionByIdAdapter.cs

```csharp
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.Collections.Queries;

internal interface ICollectionByIdAdapter
    : IOperationResponseService<ICollectionIdXfrEntity, CollectionExtEntity>;
```

#### CollectionByIdAdapter.cs

```csharp
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Collections.Queries.Mappers;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Exceptions;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Collections.Queries;

/// <summary>
/// Retrieves a collection by ID. Attempts point-read first (if owner known),
/// falls back to cross-partition query for public/shared access.
/// Note: Authorization (visibility check) should be handled by Domain/Aggregator layer.
/// </summary>
internal sealed class CollectionByIdAdapter : ICollectionByIdAdapter
{
    private readonly ICosmosGopher _gopher;
    private readonly ICosmosInquisition<CollectionIdExtEntitys> _inquisition;
    private readonly ICollectionIdXfrToReadPointMapper _readPointMapper;

    public CollectionByIdAdapter(ILogger logger)
        : this(
            new CollectionGopher(logger),
            new CollectionByIdInquisition(logger),
            new CollectionIdXfrToReadPointMapper())
    { }

    private CollectionByIdAdapter(
        ICosmosGopher gopher,
        ICosmosInquisition<CollectionIdExtEntitys> inquisition,
        ICollectionIdXfrToReadPointMapper readPointMapper)
    {
        _gopher = gopher;
        _inquisition = inquisition;
        _readPointMapper = readPointMapper;
    }

    public async Task<IOperationResponse<CollectionExtEntity>> Execute(
        ICollectionIdXfrEntity input,
        CancellationToken cancellationToken)
    {
        // Attempt point-read if owner is known
        if (string.IsNullOrEmpty(input.OwnerId) is false)
        {
            ReadPointItem readPoint = await _readPointMapper.Map(input).ConfigureAwait(false);

            OpResponse<CollectionExtEntity> pointReadResponse = await _gopher
                .ReadAsync<CollectionExtEntity>(readPoint, cancellationToken)
                .ConfigureAwait(false);

            if (pointReadResponse.IsSuccessful() && pointReadResponse.Value is not null)
            {
                return new SuccessOperationResponse<CollectionExtEntity>(pointReadResponse.Value);
            }
        }

        // Fall back to cross-partition query
        CollectionIdExtEntitys args = new() { CollectionId = input.CollectionId };

        OpResponse<IEnumerable<CollectionExtEntity>> queryResponse = await _inquisition
            .QueryAsync<CollectionExtEntity>(args, cancellationToken)
            .ConfigureAwait(false);

        if (queryResponse.IsNotSuccessful() || queryResponse.Value?.Any() is false)
        {
            return new FailureOperationResponse<CollectionExtEntity>(
                new NotFoundOperationException($"Collection not found: {input.CollectionId}"));
        }

        return new SuccessOperationResponse<CollectionExtEntity>(queryResponse.Value!.First());
    }
}
```

#### IAccessibleCollectionsAdapter.cs

> **Note:** Returns all collections a user can access (owned + shared) via single cross-partition query.

```csharp
using System.Collections.Generic;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.Collections.Queries;

internal interface IAccessibleCollectionsAdapter
    : IOperationResponseService<IUserIdXfrEntity, IEnumerable<CollectionExtEntity>>;
```

#### AccessibleCollectionsAdapter.cs

> **Design Decision:** Uses `AccessibleCollectionsInquisition` directly instead of composing
> `CollectionsByOwnerAdapter` + `SharedCollectionsAdapter`. A single cross-partition query is
> more efficient than running partition-scoped + cross-partition queries when results are always
> combined. Index on `/authorized_users/[]/user_id/?` optimizes the EXISTS clause.

```csharp
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Collections.Exceptions;
using Lib.Adapter.Collections.Queries.Mappers;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Collections.Queries;

internal sealed class AccessibleCollectionsAdapter : IAccessibleCollectionsAdapter
{
    private readonly ICosmosInquisition<UserIdExtEntitys> _inquisition;
    private readonly IUserIdXfrToArgsMapper _mapper;

    public AccessibleCollectionsAdapter(ILogger logger)
        : this(new AccessibleCollectionsInquisition(logger), new UserIdXfrToArgsMapper())
    { }

    private AccessibleCollectionsAdapter(
        ICosmosInquisition<UserIdExtEntitys> inquisition,
        IUserIdXfrToArgsMapper mapper)
    {
        _inquisition = inquisition;
        _mapper = mapper;
    }

    public async Task<IOperationResponse<IEnumerable<CollectionExtEntity>>> Execute(
        IUserIdXfrEntity input,
        CancellationToken cancellationToken)
    {
        UserIdExtEntitys args = await _mapper.Map(input).ConfigureAwait(false);

        OpResponse<IEnumerable<CollectionExtEntity>> response = await _inquisition
            .QueryAsync<CollectionExtEntity>(args, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<CollectionExtEntity>>(
                new CollectionAdapterException($"Failed to query accessible collections for user {input.UserId}"));
        }

        return new SuccessOperationResponse<IEnumerable<CollectionExtEntity>>(response.Value ?? []);
    }
}
```

---

## Part 4: Index Policies

### 4.1 Query Index Requirements

| Query | SQL | Index Needed |
|-------|-----|--------------|
| `CollectionsByOwner` | `WHERE c.owner_id = @ownerId` | Partition key only — efficient partition-scoped |
| `DefaultCollection` | Filters `CollectionsByOwner` result in memory | No index needed |
| `CollectionById` (cross-partition) | `WHERE c.id = @collectionId` | `id` always indexed — no action |
| `AccessibleCollections` (cross-partition) | `WHERE c.owner_id = @userId OR EXISTS (... au.user_id = @userId)` | `/authorized_users/[]/user_id/?` |

### 4.2 AccessibleCollections Query Optimization

**Combined Query:**
```sql
SELECT * FROM c
WHERE c.owner_id = @userId
   OR EXISTS (SELECT VALUE au FROM au IN c.authorized_users WHERE au.user_id = @userId)
```

**Query Execution Analysis:**

| Clause | Behavior | Index Usage |
|--------|----------|-------------|
| `c.owner_id = @userId` | Matches partition key | Uses partition index (free) |
| `EXISTS (... au.user_id = @userId)` | Array element scan | Requires `/authorized_users/[]/user_id/?` index |

**Why Single Query is Better:**

| Approach | RU Cost | Network Trips | Complexity |
|----------|---------|---------------|------------|
| Partition + Cross-partition (2 queries) | ~2 RU + ~5 RU = ~7 RU | 2 | High (combine results) |
| Single Cross-partition (combined) | ~5-6 RU | 1 | Low (single result set) |

The OR clause allows Cosmos to optimize by checking partition key first, then falling back to
the indexed array scan only when needed.

### 4.3 New Index Policy File

**File:** `database/.cosmosConfig/cosmos-index-policies/Collections-index-policy.json`

```json
{
  "indexingMode": "consistent",
  "automatic": true,
  "includedPaths": [
    {
      "path": "/authorized_users/[]/user_id/?"
    },
    {
      "path": "/visibility/?"
    }
  ],
  "excludedPaths": [
    {
      "path": "/*"
    },
    {
      "path": "/\"_etag\"/?"
    }
  ]
}
```

### 4.4 Index Rationale

| Path | Purpose |
|------|---------|
| `/authorized_users/[]/user_id/?` | Optimizes EXISTS clause in `AccessibleCollections` cross-partition query |
| `/visibility/?` | Future: filter public collections in cross-partition queries |

### 4.5 Index Deployment Notes

1. **Create index before deploying code** — New index builds in background
2. **Monitor indexing progress** — Check container settings in Azure Portal
3. **Expected build time** — Minutes for small containers, hours for large ones
4. **No downtime** — Index builds while queries continue (may be slower during build)

---

## Part 5: Business Logic Relocation

### 5.1 Current Problem

Line 114 of `CollectionQueryAdapter.cs`:
```csharp
if (collection.Visibility == "public")
{
    return new SuccessOperationResponse<CollectionExtEntity>(collection);
}

return new FailureOperationResponse<CollectionExtEntity>(
    new ForbiddenOperationException("Access denied to private collection"));
```

### 5.2 Resolution

This authorization logic should move to the **Domain** or **Aggregator** layer. The adapter's responsibility is data retrieval only.

**Options:**
1. Move to `Lib.Domain.Collections` - create authorization validator
2. Move to `Lib.Aggregator.Collections` - check before returning to Entry layer
3. Optimize query - add visibility filter to cross-partition query (requires `/visibility/?` index)

**Recommended:** Option 3 for efficiency, with Option 1 as fallback for complex authorization rules.

---

## Part 6: Implementation Checklist

### Infrastructure Layer (`Lib.Adapter.Scryfall.Cosmos`)

- [ ] Create `CollectionsByOwnerQueryDefinition.cs`
- [ ] Create `CollectionByIdQueryDefinition.cs`
- [ ] Create `AccessibleCollectionsQueryDefinition.cs`
- [ ] Create `UserIdExtEntitys.cs`
- [ ] Create `CollectionIdExtEntitys.cs`
- [ ] Create `CollectionsByOwnerInquisition.cs`
- [ ] Create `CollectionByIdInquisition.cs`
- [ ] Create `AccessibleCollectionsInquisition.cs`

### Adapter Layer (`Lib.Adapter.Collections`)

- [ ] Create `IUserIdXfrToArgsMapper.cs`
- [ ] Create `UserIdXfrToArgsMapper.cs`
- [ ] Create `ICollectionIdXfrToReadPointMapper.cs`
- [ ] Create `CollectionIdXfrToReadPointMapper.cs`
- [ ] Create `IDefaultCollectionAdapter.cs`
- [ ] Create `DefaultCollectionAdapter.cs`
- [ ] Create `ICollectionsByOwnerAdapter.cs`
- [ ] Create `CollectionsByOwnerAdapter.cs`
- [ ] Create `ICollectionByIdAdapter.cs`
- [ ] Create `CollectionByIdAdapter.cs`
- [ ] Create `IAccessibleCollectionsAdapter.cs`
- [ ] Create `AccessibleCollectionsAdapter.cs`
- [ ] Update `ICollectionQueryAdapter.cs` to delegate
- [ ] Update `CollectionAdapterService.cs` to wire new adapters
- [ ] Delete `CollectionQueryAdapter.cs` (after migration complete)

### Database

- [ ] Create `Collections-index-policy.json`
- [ ] Apply index policy to Collections container (before code deployment)

### Tests

- [ ] Create tests for each new Inquisition (3)
- [ ] Create tests for each new Adapter (4)
- [ ] Create tests for each new Mapper (2)
- [ ] Update existing integration tests

---

## Part 7: Summary

| Layer | New Files | Purpose |
|-------|-----------|---------|
| `database/.cosmosConfig/cosmos-index-policies` | 1 JSON file | Collections index policy |
| `Lib.Adapter.Scryfall.Cosmos` | 3 QueryDefinitions | SQL encapsulation |
| `Lib.Adapter.Scryfall.Cosmos` | 3 Inquisitions | Typed query execution |
| `Lib.Adapter.Scryfall.Cosmos` | 2 ExtEntitys | Inquisition arguments |
| `Lib.Adapter.Collections` | 8 Adapter files | 4 interface + 4 implementation |
| `Lib.Adapter.Collections` | 4 Mapper files | 2 interface + 2 implementation |
| `Lib.Adapter.Collections/Apis` | Update composite interface | Wire new adapters |

**Total: ~21 new files**, replacing 1 monolithic 166-line file.

### Design Decisions

#### DefaultCollectionAdapter (Partition-Scoped)

`DefaultCollectionAdapter` composes `CollectionsByOwnerAdapter` rather than having its own Inquisition:
- A user typically has few collections (10-50)
- Filtering in memory for `IsDefault` is negligible overhead
- Eliminates need for `/is_default/?` index
- Reduces infrastructure complexity

#### AccessibleCollectionsAdapter (Single Cross-Partition Query)

`AccessibleCollectionsAdapter` uses `AccessibleCollectionsInquisition` directly rather than composing `CollectionsByOwnerAdapter` + `SharedCollectionsAdapter`:
- Single cross-partition query is more efficient than partition-scoped + cross-partition when results are always combined
- Reduces network round trips (1 vs 2)
- Eliminates client-side result merging complexity
- Index on `/authorized_users/[]/user_id/?` optimizes the EXISTS clause
- OR clause allows Cosmos to check partition key match first, optimizing for owned collections
