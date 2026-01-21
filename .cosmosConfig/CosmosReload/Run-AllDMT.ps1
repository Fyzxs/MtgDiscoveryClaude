# Run all DMT JSON configurations
# This script runs the DMT tool for all JSON configuration files

$dmtPath = "$PSScriptRoot\.dmt\dmt.exe"
$jsonFiles = Get-ChildItem -Path $PSScriptRoot -Filter "dmt-*.json" | Sort-Object Name

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Running DMT for ALL configurations" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$successCount = 0
$failCount = 0
$totalCount = $jsonFiles.Count

foreach ($jsonFile in $jsonFiles) {
    Write-Host "Processing: $($jsonFile.Name)" -ForegroundColor Yellow
    Write-Host "-----------------------------------" -ForegroundColor Gray

    $startTime = Get-Date

    try {
        & $dmtPath --settings $jsonFile.FullName

        if ($LASTEXITCODE -eq 0) {
            $successCount++
            Write-Host "SUCCESS: $($jsonFile.Name)" -ForegroundColor Green
        } else {
            $failCount++
            Write-Host "FAILED: $($jsonFile.Name) (Exit code: $LASTEXITCODE)" -ForegroundColor Red
        }
    }
    catch {
        $failCount++
        Write-Host "ERROR: $($jsonFile.Name) - $_" -ForegroundColor Red
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
} else {
    Write-Host "All operations completed successfully!" -ForegroundColor Green
    exit 0
}
