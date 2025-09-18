using System.Collections.Generic;
using App.MtgDiscovery.GraphQL.Entities.Outs.Artists;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal interface IArtistSearchResultCollectionOufToOutMapper : ICreateMapper<IEnumerable<IArtistSearchResultItrEntity>, List<ArtistSearchResultOutEntity>>;
