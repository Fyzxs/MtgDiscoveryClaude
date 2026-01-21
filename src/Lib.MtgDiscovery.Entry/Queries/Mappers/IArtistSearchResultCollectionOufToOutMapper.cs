using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.Artists;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface IArtistSearchResultCollectionOufToOutMapper : ICreateMapper<IArtistSearchResultCollectionOufEntity, List<ArtistSearchResultOutEntity>>;
