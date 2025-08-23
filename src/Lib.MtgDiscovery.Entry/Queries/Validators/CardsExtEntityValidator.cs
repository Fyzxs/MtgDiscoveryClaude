using System.Linq;
using Lib.MtgDiscovery.Entry.Apis;

namespace Lib.MtgDiscovery.Entry.Queries.Validators;

internal sealed class CardsExtEntityValidator
{
    public bool IsValid(ICardIdsArgsEntity args)
    {
        if (args == null) return false;
        if (args.CardIds == null) return false;
        if (args.CardIds.Count == 0) return false;
        return args.CardIds.All(id => !string.IsNullOrWhiteSpace(id));
    }
}
