using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface IUserSetCardOufToOutMapper : ICreateMapper<IUserSetCardOufEntity, UserSetCardOutEntity>;
