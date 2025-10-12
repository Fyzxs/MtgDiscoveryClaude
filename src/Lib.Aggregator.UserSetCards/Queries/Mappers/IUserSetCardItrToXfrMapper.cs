using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Aggregator.UserSetCards.Entities;
using Lib.Shared.Abstractions.Mappers;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal interface IUserSetCardItrToXfrMapper : ICreateMapper<IUserSetCardItrEntity, IUserSetCardGetXfrEntity>;
