using Lib.Adapter.Cards.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Sets;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal interface ISetCodeItrToXfrMapper : ICreateMapper<ISetCodeItrEntity, ISetCodeXfrEntity>;
