using System.Collections.Generic;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface ISetIdsArgEntity
{
    ICollection<string> SetIds { get; }
}
