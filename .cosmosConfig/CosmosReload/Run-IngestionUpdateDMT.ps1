# Run DMT for Cards-related configurations only
# This script runs the DMT tool for CardItems, SetCards, and ArtistCards

$dmtPath = "$PSScriptRoot\.dmt\dmt.exe"

$cardsFiles = @(
    "dmt-ArtistCards.json",
    "dmt-ArtistItems.json",
    "dmt-ArtistNameTrigrams.json",
    "dmt-ArtistSets.json",
    "dmt-CardItems.json",
    "dmt-CardNameTrigrams.json",
    "dmt-CardsByName.json",
    "dmt-SealedProductItems.json",
    "dmt-SetArtists.json",
    "dmt-SetCards.json",
    "dmt-SetCodeToIdAssociations.json",
    "dmt-SetItems.json",
    "dmt-SetParentAssociations.json"
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Running DMT for CARDS configurations" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$successCount = 0
$failCount = 0
$totalCount = $cardsFiles.Count

foreach ($fileName in $cardsFiles) {
    $jsonFile = Join-Path -Path $PSScriptRoot -ChildPath $fileName

    if (-not (Test-Path $jsonFile)) {
        Write-Host "WARNING: File not found: $fileName" -ForegroundColor Yellow
        $failCount++
        continue
    }

    Write-Host "Processing: $fileName" -ForegroundColor Yellow
    Write-Host "-----------------------------------" -ForegroundColor Gray

    $startTime = Get-Date

    try {
        & $dmtPath --settings $jsonFile

        if ($LASTEXITCODE -eq 0) {
            $successCount++
            Write-Host "SUCCESS: $fileName" -ForegroundColor Green
        }
        else {
            $failCount++
            Write-Host "FAILED: $fileName (Exit code: $LASTEXITCODE)" -ForegroundColor Red
        }
    }
    catch {
        $failCount++
        Write-Host "ERROR: $fileName - $_" -ForegroundColor Red
    }

    $elapsed = (Get-Date) - $startTime
    Write-Host "Time: $($elapsed.ToString('mm\:ss'))" -ForegroundColor Gray
    Write-Host ""
}

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Summary" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Total:   $totalCount" -ForegroundColor White
Write-Host "Success: $successCount" -ForegroundColor Green
Write-Host "Failed:  $failCount" -ForegroundColor Red
Write-Host ""

if ($failCount -gt 0) {
    Write-Host "Some operations failed!" -ForegroundColor Red
    exit 1
}
else {
    Write-Host "All operations completed successfully!" -ForegroundColor Green
    exit 0
}
