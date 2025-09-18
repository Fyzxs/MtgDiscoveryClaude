using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs;

public interface ICardIdsItrEntity
{
    ICollection<string> CardIds { get; }
}
