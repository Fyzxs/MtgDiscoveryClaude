# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Development Commands

### Build
```bash
# Build entire solution
dotnet build src/MtgDiscoveryVibe.sln

# Build specific project
dotnet build src/Lib.Cosmos/Lib.Cosmos.csproj

# Build GraphQL API
dotnet build src/App.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL.csproj
```

### Test
```bash
# Run all tests
dotnet test src/MtgDiscoveryVibe.sln

# Run specific test project
dotnet test src/Lib.Cosmos.Tests/Lib.Cosmos.Tests.csproj

# Run with coverage
dotnet test src/MtgDiscoveryVibe.sln --collect:"XPlat Code Coverage"

# Run single test by method name
dotnet test src/Lib.Cosmos.Tests/Lib.Cosmos.Tests.csproj --filter "FullyQualifiedName~MethodName"

# Run tests for a specific class
dotnet test --filter "ClassName"

# Run tests with detailed output showing individual test results (from src directory)
dotnet vstest ProjectName.Tests/bin/Debug/net9.0/ProjectName.Tests.dll --logger:"console;verbosity=normal"
```

### Run Applications
```bash
# Run GraphQL API
dotnet run --project src/App.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL.csproj

# Run React client (from client directory)
cd client && npm run dev

# Run example applications
dotnet run --project src/Example.Core/Example.Core.csproj
dotnet run --project src/Example.Scryfall.CosmosIngestion/Example.Scryfall.CosmosIngestion.csproj
```

### Frontend Development Commands

From the `client/` directory:

```bash
# Development server
npm run dev

# Build for production
npm run build

# Run linting
npm run lint

# Generate GraphQL types
npm run codegen

# Watch GraphQL schema changes
npm run codegen:watch

# Preview production build
npm run preview
```

### Clean
```bash
dotnet clean src/MtgDiscoveryVibe.sln
```

### Update NuGet Packages
```powershell
# From src directory
./slnNugetUpdater.ps1
```

## Dependency Management

### Backend (.NET)
- Use dotnet commands to add nuget packages, not editing of files
- Do not use System.Text.Json; use only Newtonsoft.Json
- When adding project references: `dotnet add reference ../ProjectName/ProjectName.csproj`

### Frontend (React)
- Use npm for package management: `npm install package-name`
- Material-UI (@mui/material) is the primary UI component library
- Tailwind CSS is installed but being phased out - use MUI sx props instead
- Apollo Client for GraphQL integration
- React Router DOM for client-side routing
- Auth0 React SDK for authentication

## Architecture Overview

This is a Magic: The Gathering collection management platform built on .NET 9.0 following the **MicroObjects** architectural pattern - an extreme OOP approach where every concept is explicitly represented as an object.

### Full-Stack Architecture

The platform consists of:
- **Backend**: .NET 9.0 GraphQL API with layered MicroObjects architecture
- **Frontend**: React 19 client application with Material-UI components

### Backend Layered Architecture

The .NET solution implements a layered architecture following the intended data flow:

**Data Flow (Request → Response):**
1. **App Layer** (`App.MtgDiscovery.GraphQL`): Translate request into ArgEntity
2. **Entry Layer** (`Lib.MtgDiscovery.Entry`): Validates ArgEntity and maps to ItrEntity
3. **Shared Layer** (`Lib.Shared.*`): Applies rules on the data (validation, filtering, transformation)
4. **Domain Layer** (`Lib.Domain.*`): Applies ALWAYS rules on the data (business logic)
5. **Aggregator Layer** (`Lib.Aggregator.*`): Knows what adapters to talk to, orchestrates data retrieval
6. **Adapter Layer** (`Lib.Adapter.*`): Maps ItrEntity to XfrEntity for adapter operations, calls external systems (Cosmos DB via `Lib.Adapter.Scryfall.Cosmos`), maps ExtEntity back to ItrEntity
7. **Infrastructure Layer** (`Lib.Cosmos`, `Lib.Universal`, `Lib.Scryfall.Ingestion`): Core infrastructure components

**Return Flow (Response ← Request):**
- Aggregator aggregates adapter responses
- Domain applies always rules
- Shared applies rules
- Entry maps ItrEntity to OutEntity (and intermediate OufEntity where needed)
- App translates OutEntity to response

**Entity Types by Layer:**
- **ArgEntity**: Argument entities from GraphQL/external input (App → Entry)
- **ItrEntity**: Internal transfer entities between layers (Entry ↔ Shared ↔ Domain ↔ Aggregator)
- **XfrEntity**: Transfer entities within adapter layer operations (used by adapter services)
- **ExtEntity**: External system entities from Cosmos DB (Cosmos documents)
- **OutEntity**: Output entities returned to GraphQL layer (Entry → App)
- **OufEntity**: Output from domain/aggregator layers (internal layer outputs before final mapping)

**Layer Details:**
- **App Layer**: GraphQL API endpoints using HotChocolate with JWT authentication (Auth0)
- **Entry Layer**: Service entry point, request validation, response formatting
- **Shared Layer**: Cross-cutting action patterns (filtering, validation, enrichment), operation responses, entity interfaces
- **Domain Layer**: Business logic and domain operations for Cards, Sets, Artists, User, UserCards, UserSetCards
- **Aggregator Layer**: Data aggregation and transformation, coordinates data retrieval from adapters
- **Adapter Layer**: External system integration (Cosmos DB via `Lib.Adapter.Scryfall.Cosmos`, Scryfall API ingestion)
- **Infrastructure Layer**: Core infrastructure components and utilities

### Frontend Architecture

The React client (`client/` directory) follows atomic design principles:

1. **Atoms** (`client/src/components/atoms/`): Basic UI elements organized by domain (Cards/, Sets/, shared/)
2. **Molecules** (`client/src/components/molecules/`): Component combinations (ManaCost, CardLinks, ArtistInfo)
3. **Organisms** (`client/src/components/organisms/`): Complex components (CardDisplayResponsive, CardCompact, Header, Footer)
4. **Templates** (`client/src/components/templates/`): Page layout structures (Layout)
5. **Pages** (`client/src/pages/`): Complete page implementations

### Project Structure

#### Backend (.NET Projects)

**App Layer:**
- **App.MtgDiscovery.GraphQL**: GraphQL API using HotChocolate with Auth0 JWT authentication

**Entry Layer:**
- **Lib.MtgDiscovery.Entry**: Entry service layer with validation and user operations

**Shared Layer:**
- **Lib.Shared.Abstractions**: Core interfaces and abstractions for action patterns (filtering, validation, transformation)
- **Lib.Shared.DataModels**: Entity interfaces and data transfer objects (ArgEntity, ItrEntity, XfrEntity, OufEntity, OutEntity patterns)
- **Lib.Shared.Invocation**: Operation response patterns (`IOperationResponse<T>`, `OperationException`) - use these, NOT `OpResponse<T>` from Lib.Cosmos

**Domain Layer:**
- **Lib.Domain.Cards**: Card domain logic
- **Lib.Domain.Sets**: Set domain logic and operations
- **Lib.Domain.Artists**: Artist domain logic and search operations
- **Lib.Domain.User**: User domain logic and registration
- **Lib.Domain.UserCards**: User card collection domain logic
- **Lib.Domain.UserSetCards**: User set-specific card collection domain logic

**Aggregator Layer:**
- **Lib.Aggregator.Cards**: Card data aggregation
- **Lib.Aggregator.Sets**: Set data aggregation
- **Lib.Aggregator.Artists**: Artist data aggregation
- **Lib.Aggregator.User**: User data aggregation
- **Lib.Aggregator.UserCards**: User card collection aggregation
- **Lib.Aggregator.UserSetCards**: User set card collection aggregation
- **Lib.Aggregator.Scryfall.Shared**: Shared aggregation utilities and mappers

**Adapter Layer:**
- **Lib.Adapter.Cards**: Card query and command adapter abstraction
- **Lib.Adapter.Sets**: Set query adapter abstraction
- **Lib.Adapter.Artists**: Artist query adapter abstraction
- **Lib.Adapter.User**: User command adapter abstraction
- **Lib.Adapter.UserCards**: User cards command adapter abstraction
- **Lib.Adapter.UserSetCards**: User set cards query and command adapter abstraction
- **Lib.Adapter.Scryfall.Cosmos**: Cosmos DB operators (Gophers for reads, Scribes for writes, Inquisitors for queries) and ExtEntity models

**Scryfall Integration:**
- **Lib.Scryfall.Ingestion**: Scryfall API client and bulk data ingestion
- **Lib.Scryfall.Shared**: Shared Scryfall utilities and mappings

**Infrastructure Layer:**
- **Lib.Universal**: Core utilities including configuration, service locator, and base primitives
- **Lib.Cosmos**: Azure Cosmos DB integration library with MicroObjects patterns (OpResponse used internally, but adapters must return IOperationResponse)

**Testing and Examples:**
- **TestConvenience.Core**: Testing utilities including fakes, type wrappers, and reflection helpers
- **Example.***: Example applications demonstrating specific functionality

#### Frontend (React Application)
- **client/**: React 19 application with TypeScript
  - **src/components/atoms/**: Atomic UI components organized by domain (Cards/, Sets/, shared/, auth/, layouts/)
  - **src/components/molecules/**: Composed components (Cards/, Sets/, shared/)
  - **src/components/organisms/**: Complex components (CardDisplayResponsive, CardCompact, MtgCard, Header, Footer)
  - **src/components/templates/**: Layout components
  - **src/pages/**: Page components (AllSetsPage, SetPage, CardSearchPage, ArtistPage)
  - **src/theme/**: Material-UI theme configuration with MTG-specific colors (rarity colors, shadows)
  - **src/generated/**: GraphQL generated types and hooks (auto-generated, do not edit manually)
  - **src/components/auth/**: Auth0 authentication components

### Key Architectural Patterns

#### MicroObjects Philosophy
The codebase follows MicroObjects principles with pragmatic DTO usage:
- Every concept has explicit representation through interfaces and classes
- Primitives wrapped in domain objects where appropriate, strings used in DTOs for simplicity
- No nulls - use Null Object pattern (except validators at boundaries which check for null)
- Immutable objects with `private readonly` fields
- Interface for every class (1:1 mapping)
- Constructor injection only (no logic in constructors)
- No public statics (except MonoState pattern, LoggerMessage attributes, framework requirements)
- No enums, no reflection at runtime
- Composition over inheritance
- Methods expose behavior, not data (no getters/setters except DTOs)

#### Frontend Patterns
The React application follows these architectural patterns:
- **Atomic Design**: Components organized by complexity and domain (atoms → molecules → organisms)
- **Material-UI System**: Primary UI framework using sx props for styling (Tailwind being phased out)
- **Component Composition**: Reusable components with clear prop interfaces
- **Context-Aware Display**: Components adapt based on CardContext (isOnSetPage, showCollectorInfo)
- **GraphQL Integration**: Apollo Client with generated types and hooks from codegen
- **Theme-Based Styling**: Custom MTG theme extending Material-UI with rarity colors and MTG-specific shadows
- **Responsive Design**: Mobile-first approach with adaptive layouts
- **Authentication**: Auth0 integration with JWT token management

#### Configuration Pattern
Configuration classes follow a specific structure:
- Root configs use `MonoStateConfig` singleton
- Nested configs use parent key + colon separator
- Config classes prefixed with "Config" (e.g., `ConfigCosmosConfiguration`)
- Marker classes use semicolon syntax: `public abstract class Name : Base;`
- Configuration hierarchy: Root → Account → Container/Database

#### Testing Architecture
- **Framework**: MSTest with AwesomeAssertions
- **Self-contained tests**: No test class variables
- **TypeWrapper pattern**: For testing classes with private constructors
- **Fakes over mocks**: Fakes in `Fakes` folder with "Fake" suffix
- **Test naming**: `MethodName_Scenario_ExpectedBehavior`
- **Arrange-Act-Assert pattern**: Clear test structure
- **Validation counting**: Always verify fake invocation counts

#### GraphQL Schema Patterns
- **Union types**: For polymorphic responses (success/failure)
- **ObjectType classes**: Separate classes for each entity type
- **Schema extensions**: In `Schemas` folder with `[ExtendObjectType]` attribute
- **Response models**: Extend `ResponseModel` base class
- **Authentication**: `[Authorize]` attribute for protected operations with `ClaimsPrincipal` injection

#### Scope Rules
- **Public scope**: Only in `Apis` folders
- **Internal scope**: Everything outside `Apis` folders
- **Test projects**: Have `InternalsVisibleTo` access to source projects

## Important Files to Reference

1. **CODING_CRITERIA.md**: Specific patterns for this codebase including validator patterns, NoArgsEntity usage
2. **TESTING_GUIDELINES.md**: Unit testing conventions and patterns
3. **microobjects_coding_guidelines.md**: Complete MicroObjects philosophy and implementation guide
4. **PRD.md**: Product requirements and feature specifications
5. **Architecture.md**: Detailed system architecture documentation with layer definitions

## Code Style Requirements

### Backend (.NET)
- File-scoped namespaces
- No greater than operators (use `<` only)
- No boolean negation (`!`) - use `is false` or explicit inverse methods
- `ConfigureAwait(false)` on all async calls
- `init` setters for DTO-style classes
- No comments unless explicitly requested
- Use `replace_all: true` when using Edit tool
- If statement bodies MUST be block bodies, or on a single line with braces
- Classes must be `sealed` or `abstract` (very few exceptions)
- Explicit types preferred over `var` for readability

### Frontend (React/TypeScript)
- **UI Framework**: Material-UI only (Tailwind installed but being phased out)
- **Styling**: Use MUI sx props instead of className for custom styles
- **Component Naming**: Domain-organized atoms (Cards/, Sets/, shared/), App* prefix for wrapped components
- **Import Style**: Named imports preferred, avoid default imports where possible
- **Props Interface**: Each component has dedicated Props interface
- **File Organization**: Follow atomic design folder structure with domain organization
- **Theme Usage**: Reference theme.palette (including theme.palette.rarity.*) and theme.mtg for colors and spacing
- **GraphQL**: Use generated types from codegen (`npm run codegen`), no manual type definitions
- **Authentication**: Use Auth0 React SDK hooks and components

### Pragma Directives
- **Avoid #pragma directives** - they accumulate as technical debt
- When reviewing code, check if existing #pragma directives are still necessary
- Remove any #pragma that suppresses warnings that no longer occur
- If you must add a #pragma:
  - Include a comment explaining why it's necessary
  - Scope it as narrowly as possible (line-specific, not file-wide)
  - Plan to remove it in the future
- Common unnecessary pragmas to remove:
  - Formatting suppressions (IDE0055) - fix the formatting instead
  - Warnings for code that's been refactored
  - Overly broad suppressions that hide real issues

## Testing Requirements

When writing tests:
- Use MSTest framework with AwesomeAssertions
- Each test completely self-contained
- Use Arrange-Act-Assert pattern
- Return value named `actual` or use `_` discard
- Test naming: `MethodName_Scenario_ExpectedBehavior`
- Always verify fake invocation counts
- Use `ConfigureAwait(false)` on async calls
- Create `InstanceWrapper : TypeWrapper<ClassUnderTest>` for testing classes with private constructors
- No test class variables - everything in test method
- Fakes instead of mocks

## User and Authentication System

The platform implements JWT-based authentication (Auth0) with user registration functionality following the layered MicroObjects architecture.

### Authentication Components
- `App.MtgDiscovery.GraphQL/Authentication/AuthenticatedUser.cs:8-41` - Extracts user info from JWT claims
- `App.MtgDiscovery.GraphQL/Authentication/IAuthenticatedUser.cs:5-10` - Authentication interface
- `App.MtgDiscovery.GraphQL/Mutations/UserMutationMethods.cs:17-68` - GraphQL user mutations
- `client/src/components/auth/` - Frontend Auth0 integration components

### User Data Flow
Registration follows the standard layer pattern:
1. **GraphQL Layer**: `UserMutationMethods.RegisterUserInfoAsync` validates JWT claims from Auth0
2. **Entry Layer**: `IUserEntryService.RegisterUserAsync` handles request validation
3. **Domain Layer**: `IUserDomainService.RegisterUserAsync` processes business logic
4. **Aggregator Layer**: `IUserAggregatorService.RegisterUserAsync` handles data aggregation
5. **Adapter Layer**: `UserInfoExtEntity` (Cosmos document) via `Lib.Adapter.Scryfall.Cosmos` operators

### User Information Types
- `Lib.Shared.DataModels/Entities/Args/IAuthUserArgEntity.cs` - JWT authentication argument interface (Auth0 claims)
- `Lib.Shared.DataModels/Entities/Itrs/IUserInfoItrEntity.cs` - User info interface with string properties
  - Properties: `string UserId`, `string UserSourceId`, `string UserNickname`
- `Lib.Shared.DataModels/Entities/Itrs/IUserRegistrationItrEntity.cs` - Registration response
  - Property: `string UserId`

### Storage Implementation
- `Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/UserInfoExtEntity.cs` - Cosmos DB document model
- `Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Scribes/UserInfoScribe.cs` - Cosmos DB write operations
- User cards stored via `UserCardsScribe` and `UserSetCardsScribe`

### Authentication Configuration
JWT authentication is configured in `App.MtgDiscovery.GraphQL/Startup.cs:45-56` with Auth0 integration:
- Uses Auth0 domain and audience configuration (from appsettings or environment variables)
- Claims principal injection for user context
- Authorization policies for protected GraphQL mutations
- User ID generation using GUID namespace for consistency
- Frontend uses `@auth0/auth0-react` for token management

## GraphQL Development

### Query Structure
- Queries return union types for different response scenarios
- Use `... on TypeName` fragments to access type-specific fields
- Always include `__typename` for type discrimination
- Response types: `SuccessDataResponseModel<T>`, `FailureResponseModel`

### Adding New Queries
1. Create query method in `Queries` folder with `[ExtendObjectType]`
2. Define ObjectType classes for response entities in `Entities/Types`
3. Register types in schema extensions
4. Create mappers for data transformation

### Adding New Mutations
1. Create mutation class extending `ApiMutation` with `[ExtendObjectType]`
2. Use `[Authorize]` attribute for protected operations
3. Inject `ClaimsPrincipal` for user context (Auth0 JWT claims)
4. Register mutation extensions in schema configuration

### Frontend GraphQL Integration
1. Update GraphQL queries/mutations in appropriate `.graphql` files
2. Run `npm run codegen` to generate TypeScript types and hooks
3. Use generated hooks in components (`useQuery`, `useMutation` from generated code)
4. Handle loading, error, and data states
5. Pass Auth0 JWT token via Apollo Client auth link

## Critical Patterns to Follow

1. **Never modify production code for test scenarios**
2. **Always check existing patterns in neighboring files before implementing**
3. **Create interfaces before implementations**
4. **Balance MicroObjects with DTOs** - Wrap primitives in domain objects, but use strings in DTOs for simplicity
5. **Use marker classes for type safety without implementation**
6. **Follow the 3-build-failure limit before stopping**
7. **Service dependencies flow downward through layers** (Entry → Domain → Aggregator → Adapter)
8. **Use `IEntryService` for GraphQL to service layer communication**
9. **Authentication requires JWT claims principal injection** in GraphQL mutations (Auth0)
10. **User operations follow the same layered pattern** as card operations
11. **Adapters must return `IOperationResponse<T>` from `Lib.Shared.Invocation`**, NOT `OpResponse<T>` from `Lib.Cosmos`
12. **Adapter exceptions should extend `OperationException`** from `Lib.Shared.Invocation.Exceptions`
13. **Use XfrEntity for adapter layer internal transfers**, not ItrEntity at adapter service boundaries
14. **Validators check for null at boundaries** - this is correct and necessary, not a violation of No Nulls principle
15. **Frontend uses Material-UI sx props**, not Tailwind classes (Tailwind being phased out)

## Frontend Development

### Component Architecture
Components follow atomic design principles with domain organization:

#### Key Files
- `client/src/components/atoms/shared/AppButton.tsx` - Wrapped MUI Button with loading state
- `client/src/components/atoms/Cards/` - Card-specific atomic components
- `client/src/components/atoms/Sets/` - Set-specific atomic components
- `client/src/theme/index.ts:137-305` - Custom MTG theme with rarity colors
- `client/src/components/organisms/CardDisplayResponsive.tsx` - Main card display component
- `client/src/components/organisms/CardCompact.tsx` - Compact card display with MUI sx styling
- `client/src/components/auth/Auth0TokenProvider.tsx` - Auth0 authentication provider

#### Styling Guidelines
- Use `sx` props for component-specific styling: `sx={{ bgcolor: 'grey.900', borderRadius: 2 }}`
- Reference theme colors: `theme.palette.rarity.mythic`, `theme.mtg.shadows.card.hover`
- Avoid Tailwind classes - convert to MUI equivalents if found
- Use responsive breakpoints: `sx={{ width: { xs: '100%', sm: 'auto' } }}`
- Custom colors in theme: rarity-based palette, MTG-specific shadows

#### Component Integration
- **CardContext**: Pass context object to components for conditional display (isOnSetPage, showCollectorInfo)
- **Event Handlers**: Use onCardClick, onSetClick, onArtistClick patterns
- **Apollo Integration**: Generated hooks in `src/generated/` directory
- **Routing**: React Router with error boundaries per route
- **Authentication**: Auth0 hooks (`useAuth0`) for login/logout, token management

### Development Workflow
1. Generate GraphQL types: `npm run codegen` (after backend schema changes)
2. Start development server: `npm run dev`
3. Backend API should run on different port (configured in Apollo Client setup)
4. Check Material-UI documentation for component APIs
5. Use theme helpers for consistent colors and spacing
6. Auth0 configuration in environment variables

## Azure DevOps Integration

This project uses Azure DevOps for work item tracking and pull requests:

- **Azure Boards**: Work item management (User Stories, Tasks, Bugs)
- **Azure Repos**: Git repository and PR management
- Bash commands for `az boards` and `az repos` are pre-approved for use
- Azure DevOps CLI (`az devops`) configured for automation

Common Azure DevOps commands:
```bash
# Work items
az boards work-item show --id <id>
az boards work-item list
az boards work-item create --type Task --title "Task Title"

# Pull requests
az repos pr create --title "PR Title"
az repos pr list
az repos pr show --id <pr-id>
```

## Testing Requirements

When writing tests:
- Use MSTest framework with AwesomeAssertions
- Each test completely self-contained
- Use Arrange-Act-Assert pattern
- Return value named `actual` or use `_` discard
- Test naming: `MethodName_Scenario_ExpectedBehavior`
- Always verify fake invocation counts
- Use `ConfigureAwait(false)` on async calls
- Create `InstanceWrapper : TypeWrapper<ClassUnderTest>` for testing classes with private constructors
- No test class variables - everything in test method
- Fakes instead of mocks

## User and Authentication System

The platform implements JWT-based authentication (Auth0) with user registration functionality following the layered MicroObjects architecture.

### Authentication Components
- `App.MtgDiscovery.GraphQL/Authentication/AuthenticatedUser.cs:8-41` - Extracts user info from JWT claims
- `App.MtgDiscovery.GraphQL/Authentication/IAuthenticatedUser.cs:5-10` - Authentication interface
- `App.MtgDiscovery.GraphQL/Mutations/UserMutationMethods.cs:17-68` - GraphQL user mutations
- `client/src/components/auth/` - Frontend Auth0 integration components

### User Data Flow
Registration follows the standard layer pattern:
1. **GraphQL Layer**: `UserMutationMethods.RegisterUserInfoAsync` validates JWT claims from Auth0
2. **Entry Layer**: `IUserEntryService.RegisterUserAsync` handles request validation
3. **Domain Layer**: `IUserDomainService.RegisterUserAsync` processes business logic
4. **Aggregator Layer**: `IUserAggregatorService.RegisterUserAsync` handles data aggregation
5. **Adapter Layer**: `UserInfoExtEntity` (Cosmos document) via `Lib.Adapter.Scryfall.Cosmos` operators

### User Information Types
- `Lib.Shared.DataModels/Entities/Args/IAuthUserArgEntity.cs` - JWT authentication argument interface (Auth0 claims)
- `Lib.Shared.DataModels/Entities/Itrs/IUserInfoItrEntity.cs` - User info interface with string properties
  - Properties: `string UserId`, `string UserSourceId`, `string UserNickname`
- `Lib.Shared.DataModels/Entities/Itrs/IUserRegistrationItrEntity.cs` - Registration response
  - Property: `string UserId`

### Storage Implementation
- `Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/UserInfoExtEntity.cs` - Cosmos DB document model
- `Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Scribes/UserInfoScribe.cs` - Cosmos DB write operations
- User cards stored via `UserCardsScribe` and `UserSetCardsScribe`

### Authentication Configuration
JWT authentication is configured in `App.MtgDiscovery.GraphQL/Startup.cs:45-56` with Auth0 integration:
- Uses Auth0 domain and audience configuration (from appsettings or environment variables)
- Claims principal injection for user context
- Authorization policies for protected GraphQL mutations
- User ID generation using GUID namespace for consistency
- Frontend uses `@auth0/auth0-react` for token management

## GraphQL Development

### Query Structure
- Queries return union types for different response scenarios
- Use `... on TypeName` fragments to access type-specific fields
- Always include `__typename` for type discrimination
- Response types: `SuccessDataResponseModel<T>`, `FailureResponseModel`

### Adding New Queries
1. Create query method in `Queries` folder with `[ExtendObjectType]`
2. Define ObjectType classes for response entities in `Entities/Types`
3. Register types in schema extensions
4. Create mappers for data transformation

### Adding New Mutations
1. Create mutation class extending `ApiMutation` with `[ExtendObjectType]`
2. Use `[Authorize]` attribute for protected operations
3. Inject `ClaimsPrincipal` for user context (Auth0 JWT claims)
4. Register mutation extensions in schema configuration

### Frontend GraphQL Integration
1. Update GraphQL queries/mutations in appropriate `.graphql` files
2. Run `npm run codegen` to generate TypeScript types and hooks
3. Use generated hooks in components (`useQuery`, `useMutation` from generated code)
4. Handle loading, error, and data states
5. Pass Auth0 JWT token via Apollo Client auth link

## Critical Patterns to Follow

1. **Never modify production code for test scenarios**
2. **Always check existing patterns in neighboring files before implementing**
3. **Create interfaces before implementations**
4. **Balance MicroObjects with DTOs** - Wrap primitives in domain objects, but use strings in DTOs for simplicity
5. **Use marker classes for type safety without implementation**
6. **Follow the 3-build-failure limit before stopping**
7. **Service dependencies flow downward through layers** (Entry → Domain → Aggregator → Adapter)
8. **Use `IEntryService` for GraphQL to service layer communication**
9. **Authentication requires JWT claims principal injection** in GraphQL mutations (Auth0)
10. **User operations follow the same layered pattern** as card operations
11. **Adapters must return `IOperationResponse<T>` from `Lib.Shared.Invocation`**, NOT `OpResponse<T>` from `Lib.Cosmos`
12. **Adapter exceptions should extend `OperationException`** from `Lib.Shared.Invocation.Exceptions`
13. **Use XfrEntity for adapter layer internal transfers**, not ItrEntity at adapter service boundaries
14. **Validators check for null at boundaries** - this is correct and necessary, not a violation of No Nulls principle
15. **Frontend uses Material-UI sx props**, not Tailwind classes (Tailwind being phased out)

## Frontend Development

### Component Architecture
Components follow atomic design principles with domain organization:

#### Key Files
- `client/src/components/atoms/shared/AppButton.tsx` - Wrapped MUI Button with loading state
- `client/src/components/atoms/Cards/` - Card-specific atomic components
- `client/src/components/atoms/Sets/` - Set-specific atomic components
- `client/src/theme/index.ts:137-305` - Custom MTG theme with rarity colors
- `client/src/components/organisms/CardDisplayResponsive.tsx` - Main card display component
- `client/src/components/organisms/CardCompact.tsx` - Compact card display with MUI sx styling
- `client/src/components/auth/Auth0TokenProvider.tsx` - Auth0 authentication provider

#### Styling Guidelines
- Use `sx` props for component-specific styling: `sx={{ bgcolor: 'grey.900', borderRadius: 2 }}`
- Reference theme colors: `theme.palette.rarity.mythic`, `theme.mtg.shadows.card.hover`
- Avoid Tailwind classes - convert to MUI equivalents if found
- Use responsive breakpoints: `sx={{ width: { xs: '100%', sm: 'auto' } }}`
- Custom colors in theme: rarity-based palette, MTG-specific shadows

#### Component Integration
- **CardContext**: Pass context object to components for conditional display (isOnSetPage, showCollectorInfo)
- **Event Handlers**: Use onCardClick, onSetClick, onArtistClick patterns
- **Apollo Integration**: Generated hooks in `src/generated/` directory
- **Routing**: React Router with error boundaries per route
- **Authentication**: Auth0 hooks (`useAuth0`) for login/logout, token management

### Development Workflow
1. Generate GraphQL types: `npm run codegen` (after backend schema changes)
2. Start development server: `npm run dev`
3. Backend API should run on different port (configured in Apollo Client setup)
4. Check Material-UI documentation for component APIs
5. Use theme helpers for consistent colors and spacing
6. Auth0 configuration in environment variables

## Azure DevOps Integration

This project uses Azure DevOps for work item tracking and pull requests:

- **Azure Boards**: Work item management (User Stories, Tasks, Bugs)
- **Azure Repos**: Git repository and PR management
- Bash commands for `az boards` and `az repos` are pre-approved for use
- Azure DevOps CLI (`az devops`) configured for automation

Common Azure DevOps commands:
```bash
# Work items
az boards work-item show --id <id>
az boards work-item list
az boards work-item create --type Task --title "Task Title"

# Pull requests
az repos pr create --title "PR Title"
az repos pr list
az repos pr show --id <pr-id>
```

## Sessions System Behaviors

@CLAUDE.sessions.md
