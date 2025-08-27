namespace Lib.Shared.Abstractions.Identifiers;

public interface ICardNameGuidGenerator
{
    CardNameGuid GenerateGuid(string cardName);
}