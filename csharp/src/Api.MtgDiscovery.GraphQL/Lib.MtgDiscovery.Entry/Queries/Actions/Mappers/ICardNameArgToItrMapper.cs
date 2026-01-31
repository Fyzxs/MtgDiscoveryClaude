using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.Cards;
using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface ICardNameArgToItrMapper : ICreateMapper<ICardNameArgEntity, ICardNameItrEntity>;
