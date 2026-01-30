using Lib.Scryfall.Ingestion.Collections;

namespace Lib.Scryfall.Ingestion.Tests.Fakes;

public sealed class TestDtoTransformerFake<TDto, TModel> : IScryfallDtoTransformer<TDto, TModel>
{
    public TModel TransformResult { get; init; }
    public int TransformInvokeCount { get; private set; }
    public TModel Transform(TDto dto)
    {
        TransformInvokeCount++;
        return TransformResult;
    }
}
