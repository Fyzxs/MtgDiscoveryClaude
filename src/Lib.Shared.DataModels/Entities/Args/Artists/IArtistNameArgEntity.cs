using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.Shared.DataModels.Entities.Args.Artists;

public interface IArtistNameArgEntity : IUserIdArgEntity
{
    string ArtistName { get; }
}
