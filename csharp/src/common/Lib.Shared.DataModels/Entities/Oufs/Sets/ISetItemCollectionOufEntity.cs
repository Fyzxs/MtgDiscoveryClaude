using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Oufs.Sets;

public interface ISetItemCollectionOufEntity
{
    ICollection<ISetItemOufEntity> Data { get; }
}
