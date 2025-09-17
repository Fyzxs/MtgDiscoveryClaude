using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities;

public interface ICardIdsItrEntity
{
    ICollection<string> CardIds { get; }
}
