using Lib.Adapter.Cards.Apis.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal interface ICardSearchTermItrToXfrMapper : ICreateMapper<ICardSearchTermItrEntity, ICardSearchTermXfrEntity>;