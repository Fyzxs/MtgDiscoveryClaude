namespace Lib.Shared.Abstractions.Actions;

public interface ITransformationAction<in TTarget>
{
    void Transformation(TTarget target);
}
