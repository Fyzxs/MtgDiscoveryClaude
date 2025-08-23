using System.Collections.Generic;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface ICardIdsArgsEntity
{
    ICollection<string> CardIds { get; }
}
