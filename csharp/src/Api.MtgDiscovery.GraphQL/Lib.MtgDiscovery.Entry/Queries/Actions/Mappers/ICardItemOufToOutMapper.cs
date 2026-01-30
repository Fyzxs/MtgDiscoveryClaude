using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface ICardItemOufToOutMapper : ICreateMapper<ICardItemItrEntity, CardItemOutEntity>;
