using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface ISetCodeArgsToItrMapper
{
    Task<ISetCodeItrEntity> Map(ISetCodeArgEntity args);
}
