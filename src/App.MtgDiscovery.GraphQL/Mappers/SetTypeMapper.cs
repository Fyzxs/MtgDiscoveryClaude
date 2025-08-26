using App.MtgDiscovery.GraphQL.Entities.Outs.Sets;
using Lib.Shared.DataModels.Entities;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal interface IScryfallSetMapper
{
    ScryfallSetOutEntity Map(ISetItemItrEntity setItem);
}

internal sealed class ScryfallSetMapper : IScryfallSetMapper
{
    public ScryfallSetOutEntity Map(ISetItemItrEntity setItem)
    {
        return new ScryfallSetOutEntity
        {
            Id = setItem.Id,
            Code = setItem.Code,
            TcgPlayerId = setItem.TcgPlayerId,
            Name = setItem.Name,
            Uri = setItem.Uri,
            ScryfallUri = setItem.ScryfallUri,
            SearchUri = setItem.SearchUri,
            ReleasedAt = setItem.ReleasedAt,
            SetType = setItem.SetType,
            CardCount = setItem.CardCount,
            Digital = setItem.Digital,
            NonFoilOnly = setItem.NonFoilOnly,
            FoilOnly = setItem.FoilOnly,
            BlockCode = setItem.BlockCode,
            Block = setItem.Block,
            IconSvgUri = setItem.IconSvgUri,
            PrintedSize = setItem.PrintedSize
        };
    }
}
