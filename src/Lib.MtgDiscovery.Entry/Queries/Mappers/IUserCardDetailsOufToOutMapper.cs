using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface IUserCardDetailsOufToOutMapper : ICreateMapper<IUserCardDetailsOufEntity, CollectedItemOutEntity>;
