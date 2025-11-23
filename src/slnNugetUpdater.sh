#!/bin/bash

# Run the script from the directory with the *.sln in it, or pass in the path
# Usage: ./slnNugetUpdater.sh [path-to-sln]

SLN_PATH="${1:-.}"

echo "Getting Package Info for $SLN_PATH"

# Get the JSON output from dotnet list command
RAW_JSON=$(dotnet list "$SLN_PATH" package --highest-minor --outdated --format json)

# Check if jq is available
if ! command -v jq &> /dev/null; then
    echo "Error: jq is not installed. Please install jq to use this script."
    echo "On Ubuntu/Debian: sudo apt-get install jq"
    echo "On Fedora: sudo dnf install jq"
    echo "On Arch: sudo pacman -S jq"
    exit 1
fi

# Parse JSON and iterate through projects
echo "$RAW_JSON" | jq -r '.projects[] | @json' | while IFS= read -r project; do
    PROJECT_PATH=$(echo "$project" | jq -r '.path')
    echo "Processing $PROJECT_PATH"

    # Check if frameworks exist
    FRAMEWORKS=$(echo "$project" | jq -r '.frameworks // empty')
    if [ -n "$FRAMEWORKS" ]; then
        echo "Processing FRAMEWORKS"
    fi

    # Iterate through frameworks and packages
    echo "$project" | jq -c '.frameworks[]? | {framework: .framework, packages: .topLevelPackages}' | while IFS= read -r framework_data; do
        FRAMEWORK=$(echo "$framework_data" | jq -r '.framework')

        echo "$framework_data" | jq -c '.packages[]?' | while IFS= read -r package; do
            PACKAGE_ID=$(echo "$package" | jq -r '.id')
            REQUESTED_VERSION=$(echo "$package" | jq -r '.requestedVersion')
            LATEST_VERSION=$(echo "$package" | jq -r '.latestVersion')

            echo "$PROJECT_PATH $FRAMEWORK update for $PACKAGE_ID from $REQUESTED_VERSION to $LATEST_VERSION"
            dotnet add "$PROJECT_PATH" package "$PACKAGE_ID" --framework "$FRAMEWORK" --version "$LATEST_VERSION"
        done
    done

    echo "Finished processing $PROJECT_PATH"
done

echo "Finished processing all"
