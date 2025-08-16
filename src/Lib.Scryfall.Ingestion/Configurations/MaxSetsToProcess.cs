using Lib.Universal.Primitives;

namespace Lib.Scryfall.Ingestion.Configurations;

public abstract class MaxSetsToProcess : ToSystemType<int>
{
    public abstract bool IsUnlimited();
}
