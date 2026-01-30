using Lib.Shared.DataModels.Entities.Models;

namespace Lib.Shared.DataModels.Entities.Args.UserSetCards;

public interface IAddCardToSetArgEntity : IUserIdArgModel
{
    string SetId { get; }
    string CardId { get; }
    string SetGroupId { get; }
    string FinishType { get; }
    int Count { get; }
}
