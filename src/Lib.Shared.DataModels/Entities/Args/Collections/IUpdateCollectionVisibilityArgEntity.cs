using Lib.Shared.DataModels.Entities.Models;

namespace Lib.Shared.DataModels.Entities.Args.Collections;

public interface IUpdateCollectionVisibilityArgEntity : ICollectionIdArgModel
{
    string Visibility { get; }
}
