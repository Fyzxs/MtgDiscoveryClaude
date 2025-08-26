using System.Collections.Generic;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Filters;

internal sealed class ForeignSetFilter : IScryfallSetFilter
{
    private static readonly HashSet<string> ForeignSetCodes = new(System.StringComparer.OrdinalIgnoreCase)
    {
        "4BB",   // Fourth Edition Black Bordered (Foreign)
        "BCHR",  // Chronicles Foreign Black Border
        "REN",   // Renaissance
        "RIN",   // Rinascimento (Italian Renaissance)
        "FBB"    // Foreign Black Bordered
    };

    private readonly ILogger _logger;

    public ForeignSetFilter(ILogger logger)
    {
        _logger = logger;
    }

    public bool ShouldInclude(IScryfallSet set)
    {
        string setCode = set.Code();
        bool isForeign = ForeignSetCodes.Contains(setCode);
        if (isForeign)
        {
            _logger.LogForeignSetExcluded(setCode, set.Name());
        }

        return isForeign is false;
    }
}

internal static partial class ForeignSetFilterLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Excluding foreign language set {SetCode} - {SetName}")]
    public static partial void LogForeignSetExcluded(this ILogger logger, string setCode, string setName);
}