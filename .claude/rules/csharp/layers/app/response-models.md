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

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Actions/Mappers/OperationResponseToResponseModelMapper.cs`

**Key points:**
- Generic — one mapper handles all data types
- No business logic — pure type conversion
- Cast to `ResponseModel` base type for union resolution

## HotChocolate Type Registration

Each response union requires three HotChocolate types registered in the schema:

### 1. UnionType

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Entities/Types/ResponseModels/CardResponseModelUnionType.cs`

### 2. SuccessDataResponseModelType

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Entities/Types/ResponseModels/CardsSuccessDataResponseModelType.cs`

### 3. FailureResponseModelType (shared)

A single `FailureResponseModelType` is shared across all union types:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Entities/Types/ResponseModels/FailureResponseModelType.cs`

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

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Schemas/ApiQueryExtensions.cs`

## Usage in Query/Mutation Methods

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Queries/CardQueryMethods.cs`

The `[GraphQLType]` decorator tells HotChocolate which union type resolves this method's return.

## Multiple Typed Response Mappers

Query and mutation classes that handle multiple response shapes require multiple `IOperationResponseToResponseModelMapper<T>` instances, each parameterized by the specific response type:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Mutations/` (mutation classes with multiple response mappers)

**When multiple mappers are needed:**
- Different endpoints return different data types (e.g., list vs. single entity)
- The same mutation class handles operations with different response shapes
- `IEnumerable<T>` is used when the Entry service returns a lazy sequence instead of `List<T>`

Each mapper is constructed in the public constructor and assigned in the private constructor following the standard constructor chain pattern. Choose the generic parameter to match the Entry service's `IOperationResponse<T>` return type exactly.

## Single-to-List Response Conversion

When a mutation returns a single entity but the GraphQL response type expects a list (e.g., to share a `UnionType` across create/list endpoints), use a private conversion helper:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Mutations/` (mutation classes with `MapSingleToList` pattern)

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
