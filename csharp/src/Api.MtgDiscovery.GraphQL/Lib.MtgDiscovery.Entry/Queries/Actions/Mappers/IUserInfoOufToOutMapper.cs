using Lib.MtgDiscovery.Entry.Entities.Outs.User;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.User;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface IUserInfoOufToOutMapper : ICreateMapper<IUserInfoOufEntity, UserRegistrationOutEntity>
{
}
