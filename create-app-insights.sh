#!/bin/bash

# Script to create Application Insights resources

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo -e "${BLUE}========================================${NC}"
echo -e "${BLUE}Creating Application Insights Resources${NC}"
echo -e "${BLUE}========================================${NC}"
echo ""

# Configuration
APP_INSIGHTS_TYPE="web"
LOCATION="westcentralus"

# Create for DEV environment
DEV_RG="rg-mtg-dev-wcus-01"
DEV_APPI_NAME="appi-mtg-dev-wcus-01"
DEV_LOG_WORKSPACE="log-mtg-dev-wcus-01"

echo -e "${YELLOW}Creating Application Insights for DEV...${NC}"

# Get Log Analytics workspace ID
DEV_WORKSPACE_ID=$(az monitor log-analytics workspace show \
    --resource-group "$DEV_RG" \
    --workspace-name "$DEV_LOG_WORKSPACE" \
    --query id -o tsv)

if [ -z "$DEV_WORKSPACE_ID" ]; then
    echo -e "${RED}Failed to get Log Analytics workspace ID for DEV${NC}"
    exit 1
fi

# Create Application Insights
az monitor app-insights component create \
    --app "$DEV_APPI_NAME" \
    --location "$LOCATION" \
    --resource-group "$DEV_RG" \
    --application-type "$APP_INSIGHTS_TYPE" \
    --workspace "$DEV_WORKSPACE_ID" \
    --query "{name:name,instrumentationKey:instrumentationKey,connectionString:connectionString}" \
    -o table

echo -e "${GREEN}✓ DEV Application Insights created${NC}"
echo ""

# Create for PROD environment
PROD_RG="rg-mtg-prod-wcus-01"
PROD_APPI_NAME="appi-mtg-prod-wcus-01"
PROD_LOG_WORKSPACE="log-mtg-prod-wcus-01"

echo -e "${YELLOW}Creating Application Insights for PROD...${NC}"

# Get Log Analytics workspace ID
PROD_WORKSPACE_ID=$(az monitor log-analytics workspace show \
    --resource-group "$PROD_RG" \
    --workspace-name "$PROD_LOG_WORKSPACE" \
    --query id -o tsv 2>/dev/null)

if [ -z "$PROD_WORKSPACE_ID" ]; then
    echo -e "${YELLOW}Warning: Log Analytics workspace not found for PROD - checking if it exists...${NC}"

    # Check if resource group exists
    az group show --name "$PROD_RG" > /dev/null 2>&1
    if [ $? -ne 0 ]; then
        echo -e "${YELLOW}PROD resource group doesn't exist - skipping PROD Application Insights${NC}"
    else
        # Check if Log Analytics exists
        az monitor log-analytics workspace show \
            --resource-group "$PROD_RG" \
            --workspace-name "$PROD_LOG_WORKSPACE" \
            --query id -o tsv 2>/dev/null

        if [ $? -ne 0 ]; then
            echo -e "${YELLOW}Log Analytics workspace missing in PROD - creating it first...${NC}"

            az monitor log-analytics workspace create \
                --resource-group "$PROD_RG" \
                --workspace-name "$PROD_LOG_WORKSPACE" \
                --location "$LOCATION" \
                --query id -o tsv

            PROD_WORKSPACE_ID=$(az monitor log-analytics workspace show \
                --resource-group "$PROD_RG" \
                --workspace-name "$PROD_LOG_WORKSPACE" \
                --query id -o tsv)
        fi
    fi
fi

if [ -n "$PROD_WORKSPACE_ID" ]; then
    # Create Application Insights
    az monitor app-insights component create \
        --app "$PROD_APPI_NAME" \
        --location "$LOCATION" \
        --resource-group "$PROD_RG" \
        --application-type "$APP_INSIGHTS_TYPE" \
        --workspace "$PROD_WORKSPACE_ID" \
        --query "{name:name,instrumentationKey:instrumentationKey,connectionString:connectionString}" \
        -o table

    echo -e "${GREEN}✓ PROD Application Insights created${NC}"
else
    echo -e "${YELLOW}Skipped PROD Application Insights creation${NC}"
fi

echo ""
echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}✓ Application Insights Setup Complete${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""
echo -e "${BLUE}Verify resources:${NC}"
echo -e "az monitor app-insights component show --app $DEV_APPI_NAME --resource-group $DEV_RG"
if [ -n "$PROD_WORKSPACE_ID" ]; then
    echo -e "az monitor app-insights component show --app $PROD_APPI_NAME --resource-group $PROD_RG"
fi
