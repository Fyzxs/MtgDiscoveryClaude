using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Itrs.User;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface IAuthUserArgToItrMapper : ICreateMapper<IAuthUserArgEntity, IUserInfoItrEntity>;
