namespace Lib.Shared.DataModels.Entities.Itrs.Collections;

public interface ITransferCollectionOwnershipItrEntity
{
    string CollectionId { get; }
    string CurrentOwnerId { get; }
    string TargetUserId { get; }
}
