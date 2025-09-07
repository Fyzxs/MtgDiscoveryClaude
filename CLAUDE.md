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
- Tailwind CSS is being phased out in favor of MUI sx props
- Apollo Client for GraphQL integration
- React Router DOM for client-side routing

## Architecture Overview

This is a Magic: The Gathering collection management platform built on .NET 9.0 following the **MicroObjects** architectural pattern - an extreme OOP approach where every concept is explicitly represented as an object.

### Full-Stack Architecture

The platform consists of:
- **Backend**: .NET 9.0 GraphQL API with layered MicroObjects architecture
- **Frontend**: React 19 client application with Material-UI components

### Backend Layered Architecture

The .NET solution implements a layered architecture with clear separation of concerns:

1. **Entry Layer** (`Lib.MtgDiscovery.Entry`): Service entry point, request validation, response formatting
2. **Domain Layer** (`Lib.Domain.Cards`, `Lib.Domain.User`): Business logic and domain operations  
3. **Data Layer** (`Lib.MtgDiscovery.Data`): Data access coordination
4. **Aggregator Layer** (`Lib.Aggregator.Cards`, `Lib.Aggregator.User`): Data aggregation and transformation
5. **Adapter Layer** (`Lib.Adapter.Scryfall.*`): External system integration (Cosmos DB, Blob Storage)
6. **Infrastructure Layer** (`Lib.Cosmos`, `Lib.BlobStorage`): Core infrastructure components
7. **API Layer** (`App.MtgDiscovery.GraphQL`): GraphQL API endpoints

### Frontend Architecture

The React client (`client/` directory) follows atomic design principles:

1. **Atoms** (`client/src/components/atoms/`): Basic UI elements (AppButton, AppCard, AppInput)
2. **Molecules** (`client/src/components/molecules/`): Component combinations (ManaCost, CardLinks)
3. **Organisms** (`client/src/components/organisms/`): Complex components (CardDisplayResponsive, CardCompact)
4. **Templates** (`client/src/components/templates/`): Page layout structures
5. **Pages** (`client/src/pages/`): Complete page implementations

### Project Structure

#### Backend (.NET Projects)
- **App.MtgDiscovery.GraphQL**: GraphQL API using HotChocolate with authentication
- **Lib.Universal**: Core utilities including configuration, service locator, and base primitives
- **Lib.Cosmos**: Azure Cosmos DB integration library with full MicroObjects patterns
- **Lib.BlobStorage**: Azure Blob Storage integration library
- **Lib.Adapter.Scryfall.Cosmos**: Scryfall data models for Cosmos DB including user storage
- **Lib.Adapter.Scryfall.BlobStorage**: Scryfall image storage adapters
- **Lib.Scryfall.Ingestion**: Scryfall API client and data ingestion
- **Lib.MtgDiscovery.Entry**: Entry service layer with validation and user operations
- **Lib.Domain.Cards**: Card domain logic
- **Lib.Domain.User**: User domain logic and registration
- **Lib.MtgDiscovery.Data**: Data service layer
- **Lib.Aggregator.Cards**: Card data aggregation
- **Lib.Aggregator.User**: User data aggregation
- **Lib.Shared.Abstractions**: Core interfaces and abstractions
- **Lib.Shared.DataModels**: Entity interfaces and data transfer objects including user entities
- **Lib.Shared.UserInfo**: User-specific value objects (UserId, UserSourceId, UserNickname)
- **Lib.Shared.CollectorInfo**: Collector-specific data models
- **Lib.Shared.Invocation**: Operation response patterns and utilities
- **TestConvenience.Core**: Testing utilities including fakes, type wrappers, and reflection helpers
- **Example.***: Example applications demonstrating specific functionality

#### Frontend (React Application)
- **client/**: React 19 application with TypeScript
  - **src/components/atoms/shared/**: Core UI components (AppButton, AppCard, AppInput)
  - **src/components/molecules/**: Composed components (ManaCost, CardLinks, ArtistInfo)
  - **src/components/organisms/**: Complex components (CardDisplayResponsive, CardCompact)
  - **src/pages/**: Page components (AllSetsPage, SetPage, CardSearchPage)
  - **src/theme/**: Material-UI theme configuration with MTG-specific colors
  - **src/generated/**: GraphQL generated types and hooks

### Key Architectural Patterns

#### MicroObjects Philosophy
The codebase follows strict MicroObjects principles:
- Every concept has explicit representation through interfaces and classes
- No primitives - everything wrapped in domain objects
- No nulls - use Null Object pattern
- Immutable objects with `private readonly` fields
- Interface for every class (1:1 mapping)
- Constructor injection only (no logic in constructors)
- No public statics, no enums, no reflection
- Composition over inheritance
- Methods expose behavior, not data (no getters/setters except DTOs)

#### Frontend Patterns
The React application follows these architectural patterns:
- **Atomic Design**: Components organized by complexity (atoms → molecules → organisms)
- **Material-UI System**: Single UI framework using sx props for styling
- **Component Composition**: Reusable components with clear prop interfaces
- **Context-Aware Display**: Components adapt based on CardContext (isOnSetPage, showCollectorInfo)
- **GraphQL Integration**: Apollo Client with generated types and hooks
- **Theme-Based Styling**: Custom MTG theme extending Material-UI with rarity colors
- **Responsive Design**: Mobile-first approach with adaptive layouts

#### Configuration Pattern
Configuration classes follow a specific structure:
- Root configs use `MonoStateConfig` singleton
- Nested configs use parent key + colon separator
- Config classes prefixed with "Config" (e.g., `ConfigCosmosConfiguration`)
- Marker classes use semicolon syntax: `public abstract class Name : Base;`
- Configuration hierarchy: Root → Account → Container/Database

#### Testing Architecture
- MSTest framework with AwesomeAssertions
- Self-contained tests (no test class variables)
- TypeWrapper pattern for testing classes with private constructors
- Fakes in `Fakes` folder with "Fake" suffix
- No mocks - use simple fake implementations
- Test naming: `MethodName_Scenario_ExpectedBehavior`

#### GraphQL Schema Patterns
- Union types for polymorphic responses (success/failure)
- Separate ObjectType classes for each entity type
- Schema extensions in `Schemas` folder
- Query methods in separate classes with `[ExtendObjectType]` attribute
- Response models extend `ResponseModel` base class

#### Scope Rules
- Public scope: Only in `Apis` folder
- Internal scope: Everything outside `Apis` folder
- Test projects have `InternalsVisibleTo` access

## Important Files to Reference

1. **CODING_CRITERIA.md**: Specific patterns for this codebase
2. **TESTING_GUIDELINES.md**: Unit testing conventions and patterns
3. **microobjects_coding_guidelines.md**: Complete MicroObjects philosophy and implementation guide
4. **PRD.md**: Product requirements and feature specifications
5. **Architecture.md**: Detailed system architecture documentation

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

### Frontend (React/TypeScript)
- **UI Framework**: Material-UI only (no Tailwind CSS)
- **Styling**: Use MUI sx props instead of className for custom styles
- **Component Naming**: App* prefix for custom wrapped components (AppButton, AppCard)
- **Import Style**: Named imports preferred, avoid default imports where possible
- **Props Interface**: Each component has dedicated Props interface
- **File Organization**: Follow atomic design folder structure
- **Theme Usage**: Reference theme.palette and theme.mtg for colors and spacing
- **GraphQL**: Use generated types from codegen, no manual type definitions

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
- Each test completely self-contained
- Use Arrange-Act-Assert pattern
- Return value named `actual` or use `_` discard
- Test naming: `MethodName_Scenario_ExpectedBehavior`
- Always verify fake invocation counts
- Use `ConfigureAwait(false)` on async calls
- Create `InstanceWrapper : TypeWrapper<ClassUnderTest>` for testing classes with private constructors

## User Registration System

The platform implements JWT-based authentication with user registration functionality following the layered MicroObjects architecture.

### Authentication Components
- `App.MtgDiscovery.GraphQL/Authentication/AuthenticatedUser.cs:8-41` - Extracts user info from JWT claims
- `App.MtgDiscovery.GraphQL/Authentication/IAuthenticatedUser.cs:5-10` - Authentication interface
- `App.MtgDiscovery.GraphQL/Mutations/UserMutationMethods.cs:17-68` - GraphQL user mutations

### User Data Flow
Registration follows the standard layer pattern:
1. **GraphQL Layer**: `UserMutationMethods.RegisterUserInfoAsync` validates JWT claims
2. **Entry Layer**: `IUserEntryService.RegisterUserAsync` handles request validation  
3. **Domain Layer**: `IUserDomainService.RegisterUserAsync` processes business logic
4. **Data Layer**: Coordinates data access through aggregator layer
5. **Aggregator Layer**: `IUserAggregatorService.RegisterUserAsync` handles data aggregation
6. **Adapter Layer**: `UserInfoItem` and `UserInfoScribe` for Cosmos DB storage

### User Information Types
- `Lib.Shared.UserInfo/Values/UserId.cs:5-12` - Unique user identifier
- `Lib.Shared.UserInfo/Values/UserSourceId.cs` - External identity provider ID
- `Lib.Shared.UserInfo/Values/UserNickname.cs` - Display name
- `Lib.Shared.DataModels/Entities/User/IUserInfoItrEntity.cs:5-10` - User info interface
- `Lib.Shared.DataModels/Entities/User/IUserRegistrationItrEntity.cs:5-9` - Registration response

### Storage Implementation
- `Lib.Adapter.Scryfall.Cosmos/Apis/Entities/UserInfoItem.cs:6-19` - Cosmos DB document model
- `Lib.Adapter.Scryfall.Cosmos/Apis/Operators/UserInfoScribe.cs:7-13` - Cosmos DB operations

### Authentication Configuration
JWT authentication is configured in `App.MtgDiscovery.GraphQL/Startup.cs:45-56` with Auth0 integration:
- Uses Auth0 domain and audience configuration
- Claims principal injection for user context
- Authorization policies for protected GraphQL mutations
- User ID generation using GUID namespace for consistency

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
3. Inject `ClaimsPrincipal` for user context
4. Register mutation extensions in schema configuration

## Critical Patterns to Follow

1. **Never modify production code for test scenarios**
2. **Always check existing patterns in neighboring files before implementing**
3. **Create interfaces before implementations**
4. **Wrap all primitives in domain objects**
5. **Use marker classes for type safety without implementation**
6. **Follow the 3-build-failure limit before stopping**
7. **Service dependencies flow downward through layers (Entry → Domain → Data → Aggregator → Adapter)**
8. **Use `IEntryService` for GraphQL to service layer communication**
9. **Authentication requires JWT claims principal injection in GraphQL mutations**
10. **User operations follow the same layered pattern as card operations**

## Frontend Development

### Component Architecture
Components follow atomic design principles with clear separation:

#### Key Files
- `client/src/App.tsx:8-11` - Component imports using App* naming convention
- `client/src/theme/index.ts:137-305` - Custom MTG theme with rarity colors
- `client/src/components/atoms/shared/AppButton.tsx` - Wrapped MUI Button with loading state
- `client/src/components/organisms/CardDisplayResponsive.tsx` - Main card display component
- `client/src/components/organisms/CardCompact.tsx:24-41` - MUI sx styling example

#### Styling Guidelines
- Use `sx` props for component-specific styling: `sx={{ bgcolor: 'grey.900', borderRadius: 2 }}`
- Reference theme colors: `theme.palette.rarity.mythic`, `theme.mtg.shadows.card.hover`
- Avoid Tailwind classes - convert to MUI equivalents
- Use responsive breakpoints: `sx={{ width: { xs: '100%', sm: 'auto' } }}`

#### Component Integration
- **CardContext**: Pass context object to components for conditional display
- **Event Handlers**: Use onCardClick, onSetClick, onArtistClick patterns
- **Apollo Integration**: Generated hooks in `src/generated/` directory
- **Routing**: React Router with error boundaries per route

### Development Workflow
1. Generate GraphQL types: `npm run codegen`
2. Start development server: `npm run dev`  
3. Backend API should run on different port
4. Check Material-UI documentation for component APIs
5. Use theme helpers for consistent colors and spacing

## Sessions System Behaviors

@CLAUDE.sessions.md
