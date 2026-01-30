# Why am I getting a 401?
# Follow instructions here: https://github.com/microsoft/artifacts-credprovider#manual-installation-on-windows
#
# Run the script (wherever it lives) from the directory with the *.sln in it.
# or pass in the path
#

# Sometimes:
# Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass

param([string]$slnPath='.')

Write-Host "Getting Package Info for $($slnPath)"
$rawObj = dotnet list $slnPath package --highest-minor --outdated --format json | ConvertFrom-Json

$projects = $rawObj.projects

# Initialize a hash set to keep track of updated packages
$updatedPackages = @{}

foreach ($project in $projects) 
{
    $path = $project.path
    Write-Host "Processing $($path)"
    if($project.frameworks -ne $null){
        Write-Host "Processing FRAMEWORKS [$($project.frameworks)]"
    }
    foreach ($framework in $project.frameworks) 
    {
        foreach ($package in $framework.topLevelPackages) 
        {
            # Skip if the package has already been updated
            if ($updatedPackages.ContainsKey($package.id)) {
                Write-Host "Skipping $($package.id) as it has already been updated"
                continue
            }

            Write-Host "$($path) $($framework.framework) update for $($package.id) from $($package.requestedVersion) to $($package.latestVersion)"
            dotnet add $project.path package $package.id --framework $framework.framework --version $package.latestVersion
            # Mark the package as updated
            $updatedPackages[$package.id] = $true
        }
    }
    Write-Host "Finished processing $($path)"
}
Write-Host "Finished processing all"