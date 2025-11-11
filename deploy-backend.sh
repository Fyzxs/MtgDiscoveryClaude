#!/bin/bash

# Azure Container Apps Backend Deployment Script
# Usage: ./deploy-backend.sh [preview|dev|production]

set -e  # Exit on error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
PROJECT_PATH="src/App.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL.csproj"
CONTAINER_IMAGE_NAME="mtg-backend"
ACR_NAME="crmtgsharedwcus01"

# Environment-specific configuration
declare -A ENV_CONFIG
ENV_CONFIG[preview_app_name]="ca-mtg-dev-wcus-01"
ENV_CONFIG[preview_resource_group]="rg-mtg-dev-wcus-01"
ENV_CONFIG[preview_cosmos_name]="cosmos-mtg-dev-wcus-01"
ENV_CONFIG[preview_appinsights_name]="appi-mtg-dev-wcus-01"
ENV_CONFIG[preview_identity_name]="id-mtg-dev-wcus-01"
ENV_CONFIG[preview_aspnetcore_env]="Development"

ENV_CONFIG[dev_app_name]="ca-mtg-dev-wcus-01"
ENV_CONFIG[dev_resource_group]="rg-mtg-dev-wcus-01"
ENV_CONFIG[dev_cosmos_name]="cosmos-mtg-dev-wcus-01"
ENV_CONFIG[dev_appinsights_name]="appi-mtg-dev-wcus-01"
ENV_CONFIG[dev_identity_name]="id-mtg-dev-wcus-01"
ENV_CONFIG[dev_aspnetcore_env]="Development"

ENV_CONFIG[prod_app_name]="ca-mtg-prod-wcus-01"
ENV_CONFIG[prod_resource_group]="rg-mtg-prod-wcus-01"
ENV_CONFIG[prod_cosmos_name]="cosmos-mtg-prod-wcus-01"
ENV_CONFIG[prod_appinsights_name]="appi-mtg-prod-wcus-01"
ENV_CONFIG[prod_identity_name]="id-mtg-prod-wcus-01"
ENV_CONFIG[prod_aspnetcore_env]="Production"

# Get ACR credentials
get_acr_credentials() {
    echo -e "${BLUE}Retrieving Azure Container Registry credentials...${NC}"

    ACR_SERVER=$(az acr show --name "$ACR_NAME" --query loginServer -o tsv)
    if [ -z "$ACR_SERVER" ]; then
        echo -e "${RED}Failed to retrieve ACR server${NC}"
        exit 1
    fi

    ACR_USERNAME=$(az acr credential show --name "$ACR_NAME" --query username -o tsv)
    ACR_PASSWORD=$(az acr credential show --name "$ACR_NAME" --query passwords[0].value -o tsv)

    if [ -z "$ACR_USERNAME" ] || [ -z "$ACR_PASSWORD" ]; then
        echo -e "${RED}Failed to retrieve ACR credentials${NC}"
        exit 1
    fi

    echo -e "${GREEN}ACR credentials retrieved successfully${NC}"
    echo -e "ACR Server: ${GREEN}${ACR_SERVER}${NC}"
}

# Login to ACR
login_to_acr() {
    echo -e "${BLUE}Logging in to Azure Container Registry...${NC}"
    echo "$ACR_PASSWORD" | docker login "$ACR_SERVER" --username "$ACR_USERNAME" --password-stdin

    if [ $? -eq 0 ]; then
        echo -e "${GREEN}✓ Successfully logged in to ACR${NC}"
    else
        echo -e "${RED}Failed to login to ACR${NC}"
        exit 1
    fi
}

# Build and push container image
build_and_push() {
    local build_id=$(date +%s)
    echo -e "${BLUE}Building container image with tag: ${build_id}${NC}"

    # Check if project file exists
    if [ ! -f "$PROJECT_PATH" ]; then
        echo -e "${RED}Error: Project file not found at ${PROJECT_PATH}${NC}"
        exit 1
    fi

    # Build and push using .NET's built-in container support
    dotnet publish "$PROJECT_PATH" \
        --configuration Release \
        /t:PublishContainer \
        /p:ContainerRepository="$CONTAINER_IMAGE_NAME" \
        /p:ContainerImageTag="$build_id" \
        /p:ContainerRegistry="$ACR_SERVER"

    if [ $? -eq 0 ]; then
        echo -e "${GREEN}✓ Container image built and pushed successfully${NC}"
        CONTAINER_IMAGE="${ACR_SERVER}/${CONTAINER_IMAGE_NAME}:${build_id}"
        echo -e "Image: ${GREEN}${CONTAINER_IMAGE}${NC}"
    else
        echo -e "${RED}Failed to build and push container image${NC}"
        exit 1
    fi
}

# Get Azure resource information
get_resource_info() {
    local env=$1
    local rg="${ENV_CONFIG[${env}_resource_group]}"

    echo -e "${BLUE}Retrieving Azure resource information...${NC}"

    # Cosmos DB endpoint
    COSMOS_ENDPOINT=$(az cosmosdb show \
        --name "${ENV_CONFIG[${env}_cosmos_name]}" \
        --resource-group "$rg" \
        --query documentEndpoint -o tsv)

    if [ -z "$COSMOS_ENDPOINT" ]; then
        echo -e "${RED}Failed to retrieve Cosmos DB endpoint${NC}"
        exit 1
    fi

    # Application Insights connection string
    APP_INSIGHTS_CONN=$(az monitor app-insights component show \
        --app "${ENV_CONFIG[${env}_appinsights_name]}" \
        --resource-group "$rg" \
        --query connectionString -o tsv)

    if [ -z "$APP_INSIGHTS_CONN" ]; then
        echo -e "${RED}Failed to retrieve Application Insights connection string${NC}"
        echo -e "${YELLOW}Run ./create-app-insights.sh to create the missing resource${NC}"
        exit 1
    fi

    # Managed Identity client ID
    MANAGED_IDENTITY_ID=$(az identity show \
        --name "${ENV_CONFIG[${env}_identity_name]}" \
        --resource-group "$rg" \
        --query clientId -o tsv)

    if [ -z "$MANAGED_IDENTITY_ID" ]; then
        echo -e "${RED}Failed to retrieve Managed Identity client ID${NC}"
        exit 1
    fi

    echo -e "${GREEN}✓ Resource information retrieved successfully${NC}"
}

# Update Container App
update_container_app() {
    local env=$1
    local app_name="${ENV_CONFIG[${env}_app_name]}"
    local rg="${ENV_CONFIG[${env}_resource_group]}"

    echo -e "${BLUE}Updating Container App: ${app_name}${NC}"

    # Get managed identity resource ID
    local IDENTITY_NAME="${ENV_CONFIG[${env}_identity_name]}"
    local IDENTITY_RESOURCE_ID="/subscriptions/$(az account show --query id -o tsv)/resourceGroups/$rg/providers/Microsoft.ManagedIdentity/userAssignedIdentities/$IDENTITY_NAME"

    # Configure registry credentials with managed identity
    echo -e "${BLUE}Configuring ACR authentication with managed identity${NC}"
    az containerapp registry set \
        --name "$app_name" \
        --resource-group "$rg" \
        --server "$ACR_SERVER" \
        --identity "$IDENTITY_RESOURCE_ID"

    az containerapp update \
        --name "$app_name" \
        --resource-group "$rg" \
        --image "$CONTAINER_IMAGE" \
        --replace-env-vars \
            ASPNETCORE_ENVIRONMENT="${ENV_CONFIG[${env}_aspnetcore_env]}" \
            APPLICATIONINSIGHTS_CONNECTION_STRING="$APP_INSIGHTS_CONN" \
            AZURE_CLIENT_ID="$MANAGED_IDENTITY_ID"

    if [ $? -eq 0 ]; then
        echo -e "${GREEN}✓ Container App updated successfully${NC}"
    else
        echo -e "${RED}Failed to update Container App${NC}"
        exit 1
    fi
}

# Configure health probes
configure_health_probes() {
    local env=$1
    local app_name="${ENV_CONFIG[${env}_app_name]}"
    local rg="${ENV_CONFIG[${env}_resource_group]}"

    echo -e "${BLUE}Skipping health probe configuration (using Container App defaults)${NC}"
    echo -e "${CYAN}Note: Health probes can be configured via Azure Portal if custom settings are needed${NC}"

    # Health probes should be configured during initial container app creation
    # or via Azure Portal / ARM templates for fine-grained control
    # The CLI parameters for health probes have changed and are not reliably scriptable
}

# Get Container App URL
get_app_url() {
    local env=$1
    local app_name="${ENV_CONFIG[${env}_app_name]}"
    local rg="${ENV_CONFIG[${env}_resource_group]}"

    APP_FQDN=$(az containerapp show \
        --name "$app_name" \
        --resource-group "$rg" \
        --query properties.configuration.ingress.fqdn -o tsv)

    if [ -z "$APP_FQDN" ]; then
        echo -e "${YELLOW}Warning: Could not retrieve Container App FQDN${NC}"
    fi
}

# Deploy to preview
deploy_preview() {
    echo -e "${YELLOW}========================================${NC}"
    echo -e "${YELLOW}Deploying to PREVIEW environment${NC}"
    echo -e "${YELLOW}========================================${NC}"

    # Get ACR credentials and login
    get_acr_credentials
    login_to_acr

    # Build and push container image
    build_and_push

    # Get resource information
    get_resource_info "preview"

    # Update Container App
    update_container_app "preview"

    # Configure health probes
    configure_health_probes "preview"

    # Get app URL
    get_app_url "preview"

    echo -e "${GREEN}========================================${NC}"
    echo -e "${GREEN}✓ Preview deployment complete!${NC}"
    echo -e "${GREEN}========================================${NC}"
    echo -e "${GREEN}GraphQL Endpoint: https://${APP_FQDN}/graphql${NC}"
    echo -e "${GREEN}Health Endpoint: https://${APP_FQDN}/health${NC}"
    echo ""
}

# Deploy to dev
deploy_dev() {
    echo -e "${YELLOW}========================================${NC}"
    echo -e "${YELLOW}Deploying to DEV environment${NC}"
    echo -e "${YELLOW}========================================${NC}"

    # Get ACR credentials and login
    get_acr_credentials
    login_to_acr

    # Build and push container image
    build_and_push

    # Get resource information
    get_resource_info "dev"

    # Update Container App
    update_container_app "dev"

    # Configure health probes
    configure_health_probes "dev"

    # Get app URL
    get_app_url "dev"

    echo -e "${GREEN}========================================${NC}"
    echo -e "${GREEN}✓ Dev deployment complete!${NC}"
    echo -e "${GREEN}========================================${NC}"
    echo -e "${GREEN}GraphQL Endpoint: https://${APP_FQDN}/graphql${NC}"
    echo -e "${GREEN}Health Endpoint: https://${APP_FQDN}/health${NC}"
    echo ""
}

# Deploy to production
deploy_production() {
    echo -e "${YELLOW}========================================${NC}"
    echo -e "${YELLOW}Deploying to PRODUCTION environment${NC}"
    echo -e "${YELLOW}========================================${NC}"

    # Confirmation prompt
    echo -e "${RED}WARNING: This will deploy to PRODUCTION!${NC}"
    read -p "Are you sure you want to continue? (yes/no): " -r
    echo
    if [[ ! $REPLY =~ ^[Yy][Ee][Ss]$ ]]; then
        echo -e "${YELLOW}Production deployment cancelled${NC}"
        exit 0
    fi

    # Get ACR credentials and login
    get_acr_credentials
    login_to_acr

    # Build and push container image
    build_and_push

    # Get resource information
    get_resource_info "prod"

    # Update Container App
    update_container_app "prod"

    # Configure health probes
    configure_health_probes "prod"

    # Get app URL
    get_app_url "prod"

    echo -e "${GREEN}========================================${NC}"
    echo -e "${GREEN}✓ Production deployment complete!${NC}"
    echo -e "${GREEN}========================================${NC}"
    echo -e "${GREEN}GraphQL Endpoint: https://${APP_FQDN}/graphql${NC}"
    echo -e "${GREEN}Health Endpoint: https://${APP_FQDN}/health${NC}"
    echo ""
}

# Show current status
show_status() {
    echo -e "${BLUE}========================================${NC}"
    echo -e "${BLUE}Container Apps Status${NC}"
    echo -e "${BLUE}========================================${NC}"
    echo ""

    echo -e "${YELLOW}DEV Environment:${NC}"
    az containerapp show \
        --name "${ENV_CONFIG[dev_app_name]}" \
        --resource-group "${ENV_CONFIG[dev_resource_group]}" \
        --query "{name:name,status:properties.runningStatus,replicas:properties.template.scale.maxReplicas,fqdn:properties.configuration.ingress.fqdn}" \
        -o table 2>/dev/null || echo -e "${RED}Dev environment not accessible${NC}"
    echo ""

    echo -e "${YELLOW}PROD Environment:${NC}"
    az containerapp show \
        --name "${ENV_CONFIG[prod_app_name]}" \
        --resource-group "${ENV_CONFIG[prod_resource_group]}" \
        --query "{name:name,status:properties.runningStatus,replicas:properties.template.scale.maxReplicas,fqdn:properties.configuration.ingress.fqdn}" \
        -o table 2>/dev/null || echo -e "${RED}Prod environment not accessible${NC}"
    echo ""
}

# Show logs
show_logs() {
    local env=$1
    local app_name="${ENV_CONFIG[${env}_app_name]}"
    local rg="${ENV_CONFIG[${env}_resource_group]}"

    echo -e "${BLUE}========================================${NC}"
    echo -e "${BLUE}Container App Logs - ${env^^}${NC}"
    echo -e "${BLUE}========================================${NC}"

    az containerapp logs show \
        --name "$app_name" \
        --resource-group "$rg" \
        --follow
}

# Main menu
show_menu() {
    echo -e "${BLUE}========================================${NC}"
    echo -e "${BLUE}Backend Deployment Menu${NC}"
    echo -e "${BLUE}========================================${NC}"
    echo ""
    echo "1) Deploy to Preview"
    echo "2) Deploy to Dev"
    echo "3) Deploy to Production"
    echo "4) Show Status"
    echo "5) Show Dev Logs"
    echo "6) Show Prod Logs"
    echo "7) Exit"
    echo ""
    read -p "Select an option (1-7): " choice

    case $choice in
        1)
            deploy_preview
            ;;
        2)
            deploy_dev
            ;;
        3)
            deploy_production
            ;;
        4)
            show_status
            ;;
        5)
            show_logs "dev"
            ;;
        6)
            show_logs "prod"
            ;;
        7)
            echo -e "${GREEN}Exiting...${NC}"
            exit 0
            ;;
        *)
            echo -e "${RED}Invalid option${NC}"
            exit 1
            ;;
    esac
}

# Parse command line arguments
if [ $# -eq 0 ]; then
    # No arguments, show interactive menu
    show_menu
else
    case "$1" in
        preview|--preview|-p)
            deploy_preview
            ;;
        dev|--dev|-d)
            deploy_dev
            ;;
        production|prod|--production|-P)
            deploy_production
            ;;
        status|--status|-s)
            show_status
            ;;
        logs)
            if [ -z "$2" ]; then
                echo -e "${RED}Error: Please specify environment (dev or prod)${NC}"
                echo "Usage: $0 logs [dev|prod]"
                exit 1
            fi
            show_logs "$2"
            ;;
        *)
            echo -e "${RED}Unknown option: $1${NC}"
            echo ""
            echo "Usage: $0 [preview|dev|production|status|logs]"
            echo ""
            echo "Options:"
            echo "  preview, -p      Deploy to preview environment"
            echo "  dev, -d          Deploy to dev environment"
            echo "  production, -P   Deploy to production environment"
            echo "  status, -s       Show deployment status"
            echo "  logs [dev|prod]  Show logs for environment"
            echo "  (no args)        Show interactive menu"
            exit 1
            ;;
    esac
fi
