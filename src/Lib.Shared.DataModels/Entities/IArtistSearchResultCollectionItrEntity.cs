using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities;

public interface IArtistSearchResultCollectionItrEntity
{
    ICollection<IArtistSearchResultItrEntity> Artists { get; }
}
