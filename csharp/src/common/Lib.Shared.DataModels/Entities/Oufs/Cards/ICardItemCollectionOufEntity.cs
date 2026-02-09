using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Oufs.Cards;

public interface ICardItemCollectionOufEntity
{
    ICollection<ICardItemOufEntity> Data { get; }
}
