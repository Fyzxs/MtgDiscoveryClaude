using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface IUserSetCardOufToOutMapper : ICreateMapper<IUserSetCardOufEntity, UserSetCardOutEntity>;
