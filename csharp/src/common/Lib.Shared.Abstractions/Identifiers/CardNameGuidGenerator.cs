using System;
using Lib.Universal.Utilities;

namespace Lib.Shared.Abstractions.Identifiers;

public sealed class CardNameGuidGenerator : ICardNameGuidGenerator
{
    // Use a consistent namespace GUID for all MTG card names
    // This is "MtgCardNameGuid" encoded as a GUID
    private static readonly Guid s_cardNameNamespace = new("4d746743-6172-644e-616d-654775696400");

    public CardNameGuid GenerateGuid(string cardName)
    {
        if (string.IsNullOrWhiteSpace(cardName))
        {
            throw new ArgumentException("Card name cannot be empty or whitespace.", nameof(cardName));
        }

        // Normalize the card name to lowercase for consistent GUID generation
        string normalizedName = cardName.ToLowerInvariant();

        // Generate a deterministic GUID using SHA-1 hashing (version 5)
        Guid guid = GuidUtility.Create(s_cardNameNamespace, normalizedName);

        return new CardNameGuid(guid);
    }
}
