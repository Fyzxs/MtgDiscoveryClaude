using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs;

public interface ISetItemCollectionOufEntity
{
    ICollection<ISetItemItrEntity> Data { get; }
}
