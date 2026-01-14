# Task: Custom Images Fallback for Sealed Product Scraper

## Summary

Add custom-images folder support to `Cli.Sealed.ImageScraper` so that when online image sources fail, the scraper checks a local `custom-images/{setCode}/{uuid}.jpg` folder before falling back to `COMING_SOON.jpg`.

## Problem

Not all sealed products have images available from online providers (TcgPlayer, CardMarket, CardTrader). Currently, these products get the `COMING_SOON.jpg` placeholder. We need a way to manually provide images for these products.

## Solution

Add a custom-images folder check as a fallback step before using the placeholder image.

### Current Flow

```
Online Sources (TcgPlayer → CardMarket → CardTrader)
    ↓ (all fail)
Copy COMING_SOON.jpg placeholder
```

### Target Flow

```
Online Sources (TcgPlayer → CardMarket → CardTrader)
    ↓ (all fail)
Check custom-images/{setCode}/{uuid}.jpg
    ↓ (exists?)
    ├── YES → Copy custom image to sealed-images/{setCode}/{uuid}.jpg
    └── NO  → Copy COMING_SOON.jpg placeholder
```

## Folder Structure

The `custom-images/` folder lives at the **same level** as `sealed-images/` (sibling folder).

**Important**: Both directories are resolved relative to the **current working directory** when the CLI is executed (matching the existing `OutputDirectory = "sealed-images"` pattern at line 19).

```
{current-working-directory}/
├── sealed-images/        # Scraped images output (existing)
│   └── {setCode}/
│       └── {uuid}.jpg
└── custom-images/        # Manually-added custom images (new)
    └── {setCode}/
        └── {uuid}.jpg
```

## File to Modify

**`src/Cli.Sealed.ImageScraper/Orchestration/ImageScraperOrchestrator.cs`**

## Implementation Details

### Step 1: Add Custom Images Directory Constant

At line ~22 (after `PlaceholderImageFileName`), add a new constant:

```csharp
private const string CustomImagesDirectory = "custom-images";
```

### Step 2: Create TryCopyCustomImage Method

Add a new private method after `CopyPlaceholderImage` (around line 270).

**Critical**: This method MUST include exception handling because it's a mid-chain fallback. If copying fails, it should return `false` to allow the placeholder fallback, not crash the scraper.

```csharp
private static bool TryCopyCustomImage(string setCode, string uuid, string outputPath)
{
    try
    {
#pragma warning disable CA1308 // SetCode must be lowercase to match URL pattern convention
        string customImagePath = Path.Combine(
            CustomImagesDirectory,
            setCode.ToLowerInvariant(),
            $"{uuid}.jpg");
#pragma warning restore CA1308

        if (File.Exists(customImagePath) is false)
        {
            return false;
        }

        string? directory = Path.GetDirectoryName(outputPath);
        if (string.IsNullOrEmpty(directory) is false && Directory.Exists(directory) is false)
        {
            _ = Directory.CreateDirectory(directory);
        }

        File.Copy(customImagePath, outputPath, overwrite: true);
        return true;
    }
    catch (IOException)
    {
        // File system error (disk full, file locked, corruption)
        return false;
    }
    catch (UnauthorizedAccessException)
    {
        // Permission denied
        return false;
    }
    catch (NotSupportedException)
    {
        // Invalid path format
        return false;
    }
}
```

### Step 3: Update ProcessSetAsync Method

Modify the fallback logic in `ProcessSetAsync` (lines 137-142). Change from:

```csharp
if (downloaded is false)
{
    _skippedLogger.LogSkippedNoImage(product);
    CopyPlaceholderImage(outputPath);
    _dashboard.IncrementNoImage();
}
```

To:

```csharp
if (downloaded is false)
{
    if (TryCopyCustomImage(setCode, product.Uuid, outputPath))
    {
        _dashboard.IncrementDownloaded();
        _dashboard.AddLog($"Copied: {product.Name} (custom image)");
    }
    else
    {
        _skippedLogger.LogSkippedNoImage(product);
        CopyPlaceholderImage(outputPath);
        _dashboard.IncrementNoImage();
    }
}
```

**Note**: The log message uses verb-first format (`"Copied:"`) to match existing patterns like `"Generated:"` (line 161) and `"Downloaded:"` (line 248).

## Testing

### Basic Test

1. Create a `custom-images/` folder at the same level as `sealed-images/`
2. Add a test image: `custom-images/{setcode}/{uuid}.jpg` (use lowercase setCode)
3. Delete the corresponding file from `sealed-images/` if it exists
4. Run the scraper for that set
5. Verify:
   - The custom image is copied to `sealed-images/{setcode}/{uuid}.jpg`
   - Dashboard shows "Copied: {product name} (custom image)" in the log
   - The downloaded counter increments (not the no-image counter)

### Edge Case Tests

| Scenario | Expected Behavior |
|----------|-------------------|
| Custom image exists and is valid | Copied to sealed-images, logged as "Copied" |
| Custom image doesn't exist | Falls back to COMING_SOON.jpg |
| Custom image exists but is corrupted (0 bytes) | Copies anyway (no validation) |
| Custom image path has permission denied | Returns false, falls back to placeholder |
| Custom-images folder doesn't exist | Returns false, falls back to placeholder |
| SetCode case mismatch (SET vs set) | Normalized to lowercase, should find file |
| File deleted between exists check and copy | Exception caught, falls back to placeholder |

## Notes

- SetCode must be **lowercase** in the custom-images folder (matches existing convention)
- Image format should be `.jpg` to match existing pattern
- Custom images take precedence over the placeholder but NOT over successfully downloaded images
- No changes needed to frontend - it displays whatever `imageUrl` is provided from the API
- The `#pragma` directive is required for `ToLowerInvariant()` to match the existing pattern (lines 124-126)

## Architecture Review Notes

This implementation was reviewed for:
- ✅ **Pattern Consistency**: Follows existing static method pattern (`CopyPlaceholderImage`)
- ✅ **Error Handling**: Try-catch prevents mid-chain failures from crashing scraper
- ✅ **Path Resolution**: Uses same CWD-relative pattern as `OutputDirectory`
- ✅ **Logging Format**: Verb-first pattern matches existing log messages
- ✅ **MicroObjects Compliance**: Acceptable for CLI tool (not core domain layer)

### Future Considerations (Not Required)

- **Configuration**: Could make `CustomImagesDirectory` configurable via appsettings.json
- **Dedicated Counter**: Could add `IncrementCustomImage()` to dashboard for more accurate metrics
- **File Validation**: Could validate files are actually valid JPEGs before copying
