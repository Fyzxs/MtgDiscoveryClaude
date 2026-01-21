namespace Cli.Sealed.ImageScraper.Models;

internal sealed class DownloadResult
{
    public static DownloadResult Success(string filePath) => new()
    {
        IsSuccess = true,
        FilePath = filePath
    };

    public static DownloadResult Skipped(string reason) => new()
    {
        IsSuccess = true,
        WasSkipped = true,
        SkipReason = reason
    };

    public static DownloadResult Failed(string error) => new()
    {
        IsSuccess = false,
        ErrorMessage = error
    };

    public bool IsSuccess { get; private init; }
    public bool WasSkipped { get; private init; }
    public string FilePath { get; private init; }
    public string SkipReason { get; private init; }
    public string ErrorMessage { get; private init; }
}
