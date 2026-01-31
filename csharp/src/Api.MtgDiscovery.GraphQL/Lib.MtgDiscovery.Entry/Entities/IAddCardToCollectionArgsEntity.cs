using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Args.UserCards;

namespace Lib.MtgDiscovery.Entry.Entities;

public interface IAddCardToCollectionArgsEntity
{
    IAuthUserArgEntity AuthUser { get; }
    IAddUserCardArgEntity AddUserCard { get; }

    /// <summary>
    /// When true, replaces existing card counts instead of adding to them.
    /// Used by migration tools to overwrite existing collection data.
    /// </summary>
    bool ReplaceMode { get; }
}
