using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.Sets;

namespace Lib.Shared.DataModels.Entities.Oufs.Sets;

public interface ISetItemCollectionOufEntity
{
    ICollection<ISetItemItrEntity> Data { get; }
}
