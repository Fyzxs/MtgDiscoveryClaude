using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.Sets;
using Lib.Shared.DataModels.Entities.Itrs.Sets;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface ISetCodeArgToItrMapper : ICreateMapper<ISetCodeArgEntity, ISetCodeItrEntity>;
