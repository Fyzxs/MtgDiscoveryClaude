using Lib.Shared.DataModels.Entities.Models;

namespace Lib.Shared.DataModels.Entities.Args.Collections;

public interface IGrantCollectionAccessArgEntity : ICollectionIdArgModel
{
    string TargetUserId { get; }
    string Role { get; }
}
