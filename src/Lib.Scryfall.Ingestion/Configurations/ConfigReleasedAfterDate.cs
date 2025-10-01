using System;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.Scryfall.Ingestion.Configurations;

internal sealed class ConfigReleasedAfterDate : ReleasedAfterDate
{
    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigReleasedAfterDate(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override DateTime? AsSystemType()
    {
        string value = _config[_sourceKey];
        if (value.IzNullOrWhiteSpace())
        {
            return null;
        }

        if (DateTime.TryParse(value, out DateTime parsedDate))
        {
            return parsedDate;
        }

        return null;
    }

    public override bool HasDate() => AsSystemType().HasValue;
    public override bool HasNoDate() => HasDate() is false;
}
