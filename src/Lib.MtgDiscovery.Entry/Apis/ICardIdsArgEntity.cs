using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Args;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface ICardIdsArgEntity : IUserIdArgEntity
{
    ICollection<string> CardIds { get; }
}
