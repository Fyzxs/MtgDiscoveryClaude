using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface ISetCodesArgToItrMapper : ICreateMapper<ISetCodesArgEntity, ISetCodesItrEntity>;
