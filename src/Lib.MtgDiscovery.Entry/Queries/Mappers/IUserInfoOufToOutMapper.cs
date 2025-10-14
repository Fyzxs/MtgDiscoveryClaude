using Lib.MtgDiscovery.Entry.Entities.Outs.User;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface IUserInfoOufToOutMapper : ICreateMapper<IUserInfoOufEntity, UserRegistrationOutEntity>
{
}
