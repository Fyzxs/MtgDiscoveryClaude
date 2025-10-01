using System.Collections.Generic;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.Artists;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface IArtistSearchResultCollectionOufToOutMapper : ICreateMapper<IArtistSearchResultCollectionOufEntity, List<ArtistSearchResultOutEntity>>;
