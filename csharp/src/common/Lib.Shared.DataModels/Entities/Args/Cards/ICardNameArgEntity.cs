using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.Shared.DataModels.Entities.Args.Cards;

public interface ICardNameArgEntity : IUserIdArgEntity
{
    string CardName { get; }
}
