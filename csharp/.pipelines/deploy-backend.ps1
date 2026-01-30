# Azure Container Apps Backend Deployment Script (PowerShell)
# Usage: .\deploy-backend.ps1 [preview|dev|production|status|logs]

param(
    [Parameter(Position=0)]
    [string]$Command = "",
    [Parameter(Position=1)]
    [string]$LogEnv = ""
)

$ErrorActionPreference = "Stop"

# Configuration
$PROJECT_PATH = "src/App.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL.csproj"
$CONTAINER_IMAGE_NAME = "mtg-backend"
$ACR_NAME = "crmtgsharedwcus01"

# Environment-specific configuration
$EnvConfig = @{
    preview = @{
        app_name = "ca-mtg-dev-wcus-01"
        resource_group = "rg-mtg-dev-wcus-01"
        cosmos_name = "cosmos-mtg-dev-wcus-01"
        appinsights_name = "appi-mtg-dev-wcus-01"
        identity_name = "id-mtg-dev-wcus-01"
        aspnetcore_env = "Development"
    }
    dev = @{
        app_name = "ca-mtg-dev-wcus-01"
        resource_group = "rg-mtg-dev-wcus-01"
        cosmos_name = "cosmos-mtg-dev-wcus-01"
        appinsights_name = "appi-mtg-dev-wcus-01"
        identity_name = "id-mtg-dev-wcus-01"
        aspnetcore_env = "Development"
    }
    prod = @{
        app_name = "ca-mtg-prod-wcus-01"
        resource_group = "rg-mtg-prod-wcus-01"
        cosmos_name = "cosmos-mtg-prod-wcus-01"
        appinsights_name = "appi-mtg-prod-wcus-01"
        identity_name = "id-mtg-prod-wcus-01"
        aspnetcore_env = "Production"
    }
}

# Script-level variables
$script:ACR_SERVER = ""
$script:ACR_USERNAME = ""
$script:ACR_PASSWORD = ""
$script:CONTAINER_IMAGE = ""
$script:COSMOS_ENDPOINT = ""
$script:APP_INSIGHTS_CONN = ""
$script:MANAGED_IDENTITY_ID = ""
$script:APP_FQDN = ""

function Get-AcrCredentials {
    Write-Host "Retrieving Azure Container Registry credentials..." -ForegroundColor Blue

    $script:ACR_SERVER = (az acr show --name $ACR_NAME --query loginServer -o tsv).Trim()
    if ([string]::IsNullOrWhiteSpace($script:ACR_SERVER)) {
        Write-Host "Failed to retrieve ACR server" -ForegroundColor Red
        exit 1
    }

    $script:ACR_USERNAME = (az acr credential show --name $ACR_NAME --query username -o tsv).Trim()
    $script:ACR_PASSWORD = (az acr credential show --name $ACR_NAME --query "passwords[0].value" -o tsv).Trim()

    if ([string]::IsNullOrWhiteSpace($script:ACR_USERNAME) -or [string]::IsNullOrWhiteSpace($script:ACR_PASSWORD)) {
        Write-Host "Failed to retrieve ACR credentials" -ForegroundColor Red
        exit 1
    }

    Write-Host "ACR credentials retrieved successfully" -ForegroundColor Green
    Write-Host "ACR Server: $($script:ACR_SERVER)" -ForegroundColor Green
}

function Connect-ToAcr {
    Write-Host "Logging in to Azure Container Registry..." -ForegroundColor Blue
    $script:ACR_PASSWORD | docker login $script:ACR_SERVER --username $script:ACR_USERNAME --password-stdin

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Successfully logged in to ACR" -ForegroundColor Green
    } else {
        Write-Host "Failed to login to ACR" -ForegroundColor Red
        exit 1
    }
}

function Build-AndPush {
    $buildId = [DateTimeOffset]::UtcNow.ToUnixTimeSeconds()
    Write-Host "Building container image with tag: $buildId" -ForegroundColor Blue

    # Check if project file exists
    if (-not (Test-Path $PROJECT_PATH)) {
        Write-Host "Error: Project file not found at $PROJECT_PATH" -ForegroundColor Red
        exit 1
    }

    # Build and push using .NET's built-in container support
    dotnet publish $PROJECT_PATH `
        --configuration Release `
        /t:PublishContainer `
        /p:ContainerRepository=$CONTAINER_IMAGE_NAME `
        /p:ContainerImageTag=$buildId `
        /p:ContainerRegistry=$script:ACR_SERVER

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Container image built and pushed successfully" -ForegroundColor Green
        $script:CONTAINER_IMAGE = "$($script:ACR_SERVER)/$($CONTAINER_IMAGE_NAME):$buildId"
        Write-Host "Image: $($script:CONTAINER_IMAGE)" -ForegroundColor Green
    } else {
        Write-Host "Failed to build and push container image" -ForegroundColor Red
        exit 1
    }
}

function Get-ResourceInfo {
    param([string]$Env)

    $config = $EnvConfig[$Env]
    $rg = $config.resource_group

    Write-Host "Retrieving Azure resource information..." -ForegroundColor Blue

    # Cosmos DB endpoint
    $script:COSMOS_ENDPOINT = (az cosmosdb show `
        --name $config.cosmos_name `
        --resource-group $rg `
        --query documentEndpoint -o tsv).Trim()

    if ([string]::IsNullOrWhiteSpace($script:COSMOS_ENDPOINT)) {
        Write-Host "Failed to retrieve Cosmos DB endpoint" -ForegroundColor Red
        exit 1
    }

    # Application Insights connection string
    $script:APP_INSIGHTS_CONN = (az monitor app-insights component show `
        --app $config.appinsights_name `
        --resource-group $rg `
        --query connectionString -o tsv).Trim()

    if ([string]::IsNullOrWhiteSpace($script:APP_INSIGHTS_CONN)) {
        Write-Host "Failed to retrieve Application Insights connection string" -ForegroundColor Red
        Write-Host "Run .\create-app-insights.ps1 to create the missing resource" -ForegroundColor Yellow
        exit 1
    }

    # Managed Identity client ID
    $script:MANAGED_IDENTITY_ID = (az identity show `
        --name $config.identity_name `
        --resource-group $rg `
        --query clientId -o tsv).Trim()

    if ([string]::IsNullOrWhiteSpace($script:MANAGED_IDENTITY_ID)) {
        Write-Host "Failed to retrieve Managed Identity client ID" -ForegroundColor Red
        exit 1
    }

    Write-Host "Resource information retrieved successfully" -ForegroundColor Green
}

function Update-ContainerApp {
    param([string]$Env)

    $config = $EnvConfig[$Env]
    $appName = $config.app_name
    $rg = $config.resource_group

    Write-Host "Updating Container App: $appName" -ForegroundColor Blue

    # Get managed identity resource ID
    $identityName = $config.identity_name
    $subscriptionId = (az account show --query id -o tsv).Trim()
    $identityResourceId = "/subscriptions/$subscriptionId/resourceGroups/$rg/providers/Microsoft.ManagedIdentity/userAssignedIdentities/$identityName"

    # Configure registry credentials with managed identity
    Write-Host "Configuring ACR authentication with managed identity" -ForegroundColor Blue
    az containerapp registry set `
        --name $appName `
        --resource-group $rg `
        --server $script:ACR_SERVER `
        --identity $identityResourceId

    az containerapp update `
        --name $appName `
        --resource-group $rg `
        --image $script:CONTAINER_IMAGE `
        --replace-env-vars `
            ASPNETCORE_ENVIRONMENT=$($config.aspnetcore_env) `
            APPLICATIONINSIGHTS_CONNECTION_STRING=$script:APP_INSIGHTS_CONN `
            AZURE_CLIENT_ID=$script:MANAGED_IDENTITY_ID

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Container App updated successfully" -ForegroundColor Green
    } else {
        Write-Host "Failed to update Container App" -ForegroundColor Red
        exit 1
    }
}

function Get-AppUrl {
    param([string]$Env)

    $config = $EnvConfig[$Env]
    $appName = $config.app_name
    $rg = $config.resource_group

    $script:APP_FQDN = (az containerapp show `
        --name $appName `
        --resource-group $rg `
        --query properties.configuration.ingress.fqdn -o tsv).Trim()

    if ([string]::IsNullOrWhiteSpace($script:APP_FQDN)) {
        Write-Host "Warning: Could not retrieve Container App FQDN" -ForegroundColor Yellow
    }
}

function Deploy-Environment {
    param([string]$Env, [string]$DisplayName)

    Write-Host "========================================" -ForegroundColor Yellow
    Write-Host "Deploying to $DisplayName environment" -ForegroundColor Yellow
    Write-Host "========================================" -ForegroundColor Yellow

    # Get ACR credentials and login
    Get-AcrCredentials
    Connect-ToAcr

    # Build and push container image
    Build-AndPush

    # Get resource information
    Get-ResourceInfo -Env $Env

    # Update Container App
    Update-ContainerApp -Env $Env

    # Get app URL
    Get-AppUrl -Env $Env

    Write-Host "========================================" -ForegroundColor Green
    Write-Host "$DisplayName deployment complete!" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Green
    Write-Host "GraphQL Endpoint: https://$($script:APP_FQDN)/graphql" -ForegroundColor Green
    Write-Host "Health Endpoint: https://$($script:APP_FQDN)/health" -ForegroundColor Green
}

function Deploy-Preview {
    Deploy-Environment -Env "preview" -DisplayName "PREVIEW"
}

function Deploy-Dev {
    Deploy-Environment -Env "dev" -DisplayName "DEV"
}

function Deploy-Production {
    Write-Host "========================================" -ForegroundColor Yellow
    Write-Host "Deploying to PRODUCTION environment" -ForegroundColor Yellow
    Write-Host "========================================" -ForegroundColor Yellow

    # Confirmation prompt
    Write-Host "WARNING: This will deploy to PRODUCTION!" -ForegroundColor Red
    $reply = Read-Host "Are you sure you want to continue? (yes/no)"
    if ($reply -ne "yes") {
        Write-Host "Production deployment cancelled" -ForegroundColor Yellow
        exit 0
    }

    Deploy-Environment -Env "prod" -DisplayName "PRODUCTION"
}

function Show-Status {
    Write-Host "========================================" -ForegroundColor Blue
    Write-Host "Container Apps Status" -ForegroundColor Blue
    Write-Host "========================================" -ForegroundColor Blue
    Write-Host ""

    Write-Host "DEV Environment:" -ForegroundColor Yellow
    try {
        az containerapp show `
            --name $EnvConfig.dev.app_name `
            --resource-group $EnvConfig.dev.resource_group `
            --query "{name:name,status:properties.runningStatus,replicas:properties.template.scale.maxReplicas,fqdn:properties.configuration.ingress.fqdn}" `
            -o table
    } catch {
        Write-Host "Dev environment not accessible" -ForegroundColor Red
    }
    Write-Host ""

    Write-Host "PROD Environment:" -ForegroundColor Yellow
    try {
        az containerapp show `
            --name $EnvConfig.prod.app_name `
            --resource-group $EnvConfig.prod.resource_group `
            --query "{name:name,status:properties.runningStatus,replicas:properties.template.scale.maxReplicas,fqdn:properties.configuration.ingress.fqdn}" `
            -o table
    } catch {
        Write-Host "Prod environment not accessible" -ForegroundColor Red
    }
    Write-Host ""
}

function Show-Logs {
    param([string]$Env)

    if ([string]::IsNullOrWhiteSpace($Env)) {
        Write-Host "Error: Please specify environment (dev or prod)" -ForegroundColor Red
        Write-Host "Usage: .\deploy-backend.ps1 logs [dev|prod]"
        exit 1
    }

    $config = $EnvConfig[$Env]
    if ($null -eq $config) {
        Write-Host "Error: Unknown environment '$Env'. Use 'dev' or 'prod'" -ForegroundColor Red
        exit 1
    }

    Write-Host "========================================" -ForegroundColor Blue
    Write-Host "Container App Logs - $($Env.ToUpper())" -ForegroundColor Blue
    Write-Host "========================================" -ForegroundColor Blue

    az containerapp logs show `
        --name $config.app_name `
        --resource-group $config.resource_group `
        --follow
}

function Show-Menu {
    Write-Host "========================================" -ForegroundColor Blue
    Write-Host "Backend Deployment Menu" -ForegroundColor Blue
    Write-Host "========================================" -ForegroundColor Blue
    Write-Host ""
    Write-Host "1) Deploy to Preview"
    Write-Host "2) Deploy to Dev"
    Write-Host "3) Deploy to Production"
    Write-Host "4) Show Status"
    Write-Host "5) Show Dev Logs"
    Write-Host "6) Show Prod Logs"
    Write-Host "7) Exit"
    Write-Host ""
    $choice = Read-Host "Select an option (1-7)"

    switch ($choice) {
        "1" { Deploy-Preview }
        "2" { Deploy-Dev }
        "3" { Deploy-Production }
        "4" { Show-Status }
        "5" { Show-Logs -Env "dev" }
        "6" { Show-Logs -Env "prod" }
        "7" { Write-Host "Exiting..." -ForegroundColor Green; exit 0 }
        default { Write-Host "Invalid option" -ForegroundColor Red; exit 1 }
    }
}

function Show-Help {
    Write-Host "Usage: .\deploy-backend.ps1 [preview|dev|production|status|logs]"
    Write-Host ""
    Write-Host "Options:"
    Write-Host "  preview      Deploy to preview environment"
    Write-Host "  dev          Deploy to dev environment"
    Write-Host "  production   Deploy to production environment"
    Write-Host "  status       Show deployment status"
    Write-Host "  logs [env]   Show logs for environment (dev or prod)"
    Write-Host "  help         Show this help message"
    Write-Host "  (no args)    Show interactive menu"
}

# Main
switch ($Command.ToLower()) {
    "" { Show-Menu }
    "preview" { Deploy-Preview }
    "dev" { Deploy-Dev }
    "production" { Deploy-Production }
    "prod" { Deploy-Production }
    "status" { Show-Status }
    "logs" { Show-Logs -Env $LogEnv }
    "help" { Show-Help }
    "-h" { Show-Help }
    "--help" { Show-Help }
    default {
        Write-Host "Unknown option: $Command" -ForegroundColor Red
        Write-Host ""
        Show-Help
        exit 1
    }
}
