using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface ISetIdsArgToItrMapper : ICreateMapper<ISetIdsArgEntity, ISetIdsItrEntity>;
