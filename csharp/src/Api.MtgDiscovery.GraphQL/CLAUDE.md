# GraphQL API Layer

## Purpose
Thin entry point that translates HTTP requests to Entry service calls and returns type-safe responses. Handles authentication, authorization, and schema definition.

## Architecture

**App → Entry → Domain → Aggregators → Adapters**

- **App.MtgDiscovery.GraphQL**: HTTP → ArgEntity → IEntryService → ResponseModel → HTTP
- **Lib.MtgDiscovery.Entry**: Validates, maps entities, calls Domain, returns IOperationResponse<OutEntity>

**Query/Mutation Pattern**: Extend `ApiQuery`/`ApiMutation` marker classes using `[ExtendObjectType]`. See: `Queries/CardQueryMethods.cs`, `Mutations/UserCardsMutationMethods.cs`

## Key Files

- `AppMtgDiscoveryGraphQlProgram.cs` — Entry point, DI
- `Startup.cs` — HotChocolate setup, Auth0 JWT, error filters
- `Queries/ApiQuery.cs`, `Mutations/ApiMutation.cs` — Marker classes
- `Entities/Args/` — Input entities (ArgEntity)
- `Entities/Types/ResponseModels/` — Union response types
- `Entities/Types/Schemas/` — Schema extensions (SetSchemaExtensions.cs, etc.)
- `Lib.MtgDiscovery.Entry/Apis/IEntryService.cs` — Main service composition

## Authentication

**JWT Token** (Auth0):
- Mark mutations with `[Authorize]`
- HotChocolate validates token and injects `ClaimsPrincipal`
- Extract user ID via `AuthUserArgEntity`
- Errors: 401 (invalid token), 403 (missing authorization)

See: `Mutations/UserCardsMutationMethods.cs:RegisterUserInfoAsync()`, `ErrorHandling/HttpStatusCodeErrorFilter.cs`

## Response Pattern

All operations return union types (success | failure):
- Success: `SuccessDataResponseModel<T>` with data + status
- Failure: `FailureResponseModel` with status + error info

Mapper: `OperationResponseToResponseModelMapper<T>` converts `IOperationResponse<OutEntity>` → `ResponseModel`

See: `Entities/Types/ResponseModels/`, `Actions/Mappers/OperationResponseToResponseModelMapper.cs`

## Adding Queries/Mutations

1. Create method class: Extend `ApiQuery` or `ApiMutation` with `[ExtendObjectType]`
2. Define input: Create `ArgEntity` implementing `IArgEntity`
3. Create Entry service: Validate → map → call Domain → return `IOperationResponse<OutEntity>`
4. Register: Add to `Lib.MtgDiscovery.Entry/Apis/IEntryService.cs`
5. Schema: Register schema extensions if new domain type

See: `Queries/CardQueryMethods.cs` (complete query example), `Lib.MtgDiscovery.Entry/Queries/Cards/CardsByIdsEntryService.cs` (Entry service example)

## Key Principle

This layer is **intentionally thin** — translate GraphQL → Entry → translate response. Business logic belongs in Entry/Domain, not here.
