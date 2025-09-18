namespace Lib.Shared.DataModels.Entities.Args;

public interface IAuthUserArgEntity
{
    string UserId { get; }
    string SourceId { get; }
    string DisplayName { get; }
}
