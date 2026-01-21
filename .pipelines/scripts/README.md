# Infrastructure Creation Scripts

This directory contains scripts for creating Azure infrastructure when Azure DevOps pipelines are unavailable.

## Available Scripts

- **`Create-Infrastructure.ps1`** - PowerShell version (recommended for Windows)
- **`create-infrastructure.sh`** - Bash version (Linux/macOS/WSL)

Both scripts create identical infrastructure.

## Prerequisites

1. **Azure CLI installed**:
   ```powershell
   # Check if installed
   az --version

   # Install on Windows (PowerShell as Administrator)
   winget install Microsoft.AzureCLI
   # OR download from: https://aka.ms/installazurecliwindows
   ```

2. **Logged into Azure**:
   ```powershell
   az login

   # Select the correct subscription
   az account list --output table
   az account set --subscription "Your Subscription Name"
   ```

3. **Permissions**: You need Contributor access to the subscription to create resources

## Create-Infrastructure.ps1 (PowerShell)

**Recommended for Windows users**

Creates all Azure resources for the MTG Discovery application.

### What It Creates

**Shared Resources**:
- Resource Group: `rg-mtg-shared-wcus-01`
- Container Registry: `crmtgsharedwcus01`

**Dev Environment** (in `rg-mtg-dev-wcus-01`):
- Log Analytics Workspace
- Application Insights
- App Configuration (in East US) with Auth0 settings pre-populated
- Cosmos DB (Serverless mode)
- User-assigned Managed Identity
- Container Apps Environment
- Container App (with placeholder image)
- Static Web App (Free tier)
- RBAC role assignments (Cosmos DB, ACR, App Configuration)

**Prod Environment** (in `rg-mtg-prod-wcus-01`):
- Same as dev, but:
  - App Configuration in West Central US
  - Container App min replicas = 1 (always on)
  - Static Web App Standard tier

### Usage (PowerShell)

```powershell
# Navigate to the scripts directory
cd D:\src\MtgDiscoveryVibeWorkspace\infrastuff\.pipelines\scripts

# Run the PowerShell script
.\Create-Infrastructure.ps1
```

**If you get an execution policy error**:
```powershell
# Run PowerShell as Administrator and execute:
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser

# Then run the script again
.\Create-Infrastructure.ps1
```

**Interactive Options**:
1. Shared resources only
2. Dev environment (includes shared)
3. Prod environment (includes shared)
4. All (shared + dev + prod)

## create-infrastructure.sh (Bash)

**For Linux/macOS/WSL users**

### Usage (Bash)

```bash
# Run the script
cd /mnt/d/src/MtgDiscoveryVibeWorkspace/infrastuff/.pipelines/scripts
./create-infrastructure.sh
```

**Interactive Options**:
1. Shared resources only
2. Dev environment (includes shared)
3. Prod environment (includes shared)
4. All (shared + dev + prod)

### Configuration

The script uses these default values (edit script to change):

```bash
# Application
APP_NAME="mtg"
LOCATION="westcentralus"

# Auth0 (same as in pipeline variables)
AUTH0_DOMAIN="dev-63szoyl0kt0p7e5q.us.auth0.com"
AUTH0_AUDIENCE="api://mtg-discovery"
AUTH0_CLIENT_ID="OXYTiA2a2SpN3cNf4Er4ix9DHHhLdYC5"

# Container App
CA_CPU="0.5"
CA_MEMORY="1.0Gi"
CA_MAX_REPLICAS=10

# Cosmos DB
COSMOS_CONSISTENCY="Session"
```

### Example Run (PowerShell)

```powershell
PS> .\Create-Infrastructure.ps1

[INFO] Starting infrastructure creation for MTG Discovery application
[INFO] Subscription: My Azure Subscription

Which environment(s) do you want to create?
1) Shared resources only
2) Dev environment (includes shared)
3) Prod environment (includes shared)
4) All (shared + dev + prod)
Enter choice (1-4): 2

[INFO] ========================================
[INFO] Creating Shared Resources
[INFO] ========================================
[INFO] Creating shared resource group: rg-mtg-shared-wcus-01
[INFO] Creating Azure Container Registry: crmtgsharedwcus01
[INFO] Shared resources created successfully

[INFO] ========================================
[INFO] Creating dev Environment
[INFO] ========================================
[INFO] Creating resource group: rg-mtg-dev-wcus-01
[INFO] Creating Log Analytics workspace: log-mtg-dev-wcus-01
[INFO] Creating Application Insights: appi-mtg-dev-wcus-01
[INFO] Creating App Configuration store: appconfig-mtg-dev-wcus-01 (Location: eastus)
[INFO] Populating App Configuration with Auth0 settings
[INFO] Creating Cosmos DB account (Serverless): cosmos-mtg-dev-wcus-01
[INFO] Creating user-assigned managed identity: id-mtg-dev-wcus-01
[INFO] Creating Container Apps Environment: cae-mtg-dev-wcus-01
[INFO] Creating Container App: ca-mtg-dev-wcus-01
[INFO] Creating Static Web App: swa-mtg-dev-wcus-01
[INFO] Assigning Cosmos DB Built-in Data Contributor role to managed identity
[INFO] Assigning AcrPull role to managed identity for shared ACR
[INFO] Assigning App Configuration Data Reader role to managed identity
[INFO] ========================================
[INFO] dev Environment Created Successfully
[INFO] ========================================
[INFO] Resource Group: rg-mtg-dev-wcus-01
[INFO] Managed Identity: id-mtg-dev-wcus-01 (Client ID: ...)
[INFO] App Configuration: appconfig-mtg-dev-wcus-01 (Location: eastus)
[INFO] Cosmos DB: cosmos-mtg-dev-wcus-01 (Serverless)
[INFO] Container App: ca-mtg-dev-wcus-01
[INFO] Static Web App: swa-mtg-dev-wcus-01
[INFO] Application Insights: appi-mtg-dev-wcus-01
```

### What Happens

1. **Validates Azure CLI** is installed and you're logged in
2. **Creates shared resources** (if needed)
3. **Creates environment resources** in order:
   - Resource Group
   - Log Analytics Workspace
   - Application Insights
   - App Configuration (with Auth0 settings)
   - Cosmos DB (Serverless)
   - Managed Identity
   - Container Apps Environment
   - Container App
   - Static Web App
4. **Assigns RBAC roles**:
   - Cosmos DB Built-in Data Contributor
   - AcrPull on shared ACR
   - App Configuration Data Reader

### Resource Locations

| Resource | Dev Location | Prod Location | Reason |
|----------|-------------|---------------|--------|
| Most resources | West Central US | West Central US | Primary region |
| App Configuration | **East US** | West Central US | Free tier limit (1 per region) |

### Troubleshooting

**Error: "App Configuration name already exists"**
- Azure App Configuration names are globally unique
- Try changing `APP_NAME` or `SEQUENCE` in the script

**Error: "Cosmos DB operation is taking too long"**
- Cosmos DB creation can take 5-10 minutes
- The script will wait, but you can re-run if it times out

**Error: "Role assignment already exists"**
- This is a warning, not an error
- The script continues and doesn't fail

**Error: "Free tier App Configuration limit"**
- You already have a Free tier App Configuration in that region
- The script uses different regions for dev (East US) and prod (West Central US) to avoid this

### Verification

After the script completes, verify resources were created:

```powershell
# List all resource groups
az group list --query "[?starts_with(name, 'rg-mtg')].name" -o table

# Check dev resources
az resource list --resource-group rg-mtg-dev-wcus-01 --output table

# Check prod resources
az resource list --resource-group rg-mtg-prod-wcus-01 --output table

# Verify App Configuration has Auth0 settings
az appconfig kv list --name appconfig-mtg-dev-wcus-01 --output table
```

Expected Auth0 configuration keys:
- `Auth0:Domain` = `dev-63szoyl0kt0p7e5q.us.auth0.com`
- `Auth0:Audience` = `api://mtg-discovery`
- `Auth0:ClientId` = `OXYTiA2a2SpN3cNf4Er4ix9DHHhLdYC5`

### Cleanup

To delete all resources created by the script:

```powershell
# Delete dev environment
az group delete --name rg-mtg-dev-wcus-01 --yes --no-wait

# Delete prod environment
az group delete --name rg-mtg-prod-wcus-01 --yes --no-wait

# Delete shared resources (do this last, after dev/prod are deleted)
az group delete --name rg-mtg-shared-wcus-01 --yes --no-wait
```

**Note**: Resource group deletion can take 5-15 minutes. The `--no-wait` flag returns immediately without waiting for completion.

### Re-running the Script

The script is idempotent for most resources:
- ✅ Will skip if resource group exists
- ✅ Will skip if Cosmos DB exists
- ✅ Will skip if ACR exists
- ✅ Will update App Configuration values
- ⚠️ May error on duplicate role assignments (can be ignored)

### Next Steps After Running

1. **Deploy Backend**:
   - Build container image: `dotnet publish /t:PublishContainer`
   - Push to ACR: `docker push crmtgsharedwcus01.azurecr.io/mtg-backend:tag`
   - Update Container App with new image

2. **Deploy Frontend**:
   - Build React app: `cd client && npm run build`
   - Deploy to Static Web App using deployment token

3. **Verify RBAC**:
   - Container App can read from Cosmos DB
   - Container App can read from App Configuration
   - No connection strings needed!

### Cost Estimate

After running this script for both dev and prod:
- **Dev**: ~$10-20/month (scale-to-zero, serverless, free tiers)
- **Prod**: ~$20-60/month (always-on Container App, serverless Cosmos)
- **Shared**: ~$5/month (ACR Basic tier)
- **Total**: ~$35-85/month depending on usage

### Support

For issues or questions:
- Check Azure Portal for resource status
- Review Azure CLI output for error messages
- Check `.pipelines/README.md` for architecture details
