using System.Collections.Generic;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.Cards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface ICollectionCardItemToByIdsItrMapper : ICreateMapper<List<CardItemOutEntity>, IUserIdArgEntity, IUserCardsByIdsItrEntity>;
