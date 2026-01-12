# Sealed Products Implementation Tasks

This document contains granular, independently implementable tasks for the sealed products feature.

## Prerequisites

**Already Complete:**
- [x] Image Scraper CLI (`Cli.Sealed.ImageScraper`)
- [x] Data Ingestion CLI (`Cli.Sealed.Ingestion`)
- [x] `SealedProductExtEntity` in Cosmos
- [x] `SealedProductsScribe` (write operator)
- [x] `SealedProductsCosmosContainer` and definition

**Reference Files:**
| Pattern | Reference File |
|---------|---------------|
| Inquisition | `Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitions/CardsBySetIdInquisition.cs` |
| Adapter | `Lib.Adapter.Cards/Queries/CardsBySetCodeAdapter.cs` |
| Aggregator | `Lib.Aggregator.Cards/Apis/Queries/CardsBySetCodeAggregator.cs` |
| Domain | `Lib.Domain.Cards/Apis/Queries/CardsBySetCodeDomain.cs` |
| Entry Service | `Lib.MtgDiscovery.Entry/Queries/Cards/CardsBySetCodeEntryService.cs` |
| GraphQL Query | `App.MtgDiscovery.GraphQL/Queries/SetQueryMethods.cs` |

---

## Phase 1: Cosmos Read Infrastructure

### Task 1.1: Create SealedProductsBySetId Query Args
**Status:** [x] Complete
**Depends on:** Nothing
**Location:** `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitions/Args/`

**Files to Create:**
```
SealedProductsBySetIdArgs.cs
```

**SealedProductsBySetIdArgs.cs:**
```csharp
namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;

internal sealed class SealedProductsBySetIdArgs : InquisitionArgs
{
    public string SetId { get; init; }
}
```

**Acceptance Criteria:**
- [ ] File compiles without errors
- [ ] Follows existing `Args` pattern (see `CardsBySetIdArgs.cs`)

---

### Task 1.2: Create SealedProductsBySetId Query Definition
**Status:** [x] Complete
**Depends on:** Nothing
**Location:** `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitions/`

**Files to Create:**
```
SealedProductsBySetIdQueryDefinition.cs
```

**SealedProductsBySetIdQueryDefinition.cs:**
```csharp
using Microsoft.Azure.Cosmos;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;

internal sealed class SealedProductsBySetIdQueryDefinition : InquiryDefinition
{
    public override QueryDefinition AsSystemType() =>
        new("SELECT * FROM c WHERE c.partition = @setId");
}
```

**Acceptance Criteria:**
- [ ] File compiles without errors
- [ ] Query selects by partition (setId)

---

### Task 1.3: Create SealedProductsInquisitor
**Status:** [x] Complete
**Depends on:** Nothing
**Location:** `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitors/`

**Files to Create:**
```
SealedProductsInquisitor.cs
```

**SealedProductsInquisitor.cs:**
```csharp
using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis.Operators.Inquisitors;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;

internal sealed class SealedProductsInquisitor : CosmosInquisitor
{
    public SealedProductsInquisitor(SealedProductsCosmosContainer container)
        : base(container) { }
}
```

**Acceptance Criteria:**
- [ ] File compiles without errors
- [ ] Uses `SealedProductsCosmosContainer`

---

### Task 1.4: Create SealedProductsBySetIdInquisition
**Status:** [x] Complete
**Depends on:** Task 1.1, 1.2, 1.3
**Location:** `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitions/`

**Files to Create:**
```
SealedProductsBySetIdInquisition.cs
```

**SealedProductsBySetIdInquisition.cs:**
```csharp
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;
using Lib.Cosmos.Apis.Operators.Inquisitions;
using Microsoft.Azure.Cosmos;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;

internal sealed class SealedProductsBySetIdInquisition
    : CosmosInquisition<SealedProductsBySetIdArgs>
{
    public SealedProductsBySetIdInquisition(
        SealedProductsInquisitor inquisitor,
        SealedProductsBySetIdQueryDefinition queryDefinition)
        : base(inquisitor, queryDefinition) { }

    protected override QueryDefinition ApplyParameters(
        QueryDefinition query,
        SealedProductsBySetIdArgs args) =>
        query.WithParameter("@setId", args.SetId);
}
```

**Acceptance Criteria:**
- [ ] File compiles without errors
- [ ] Binds `@setId` parameter correctly

---

### Task 1.5: Create SealedProductsGopher
**Status:** [x] Complete
**Depends on:** Nothing
**Location:** `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Gophers/`

**Files to Create:**
```
SealedProductsGopher.cs
```

**SealedProductsGopher.cs:**
```csharp
using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis.Operators.Gophers;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;

internal sealed class SealedProductsGopher : CosmosGopher
{
    public SealedProductsGopher(SealedProductsCosmosContainer container)
        : base(container) { }
}
```

**Acceptance Criteria:**
- [ ] File compiles without errors
- [ ] Uses `SealedProductsCosmosContainer`

---

## Phase 2: Shared Data Models

### Task 2.1: Create SealedProductsBySetCode Arg Entity
**Status:** [x] Complete
**Depends on:** Nothing
**Location:** `src/Lib.Shared.DataModels/Entities/Args/SealedProducts/`

**Files to Create:**
```
ISealedProductsBySetCodeArgEntity.cs
```

**ISealedProductsBySetCodeArgEntity.cs:**
```csharp
namespace Lib.Shared.DataModels.Entities.Args.SealedProducts;

public interface ISealedProductsBySetCodeArgEntity : IArgEntity
{
    string SetCode { get; }
}
```

**Acceptance Criteria:**
- [ ] File compiles without errors
- [ ] Extends `IArgEntity`

---

### Task 2.2: Create SealedProduct Itr Entity
**Status:** [x] Complete
**Depends on:** Nothing
**Location:** `src/Lib.Shared.DataModels/Entities/Itrs/SealedProducts/`

**Files to Create:**
```
ISealedProductItrEntity.cs
```

**ISealedProductItrEntity.cs:**
```csharp
namespace Lib.Shared.DataModels.Entities.Itrs.SealedProducts;

public interface ISealedProductItrEntity : IItrEntity
{
    string Uuid { get; }
    string SetId { get; }
    string SetCode { get; }
    string SetName { get; }
    string Name { get; }
    string Category { get; }
    string Subtype { get; }
    int? CardCount { get; }
    string ReleaseDate { get; }
    string TcgplayerProductId { get; }
    string ImageUrl { get; }
    string PurchaseUrlTcgplayer { get; }
    string PurchaseUrlCardmarket { get; }
    string PurchaseUrlCardKingdom { get; }
}
```

**Acceptance Criteria:**
- [ ] File compiles without errors
- [ ] Contains all sealed product properties

---

### Task 2.3: Create SealedProductsBySetCode Itr Entity
**Status:** [x] Complete
**Depends on:** Nothing
**Location:** `src/Lib.Shared.DataModels/Entities/Itrs/SealedProducts/`

**Files to Create:**
```
ISealedProductsBySetCodeItrEntity.cs
```

**ISealedProductsBySetCodeItrEntity.cs:**
```csharp
namespace Lib.Shared.DataModels.Entities.Itrs.SealedProducts;

public interface ISealedProductsBySetCodeItrEntity : IItrEntity
{
    string SetCode { get; }
}
```

**Acceptance Criteria:**
- [ ] File compiles without errors
- [ ] Contains SetCode for query parameter

---

### Task 2.4: Create SealedProduct Ouf Entity
**Status:** [x] Complete
**Depends on:** Nothing
**Location:** `src/Lib.Shared.DataModels/Entities/Oufs/SealedProducts/`

**Files to Create:**
```
ISealedProductOufEntity.cs
```

**ISealedProductOufEntity.cs:**
```csharp
namespace Lib.Shared.DataModels.Entities.Oufs.SealedProducts;

public interface ISealedProductOufEntity : IOufEntity
{
    string Uuid { get; }
    string SetId { get; }
    string SetCode { get; }
    string SetName { get; }
    string Name { get; }
    string Category { get; }
    string Subtype { get; }
    int? CardCount { get; }
    string ReleaseDate { get; }
    string TcgplayerProductId { get; }
    string ImageUrl { get; }
    string PurchaseUrlTcgplayer { get; }
    string PurchaseUrlCardmarket { get; }
    string PurchaseUrlCardKingdom { get; }
}
```

**Acceptance Criteria:**
- [ ] File compiles without errors
- [ ] Mirrors `ISealedProductItrEntity` properties

---

## Phase 3: Adapter Layer

### Task 3.1: Create Lib.Adapter.SealedProducts Project
**Status:** [x] Complete
**Depends on:** Nothing
**Location:** `src/Lib.Adapter.SealedProducts/`

**Commands:**
```bash
dotnet new classlib -n Lib.Adapter.SealedProducts -o src/Lib.Adapter.SealedProducts
dotnet sln src/MtgDiscoveryVibe.sln add src/Lib.Adapter.SealedProducts/Lib.Adapter.SealedProducts.csproj
```

**Update .csproj with references:**
```xml
<ItemGroup>
  <ProjectReference Include="..\Lib.Cosmos\Lib.Cosmos.csproj" />
  <ProjectReference Include="..\Lib.Adapter.Scryfall.Cosmos\Lib.Adapter.Scryfall.Cosmos.csproj" />
  <ProjectReference Include="..\Lib.Shared.Abstractions\Lib.Shared.Abstractions.csproj" />
  <ProjectReference Include="..\Lib.Shared.DataModels\Lib.Shared.DataModels.csproj" />
  <ProjectReference Include="..\Lib.Shared.Invocation\Lib.Shared.Invocation.csproj" />
</ItemGroup>
```

**Delete auto-generated Class1.cs**

**Acceptance Criteria:**
- [ ] Project added to solution
- [ ] `dotnet build` succeeds
- [ ] References added correctly

---

### Task 3.2: Create SealedProducts Adapter Exception
**Status:** [x] Complete
**Depends on:** Task 3.1
**Location:** `src/Lib.Adapter.SealedProducts/Exceptions/`

**Files to Create:**
```
SealedProductsAdapterException.cs
```

**SealedProductsAdapterException.cs:**
```csharp
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Adapter.SealedProducts.Exceptions;

internal sealed class SealedProductsAdapterException : OperationException
{
    public SealedProductsAdapterException(string message) : base(message) { }
    public SealedProductsAdapterException(string message, Exception innerException)
        : base(message, innerException) { }
}
```

**Acceptance Criteria:**
- [ ] File compiles without errors
- [ ] Extends `OperationException`

---

### Task 3.3: Create SealedProductsBySetCode Xfr Entity
**Status:** [x] Complete
**Depends on:** Task 3.1
**Location:** `src/Lib.Adapter.SealedProducts/Apis/Entities/`

**Files to Create:**
```
ISealedProductsBySetCodeXfrEntity.cs
SealedProductsBySetCodeXfrEntity.cs
```

**ISealedProductsBySetCodeXfrEntity.cs:**
```csharp
namespace Lib.Adapter.SealedProducts.Apis.Entities;

internal interface ISealedProductsBySetCodeXfrEntity
{
    string SetCode { get; }
}
```

**SealedProductsBySetCodeXfrEntity.cs:**
```csharp
namespace Lib.Adapter.SealedProducts.Apis.Entities;

internal sealed class SealedProductsBySetCodeXfrEntity : ISealedProductsBySetCodeXfrEntity
{
    public string SetCode { get; init; }
}
```

**Acceptance Criteria:**
- [ ] Files compile without errors
- [ ] Interface and implementation match

---

### Task 3.4: Create SealedProduct Ouf Entity Implementation
**Status:** [x] Complete
**Depends on:** Task 3.1, 2.4
**Location:** `src/Lib.Adapter.SealedProducts/Apis/Entities/`

**Files to Create:**
```
SealedProductOufEntity.cs
```

**SealedProductOufEntity.cs:**
```csharp
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;

namespace Lib.Adapter.SealedProducts.Apis.Entities;

internal sealed class SealedProductOufEntity : ISealedProductOufEntity
{
    public string Uuid { get; init; }
    public string SetId { get; init; }
    public string SetCode { get; init; }
    public string SetName { get; init; }
    public string Name { get; init; }
    public string Category { get; init; }
    public string Subtype { get; init; }
    public int? CardCount { get; init; }
    public string ReleaseDate { get; init; }
    public string TcgplayerProductId { get; init; }
    public string ImageUrl { get; init; }
    public string PurchaseUrlTcgplayer { get; init; }
    public string PurchaseUrlCardmarket { get; init; }
    public string PurchaseUrlCardKingdom { get; init; }
}
```

**Acceptance Criteria:**
- [ ] File compiles without errors
- [ ] Implements `ISealedProductOufEntity`

---

### Task 3.5: Create SealedProduct ExtToOuf Mapper
**Status:** [x] Complete
**Depends on:** Task 3.1, 3.4
**Location:** `src/Lib.Adapter.SealedProducts/Queries/Mappers/`

**Files to Create:**
```
ISealedProductExtToOufMapper.cs
SealedProductExtToOufMapper.cs
```

**ISealedProductExtToOufMapper.cs:**
```csharp
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;

namespace Lib.Adapter.SealedProducts.Queries.Mappers;

internal interface ISealedProductExtToOufMapper
{
    ISealedProductOufEntity Map(SealedProductExtEntity source);
}
```

**SealedProductExtToOufMapper.cs:**
```csharp
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.SealedProducts.Apis.Entities;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;

namespace Lib.Adapter.SealedProducts.Queries.Mappers;

internal sealed class SealedProductExtToOufMapper : ISealedProductExtToOufMapper
{
    public ISealedProductOufEntity Map(SealedProductExtEntity source) =>
        new SealedProductOufEntity
        {
            Uuid = source.Uuid,
            SetId = source.SetId,
            SetCode = source.SetCode,
            SetName = source.SetName,
            Name = source.Name,
            Category = source.Category,
            Subtype = source.Subtype,
            CardCount = source.CardCount,
            ReleaseDate = source.ReleaseDate,
            TcgplayerProductId = source.TcgplayerProductId,
            ImageUrl = source.ImageUrl,
            PurchaseUrlTcgplayer = source.PurchaseUrlTcgplayer,
            PurchaseUrlCardmarket = source.PurchaseUrlCardmarket,
            PurchaseUrlCardKingdom = source.PurchaseUrlCardKingdom
        };
}
```

**Acceptance Criteria:**
- [ ] Files compile without errors
- [ ] Maps all properties from ExtEntity to OufEntity

---

### Task 3.6: Create SealedProductsBySetCodeAdapter
**Status:** [x] Complete
**Depends on:** Task 1.4, 3.3, 3.5
**Location:** `src/Lib.Adapter.SealedProducts/Apis/Queries/`

**Files to Create:**
```
ISealedProductsBySetCodeAdapter.cs
SealedProductsBySetCodeAdapter.cs
```

**ISealedProductsBySetCodeAdapter.cs:**
```csharp
using Lib.Adapter.SealedProducts.Apis.Entities;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation;

namespace Lib.Adapter.SealedProducts.Apis.Queries;

internal interface ISealedProductsBySetCodeAdapter
{
    Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> ExecuteAsync(
        ISealedProductsBySetCodeXfrEntity input,
        CancellationToken cancellationToken);
}
```

**SealedProductsBySetCodeAdapter.cs:**
```csharp
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;
using Lib.Adapter.SealedProducts.Apis.Entities;
using Lib.Adapter.SealedProducts.Exceptions;
using Lib.Adapter.SealedProducts.Queries.Mappers;
using Lib.Cosmos.Apis;
using Lib.Cosmos.Apis.Primitives;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation;

namespace Lib.Adapter.SealedProducts.Apis.Queries;

internal sealed class SealedProductsBySetCodeAdapter : ISealedProductsBySetCodeAdapter
{
    private readonly ScryfallSetCodeIndexGopher _setCodeIndexGopher;
    private readonly SealedProductsBySetIdInquisition _inquisition;
    private readonly ISealedProductExtToOufMapper _mapper;

    public SealedProductsBySetCodeAdapter(
        ScryfallSetCodeIndexGopher setCodeIndexGopher,
        SealedProductsBySetIdInquisition inquisition,
        ISealedProductExtToOufMapper mapper)
    {
        _setCodeIndexGopher = setCodeIndexGopher;
        _inquisition = inquisition;
        _mapper = mapper;
    }

    public async Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> ExecuteAsync(
        ISealedProductsBySetCodeXfrEntity input,
        CancellationToken cancellationToken)
    {
        // 1. Lookup setId from setCode
#pragma warning disable CA1308 // SetCode in index is lowercase
        string lowercaseSetCode = input.SetCode.ToLowerInvariant();
#pragma warning restore CA1308

        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(lowercaseSetCode),
            Partition = new ProvidedPartitionKeyValue(lowercaseSetCode)
        };

        OpResponse<ScryfallSetCodeIndexExtEntity> indexResponse = await _setCodeIndexGopher
            .ReadAsync<ScryfallSetCodeIndexExtEntity>(readPoint, cancellationToken)
            .ConfigureAwait(false);

        if (indexResponse.IsFailure)
        {
            return OperationResponse<IEnumerable<ISealedProductOufEntity>>.Failure(
                new SealedProductsAdapterException($"Set code '{input.SetCode}' not found"));
        }

        string setId = indexResponse.Value.SetId;

        // 2. Query sealed products by setId
        SealedProductsBySetIdArgs args = new() { SetId = setId };

        OpResponse<IEnumerable<SealedProductExtEntity>> response = await _inquisition
            .QueryAsync<SealedProductExtEntity>(args, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return OperationResponse<IEnumerable<ISealedProductOufEntity>>.Failure(
                new SealedProductsAdapterException("Failed to query sealed products"));
        }

        // 3. Map ExtEntity to OufEntity
        IEnumerable<ISealedProductOufEntity> products = response.Value.Select(_mapper.Map);

        return OperationResponse<IEnumerable<ISealedProductOufEntity>>.Success(products);
    }
}
```

**Acceptance Criteria:**
- [ ] Files compile without errors
- [ ] Performs setCode → setId lookup
- [ ] Returns `IOperationResponse<T>` (not `OpResponse<T>`)
- [ ] Uses lowercase setCode for index lookup

---

### Task 3.7: Create SealedProductsAdapterService
**Status:** [x] Complete
**Depends on:** Task 3.6
**Location:** `src/Lib.Adapter.SealedProducts/Apis/`

**Files to Create:**
```
ISealedProductsAdapterService.cs
SealedProductsAdapterService.cs
```

**ISealedProductsAdapterService.cs:**
```csharp
using Lib.Adapter.SealedProducts.Apis.Queries;

namespace Lib.Adapter.SealedProducts.Apis;

internal interface ISealedProductsAdapterService
{
    ISealedProductsBySetCodeAdapter SealedProductsBySetCodeAdapter { get; }
}
```

**SealedProductsAdapterService.cs:**
```csharp
using Lib.Adapter.SealedProducts.Apis.Queries;

namespace Lib.Adapter.SealedProducts.Apis;

internal sealed class SealedProductsAdapterService : ISealedProductsAdapterService
{
    public SealedProductsAdapterService(
        ISealedProductsBySetCodeAdapter sealedProductsBySetCodeAdapter)
    {
        SealedProductsBySetCodeAdapter = sealedProductsBySetCodeAdapter;
    }

    public ISealedProductsBySetCodeAdapter SealedProductsBySetCodeAdapter { get; }
}
```

**Acceptance Criteria:**
- [ ] Files compile without errors
- [ ] Service exposes adapter

---

## Phase 4: Aggregator Layer

### Task 4.1: Create Lib.Aggregator.SealedProducts Project
**Status:** [x] Complete
**Depends on:** Nothing
**Location:** `src/Lib.Aggregator.SealedProducts/`

**Commands:**
```bash
dotnet new classlib -n Lib.Aggregator.SealedProducts -o src/Lib.Aggregator.SealedProducts
dotnet sln src/MtgDiscoveryVibe.sln add src/Lib.Aggregator.SealedProducts/Lib.Aggregator.SealedProducts.csproj
```

**Update .csproj with references:**
```xml
<ItemGroup>
  <ProjectReference Include="..\Lib.Adapter.SealedProducts\Lib.Adapter.SealedProducts.csproj" />
  <ProjectReference Include="..\Lib.Shared.Abstractions\Lib.Shared.Abstractions.csproj" />
  <ProjectReference Include="..\Lib.Shared.DataModels\Lib.Shared.DataModels.csproj" />
  <ProjectReference Include="..\Lib.Shared.Invocation\Lib.Shared.Invocation.csproj" />
</ItemGroup>
```

**Delete auto-generated Class1.cs**

**Acceptance Criteria:**
- [ ] Project added to solution
- [ ] `dotnet build` succeeds

---

### Task 4.2: Create SealedProductsBySetCode ItrToXfr Mapper
**Status:** [x] Complete
**Depends on:** Task 4.1, 2.3, 3.3
**Location:** `src/Lib.Aggregator.SealedProducts/Queries/Mappers/`

**Files to Create:**
```
ISealedProductsBySetCodeItrToXfrMapper.cs
SealedProductsBySetCodeItrToXfrMapper.cs
```

**ISealedProductsBySetCodeItrToXfrMapper.cs:**
```csharp
using Lib.Adapter.SealedProducts.Apis.Entities;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;

namespace Lib.Aggregator.SealedProducts.Queries.Mappers;

internal interface ISealedProductsBySetCodeItrToXfrMapper
{
    ISealedProductsBySetCodeXfrEntity Map(ISealedProductsBySetCodeItrEntity source);
}
```

**SealedProductsBySetCodeItrToXfrMapper.cs:**
```csharp
using Lib.Adapter.SealedProducts.Apis.Entities;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;

namespace Lib.Aggregator.SealedProducts.Queries.Mappers;

internal sealed class SealedProductsBySetCodeItrToXfrMapper : ISealedProductsBySetCodeItrToXfrMapper
{
    public ISealedProductsBySetCodeXfrEntity Map(ISealedProductsBySetCodeItrEntity source) =>
        new SealedProductsBySetCodeXfrEntity
        {
            SetCode = source.SetCode
        };
}
```

**Acceptance Criteria:**
- [ ] Files compile without errors
- [ ] Maps ItrEntity to XfrEntity

---

### Task 4.3: Create SealedProductsBySetCodeAggregator
**Status:** [ ] Not Started
**Depends on:** Task 3.7, 4.2
**Location:** `src/Lib.Aggregator.SealedProducts/Apis/Queries/`

**Files to Create:**
```
ISealedProductsBySetCodeAggregator.cs
SealedProductsBySetCodeAggregator.cs
```

**ISealedProductsBySetCodeAggregator.cs:**
```csharp
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation;

namespace Lib.Aggregator.SealedProducts.Apis.Queries;

internal interface ISealedProductsBySetCodeAggregator
{
    Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> ExecuteAsync(
        ISealedProductsBySetCodeItrEntity input,
        CancellationToken cancellationToken);
}
```

**SealedProductsBySetCodeAggregator.cs:**
```csharp
using Lib.Adapter.SealedProducts.Apis;
using Lib.Aggregator.SealedProducts.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation;

namespace Lib.Aggregator.SealedProducts.Apis.Queries;

internal sealed class SealedProductsBySetCodeAggregator : ISealedProductsBySetCodeAggregator
{
    private readonly ISealedProductsAdapterService _adapterService;
    private readonly ISealedProductsBySetCodeItrToXfrMapper _mapper;

    public SealedProductsBySetCodeAggregator(
        ISealedProductsAdapterService adapterService,
        ISealedProductsBySetCodeItrToXfrMapper mapper)
    {
        _adapterService = adapterService;
        _mapper = mapper;
    }

    public async Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> ExecuteAsync(
        ISealedProductsBySetCodeItrEntity input,
        CancellationToken cancellationToken)
    {
        var xfrEntity = _mapper.Map(input);

        return await _adapterService.SealedProductsBySetCodeAdapter
            .ExecuteAsync(xfrEntity, cancellationToken)
            .ConfigureAwait(false);
    }
}
```

**Acceptance Criteria:**
- [ ] Files compile without errors
- [ ] Maps ItrEntity to XfrEntity before calling adapter

---

### Task 4.4: Create SealedProductsAggregatorService
**Status:** [ ] Not Started
**Depends on:** Task 4.3
**Location:** `src/Lib.Aggregator.SealedProducts/Apis/`

**Files to Create:**
```
ISealedProductsAggregatorService.cs
SealedProductsAggregatorService.cs
```

**ISealedProductsAggregatorService.cs:**
```csharp
using Lib.Aggregator.SealedProducts.Apis.Queries;

namespace Lib.Aggregator.SealedProducts.Apis;

internal interface ISealedProductsAggregatorService
{
    ISealedProductsBySetCodeAggregator SealedProductsBySetCodeAggregator { get; }
}
```

**SealedProductsAggregatorService.cs:**
```csharp
using Lib.Aggregator.SealedProducts.Apis.Queries;

namespace Lib.Aggregator.SealedProducts.Apis;

internal sealed class SealedProductsAggregatorService : ISealedProductsAggregatorService
{
    public SealedProductsAggregatorService(
        ISealedProductsBySetCodeAggregator sealedProductsBySetCodeAggregator)
    {
        SealedProductsBySetCodeAggregator = sealedProductsBySetCodeAggregator;
    }

    public ISealedProductsBySetCodeAggregator SealedProductsBySetCodeAggregator { get; }
}
```

**Acceptance Criteria:**
- [ ] Files compile without errors
- [ ] Service exposes aggregator

---

## Phase 5: Domain Layer

### Task 5.1: Create Lib.Domain.SealedProducts Project
**Status:** [ ] Not Started
**Depends on:** Nothing
**Location:** `src/Lib.Domain.SealedProducts/`

**Commands:**
```bash
dotnet new classlib -n Lib.Domain.SealedProducts -o src/Lib.Domain.SealedProducts
dotnet sln src/MtgDiscoveryVibe.sln add src/Lib.Domain.SealedProducts/Lib.Domain.SealedProducts.csproj
```

**Update .csproj with references:**
```xml
<ItemGroup>
  <ProjectReference Include="..\Lib.Aggregator.SealedProducts\Lib.Aggregator.SealedProducts.csproj" />
  <ProjectReference Include="..\Lib.Shared.Abstractions\Lib.Shared.Abstractions.csproj" />
  <ProjectReference Include="..\Lib.Shared.DataModels\Lib.Shared.DataModels.csproj" />
  <ProjectReference Include="..\Lib.Shared.Invocation\Lib.Shared.Invocation.csproj" />
</ItemGroup>
```

**Delete auto-generated Class1.cs**

**Acceptance Criteria:**
- [ ] Project added to solution
- [ ] `dotnet build` succeeds

---

### Task 5.2: Create SealedProductsBySetCodeDomain
**Status:** [ ] Not Started
**Depends on:** Task 4.4, 5.1
**Location:** `src/Lib.Domain.SealedProducts/Apis/Queries/`

**Files to Create:**
```
ISealedProductsBySetCodeDomain.cs
SealedProductsBySetCodeDomain.cs
```

**ISealedProductsBySetCodeDomain.cs:**
```csharp
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation;

namespace Lib.Domain.SealedProducts.Apis.Queries;

internal interface ISealedProductsBySetCodeDomain
{
    Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> ExecuteAsync(
        ISealedProductsBySetCodeItrEntity input,
        CancellationToken cancellationToken);
}
```

**SealedProductsBySetCodeDomain.cs:**
```csharp
using Lib.Aggregator.SealedProducts.Apis;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation;

namespace Lib.Domain.SealedProducts.Apis.Queries;

internal sealed class SealedProductsBySetCodeDomain : ISealedProductsBySetCodeDomain
{
    private readonly ISealedProductsAggregatorService _aggregatorService;

    public SealedProductsBySetCodeDomain(ISealedProductsAggregatorService aggregatorService)
    {
        _aggregatorService = aggregatorService;
    }

    public async Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> ExecuteAsync(
        ISealedProductsBySetCodeItrEntity input,
        CancellationToken cancellationToken) =>
        await _aggregatorService.SealedProductsBySetCodeAggregator
            .ExecuteAsync(input, cancellationToken)
            .ConfigureAwait(false);
}
```

**Acceptance Criteria:**
- [ ] Files compile without errors
- [ ] Pass-through to aggregator (no business rules yet)

---

### Task 5.3: Create SealedProductsDomainService
**Status:** [ ] Not Started
**Depends on:** Task 5.2
**Location:** `src/Lib.Domain.SealedProducts/Apis/`

**Files to Create:**
```
ISealedProductsDomainService.cs
SealedProductsDomainService.cs
```

**ISealedProductsDomainService.cs:**
```csharp
using Lib.Domain.SealedProducts.Apis.Queries;

namespace Lib.Domain.SealedProducts.Apis;

internal interface ISealedProductsDomainService
{
    ISealedProductsBySetCodeDomain SealedProductsBySetCodeDomain { get; }
}
```

**SealedProductsDomainService.cs:**
```csharp
using Lib.Domain.SealedProducts.Apis.Queries;

namespace Lib.Domain.SealedProducts.Apis;

internal sealed class SealedProductsDomainService : ISealedProductsDomainService
{
    public SealedProductsDomainService(
        ISealedProductsBySetCodeDomain sealedProductsBySetCodeDomain)
    {
        SealedProductsBySetCodeDomain = sealedProductsBySetCodeDomain;
    }

    public ISealedProductsBySetCodeDomain SealedProductsBySetCodeDomain { get; }
}
```

**Acceptance Criteria:**
- [ ] Files compile without errors
- [ ] Service exposes domain operation

---

## Phase 6: Entry Layer

### Task 6.1: Create SealedProduct OutEntity
**Status:** [ ] Not Started
**Depends on:** Nothing
**Location:** `src/Lib.MtgDiscovery.Entry/Entities/Outs/SealedProducts/`

**Files to Create:**
```
SealedProductOutEntity.cs
```

**SealedProductOutEntity.cs:**
```csharp
namespace Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;

public sealed class SealedProductOutEntity
{
    public string Uuid { get; init; }
    public string SetId { get; init; }
    public string SetCode { get; init; }
    public string SetName { get; init; }
    public string Name { get; init; }
    public string Category { get; init; }
    public string Subtype { get; init; }
    public int? CardCount { get; init; }
    public string ReleaseDate { get; init; }
    public string TcgplayerProductId { get; init; }
    public string ImageUrl { get; init; }
    public string PurchaseUrlTcgplayer { get; init; }
    public string PurchaseUrlCardmarket { get; init; }
    public string PurchaseUrlCardKingdom { get; init; }
}
```

**Acceptance Criteria:**
- [ ] File compiles without errors
- [ ] Public class for GraphQL layer consumption

---

### Task 6.2: Create SealedProductsBySetCode Itr Entity Implementation
**Status:** [ ] Not Started
**Depends on:** Task 2.3
**Location:** `src/Lib.MtgDiscovery.Entry/Entities/Itrs/SealedProducts/`

**Files to Create:**
```
SealedProductsBySetCodeItrEntity.cs
```

**SealedProductsBySetCodeItrEntity.cs:**
```csharp
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;

namespace Lib.MtgDiscovery.Entry.Entities.Itrs.SealedProducts;

internal sealed class SealedProductsBySetCodeItrEntity : ISealedProductsBySetCodeItrEntity
{
    public string SetCode { get; init; }
}
```

**Acceptance Criteria:**
- [ ] File compiles without errors
- [ ] Implements interface

---

### Task 6.3: Create SealedProductsBySetCode ArgToItr Mapper
**Status:** [ ] Not Started
**Depends on:** Task 2.1, 6.2
**Location:** `src/Lib.MtgDiscovery.Entry/Queries/Actions/Mappers/`

**Files to Create:**
```
ISealedProductsBySetCodeArgToItrMapper.cs
SealedProductsBySetCodeArgToItrMapper.cs
```

**ISealedProductsBySetCodeArgToItrMapper.cs:**
```csharp
using Lib.Shared.DataModels.Entities.Args.SealedProducts;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface ISealedProductsBySetCodeArgToItrMapper
{
    ISealedProductsBySetCodeItrEntity Map(ISealedProductsBySetCodeArgEntity source);
}
```

**SealedProductsBySetCodeArgToItrMapper.cs:**
```csharp
using Lib.MtgDiscovery.Entry.Entities.Itrs.SealedProducts;
using Lib.Shared.DataModels.Entities.Args.SealedProducts;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class SealedProductsBySetCodeArgToItrMapper : ISealedProductsBySetCodeArgToItrMapper
{
    public ISealedProductsBySetCodeItrEntity Map(ISealedProductsBySetCodeArgEntity source) =>
        new SealedProductsBySetCodeItrEntity
        {
            SetCode = source.SetCode
        };
}
```

**Acceptance Criteria:**
- [ ] Files compile without errors
- [ ] Maps ArgEntity to ItrEntity

---

### Task 6.4: Create SealedProduct OufToOut Mapper
**Status:** [ ] Not Started
**Depends on:** Task 2.4, 6.1
**Location:** `src/Lib.MtgDiscovery.Entry/Queries/Actions/Mappers/`

**Files to Create:**
```
ISealedProductOufToOutMapper.cs
SealedProductOufToOutMapper.cs
```

**ISealedProductOufToOutMapper.cs:**
```csharp
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface ISealedProductOufToOutMapper
{
    SealedProductOutEntity Map(ISealedProductOufEntity source);
}
```

**SealedProductOufToOutMapper.cs:**
```csharp
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class SealedProductOufToOutMapper : ISealedProductOufToOutMapper
{
    public SealedProductOutEntity Map(ISealedProductOufEntity source) =>
        new()
        {
            Uuid = source.Uuid,
            SetId = source.SetId,
            SetCode = source.SetCode,
            SetName = source.SetName,
            Name = source.Name,
            Category = source.Category,
            Subtype = source.Subtype,
            CardCount = source.CardCount,
            ReleaseDate = source.ReleaseDate,
            TcgplayerProductId = source.TcgplayerProductId,
            ImageUrl = source.ImageUrl,
            PurchaseUrlTcgplayer = source.PurchaseUrlTcgplayer,
            PurchaseUrlCardmarket = source.PurchaseUrlCardmarket,
            PurchaseUrlCardKingdom = source.PurchaseUrlCardKingdom
        };
}
```

**Acceptance Criteria:**
- [ ] Files compile without errors
- [ ] Maps OufEntity to OutEntity

---

### Task 6.5: Create SealedProductsBySetCode Validator
**Status:** [ ] Not Started
**Depends on:** Task 2.1
**Location:** `src/Lib.MtgDiscovery.Entry/Commands/Actions/Validators/SealedProducts/`

**Files to Create:**
```
ISealedProductsBySetCodeArgEntityValidator.cs
SealedProductsBySetCodeArgEntityValidator.cs
```

**ISealedProductsBySetCodeArgEntityValidator.cs:**
```csharp
using Lib.Shared.DataModels.Entities.Args.SealedProducts;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Validators.SealedProducts;

internal interface ISealedProductsBySetCodeArgEntityValidator
{
    bool IsValid(ISealedProductsBySetCodeArgEntity entity, out string errorMessage);
}
```

**SealedProductsBySetCodeArgEntityValidator.cs:**
```csharp
using Lib.Shared.DataModels.Entities.Args.SealedProducts;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Validators.SealedProducts;

internal sealed class SealedProductsBySetCodeArgEntityValidator
    : ISealedProductsBySetCodeArgEntityValidator
{
    public bool IsValid(ISealedProductsBySetCodeArgEntity entity, out string errorMessage)
    {
        if (entity is null)
        {
            errorMessage = "Request cannot be null";
            return false;
        }

        if (string.IsNullOrWhiteSpace(entity.SetCode))
        {
            errorMessage = "SetCode is required";
            return false;
        }

        errorMessage = null;
        return true;
    }
}
```

**Acceptance Criteria:**
- [ ] Files compile without errors
- [ ] Validates null and empty SetCode

---

### Task 6.6: Create SealedProductsBySetCodeEntryService
**Status:** [ ] Not Started
**Depends on:** Task 5.3, 6.3, 6.4, 6.5
**Location:** `src/Lib.MtgDiscovery.Entry/Queries/SealedProducts/`

**Files to Create:**
```
ISealedProductsBySetCodeEntryService.cs
SealedProductsBySetCodeEntryService.cs
```

**ISealedProductsBySetCodeEntryService.cs:**
```csharp
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.Shared.DataModels.Entities.Args.SealedProducts;
using Lib.Shared.Invocation;

namespace Lib.MtgDiscovery.Entry.Queries.SealedProducts;

public interface ISealedProductsBySetCodeEntryService
{
    Task<IOperationResponse<IEnumerable<SealedProductOutEntity>>> ExecuteAsync(
        ISealedProductsBySetCodeArgEntity args,
        CancellationToken cancellationToken);
}
```

**SealedProductsBySetCodeEntryService.cs:**
```csharp
using Lib.Domain.SealedProducts.Apis;
using Lib.MtgDiscovery.Entry.Commands.Actions.Validators.SealedProducts;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.MtgDiscovery.Entry.Queries.SealedProducts;

internal sealed class SealedProductsBySetCodeEntryService : ISealedProductsBySetCodeEntryService
{
    private readonly ISealedProductsDomainService _domainService;
    private readonly ISealedProductsBySetCodeArgEntityValidator _validator;
    private readonly ISealedProductsBySetCodeArgToItrMapper _argToItrMapper;
    private readonly ISealedProductOufToOutMapper _oufToOutMapper;

    public SealedProductsBySetCodeEntryService(
        ISealedProductsDomainService domainService,
        ISealedProductsBySetCodeArgEntityValidator validator,
        ISealedProductsBySetCodeArgToItrMapper argToItrMapper,
        ISealedProductOufToOutMapper oufToOutMapper)
    {
        _domainService = domainService;
        _validator = validator;
        _argToItrMapper = argToItrMapper;
        _oufToOutMapper = oufToOutMapper;
    }

    public async Task<IOperationResponse<IEnumerable<SealedProductOutEntity>>> ExecuteAsync(
        ISealedProductsBySetCodeArgEntity args,
        CancellationToken cancellationToken)
    {
        if (_validator.IsValid(args, out string errorMessage) is false)
        {
            return OperationResponse<IEnumerable<SealedProductOutEntity>>.Failure(
                new OperationException(errorMessage));
        }

        var itrEntity = _argToItrMapper.Map(args);

        IOperationResponse<IEnumerable<ISealedProductOufEntity>> response =
            await _domainService.SealedProductsBySetCodeDomain
                .ExecuteAsync(itrEntity, cancellationToken)
                .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return OperationResponse<IEnumerable<SealedProductOutEntity>>.Failure(
                response.Exception);
        }

        IEnumerable<SealedProductOutEntity> outEntities = response.Value.Select(_oufToOutMapper.Map);

        return OperationResponse<IEnumerable<SealedProductOutEntity>>.Success(outEntities);
    }
}
```

**Acceptance Criteria:**
- [ ] Files compile without errors
- [ ] Validates, maps, calls domain, maps response

---

### Task 6.7: Create ISealedProductsEntryService
**Status:** [ ] Not Started
**Depends on:** Task 6.6
**Location:** `src/Lib.MtgDiscovery.Entry/Apis/`

**Files to Create:**
```
ISealedProductsEntryService.cs
SealedProductsEntryService.cs
```

**ISealedProductsEntryService.cs:**
```csharp
using Lib.MtgDiscovery.Entry.Queries.SealedProducts;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface ISealedProductsEntryService
{
    ISealedProductsBySetCodeEntryService SealedProductsBySetCodeEntryService { get; }
}
```

**SealedProductsEntryService.cs:**
```csharp
using Lib.MtgDiscovery.Entry.Queries.SealedProducts;

namespace Lib.MtgDiscovery.Entry.Apis;

internal sealed class SealedProductsEntryService : ISealedProductsEntryService
{
    public SealedProductsEntryService(
        ISealedProductsBySetCodeEntryService sealedProductsBySetCodeEntryService)
    {
        SealedProductsBySetCodeEntryService = sealedProductsBySetCodeEntryService;
    }

    public ISealedProductsBySetCodeEntryService SealedProductsBySetCodeEntryService { get; }
}
```

**Acceptance Criteria:**
- [ ] Files compile without errors
- [ ] Service exposes entry service

---

### Task 6.8: Update IEntryService
**Status:** [ ] Not Started
**Depends on:** Task 6.7
**Location:** `src/Lib.MtgDiscovery.Entry/Apis/`

**Files to Modify:**
```
IEntryService.cs
EntryService.cs
```

**Add to IEntryService.cs:**
```csharp
ISealedProductsEntryService SealedProductsEntryService { get; }
```

**Add to EntryService.cs constructor and property:**
```csharp
// Constructor parameter
ISealedProductsEntryService sealedProductsEntryService

// Property
public ISealedProductsEntryService SealedProductsEntryService { get; }

// Assignment in constructor
SealedProductsEntryService = sealedProductsEntryService;
```

**Acceptance Criteria:**
- [ ] Files compile without errors
- [ ] IEntryService exposes SealedProductsEntryService

---

## Phase 7: GraphQL Layer

### Task 7.1: Create SealedProducts GraphQL ArgEntity
**Status:** [ ] Not Started
**Depends on:** Task 2.1
**Location:** `src/App.MtgDiscovery.GraphQL/Entities/Args/SealedProducts/`

**Files to Create:**
```
GetSealedProductsBySetCodeArgEntity.cs
```

**GetSealedProductsBySetCodeArgEntity.cs:**
```csharp
using Lib.Shared.DataModels.Entities.Args.SealedProducts;

namespace App.MtgDiscovery.GraphQL.Entities.Args.SealedProducts;

public sealed class GetSealedProductsBySetCodeArgEntity : ISealedProductsBySetCodeArgEntity
{
    public string SetCode { get; init; }
}
```

**Acceptance Criteria:**
- [ ] File compiles without errors
- [ ] Implements interface

---

### Task 7.2: Create SealedProducts Args Mapper
**Status:** [ ] Not Started
**Depends on:** Task 7.1
**Location:** `src/App.MtgDiscovery.GraphQL/Actions/Mappers/`

**Files to Create:**
```
IGetSealedProductsBySetCodeArgsMapper.cs
GetSealedProductsBySetCodeArgsMapper.cs
```

**IGetSealedProductsBySetCodeArgsMapper.cs:**
```csharp
using App.MtgDiscovery.GraphQL.Entities.Args.SealedProducts;
using Lib.Shared.DataModels.Entities.Args.SealedProducts;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers;

internal interface IGetSealedProductsBySetCodeArgsMapper
{
    ISealedProductsBySetCodeArgEntity Map(GetSealedProductsBySetCodeArgEntity source);
}
```

**GetSealedProductsBySetCodeArgsMapper.cs:**
```csharp
using App.MtgDiscovery.GraphQL.Entities.Args.SealedProducts;
using Lib.Shared.DataModels.Entities.Args.SealedProducts;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers;

internal sealed class GetSealedProductsBySetCodeArgsMapper : IGetSealedProductsBySetCodeArgsMapper
{
    public ISealedProductsBySetCodeArgEntity Map(GetSealedProductsBySetCodeArgEntity source) =>
        source;
}
```

**Acceptance Criteria:**
- [ ] Files compile without errors
- [ ] Pass-through mapper (entity already implements interface)

---

### Task 7.3: Create SealedProductType
**Status:** [ ] Not Started
**Depends on:** Task 6.1
**Location:** `src/App.MtgDiscovery.GraphQL/Entities/Types/SealedProducts/`

**Files to Create:**
```
SealedProductType.cs
```

**SealedProductType.cs:**
```csharp
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;

namespace App.MtgDiscovery.GraphQL.Entities.Types.SealedProducts;

public sealed class SealedProductType : ObjectType<SealedProductOutEntity>
{
    protected override void Configure(IObjectTypeDescriptor<SealedProductOutEntity> descriptor)
    {
        descriptor.Name("SealedProduct");

        descriptor.Field(f => f.Uuid).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.SetId).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.SetCode).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.SetName).Type<StringType>();
        descriptor.Field(f => f.Name).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.Category).Type<StringType>();
        descriptor.Field(f => f.Subtype).Type<StringType>();
        descriptor.Field(f => f.CardCount).Type<IntType>();
        descriptor.Field(f => f.ReleaseDate).Type<StringType>();
        descriptor.Field(f => f.TcgplayerProductId).Type<StringType>();
        descriptor.Field(f => f.ImageUrl).Type<StringType>();
        descriptor.Field(f => f.PurchaseUrlTcgplayer).Type<StringType>();
        descriptor.Field(f => f.PurchaseUrlCardmarket).Type<StringType>();
        descriptor.Field(f => f.PurchaseUrlCardKingdom).Type<StringType>();
    }
}
```

**Acceptance Criteria:**
- [ ] File compiles without errors
- [ ] All fields configured

---

### Task 7.4: Create SealedProducts ResponseModel Types
**Status:** [ ] Not Started
**Depends on:** Task 7.3
**Location:** `src/App.MtgDiscovery.GraphQL/Entities/Types/ResponseModels/`

**Files to Create:**
```
SealedProductsResponseModelUnionType.cs
SealedProductsSuccessDataResponseModelType.cs
```

**SealedProductsSuccessDataResponseModelType.cs:**
```csharp
using App.MtgDiscovery.GraphQL.Entities.Types.SealedProducts;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class SealedProductsSuccessDataResponseModelType
    : ObjectType<SuccessDataResponseModel<IEnumerable<SealedProductOutEntity>>>
{
    protected override void Configure(
        IObjectTypeDescriptor<SuccessDataResponseModel<IEnumerable<SealedProductOutEntity>>> descriptor)
    {
        descriptor.Name("SealedProductsSuccessDataResponseModel");
        descriptor.Field(f => f.Data).Type<NonNullType<ListType<NonNullType<SealedProductType>>>>();
    }
}
```

**SealedProductsResponseModelUnionType.cs:**
```csharp
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;

namespace App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;

public sealed class SealedProductsResponseModelUnionType
    : UnionType<IResponseModel>
{
    protected override void Configure(IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("SealedProductsResponseModel");
        descriptor.Type<SealedProductsSuccessDataResponseModelType>();
        descriptor.Type<FailureResponseModelType>();
    }
}
```

**Acceptance Criteria:**
- [ ] Files compile without errors
- [ ] Union includes success and failure types

---

### Task 7.5: Create SealedProductsQueryMethods
**Status:** [ ] Not Started
**Depends on:** Task 6.8, 7.2, 7.4
**Location:** `src/App.MtgDiscovery.GraphQL/Queries/`

**Files to Create:**
```
SealedProductsQueryMethods.cs
```

**SealedProductsQueryMethods.cs:**
```csharp
using App.MtgDiscovery.GraphQL.Actions.Mappers;
using App.MtgDiscovery.GraphQL.Entities.Args.SealedProducts;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.Shared.Invocation;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
public sealed class SealedProductsQueryMethods
{
    [GraphQLType(typeof(SealedProductsResponseModelUnionType))]
    public async Task<IResponseModel> SealedProductsBySetCode(
        [Service] IEntryService entryService,
        [Service] IGetSealedProductsBySetCodeArgsMapper argsMapper,
        GetSealedProductsBySetCodeArgEntity args,
        CancellationToken cancellationToken)
    {
        var mappedArgs = argsMapper.Map(args);

        IOperationResponse<IEnumerable<SealedProductOutEntity>> response =
            await entryService.SealedProductsEntryService.SealedProductsBySetCodeEntryService
                .ExecuteAsync(mappedArgs, cancellationToken)
                .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureResponseModel(response.Exception.Message);
        }

        return new SuccessDataResponseModel<IEnumerable<SealedProductOutEntity>>(response.Value);
    }
}
```

**Acceptance Criteria:**
- [ ] File compiles without errors
- [ ] Query method returns union type

---

### Task 7.6: Register GraphQL Types in Startup
**Status:** [ ] Not Started
**Depends on:** Task 7.5
**Location:** `src/App.MtgDiscovery.GraphQL/Startup.cs`

**Modifications:**
- Register `SealedProductType`
- Register `SealedProductsSuccessDataResponseModelType`
- Register `SealedProductsResponseModelUnionType`
- Register `SealedProductsQueryMethods` extension
- Register `IGetSealedProductsBySetCodeArgsMapper`

**Find existing type registrations and add:**
```csharp
.AddType<SealedProductType>()
.AddType<SealedProductsSuccessDataResponseModelType>()
.AddType<SealedProductsResponseModelUnionType>()
.AddTypeExtension<SealedProductsQueryMethods>()
```

**Register mapper in DI:**
```csharp
services.AddSingleton<IGetSealedProductsBySetCodeArgsMapper, GetSealedProductsBySetCodeArgsMapper>();
```

**Acceptance Criteria:**
- [ ] App compiles and starts
- [ ] Query appears in GraphQL schema
- [ ] Query works in Playground

**Verification Query:**
```graphql
query {
  sealedProductsBySetCode(args: { setCode: "MKM" }) {
    __typename
    ... on SealedProductsSuccessDataResponseModel {
      data {
        uuid
        name
        category
        imageUrl
      }
    }
    ... on FailureResponseModel {
      status { message }
    }
  }
}
```

---

## Phase 8: Frontend

### Task 8.1: Create GraphQL Query
**Status:** [ ] Not Started
**Depends on:** Task 7.6
**Location:** `client/src/graphql/queries/`

**Files to Create:**
```
sealedProducts.ts
```

**sealedProducts.ts:**
```typescript
import { gql } from '@apollo/client';

export const GET_SEALED_PRODUCTS_BY_SET_CODE = gql`
  query GetSealedProductsBySetCode($args: GetSealedProductsBySetCodeArgEntityInput!) {
    sealedProductsBySetCode(args: $args) {
      __typename
      ... on SealedProductsSuccessDataResponseModel {
        data {
          uuid
          setId
          setCode
          setName
          name
          category
          subtype
          cardCount
          releaseDate
          tcgplayerProductId
          imageUrl
          purchaseUrlTcgplayer
          purchaseUrlCardmarket
          purchaseUrlCardKingdom
        }
      }
      ... on FailureResponseModel {
        status {
          message
        }
      }
    }
  }
`;
```

**Run:** `npm run codegen`

**Acceptance Criteria:**
- [ ] File created
- [ ] `npm run codegen` succeeds
- [ ] Generated types available

---

### Task 8.2: Create useSealedProductsData Hook
**Status:** [ ] Not Started
**Depends on:** Task 8.1
**Location:** `client/src/hooks/`

**Files to Create:**
```
useSealedProductsData.ts
```

**useSealedProductsData.ts:**
```typescript
import { useState, useEffect } from 'react';
import { useApolloClient } from '@apollo/client';
import { GET_SEALED_PRODUCTS_BY_SET_CODE } from '../graphql/queries/sealedProducts';

export interface SealedProduct {
  uuid: string;
  setId: string;
  setCode: string;
  setName?: string;
  name: string;
  category?: string;
  subtype?: string;
  cardCount?: number;
  releaseDate?: string;
  tcgplayerProductId?: string;
  imageUrl?: string;
  purchaseUrlTcgplayer?: string;
  purchaseUrlCardmarket?: string;
  purchaseUrlCardKingdom?: string;
}

interface UseSealedProductsDataResult {
  sealedProducts: SealedProduct[];
  loading: boolean;
  error: Error | null;
}

export const useSealedProductsData = (
  setCode: string,
  isActive: boolean
): UseSealedProductsDataResult => {
  const apolloClient = useApolloClient();
  const [sealedProducts, setSealedProducts] = useState<SealedProduct[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<Error | null>(null);

  useEffect(() => {
    if (!isActive || !setCode) {
      return;
    }

    const fetchSealedProducts = async () => {
      setLoading(true);
      setError(null);

      try {
        const response = await apolloClient.query({
          query: GET_SEALED_PRODUCTS_BY_SET_CODE,
          variables: { args: { setCode } },
          fetchPolicy: 'cache-first',
        });

        const data = response.data?.sealedProductsBySetCode;
        if (data?.__typename === 'SealedProductsSuccessDataResponseModel') {
          setSealedProducts(data.data || []);
        } else if (data?.__typename === 'FailureResponseModel') {
          setError(new Error(data.status?.message || 'Failed to fetch sealed products'));
        }
      } catch (err) {
        setError(err as Error);
      } finally {
        setLoading(false);
      }
    };

    fetchSealedProducts();
  }, [setCode, isActive, apolloClient]);

  return { sealedProducts, loading, error };
};
```

**Acceptance Criteria:**
- [ ] File created
- [ ] No TypeScript errors
- [ ] Only fetches when `isActive` is true

---

### Task 8.3: Create SealedProductCard Component
**Status:** [ ] Not Started
**Depends on:** Nothing (can mock data)
**Location:** `client/src/components/atoms/Sealed/`

**Files to Create:**
```
SealedProductCard.tsx
index.ts
```

**SealedProductCard.tsx:**
```typescript
import React from 'react';
import { Box, Typography, Chip, IconButton, Stack } from '@mui/material';
import type { SealedProduct } from '../../../hooks/useSealedProductsData';

interface SealedProductCardProps {
  product: SealedProduct;
  onProductClick?: (product: SealedProduct) => void;
}

export const SealedProductCard: React.FC<SealedProductCardProps> = ({
  product,
  onProductClick,
}) => {
  const categoryLabel = product.category?.replace(/_/g, ' ').toUpperCase();

  const handlePurchaseClick = (url: string | undefined, e: React.MouseEvent) => {
    e.stopPropagation();
    if (url) {
      window.open(url, '_blank', 'noopener,noreferrer');
    }
  };

  return (
    <Box
      sx={{
        bgcolor: 'grey.900',
        borderRadius: 2,
        overflow: 'hidden',
        cursor: onProductClick ? 'pointer' : 'default',
        '&:hover': {
          transform: 'translateY(-4px)',
          boxShadow: 4,
        },
        transition: 'transform 0.2s, box-shadow 0.2s',
      }}
      onClick={() => onProductClick?.(product)}
    >
      {/* Product Image */}
      <Box sx={{ position: 'relative', paddingTop: '100%' }}>
        <Box
          component="img"
          src={product.imageUrl || '/placeholder-sealed.png'}
          alt={product.name}
          sx={{
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%',
            height: '100%',
            objectFit: 'cover',
          }}
          onError={(e) => {
            (e.target as HTMLImageElement).src = '/placeholder-sealed.png';
          }}
        />
      </Box>

      {/* Product Info */}
      <Box sx={{ p: 2 }}>
        <Typography variant="subtitle2" noWrap title={product.name}>
          {product.name}
        </Typography>

        {categoryLabel && (
          <Chip
            label={categoryLabel}
            size="small"
            sx={{ mt: 1, fontSize: '0.65rem' }}
          />
        )}

        {product.cardCount && (
          <Typography variant="caption" color="text.secondary" display="block" sx={{ mt: 0.5 }}>
            {product.cardCount} cards
          </Typography>
        )}

        {/* Purchase Links */}
        <Stack direction="row" spacing={0.5} sx={{ mt: 1 }}>
          {product.purchaseUrlTcgplayer && (
            <Chip
              label="TCG"
              size="small"
              clickable
              onClick={(e) => handlePurchaseClick(product.purchaseUrlTcgplayer, e)}
              sx={{ fontSize: '0.6rem' }}
            />
          )}
          {product.purchaseUrlCardmarket && (
            <Chip
              label="CM"
              size="small"
              clickable
              onClick={(e) => handlePurchaseClick(product.purchaseUrlCardmarket, e)}
              sx={{ fontSize: '0.6rem' }}
            />
          )}
          {product.purchaseUrlCardKingdom && (
            <Chip
              label="CK"
              size="small"
              clickable
              onClick={(e) => handlePurchaseClick(product.purchaseUrlCardKingdom, e)}
              sx={{ fontSize: '0.6rem' }}
            />
          )}
        </Stack>
      </Box>
    </Box>
  );
};
```

**index.ts:**
```typescript
export { SealedProductCard } from './SealedProductCard';
```

**Acceptance Criteria:**
- [ ] Component renders
- [ ] Displays image, name, category, card count
- [ ] Purchase links open in new tab
- [ ] Handles missing images

---

### Task 8.4: Create SealedProductGrid Component
**Status:** [ ] Not Started
**Depends on:** Task 8.3
**Location:** `client/src/components/organisms/Sealed/`

**Files to Create:**
```
SealedProductGrid.tsx
index.ts
```

**SealedProductGrid.tsx:**
```typescript
import React from 'react';
import { Box, Typography, CircularProgress } from '@mui/material';
import { SealedProductCard } from '../../atoms/Sealed';
import type { SealedProduct } from '../../../hooks/useSealedProductsData';

interface SealedProductGridProps {
  products: SealedProduct[];
  loading: boolean;
  error?: Error | null;
  onProductClick?: (product: SealedProduct) => void;
}

export const SealedProductGrid: React.FC<SealedProductGridProps> = ({
  products,
  loading,
  error,
  onProductClick,
}) => {
  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}>
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return (
      <Box sx={{ textAlign: 'center', py: 4 }}>
        <Typography color="error">
          Error loading sealed products: {error.message}
        </Typography>
      </Box>
    );
  }

  if (products.length === 0) {
    return (
      <Box sx={{ textAlign: 'center', py: 4 }}>
        <Typography color="text.secondary">
          No sealed products found for this set.
        </Typography>
      </Box>
    );
  }

  return (
    <Box
      sx={{
        display: 'grid',
        gridTemplateColumns: {
          xs: 'repeat(2, 1fr)',
          sm: 'repeat(3, 1fr)',
          md: 'repeat(4, 1fr)',
          lg: 'repeat(5, 1fr)',
        },
        gap: 2,
      }}
    >
      {products.map((product) => (
        <SealedProductCard
          key={product.uuid}
          product={product}
          onProductClick={onProductClick}
        />
      ))}
    </Box>
  );
};
```

**index.ts:**
```typescript
export { SealedProductGrid } from './SealedProductGrid';
```

**Acceptance Criteria:**
- [ ] Grid displays products responsively
- [ ] Loading state shown
- [ ] Error state shown
- [ ] Empty state shown

---

### Task 8.5: Integrate Sealed Tab into SetPage
**Status:** [ ] Not Started
**Depends on:** Task 8.2, 8.4
**Location:** `client/src/components/pages/SetPage.tsx`

**Modifications:**

1. Add imports:
```typescript
import { ToggleButtonGroup, ToggleButton } from '@mui/material';
import { useSealedProductsData } from '../../hooks/useSealedProductsData';
import { SealedProductGrid } from '../organisms/Sealed';
```

2. Add state:
```typescript
const [activeTab, setActiveTab] = useState<'cards' | 'sealed'>('cards');
```

3. Add hook (with lazy loading):
```typescript
const { sealedProducts, loading: sealedLoading, error: sealedError } = useSealedProductsData(
  setCode,
  activeTab === 'sealed'
);
```

4. Add tab toggle UI (after set header, before card grid):
```typescript
<ToggleButtonGroup
  value={activeTab}
  exclusive
  onChange={(_, value) => value && setActiveTab(value)}
  sx={{ mb: 2 }}
>
  <ToggleButton value="cards">
    Cards ({cards.length})
  </ToggleButton>
  <ToggleButton value="sealed">
    Sealed ({sealedProducts.length})
  </ToggleButton>
</ToggleButtonGroup>
```

5. Add conditional rendering:
```typescript
{activeTab === 'cards' && (
  // Existing card display component
)}
{activeTab === 'sealed' && (
  <SealedProductGrid
    products={sealedProducts}
    loading={sealedLoading}
    error={sealedError}
  />
)}
```

**Acceptance Criteria:**
- [ ] Tab toggle appears on set page
- [ ] Cards tab shows existing card grid
- [ ] Sealed tab shows sealed products
- [ ] Sealed products only fetch when tab is active
- [ ] Tab counts update after data loads
- [ ] Works on mobile

---

## Summary

| Phase | Tasks | Files |
|-------|-------|-------|
| 1. Cosmos Read | 5 | 5 |
| 2. Shared Models | 4 | 4 |
| 3. Adapter | 7 | 14 |
| 4. Aggregator | 4 | 8 |
| 5. Domain | 3 | 6 |
| 6. Entry | 8 | 14 |
| 7. GraphQL | 6 | 7 |
| 8. Frontend | 5 | 6 |
| **Total** | **42** | **~64** |

## Dependency Graph

```
Phase 1 (Cosmos) ──┐
                   ├──► Phase 3 (Adapter) ──► Phase 4 (Aggregator) ──► Phase 5 (Domain) ──┐
Phase 2 (Models) ──┘                                                                       │
                                                                                           ▼
Phase 6 (Entry) ◄──────────────────────────────────────────────────────────────────────────┘
       │
       ▼
Phase 7 (GraphQL)
       │
       ▼
Phase 8 (Frontend)
```

**Parallel Work Possible:**
- Phase 1 and Phase 2 can be done in parallel
- Task 8.3 (SealedProductCard) can be done independently with mock data
- Frontend components can be built with mocked data before backend is complete
