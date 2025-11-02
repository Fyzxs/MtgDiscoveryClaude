# Security Architecture - RBAC-Based Azure Deployment

This document describes the security model for the MTG Discovery Vibe Azure deployment, which uses **Azure RBAC (Role-Based Access Control)** with **Managed Identities** instead of connection strings, passwords, or keys.

## Security Principles

### Zero Secrets Stored
**No secrets are stored in:**
- Pipeline YAML files
- Variable files
- Azure DevOps variable groups
- Environment variables (except Application Insights connection string)

### RBAC Everywhere
All Azure resource access uses Azure RBAC with managed identities:
- ✅ Cosmos DB access
- ✅ Container Registry pulls
- ✅ Azure resource management

### What's NOT a Secret

The following values are **public identifiers** (not secrets):
- Auth0 Domain (e.g., `your-tenant.auth0.com`)
- Auth0 Client ID (visible in browser JavaScript)
- Auth0 Audience (API identifier)
- Cosmos DB Endpoint URL
- Container Registry name
- Resource names

These are stored directly in variable files and version-controlled.

## Managed Identity Architecture

### User-Assigned Managed Identity

Each environment (dev/prod) has a **user-assigned managed identity**:

**Name Format**: `id-{appName}-{environment}-{regionShortName}-{sequenceNumber}`

**Examples**:
- Dev: `id-mtg-dev-wcus-01`
- Prod: `id-mtg-prod-wcus-01`

**Why User-Assigned vs System-Assigned?**
- **Lifecycle independence**: Identity persists even if Container App is recreated
- **Centralized management**: Same identity can be assigned to multiple resources
- **Permission clarity**: RBAC roles are on the identity, not tied to specific resources
- **Easier troubleshooting**: Can view all permissions in one place

### RBAC Role Assignments

The infrastructure pipeline automatically assigns these roles to the managed identity:

| Role | Scope | Purpose |
|------|-------|---------|
| **Cosmos DB Built-in Data Contributor** | Cosmos DB account | Read/write access to Cosmos data using RBAC |
| **AcrPull** | Container Registry | Pull container images from ACR |

## Authentication Flows

### 1. Container App → Cosmos DB

**Without RBAC (OLD - NOT USED)**:
```yaml
env_vars:
  CosmosConfiguration__Key: <primary-key>  # ❌ Secret exposed
```

**With RBAC (NEW - CURRENT)**:
```yaml
env_vars:
  AZURE_CLIENT_ID: <managed-identity-client-id>  # ✅ Just an identifier
  # No key needed!
```

**How it works**:
1. Container App has user-assigned managed identity attached
2. `AZURE_CLIENT_ID` environment variable tells Azure.Identity SDK which identity to use
3. .NET code uses `DefaultAzureCredential` which automatically retrieves a token
4. Cosmos DB validates the token and checks RBAC permissions
5. Access granted/denied based on role assignment

**Required .NET Configuration**:
```csharp
// Your code should already use this pattern
builder.Services.AddSingleton<CosmosClient>(sp =>
{
    var endpoint = configuration["CosmosConfiguration__Endpoint"];
    return new CosmosClient(endpoint, new DefaultAzureCredential());
});
```

### 2. Container App → Container Registry

**Without RBAC (OLD - NOT USED)**:
```bash
az containerapp update \
  --registry-username <username> \
  --registry-password <password>  # ❌ Secrets in pipeline
```

**With RBAC (NEW - CURRENT)**:
```bash
az containerapp update \
  --image cr-registry.azurecr.io/image:tag  # ✅ No credentials needed
  # Managed identity used automatically!
```

**How it works**:
1. Container App has AcrPull role on the ACR
2. When pulling images, Azure automatically uses the managed identity
3. ACR validates the identity has AcrPull permission
4. Image pulled without any credentials in configuration

### 3. Frontend → Backend (Auth0 JWT)

**Flow**:
1. User logs in via Auth0 in React app
2. Auth0 returns JWT token with `audience` matching backend API
3. Frontend sends token in `Authorization: Bearer <token>` header
4. Backend validates JWT signature and claims
5. Backend authorizes request based on token claims

**Configuration** (in variable files, NOT secrets):
- `auth0Domain`: Auth0 tenant domain
- `auth0Audience`: API identifier
- `auth0ClientId`: SPA public client ID

## Pipeline Security

### Infrastructure Pipeline

**What it does with security**:
1. Creates user-assigned managed identity
2. Assigns Cosmos DB RBAC role to identity
3. Assigns AcrPull role to identity
4. Attaches identity to Container App

**No secrets**:
- ✅ Cosmos keys are retrieved but NOT stored or passed to applications
- ✅ ACR credentials are never touched
- ✅ All permissions configured via RBAC

### Backend Deployment Pipeline

**What it does**:
1. Builds container image using `dotnet publish /t:PublishContainer`
2. Pushes to ACR (pipeline service principal has push permissions)
3. Updates Container App with new image
4. Sets environment variables (NO SECRETS except App Insights)

**Environment Variables Set**:
```yaml
ASPNETCORE_ENVIRONMENT: Development|Production
CosmosConfiguration__Endpoint: <endpoint-url>  # Not a secret
CosmosConfiguration__DatabaseName: MtgDiscoveryDb  # Not a secret
Auth0__Domain: <tenant>.auth0.com  # Not a secret
Auth0__Audience: https://api.mtgdiscovery.*  # Not a secret
APPLICATIONINSIGHTS_CONNECTION_STRING: <conn-string>  # Less sensitive, kept for simplicity
AZURE_CLIENT_ID: <managed-identity-client-id>  # Not a secret, just an identifier
```

**No Cosmos Key**: The `CosmosConfiguration__Key` is intentionally NOT set. Azure.Identity SDK uses the managed identity instead.

## What About Application Insights?

**Decision**: Keep using connection string

**Why?**:
- Application Insights connection strings are **less sensitive** than Cosmos DB keys
- They only allow sending telemetry, not querying production data
- Simpler configuration (no code changes needed)
- Microsoft's recommended approach for most scenarios

**Alternative**: Can use managed identity for App Insights too, but requires code changes to use Azure.Monitor.OpenTelemetry with managed identity.

## Static Web App Deployment Token

**The One Exception**: Static Web Apps require a deployment token

**Why we can't avoid it**:
- Azure DevOps Static Web App task requires the token
- No RBAC alternative available (platform limitation)
- Token is automatically rotated by Azure

**Mitigation**:
- Token retrieved dynamically during pipeline execution
- Marked as secret in pipeline variables
- Never stored in code or configuration files

## Security Checklist

### ✅ Implemented
- [x] Managed identities for all Azure resource access
- [x] Cosmos DB RBAC (no connection strings)
- [x] ACR pull with managed identity (no registry credentials)
- [x] User-assigned managed identity for lifecycle independence
- [x] Auth0 values stored as configuration (not secrets)
- [x] No secrets in variable files
- [x] Pipeline retrieves secrets only when needed (transiently)
- [x] Least privilege RBAC assignments

### ❌ Not Implemented (Future Improvements)
- [ ] Azure Key Vault for Application Insights connection string
- [ ] Managed identity for Application Insights
- [ ] VNet integration for Container Apps
- [ ] Private endpoints for Cosmos DB
- [ ] Azure Front Door with WAF
- [ ] Conditional access policies for pipeline service principal

## Viewing Permissions

### Check Managed Identity Permissions

**Cosmos DB**:
```bash
az cosmosdb sql role assignment list \
  --account-name cosmos-mtg-dev-wcus-01 \
  --resource-group rg-mtg-dev-wcus-01
```

**Container Registry**:
```bash
az role assignment list \
  --scope /subscriptions/<sub-id>/resourceGroups/rg-mtg-shared-wcus-01/providers/Microsoft.ContainerRegistry/registries/crmtgsharedwcus01 \
  --query "[?principalType=='ServicePrincipal'].{Role:roleDefinitionName, Principal:principalName}"
```

### Check Identity Attached to Container App

```bash
az containerapp show \
  --name ca-mtg-dev-wcus-01 \
  --resource-group rg-mtg-dev-wcus-01 \
  --query identity
```

Should show:
```json
{
  "type": "UserAssigned",
  "userAssignedIdentities": {
    "/subscriptions/<sub>/resourcegroups/rg-mtg-dev-wcus-01/providers/Microsoft.ManagedIdentity/userAssignedIdentities/id-mtg-dev-wcus-01": {
      "clientId": "<client-id>",
      "principalId": "<principal-id>"
    }
  }
}
```

## Troubleshooting

### Cosmos DB 401 Unauthorized

**Symptom**: Application logs show 401 errors when accessing Cosmos DB

**Check**:
1. Is `AZURE_CLIENT_ID` environment variable set correctly?
```bash
az containerapp show \
  --name ca-mtg-dev-wcus-01 \
  --resource-group rg-mtg-dev-wcus-01 \
  --query properties.template.containers[0].env
```

2. Is managed identity assigned to Container App?
```bash
az containerapp show \
  --name ca-mtg-dev-wcus-01 \
  --resource-group rg-mtg-dev-wcus-01 \
  --query identity.userAssignedIdentities
```

3. Does identity have Cosmos DB RBAC role?
```bash
az cosmosdb sql role assignment list \
  --account-name cosmos-mtg-dev-wcus-01 \
  --resource-group rg-mtg-dev-wcus-01
```

4. Is code using `DefaultAzureCredential`?
Check your Cosmos client initialization.

### Container App Can't Pull Image

**Symptom**: Container App shows "ImagePullBackOff" or similar error

**Check**:
1. Does managed identity have AcrPull role on ACR?
```bash
# Get identity principal ID
PRINCIPAL_ID=$(az identity show \
  --name id-mtg-dev-wcus-01 \
  --resource-group rg-mtg-dev-wcus-01 \
  --query principalId -o tsv)

# Check ACR role assignments
az role assignment list \
  --assignee $PRINCIPAL_ID \
  --scope /subscriptions/<sub-id>/resourceGroups/rg-mtg-shared-wcus-01/providers/Microsoft.ContainerRegistry/registries/crmtgsharedwcus01
```

2. Is managed identity properly attached?
```bash
az containerapp show \
  --name ca-mtg-dev-wcus-01 \
  --resource-group rg-mtg-dev-wcus-01 \
  --query "properties.template.containers[0].image"
```

### Auth0 Token Validation Fails

**Symptom**: Backend rejects valid Auth0 tokens

**Check**:
1. Do Auth0 configuration values match between frontend and backend?
- `auth0Domain` must be identical
- `auth0Audience` must match exactly

2. Is the JWT token being sent correctly?
Check browser network tab for `Authorization: Bearer <token>` header

## Best Practices

### DO
✅ Use user-assigned managed identities for production workloads
✅ Grant least privilege RBAC roles (e.g., Data Reader if write not needed)
✅ Rotate Application Insights connection strings periodically
✅ Use different Auth0 tenants for dev/prod
✅ Monitor role assignments for changes
✅ Use Azure Policy to enforce managed identity usage

### DON'T
❌ Store any secrets in variable files or code
❌ Use connection strings when RBAC is available
❌ Use system-assigned identities for multi-resource scenarios
❌ Grant broad permissions (e.g., Contributor) when narrow roles exist
❌ Disable managed identity to "fix" authentication issues
❌ Share managed identities across security boundaries

## Migration from Connection Strings

If you previously used connection strings:

**OLD CODE**:
```csharp
var connectionString = configuration["CosmosConfiguration__ConnectionString"];
var client = new CosmosClient(connectionString);
```

**NEW CODE**:
```csharp
var endpoint = configuration["CosmosConfiguration__Endpoint"];
var client = new CosmosClient(endpoint, new DefaultAzureCredential());
```

**Environment Variables**:
- Remove: `CosmosConfiguration__Key` or `CosmosConfiguration__ConnectionString`
- Add: `AZURE_CLIENT_ID` (set by pipeline automatically)
- Keep: `CosmosConfiguration__Endpoint`

## Additional Resources

- [Azure Managed Identities Overview](https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/overview)
- [Cosmos DB RBAC](https://docs.microsoft.com/en-us/azure/cosmos-db/how-to-setup-rbac)
- [Azure Container Registry Authentication](https://docs.microsoft.com/en-us/azure/container-registry/container-registry-authentication-managed-identity)
- [Azure.Identity DefaultAzureCredential](https://docs.microsoft.com/en-us/dotnet/api/azure.identity.defaultazurecredential)
- [Auth0 SPA Quickstart](https://auth0.com/docs/quickstart/spa)
