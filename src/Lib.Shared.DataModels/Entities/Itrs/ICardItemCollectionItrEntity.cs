using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs;

public interface ICardItemCollectionOufEntity
{
    ICollection<ICardItemItrEntity> Data { get; }
}
