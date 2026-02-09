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

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Schemas/ApiQueryExtensions.cs`

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

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Startup.cs`

## Domain-Specific Extensions

Domain extensions (`SetSchemaExtensions`, `ArtistSchemaExtensions`, etc.) register **output types only** that are specific to that domain. They do NOT register input types or type extensions -- those belong in `ApiQueryExtensions` or `ApiMutationExtensions`.

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Schemas/SetSchemaExtensions.cs`

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
