using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities;

public interface ISetItemCollectionItrEntity
{
    IReadOnlyCollection<ISetItemItrEntity> Data { get; }
}