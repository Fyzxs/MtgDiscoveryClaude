namespace Lib.Shared.DataModels.Entities.Args.User;

public interface IAuthUserArgEntity
{
    string UserId { get; }
    string SourceId { get; }
    string DisplayName { get; }
    string Email { get; }
}
