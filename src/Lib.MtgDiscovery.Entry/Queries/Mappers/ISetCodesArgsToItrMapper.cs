using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface ISetCodesArgsToItrMapper : ICreateMapper<ISetCodesArgEntity, ISetCodesItrEntity>;
