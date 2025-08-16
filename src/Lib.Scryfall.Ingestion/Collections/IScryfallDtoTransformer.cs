namespace Lib.Scryfall.Ingestion.Internal.Collections;
internal interface IScryfallDtoTransformer<in TDto, out TDomain>
{
    TDomain Transform(TDto dto);
}
