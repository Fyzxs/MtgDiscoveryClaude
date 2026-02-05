using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.Collections.Apis.Entities;

public interface IGrantCollectionAccessXfrEntity : IXfrEntity
{
    string CollectionId { get; }
    string GrantorUserId { get; }
    string TargetUserId { get; }
    string Role { get; }
}
