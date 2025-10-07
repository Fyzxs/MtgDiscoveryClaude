namespace Lib.Shared.DataModels.Entities.Args.UserSetCards;

public interface IAddSetGroupToUserSetCardArgEntity
{
    string SetId { get; }
    string SetGroupId { get; }
    bool Collecting { get; }
    int Count { get; }
}
