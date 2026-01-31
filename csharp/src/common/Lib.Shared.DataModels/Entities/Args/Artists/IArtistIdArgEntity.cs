using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.Shared.DataModels.Entities.Args.Artists;

public interface IArtistIdArgEntity : IUserIdArgEntity
{
    string ArtistId { get; }
}
