using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.Collections.Apis.Entities;

public interface IDeleteCollectionXfrEntity : IXfrEntity
{
    string CollectionId { get; }
    string OwnerId { get; }
}
