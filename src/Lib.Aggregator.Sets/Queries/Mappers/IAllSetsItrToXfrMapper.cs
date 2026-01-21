using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Xfrs.Sets;

namespace Lib.Aggregator.Sets.Queries.Mappers;

/// <summary>
/// Maps no-args entity from ITR (internal transfer) to XFR (adapter transfer).
/// </summary>
internal interface IAllSetsItrToXfrMapper
{
    Task<IAllSetsXfrEntity> Map(IAllSetsItrEntity input);
}
