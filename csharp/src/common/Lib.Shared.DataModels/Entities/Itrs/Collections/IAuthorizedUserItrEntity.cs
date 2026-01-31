namespace Lib.Shared.DataModels.Entities.Itrs.Collections;

public interface IAuthorizedUserItrEntity
{
    string UserId { get; }
    string Role { get; }
    string GrantedAt { get; }
    string GrantedBy { get; }
}
