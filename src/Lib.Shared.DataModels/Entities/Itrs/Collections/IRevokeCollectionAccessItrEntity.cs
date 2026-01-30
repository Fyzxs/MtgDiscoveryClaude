namespace Lib.Shared.DataModels.Entities.Itrs.Collections;

public interface IRevokeCollectionAccessItrEntity
{
    string CollectionId { get; }
    string RevokerUserId { get; }
    string TargetUserId { get; }
}
