using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface ICardIdsArgEntity : IUserIdArgEntity
{
    ICollection<string> CardIds { get; }
}
