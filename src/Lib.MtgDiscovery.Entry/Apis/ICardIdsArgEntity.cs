using System.Collections.Generic;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface ICardIdsArgEntity
{
    ICollection<string> CardIds { get; }
}
