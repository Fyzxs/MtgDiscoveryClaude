using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Example.Core;
using Lib.Scryfall.Ingestion.Collections;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging.Abstractions;

namespace Example.Scryfall.ApiDemo;

public sealed class ScryfallApiDemoApplication : ExampleApplication
{
    protected override async Task Execute()
    {
        await Console.Out.WriteLineAsync("Starting Scryfall API Demo...").ConfigureAwait(false);
        await Console.Out.WriteLineAsync("").ConfigureAwait(false);

        Stopwatch stopwatch = Stopwatch.StartNew();

        await Console.Out.WriteLineAsync("Fetching all sets from Scryfall...").ConfigureAwait(false);
        HttpScryfallSetCollection sets = new(NullLogger.Instance);

        int setCount = 0;
        int totalCards = 0;
        const int MaxSetsToProcess = 3;

        await foreach (IScryfallSet set in sets.ConfigureAwait(false))
        {
            setCount++;
            await Console.Out.WriteLineAsync($"Set {setCount}: {set.Name()} ({set.Code()})").ConfigureAwait(false);

            if (setCount > MaxSetsToProcess) continue;

            await Console.Out.WriteLineAsync($"  Fetching cards for {set.Name()}...").ConfigureAwait(false);
            HttpScryfallCardCollection cards = new(set, NullLogger.Instance);

            int cardCount = 0;
            const int MaxCardsToShow = 5;

            await foreach (IScryfallCard card in cards.ConfigureAwait(false))
            {
                cardCount++;
                if (cardCount > MaxCardsToShow) continue;

                await Console.Out.WriteLineAsync($"    Card {cardCount}: {card.Name()}").ConfigureAwait(false);
            }

            totalCards += cardCount;
            await Console.Out.WriteLineAsync($"  Total cards in set: {cardCount}").ConfigureAwait(false);
            await Console.Out.WriteLineAsync("").ConfigureAwait(false);
        }

        stopwatch.Stop();

        await Console.Out.WriteLineAsync("").ConfigureAwait(false);
        await Console.Out.WriteLineAsync("Summary:").ConfigureAwait(false);
        await Console.Out.WriteLineAsync($"  Total sets found: {setCount}").ConfigureAwait(false);
        await Console.Out.WriteLineAsync($"  Sets processed for cards: {Math.Min(setCount, MaxSetsToProcess)}").ConfigureAwait(false);
        await Console.Out.WriteLineAsync($"  Total cards retrieved: {totalCards}").ConfigureAwait(false);
        await Console.Out.WriteLineAsync($"  Time elapsed: {stopwatch.Elapsed.TotalSeconds:F2} seconds").ConfigureAwait(false);
        await Console.Out.WriteLineAsync("").ConfigureAwait(false);
        await Console.Out.WriteLineAsync("Rate limiting is active (100ms between requests)").ConfigureAwait(false);
        await Console.Out.WriteLineAsync("").ConfigureAwait(false);
        await Console.Out.WriteLineAsync("Demo completed successfully!").ConfigureAwait(false);
    }
}
