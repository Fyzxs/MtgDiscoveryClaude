namespace Lib.Shared.Abstractions.Actions.Transformations;

public interface ITransformationAction<in TTarget>
{
    void Transformation(TTarget target);
}
