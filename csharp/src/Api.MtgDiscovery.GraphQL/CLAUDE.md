# GraphQL API Layer

## Purpose
Thin GraphQL entry point that translates HTTP requests into Entry service calls and returns type-safe responses. Handles authentication, authorization, type mapping, and schema definition.

## Projects

### App.MtgDiscovery.GraphQL
The ASP.NET Core GraphQL application with HotChocolate schema and request handling.

**Key Files**:
- `AppMtgDiscoveryGraphQlProgram.cs` - Entry point, DI configuration
- `Startup.cs` - HotChocolate server setup, Auth0 JWT, schema extensions, error handling
- `Queries/ApiQuery.cs` - Marker class extended by query methods
- `Mutations/ApiMutation.cs` - Marker class extended by mutation methods
- `Entities/Args/` - Input argument entities (ArgEntity types from GraphQL)
- `Entities/Types/ResponseModels/` - Union types for success/failure responses
- `Entities/Types/` - HotChocolate type configurations for schema
- `Actions/Mappers/` - Mappers between GraphQL args and Entry args
- `Authentication/AuthUserArgEntity.cs` - Extracts user ID from JWT claims

**Structure**:
Query/mutation methods use `[ExtendObjectType(typeof(ApiQuery/Mutation))]` pattern to extend marker classes. See: `Queries/CardQueryMethods.cs`, `Queries/ArtistQueryMethods.cs`, `Mutations/UserCardsMutationMethods.cs`

### Lib.MtgDiscovery.Entry
Orchestration layer between GraphQL and Domain layers. Handles validation, mapping, and service composition.

**Public API**:
- `Apis/IEntryService.cs` - Composition interface with all query/command services
- `Apis/EntryService.cs` - Delegates to specialized entry services

**Structure**:
- `Queries/` - Query entry services (CardEntryService, ArtistEntryService, SetEntryService, etc.)
- `Commands/` - Command entry services (UserCardsEntryService, CollectionsEntryService, etc.)
- `Entities/Ins/` - Input entities (ArgEntity and ItrEntity types)
- `Entities/Outs/` - Output entities (OutEntity types returned to App layer)
- `Actions/` - Validators, mappers, and enrichments for queries/commands

See: `Lib.MtgDiscovery.Entry/Apis/IEntryService.cs` for complete service interface

## Request/Response Flow

**Entity Type Transformation Pipeline**:
```
GraphQL Args (ArgEntity)
    ↓ App Mapper (if needed)
Entry Service receives args
    ↓ Validator Action
    ↓ Entry Mapper (ArgEntity → ItrEntity)
Domain Service processes
    ↓ Domain returns OufEntity
    ↓ Entry Mapper (OufEntity → OutEntity)
    ↓ Enrichment Action (add async data)
Entry Service returns IOperationResponse<OutEntity>
    ↓ App Response Mapper (IOperationResponse → ResponseModel)
GraphQL Response (union type: success | failure)
```

See specific flows:
- Query example: `Queries/CardQueryMethods.cs:CardsById()` → `Lib.MtgDiscovery.Entry/Queries/Cards/CardsByIdsEntryService.cs:Execute()`
- Mutation example: `Mutations/UserCardsMutationMethods.cs:AddCardToCollectionAsync()` → `Lib.MtgDiscovery.Entry/Commands/UserCards/AddCardToCollectionEntryService.cs`

## GraphQL Schema Organization

**Extension Pattern**:
Schema is built modularly via extension classes. Each domain concern registers its types and fields through schema extensions. See: `Entities/Types/Schemas/SetSchemaExtensions.cs`, `Entities/Types/Schemas/ArtistSchemaExtensions.cs`

**Schema Registration**: See `Startup.cs` - registers API query/mutation markers and all domain schema extensions. Each schema extension registers custom ObjectType configurations (field names, descriptions, nullability). See: `Entities/Types/Cards/ScryfallCardOutEntityType.cs` for example with 360+ field definitions, and `Entities/Types/Schemas/SetSchemaExtensions.cs` for pattern of how extensions combine types.

**HotChocolate Configuration** (Startup.cs):
- Auth0 JWT validation (issuer, audience, signing key)
- `[Authorize]` directive support for protected mutations
- `HttpStatusCodeErrorFilter` for mapping GraphQL errors to HTTP status codes
- Introspection disabled in production
- `@defer` directive enabled for deferred fields

## Authentication & Authorization

**JWT Token**:
Auth0 tokens validated on every request. Token must include:
- `sub` claim (user ID) - mapped to deterministic GUID for internal use
- Custom `profile_name` and `email` claims (from Auth0 rules)

**Protected Mutations**:
Mark mutations with `[Authorize]` attribute. HotChocolate automatically:
1. Validates JWT token
2. Injects `ClaimsPrincipal` as method parameter
3. Extracts claims via `AuthUserArgEntity`

See: `Mutations/UserCardsMutationMethods.cs:RegisterUserInfoAsync()` - uses `[Authorize]` decorator

**Error Mapping**:
- Invalid/missing token → `HttpStatusCode.Unauthorized` (401)
- Valid token but missing authorization → `HttpStatusCode.Forbidden` (403)
- See: `ErrorHandling/HttpStatusCodeErrorFilter.cs`

## Response Patterns

**Union Response Types**:
All operations return union types containing success or failure variants:
- Success: `SuccessDataResponseModel<T>` with data + status + metadata
- Failure: `FailureResponseModel` with status + metadata

Examples: `CardResponseModelUnionType`, `ArtistSearchResponseModelUnionType`, `CollectionResponseModelUnionType`

See: `Entities/Types/ResponseModels/` for all union types, `Entities/Types/ResponseModels/Common/` for wrapper models

**Response Mapping**:
`OperationResponseToResponseModelMapper<T>` converts `IOperationResponse<OutEntity>` to GraphQL `ResponseModel`. See: `Actions/Mappers/OperationResponseToResponseModelMapper.cs`

## Adding New Queries

1. Create query method class extending `ApiQuery`: See `Queries/CardQueryMethods.cs` pattern
   - Use `[ExtendObjectType(typeof(ApiQuery))]`
   - Use `[GraphQLType(typeof(UnionType))]` for response type
   - Mark with `[Authorize]` if authentication required

2. Define ArgEntity (input type): See `Entities/Args/CardIdsArgEntity.cs`
   - Implement `IArgEntity` interface
   - Include userId if user-specific

3. Create Entry service: See `Lib.MtgDiscovery.Entry/Queries/Cards/CardsByIdsEntryService.cs:Execute()`
   - Validate using validator actions
   - Map ArgEntity → ItrEntity
   - Call Domain service
   - Map OufEntity → OutEntity
   - Apply enrichments
   - Return `IOperationResponse<OutEntity>`

4. Register in Entry service interface: See `Lib.MtgDiscovery.Entry/Apis/IEntryService.cs`

5. Register schema extensions if new domain: See `Entities/Types/Schemas/SetSchemaExtensions.cs` pattern

## Adding New Mutations

Same as queries, but:
- Extend `ApiMutation` instead of `ApiQuery`
- Mark with `[Authorize]` if user action required
- Entry service is in `Lib.MtgDiscovery.Entry/Commands/` not `Queries/`
- Extract user ID from `ClaimsPrincipal` via `AuthUserArgEntity`

See: `Mutations/UserCardsMutationMethods.cs` and `Lib.MtgDiscovery.Entry/Commands/UserCards/AddCardToCollectionEntryService.cs`

## Key Principle

This layer is **intentionally thin** - translate GraphQL → Entry service → translate response. All business logic belongs in Entry layer or Domain layers, not here. Schema extensions keep related types together by domain concern.
