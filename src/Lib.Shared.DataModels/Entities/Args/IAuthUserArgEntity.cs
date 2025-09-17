namespace Lib.Shared.DataModels.Entities;

public interface IAuthUserArgEntity
{
    string UserId { get; }
    string SourceId { get; }
    string DisplayName { get; }
}
