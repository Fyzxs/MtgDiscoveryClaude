# Development Guide

Generated: 2026-01-29

## Prerequisites

### Required Software

| Software | Version | Purpose |
|----------|---------|---------|
| .NET SDK | 10.0+ | Backend development |
| Node.js | 18+ | Frontend development |
| npm | Latest | Package management |
| Visual Studio 2022 or VS Code | Latest | IDE |
| Git | Latest | Version control |

### Required Accounts/Access

- Azure subscription (Cosmos DB, Blob Storage)
- Auth0 tenant (authentication)
- Azure DevOps (CI/CD, work items)

---

## Quick Start

### Clone and Setup

```bash
# Clone repository
git clone https://github.com/fyzxs/MtgDiscoveryVibe.git
cd MtgDiscoveryVibe
```

### Backend Setup

```bash
# Build entire solution
dotnet build src/MtgDiscoveryVibe.sln

# Run tests
dotnet test src/MtgDiscoveryVibe.sln

# Run GraphQL API
dotnet run --project src/App.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL.csproj
```

### Frontend Setup

```bash
# Navigate to client
cd client

# Install dependencies
npm install

# Generate GraphQL types
npm run codegen

# Start development server
npm run dev
```

---

## Development Commands

### Backend Commands

```bash
# Build
dotnet build src/MtgDiscoveryVibe.sln

# Build specific project
dotnet build src/Lib.Cosmos/Lib.Cosmos.csproj

# Run all tests
dotnet test src/MtgDiscoveryVibe.sln

# Run specific test project
dotnet test src/Lib.Cosmos.Tests/Lib.Cosmos.Tests.csproj

# Run with coverage
dotnet test src/MtgDiscoveryVibe.sln --collect:"XPlat Code Coverage"

# Run single test by method name
dotnet test --filter "FullyQualifiedName~MethodName"

# Run GraphQL API
dotnet run --project src/App.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL.csproj

# Clean build artifacts
dotnet clean src/MtgDiscoveryVibe.sln

# Update NuGet packages (PowerShell from src/)
./slnNugetUpdater.ps1
```

### Frontend Commands

```bash
# Development server
npm run dev

# Build for production
npm run build

# Type-checked build
npm run build:check

# Run linting
npm run lint

# Generate GraphQL types
npm run codegen

# Watch GraphQL schema changes
npm run codegen:watch

# Preview production build
npm run preview

# Deploy to preview environment
npm run deploy:preview

# Deploy to production
npm run deploy:production
```

---

## Environment Configuration

### Backend Configuration

Configuration is loaded from:
1. `appsettings.json` - Base configuration
2. `appsettings.{Environment}.json` - Environment-specific
3. Azure App Configuration - Runtime configuration
4. Environment variables

Key configuration sections:
- Cosmos DB connection strings
- Auth0 domain and audience
- Application Insights connection
- Blob Storage connection

### Frontend Configuration

Environment variables (`.env.local`):
```
VITE_AUTH0_DOMAIN=your-auth0-domain
VITE_AUTH0_CLIENT_ID=your-auth0-client-id
VITE_AUTH0_REDIRECT_URI=http://localhost:5173/signin-redirect
VITE_GRAPHQL_ENDPOINT=https://localhost:65203/graphql
```

---

## Coding Standards

### Backend (C# .NET)

- **File-scoped namespaces** - Always use
- **No greater than operators** - Use `<` only
- **No boolean negation** - Use `is false` instead of `!`
- **ConfigureAwait(false)** - On all async calls
- **Sealed or abstract** - All classes must be one or the other
- **Interface for every class** - 1:1 mapping
- **No nulls** - Use Null Object pattern
- **No enums** - Use class hierarchies
- **No public statics** - Except MonoState pattern

### Frontend (TypeScript/React)

- **Material-UI sx props** - Primary styling method
- **No Tailwind classes** - Being phased out
- **Atomic design** - Organize by complexity
- **Generated GraphQL types** - No manual type definitions
- **Named imports** - Prefer over default imports

---

## Testing

### Backend Testing

- **Framework**: MSTest with AwesomeAssertions
- **Pattern**: Arrange-Act-Assert
- **Naming**: `MethodName_Scenario_ExpectedBehavior`
- **Fakes over mocks** - Create fake implementations
- **Self-contained tests** - No test class variables

```bash
# Run all tests
dotnet test src/MtgDiscoveryVibe.sln

# Run with verbose output
dotnet vstest ProjectName.Tests/bin/Debug/net10.0/ProjectName.Tests.dll --logger:"console;verbosity=normal"
```

### Frontend Testing

- Component testing with React Testing Library
- Mock Apollo Client for GraphQL tests
- Mock Auth0 for authentication tests

---

## Git Workflow

### Branch Naming

- `main` - Production-ready code
- `feature/*` - New features
- `bugfix/*` - Bug fixes
- `NNN-*` - Work item number prefix

### Commit Standards

- Clear, concise commit messages
- Reference work item numbers
- Atomic commits (one logical change)

### Pull Request Process

1. Create PR from feature branch to main
2. Fill out PR template (`.azuredevops/pull_request_template.md`)
3. Pass CI/CD checks
4. Code review approval
5. Merge

---

## Azure DevOps Integration

### Work Items

```bash
# Show work item
az boards work-item show --id <id>

# List work items
az boards work-item list

# Create task
az boards work-item create --type Task --title "Task Title"
```

### Pull Requests

```bash
# Create PR
az repos pr create --title "PR Title"

# List PRs
az repos pr list

# Show PR details
az repos pr show --id <pr-id>
```

---

## Troubleshooting

### Common Issues

**Backend won't build:**
- Check .NET 10.0 SDK is installed
- Run `dotnet restore` first
- Check for NuGet source configuration

**Frontend codegen fails:**
- Ensure backend is running on port 65203
- Check schema endpoint is accessible
- Verify HTTPS certificate is trusted

**Tests fail:**
- Check test database/emulator is running
- Verify environment configuration
- Check for async/await issues (ConfigureAwait)

**Auth issues:**
- Verify Auth0 configuration
- Check JWT token expiry
- Validate redirect URIs

---

## IDE Setup

### Visual Studio 2022

- Install .NET 10.0 SDK workload
- Install ASP.NET and web development workload
- Enable EditorConfig support

### VS Code

Recommended extensions:
- C# (Microsoft)
- ESLint
- Prettier
- GraphQL
- Azure Tools

### JetBrains Rider

- Full solution support
- EditorConfig integration
- Built-in terminal
