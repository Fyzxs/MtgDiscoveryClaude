using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;

/// <summary>
/// Root result entity for convention signing data containing cards grouped by set and artist.
/// </summary>
public interface ISigningResultOufEntity
{
    /// <summary>
    /// The sets containing cards by the selected artists, grouped with signing statistics.
    /// </summary>
    IEnumerable<ISigningSetGroupOufEntity> Sets { get; }
}
