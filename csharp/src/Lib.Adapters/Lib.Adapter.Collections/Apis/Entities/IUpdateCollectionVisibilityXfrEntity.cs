using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.Collections.Apis.Entities;

public interface IUpdateCollectionVisibilityXfrEntity : IXfrEntity
{
    string CollectionId { get; }
    string OwnerId { get; }
    string Visibility { get; }
}
