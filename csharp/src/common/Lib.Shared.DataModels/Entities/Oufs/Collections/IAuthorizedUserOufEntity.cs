namespace Lib.Shared.DataModels.Entities.Oufs.Collections;

public interface IAuthorizedUserOufEntity
{
    string UserId { get; }
    string Role { get; }
    string GrantedAt { get; }
    string GrantedBy { get; }
}
