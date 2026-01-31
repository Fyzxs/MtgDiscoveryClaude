using Lib.Adapter.Cards.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal interface ICardNameItrToXfrMapper : ICreateMapper<ICardNameItrEntity, ICardNameXfrEntity>;
