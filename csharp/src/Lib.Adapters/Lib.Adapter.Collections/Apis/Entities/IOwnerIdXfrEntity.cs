using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.Collections.Apis.Entities;

public interface IOwnerIdXfrEntity : IXfrEntity
{
    string OwnerId { get; }
}
