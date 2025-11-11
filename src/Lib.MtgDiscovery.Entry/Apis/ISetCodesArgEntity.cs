using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Args;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface ISetCodesArgEntity : IUserIdArgEntity
{
    ICollection<string> SetCodes { get; }
}
