using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Abstractions.Mappers;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface IUserSetCardOufToOutMapper : ICreateMapper<IUserSetCardOufEntity, UserSetCardOutEntity>;
