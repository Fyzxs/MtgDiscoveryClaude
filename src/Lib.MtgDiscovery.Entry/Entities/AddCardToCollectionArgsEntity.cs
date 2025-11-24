using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Args.UserCards;

namespace Lib.MtgDiscovery.Entry.Entities;

public sealed class AddCardToCollectionArgsEntity : IAddCardToCollectionArgsEntity
{
    public IAuthUserArgEntity AuthUser { get; init; }
    public IAddUserCardArgEntity AddUserCard { get; init; }
}
