using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.UserSetCards.Apis.Entities;

public interface IAllUserSetCardsXfrEntity : IXfrEntity
{
    string UserId { get; }
}
