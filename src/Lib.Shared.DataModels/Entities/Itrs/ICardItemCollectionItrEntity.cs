using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs;

public interface ICardItemCollectionItrEntity
{
    ICollection<ICardItemItrEntity> Data { get; }
}
