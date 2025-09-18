using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs;

public interface IArtistSearchResultCollectionOufEntity
{
    ICollection<IArtistSearchResultItrEntity> Artists { get; }
}
