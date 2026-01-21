using System.Threading.Tasks;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Queries.Mappers;

internal interface IUserCardsByIdsItrToXfrMapper
{
    Task<IUserCardsByIdsXfrEntity> Map(IUserCardsByIdsItrEntity source);
}
