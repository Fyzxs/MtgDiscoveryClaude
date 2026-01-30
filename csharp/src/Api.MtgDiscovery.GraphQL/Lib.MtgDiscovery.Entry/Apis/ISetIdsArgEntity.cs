using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface ISetIdsArgEntity : IUserIdArgEntity
{
    ICollection<string> SetIds { get; }
}
