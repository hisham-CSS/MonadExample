using UnityEngine;

public abstract class AbilityFunctionBase : ScriptableObject
{
    // Common properties and methods shared by all steps
    public string Name => name;

    public abstract string Validate();
}
