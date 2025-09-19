using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs;

public interface ICardNameSearchCollectionOufEntity
{
    ICollection<ICardNameSearchResultItrEntity> Names { get; }
}
