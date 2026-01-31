using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

internal interface IAddUserCardItrToXfrMapper : ICreateMapper<IUserCardItrEntity, IAddUserCardXfrEntity>;
