using Lib.Shared.DataModels.Entities.Models;

namespace Lib.Shared.DataModels.Entities.Args.Collections;

public interface IRevokeCollectionAccessArgEntity : ICollectionIdArgModel
{
    string TargetUserId { get; }
}
