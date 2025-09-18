using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs;

public interface IArtistSearchResultCollectionItrEntity
{
    ICollection<IArtistSearchResultItrEntity> Artists { get; }
}
