using System.Collections.Generic;
using Lib.Universal.Primitives;

namespace Lib.Scryfall.Ingestion.Configurations;

public abstract class SpecificSetCodes : ToSystemType<ISet<string>>
{
    public abstract bool HasSpecificSets();
    public abstract bool HasNoSpecificSets();
}
