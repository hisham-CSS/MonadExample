using static UnityEngine.GraphicsBuffer;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbilityContext))]
public class AbilityContextEditor : Editor
{
    private string validationMessage = string.Empty;
    private SerializedProperty executionStepsProperty;

    private void OnEnable()
    {
        executionStepsProperty = serializedObject.FindProperty("ExecutionSteps");

        // Force the AbilityName to match the ScriptableObject name
        UpdateAbilityName();
    }

    public override void OnInspectorGUI()
    {
        // Update serialized object
        serializedObject.Update();

        AbilityContext context = (AbilityContext)target;

        // Force the AbilityName to match the ScriptableObject name
        UpdateAbilityName();

        // Display the read-only Ability Name
        EditorGUILayout.LabelField("Ability Name", context.AbilityName);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Execution Steps", EditorStyles.boldLabel);

        if (context.ExecutionSteps == null)
        {
            context.ExecutionSteps = new List<AbilityFunctionBase>();
        }

        // Display and edit execution steps
        for (int i = 0; i < executionStepsProperty.arraySize; i++)
        {
            var stepProperty = executionStepsProperty.GetArrayElementAtIndex(i);

            if (stepProperty.objectReferenceValue == null)
            {
                continue;
            }

            EditorGUILayout.BeginVertical("box");
            SerializedObject stepSerializedObject = new SerializedObject(stepProperty.objectReferenceValue);

            EditorGUILayout.LabelField($"Step {i + 1}: {stepProperty.objectReferenceValue.GetType().Name}", EditorStyles.boldLabel);

            var iterator = stepSerializedObject.GetIterator();
            iterator.NextVisible(true); // Skip generic "Object" fields

            while (iterator.NextVisible(false))
            {
                EditorGUILayout.PropertyField(iterator, true);
            }

            stepSerializedObject.ApplyModifiedProperties();

            // Remove step button
            if (GUILayout.Button("Remove Step"))
            {
                var step = stepProperty.objectReferenceValue as ScriptableObject;
                if (step != null && AssetDatabase.Contains(step))
                {
                    AssetDatabase.RemoveObjectFromAsset(step);
                    DestroyImmediate(step, true);
                }
                executionStepsProperty.DeleteArrayElementAtIndex(i);
                break;
            }

            EditorGUILayout.EndVertical();
        }

        // Add new execution step button
        if (GUILayout.Button("Add Execution Step"))
        {
            GenericMenu menu = new GenericMenu();

            foreach (var type in AbilityFunctionRegistry.GetAllAbilityFunctions())
            {
                menu.AddItem(new GUIContent(type.Name), false, () =>
                {
                    // Create a new instance of the selected execution step type
                    var newFunction = ScriptableObject.CreateInstance(type) as AbilityFunctionBase;

                    if (newFunction != null)
                    {
                        // Add the new function to the AbilityContext asset
                        AssetDatabase.AddObjectToAsset(newFunction, target);
                        AssetDatabase.SaveAssets();

                        // Update the SerializedProperty array
                        executionStepsProperty.arraySize++;
                        executionStepsProperty.GetArrayElementAtIndex(executionStepsProperty.arraySize - 1).objectReferenceValue = newFunction;

                        // Apply changes to the serialized object
                        serializedObject.ApplyModifiedProperties();

                        // Refresh the Inspector to display the new step
                        EditorUtility.SetDirty(target);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                    else
                    {
                        Debug.LogError($"Failed to create instance of {type.Name}");
                    }
                });
            }

            menu.ShowAsContext();
        }

        serializedObject.ApplyModifiedProperties();

        // Run validation
        validationMessage = context.Validate();

        EditorGUILayout.Space();

        // Display validation messages
        if (!string.IsNullOrEmpty(validationMessage))
        {
            EditorGUILayout.HelpBox($"Validation Error: {validationMessage}", MessageType.Error);
        }
    }

    private void UpdateAbilityName()
    {
        AbilityContext context = (AbilityContext)target;

        // Set the AbilityName to match the ScriptableObject name
        if (context.AbilityName != context.name)
        {
            context.AbilityName = context.name;
            EditorUtility.SetDirty(context); // Mark the object as dirty to save changes
        }
    }
}
