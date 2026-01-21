namespace Lib.Adapter.UserSetCards.Apis.Entities;

public interface IUserSetCardUpsertXfrEntity
{
    string UserId { get; }
    string SetId { get; }
    int TotalCards { get; }
    int UniqueCards { get; }
    ICollection<string> Collecting { get; }
    IDictionary<string, IUserSetCardGroupXfrEntity> Groups { get; }
}
