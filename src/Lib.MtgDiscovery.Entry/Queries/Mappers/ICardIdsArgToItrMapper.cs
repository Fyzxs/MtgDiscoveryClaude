using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface ICardIdsArgToItrMapper : ICreateMapper<ICardIdsArgEntity, ICardIdsItrEntity>;
