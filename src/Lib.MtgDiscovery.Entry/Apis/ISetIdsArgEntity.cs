using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Args;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface ISetIdsArgEntity : IUserIdArgEntity
{
    ICollection<string> SetIds { get; }
}
