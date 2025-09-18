using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs;

public interface IArtistItemCollectionItrEntity
{
    ICollection<IArtistItemItrEntity> Data { get; }
}
