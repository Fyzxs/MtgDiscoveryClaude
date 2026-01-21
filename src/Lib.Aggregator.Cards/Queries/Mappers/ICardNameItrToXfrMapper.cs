using Lib.Adapter.Cards.Apis.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal interface ICardNameItrToXfrMapper : ICreateMapper<ICardNameItrEntity, ICardNameXfrEntity>;