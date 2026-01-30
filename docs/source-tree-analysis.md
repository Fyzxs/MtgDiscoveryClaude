# Source Tree Analysis

Generated: 2026-01-29

## Project Structure Overview

```
MtgDiscoveryVibe/
├── client/                          # React Frontend (Part: client)
│   ├── src/
│   │   ├── components/              # Atomic Design Components
│   │   │   ├── atoms/               # Basic UI elements
│   │   │   │   ├── Binder/          # Binder-specific atoms
│   │   │   │   ├── Cards/           # Card-specific atoms (ManaSymbol, etc.)
│   │   │   │   ├── Sealed/          # Sealed product atoms
│   │   │   │   ├── Sets/            # Set-specific atoms (SetIcon, etc.)
│   │   │   │   ├── accessibility/   # A11y components (SkipNavigation)
│   │   │   │   ├── layouts/         # Layout atoms (Grid, Flex)
│   │   │   │   ├── mui-wrappers/    # MUI component wrappers (40+)
│   │   │   │   └── shared/          # Shared atoms (DarkBadge, etc.)
│   │   │   ├── molecules/           # Composed components
│   │   │   │   ├── Binder/          # Binder molecules
│   │   │   │   ├── Cards/           # Card molecules (ManaCost, etc.)
│   │   │   │   ├── Sealed/          # Sealed product molecules
│   │   │   │   ├── Sets/            # Set molecules (MtgSetCard)
│   │   │   │   └── shared/          # Shared molecules
│   │   │   ├── organisms/           # Complex components
│   │   │   │   ├── Artists/         # Artist-related organisms
│   │   │   │   ├── Binder/          # Binder organisms
│   │   │   │   ├── Convention/      # Convention signing organisms
│   │   │   │   └── shared/          # Header, Footer, Navigation
│   │   │   ├── templates/           # Page layout templates
│   │   │   ├── pages/               # Page components
│   │   │   └── auth/                # Auth0 components
│   │   ├── contexts/                # React contexts
│   │   ├── generated/               # GraphQL codegen output
│   │   ├── graphql/                 # GraphQL operations
│   │   │   ├── queries/             # 13+ query definitions
│   │   │   └── mutations/           # 13+ mutation definitions
│   │   ├── hooks/                   # Custom React hooks
│   │   ├── i18n/                    # Internationalization
│   │   ├── pages/                   # Route page components
│   │   ├── theme/                   # MUI theme configuration
│   │   ├── types/                   # TypeScript types
│   │   ├── utils/                   # Utility functions
│   │   └── styles/                  # Shared styles
│   ├── public/                      # Static assets
│   ├── package.json                 # Frontend dependencies
│   ├── vite.config.ts               # Build configuration
│   └── codegen.ts                   # GraphQL codegen config
│
├── src/                             # .NET Backend (Part: backend)
│   ├── App.MtgDiscovery.GraphQL/    # 🎯 ENTRY POINT - GraphQL API
│   │   ├── Authentication/          # Auth0 JWT authentication
│   │   ├── Queries/                 # GraphQL query resolvers
│   │   ├── Mutations/               # GraphQL mutation resolvers
│   │   ├── Entities/                # GraphQL types and args
│   │   │   ├── Args/                # Input argument entities
│   │   │   └── Types/               # Output type definitions
│   │   ├── Actions/                 # Action mappers
│   │   ├── Schemas/                 # Schema extensions
│   │   └── Program.cs               # Application entry point
│   │
│   ├── Lib.MtgDiscovery.Entry/      # Entry Layer - Request handling
│   │   ├── Apis/                    # Entry service interfaces
│   │   ├── Commands/                # Command handlers
│   │   │   └── Collections/         # Collection commands
│   │   ├── Queries/                 # Query handlers
│   │   │   └── User/                # User queries
│   │   └── Entities/                # Entry layer entities
│   │
│   ├── Lib.Shared.*/                # Shared Layer - Cross-cutting
│   │   ├── Lib.Shared.Abstractions/ # Action patterns, validators
│   │   ├── Lib.Shared.DataModels/   # Entity interfaces (Arg/Itr/Ouf)
│   │   └── Lib.Shared.Invocation/   # Operation response patterns
│   │
│   ├── Lib.Domain.*/                # Domain Layer - Business logic
│   │   ├── Lib.Domain.Artists/      # Artist domain services
│   │   ├── Lib.Domain.Cards/        # Card domain services
│   │   ├── Lib.Domain.Collections/  # Collection domain services
│   │   ├── Lib.Domain.Sets/         # Set domain services
│   │   ├── Lib.Domain.User/         # User domain services
│   │   ├── Lib.Domain.UserCards/    # User cards domain
│   │   ├── Lib.Domain.UserSetCards/ # User set cards domain
│   │   ├── Lib.Domain.UserWishlistCards/
│   │   ├── Lib.Domain.SealedProducts/
│   │   └── Lib.Domain.UserSealedProducts/
│   │
│   ├── Lib.Aggregator.*/            # Aggregator Layer - Data aggregation
│   │   ├── Lib.Aggregator.Artists/
│   │   ├── Lib.Aggregator.Cards/
│   │   ├── Lib.Aggregator.Collections/
│   │   ├── Lib.Aggregator.Scryfall.Shared/
│   │   ├── Lib.Aggregator.SealedProducts/
│   │   ├── Lib.Aggregator.Sets/
│   │   ├── Lib.Aggregator.User/
│   │   ├── Lib.Aggregator.UserCards/
│   │   ├── Lib.Aggregator.UserSetCards/
│   │   ├── Lib.Aggregator.UserWishlistCards/
│   │   └── Lib.Aggregator.UserSealedProducts/
│   │
│   ├── Lib.Adapter.*/               # Adapter Layer - External integration
│   │   ├── Lib.Adapter.Artists/
│   │   ├── Lib.Adapter.Cards/
│   │   ├── Lib.Adapter.Collections/
│   │   ├── Lib.Adapter.Sets/
│   │   ├── Lib.Adapter.User/
│   │   ├── Lib.Adapter.UserCards/
│   │   ├── Lib.Adapter.UserSetCards/
│   │   ├── Lib.Adapter.UserWishlistCards/
│   │   ├── Lib.Adapter.SealedProducts/
│   │   ├── Lib.Adapter.UserSealedProducts/
│   │   └── Lib.Adapter.Scryfall.Cosmos/  # Cosmos DB operators
│   │       ├── Apis/
│   │       │   ├── CosmosItems/     # ExtEntity documents
│   │       │   ├── Mappers/         # Entity mappers
│   │       │   └── Operators/       # Cosmos operators
│   │       │       ├── Gophers/     # Read operations
│   │       │       ├── Scribes/     # Write operations
│   │       │       ├── Janitors/    # Delete operations
│   │       │       └── Inquisitors/ # Query operations
│   │       └── Cosmos/
│   │           └── Containers/      # Container definitions
│   │
│   ├── Lib.Cosmos/                  # Infrastructure - Cosmos DB
│   │   ├── Adapters/                # Client adapters
│   │   ├── Apis/                    # Container operations
│   │   └── Operators/               # Query operators
│   │
│   ├── Lib.Universal/               # Infrastructure - Utilities
│   │   ├── Configuration/           # Config patterns
│   │   ├── ServiceLocator/          # DI helpers
│   │   └── Primitives/              # Base types
│   │
│   ├── Lib.Scryfall.*/              # Scryfall Integration
│   │   ├── Lib.Scryfall.Ingestion/  # Bulk data ingestion
│   │   └── Lib.Scryfall.Shared/     # Shared utilities
│   │
│   ├── Cli.*/                       # CLI Tools
│   │   ├── Cli.MtgDiscovery.DataMigration/
│   │   ├── Cli.MtgDiscovery.PriceUpdate/
│   │   ├── Cli.MtgDiscovery.UserDataReconciler/
│   │   ├── Cli.Sealed.ImageScraper/
│   │   └── Cli.Sealed.Ingestion/
│   │
│   ├── Example.*/                   # Example Applications
│   │   ├── Example.Core/
│   │   ├── Example.LibCosmos/
│   │   ├── Example.Scryfall.ApiDemo/
│   │   ├── Example.Scryfall.BulkIngestion/
│   │   └── Example.Scryfall.FilterTest/
│   │
│   ├── TestConvenience.Core/        # Testing Utilities
│   │   ├── Fakes/                   # Fake implementations
│   │   └── TypeWrappers/            # Reflection helpers
│   │
│   ├── *.Tests/                     # Test Projects (paired with source)
│   │
│   ├── Directory.Build.props        # Central build configuration
│   ├── Directory.Packages.props     # Centralized package versions
│   └── MtgDiscoveryVibe.sln         # Solution file
│
├── .docs/                           # Feature Plans and Designs
│   ├── 010-sqlite-migration-and-search-design.md
│   ├── 040a-COLLECTION_IDENTITY_IMPLEMENTATION_PLAN.md
│   └── ...
│
├── .pipelines/                      # CI/CD Configuration
├── .github/                         # GitHub configuration
├── .azuredevops/                    # Azure DevOps configuration
├── scripts/                         # Utility scripts
├── specs/                           # Specifications
│
├── CLAUDE.md                        # AI Assistant Guide
├── PRD.md                           # Product Requirements
├── CODING_CRITERIA.md               # Coding Standards
├── TESTING_GUIDELINES.md            # Testing Patterns
├── Architecture_Layers_Patterns.md  # Layer Patterns
├── TECHNICAL_ARCHITECTURE_MANUAL.md # Architecture Manual
└── README.md                        # Project Overview
```

---

## Critical Directories

### Backend Entry Points

| Path | Purpose |
|------|---------|
| `src/App.MtgDiscovery.GraphQL/Program.cs` | Main application entry point |
| `src/App.MtgDiscovery.GraphQL/Queries/` | GraphQL query resolvers |
| `src/App.MtgDiscovery.GraphQL/Mutations/` | GraphQL mutation resolvers |

### Layer Organization

| Layer | Pattern | Purpose |
|-------|---------|---------|
| App | `App.*` | GraphQL API, authentication |
| Entry | `Lib.MtgDiscovery.Entry` | Request validation, response formatting |
| Shared | `Lib.Shared.*` | Cross-cutting concerns, interfaces |
| Domain | `Lib.Domain.*` | Business logic, rules |
| Aggregator | `Lib.Aggregator.*` | Data aggregation, coordination |
| Adapter | `Lib.Adapter.*` | External system integration |
| Infrastructure | `Lib.Cosmos`, `Lib.Universal` | Core utilities |

### Frontend Organization

| Path | Purpose |
|------|---------|
| `client/src/components/atoms/` | Basic UI building blocks |
| `client/src/components/molecules/` | Composed components |
| `client/src/components/organisms/` | Complex UI sections |
| `client/src/graphql/` | GraphQL operation definitions |
| `client/src/generated/` | Auto-generated types/hooks |
| `client/src/contexts/` | React context providers |

---

## Integration Points

### Client → Backend

```
[React App] → Apollo Client → GraphQL HTTP → [HotChocolate API]
     ↓                                              ↓
Auth0 Token ────────────────────────────────→ JWT Validation
```

### Backend Data Flow

```
GraphQL Request
     ↓
App Layer (Queries/Mutations)
     ↓ ArgEntity
Entry Layer (Validation)
     ↓ ItrEntity
Domain Layer (Business Rules)
     ↓ ItrEntity
Aggregator Layer (Coordination)
     ↓ XfrEntity
Adapter Layer (Data Access)
     ↓ ExtEntity
Cosmos DB / External APIs
```

---

## File Counts

| Category | Count |
|----------|-------|
| Backend Projects (.csproj) | 70+ |
| Frontend Components (.tsx) | 100+ |
| GraphQL Operations | 26 |
| Entity Files | 100+ |
| Test Projects | 15+ |
| Documentation Files | 30+ |
