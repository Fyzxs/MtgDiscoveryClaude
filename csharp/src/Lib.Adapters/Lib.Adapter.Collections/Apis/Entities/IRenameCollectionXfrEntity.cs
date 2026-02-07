using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.Collections.Apis.Entities;

public interface IRenameCollectionXfrEntity : IXfrEntity
{
    string CollectionId { get; }
    string OwnerId { get; }
    string Name { get; }
}
