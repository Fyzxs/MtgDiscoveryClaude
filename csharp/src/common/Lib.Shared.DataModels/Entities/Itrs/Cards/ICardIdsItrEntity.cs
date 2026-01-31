using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs.Cards;

public interface ICardIdsItrEntity
{
    ICollection<string> CardIds { get; }
}
