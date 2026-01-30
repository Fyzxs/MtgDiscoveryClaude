namespace Lib.Scryfall.Ingestion.Collections;

internal interface IScryfallDtoTransformer<in TDto, out TDomain>
{
    TDomain Transform(TDto dto);
}
