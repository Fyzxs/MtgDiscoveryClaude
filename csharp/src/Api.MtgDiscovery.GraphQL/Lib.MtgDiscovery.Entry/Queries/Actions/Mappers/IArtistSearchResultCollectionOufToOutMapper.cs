using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.Artists;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Artists;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface IArtistSearchResultCollectionOufToOutMapper : ICreateMapper<IArtistSearchResultCollectionOufEntity, List<ArtistSearchResultOutEntity>>;
