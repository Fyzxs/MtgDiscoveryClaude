using System;
using Lib.Universal.Primitives;

namespace Lib.Scryfall.Ingestion.Configurations;

internal abstract class ReleasedAfterDate : ToSystemType<DateTime?>
{
    public abstract bool HasDate();
    public abstract bool HasNoDate();
}