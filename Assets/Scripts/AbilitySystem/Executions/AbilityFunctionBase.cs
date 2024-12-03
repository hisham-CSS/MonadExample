using UnityEngine;

public abstract class AbilityFunctionBase : ScriptableObject, IAbilityFunction
{
    public abstract Monad<AbilityRuntimeContext> Execute(Monad<AbilityRuntimeContext> runtimeContext);

    public virtual string Validate()
    {
        return string.Empty; // Default: No validation errors
    }
}
