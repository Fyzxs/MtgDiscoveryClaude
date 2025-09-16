using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface IAuthUserArgsToItrMapper : ICreateMapper<IAuthUserArgEntity, IUserInfoItrEntity>;
