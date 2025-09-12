using System.Collections.Generic;
using System.Linq;
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
        ICollection<SetGroupingOutEntity> groupings = null;

        if (setItem.Groupings != null)
        {
            groupings = [.. setItem.Groupings.Select(g => new SetGroupingOutEntity
            {
                Id = g.Id,
                DisplayName = g.DisplayName,
                Order = g.Order,
                CardCount = g.CardCount,
                RawQuery = g.RawQuery,
                Filters = g.Filters != null ? new GroupingFiltersOutEntity
                {
                    CollectorNumberRange = g.Filters.CollectorNumberRange != null
                        ? new CollectorNumberRangeOutEntity
                        {
                            Min = g.Filters.CollectorNumberRange.Min,
                            Max = g.Filters.CollectorNumberRange.Max,
                            OrConditions = g.Filters.CollectorNumberRange.OrConditions
                        }
                        : null,
                    Properties = g.Filters.Properties
                } : null
            })];
        }

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
            PrintedSize = setItem.PrintedSize,
            Groupings = groupings
        };
    }
}
