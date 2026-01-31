namespace Lib.Shared.DataModels.Entities.Itrs.Collections;

public interface IGrantCollectionAccessItrEntity
{
    string CollectionId { get; }
    string GrantorUserId { get; }
    string TargetUserId { get; }
    string Role { get; }
}
