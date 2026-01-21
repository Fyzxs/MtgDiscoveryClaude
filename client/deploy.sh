#!/bin/bash

# Azure Static Web App Deployment Script
# Usage: ./deploy.sh [preview|production]

set -e  # Exit on error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
APP_NAME="swa-mtg-dev-wcus-01"
RESOURCE_GROUP="rg-mtg-dev-wcus-01"
PREVIEW_URL="https://ambitious-smoke-0f17c3f1e-preview.westus2.3.azurestaticapps.net"
PRODUCTION_URL="https://ambitious-smoke-0f17c3f1e.3.azurestaticapps.net"

# Get deployment token
get_deployment_token() {
    echo -e "${BLUE}Retrieving deployment token...${NC}"
    TOKEN=$(az staticwebapp secrets list \
        --name "$APP_NAME" \
        --resource-group "$RESOURCE_GROUP" \
        --query "properties.apiKey" -o tsv)

    if [ -z "$TOKEN" ]; then
        echo -e "${RED}Failed to retrieve deployment token${NC}"
        exit 1
    fi
    echo -e "${GREEN}Token retrieved successfully${NC}"
}

# Deploy to preview
deploy_preview() {
    echo -e "${YELLOW}========================================${NC}"
    echo -e "${YELLOW}Deploying to PREVIEW environment${NC}"
    echo -e "${YELLOW}========================================${NC}"

    # Check if .env.preview.local exists
    if [ ! -f ".env.preview.local" ]; then
        echo -e "${RED}Error: .env.preview.local not found${NC}"
        echo -e "${YELLOW}Please create .env.preview.local with your preview Auth0 credentials${NC}"
        exit 1
    fi

    # Build with preview environment
    echo -e "${BLUE}Building application for preview...${NC}"
    npm run build -- --mode preview

    # Get deployment token
    get_deployment_token

    # Deploy to preview
    echo -e "${BLUE}Deploying to preview environment...${NC}"
    swa deploy ./dist \
        --deployment-token "$TOKEN" \
        --app-name "$APP_NAME" \
        --env preview

    echo -e "${GREEN}========================================${NC}"
    echo -e "${GREEN}✓ Preview deployment complete!${NC}"
    echo -e "${GREEN}========================================${NC}"
    echo -e "${GREEN}Preview URL: ${PREVIEW_URL}${NC}"
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

    # Check if .env.production.local exists
    if [ ! -f ".env.production.local" ]; then
        echo -e "${RED}Error: .env.production.local not found${NC}"
        echo -e "${YELLOW}Please create .env.production.local with your production Auth0 credentials${NC}"
        exit 1
    fi

    # Build with production environment
    echo -e "${BLUE}Building application for production...${NC}"
    npm run build

    # Get deployment token
    get_deployment_token

    # Deploy to production
    echo -e "${BLUE}Deploying to production environment...${NC}"
    swa deploy ./dist \
        --deployment-token "$TOKEN" \
        --app-name "$APP_NAME" \
        --env production

    echo -e "${GREEN}========================================${NC}"
    echo -e "${GREEN}✓ Production deployment complete!${NC}"
    echo -e "${GREEN}========================================${NC}"
    echo -e "${GREEN}Production URL: ${PRODUCTION_URL}${NC}"
    echo ""
}

# Show current deployments
show_status() {
    echo -e "${BLUE}========================================${NC}"
    echo -e "${BLUE}Current Deployment Status${NC}"
    echo -e "${BLUE}========================================${NC}"
    echo -e "App Name: ${GREEN}${APP_NAME}${NC}"
    echo -e "Resource Group: ${GREEN}${RESOURCE_GROUP}${NC}"
    echo ""
    echo -e "Preview URL:    ${GREEN}${PREVIEW_URL}${NC}"
    echo -e "Production URL: ${GREEN}${PRODUCTION_URL}${NC}"
    echo ""

    echo -e "${BLUE}Checking Azure Static Web App details...${NC}"
    az staticwebapp show \
        --name "$APP_NAME" \
        --resource-group "$RESOURCE_GROUP" \
        --query "{name:name,defaultHostname:defaultHostname,location:location}" \
        -o table
    echo ""
}

# Main menu
show_menu() {
    echo -e "${BLUE}========================================${NC}"
    echo -e "${BLUE}Azure Static Web App Deployment${NC}"
    echo -e "${BLUE}========================================${NC}"
    echo ""
    echo "1) Deploy to Preview"
    echo "2) Deploy to Production"
    echo "3) Show Status"
    echo "4) Exit"
    echo ""
    read -p "Select an option (1-4): " choice

    case $choice in
        1)
            deploy_preview
            ;;
        2)
            deploy_production
            ;;
        3)
            show_status
            ;;
        4)
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
        production|prod|--production|-P)
            deploy_production
            ;;
        status|--status|-s)
            show_status
            ;;
        *)
            echo -e "${RED}Unknown option: $1${NC}"
            echo ""
            echo "Usage: $0 [preview|production|status]"
            echo ""
            echo "Options:"
            echo "  preview, -p      Deploy to preview environment"
            echo "  production, -P   Deploy to production environment"
            echo "  status, -s       Show deployment status"
            echo "  (no args)        Show interactive menu"
            exit 1
            ;;
    esac
fi
