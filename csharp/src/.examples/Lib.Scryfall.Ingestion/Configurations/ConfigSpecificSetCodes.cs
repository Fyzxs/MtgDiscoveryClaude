using System;
using System.Collections.Generic;
using System.Linq;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.Scryfall.Ingestion.Configurations;

internal sealed class ConfigSpecificSetCodes : SpecificSetCodes
{
    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigSpecificSetCodes(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override ISet<string> AsSystemType()
    {
        string value = _config[_sourceKey];
        if (value.IzNullOrWhiteSpace()) return new HashSet<string>();

        return new HashSet<string>(value.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim().ToLowerInvariant()));
    }

    public override bool HasSpecificSets() => 0 < AsSystemType().Count;
    public override bool HasNoSpecificSets() => HasSpecificSets() is false;
}
