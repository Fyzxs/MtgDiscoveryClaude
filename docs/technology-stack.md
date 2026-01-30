# Technology Stack

Generated: 2026-01-29

## Overview

MtgDiscoveryVibe is a full-stack Magic: The Gathering collection management platform with a React frontend and .NET backend communicating via GraphQL.

---

## Frontend (client/)

### Core Framework

| Technology | Version | Purpose |
|------------|---------|---------|
| React | 19.1.1 | UI framework with concurrent features |
| TypeScript | 5.8.3 | Type-safe JavaScript |
| Vite | 7.1.12 | Build tool and dev server |

### UI & Styling

| Technology | Version | Purpose |
|------------|---------|---------|
| Material-UI (@mui/material) | 7.3.1 | Primary UI component library |
| @emotion/react | 11.14.0 | CSS-in-JS styling engine |
| @emotion/styled | 11.14.1 | Styled components |
| @mui/icons-material | 7.3.1 | Material icons |

### API & State

| Technology | Version | Purpose |
|------------|---------|---------|
| Apollo Client | 4.0.0 | GraphQL client with caching |
| GraphQL | 16.11.0 | Query language |
| @graphql-codegen/cli | 5.0.7 | Type generation from schema |

### Authentication

| Technology | Version | Purpose |
|------------|---------|---------|
| @auth0/auth0-react | 2.4.0 | Auth0 React SDK |

### Routing & Navigation

| Technology | Version | Purpose |
|------------|---------|---------|
| react-router-dom | 7.9.1 | Client-side routing |

### Internationalization

| Technology | Version | Purpose |
|------------|---------|---------|
| i18next | 25.5.2 | i18n framework |
| react-i18next | 16.0.0 | React bindings |
| i18next-browser-languagedetector | 8.2.0 | Language detection |
| i18next-http-backend | 3.0.2 | Translation loading |

### Utilities

| Technology | Version | Purpose |
|------------|---------|---------|
| date-fns | 4.1.0 | Date manipulation |

### Build Configuration

- **Chunking Strategy**: Manual chunks for React, MUI, Apollo, Auth0
- **Minification**: esbuild for production builds
- **Development**: WSL2 polling enabled, GraphQL proxy to backend

---

## Backend (src/)

### Core Framework

| Technology | Version | Purpose |
|------------|---------|---------|
| .NET | 10.0 | Runtime and SDK |
| C# | Latest (via LangVersion) | Programming language |

### API Layer

| Technology | Version | Purpose |
|------------|---------|---------|
| HotChocolate.AspNetCore | 15.1.11 | GraphQL server |
| HotChocolate.AspNetCore.Authorization | 15.1.11 | GraphQL authorization |
| HotChocolate.Data | 15.1.11 | GraphQL data layer |

### Database & Storage

| Technology | Version | Purpose |
|------------|---------|---------|
| Microsoft.Azure.Cosmos | 3.56.0 | Azure Cosmos DB SDK |
| Azure.Storage.Blobs | 12.25.0 | Azure Blob Storage |

### Authentication & Security

| Technology | Version | Purpose |
|------------|---------|---------|
| Microsoft.AspNetCore.Authentication.JwtBearer | 10.0.1 | JWT authentication |
| Microsoft.IdentityModel.JsonWebTokens | 8.14.0 | JWT handling |
| System.IdentityModel.Tokens.Jwt | 8.14.0 | JWT tokens |
| Azure.Identity | 1.17.1 | Azure authentication |

### Configuration & Hosting

| Technology | Version | Purpose |
|------------|---------|---------|
| Microsoft.Extensions.Configuration | 10.0.1 | Configuration management |
| Microsoft.Azure.AppConfiguration.AspNetCore | 8.4.0 | Azure App Config |
| Microsoft.Extensions.Hosting | 10.0.1 | Application hosting |
| Microsoft.Extensions.DependencyInjection | 9.0.9 | DI container |
| Microsoft.Extensions.Caching.Memory | 10.0.1 | In-memory caching |

### Monitoring & Telemetry

| Technology | Version | Purpose |
|------------|---------|---------|
| Microsoft.ApplicationInsights.AspNetCore | 2.23.0 | Application Insights |
| Azure.Monitor.OpenTelemetry.AspNetCore | 1.3.0 | OpenTelemetry |

### Resilience

| Technology | Version | Purpose |
|------------|---------|---------|
| Polly | 8.6.5 | Resilience and transient fault handling |
| Polly.RateLimiting | 8.6.5 | Rate limiting |

### Serialization

| Technology | Version | Purpose |
|------------|---------|---------|
| Newtonsoft.Json | 13.0.4 | JSON serialization (primary) |

### Azure Functions (Optional)

| Technology | Version | Purpose |
|------------|---------|---------|
| Microsoft.Azure.Functions.Worker | 2.0.0 | Azure Functions runtime |
| Microsoft.Azure.Functions.Worker.Extensions.ServiceBus | 5.23.0 | Service Bus triggers |
| Azure.Messaging.ServiceBus | 7.20.1 | Service Bus messaging |

### Testing

| Technology | Version | Purpose |
|------------|---------|---------|
| MSTest.TestFramework | 4.0.2 | Test framework |
| MSTest.TestAdapter | 4.0.2 | Test adapter |
| AwesomeAssertions | 9.3.0 | Fluent assertions |
| coverlet.collector | 6.0.4 | Code coverage |

### Code Quality

| Technology | Version | Purpose |
|------------|---------|---------|
| Microsoft.CodeAnalysis | 4.14.0 | Roslyn analyzers |
| Microsoft.CodeAnalysis.BannedApiAnalyzers | 4.14.0 | API banning |
| JetBrains.Annotations | 2025.2.4 | Code annotations |

---

## Infrastructure

### Hosting

| Service | Purpose |
|---------|---------|
| Azure Container Apps | Backend API hosting |
| Azure Static Web Apps | Frontend hosting |

### Data Storage

| Service | Purpose |
|---------|---------|
| Azure Cosmos DB | Primary database (user data, cards, collections) |
| Azure Blob Storage | Card images, assets |

### Identity & Security

| Service | Purpose |
|---------|---------|
| Auth0 | User authentication and authorization |
| Azure Key Vault | Secrets management |

### Monitoring

| Service | Purpose |
|---------|---------|
| Azure Application Insights | APM and logging |
| Azure Monitor | Infrastructure monitoring |

### CI/CD

| Service | Purpose |
|---------|---------|
| Azure DevOps Pipelines | Build and deployment |
| GitHub Actions | (Alternative CI/CD) |

---

## Architecture Patterns

### Backend: MicroObjects Layered Architecture

```
Request → App → Entry → Shared → Domain → Aggregator → Adapter → Cosmos/External
         ↓      ↓        ↓        ↓          ↓           ↓
       ArgEntity → ItrEntity → ItrEntity → ItrEntity → XfrEntity → ExtEntity
```

**Key Principles:**
- Every concept explicitly represented as object
- No nulls (Null Object pattern)
- Immutable objects with private readonly fields
- Interface for every class (1:1 mapping)
- Constructor injection only
- Composition over inheritance

### Frontend: Atomic Design

```
atoms → molecules → organisms → templates → pages
```

**Key Principles:**
- Component hierarchy by complexity
- Domain-organized folders (Cards/, Sets/, shared/)
- Material-UI sx props for styling
- Context-aware display components
- Generated GraphQL types and hooks

---

## Development Requirements

### Prerequisites

- .NET 10.0 SDK
- Node.js 18+ and npm
- Azure subscription (for cloud services)
- Auth0 tenant
- Visual Studio 2022 or VS Code

### Local Development

```bash
# Backend
dotnet build src/MtgDiscoveryVibe.sln
dotnet run --project src/App.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL.csproj

# Frontend
cd client
npm install
npm run codegen
npm run dev
```
