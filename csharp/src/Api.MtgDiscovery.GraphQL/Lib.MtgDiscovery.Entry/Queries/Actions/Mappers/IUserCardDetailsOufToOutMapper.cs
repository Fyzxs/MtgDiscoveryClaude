using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface IUserCardDetailsOufToOutMapper : ICreateMapper<IUserCardDetailsOufEntity, CollectedItemOutEntity>;
