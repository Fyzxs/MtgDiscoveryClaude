using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

internal interface IAddUserCardItrToXfrMapper : ICreateMapper<IUserCardItrEntity, IAddUserCardXfrEntity>;
