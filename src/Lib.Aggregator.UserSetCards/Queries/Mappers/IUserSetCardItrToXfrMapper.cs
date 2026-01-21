using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal interface IUserSetCardItrToXfrMapper : ICreateMapper<IUserSetCardItrEntity, IUserSetCardGetXfrEntity>;
