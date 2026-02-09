---
paths:
  - "csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Schemas/**"
---

# Schema Extension Pattern

## Purpose

Schema extensions centralize HotChocolate type registration using `IRequestExecutorBuilder` extension methods. Each extension method registers a group of related type extensions, input types, and output types.

## Files

| File | Registers |
|------|-----------|
| `ApiQueryExtensions.cs` | Root query type + all query extensions + query input/output types |
| `ApiMutationExtensions.cs` | Root mutation type + all mutation extensions + mutation input/output types |
| `SetSchemaExtensions.cs` | Set-specific output types |
| `ArtistSchemaExtensions.cs` | Artist-specific output types |
| `SealedProductsSchemaExtensions.cs` | Sealed product output types |

## Implementation Pattern

```csharp
internal static class ApiQueryExtensions
{
    public static IRequestExecutorBuilder AddApiQuery(this IRequestExecutorBuilder builder)
    {
        return builder
            // 1. Root type
            .AddQueryType<ApiQuery>()

            // 2. Type extensions (method classes)
            .AddTypeExtension<CardQueryMethods>()
            .AddTypeExtension<SetQueryMethods>()

            // 3. Input types (ArgEntity descriptors)
            .AddType<CardIdsArgEntityInputType>()
            .AddType<CardNameArgEntityInputType>()

            // 4. Response union types
            .AddType<CardResponseModelUnionType>()
            .AddType<CardsSuccessDataResponseModelType>()
            .AddType<FailureResponseModelType>()

            // 5. Output entity types
            .AddType<ScryfallCardOutEntityType>()
            .AddType<StatusDataModelType>()
            .AddType<MetaDataModelType>()

            // 6. Runtime options
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);
    }
}
```

## Registration Order

Within a schema extension method, register in this order:

1. `.AddQueryType<>()` or `.AddMutationType<>()` -- root type (only in Api extensions)
2. `.AddTypeExtension<>()` -- method classes that extend the root type
3. `.AddType<>()` -- input types (`InputObjectType` descriptors)
4. `.AddType<>()` -- response model types (union + success + failure)
5. `.AddType<>()` -- output entity types (`ObjectType` descriptors)
6. `.ModifyRequestOptions()` -- runtime configuration

## Startup Integration

All schema extensions are chained in `Startup.ConfigureServices()`:

```csharp
_ = services
    .AddGraphQLServer()
    .AddApiQuery()              // Queries + query types
    .AddApiMutation()           // Mutations + mutation types
    .AddSetSchemaExtensions()   // Set output types
    .AddArtistSchemaExtensions()
    .AddSealedProductsSchemaExtensions()
    .AddAuthorization()
    .AddErrorFilter<HttpStatusCodeErrorFilter>()
    // ... more configuration
```

## Domain-Specific Extensions

Domain extensions (`SetSchemaExtensions`, `ArtistSchemaExtensions`, etc.) register **output types only** that are specific to that domain. They do NOT register input types or type extensions -- those belong in `ApiQueryExtensions` or `ApiMutationExtensions`.

```csharp
internal static class SetSchemaExtensions
{
    public static IRequestExecutorBuilder AddSetSchemaExtensions(this IRequestExecutorBuilder builder)
    {
        return builder
            .AddTypeExtension<SetQueryMethods>()
            .AddType<ScryfallSetOutEntityType>()
            .AddType<SetGroupingOutEntityType>()
            .AddType<SetGroupingFinishCountsType>();
    }
}
```

## Key Rules

1. **Every type used in GraphQL must be registered** -- unregistered types cause runtime schema errors
2. **`FailureResponseModelType`, `StatusDataModelType`, `MetaDataModelType`** are shared types -- registered in both query and mutation extensions
3. **Use comments to group registrations** by domain (e.g., `// Input types for queries - Cards`)
4. **One extension method per concern** -- queries, mutations, and domain-specific types each get their own

## Reference Files

- **Query registration**: `Schemas/ApiQueryExtensions.cs`
- **Mutation registration**: `Schemas/ApiMutationExtensions.cs`
- **Domain extension**: `Schemas/SetSchemaExtensions.cs`
- **Startup wiring**: `Startup.cs` (`ConfigureServices` method)
