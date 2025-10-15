using System;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Filters;

internal sealed class PreviewSetFilter : IScryfallSetFilter
{
    private readonly ILogger _logger;

    public PreviewSetFilter(ILogger logger) => _logger = logger;

    public bool ShouldInclude(IScryfallSet set)
    {
        dynamic data = set.Data();
        string releasedAt = data.released_at;

        if (string.IsNullOrWhiteSpace(releasedAt))
        {
            _logger.LogPreviewSetNoReleaseDate(set.Code());
            return true;
        }

        if (DateTime.TryParse(releasedAt, out DateTime releaseDate) is false)
        {
            _logger.LogPreviewSetInvalidReleaseDate(set.Code(), releasedAt);
            return true;
        }

        bool isPreview = releaseDate > DateTime.UtcNow;

        if (isPreview)
        {
            _logger.LogPreviewSetExcluded(set.Code(), set.Name(), releasedAt);
        }

        return isPreview is false;
    }
}

internal static partial class PreviewSetFilterLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Set {SetCode} has no release date, including by default")]
    public static partial void LogPreviewSetNoReleaseDate(this ILogger logger, string setCode);

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Set {SetCode} has invalid release date '{ReleaseDate}', including by default")]
    public static partial void LogPreviewSetInvalidReleaseDate(this ILogger logger, string setCode, string releaseDate);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Excluding preview set {SetCode} - {SetName} (releases {ReleaseDate})")]
    public static partial void LogPreviewSetExcluded(this ILogger logger, string setCode, string setName, string releaseDate);
}
