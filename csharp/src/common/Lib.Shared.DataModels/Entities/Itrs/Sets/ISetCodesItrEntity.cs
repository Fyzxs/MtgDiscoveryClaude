using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs.Sets;

public interface ISetCodesItrEntity
{
    ICollection<string> SetCodes { get; }
}
