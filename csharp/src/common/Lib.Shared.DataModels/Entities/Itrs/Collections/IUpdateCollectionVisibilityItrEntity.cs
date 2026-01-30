namespace Lib.Shared.DataModels.Entities.Itrs.Collections;

public interface IUpdateCollectionVisibilityItrEntity
{
    string CollectionId { get; }
    string OwnerId { get; }
    string Visibility { get; }
}
