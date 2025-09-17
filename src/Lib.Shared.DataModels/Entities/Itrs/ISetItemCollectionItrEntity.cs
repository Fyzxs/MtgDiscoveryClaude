using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities;

public interface ISetItemCollectionItrEntity
{
    ICollection<ISetItemItrEntity> Data { get; }
}
