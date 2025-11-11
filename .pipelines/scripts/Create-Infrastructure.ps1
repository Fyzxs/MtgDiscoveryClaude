# Azure Infrastructure Creation Script (PowerShell)
# This script creates all Azure resources for the MTG Discovery application
# Run this when Azure DevOps build agents are down

# Stop on errors
$ErrorActionPreference = "Stop"

# Configuration
$appName = "mtg"
$location = "westcentralus"
$regionShort = "wcus"
$swaLocation = "westus2"  # Static Web App location
$appInsightsLocation = "westus2"  # Application Insights location
$sequence = "01"

# Resource short names
$rgShort = "rg"
$swaShort = "swa"
$crShort = "cr"
$caShort = "ca"
$appiShort = "appi"
$cosmosShort = "cosmos"
$caeShort = "cae"
$logShort = "log"
$idShort = "id"
$appConfigShort = "appconfig"

# Container App settings
$caPort = 8080
$caCpu = "0.5"
$caMemory = "1.0Gi"
$caMaxReplicas = 10

# Auth0 configuration
$auth0Domain = "dev-63szoyl0kt0p7e5q.us.auth0.com"
$auth0Audience = "api://mtg-discovery"
$auth0ClientId = "OXYTiA2a2SpN3cNf4Er4ix9DHHhLdYC5"

# Cosmos DB settings
$cosmosConsistency = "Session"

# Functions
function Write-InfoLog {
    param([string]$Message)
    Write-Host "[INFO] $Message" -ForegroundColor Green
}

function Write-WarnLog {
    param([string]$Message)
    Write-Host "[WARN] $Message" -ForegroundColor Yellow
}

function Write-ErrorLog {
    param([string]$Message)
    Write-Host "[ERROR] $Message" -ForegroundColor Red
}

# Check if Azure CLI is installed
try {
    $null = az --version
} catch {
    Write-ErrorLog "Azure CLI is not installed. Please install it first."
    Write-Host "Download from: https://aka.ms/installazurecliwindows"
    exit 1
}

# Check if logged in
try {
    $null = az account show 2>$null
} catch {
    Write-ErrorLog "Not logged into Azure. Please run 'az login' first."
    exit 1
}

Write-InfoLog "Starting infrastructure creation for MTG Discovery application"
$subscriptionName = az account show --query name -o tsv
Write-InfoLog "Subscription: $subscriptionName"

# Ask which environment to create
Write-Host ""
Write-Host "Which environment(s) do you want to create?"
Write-Host "1) Shared resources only"
Write-Host "2) Dev environment (includes shared)"
Write-Host "3) Prod environment (includes shared)"
Write-Host "4) All (shared + dev + prod)"
$choice = Read-Host "Enter choice (1-4)"

$createShared = $false
$createDev = $false
$createProd = $false

switch ($choice) {
    "1" { $createShared = $true }
    "2" { $createShared = $true; $createDev = $true }
    "3" { $createShared = $true; $createProd = $true }
    "4" { $createShared = $true; $createDev = $true; $createProd = $true }
    default { Write-ErrorLog "Invalid choice"; exit 1 }
}

################################################################################
# CREATE SHARED RESOURCES
################################################################################

if ($createShared) {
    Write-InfoLog "========================================"
    Write-InfoLog "Creating Shared Resources"
    Write-InfoLog "========================================"

    # Shared resource names
    $sharedRg = "$rgShort-$appName-shared-$regionShort-$sequence"
    $sharedCr = "$crShort$appName" + "shared" + "$regionShort$sequence"

    Write-InfoLog "Creating shared resource group: $sharedRg"
    az group create `
        --name $sharedRg `
        --location $location `
        --tags Environment=shared Application=$appName ManagedBy=Script

    Write-InfoLog "Creating Azure Container Registry: $sharedCr"
    # Check if ACR exists (use list to avoid errors)
    $acrResult = az acr list --query "[?name=='$sharedCr'].id" -o tsv
    $acrExists = -not [string]::IsNullOrWhiteSpace($acrResult)

    if (-not $acrExists) {
        az acr create `
            --name $sharedCr `
            --resource-group $sharedRg `
            --location $location `
            --sku Basic `
            --admin-enabled true `
            --tags Environment=shared Application=$appName ManagedBy=Script

        if ($LASTEXITCODE -ne 0) {
            Write-ErrorLog "Failed to create Container Registry"
            exit 1
        }
    } else {
        Write-WarnLog "Container Registry already exists, skipping"
    }

    Write-InfoLog "Shared resources created successfully"
    Write-Host ""
}

################################################################################
# FUNCTION: CREATE ENVIRONMENT
################################################################################

function New-Environment {
    param(
        [string]$Env,
        [string]$EnvLocation,
        [string]$AppConfigLocation,
        [int]$CaMinReplicas,
        [int]$CaMaxReplicasEnv,
        [string]$SwaSku
    )

    Write-InfoLog "========================================"
    Write-InfoLog "Creating $Env Environment"
    Write-InfoLog "========================================"

    # Generate resource names
    $rg = "$rgShort-$appName-$Env-$regionShort-$sequence"
    $cosmos = "$cosmosShort-$appName-$Env-$regionShort-$sequence"
    $cae = "$caeShort-$appName-$Env-$regionShort-$sequence"
    $ca = "$caShort-$appName-$Env-$regionShort-$sequence"
    $swa = "$swaShort-$appName-$Env-$regionShort-$sequence"
    $appi = "$appiShort-$appName-$Env-$regionShort-$sequence"
    $log = "$logShort-$appName-$Env-$regionShort-$sequence"
    $identity = "$idShort-$appName-$Env-$regionShort-$sequence"
    $appConfig = "$appConfigShort-$appName-$Env-$regionShort-$sequence"

    # Create resource group
    Write-InfoLog "Creating resource group: $rg"
    az group create `
        --name $rg `
        --location $EnvLocation `
        --tags Environment=$Env Application=$appName ManagedBy=Script

    # Create Log Analytics workspace
    Write-InfoLog "Creating Log Analytics workspace: $log"
    az monitor log-analytics workspace create `
        --resource-group $rg `
        --workspace-name $log `
        --location $EnvLocation `
        --tags Environment=$Env Application=$appName

    $workspaceId = az monitor log-analytics workspace show `
        --resource-group $rg `
        --workspace-name $log `
        --query customerId `
        -o tsv

    $workspaceKey = az monitor log-analytics workspace get-shared-keys `
        --resource-group $rg `
        --workspace-name $log `
        --query primarySharedKey `
        -o tsv

    # Create Application Insights
    Write-InfoLog "Creating Application Insights: $appi in $appInsightsLocation"
    az monitor app-insights component create `
        --app $appi `
        --location $appInsightsLocation `
        --resource-group $rg `
        --application-type web `
        --workspace $workspaceId `
        --tags Environment=$Env Application=$appName

    # Create App Configuration
    Write-InfoLog "Creating App Configuration store: $appConfig (Location: $AppConfigLocation)"
    az appconfig create `
        --name $appConfig `
        --resource-group $rg `
        --location $AppConfigLocation `
        --sku Free `
        --tags Environment=$Env Application=$appName

    # Populate App Configuration with Auth0 settings
    Write-InfoLog "Populating App Configuration with Auth0 settings"
    az appconfig kv set --name $appConfig --key "Auth0:Domain" --value $auth0Domain --yes
    az appconfig kv set --name $appConfig --key "Auth0:Audience" --value $auth0Audience --yes
    az appconfig kv set --name $appConfig --key "Auth0:ClientId" --value $auth0ClientId --yes

    # Create Cosmos DB (Serverless)
    Write-InfoLog "Creating Cosmos DB account (Serverless): $cosmos"
    # Check if Cosmos DB exists (use list to avoid errors)
    $cosmosResult = az cosmosdb list --resource-group $rg --query "[?name=='$cosmos'].id" -o tsv
    $cosmosExists = -not [string]::IsNullOrWhiteSpace($cosmosResult)

    if (-not $cosmosExists) {
        az cosmosdb create `
            --name $cosmos `
            --resource-group $rg `
            --locations regionName=$EnvLocation `
            --capabilities EnableServerless `
            --default-consistency-level $cosmosConsistency `
            --tags Environment=$Env Application=$appName CapacityMode=Serverless

        if ($LASTEXITCODE -ne 0) {
            Write-ErrorLog "Failed to create Cosmos DB"
            exit 1
        }
    } else {
        Write-WarnLog "Cosmos DB already exists, skipping"
    }

    # Create Managed Identity
    Write-InfoLog "Creating user-assigned managed identity: $identity"
    az identity create `
        --name $identity `
        --resource-group $rg `
        --location $EnvLocation `
        --tags Environment=$Env Application=$appName

    $identityId = az identity show --name $identity --resource-group $rg --query id -o tsv
    $principalId = az identity show --name $identity --resource-group $rg --query principalId -o tsv
    $clientId = az identity show --name $identity --resource-group $rg --query clientId -o tsv

    Write-InfoLog "Managed Identity Principal ID: $principalId"
    Write-InfoLog "Managed Identity Client ID: $clientId"

    # Create Container Apps Environment
    Write-InfoLog "Creating Container Apps Environment: $cae"
    az containerapp env create `
        --name $cae `
        --resource-group $rg `
        --location $EnvLocation `
        --logs-workspace-id $workspaceId `
        --logs-workspace-key $workspaceKey `
        --tags Environment=$Env Application=$appName

    # Create Container App (with placeholder image)
    Write-InfoLog "Creating Container App: $ca"
    az containerapp create `
        --name $ca `
        --resource-group $rg `
        --environment $cae `
        --image mcr.microsoft.com/azuredocs/containerapps-helloworld:latest `
        --target-port $caPort `
        --ingress external `
        --cpu $caCpu `
        --memory $caMemory `
        --min-replicas $CaMinReplicas `
        --max-replicas $CaMaxReplicasEnv `
        --user-assigned $identityId `
        --tags Environment=$Env Application=$appName

    # Create Static Web App
    Write-InfoLog "Creating Static Web App: $swa in $swaLocation"
    az staticwebapp create `
        --name $swa `
        --resource-group $rg `
        --location $swaLocation `
        --sku $SwaSku `
        --tags Environment=$Env Application=$appName

    # Assign Cosmos DB RBAC role
    Write-InfoLog "Assigning Cosmos DB Built-in Data Contributor role to managed identity"
    $roleId = "00000000-0000-0000-0000-000000000002"

    # Check existing assignments (suppress errors)
    $existingAssignment = az cosmosdb sql role assignment list `
        --account-name $cosmos `
        --resource-group $rg `
        --query "[?principalId=='$principalId'].id" -o tsv 2>&1 | Where-Object { $_ -notmatch '^ERROR:' }

    if ([string]::IsNullOrWhiteSpace($existingAssignment)) {
        az cosmosdb sql role assignment create `
            --account-name $cosmos `
            --resource-group $rg `
            --role-definition-id $roleId `
            --principal-id $principalId `
            --scope "/"
    } else {
        Write-WarnLog "Cosmos DB RBAC role already assigned, skipping"
    }

    # Assign ACR Pull role
    Write-InfoLog "Assigning AcrPull role to managed identity for shared ACR"
    $acrId = az acr show --name $sharedCr --query id -o tsv

    az role assignment create `
        --assignee $principalId `
        --role AcrPull `
        --scope $acrId 2>&1 | Out-Null

    if ($LASTEXITCODE -ne 0) {
        Write-WarnLog "AcrPull role may already be assigned"
    }

    # Assign App Configuration Data Reader role
    Write-InfoLog "Assigning App Configuration Data Reader role to managed identity"
    $appConfigId = az appconfig show --name $appConfig --resource-group $rg --query id -o tsv

    az role assignment create `
        --assignee $principalId `
        --role "App Configuration Data Reader" `
        --scope $appConfigId 2>&1 | Out-Null

    if ($LASTEXITCODE -ne 0) {
        Write-WarnLog "App Configuration role may already be assigned"
    }

    # Summary
    Write-Host ""
    Write-InfoLog "========================================"
    Write-InfoLog "$Env Environment Created Successfully"
    Write-InfoLog "========================================"
    Write-InfoLog "Resource Group: $rg"
    Write-InfoLog "Managed Identity: $identity (Client ID: $clientId)"
    Write-InfoLog "App Configuration: $appConfig (Location: $AppConfigLocation)"
    Write-InfoLog "Cosmos DB: $cosmos (Serverless)"
    Write-InfoLog "Container App: $ca"
    Write-InfoLog "Static Web App: $swa"
    Write-InfoLog "Application Insights: $appi"
    Write-Host ""
}

################################################################################
# CREATE DEV ENVIRONMENT
################################################################################

if ($createDev) {
    New-Environment -Env "dev" -EnvLocation $location -AppConfigLocation "eastus" -CaMinReplicas 0 -CaMaxReplicasEnv 1 -SwaSku "Free"
}

################################################################################
# CREATE PROD ENVIRONMENT
################################################################################

if ($createProd) {
    New-Environment -Env "prod" -EnvLocation $location -AppConfigLocation "westcentralus" -CaMinReplicas 1 -CaMaxReplicasEnv 2 -SwaSku "Free"
}

################################################################################
# COMPLETION
################################################################################

Write-InfoLog "========================================"
Write-InfoLog "Infrastructure Creation Complete!"
Write-InfoLog "========================================"
Write-InfoLog "Next steps:"
Write-InfoLog "1. Deploy backend application to Container Apps"
Write-InfoLog "2. Deploy frontend application to Static Web Apps"
Write-InfoLog "3. All authentication uses RBAC - no keys or passwords!"
Write-InfoLog "4. Auth0 configuration stored in App Configuration"
