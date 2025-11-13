using System.Threading.Tasks;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Commands.Mappers;

internal interface IAddCardToSetItrToXfrMapper
{
    Task<IAddCardToSetXfrEntity> Map(IAddCardToSetItrEntity source);
}
