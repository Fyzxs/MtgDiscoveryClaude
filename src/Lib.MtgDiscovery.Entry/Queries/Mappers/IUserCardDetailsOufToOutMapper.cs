using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface IUserCardDetailsOufToOutMapper : ICreateMapper<IUserCardDetailsOufEntity, CollectedItemOutEntity>;
