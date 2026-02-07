using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.Collections.Apis.Entities;

public interface IRevokeCollectionAccessXfrEntity : IXfrEntity
{
    string CollectionId { get; }
    string RevokerUserId { get; }
    string TargetUserId { get; }
}
