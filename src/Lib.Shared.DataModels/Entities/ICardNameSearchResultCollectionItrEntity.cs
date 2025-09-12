using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities;

public interface ICardNameSearchResultCollectionItrEntity
{
    ICollection<ICardNameSearchResultItrEntity> Names { get; }
}
