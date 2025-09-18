using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs;

public interface ICardNameSearchResultCollectionItrEntity
{
    ICollection<ICardNameSearchResultItrEntity> Names { get; }
}
