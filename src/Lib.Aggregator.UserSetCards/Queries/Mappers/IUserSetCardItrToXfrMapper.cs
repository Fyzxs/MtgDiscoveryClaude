using System.Threading.Tasks;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Aggregator.UserSetCards.Entities;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal interface IUserSetCardItrToXfrMapper
{
    Task<IUserSetCardGetXfrEntity> Map(IUserSetCardItrEntity userSetCard);
}
