# Azure Pipelines Documentation

This directory contains Azure DevOps pipelines for deploying the MTG Discovery Vibe application to Azure.

## Directory Structure

```
.pipelines/
├── vars/                           # Variable definitions
│   ├── consts.yaml                 # Constants shared across all environments
│   ├── dev.yaml                    # Development environment variables
│   └── prod.yaml                   # Production environment variables
├── shared/                         # Shared templates
│   ├── resource-names.yml          # Resource name generation template
│   └── get-resource-info.yml       # Resource information retrieval template
├── infra/                          # Infrastructure pipeline
│   ├── infrastructure-pipeline.yml # Main infrastructure pipeline
│   └── steps/
│       ├── create-shared.yml       # Creates shared resources (RG, ACR)
│       └── create-environment.yml  # Creates environment-specific resources
├── deploy/                         # Deployment pipelines
│   ├── backend-pipeline.yml        # Backend deployment pipeline
│   ├── frontend-pipeline.yml       # Frontend deployment pipeline
│   └── steps/
│       └── verify-deployment.yml   # Deployment verification template
└── README.md                       # This file
```

## Resource Naming Convention

All Azure resources follow this naming convention:

**Format**: `{resourceShortName}-{appName}-{environment}-{regionShortName}-{sequenceNumber}`

**Example**: `ca-mtg-dev-wcus-01` (Container App for MTG Dev in West Central US, sequence 01)

### Resource Short Names

| Resource Type | Short Name |
|--------------|------------|
| Resource Group | `rg` |
| Static Web App | `swa` |
| Container Registry | `cr` |
| Container App | `ca` |
| Container App Environment | `cae` |
| Application Insights | `appi` |
| Cosmos DB | `cosmos` |
| Log Analytics | `log` |

### Constants

- **appName**: `mtg`
- **region**: `westcentralus` (West Central US)
- **regionShortName**: `wcus`
- **sequenceNumber**: `01` (per environment)

## Azure Resources

### Shared Resources (rg-mtg-shared-wcus-01)

- **Container Registry**: `crmtgsharedwcus01`
  - Shared between dev and prod environments
  - Stores Docker images for backend application

### Per-Environment Resources

Each environment (dev, prod) has:

- **Resource Group**: `rg-mtg-{env}-wcus-01`
- **App Configuration**: `appconfig-mtg-{env}-wcus-01`
  - Centralized configuration management
  - Stores Auth0 settings and other runtime configuration
  - Free tier (1,000 requests/day)
  - Accessed via managed identity (RBAC)
  - **Location**: Dev in East US, Prod in West Central US (Free tier limit: 1 per region)
- **Cosmos DB**: `cosmos-mtg-{env}-wcus-01`
  - Serverless capacity mode (pay-per-request)
  - Account only, databases/containers created separately
- **Container Apps Environment**: `cae-mtg-{env}-wcus-01`
  - Consumption model with scale-to-zero capability
- **Container App**: `ca-mtg-{env}-wcus-01`
  - Runs the .NET GraphQL API
  - Consumption: 0.5 CPU, 1.0 GB memory
  - Scale: 0-10 replicas (dev: 0-3)
- **Static Web App**: `swa-mtg-{env}-wcus-01`
  - Hosts the React frontend
  - Dev: Free tier, Prod: Standard tier
- **Application Insights**: `appi-mtg-{env}-wcus-01`
  - Application monitoring and diagnostics
- **Log Analytics**: `log-mtg-{env}-wcus-01`
  - Container Apps logging
- **Managed Identity**: `id-mtg-{env}-wcus-01`
  - User-assigned identity for RBAC access

## Pipelines

### 1. Infrastructure Pipeline

**File**: `.pipelines/infra/infrastructure-pipeline.yml`

**Purpose**: Creates all Azure resources for both dev and prod environments

**Trigger**: Manual only

**Stages**:
1. **CreateSharedResources**: Creates shared resource group and ACR
2. **CreateDevEnvironment**: Creates all dev environment resources
3. **CreateProdEnvironment**: Creates all prod environment resources

**How to Run**:
1. Navigate to Azure DevOps → Pipelines
2. Create new pipeline from `infra/infrastructure-pipeline.yml`
3. Run manually when you need to create or recreate infrastructure

**What It Creates**:
- All resource groups (shared, dev, prod)
- Azure Container Registry (shared)
- User-assigned managed identities (dev, prod)
- App Configuration stores with Auth0 settings (dev, prod)
- Cosmos DB accounts in serverless mode (dev, prod)
- Container Apps environments and apps (dev, prod)
- Static Web Apps (dev, prod)
- Application Insights (dev, prod)
- Log Analytics workspaces (dev, prod)
- RBAC role assignments (Cosmos DB, ACR, App Configuration)

### 2. Backend Deployment Pipeline

**File**: `.pipelines/deploy/backend-pipeline.yml`

**Purpose**: Builds and deploys the .NET GraphQL API to Container Apps

**Trigger**: Automatic on changes to `src/**` or the pipeline file

**Stages**:
1. **Build**: Builds container image using `dotnet publish /t:PublishContainer`
2. **DeployDev**: Deploys to dev environment (automatic)
3. **DeployProd**: Deploys to production environment (requires manual approval)

**Build Process**:
- Uses .NET 9.0 SDK
- Runs `dotnet publish /t:PublishContainer` to build and push image to ACR
- Image tagged with build ID for traceability

**Deployment Process**:
- Retrieves Cosmos DB and App Insights connection info
- Updates Container App with new image
- Configures environment variables (Auth0, Cosmos, App Insights)
- Verifies deployment success

### 3. Frontend Deployment Pipeline

**File**: `.pipelines/deploy/frontend-pipeline.yml`

**Purpose**: Builds and deploys the React application to Static Web Apps

**Trigger**: Automatic on changes to `client/**` or the pipeline file

**Stages**:
1. **Build**: Builds React application with npm
2. **DeployDev**: Deploys to dev Static Web App (automatic)
3. **DeployProd**: Deploys to production Static Web App (requires manual approval)

**Build Process**:
- Uses Node.js 20.x
- Runs `npm ci` and `npm run build`
- Creates production build in `client/dist`

**Deployment Process**:
- Retrieves Container App FQDN for GraphQL endpoint
- Creates environment-specific `.env.production` file
- Rebuilds with environment-specific configuration
- Deploys to Azure Static Web Apps

## Setup Instructions

### Prerequisites

1. **Azure Subscription** with permissions to create resources
2. **Azure DevOps Organization** and project
3. **Auth0 Account** for authentication (or remove Auth0 config if not needed)

### 1. Create Azure Service Connection

1. Navigate to Azure DevOps → Project Settings → Service connections
2. Create new service connection:
   - Type: **Azure Resource Manager**
   - Authentication: **Service Principal (automatic)**
   - Scope: **Subscription**
   - Name: `azure-service-connection` (or update in pipeline variables)
3. Grant access to all pipelines or specific pipelines

### 2. Configure Auth0 (if using authentication)

1. Create Auth0 applications:
   - One for dev environment
   - One for prod environment
2. Configure allowed callback URLs to include Static Web App URLs
3. Note the following for each environment:
   - Domain (e.g., `your-tenant.auth0.com`)
   - API Audience (e.g., `https://api.mtgdiscovery.dev`)
   - Client ID (for frontend SPA)

### 3. Create Variable Groups in Azure DevOps

Create two variable groups in Azure DevOps Library:

#### Variable Group: `mtg-dev-secrets`

| Variable Name | Value | Secret? |
|--------------|-------|---------|
| `auth0Domain` | Your Auth0 dev domain | No |
| `auth0Audience` | Your Auth0 dev API audience | No |
| `auth0ClientId` | Your Auth0 dev client ID | No |

#### Variable Group: `mtg-prod-secrets`

| Variable Name | Value | Secret? |
|--------------|-------|---------|
| `auth0Domain` | Your Auth0 prod domain | No |
| `auth0Audience` | Your Auth0 prod API audience | No |
| `auth0ClientId` | Your Auth0 prod client ID | No |

**Note**: These variable groups should be linked to the deployment pipelines.

### 4. Create Environments in Azure DevOps

1. Navigate to Azure DevOps → Pipelines → Environments
2. Create `dev` environment:
   - No approvals needed
3. Create `production` environment:
   - Add approval checks (require manual approval before deployment)
   - Add branch control (only `main` branch can deploy to prod)

### 5. Run Infrastructure Pipeline

1. Create pipeline from `infra/infrastructure-pipeline.yml`
2. Run the pipeline manually
3. Wait for all resources to be created (can take 10-15 minutes)
4. Verify resources in Azure Portal

### 6. Create Deployment Pipelines

#### Backend Pipeline

1. Create pipeline from `deploy/backend-pipeline.yml`
2. Link variable groups (`mtg-dev-secrets`, `mtg-prod-secrets`)
3. Pipeline will trigger automatically on `src/**` changes

#### Frontend Pipeline

1. Create pipeline from `deploy/frontend-pipeline.yml`
2. Link variable groups (`mtg-dev-secrets`, `mtg-prod-secrets`)
3. Pipeline will trigger automatically on `client/**` changes

## Usage

### First Deployment

1. **Run Infrastructure Pipeline** (one time):
   ```
   Pipelines → infrastructure-pipeline → Run
   ```
   This creates all Azure resources.

2. **Push Code to Trigger Deployments**:
   - Commit changes to `src/**` → triggers backend pipeline
   - Commit changes to `client/**` → triggers frontend pipeline

3. **Automatic Flow**:
   - Build stage runs
   - Dev stage deploys automatically
   - Prod stage waits for manual approval

4. **Approve Production Deployment**:
   - Navigate to pipeline run
   - Review changes
   - Click "Approve" on production stage

### Subsequent Deployments

Just push code changes - pipelines run automatically:
- **Backend**: Changes to `src/**` trigger backend-pipeline
- **Frontend**: Changes to `client/**` trigger frontend-pipeline

### Manual Deployment

To manually trigger a deployment:
1. Navigate to Pipelines
2. Select the pipeline (backend or frontend)
3. Click "Run pipeline"
4. Select branch and run

## Configuration Management

### Azure App Configuration

**Resource Locations**:
- **Dev**: East US (`appconfig-mtg-dev-wcus-01`)
- **Prod**: West Central US (`appconfig-mtg-prod-wcus-01`)
- **Why different?**: Azure allows only 1 Free tier App Configuration per region per subscription

**What's Stored**:
- Auth0 configuration (Domain, Audience, Client ID)
- Other runtime configuration values
- Environment-specific settings

**How to Access**:
- Backend: Use Azure App Configuration provider with managed identity
- Values stored with keys like `Auth0:Domain`, `Auth0:Audience`, `Auth0:ClientId`
- Pipeline automatically populates Auth0 settings during infrastructure creation

**How to Update**:
```bash
# Update a configuration value in dev (East US)
az appconfig kv set \
  --name appconfig-mtg-dev-wcus-01 \
  --key "Auth0:Domain" \
  --value "your-new-value" \
  --yes

# Update a configuration value in prod (West Central US)
az appconfig kv set \
  --name appconfig-mtg-prod-wcus-01 \
  --key "Auth0:Domain" \
  --value "your-new-value" \
  --yes
```

**RBAC Access**:
- Managed identity has "App Configuration Data Reader" role
- No connection strings needed
- Configuration changes take effect immediately (no redeployment required)

### Backend Container App Environment Variables

Set automatically by deployment pipeline:

- `ASPNETCORE_ENVIRONMENT`: `Development` (dev) or `Production` (prod)
- `CosmosConfiguration__Endpoint`: Cosmos DB endpoint (no key - uses managed identity)
- `CosmosConfiguration__DatabaseName`: `MtgDiscoveryDb`
- `APPLICATIONINSIGHTS_CONNECTION_STRING`: App Insights connection (secret)
- `AZURE_CLIENT_ID`: Managed identity client ID (for RBAC authentication)

### Frontend Static Web App Environment Variables

Set during build (written to `.env.production`):

- `VITE_GRAPHQL_ENDPOINT`: Backend GraphQL endpoint URL
- `VITE_AUTH0_DOMAIN`: Auth0 tenant domain
- `VITE_AUTH0_CLIENT_ID`: Auth0 client ID for frontend
- `VITE_AUTH0_AUDIENCE`: Auth0 API audience

## Troubleshooting

### Infrastructure Pipeline Fails

**Issue**: Resource group already exists
- **Solution**: Delete the resource group or modify sequence number in variable files

**Issue**: Cosmos DB creation times out
- **Solution**: Cosmos DB can take 5-10 minutes. Re-run the failed stage.

**Issue**: ACR name already taken
- **Solution**: Container Registry names are globally unique. Modify `appName` or `sequenceNumber`

### Backend Deployment Fails

**Issue**: `dotnet publish /t:PublishContainer` fails
- **Solution**: Ensure project file has `<EnableSdkContainerSupport>true</EnableSdkContainerSupport>`

**Issue**: Container App not starting
- **Solution**: Check logs: `az containerapp logs show --name ca-mtg-dev-wcus-01 --resource-group rg-mtg-dev-wcus-01`

**Issue**: Auth0 configuration error
- **Solution**: Verify Auth0 domain, audience, and client ID in variable groups

### Frontend Deployment Fails

**Issue**: Static Web App deployment token invalid
- **Solution**: Token is retrieved automatically. Check if Static Web App exists.

**Issue**: Build fails with missing environment variables
- **Solution**: Ensure Container App has been deployed first (frontend needs backend URL)

**Issue**: GraphQL endpoint not accessible
- **Solution**: Verify Container App is running and ingress is configured

### Container App Not Scaling

**Issue**: App stays at 0 replicas
- **Solution**: This is correct for consumption model with scale-to-zero. First request will cold start the app.

**Issue**: App not scaling up under load
- **Solution**: Check scaling rules and CPU/memory limits

## Monitoring

### Application Insights

Each environment has Application Insights configured:

- **Dev**: `appi-mtg-dev-wcus-01`
- **Prod**: `appi-mtg-prod-wcus-01`

Access in Azure Portal:
1. Navigate to Application Insights resource
2. View metrics, logs, and performance data

### Container App Logs

View real-time logs:

```bash
az containerapp logs show \
  --name ca-mtg-dev-wcus-01 \
  --resource-group rg-mtg-dev-wcus-01 \
  --follow
```

### Cosmos DB Monitoring

View Cosmos DB metrics in Azure Portal:
1. Navigate to Cosmos DB account
2. View request units, latency, availability

## Cost Optimization

### Development Environment

- Container App: **Scale to zero** (min replicas = 0) - no cost when idle
- Static Web App: **Free tier** - no cost for basic hosting
- App Configuration: **Free tier** - no cost for up to 1,000 requests/day
- Cosmos DB: **Serverless** - ~$0.25 per million RUs consumed + ~$0.25/GB storage - typically $3-10/month for low usage
- Application Insights: **Pay-as-you-go** - minimal cost for dev traffic

**Estimated Dev Cost**: ~$10-20/month (low usage)

### Production Environment

- Container App: **Min 1 replica** - always available, auto-scale to 10
- Static Web App: **Standard tier** - ~$9/month
- App Configuration: **Free tier** - sufficient for low traffic (can upgrade to Standard if needed)
- Cosmos DB: **Serverless** - ~$0.25 per million RUs consumed + ~$0.25/GB storage - scales with actual usage
- Application Insights: **Pay-as-you-go** - based on usage

**Estimated Prod Cost**: ~$20-60/month (low to moderate traffic)

**Note**: Serverless Cosmos DB pricing scales with actual usage. For very low traffic sites, costs can be as low as a few dollars per month. Max limits: 5,000 RU/s burst per container, 50 GB per container. App Configuration Free tier supports 1,000 requests/day which is sufficient for most low-traffic applications.

### Shared Resources

- Container Registry: **Basic tier** - ~$5/month

## Security Best Practices

1. **Secrets Management**:
   - Use Azure DevOps variable groups with secret variables
   - Consider Azure Key Vault for production secrets
   - Never commit secrets to source control

2. **Network Security**:
   - Container Apps have public ingress (consider VNet integration for prod)
   - Static Web App has automatic DDoS protection
   - Use Azure Front Door for additional WAF protection

3. **Authentication**:
   - Auth0 handles authentication and authorization
   - Backend validates JWT tokens
   - Use separate Auth0 tenants for dev/prod

4. **Resource Access**:
   - Use managed identities where possible
   - Rotate Cosmos DB keys regularly
   - Use Azure RBAC for resource access control

## Next Steps

1. **Custom Domains**:
   - Configure custom domain for Static Web App
   - Configure custom domain for Container App if needed

2. **Database Setup**:
   - Create Cosmos DB databases and containers
   - Run data migration/seeding scripts

3. **Monitoring and Alerts**:
   - Configure Application Insights alerts
   - Set up availability tests
   - Configure Log Analytics queries

4. **CI/CD Enhancements**:
   - Add automated testing stages
   - Add security scanning (container images, dependencies)
   - Add performance testing

## Support

For issues or questions:
- Check Azure DevOps pipeline logs
- Review Azure Portal resource health
- Consult Azure Container Apps documentation
- Consult Azure Static Web Apps documentation
