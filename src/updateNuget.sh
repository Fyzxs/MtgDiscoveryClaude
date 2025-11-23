#!/bin/bash

# Simple NuGet package updater
# Updates all packages to their latest versions
# Usage: ./updateNuget.sh [path-to-sln]

SLN_PATH="${1:-.}"

echo "Updating NuGet packages for $SLN_PATH"

# Get all projects in the solution
dotnet sln "$SLN_PATH" list | tail -n +3 | while IFS= read -r project; do
    if [ -n "$project" ]; then
        echo "Updating packages in: $project"

        # List all packages and update each one
        dotnet list "$project" package | grep ">" | awk '{print $2}' | while IFS= read -r package; do
            if [ -n "$package" ]; then
                echo "  Updating $package"
                dotnet add "$project" package "$package" --no-restore
            fi
        done
    fi
done

echo "Finished updating all packages"
