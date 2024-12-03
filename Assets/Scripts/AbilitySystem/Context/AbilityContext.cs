using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbilityContext", menuName = "Abilities/Ability Context")]
public class AbilityContext : ScriptableObject
{
    [HideInInspector] // Hide from the Inspector; we'll make it read-only in the custom editor.
    public string AbilityName;

    [SerializeReference]
    public List<AbilityFunctionBase> ExecutionSteps = new List<AbilityFunctionBase>();

    private void OnValidate()
    {
        // Automatically set AbilityName to the name of the ScriptableObject
        AbilityName = name;
    }

    public string Validate()
    {
        if (string.IsNullOrWhiteSpace(AbilityName))
            return "Ability Name cannot be empty.";

        for (int i = 0; i < ExecutionSteps.Count; i++)
        {
            var step = ExecutionSteps[i];
            if (step == null)
                return $"Step {i + 1} is null.";

            string error = step.Validate();
            if (!string.IsNullOrEmpty(error))
                return $"Step {i + 1}: {error}";
        }

        return string.Empty; // No errors
    }
}
