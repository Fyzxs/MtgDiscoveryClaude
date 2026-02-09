using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Xfrs.Sets;

namespace Lib.Aggregator.Sets.Queries.Mappers;

internal interface IAllSetsItrToXfrMapper : ICreateMapper<IAllSetsItrEntity, IAllSetsXfrEntity>;
