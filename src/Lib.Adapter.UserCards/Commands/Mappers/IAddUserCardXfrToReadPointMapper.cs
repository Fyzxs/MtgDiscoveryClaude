using System.Threading.Tasks;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.UserCards.Commands.Mappers;

/// <summary>
/// Maps AddUserCardXfrEntity to a Cosmos read point for querying user card data.
/// </summary>
internal interface IAddUserCardXfrToReadPointMapper
{
    /// <summary>
    /// Maps the transfer entity to a read point item for Cosmos queries.
    /// </summary>
    /// <param name="source">The source transfer entity.</param>
    /// <returns>A read point item configured for the user card lookup.</returns>
    Task<ReadPointItem> Map(IAddUserCardXfrEntity source);
}
