using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities;

public interface ICardItemCollectionItrEntity
{
    ICollection<ICardItemItrEntity> Data { get; }
}
