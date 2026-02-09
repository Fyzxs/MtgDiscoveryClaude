---
paths:
  - "csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Entities/Types/ResponseModels/**"
  - "csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Actions/Mappers/**"
---

# GraphQL Response Model Pattern

## Purpose

ResponseModels provide a **type-safe union response** for all GraphQL endpoints. Every query and mutation returns a `ResponseModel` that is either a success (with data) or failure (with status).

## Response Type Hierarchy

```
ResponseModel (abstract base)
├── SuccessDataResponseModel<TData>    (success branch — carries response data)
└── FailureResponseModel               (failure branch — carries error status)
    └── StatusDataModel                    (error code + message)
```

## Mapper: IOperationResponse → ResponseModel

A single generic mapper converts `IOperationResponse<T>` from the Entry layer into `ResponseModel` for GraphQL:

```csharp
// Interface
internal interface IOperationResponseToResponseModelMapper<TData>
    : ICreateMapper<IOperationResponse<TData>, ResponseModel>;

// Implementation
internal sealed class OperationResponseToResponseModelMapper<TData>
    : IOperationResponseToResponseModelMapper<TData>
{
    public Task<ResponseModel> Map(IOperationResponse<TData> source)
    {
        if (source.IsSuccess)
            return Task.FromResult(
                (ResponseModel)new SuccessDataResponseModel<TData>
                {
                    Data = source.ResponseData
                });

        return Task.FromResult(
            (ResponseModel)new FailureResponseModel
            {
                Status = new StatusDataModel
                {
                    Message = source.OuterException.StatusMessage,
                    StatusCode = source.OuterException.StatusCode
                }
            });
    }
}
```

**Key points:**
- Generic — one mapper handles all data types
- No business logic — pure type conversion
- Cast to `ResponseModel` base type for union resolution

## HotChocolate Type Registration

Each response union requires three HotChocolate types registered in the schema:

### 1. UnionType

```csharp
internal class CardResponseModelUnionType : UnionType
{
    protected override void Configure([NotNull] IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("CardResponse")
            .Description("Union type for card query response")
            .Type<CardsSuccessDataResponseModelType>()
            .Type<FailureResponseModelType>();
    }
}
```

### 2. SuccessDataResponseModelType

```csharp
internal class CardsSuccessDataResponseModelType
    : ObjectType<SuccessDataResponseModel<List<CardItemOutEntity>>>
{
    protected override void Configure(
        [NotNull] IObjectTypeDescriptor<SuccessDataResponseModel<List<CardItemOutEntity>>> descriptor)
    {
        descriptor.Name("CardsSuccessResponse")
            .Description("Response returned when cards are successfully retrieved");

        descriptor.Field(f => f.Data)
            .Name("data")
            .Type<ListType<ScryfallCardOutEntityType>>()
            .Description("The list of cards retrieved");
        descriptor.Field(f => f.Status)
            .Name("status")
            .Type<StatusDataModelType>()
            .Description("Status information about the success");
        descriptor.Field(f => f.MetaData)
            .Name("metaData")
            .Type<MetaDataModelType>()
            .Description("Metadata about the response");
    }
}
```

### 3. FailureResponseModelType (shared)

A single `FailureResponseModelType` is shared across all union types:

```csharp
internal class FailureResponseModelType : ObjectType<FailureResponseModel>
{
    protected override void Configure(
        [NotNull] IObjectTypeDescriptor<FailureResponseModel> descriptor)
    {
        descriptor.Name("FailureResponse")
            .Description("Response returned when the query fails");

        descriptor.Field(f => f.Status)
            .Name("status")
            .Type<StatusDataModelType>()
            .Description("Status information about the failure");
        descriptor.Field(f => f.MetaData)
            .Name("metaData")
            .Type<MetaDataModelType>()
            .Description("Metadata about the response");
    }
}
```

## Naming Conventions

| Type | Pattern | Example |
|------|---------|---------|
| Union type | `{Feature}ResponseModelUnionType` | `CardResponseModelUnionType` |
| Success type | `{Feature}SuccessDataResponseModelType` | `CardsSuccessDataResponseModelType` |
| Failure type | `FailureResponseModelType` | (shared singleton) |
| GraphQL name | `"{Feature}Response"` | `"CardResponse"` |
| Success name | `"{Feature}SuccessResponse"` | `"CardsSuccessResponse"` |

## Key Rules

1. **Every descriptor MUST have `Name` set** — HotChocolate types without explicit names can cause schema conflicts
2. **Every descriptor MUST have `Description` set** — provides API documentation
3. All field names use **camelCase** in the GraphQL schema
4. `FailureResponseModelType` and its supporting types (`StatusDataModelType`, `MetaDataModelType`) are registered once and shared
5. Union types always include exactly two branches: the success type and `FailureResponseModelType`
6. **All type descriptors MUST be `internal sealed class`** — UnionTypes, SuccessDataResponseModelTypes, and supporting types like `StatusDataModelType` and `MetaDataModelType` are never public or abstract

## Schema Registration

All response types must be registered in either `ApiQueryExtensions.cs` or `ApiMutationExtensions.cs`:

```csharp
// In ApiQueryExtensions.AddApiQuery():
.AddType<CardResponseModelUnionType>()
.AddType<CardsSuccessDataResponseModelType>()
.AddType<FailureResponseModelType>()
.AddType<StatusDataModelType>()
.AddType<MetaDataModelType>()
```

## Usage in Query/Mutation Methods

```csharp
[GraphQLType(typeof(CardResponseModelUnionType))]
public async Task<ResponseModel> CardsById(
    CardIdsArgEntity ids, CancellationToken cancellationToken)
{
    IOperationResponse<List<CardItemOutEntity>> response =
        await _entryService.CardsByIdsAsync(ids, cancellationToken).ConfigureAwait(false);
    return await _cardResponseMapper.Map(response).ConfigureAwait(false);
}
```

The `[GraphQLType]` decorator tells HotChocolate which union type resolves this method's return.

## Multiple Typed Response Mappers

Query and mutation classes that handle multiple response shapes require multiple `IOperationResponseToResponseModelMapper<T>` instances, each parameterized by the specific response type:

```csharp
private readonly IOperationResponseToResponseModelMapper<List<CollectionOutEntity>> _listResponseMapper;
private readonly IOperationResponseToResponseModelMapper<CollectionOutEntity> _singleResponseMapper;
private readonly IOperationResponseToResponseModelMapper<IEnumerable<AuthorizedUserOutEntity>> _authorizedUsersResponseMapper;
```

**When multiple mappers are needed:**
- Different endpoints return different data types (e.g., list vs. single entity)
- The same mutation class handles operations with different response shapes
- `IEnumerable<T>` is used when the Entry service returns a lazy sequence instead of `List<T>`

Each mapper is constructed in the public constructor and assigned in the private constructor following the standard constructor chain pattern. Choose the generic parameter to match the Entry service's `IOperationResponse<T>` return type exactly.

## Single-to-List Response Conversion

When a mutation returns a single entity but the GraphQL response type expects a list (e.g., to share a `UnionType` across create/list endpoints), use a private conversion helper:

```csharp
private Task<ResponseModel> MapSingleToList(IOperationResponse<CollectionOutEntity> response)
{
    if (response.IsSuccess)
    {
        return _responseMapper.Map(new SuccessOperationResponse<List<CollectionOutEntity>>([response.ResponseData]));
    }

    return _responseMapper.Map(new FailureOperationResponse<List<CollectionOutEntity>>(response.OuterException));
}
```

**When to use:**
- A mutation returns `IOperationResponse<TEntity>` (single) but the `UnionType` is configured for `List<TEntity>`
- Avoids creating a separate `UnionType` for single vs. list responses

**Key points:**
- The helper wraps the single entity in a collection expression `[response.ResponseData]`
- Failure responses are re-wrapped to match the `List<T>` type parameter
- Lives as a `private` method on the mutation class — not a shared utility

## ObjectType vs InputObjectType

HotChocolate uses two distinct descriptor base classes for GraphQL types:

| Base Class | Purpose | Location | Documented In |
|------------|---------|----------|---------------|
| `InputObjectType<T>` | Describes **input** types (query/mutation parameters) | `Entities/Types/Args/` | `layers/app/input-types.md` |
| `ObjectType<T>` | Describes **output** types (response fields) | `Entities/Types/{Domain}/` and `Entities/Types/ResponseModels/` | This file |

**Input types** map `ArgEntity` → GraphQL input fields. **Object types** map `OutEntity` and `ResponseModel` → GraphQL output fields. Never mix them — a type is either input or output, never both.

## File Location

| Type | Location |
|------|----------|
| Mapper interface | `Actions/Mappers/IOperationResponseToResponseModelMapper.cs` |
| Mapper implementation | `Actions/Mappers/OperationResponseToResponseModelMapper.cs` |
| Union types | `Entities/Types/ResponseModels/` |
| Success types | `Entities/Types/ResponseModels/` |
| Failure type | `Entities/Types/ResponseModels/FailureResponseModelType.cs` |

## Reference Implementations

- **Mapper**: `Actions/Mappers/OperationResponseToResponseModelMapper.cs`
- **Union type**: `Entities/Types/ResponseModels/CardResponseModelUnionType.cs`
- **Success type**: `Entities/Types/ResponseModels/CardsSuccessDataResponseModelType.cs`
- **Schema registration**: `Schemas/ApiQueryExtensions.cs`
