using Lib.Shared.DataModels.Abstractions;

namespace Lib.Shared.DataModels.Entities.Itrs.Collections;

public interface IOwnerIdItrEntity : IItrEntity
{
    string OwnerId { get; }
}
