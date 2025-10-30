using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Xfrs;

namespace Lib.Aggregator.Sets.Queries.Mappers;

/// <summary>
/// Maps no-args entity from ITR (internal transfer) to XFR (adapter transfer).
/// </summary>
internal interface IAllSetsItrToXfrMapper
{
    Task<IAllSetsXfrEntity> Map(IAllSetsItrEntity input);
}
