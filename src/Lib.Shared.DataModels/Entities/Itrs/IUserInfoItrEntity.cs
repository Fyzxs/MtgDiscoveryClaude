namespace Lib.Shared.DataModels.Entities.Itrs;

public interface IUserInfoOufEntity
{
    string UserId { get; }
    string UserSourceId { get; }
    string UserNickname { get; }
}

public interface IUserInfoItrEntity
{
    string UserId { get; }
    string UserSourceId { get; }
    string UserNickname { get; }
}
