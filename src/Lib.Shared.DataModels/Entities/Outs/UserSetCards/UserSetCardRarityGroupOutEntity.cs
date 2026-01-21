namespace Lib.Shared.DataModels.Entities.Outs.UserSetCards;

public sealed class UserSetCardRarityGroupOutEntity
{
    public string Rarity { get; init; }
    public UserSetCardGroupOutEntity Group { get; init; }
}
