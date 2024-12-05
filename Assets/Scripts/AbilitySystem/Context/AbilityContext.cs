using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbilityContext", menuName = "Abilities/Ability Context")]
public class AbilityContext : ScriptableObject
{
    [HideInInspector] // Hide from the Inspector; we'll make it read-only in the custom editor.
    public string AbilityName;

    [SerializeReference]
    public List<AbilityFunctionBase> ExecutionSteps = new List<AbilityFunctionBase>();

    [SerializeReference]
    public List<AbilityFunctionBase> PostExecutionSteps = new List<AbilityFunctionBase>();

    private void OnValidate()
    {
        // Automatically set AbilityName to the name of the ScriptableObject
        AbilityName = name;
    }

    public string Validate()
    {
        string errorMsg = string.Empty;
        if (string.IsNullOrWhiteSpace(AbilityName))
            errorMsg = "Ability Name cannot be empty. ";

        for (int i = 0; i < ExecutionSteps.Count; i++)
        {
            var step = ExecutionSteps[i];
            if (step == null)
            {
                errorMsg += $"Execution Step {i + 1} is null. ";
                break;
            }

            string error = step.Validate();
            if (!string.IsNullOrEmpty(error))
                errorMsg += $"Execution Step {i + 1}: {error} ";
        }

        for (int i = 0; i < PostExecutionSteps.Count; i++)
        {
            var step = PostExecutionSteps[i];
            if (step == null)
            {
                errorMsg += $"Post Execution Step {i + 1} is null. ";
                break;
            }

            string error = step.Validate();
            if (!string.IsNullOrEmpty(error))
                errorMsg += $"Post Execution Step {i + 1}: {error}";
        }

        return errorMsg; // No errors
    }
}
