using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Queries.Mappers;

internal interface IUserCardsSetItrToXfrMapper : ICreateMapper<IUserCardsSetItrEntity, IUserCardsSetXfrEntity>;
