using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface ICardItemOufToOutMapper : ICreateMapper<ICardItemItrEntity, CardItemOutEntity>;
