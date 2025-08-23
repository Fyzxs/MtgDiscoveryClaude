using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Cards.Models;

internal sealed class LegalitiesItrEntity : ILegalitiesItrEntity
{
    private readonly dynamic _data;

    public LegalitiesItrEntity(dynamic data)
    {
        _data = data;
    }

    public string Standard => _data?.standard;
    public string Future => _data?.future;
    public string Historic => _data?.historic;
    public string Timeless => _data?.timeless;
    public string Gladiator => _data?.gladiator;
    public string Pioneer => _data?.pioneer;
    public string Explorer => _data?.explorer;
    public string Modern => _data?.modern;
    public string Legacy => _data?.legacy;
    public string Pauper => _data?.pauper;
    public string Vintage => _data?.vintage;
    public string Penny => _data?.penny;
    public string Commander => _data?.commander;
    public string Oathbreaker => _data?.oathbreaker;
    public string StandardBrawl => _data?.standardbrawl;
    public string Brawl => _data?.brawl;
    public string Alchemy => _data?.alchemy;
    public string PauperCommander => _data?.paupercommander;
    public string Duel => _data?.duel;
    public string OldSchool => _data?.oldschool;
    public string Premodern => _data?.premodern;
    public string PrEdh => _data?.predh;
}
