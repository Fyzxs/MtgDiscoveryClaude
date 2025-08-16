using Lib.Universal.Primitives;

namespace Lib.Scryfall.Ingestion.Configurations;

internal abstract class MaxSetsToProcess : ToSystemType<int>
{
    public abstract bool IsUnlimited();
}
