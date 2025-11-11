#!/bin/bash
# Azure Infrastructure Creation Script
# This script creates all Azure resources for the MTG Discovery application
# Run this when Azure DevOps build agents are down

set -e  # Exit on error

# Color output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Configuration
APP_NAME="mtg"
LOCATION="westcentralus"
REGION_SHORT="wcus"
SWA_LOCATION="westus2"  # Static Web App location
APPI_LOCATION="westus2"  # Application Insights location
SEQUENCE="01"

# Resource short names
RG_SHORT="rg"
SWA_SHORT="swa"
CR_SHORT="cr"
CA_SHORT="ca"
APPI_SHORT="appi"
COSMOS_SHORT="cosmos"
CAE_SHORT="cae"
LOG_SHORT="log"
ID_SHORT="id"
APPCONFIG_SHORT="appconfig"

# Container App settings
CA_PORT=8080
CA_CPU="0.5"
CA_MEMORY="1.0Gi"
CA_MAX_REPLICAS=10

# Auth0 configuration
AUTH0_DOMAIN="dev-63szoyl0kt0p7e5q.us.auth0.com"
AUTH0_AUDIENCE="api://mtg-discovery"
AUTH0_CLIENT_ID="OXYTiA2a2SpN3cNf4Er4ix9DHHhLdYC5"

# Cosmos DB settings
COSMOS_CONSISTENCY="Session"

# Functions
log_info() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

log_warn() {
    echo -e "${YELLOW}[WARN]${NC} $1"
}

log_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Check if Azure CLI is installed
if ! command -v az &> /dev/null; then
    log_error "Azure CLI is not installed. Please install it first."
    exit 1
fi

# Check if logged in
if ! az account show &> /dev/null; then
    log_error "Not logged into Azure. Please run 'az login' first."
    exit 1
fi

log_info "Starting infrastructure creation for MTG Discovery application"
log_info "Subscription: $(az account show --query name -o tsv)"

# Ask which environment to create
echo ""
echo "Which environment(s) do you want to create?"
echo "1) Shared resources only"
echo "2) Dev environment (includes shared)"
echo "3) Prod environment (includes shared)"
echo "4) All (shared + dev + prod)"
read -p "Enter choice (1-4): " CHOICE

CREATE_SHARED=false
CREATE_DEV=false
CREATE_PROD=false

case $CHOICE in
    1) CREATE_SHARED=true ;;
    2) CREATE_SHARED=true; CREATE_DEV=true ;;
    3) CREATE_SHARED=true; CREATE_PROD=true ;;
    4) CREATE_SHARED=true; CREATE_DEV=true; CREATE_PROD=true ;;
    *) log_error "Invalid choice"; exit 1 ;;
esac

################################################################################
# CREATE SHARED RESOURCES
################################################################################

if [ "$CREATE_SHARED" = true ]; then
    log_info "========================================"
    log_info "Creating Shared Resources"
    log_info "========================================"

    # Shared resource names
    SHARED_RG="${RG_SHORT}-${APP_NAME}-shared-${REGION_SHORT}-${SEQUENCE}"
    SHARED_CR="${CR_SHORT}${APP_NAME}shared${REGION_SHORT}${SEQUENCE}"

    log_info "Creating shared resource group: $SHARED_RG"
    az group create \
        --name "$SHARED_RG" \
        --location "$LOCATION" \
        --tags Environment=shared Application="$APP_NAME" ManagedBy=Script

    log_info "Creating Azure Container Registry: $SHARED_CR"
    ACR_RESULT=$(az acr list --query "[?name=='$SHARED_CR'].id" -o tsv)
    if [ -z "$ACR_RESULT" ]; then
        az acr create \
            --name "$SHARED_CR" \
            --resource-group "$SHARED_RG" \
            --location "$LOCATION" \
            --sku Basic \
            --admin-enabled true \
            --tags Environment=shared Application="$APP_NAME" ManagedBy=Script
    else
        log_warn "Container Registry already exists, skipping"
    fi

    log_info "Shared resources created successfully"
    echo ""
fi

################################################################################
# FUNCTION: CREATE ENVIRONMENT
################################################################################

create_environment() {
    local ENV=$1
    local ENV_LOCATION=$2
    local APPCONFIG_LOCATION=$3
    local CA_MIN_REPLICAS=$4
    local CA_MAX_REPLICAS_ENV=$5
    local SWA_SKU=$6

    log_info "========================================"
    log_info "Creating $ENV Environment"
    log_info "========================================"

    # Generate resource names
    local RG="${RG_SHORT}-${APP_NAME}-${ENV}-${REGION_SHORT}-${SEQUENCE}"
    local COSMOS="${COSMOS_SHORT}-${APP_NAME}-${ENV}-${REGION_SHORT}-${SEQUENCE}"
    local CAE="${CAE_SHORT}-${APP_NAME}-${ENV}-${REGION_SHORT}-${SEQUENCE}"
    local CA="${CA_SHORT}-${APP_NAME}-${ENV}-${REGION_SHORT}-${SEQUENCE}"
    local SWA="${SWA_SHORT}-${APP_NAME}-${ENV}-${REGION_SHORT}-${SEQUENCE}"
    local APPI="${APPI_SHORT}-${APP_NAME}-${ENV}-${REGION_SHORT}-${SEQUENCE}"
    local LOG="${LOG_SHORT}-${APP_NAME}-${ENV}-${REGION_SHORT}-${SEQUENCE}"
    local IDENTITY="${ID_SHORT}-${APP_NAME}-${ENV}-${REGION_SHORT}-${SEQUENCE}"
    local APPCONFIG="${APPCONFIG_SHORT}-${APP_NAME}-${ENV}-${REGION_SHORT}-${SEQUENCE}"

    # Create resource group
    log_info "Creating resource group: $RG"
    az group create \
        --name "$RG" \
        --location "$ENV_LOCATION" \
        --tags Environment="$ENV" Application="$APP_NAME" ManagedBy=Script

    # Create Log Analytics workspace
    log_info "Creating Log Analytics workspace: $LOG"
    az monitor log-analytics workspace create \
        --resource-group "$RG" \
        --workspace-name "$LOG" \
        --location "$ENV_LOCATION" \
        --tags Environment="$ENV" Application="$APP_NAME"

    local WORKSPACE_ID=$(az monitor log-analytics workspace show \
        --resource-group "$RG" \
        --workspace-name "$LOG" \
        --query customerId \
        -o tsv)

    local WORKSPACE_KEY=$(az monitor log-analytics workspace get-shared-keys \
        --resource-group "$RG" \
        --workspace-name "$LOG" \
        --query primarySharedKey \
        -o tsv)

    # Create Application Insights
    log_info "Creating Application Insights: $APPI in $APPI_LOCATION"
    az monitor app-insights component create \
        --app "$APPI" \
        --location "$APPI_LOCATION" \
        --resource-group "$RG" \
        --application-type web \
        --workspace "$WORKSPACE_ID" \
        --tags Environment="$ENV" Application="$APP_NAME"

    # Create App Configuration
    log_info "Creating App Configuration store: $APPCONFIG (Location: $APPCONFIG_LOCATION)"
    az appconfig create \
        --name "$APPCONFIG" \
        --resource-group "$RG" \
        --location "$APPCONFIG_LOCATION" \
        --sku Free \
        --tags Environment="$ENV" Application="$APP_NAME"

    # Populate App Configuration with Auth0 settings
    log_info "Populating App Configuration with Auth0 settings"
    az appconfig kv set --name "$APPCONFIG" --key "Auth0:Domain" --value "$AUTH0_DOMAIN" --yes
    az appconfig kv set --name "$APPCONFIG" --key "Auth0:Audience" --value "$AUTH0_AUDIENCE" --yes
    az appconfig kv set --name "$APPCONFIG" --key "Auth0:ClientId" --value "$AUTH0_CLIENT_ID" --yes

    # Create Cosmos DB (Serverless)
    log_info "Creating Cosmos DB account (Serverless): $COSMOS"
    COSMOS_RESULT=$(az cosmosdb list --resource-group "$RG" --query "[?name=='$COSMOS'].id" -o tsv)
    if [ -z "$COSMOS_RESULT" ]; then
        az cosmosdb create \
            --name "$COSMOS" \
            --resource-group "$RG" \
            --locations regionName="$ENV_LOCATION" \
            --capabilities EnableServerless \
            --default-consistency-level "$COSMOS_CONSISTENCY" \
            --tags Environment="$ENV" Application="$APP_NAME" CapacityMode=Serverless
    else
        log_warn "Cosmos DB already exists, skipping"
    fi

    # Create Managed Identity
    log_info "Creating user-assigned managed identity: $IDENTITY"
    az identity create \
        --name "$IDENTITY" \
        --resource-group "$RG" \
        --location "$ENV_LOCATION" \
        --tags Environment="$ENV" Application="$APP_NAME"

    local IDENTITY_ID=$(az identity show --name "$IDENTITY" --resource-group "$RG" --query id -o tsv)
    local PRINCIPAL_ID=$(az identity show --name "$IDENTITY" --resource-group "$RG" --query principalId -o tsv)
    local CLIENT_ID=$(az identity show --name "$IDENTITY" --resource-group "$RG" --query clientId -o tsv)

    log_info "Managed Identity Principal ID: $PRINCIPAL_ID"
    log_info "Managed Identity Client ID: $CLIENT_ID"

    # Create Container Apps Environment
    log_info "Creating Container Apps Environment: $CAE"
    az containerapp env create \
        --name "$CAE" \
        --resource-group "$RG" \
        --location "$ENV_LOCATION" \
        --logs-workspace-id "$WORKSPACE_ID" \
        --logs-workspace-key "$WORKSPACE_KEY" \
        --tags Environment="$ENV" Application="$APP_NAME"

    # Create Container App (with placeholder image)
    log_info "Creating Container App: $CA"
    az containerapp create \
        --name "$CA" \
        --resource-group "$RG" \
        --environment "$CAE" \
        --image mcr.microsoft.com/azuredocs/containerapps-helloworld:latest \
        --target-port "$CA_PORT" \
        --ingress external \
        --cpu "$CA_CPU" \
        --memory "$CA_MEMORY" \
        --min-replicas "$CA_MIN_REPLICAS" \
        --max-replicas "$CA_MAX_REPLICAS_ENV" \
        --user-assigned "$IDENTITY_ID" \
        --tags Environment="$ENV" Application="$APP_NAME"

    # Create Static Web App
    log_info "Creating Static Web App: $SWA in $SWA_LOCATION"
    az staticwebapp create \
        --name "$SWA" \
        --resource-group "$RG" \
        --location "$SWA_LOCATION" \
        --sku "$SWA_SKU" \
        --tags Environment="$ENV" Application="$APP_NAME"

    # Assign Cosmos DB RBAC role
    log_info "Assigning Cosmos DB Built-in Data Contributor role to managed identity"
    local ROLE_ID="00000000-0000-0000-0000-000000000002"

    if ! az cosmosdb sql role assignment list \
        --account-name "$COSMOS" \
        --resource-group "$RG" \
        --query "[?principalId=='$PRINCIPAL_ID'].id" -o tsv 2>/dev/null | grep -q .; then

        az cosmosdb sql role assignment create \
            --account-name "$COSMOS" \
            --resource-group "$RG" \
            --role-definition-id "$ROLE_ID" \
            --principal-id "$PRINCIPAL_ID" \
            --scope "/"
    else
        log_warn "Cosmos DB RBAC role already assigned, skipping"
    fi

    # Assign ACR Pull role
    log_info "Assigning AcrPull role to managed identity for shared ACR"
    local ACR_ID=$(az acr show --name "$SHARED_CR" --query id -o tsv)

    az role assignment create \
        --assignee "$PRINCIPAL_ID" \
        --role AcrPull \
        --scope "$ACR_ID" \
        2>/dev/null || log_warn "AcrPull role may already be assigned"

    # Assign App Configuration Data Reader role
    log_info "Assigning App Configuration Data Reader role to managed identity"
    local APPCONFIG_ID=$(az appconfig show --name "$APPCONFIG" --resource-group "$RG" --query id -o tsv)

    az role assignment create \
        --assignee "$PRINCIPAL_ID" \
        --role "App Configuration Data Reader" \
        --scope "$APPCONFIG_ID" \
        2>/dev/null || log_warn "App Configuration role may already be assigned"

    # Summary
    echo ""
    log_info "========================================"
    log_info "$ENV Environment Created Successfully"
    log_info "========================================"
    log_info "Resource Group: $RG"
    log_info "Managed Identity: $IDENTITY (Client ID: $CLIENT_ID)"
    log_info "App Configuration: $APPCONFIG (Location: $APPCONFIG_LOCATION)"
    log_info "Cosmos DB: $COSMOS (Serverless)"
    log_info "Container App: $CA"
    log_info "Static Web App: $SWA"
    log_info "Application Insights: $APPI"
    echo ""
}

################################################################################
# CREATE DEV ENVIRONMENT
################################################################################

if [ "$CREATE_DEV" = true ]; then
    create_environment "dev" "$LOCATION" "eastus" 0 1 "Free"
fi

################################################################################
# CREATE PROD ENVIRONMENT
################################################################################

if [ "$CREATE_PROD" = true ]; then
    create_environment "prod" "$LOCATION" "westcentralus" 1 2 "Free"
fi

################################################################################
# COMPLETION
################################################################################

log_info "========================================"
log_info "Infrastructure Creation Complete!"
log_info "========================================"
log_info "Next steps:"
log_info "1. Deploy backend application to Container Apps"
log_info "2. Deploy frontend application to Static Web Apps"
log_info "3. All authentication uses RBAC - no keys or passwords!"
log_info "4. Auth0 configuration stored in App Configuration"
