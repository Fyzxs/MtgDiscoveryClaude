namespace Lib.Aggregator.Cards.Tests.Fakes;

internal static class ScryfallCardItemFactoryFake
{
    public static ScryfallCardItemExtEntity Create(dynamic data) => new() { Data = data };
}
