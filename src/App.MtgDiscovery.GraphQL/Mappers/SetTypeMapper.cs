using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Outs.Sets;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal interface ISetItemItrToOutMapper : ICreateMapper<ISetItemItrEntity, ScryfallSetOutEntity>
{
}

internal sealed class SetItemItrToOutMapper : ISetItemItrToOutMapper
{
    public Task<ScryfallSetOutEntity> Map(ISetItemItrEntity setItem)
    {
        ScryfallSetOutEntity result = new()
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
            Groupings = SetGroupingOutEntities(setItem)
        };

        return Task.FromResult(result);
    }

    private static ICollection<SetGroupingOutEntity> SetGroupingOutEntities(ISetItemItrEntity setItem)
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

        return groupings;
    }

}
