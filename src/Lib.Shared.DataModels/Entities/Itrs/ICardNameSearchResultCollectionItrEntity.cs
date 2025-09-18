using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs;

public interface ICardNameSearchResultCollectionOufEntity
{
    ICollection<ICardNameSearchResultItrEntity> Names { get; }
}
