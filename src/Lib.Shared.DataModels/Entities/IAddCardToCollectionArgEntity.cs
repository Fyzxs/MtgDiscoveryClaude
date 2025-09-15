using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities;

public interface IAddCardToCollectionArgEntity
{
    string UserId { get; }
    string CardId { get; }
    string SetId { get; }
    ICollection<ICollectedItemArgEntity> CollectedList { get; }
}

public interface ICollectedItemArgEntity
{
    string Finish { get; }
    string Special { get; }
    int Count { get; }
}
