namespace Lib.Shared.DataModels.Entities.Args.User;

public interface IAuthUserArgEntity : IUserIdArgEntity
{
    string SourceId { get; }
    string DisplayName { get; }
    string Email { get; }
}
