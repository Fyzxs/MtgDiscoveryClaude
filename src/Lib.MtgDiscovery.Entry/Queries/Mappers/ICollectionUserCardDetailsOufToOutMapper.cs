using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface ICollectionUserCardDetailsOufToOutMapper : ICreateMapper<IEnumerable<IUserCardDetailsOufEntity>, ICollection<CollectedItemOutEntity>>;
