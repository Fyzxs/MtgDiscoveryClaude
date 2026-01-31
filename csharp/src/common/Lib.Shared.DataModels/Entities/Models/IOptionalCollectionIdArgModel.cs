namespace Lib.Shared.DataModels.Entities.Models;

public interface IOptionalCollectionIdArgModel : ICollectionIdArgModel
{
    bool HasCollectionId => string.IsNullOrEmpty(CollectionId) is false;
}
