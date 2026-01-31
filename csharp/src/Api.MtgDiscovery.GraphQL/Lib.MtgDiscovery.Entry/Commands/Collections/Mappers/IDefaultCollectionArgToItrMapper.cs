using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Mappers;

internal interface IDefaultCollectionArgToItrMapper : ICreateMapper<IUserIdArgEntity, ICollectionItrEntity>
{
}
