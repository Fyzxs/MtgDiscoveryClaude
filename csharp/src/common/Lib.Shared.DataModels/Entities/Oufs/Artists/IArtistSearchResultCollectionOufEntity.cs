using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Oufs.Artists;

public interface IArtistSearchResultCollectionOufEntity
{
    ICollection<IArtistSearchResultOufEntity> Artists { get; }
}
