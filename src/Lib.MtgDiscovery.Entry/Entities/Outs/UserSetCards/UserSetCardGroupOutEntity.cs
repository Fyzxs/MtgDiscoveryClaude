namespace Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;

public sealed class UserSetCardGroupOutEntity
{
    public UserSetCardFinishGroupOutEntity NonFoil { get; init; }
    public UserSetCardFinishGroupOutEntity Foil { get; init; }
    public UserSetCardFinishGroupOutEntity Etched { get; init; }
}
