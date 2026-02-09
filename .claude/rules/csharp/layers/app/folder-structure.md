---
paths:
  - "csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/**"
---

# App Layer Folder Structure

## Directory Hierarchy

```
App.MtgDiscovery.GraphQL/
├── Actions/
│   └── Mappers/                          # OperationResponse → ResponseModel mappers
│       └── Collections/                  # Collection-specific array mappers
├── Authentication/                       # AuthUserArgEntity (JWT → typed entity)
├── Entities/
│   ├── Args/                             # ArgEntity classes (GraphQL input data)
│   │   ├── Collections/
│   │   ├── SealedProducts/
│   │   ├── UserCards/
│   │   ├── UserSealedProducts/
│   │   ├── UserSetCards/
│   │   └── UserWishlistCards/
│   └── Types/                            # HotChocolate type descriptors
│       ├── Args/                         # InputObjectType<ArgEntity> descriptors
│       │   ├── Collections/
│       │   ├── SealedProducts/
│       │   ├── UserCards/
│       │   ├── UserSealedProducts/
│       │   ├── UserSetCards/
│       │   └── UserWishlistCards/
│       ├── Artists/                      # ObjectType descriptors for artist output
│       ├── Cards/                        # ObjectType descriptors for card output
│       ├── Collections/                  # ObjectType descriptors for collection output
│       ├── ResponseModels/               # UnionType + SuccessDataResponseModelType
│       ├── SealedProducts/
│       ├── Sets/
│       ├── Signing/
│       ├── User/
│       ├── UserCards/
│       ├── UserSetCards/
│       └── UserWishlistCards/
├── ErrorHandling/                        # IErrorFilter implementation
├── Mutations/                            # Mutation method classes (ExtendObjectType)
├── Queries/                              # Query method classes (ExtendObjectType)
└── Schemas/                              # Schema registration extensions
```

## Folder Purposes

| Folder | Contains | Example |
|--------|----------|---------|
| `Actions/Mappers/` | `IOperationResponseToResponseModelMapper<T>` instances | `OperationResponseToResponseModelMapper.cs` |
| `Actions/Mappers/Collections/` | Collection-specific ArgsMappers (ClaimsPrincipal + ArgEntity → combined args) | `CreateCollectionArgsMapper.cs` |
| `Authentication/` | JWT claim extraction | `AuthUserArgEntity.cs` |
| `Entities/Args/` | C# input classes implementing Entry-layer interfaces | `CardIdsArgEntity.cs` |
| `Entities/Types/Args/` | `InputObjectType<ArgEntity>` descriptors for GraphQL schema | `CardIdsArgEntityInputType.cs` |
| `Entities/Types/{Domain}/` | `ObjectType<OutEntity>` descriptors for GraphQL output | `ScryfallCardOutEntityType.cs` |
| `Entities/Types/ResponseModels/` | `UnionType` + `SuccessDataResponseModelType` per endpoint | `CardResponseModelUnionType.cs` |
| `ErrorHandling/` | `IErrorFilter` implementation | `HttpStatusCodeErrorFilter.cs` |
| `Mutations/` | `[ExtendObjectType(typeof(ApiMutation))]` classes | `UserCardsMutationMethods.cs` |
| `Queries/` | `[ExtendObjectType(typeof(ApiQuery))]` classes | `CardQueryMethods.cs` |
| `Schemas/` | `IRequestExecutorBuilder` extension methods | `ApiQueryExtensions.cs` |

## Key Relationships

```
Entities/Args/CardIdsArgEntity.cs          ← C# class (implements ICardIdsArgEntity)
    ↕ paired with
Entities/Types/Args/CardIdsArgEntityInputType.cs  ← GraphQL descriptor

Queries/CardQueryMethods.cs                ← Uses ArgEntity as method parameter
    ↕ registered in
Schemas/ApiQueryExtensions.cs              ← .AddTypeExtension<CardQueryMethods>()
                                              .AddType<CardIdsArgEntityInputType>()
```

## Conventions

- **Args subfolders mirror domain**: `Args/UserCards/`, `Args/Collections/`, etc.
- **Types/Args mirrors Entities/Args**: Every `ArgEntity` has a matching `InputType` in the parallel `Types/Args/` folder
- **Types/{Domain} folders hold output descriptors**: One folder per domain (Cards, Sets, Artists, etc.)
- **ResponseModels is flat**: All union types, success types, and the shared `FailureResponseModelType` live in one folder
- **Mutations and Queries are flat**: One file per domain, named `{Domain}QueryMethods.cs` or `{Domain}MutationMethods.cs`

## Reference Files

- **Folder structure**: Browse `App.MtgDiscovery.GraphQL/` directly
- **ArgEntity pairing**: `Entities/Args/CardIdsArgEntity.cs` + `Entities/Types/Args/CardIdsArgEntityInputType.cs`
- **Complete query**: `Queries/CardQueryMethods.cs`
- **Complete mutation**: `Mutations/UserCardsMutationMethods.cs`
