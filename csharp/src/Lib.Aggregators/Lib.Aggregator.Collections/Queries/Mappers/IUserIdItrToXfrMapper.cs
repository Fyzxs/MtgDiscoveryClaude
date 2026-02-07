using Lib.Adapter.Collections.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.User;

namespace Lib.Aggregator.Collections.Queries.Mappers;

internal interface IUserIdItrToXfrMapper : ICreateMapper<IUserIdItrEntity, IUserIdXfrEntity>;
