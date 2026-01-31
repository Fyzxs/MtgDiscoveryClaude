using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.Shared.DataModels.Entities.Oufs.Cards;

public interface ICardItemCollectionOufEntity
{
    ICollection<ICardItemItrEntity> Data { get; }
}
