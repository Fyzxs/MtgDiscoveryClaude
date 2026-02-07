using System.Collections.Generic;
using Lib.Scryfall.Ingestion.Configurations;

namespace Lib.Scryfall.Ingestion.Tests.Fakes;

internal sealed class SpecificSetCodesFake : SpecificSetCodes
{
    private readonly ISet<string> _codes;

    public SpecificSetCodesFake(params string[] codes)
    {
        _codes = new HashSet<string>(codes);
    }

    public override ISet<string> AsSystemType() => _codes;
    public override bool HasSpecificSets() => _codes.Count > 0;
    public override bool HasNoSpecificSets() => _codes.Count == 0;
}
