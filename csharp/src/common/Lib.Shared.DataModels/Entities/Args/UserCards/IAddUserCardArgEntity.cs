using Lib.Shared.DataModels.Entities.Models;

namespace Lib.Shared.DataModels.Entities.Args.UserCards;

public interface IAddUserCardArgEntity : IUserIdArgModel
{
    string CardId { get; }
    string SetId { get; }
    IUserCardDetailsArgEntity UserCardDetails { get; }
}
