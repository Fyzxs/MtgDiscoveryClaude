namespace Lib.Shared.DataModels.Entities.Itrs.Collections;

public interface IRenameCollectionItrEntity
{
    string CollectionId { get; }
    string OwnerId { get; }
    string Name { get; }
}
