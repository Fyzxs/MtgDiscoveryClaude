using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Args.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Entities;

/// <summary>
/// Combined argument entity for adding a card to a set, including authentication.
/// Used by migration tools for direct UserSetCards updates.
/// </summary>
public interface IAddCardToSetArgsEntity
{
    IAuthUserArgEntity AuthUser { get; }
    IAddCardToSetArgEntity AddCardToSet { get; }
}
