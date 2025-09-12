namespace Lib.Shared.Abstractions.Actions;

public abstract class TransformationActionContainer<TTarget> : ITransformationAction<TTarget>
{
    private readonly ITransformationAction<TTarget>[] _actions;

    protected TransformationActionContainer(params ITransformationAction<TTarget>[] actions) => _actions = actions;

    public void Transformation(TTarget target)
    {
        foreach (ITransformationAction<TTarget> action in _actions)
        {
            action.Transformation(target);
        }
    }
}
