using System.Linq;
using Lib.MtgDiscovery.Entry.Apis;

namespace Lib.MtgDiscovery.Entry.Queries.Validators;

internal sealed class CardsExtEntityValidator
{
    public bool IsValid(ICardIdsArgEntity arg)
    {
        if (arg == null) return false;
        if (arg.CardIds == null) return false;
        if (arg.CardIds.Count == 0) return false;
        return arg.CardIds.All(id => !string.IsNullOrWhiteSpace(id));
    }
}
