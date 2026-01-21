using System.Threading.Tasks;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Adapter.UserSetCards.Apis.Entities;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

/// <summary>
/// Maps IAddUserCardXfrEntity to IAddCardToSetXfrEntity.
/// Creates AddCardToSetXfrEntity for updating UserSetCards aggregation.
/// </summary>
internal interface IAddUserCardXfrToAddCardToSetXfrMapper
{
    Task<IAddCardToSetXfrEntity> Map(IAddUserCardXfrEntity source);
}
