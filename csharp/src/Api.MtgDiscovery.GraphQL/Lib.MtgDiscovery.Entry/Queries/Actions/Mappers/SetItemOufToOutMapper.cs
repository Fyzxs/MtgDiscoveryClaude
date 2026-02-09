using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Sets;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class SetItemOufToOutMapper : ISetItemOufToOutMapper
{
    public Task<SetItemOutEntity> Map(ISetItemOufEntity setItem)
    {
        SetItemOutEntity result = new()
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
            Groupings = SetGroupingOutEntities(setItem),
            UserCollection = null
        };

        return Task.FromResult(result);
    }

    private static ICollection<SetGroupingOutEntity> SetGroupingOutEntities(ISetItemOufEntity setItem)
    {
        ICollection<SetGroupingOutEntity> groupings = null;

        if (setItem.Groupings != null)
        {
            groupings = [.. setItem.Groupings.Select(g => new SetGroupingOutEntity
            {
                Id = g.Id,
                DisplayName = g.DisplayName,
                Order = g.Order,
                CardCounts = g.CardCounts != null ? new FinishCountsOutEntity
                {
                    Total = g.CardCounts.Total,
                    NonFoil = g.CardCounts.NonFoil,
                    Foil = g.CardCounts.Foil,
                    Etched = g.CardCounts.Etched
                } : null,
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
