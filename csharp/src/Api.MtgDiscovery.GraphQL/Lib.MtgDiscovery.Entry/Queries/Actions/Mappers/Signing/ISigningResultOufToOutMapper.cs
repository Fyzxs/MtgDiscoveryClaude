using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Signing;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers.Signing;

internal interface ISigningResultOufToOutMapper
{
    Task<SigningResultOutEntity> Map(ISigningResultOufEntity oufEntity);
}
