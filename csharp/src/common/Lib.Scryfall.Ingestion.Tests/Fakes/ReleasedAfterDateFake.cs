using System;
using Lib.Scryfall.Ingestion.Configurations;

namespace Lib.Scryfall.Ingestion.Tests.Fakes;

internal sealed class ReleasedAfterDateFake : ReleasedAfterDate
{
    private readonly DateTime? _date;

    public ReleasedAfterDateFake(DateTime? date = null)
    {
        _date = date;
    }

    public override DateTime? AsSystemType() => _date;
    public override bool HasDate() => _date.HasValue;
    public override bool HasNoDate() => _date.HasValue is false;
}
