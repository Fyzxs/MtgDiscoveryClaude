# Sealed Products Integration Plan

## Executive Summary

**Goal:** Integrate MTG sealed products (boosters, bundles, precons) from MTGJSON into the MTG Discovery collection platform.

**Scope:**
- Display sealed products on set pages via "Sealed" tab
- Users can add/remove sealed products to their collection
- Manual data refresh via CLI command
- Basic display: name, image, category, card count, purchase links

**Estimated Effort:** ~145 new files across 4 phases

---

## Prerequisites

Before starting implementation:
1. Review existing patterns in `Lib.Adapter.UserWishlistCards` (reference implementation)
2. Review `CardsBySetCodeAdapter.cs` for setCode→setId lookup pattern
3. Understand the entity flow: ArgEntity → ItrEntity → XfrEntity → ExtEntity → OufEntity → OutEntity
4. Have access to Azure Cosmos DB emulator or development instance

---

## Phase 1: Shared Data Models & Cosmos Infrastructure

**Goal:** Create the foundational interfaces and Cosmos DB containers

### Task 1.1: Create Shared DataModel Interfaces

**Location:** `src/Lib.Shared.DataModels/Entities/`

**Files to Create:**

```
Args/SealedProducts/
├── ISealedProductsBySetCodeArgEntity.cs
├── IAddUserSealedProductArgEntity.cs
└── IRemoveUserSealedProductArgEntity.cs

Itrs/SealedProducts/
├── ISealedProductItrEntity.cs
├── ISealedProductsBySetCodeItrEntity.cs
└── IUserSealedProductItrEntity.cs

Oufs/SealedProducts/
├── ISealedProductOufEntity.cs
└── IUserSealedProductOufEntity.cs
```

**ISealedProductsBySetCodeArgEntity.cs:**
```csharp
public interface ISealedProductsBySetCodeArgEntity : IArgEntity
{
    string SetCode { get; }
    string UserId { get; }  // Optional - for user collection data
}
```

**ISealedProductItrEntity.cs:**
```csharp
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
- [ ] All interfaces compile without errors
- [ ] Interfaces follow existing naming conventions
- [ ] No implementation classes yet (interfaces only)

---

### Task 1.2: Create SealedProducts Cosmos Container

**Location:** `src/Lib.Adapter.Scryfall.Cosmos/`

**Files to Create:**

```
Cosmos/Primitives/
└── SealedProductsCosmosContainerName.cs

Cosmos/Containers/
└── SealedProductsCosmosContainer.cs

Cosmos/Containers/Definitions/
└── SealedProductsCosmosContainerDefinition.cs

Apis/CosmosItems/
└── SealedProductExtEntity.cs
```

**SealedProductsCosmosContainerName.cs:**
```csharp
namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Primitives;

internal sealed class SealedProductsCosmosContainerName : CosmosContainerName
{
    public SealedProductsCosmosContainerName() : base("SealedProducts") { }
}
```

**SealedProductExtEntity.cs:**
```csharp
public sealed class SealedProductExtEntity : CosmosItem
{
    public override string Id => Uuid;
    public override string Partition => SetId;  // Uses setId for partition (like cards)

    [JsonProperty("uuid")]
    public string Uuid { get; init; }

    [JsonProperty("set_id")]
    public string SetId { get; init; }  // Scryfall GUID (looked up during ingestion)

    [JsonProperty("set_code")]
    public string SetCode { get; init; }  // For display

    [JsonProperty("name")]
    public string Name { get; init; }

    [JsonProperty("category")]
    public string Category { get; init; }

    [JsonProperty("subtype")]
    public string Subtype { get; init; }

    [JsonProperty("card_count")]
    public int? CardCount { get; init; }

    [JsonProperty("release_date")]
    public string ReleaseDate { get; init; }

    [JsonProperty("tcgplayer_product_id")]
    public string TcgplayerProductId { get; init; }

    [JsonProperty("image_url")]
    public string ImageUrl { get; init; }

    [JsonProperty("purchase_url_tcgplayer")]
    public string PurchaseUrlTcgplayer { get; init; }

    [JsonProperty("purchase_url_cardmarket")]
    public string PurchaseUrlCardmarket { get; init; }

    [JsonProperty("purchase_url_card_kingdom")]
    public string PurchaseUrlCardKingdom { get; init; }
}
```

**Reference Pattern:** Copy from `UserWishlistCardsCosmosContainerDefinition.cs`

**Acceptance Criteria:**
- [ ] Container definition compiles
- [ ] Partition key path is `/partition`
- [ ] ExtEntity has all required JsonProperty attributes
- [ ] Solution builds successfully

---

### Task 1.3: Create SealedProducts Cosmos Operators

**Location:** `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/`

**Files to Create:**

```
Gophers/
└── SealedProductsGopher.cs

Scribes/
└── SealedProductsScribe.cs

Inquisitors/
└── SealedProductsInquisitor.cs

Inquisitions/
├── SealedProductsBySetIdInquisition.cs
├── SealedProductsBySetIdQueryDefinition.cs
└── Args/SealedProductsBySetIdArgs.cs
```

**SealedProductsBySetIdQueryDefinition.cs:**
```csharp
internal sealed class SealedProductsBySetIdQueryDefinition : InquiryDefinition
{
    public override QueryDefinition AsSystemType() =>
        new("SELECT * FROM c WHERE c.partition = @setId");
}
```

**Reference Pattern:** Copy from `CardsBySetIdInquisition.cs`

**Acceptance Criteria:**
- [ ] Gopher can read single documents
- [ ] Scribe can write/upsert documents
- [ ] Inquisition queries by setId partition
- [ ] All operators compile without errors

---

### Task 1.4: Create UserSealedProducts Cosmos Container

**Location:** `src/Lib.Adapter.Scryfall.Cosmos/`

**Files to Create:**

```
Cosmos/Primitives/
└── UserSealedProductsCosmosContainerName.cs

Cosmos/Containers/
└── UserSealedProductsCosmosContainer.cs

Cosmos/Containers/Definitions/
└── UserSealedProductsCosmosContainerDefinition.cs

Apis/CosmosItems/
└── UserSealedProductExtEntity.cs
```

**UserSealedProductExtEntity.cs:**
```csharp
public sealed class UserSealedProductExtEntity : CosmosItem
{
    public override string Id => $"{UserId}_{ProductId}";
    public override string Partition => UserId;

    [JsonProperty("user_id")]
    public string UserId { get; init; }

    [JsonProperty("product_id")]
    public string ProductId { get; init; }

    [JsonProperty("product_name")]
    public string ProductName { get; init; }

    [JsonProperty("set_id")]
    public string SetId { get; init; }

    [JsonProperty("set_code")]
    public string SetCode { get; init; }

    [JsonProperty("set_name")]
    public string SetName { get; init; }

    [JsonProperty("category")]
    public string Category { get; init; }

    [JsonProperty("subtype")]
    public string Subtype { get; init; }

    [JsonProperty("tcgplayer_product_id")]
    public string TcgplayerProductId { get; init; }

    [JsonProperty("image_url")]
    public string ImageUrl { get; init; }

    [JsonProperty("count")]
    public int Count { get; init; }

    [JsonProperty("created_at")]
    public string CreatedAt { get; init; }

    [JsonProperty("updated_at")]
    public string UpdatedAt { get; init; }
}
```

**Acceptance Criteria:**
- [ ] Container partitions by userId
- [ ] Document ID is composite `{userId}_{productId}`
- [ ] All denormalized fields present for display without joins

---

### Task 1.5: Create UserSealedProducts Cosmos Operators

**Files to Create:**

```
Gophers/
└── UserSealedProductsGopher.cs

Scribes/
└── UserSealedProductsScribe.cs

Janitors/
└── UserSealedProductsJanitor.cs

Inquisitors/
└── UserSealedProductsInquisitor.cs

Inquisitions/
├── AllUserSealedProductsInquisition.cs
├── AllUserSealedProductsQueryDefinition.cs
└── Args/AllUserSealedProductsArgs.cs
```

**Acceptance Criteria:**
- [ ] Janitor can delete documents
- [ ] Inquisition queries all user's sealed products
- [ ] Solution builds successfully

---

## Phase 2: Backend Service Layers (Sealed Products Query)

**Goal:** Implement read-only sealed products display through all backend layers

**Dependencies:** Phase 1 complete

### Task 2.1: Create Lib.Adapter.SealedProducts Project

**Create new project:** `src/Lib.Adapter.SealedProducts/Lib.Adapter.SealedProducts.csproj`

**Add to solution:** `dotnet sln src/MtgDiscoveryVibe.sln add src/Lib.Adapter.SealedProducts/Lib.Adapter.SealedProducts.csproj`

**Project references:**
- Lib.Cosmos
- Lib.Adapter.Scryfall.Cosmos
- Lib.Shared.Abstractions
- Lib.Shared.DataModels
- Lib.Shared.Invocation

**Files to Create:**

```
Apis/
├── ISealedProductsAdapterService.cs
├── SealedProductsAdapterService.cs
├── Entities/
│   └── ISealedProductsBySetCodeXfrEntity.cs
└── Queries/
    ├── ISealedProductsBySetCodeAdapter.cs
    └── SealedProductsBySetCodeAdapter.cs

Exceptions/
└── SealedProductsAdapterException.cs

Queries/Mappers/
├── ISealedProductExtToOufMapper.cs
└── SealedProductExtToOufMapper.cs
```

**CRITICAL - SetCode→SetId Lookup Pattern:**

The adapter MUST perform setCode→setId lookup. Reference `CardsBySetCodeAdapter.cs`:

```csharp
public sealed class SealedProductsBySetCodeAdapter
{
    private readonly ICosmosGopher _setCodeIndexGopher;  // ScryfallSetCodeIndexGopher
    private readonly ICosmosInquisition<SealedProductsBySetIdArgs> _sealedProductsInquisition;

    public async Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> Execute(
        ISealedProductsBySetCodeXfrEntity input,
        CancellationToken cancellationToken)
    {
        // 1. Create ReadPointItem for index lookup
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(input.SetCode),
            Partition = new ProvidedPartitionKeyValue(input.SetCode)
        };

        // 2. Lookup setId from SetCodeToIdAssociations container
        OpResponse<ScryfallSetCodeIndexExtEntity> indexResponse =
            await _setCodeIndexGopher.ReadAsync<ScryfallSetCodeIndexExtEntity>(readPoint)
                .ConfigureAwait(false);

        // 3. Extract setId and query sealed products
        string setId = indexResponse.Value.SetId;
        SealedProductsBySetIdArgs args = new() { SetId = setId };

        OpResponse<IEnumerable<SealedProductExtEntity>> response =
            await _sealedProductsInquisition.QueryAsync<SealedProductExtEntity>(args)
                .ConfigureAwait(false);

        // 4. Map ExtEntity → OufEntity and return
        return _mapper.Map(response);
    }
}
```

**Acceptance Criteria:**
- [ ] Adapter performs setCode→setId lookup using `ScryfallSetCodeIndexGopher`
- [ ] Returns `IOperationResponse<T>` (NOT `OpResponse<T>`)
- [ ] Exception extends `OperationException`
- [ ] Mapper converts ExtEntity to OufEntity

---

### Task 2.2: Create Lib.Aggregator.SealedProducts Project

**Create new project:** `src/Lib.Aggregator.SealedProducts/Lib.Aggregator.SealedProducts.csproj`

**Project references:**
- Lib.Adapter.SealedProducts
- Lib.Shared.Abstractions
- Lib.Shared.DataModels
- Lib.Shared.Invocation

**Files to Create:**

```
Apis/
├── ISealedProductsAggregatorService.cs
├── SealedProductsAggregatorService.cs
└── Queries/
    ├── ISealedProductsBySetCodeAggregator.cs
    └── SealedProductsBySetCodeAggregator.cs

Queries/Mappers/
├── ISealedProductsBySetCodeItrToXfrMapper.cs
└── SealedProductsBySetCodeItrToXfrMapper.cs
```

**Acceptance Criteria:**
- [ ] Aggregator receives ItrEntity, maps to XfrEntity
- [ ] Calls adapter service
- [ ] Returns OufEntity collection

---

### Task 2.3: Create Lib.Domain.SealedProducts Project

**Create new project:** `src/Lib.Domain.SealedProducts/Lib.Domain.SealedProducts.csproj`

**Project references:**
- Lib.Aggregator.SealedProducts
- Lib.Shared.Abstractions
- Lib.Shared.DataModels
- Lib.Shared.Invocation

**Files to Create:**

```
Apis/
├── ISealedProductsDomainService.cs
├── SealedProductsDomainService.cs
└── Queries/
    ├── ISealedProductsBySetCodeDomain.cs
    └── SealedProductsBySetCodeDomain.cs
```

**Acceptance Criteria:**
- [ ] Domain service passes ItrEntity to aggregator
- [ ] No business rules applied (pass-through for now)
- [ ] Returns OufEntity collection

---

### Task 2.4: Update Entry Layer for Sealed Products

**Location:** `src/Lib.MtgDiscovery.Entry/`

**Files to Create:**

```
Apis/
└── ISealedProductsEntryService.cs

Entities/Outs/SealedProducts/
└── SealedProductOutEntity.cs

Queries/SealedProducts/
├── ISealedProductsBySetCodeEntryService.cs
└── SealedProductsBySetCodeEntryService.cs

Queries/Actions/Mappers/
├── ISealedProductsBySetCodeArgToItrMapper.cs
├── SealedProductsBySetCodeArgToItrMapper.cs
├── ISealedProductOufToOutMapper.cs
└── SealedProductOufToOutMapper.cs

Commands/Actions/Validators/SealedProducts/
├── ISealedProductsBySetCodeArgEntityValidator.cs
└── SealedProductsBySetCodeArgEntityValidator.cs
```

**SealedProductOutEntity.cs:**
```csharp
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

**Update IEntryService.cs:**
```csharp
// Add to interface
ISealedProductsEntryService SealedProductsEntryService { get; }
```

**Acceptance Criteria:**
- [ ] Entry service validates input
- [ ] Maps ArgEntity → ItrEntity → Domain → OufEntity → OutEntity
- [ ] IEntryService composition updated
- [ ] Solution builds successfully

---

### Task 2.5: Create GraphQL Query Endpoint

**Location:** `src/App.MtgDiscovery.GraphQL/`

**Files to Create:**

```
Queries/
└── SealedProductsQueryMethods.cs

Entities/Args/SealedProducts/
└── GetSealedProductsBySetCodeArgEntity.cs

Entities/Types/SealedProducts/
└── SealedProductType.cs

Entities/Types/ResponseModels/
├── SealedProductsResponseModelUnionType.cs
└── SealedProductsSuccessDataResponseModelType.cs

Actions/Mappers/
├── IGetSealedProductsBySetCodeArgsMapper.cs
└── GetSealedProductsBySetCodeArgsMapper.cs
```

**SealedProductsQueryMethods.cs:**
```csharp
[ExtendObjectType(typeof(ApiQuery))]
public sealed class SealedProductsQueryMethods
{
    public async Task<IResponseModel> SealedProductsBySetCode(
        [Service] IEntryService entryService,
        GetSealedProductsBySetCodeArgEntity args,
        CancellationToken cancellationToken)
    {
        // Map args, call entry service, return response model
    }
}
```

**Acceptance Criteria:**
- [ ] Query accepts setCode parameter
- [ ] Returns union type (Success/Failure)
- [ ] GraphQL schema generates correctly
- [ ] Query works in GraphQL Playground

**Verification:**
```graphql
query {
  sealedProductsBySetCode(setCode: { setCode: "MKM" }) {
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

## Phase 3: Data Ingestion CLI

**Goal:** Create CLI tool to import sealed products from MTGJSON

**Dependencies:** Phase 1 complete (Cosmos containers exist)

### Task 3.1: Create Ingestion DTOs

**Location:** `src/Lib.Scryfall.Ingestion/SealedProducts/Dtos/`

**Files to Create:**

```
MtgJsonSetDto.cs
MtgJsonSealedProductDto.cs
MtgJsonIdentifiersDto.cs
MtgJsonPurchaseUrlsDto.cs
```

**MtgJsonSealedProductDto.cs:**
```csharp
public sealed class MtgJsonSealedProductDto
{
    [JsonProperty("uuid")]
    public string Uuid { get; init; }

    [JsonProperty("name")]
    public string Name { get; init; }

    [JsonProperty("category")]
    public string Category { get; init; }

    [JsonProperty("subtype")]
    public string Subtype { get; init; }

    [JsonProperty("cardCount")]
    public int? CardCount { get; init; }

    [JsonProperty("releaseDate")]
    public string ReleaseDate { get; init; }

    [JsonProperty("identifiers")]
    public MtgJsonIdentifiersDto Identifiers { get; init; }

    [JsonProperty("purchaseUrls")]
    public MtgJsonPurchaseUrlsDto PurchaseUrls { get; init; }
}
```

**Acceptance Criteria:**
- [ ] DTOs match MTGJSON structure exactly
- [ ] All JsonProperty attributes use camelCase (MTGJSON format)

---

### Task 3.2: Create Ingestion Service

**Location:** `src/Lib.Scryfall.Ingestion/SealedProducts/`

**Files to Create:**

```
IMtgJsonSetFetcher.cs
MtgJsonSetFetcher.cs
ISealedProductMapper.cs
SealedProductMapper.cs
ISealedProductIngestionService.cs
SealedProductIngestionService.cs
```

**Ingestion Flow:**
1. Fetch `https://mtgjson.com/api/v5/{SET_CODE}.json`
2. Deserialize, extract `sealedProduct[]` array
3. **Lookup setId** from `SetCodeToIdAssociations` using set code
4. Map each product to `SealedProductExtEntity`:
   - `SetId` = looked up Scryfall GUID
   - `SetCode` = original MTGJSON code
   - `ImageUrl` = `https://tcgplayer-cdn.tcgplayer.com/product/{tcgplayerProductId}_in_400x400.jpg`
5. Upsert to `SealedProducts` container

**Acceptance Criteria:**
- [ ] Fetches from MTGJSON API
- [ ] Performs setCode→setId lookup
- [ ] Computes TCGPlayer image URL
- [ ] Handles missing identifiers gracefully

---

### Task 3.3: Create CLI Tool Project

**Create new project:** `src/Example.SealedProductIngestion/Example.SealedProductIngestion.csproj`

**Files to Create:**
```
Program.cs
```

**Usage:**
```bash
dotnet run --project src/Example.SealedProductIngestion -- MKM
dotnet run --project src/Example.SealedProductIngestion -- DSK WOE BLB
```

**Acceptance Criteria:**
- [ ] Accepts set code(s) as command line arguments
- [ ] Logs progress and results
- [ ] Handles errors gracefully
- [ ] Verify data in Cosmos DB after run

---

## Phase 4: Frontend Integration

**Goal:** Display sealed products on set pages with collection management

**Dependencies:** Phase 2 complete (GraphQL endpoint working)

### Task 4.1: Create TypeScript Types

**Location:** `client/src/types/`

**File to Create:** `sealedProduct.ts`

```typescript
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

export interface UserSealedProduct extends SealedProduct {
  count: number;
  createdAt?: string;
  updatedAt?: string;
}
```

**Acceptance Criteria:**
- [ ] Types match GraphQL schema
- [ ] No TypeScript errors

---

### Task 4.2: Create GraphQL Query

**Location:** `client/src/graphql/queries/`

**File to Create:** `sealedProducts.ts`

```typescript
import { gql } from '@apollo/client';

export const GET_SEALED_PRODUCTS_BY_SET_CODE = gql`
  query GetSealedProductsBySetCode($setCode: GetSealedProductsBySetCodeArgEntityInput!) {
    sealedProductsBySetCode(args: $setCode) {
      __typename
      ... on SealedProductsSuccessDataResponseModel {
        data {
          uuid
          setId
          setCode
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

**Run codegen:** `npm run codegen`

**Acceptance Criteria:**
- [ ] Query matches GraphQL schema
- [ ] Codegen generates types successfully
- [ ] Generated hooks available

---

### Task 4.3: Create useSealedProductsData Hook

**Location:** `client/src/hooks/`

**File to Create:** `useSealedProductsData.ts`

```typescript
import { useState, useEffect } from 'react';
import { useApolloClient } from '@apollo/client';
import { GET_SEALED_PRODUCTS_BY_SET_CODE } from '../graphql/queries/sealedProducts';
import type { SealedProduct } from '../types/sealedProduct';

export const useSealedProductsData = (setCode: string, isActive: boolean) => {
  const apolloClient = useApolloClient();
  const [sealedProducts, setSealedProducts] = useState<SealedProduct[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<Error | null>(null);

  useEffect(() => {
    if (!isActive || !setCode) return;

    const fetchSealedProducts = async () => {
      setLoading(true);
      setError(null);
      try {
        const response = await apolloClient.query({
          query: GET_SEALED_PRODUCTS_BY_SET_CODE,
          variables: { setCode: { setCode } },
          fetchPolicy: 'cache-first'
        });

        const data = response.data?.sealedProductsBySetCode;
        if (data?.__typename === 'SealedProductsSuccessDataResponseModel') {
          setSealedProducts(data.data || []);
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
- [ ] Only fetches when `isActive` is true (lazy loading)
- [ ] Handles loading and error states
- [ ] Uses cache-first fetch policy

---

### Task 4.4: Create SealedProductCard Component

**Location:** `client/src/components/atoms/Sealed/`

**File to Create:** `SealedProductCard.tsx`

```typescript
import React from 'react';
import { Box, Typography, Chip } from '@mui/material';
import type { SealedProduct } from '../../../types/sealedProduct';

interface SealedProductCardProps {
  product: SealedProduct;
  onProductClick?: (product: SealedProduct) => void;
}

export const SealedProductCard: React.FC<SealedProductCardProps> = ({
  product,
  onProductClick
}) => {
  const categoryLabel = product.category?.replace('_', ' ').toUpperCase();

  return (
    <Box
      sx={{
        bgcolor: 'grey.900',
        borderRadius: 2,
        overflow: 'hidden',
        cursor: onProductClick ? 'pointer' : 'default',
        '&:hover': {
          transform: 'translateY(-4px)',
          boxShadow: 4
        },
        transition: 'transform 0.2s, box-shadow 0.2s'
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
            objectFit: 'cover'
          }}
        />
      </Box>

      {/* Product Info */}
      <Box sx={{ p: 2 }}>
        <Typography variant="subtitle2" noWrap>
          {product.name}
        </Typography>
        {categoryLabel && (
          <Chip
            label={categoryLabel}
            size="small"
            sx={{ mt: 1 }}
          />
        )}
        {product.cardCount && (
          <Typography variant="caption" color="text.secondary" display="block">
            {product.cardCount} cards
          </Typography>
        )}
      </Box>
    </Box>
  );
};
```

**Acceptance Criteria:**
- [ ] Displays product image from TCGPlayer CDN
- [ ] Shows category badge
- [ ] Handles missing images gracefully
- [ ] Follows existing card display patterns

---

### Task 4.5: Create SealedProductGrid Component

**Location:** `client/src/components/organisms/Sealed/`

**File to Create:** `SealedProductGrid.tsx`

**Reference:** Copy pattern from `CardGrid.tsx`

**Acceptance Criteria:**
- [ ] Uses ResponsiveGridAutoFit layout
- [ ] Displays loading state
- [ ] Handles empty state
- [ ] Responsive on mobile

---

### Task 4.6: Integrate Sealed Tab into SetPage

**Location:** `client/src/components/pages/SetPage.tsx`

**Modifications:**

1. Add tab state:
```typescript
const [activeTab, setActiveTab] = useState<'cards' | 'sealed'>('cards');
```

2. Add ToggleButtonGroup:
```typescript
import { ToggleButtonGroup, ToggleButton } from '../../atoms';

<ToggleButtonGroup
  value={activeTab}
  onChange={(_, value) => value && setActiveTab(value)}
  exclusive
>
  <ToggleButton value="cards">Cards ({cards.length})</ToggleButton>
  <ToggleButton value="sealed">Sealed ({sealedProducts.length})</ToggleButton>
</ToggleButtonGroup>
```

3. Conditional rendering:
```typescript
{activeTab === 'cards' && <SetPageCardDisplay ... />}
{activeTab === 'sealed' && <SealedProductGrid products={sealedProducts} loading={sealedLoading} />}
```

4. Add sealed products hook:
```typescript
const { sealedProducts, loading: sealedLoading } = useSealedProductsData(
  setCode,
  activeTab === 'sealed'  // Only fetch when tab active
);
```

**Acceptance Criteria:**
- [ ] Tab toggle works correctly
- [ ] Sealed products load only when tab is active
- [ ] Tab shows count after data loads
- [ ] Works on mobile

---

## Phase 5: User Collection (Optional/Future)

**Goal:** Allow users to add/remove sealed products from their collection

**Dependencies:** Phase 4 complete

### Task 5.1: Backend Mutations

- Create `Lib.Adapter.UserSealedProducts` project
- Create `Lib.Aggregator.UserSealedProducts` project
- Create `Lib.Domain.UserSealedProducts` project
- Update Entry layer with commands
- Create GraphQL mutations

### Task 5.2: Frontend Collection UI

- Add mutation hooks
- Add add/remove buttons to SealedProductCard
- Show collection count

---

## Verification Checklist

### Backend
- [ ] `dotnet build src/MtgDiscoveryVibe.sln` succeeds
- [ ] `dotnet test src/MtgDiscoveryVibe.sln` passes
- [ ] GraphQL query returns data in Playground

### Data Ingestion
- [ ] CLI imports MKM sealed products
- [ ] Data visible in Cosmos DB
- [ ] TCGPlayer image URLs load correctly

### Frontend
- [ ] `npm run codegen` succeeds
- [ ] `npm run build` succeeds
- [ ] Sealed tab appears on set page
- [ ] Products display with images
- [ ] Tab switching works
- [ ] Mobile responsive

---

## Reference Files

| Pattern | Reference File |
|---------|---------------|
| SetCode→SetId Lookup | `src/Lib.Adapter.Cards/Queries/CardsBySetCodeAdapter.cs` |
| Container Definition | `src/Lib.Adapter.Scryfall.Cosmos/Cosmos/Containers/Definitions/UserWishlistCardsCosmosContainerDefinition.cs` |
| Adapter Exception | `src/Lib.Adapter.UserWishlistCards/Exceptions/UserWishlistCardsAdapterException.cs` |
| GraphQL Query | `src/App.MtgDiscovery.GraphQL/Queries/UserWishlistCardsQueryMethods.cs` |
| Card Display Component | `client/src/components/organisms/Cards/CardCompact.tsx` |
| Data Hook Pattern | `client/src/hooks/useSetPageData.ts` |

---

## Data Source Reference

**MTGJSON API:** `https://mtgjson.com/api/v5/{SET_CODE}.json`

**TCGPlayer Image URL:** `https://tcgplayer-cdn.tcgplayer.com/product/{tcgplayerProductId}_in_400x400.jpg`

**Category Values:**
- `booster_pack` - Single booster pack
- `booster_box` - Box containing multiple booster packs
- `booster_case` - Case containing multiple booster boxes
- `bundle` - Collection with cards and accessories
- `bundle_case` - Case containing multiple bundles
- `box_set` - Complete set (MTGO redemption)
- `limited_aid_tool` - Limited format product (prerelease kits)

**IMPORTANT:** MTGJSON does NOT include Scryfall set IDs. Use `SetCodeToIdAssociations` container to lookup setId from setCode.
