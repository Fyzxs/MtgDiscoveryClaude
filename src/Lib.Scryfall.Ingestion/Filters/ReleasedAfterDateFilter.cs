using System;
using Lib.Scryfall.Ingestion.Configurations;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Filters;

internal sealed class ReleasedAfterDateFilter : IScryfallSetFilter
{
    private const bool Include = true;
    private readonly IScryfallIngestionConfiguration _config;

    public ReleasedAfterDateFilter(IScryfallIngestionConfiguration config)
    {
        _config = config;
    }

    public bool ShouldInclude(IScryfallSet set)
    {
        ReleasedAfterDate releasedAfter = _config.ProcessingConfig().SetsReleasedAfter();
        if (releasedAfter.HasNoDate())
        {
            return Include;
        }

        DateTime? filterDate = releasedAfter.AsSystemType();
        if (filterDate.HasValue is false)
        {
            return Include;
        }

        DateTime setReleaseDate = GetSetReleaseDate(set);
        return setReleaseDate >= filterDate.Value;
    }

    private static DateTime GetSetReleaseDate(IScryfallSet set)
    {
        try
        {
            dynamic data = set.Data();
            string releasedAt = data.released_at;
            if (string.IsNullOrEmpty(releasedAt) is false && DateTime.TryParse(releasedAt, out DateTime parsedDate))
            {
                return parsedDate;
            }
        }
        catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
        {
            return DateTime.MinValue;
        }

        return DateTime.MinValue;
    }
}