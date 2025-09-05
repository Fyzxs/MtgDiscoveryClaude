# App.MtgDiscovery.GraphQL CLAUDE.md

## Purpose
GraphQL API endpoint providing MTG collection management functionality with JWT-based authentication.

## Narrative Summary
This application serves as the main API gateway for the MTG Discovery platform. It exposes GraphQL queries and mutations for card, set, artist, and user operations. The service implements HotChocolate GraphQL server with Auth0 JWT authentication integration, following the layered MicroObjects architecture pattern for all operations.

## Key Files
- `AppMtgDiscoveryGraphQlProgram.cs` - Application entry point
- `Startup.cs:45-56` - Auth0 JWT authentication configuration
- `Authentication/AuthUserArgEntity.cs:8-35` - JWT claims extraction for user operations
- `Mutations/UserMutationMethods.cs:32-60` - User registration GraphQL mutation
- `Schemas/` - GraphQL schema definitions and extensions
- `Entities/Types/` - GraphQL ObjectType definitions for response models

## GraphQL Endpoints
### Queries
- Card operations: Card search, retrieval by ID/name
- Set operations: Set listing, retrieval by code/ID
- Artist operations: Artist search, cards by artist
- User operations: User information queries

### Mutations
- `registerUserInfo` - JWT-authenticated user registration

## Authentication Integration
### JWT Configuration
- Uses Auth0 domain and audience from configuration
- Extracts user claims for identity operations
- Deterministic GUID generation using namespace for consistent user IDs

### User Registration Flow
1. Client provides JWT token with Auth0 claims
2. `AuthUserArgEntity` extracts claims (`sub`, `nickname`)
3. Generates consistent `UserId` using GUID namespace
4. Calls `IEntryService.RegisterUserAsync` for business logic

## Integration Points
### Consumes
- Entry Service: `Lib.MtgDiscovery.Entry.Apis.IEntryService`
- Auth0: JWT token validation and claims extraction

### Provides
- `/graphql` - Main GraphQL endpoint
- Union type responses for success/failure scenarios
- JWT-authenticated mutation operations

## Configuration
Required environment variables:
- `Auth0:Domain` - Auth0 authentication domain
- `Auth0:Audience` - Auth0 API audience identifier

## Key Patterns
- Union types for polymorphic GraphQL responses
- `[Authorize]` attribute for protected mutations
- `ClaimsPrincipal` injection for user context
- Response model transformation from ITR entities to GraphQL entities
- Schema extension pattern for modular GraphQL organization

## Related Documentation
- `../Lib.MtgDiscovery.Entry/CLAUDE.md` - Entry service layer
- `../Lib.Shared.DataModels/CLAUDE.md` - Shared entity interfaces