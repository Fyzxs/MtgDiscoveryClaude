using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.Collections.Apis.Entities;

public interface ICollectionIdXfrEntity : IXfrEntity
{
    string CollectionId { get; }
    string OwnerId { get; }
}
