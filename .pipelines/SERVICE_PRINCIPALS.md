# Service Principal Setup Guide

This document describes how to set up the three service principals required for Azure deployment pipelines.

## Overview

The pipelines use **three separate service principals**, each with specific permissions scoped to their respective resource groups:

| Service Principal | Service Connection Name | Scope | Purpose |
|-------------------|------------------------|-------|---------|
| **mtg-shared** | `mtg-shared` | Shared resource group | Manages shared resources (ACR) |
| **mtg-dev** | `mtg-dev` | Dev resource group | Manages dev environment resources |
| **mtg-prod** | `mtg-prod` | Prod resource group | Manages prod environment resources |

### Why Three Service Principals?

**Security Benefits**:
- **Least Privilege**: Each service principal only has permissions for its scope
- **Blast Radius Limitation**: Compromised dev credentials can't affect production
- **Audit Trail**: Clear separation of who did what in which environment
- **Compliance**: Meets security requirements for production/non-production separation

**Operational Benefits**:
- **Team Structure**: Different teams can manage different environments
- **Approval Workflows**: Production changes require different approvals
- **Cost Management**: Clear attribution of resource changes

## Required Permissions

### mtg-shared Service Principal

**Scope**: `/subscriptions/{subscription-id}/resourceGroups/rg-mtg-shared-wcus-01`

**Role**: **Contributor**

**Why**: Needs to create and manage:
- Container Registry (ACR)
- Assign roles on ACR (for dev/prod managed identities to pull images)

**Used By**:
- Infrastructure pipeline (CreateSharedResources stage)
- Backend pipeline (Build stage - for ACR push)
- Infra create-environment steps (for ACR role assignments to managed identities)

### mtg-dev Service Principal

**Scope**: `/subscriptions/{subscription-id}/resourceGroups/rg-mtg-dev-wcus-01`

**Role**: **Contributor**

**Why**: Needs to create and manage:
- Dev resource group
- User-assigned managed identity
- Cosmos DB account
- Container Apps environment and app
- Static Web App
- Application Insights
- Log Analytics workspace
- RBAC role assignments within dev scope

**Used By**:
- Infrastructure pipeline (CreateDevEnvironment stage)
- Backend pipeline (DeployDev stage)
- Frontend pipeline (DeployDev stage)

### mtg-prod Service Principal

**Scope**: `/subscriptions/{subscription-id}/resourceGroups/rg-mtg-prod-wcus-01`

**Role**: **Contributor**

**Why**: Needs to create and manage:
- Prod resource group
- User-assigned managed identity
- Cosmos DB account
- Container Apps environment and app
- Static Web App
- Application Insights
- Log Analytics workspace
- RBAC role assignments within prod scope

**Used By**:
- Infrastructure pipeline (CreateProdEnvironment stage)
- Backend pipeline (DeployProd stage)
- Frontend pipeline (DeployProd stage)

## Setup Instructions

### Step 1: Create Resource Groups (if not already created)

```bash
# Shared resource group
az group create \
  --name rg-mtg-shared-wcus-01 \
  --location westcentralus

# Dev resource group
az group create \
  --name rg-mtg-dev-wcus-01 \
  --location westcentralus

# Prod resource group
az group create \
  --name rg-mtg-prod-wcus-01 \
  --location westcentralus
```

### Step 2: Create Service Principals

#### Create mtg-shared Service Principal

```bash
# Create service principal scoped to shared resource group
az ad sp create-for-rbac \
  --name mtg-shared \
  --role Contributor \
  --scopes /subscriptions/{subscription-id}/resourceGroups/rg-mtg-shared-wcus-01

# Save the output - you'll need:
# - appId (client ID)
# - password (client secret)
# - tenant
```

**Output example**:
```json
{
  "appId": "00000000-0000-0000-0000-000000000000",
  "displayName": "mtg-shared",
  "password": "secret-value",
  "tenant": "00000000-0000-0000-0000-000000000000"
}
```

#### Create mtg-dev Service Principal

```bash
# Create service principal scoped to dev resource group
az ad sp create-for-rbac \
  --name mtg-dev \
  --role Contributor \
  --scopes /subscriptions/{subscription-id}/resourceGroups/rg-mtg-dev-wcus-01

# Save the output
```

#### Create mtg-prod Service Principal

```bash
# Create service principal scoped to prod resource group
az ad sp create-for-rbac \
  --name mtg-prod \
  --role Contributor \
  --scopes /subscriptions/{subscription-id}/resourceGroups/rg-mtg-prod-wcus-01

# Save the output
```

### Step 3: Grant Additional Permissions

#### mtg-shared: Additional ACR Permissions

The mtg-shared service principal needs to be able to assign roles on the ACR to managed identities:

```bash
# Get the shared service principal's object ID
SHARED_SP_ID=$(az ad sp list --display-name mtg-shared --query [0].id -o tsv)

# Get the subscription ID
SUBSCRIPTION_ID=$(az account show --query id -o tsv)

# Grant User Access Administrator role on shared resource group
# This allows the SP to assign the AcrPull role to managed identities
az role assignment create \
  --assignee $SHARED_SP_ID \
  --role "User Access Administrator" \
  --scope /subscriptions/$SUBSCRIPTION_ID/resourceGroups/rg-mtg-shared-wcus-01
```

#### mtg-dev & mtg-prod: Cosmos DB RBAC Permissions

Both dev and prod service principals need to assign Cosmos DB RBAC roles:

```bash
# For Dev
DEV_SP_ID=$(az ad sp list --display-name mtg-dev --query [0].id -o tsv)

az role assignment create \
  --assignee $DEV_SP_ID \
  --role "User Access Administrator" \
  --scope /subscriptions/$SUBSCRIPTION_ID/resourceGroups/rg-mtg-dev-wcus-01

# For Prod
PROD_SP_ID=$(az ad sp list --display-name mtg-prod --query [0].id -o tsv)

az role assignment create \
  --assignee $PROD_SP_ID \
  --role "User Access Administrator" \
  --scope /subscriptions/$SUBSCRIPTION_ID/resourceGroups/rg-mtg-prod-wcus-01
```

### Step 4: Create Azure DevOps Service Connections

#### Create mtg-shared Service Connection

1. Navigate to Azure DevOps → Project Settings → Service connections
2. Click "New service connection"
3. Select "Azure Resource Manager"
4. Choose "Service principal (manual)"
5. Fill in details:
   - **Service connection name**: `mtg-shared`
   - **Environment**: Azure Cloud
   - **Scope level**: Subscription (or Resource Group)
   - **Subscription ID**: Your subscription ID
   - **Subscription Name**: Your subscription name
   - **Service Principal ID**: appId from Step 2 (mtg-shared)
   - **Service principal key**: password from Step 2
   - **Tenant ID**: tenant from Step 2
6. Click "Verify"
7. Save service connection

#### Create mtg-dev Service Connection

Repeat the above steps with:
- **Service connection name**: `mtg-dev`
- **Service Principal ID**: appId from mtg-dev
- **Service principal key**: password from mtg-dev
- **Tenant ID**: tenant from mtg-dev

#### Create mtg-prod Service Connection

Repeat the above steps with:
- **Service connection name**: `mtg-prod`
- **Service Principal ID**: appId from mtg-prod
- **Service principal key**: password from mtg-prod
- **Tenant ID**: tenant from mtg-prod

### Step 5: Grant Pipeline Access

For each service connection:

1. Navigate to the service connection settings
2. Click "Security"
3. Either:
   - **Option A**: Check "Grant access permission to all pipelines" (easier, less secure)
   - **Option B**: Manually authorize specific pipelines (more secure)

**Recommended for production**: Use Option B and only grant access to the specific pipelines that need it.

## Pipeline Usage Matrix

This table shows which service connection is used where:

| Pipeline | Stage | Service Connection(s) Used | Purpose |
|----------|-------|---------------------------|---------|
| **Infrastructure** | CreateSharedResources | `mtg-shared` | Create shared RG and ACR |
| **Infrastructure** | CreateDevEnvironment | `mtg-dev`, `mtg-shared`* | Create dev resources, assign ACR role |
| **Infrastructure** | CreateProdEnvironment | `mtg-prod`, `mtg-shared`* | Create prod resources, assign ACR role |
| **Backend** | Build | `mtg-shared` | Push images to ACR |
| **Backend** | DeployDev | `mtg-dev`, `mtg-shared`** | Deploy to dev, get ACR info |
| **Backend** | DeployProd | `mtg-prod`, `mtg-shared`** | Deploy to prod, get ACR info |
| **Frontend** | Build | *(none)* | npm build (no Azure access) |
| **Frontend** | DeployDev | `mtg-dev` | Deploy to dev Static Web App |
| **Frontend** | DeployProd | `mtg-prod` | Deploy to prod Static Web App |

\* `mtg-shared` used for ACR role assignment step
\*\* `mtg-shared` used to query ACR (in shared resource group)

## Security Best Practices

### Service Principal Management

1. **Rotate Secrets Regularly**:
   ```bash
   # Reset service principal password
   az ad sp credential reset --id <sp-app-id>
   # Update Azure DevOps service connection with new password
   ```

2. **Use Managed Identities When Possible**:
   - Service principals are used for CI/CD pipeline access
   - Application runtime uses managed identities (no secrets)

3. **Monitor Service Principal Usage**:
   ```bash
   # Check sign-in logs
   az monitor activity-log list \
     --caller <sp-app-id> \
     --start-time 2024-01-01
   ```

4. **Principle of Least Privilege**:
   - ✅ Service principals scoped to specific resource groups
   - ✅ Only Contributor + User Access Administrator (for RBAC assignments)
   - ❌ Never use Owner role
   - ❌ Never scope to entire subscription unless necessary

### Azure DevOps Security

1. **Service Connection Permissions**:
   - Only grant access to specific pipelines (not all pipelines)
   - Review permissions regularly

2. **Branch Policies**:
   - Require pull request reviews for main branch
   - Production deployments only from main branch
   - Use environment approvals for production

3. **Environment Approvals**:
   - Configure manual approval for production environment
   - Require specific approvers
   - Add deployment gates (e.g., successful tests)

4. **Audit Logging**:
   - Enable Azure DevOps audit logging
   - Review service principal usage
   - Monitor unauthorized access attempts

## Troubleshooting

### Permission Errors

**Error**: `Authorization failed for <service-connection> with error: Insufficient privileges to complete the operation`

**Solutions**:
1. Verify service principal has Contributor role:
   ```bash
   az role assignment list --assignee <sp-app-id>
   ```

2. Check if resource is in the correct resource group
3. Verify service connection is configured correctly in Azure DevOps

### Role Assignment Fails

**Error**: `The client '<sp-id>' with object id '<object-id>' does not have authorization to perform action 'Microsoft.Authorization/roleAssignments/write'`

**Solution**: Grant User Access Administrator role (see Step 3 above)

### Service Connection Not Found

**Error**: `Could not find a service connection with name 'mtg-shared'`

**Solutions**:
1. Verify service connection name exactly matches (case-sensitive)
2. Check service connection is not scoped to a different project
3. Verify service connection has pipeline access granted

### ACR Access Denied

**Error**: Container App can't pull images from ACR

**Check**:
1. Managed identity has AcrPull role:
   ```bash
   az role assignment list \
     --assignee <managed-identity-principal-id> \
     --scope /subscriptions/<sub-id>/resourceGroups/rg-mtg-shared-wcus-01/providers/Microsoft.ContainerRegistry/registries/crmtgsharedwcus01
   ```

2. Infrastructure pipeline ran successfully (assigns the role)

## Maintenance

### Rotating Service Principal Secrets

Recommended frequency: Every 90 days

```bash
# 1. Reset the service principal credential
NEW_PASSWORD=$(az ad sp credential reset \
  --id <sp-app-id> \
  --query password -o tsv)

echo "New password: $NEW_PASSWORD"

# 2. Update Azure DevOps service connection
# - Navigate to service connection in Azure DevOps
# - Edit connection
# - Update "Service principal key" with new password
# - Verify and save

# 3. Test the connection
# - Run a pipeline that uses the service connection
# - Verify it succeeds
```

### Adding New Environments

If you add new environments (e.g., `staging`):

1. Create resource group:
   ```bash
   az group create --name rg-mtg-staging-wcus-01 --location westcentralus
   ```

2. Create service principal:
   ```bash
   az ad sp create-for-rbac \
     --name mtg-staging \
     --role Contributor \
     --scopes /subscriptions/{sub-id}/resourceGroups/rg-mtg-staging-wcus-01
   ```

3. Grant User Access Administrator role (for RBAC assignments)

4. Create Azure DevOps service connection: `mtg-staging`

5. Update pipeline YAML to use the new service connection

## Reference

### Service Principal Details

After setup, you can view service principal details:

```bash
# List all service principals
az ad sp list --display-name mtg-shared
az ad sp list --display-name mtg-dev
az ad sp list --display-name mtg-prod

# View role assignments
az role assignment list --assignee <sp-app-id>

# View service principal details in Azure DevOps
# Navigate to: Project Settings → Service connections → <connection-name>
```

### Useful Commands

```bash
# Get subscription ID
az account show --query id -o tsv

# List all resource groups
az group list --query "[].name" -o table

# Check if service principal can access a resource
az resource show \
  --ids /subscriptions/{sub-id}/resourceGroups/{rg-name} \
  --query id

# Test service principal login
az login --service-principal \
  --username <sp-app-id> \
  --password <sp-password> \
  --tenant <tenant-id>
```

## Additional Resources

- [Azure Service Principals](https://docs.microsoft.com/en-us/azure/active-directory/develop/app-objects-and-service-principals)
- [Azure RBAC](https://docs.microsoft.com/en-us/azure/role-based-access-control/overview)
- [Azure DevOps Service Connections](https://docs.microsoft.com/en-us/azure/devops/pipelines/library/service-endpoints)
- [Scope-Based RBAC](https://docs.microsoft.com/en-us/azure/role-based-access-control/scope-overview)
