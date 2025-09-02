namespace Lib.Scryfall.Shared.Apis.Models;

public interface IScryfallRuling
{
    string OracleId();
    AggregatedRulingData Data();
}