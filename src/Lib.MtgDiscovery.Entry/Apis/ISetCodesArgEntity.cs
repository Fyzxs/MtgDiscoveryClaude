using System.Collections.Generic;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface ISetCodesArgEntity
{
    ICollection<string> SetCodes { get; }
}
