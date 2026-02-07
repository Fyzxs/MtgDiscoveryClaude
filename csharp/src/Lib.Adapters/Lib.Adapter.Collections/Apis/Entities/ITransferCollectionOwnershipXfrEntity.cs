using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.Collections.Apis.Entities;

public interface ITransferCollectionOwnershipXfrEntity : IXfrEntity
{
    string CollectionId { get; }
    string CurrentOwnerId { get; }
    string TargetUserId { get; }
}
