using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs;

public interface ISetCodesItrEntity
{
    ICollection<string> SetCodes { get; }
}
