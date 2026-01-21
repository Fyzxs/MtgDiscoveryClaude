# Azure Static Web App Deployment Script (PowerShell)
# Usage: .\deploy.ps1 [preview|production|status|help]

param(
    [Parameter(Position=0)]
    [string]$Command = ""
)

$ErrorActionPreference = "Stop"

# Configuration
$APP_NAME = "swa-mtg-dev-wcus-01"
$RESOURCE_GROUP = "rg-mtg-dev-wcus-01"
$PREVIEW_URL = "https://ambitious-smoke-0f17c3f1e-preview.westus2.3.azurestaticapps.net"
$PRODUCTION_URL = "https://ambitious-smoke-0f17c3f1e.3.azurestaticapps.net"

function Get-DeploymentToken {
    Write-Host "Retrieving deployment token..." -ForegroundColor Blue
    $token = az staticwebapp secrets list `
        --name $APP_NAME `
        --resource-group $RESOURCE_GROUP `
        --query "properties.apiKey" -o tsv

    if ([string]::IsNullOrWhiteSpace($token)) {
        Write-Host "Failed to retrieve deployment token" -ForegroundColor Red
        exit 1
    }
    Write-Host "Token retrieved successfully" -ForegroundColor Green
    return $token.Trim()
}

function Deploy-Preview {
    Write-Host "========================================" -ForegroundColor Yellow
    Write-Host "Deploying to PREVIEW environment" -ForegroundColor Yellow
    Write-Host "========================================" -ForegroundColor Yellow

    # Check if .env.preview.local exists
    if (-not (Test-Path ".env.preview.local")) {
        Write-Host "Error: .env.preview.local not found" -ForegroundColor Red
        Write-Host "Please create .env.preview.local with your preview Auth0 credentials" -ForegroundColor Yellow
        exit 1
    }

    # Build with preview environment
    Write-Host "Building application for preview..." -ForegroundColor Blue
    npx vite build --mode preview
    if ($LASTEXITCODE -ne 0) { exit 1 }

    # Get deployment token
    $token = Get-DeploymentToken

    # Deploy to preview
    Write-Host "Deploying to preview environment..." -ForegroundColor Blue
    swa deploy ./dist `
        --deployment-token $token `
        --app-name $APP_NAME `
        --env preview

    if ($LASTEXITCODE -ne 0) { exit 1 }

    Write-Host "========================================" -ForegroundColor Green
    Write-Host "Preview deployment complete!" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Green
    Write-Host "Preview URL: $PREVIEW_URL" -ForegroundColor Green
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

    # Check if .env.production.local exists
    if (-not (Test-Path ".env.production.local")) {
        Write-Host "Error: .env.production.local not found" -ForegroundColor Red
        Write-Host "Please create .env.production.local with your production Auth0 credentials" -ForegroundColor Yellow
        exit 1
    }

    # Build with production environment
    Write-Host "Building application for production..." -ForegroundColor Blue
    npm run build
    if ($LASTEXITCODE -ne 0) { exit 1 }

    # Get deployment token
    $token = Get-DeploymentToken

    # Deploy to production
    Write-Host "Deploying to production environment..." -ForegroundColor Blue
    swa deploy ./dist `
        --deployment-token $token `
        --app-name $APP_NAME `
        --env production

    if ($LASTEXITCODE -ne 0) { exit 1 }

    Write-Host "========================================" -ForegroundColor Green
    Write-Host "Production deployment complete!" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Green
    Write-Host "Production URL: $PRODUCTION_URL" -ForegroundColor Green
}

function Show-Status {
    Write-Host "========================================" -ForegroundColor Blue
    Write-Host "Current Deployment Status" -ForegroundColor Blue
    Write-Host "========================================" -ForegroundColor Blue
    Write-Host "App Name: $APP_NAME" -ForegroundColor Green
    Write-Host "Resource Group: $RESOURCE_GROUP" -ForegroundColor Green
    Write-Host ""
    Write-Host "Preview URL:    $PREVIEW_URL" -ForegroundColor Green
    Write-Host "Production URL: $PRODUCTION_URL" -ForegroundColor Green
    Write-Host ""

    Write-Host "Checking Azure Static Web App details..." -ForegroundColor Blue
    az staticwebapp show `
        --name $APP_NAME `
        --resource-group $RESOURCE_GROUP `
        --query "{name:name,defaultHostname:defaultHostname,location:location}" `
        -o table
}

function Show-Menu {
    Write-Host "========================================" -ForegroundColor Blue
    Write-Host "Azure Static Web App Deployment" -ForegroundColor Blue
    Write-Host "========================================" -ForegroundColor Blue
    Write-Host ""
    Write-Host "1) Deploy to Preview"
    Write-Host "2) Deploy to Production"
    Write-Host "3) Show Status"
    Write-Host "4) Exit"
    Write-Host ""
    $choice = Read-Host "Select an option (1-4)"

    switch ($choice) {
        "1" { Deploy-Preview }
        "2" { Deploy-Production }
        "3" { Show-Status }
        "4" { Write-Host "Exiting..." -ForegroundColor Green; exit 0 }
        default { Write-Host "Invalid option" -ForegroundColor Red; exit 1 }
    }
}

function Show-Help {
    Write-Host "Usage: .\deploy.ps1 [preview|production|status]"
    Write-Host ""
    Write-Host "Options:"
    Write-Host "  preview      Deploy to preview environment"
    Write-Host "  production   Deploy to production environment"
    Write-Host "  status       Show deployment status"
    Write-Host "  help         Show this help message"
    Write-Host "  (no args)    Show interactive menu"
}

# Main
switch ($Command.ToLower()) {
    "" { Show-Menu }
    "preview" { Deploy-Preview }
    "production" { Deploy-Production }
    "prod" { Deploy-Production }
    "status" { Show-Status }
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
