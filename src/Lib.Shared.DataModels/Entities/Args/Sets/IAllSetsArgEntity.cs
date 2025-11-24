using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.Shared.DataModels.Entities.Args.Sets;

/// <summary>
/// Argument entity for AllSets query operations.
/// Supports optional user identification for enrichment with collection data.
/// </summary>
public interface IAllSetsArgEntity : IUserIdArgEntity
{
}
