using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs;

public interface ISetItemCollectionItrEntity
{
    ICollection<ISetItemItrEntity> Data { get; }
}
