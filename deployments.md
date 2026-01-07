# Deployments Guide

This document describes the deployment scripts, infrastructure, and procedures for the MTG Discovery application.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Environment Overview](#environment-overview)
- [Resource Naming Convention](#resource-naming-convention)
- [Infrastructure Creation](#infrastructure-creation)
- [Backend Deployment](#backend-deployment)
- [Frontend Deployment](#frontend-deployment)
- [Data Migration](#data-migration)
- [Utility Scripts](#utility-scripts)

---

## Prerequisites

### Required Tools

| Tool | Purpose | Installation |
|------|---------|--------------|
| Azure CLI | Azure resource management | `winget install Microsoft.AzureCLI` |
| Docker | Container builds | [Docker Desktop](https://www.docker.com/products/docker-desktop) |
| .NET SDK 10.0 | Backend builds | `winget install Microsoft.DotNet.SDK.10` |
| Node.js | Frontend builds | `winget install OpenJS.NodeJS.LTS` |
| SWA CLI | Static Web App deployment | `npm install -g @azure/static-web-apps-cli` |

### Azure Authentication

```bash
# Login to Azure
az login

# Verify subscription
az account show
```

---

## Environment Overview

The application uses three environment tiers:

| Environment | Purpose | Scaling |
|-------------|---------|---------|
| **Shared** | Container Registry (shared across environments) | N/A |
| **Dev** | Development and preview deployments | 0-1 replicas |
| **Prod** | Production workloads | 1-2 replicas |

### Azure Regions

| Resource Type | Region |
|---------------|--------|
| Primary Resources | West Central US (`westcentralus`) |
| Static Web Apps | West US 2 (`westus2`) |
| Application Insights | West US 2 (`westus2`) |
| App Configuration (Dev) | East US (`eastus`) |
| App Configuration (Prod) | West Central US (`westcentralus`) |

---

## Resource Naming Convention

Resources follow the pattern: `{type}-{app}-{env}-{region}-{sequence}`

| Resource Type | Prefix | Example |
|---------------|--------|---------|
| Resource Group | `rg` | `rg-mtg-dev-wcus-01` |
| Container Registry | `cr` | `crmtgsharedwcus01` |
| Container App | `ca` | `ca-mtg-dev-wcus-01` |
| Container App Environment | `cae` | `cae-mtg-dev-wcus-01` |
| Static Web App | `swa` | `swa-mtg-dev-wcus-01` |
| Cosmos DB | `cosmos` | `cosmos-mtg-dev-wcus-01` |
| Application Insights | `appi` | `appi-mtg-dev-wcus-01` |
| Log Analytics | `log` | `log-mtg-dev-wcus-01` |
| Managed Identity | `id` | `id-mtg-dev-wcus-01` |
| App Configuration | `appconfig` | `appconfig-mtg-dev-wcus-01` |

---

## Infrastructure Creation

Creates all Azure resources needed for the application. Use when setting up new environments or when Azure DevOps pipelines are unavailable.

### Scripts

| Script | Platform |
|--------|----------|
| `.pipelines/scripts/create-infrastructure.sh` | Bash (Linux/macOS/WSL) |
| `.pipelines/scripts/Create-Infrastructure.ps1` | PowerShell (Windows) |

### Usage

```bash
# Bash
./.pipelines/scripts/create-infrastructure.sh

# PowerShell
.\.pipelines\scripts\Create-Infrastructure.ps1
```

### Interactive Menu

```
Which environment(s) do you want to create?
1) Shared resources only
2) Dev environment (includes shared)
3) Prod environment (includes shared)
4) All (shared + dev + prod)
```

### Resources Created

#### Shared Resources
- Resource Group: `rg-mtg-shared-wcus-01`
- Azure Container Registry: `crmtgsharedwcus01` (Basic SKU, admin enabled)

#### Per-Environment Resources
- Resource Group
- Log Analytics Workspace
- Application Insights (linked to Log Analytics)
- App Configuration Store (Free SKU)
- Cosmos DB Account (Serverless, Session consistency)
- User-Assigned Managed Identity
- Container Apps Environment
- Container App (placeholder image initially)
- Static Web App (Free SKU)

### RBAC Assignments

The managed identity receives these roles:

| Role | Scope | Purpose |
|------|-------|---------|
| Cosmos DB Built-in Data Contributor | Cosmos DB Account | Read/write data |
| Cosmos DB Account Reader | Cosmos DB Account | Read account metadata |
| DocumentDB Account Contributor | Cosmos DB Account | Create databases/containers |
| AcrPull | Container Registry | Pull container images |
| App Configuration Data Reader | App Configuration | Read configuration |
| Reader | Resource Group | General metadata access |

### App Configuration Values

The following Auth0 settings are automatically populated:

| Key | Description |
|-----|-------------|
| `Auth0:Domain` | Auth0 tenant domain |
| `Auth0:Audience` | API audience identifier |
| `Auth0:ClientId` | Auth0 client ID |

---

## Backend Deployment

Deploys the .NET GraphQL API to Azure Container Apps.

### Script

```
deploy-backend.sh
```

### Usage

```bash
# Interactive menu
./deploy-backend.sh

# Direct deployment
./deploy-backend.sh preview    # Deploy to preview
./deploy-backend.sh dev        # Deploy to dev
./deploy-backend.sh production # Deploy to production (requires confirmation)

# Status and logs
./deploy-backend.sh status           # Show deployment status
./deploy-backend.sh logs dev         # Stream dev logs
./deploy-backend.sh logs prod        # Stream prod logs
```

### Interactive Menu Options

```
1) Deploy to Preview
2) Deploy to Dev
3) Deploy to Production
4) Show Status
5) Show Dev Logs
6) Show Prod Logs
7) Exit
```

### Deployment Process

1. **Retrieve ACR credentials** from Azure Container Registry
2. **Login to ACR** using Docker
3. **Build and push** container image using .NET's built-in container support
4. **Retrieve resource information** (Cosmos DB endpoint, App Insights connection string, Managed Identity)
5. **Configure ACR authentication** with managed identity
6. **Update Container App** with new image and environment variables

### Environment Variables Set

| Variable | Description |
|----------|-------------|
| `ASPNETCORE_ENVIRONMENT` | Development or Production |
| `APPLICATIONINSIGHTS_CONNECTION_STRING` | App Insights connection |
| `AZURE_CLIENT_ID` | Managed Identity client ID |

### Container Configuration

| Setting | Value |
|---------|-------|
| Port | 8080 |
| CPU | 0.5 cores |
| Memory | 1.0 Gi |
| Image Repository | `mtg-backend` |

### Endpoints

After deployment, the following endpoints are available:

- **GraphQL**: `https://{fqdn}/graphql`
- **Health**: `https://{fqdn}/health`

---

## Frontend Deployment

Deploys the React client to Azure Static Web Apps.

### Script

```
client/deploy.sh
```

### Usage

```bash
cd client

# Interactive menu
./deploy.sh

# Direct deployment
./deploy.sh preview    # Deploy to preview
./deploy.sh production # Deploy to production (requires confirmation)
./deploy.sh status     # Show deployment status
```

### Prerequisites

Environment files must exist before deployment:

| Environment | File | Purpose |
|-------------|------|---------|
| Preview | `.env.preview.local` | Preview Auth0 credentials |
| Production | `.env.production.local` | Production Auth0 credentials |

### Deployment Process

1. **Verify environment file** exists
2. **Build application** with appropriate mode (`npm run build -- --mode preview` or `npm run build`)
3. **Retrieve deployment token** from Azure Static Web App
4. **Deploy** using SWA CLI

### URLs

| Environment | URL |
|-------------|-----|
| Preview | `https://ambitious-smoke-0f17c3f1e-preview.westus2.3.azurestaticapps.net` |
| Production | `https://ambitious-smoke-0f17c3f1e.3.azurestaticapps.net` |

### Static Web App Configuration

| Setting | Value |
|---------|-------|
| App Name | `swa-mtg-dev-wcus-01` |
| Resource Group | `rg-mtg-dev-wcus-01` |
| Output Directory | `dist` |

---

## Data Migration

Cosmos DB data migration using the Data Migration Tool (DMT).

### Scripts

| Script | Purpose |
|--------|---------|
| `.cosmosConfig/CosmosReload/Run-AllDMT.ps1` | Run all DMT configurations |
| `.cosmosConfig/CosmosReload/Run-SetsDMT.ps1` | Migrate Sets data only |
| `.cosmosConfig/CosmosReload/Run-UserDMT.ps1` | Migrate User data only |

### Usage

```powershell
cd .cosmosConfig/CosmosReload

# Run all migrations
./Run-AllDMT.ps1

# Run Sets migrations only (SetItems, SetCards)
./Run-SetsDMT.ps1

# Run User migrations only (UserCards, UserSetCards)
./Run-UserDMT.ps1
```

### DMT Configuration Files

Located in `.cosmosConfig/CosmosReload/`:

| File | Description |
|------|-------------|
| `dmt-SetItems.json` | Set metadata migration |
| `dmt-SetCards.json` | Set cards migration |
| `dmt-UserCards.json` | User card collection migration |
| `dmt-UserSetCards.json` | User set-specific cards migration |
| `dmt-SetParentAssociations.json` | Set parent relationships |

### Prerequisites

The DMT executable must be installed at:
```
.cosmosConfig/CosmosReload/.dmt/dmt.exe
```

---

## Utility Scripts

### Application Insights Creation

Creates Application Insights resources if missing.

```bash
./create-app-insights.sh
```

Creates resources for both dev and prod environments, linked to their respective Log Analytics workspaces.

### NuGet Package Updater

Updates all NuGet packages in the solution to their latest minor versions.

```powershell
cd src
./slnNugetUpdater.ps1

# Or specify a path
./slnNugetUpdater.ps1 -slnPath ./MtgDiscoveryVibe.sln
```

**Note**: If encountering 401 errors, install the Azure Artifacts credential provider:
https://github.com/microsoft/artifacts-credprovider#manual-installation-on-windows

---

## Deployment Checklist

### First-Time Setup

- [ ] Install all prerequisite tools
- [ ] Login to Azure CLI (`az login`)
- [ ] Run infrastructure creation script
- [ ] Create frontend environment files (`.env.preview.local`, `.env.production.local`)
- [ ] Verify managed identity has correct RBAC assignments

### Backend Deployment

- [ ] Ensure Docker is running
- [ ] Run `./deploy-backend.sh {environment}`
- [ ] Verify health endpoint responds
- [ ] Check Application Insights for startup logs

### Frontend Deployment

- [ ] Ensure environment file exists for target environment
- [ ] Run `cd client && ./deploy.sh {environment}`
- [ ] Verify site loads at deployment URL
- [ ] Test authentication flow

### Post-Deployment Verification

- [ ] GraphQL endpoint accessible
- [ ] Authentication working (Auth0)
- [ ] Cosmos DB connectivity (check health endpoint)
- [ ] Application Insights receiving telemetry

---

## Troubleshooting

### Common Issues

| Issue | Solution |
|-------|----------|
| ACR login fails | Verify `az login` is current; check ACR admin is enabled |
| Container App update fails | Check managed identity has AcrPull role |
| App Insights missing | Run `./create-app-insights.sh` |
| Cosmos DB connection fails | Verify managed identity RBAC assignments |
| SWA deployment token error | Ensure Static Web App exists and you have access |

### Viewing Logs

```bash
# Backend logs
./deploy-backend.sh logs dev
./deploy-backend.sh logs prod

# Azure Portal
# Navigate to Container App > Log stream
```

### Useful Azure CLI Commands

```bash
# Check Container App status
az containerapp show --name ca-mtg-dev-wcus-01 --resource-group rg-mtg-dev-wcus-01

# View Container App revisions
az containerapp revision list --name ca-mtg-dev-wcus-01 --resource-group rg-mtg-dev-wcus-01

# Check Static Web App
az staticwebapp show --name swa-mtg-dev-wcus-01 --resource-group rg-mtg-dev-wcus-01

# View Cosmos DB endpoint
az cosmosdb show --name cosmos-mtg-dev-wcus-01 --resource-group rg-mtg-dev-wcus-01 --query documentEndpoint
```
