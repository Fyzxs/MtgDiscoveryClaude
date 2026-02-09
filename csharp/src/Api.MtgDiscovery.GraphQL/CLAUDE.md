# GraphQL API Layer

## Purpose
Thin entry point that translates HTTP requests to Entry service calls and returns type-safe responses. Handles authentication, authorization, and schema definition.

## Detailed Documentation

See `.claude/rules/csharp/graphql-conventions.md` for the full implementation checklist covering:
- Request/Response flow, ArgEntity → EntryService → ResponseModel
- Query/Mutation method patterns with examples
- Type registration (Schemas)
- Key rules and real examples to copy from

## Key Files

- `AppMtgDiscoveryGraphQlProgram.cs` — Entry point, DI
- `Startup.cs` — HotChocolate setup, Auth0 JWT, error filters
- `Queries/ApiQuery.cs`, `Mutations/ApiMutation.cs` — Marker classes
- `Entities/Args/` — Input entities (ArgEntity)
- `Entities/Types/ResponseModels/` — Union response types
- `Schemas/` — Schema extensions (ApiQueryExtensions.cs, SetSchemaExtensions.cs, etc.)
- `Lib.MtgDiscovery.Entry/Apis/IEntryService.cs` — Main service composition

## Authentication

**JWT Token** (Auth0):
- Mark mutations with `[Authorize]`
- HotChocolate validates token and injects `ClaimsPrincipal`
- Extract user ID via `AuthUserArgEntity`
- Errors: 401 (invalid token), 403 (missing authorization)

See: `Mutations/UserCardsMutationMethods.cs:RegisterUserInfoAsync()`, `ErrorHandling/HttpStatusCodeErrorFilter.cs`
