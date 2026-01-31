using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.Artists;

namespace Lib.Shared.DataModels.Entities.Oufs.Artists;

public interface IArtistSearchResultCollectionOufEntity
{
    ICollection<IArtistSearchResultItrEntity> Artists { get; }
}
