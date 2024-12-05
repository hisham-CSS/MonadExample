using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbilityContext))]
public class AbilityContextEditor : Editor
{
    private SerializedProperty executionStepsProperty;
    private SerializedProperty postExecutionStepsProperty;
    private string validationMessage = string.Empty;

    private void OnEnable()
    {
        executionStepsProperty = serializedObject.FindProperty("ExecutionSteps");
        postExecutionStepsProperty = serializedObject.FindProperty("PostExecutionSteps");
        UpdateAbilityName(); // Ensure AbilityName matches ScriptableObject name
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Display ability name
        DisplayAbilityName();

        // Display and manage Execution Steps
        DisplayStepList<IExecution>(executionStepsProperty, "Execution Steps");

        // Display and manage Post Execution Steps
        DisplayStepList<IPostExecution>(postExecutionStepsProperty, "Post Execution Steps");

        // Validate ability configuration
        DisplayValidationMessage();

        serializedObject.ApplyModifiedProperties();
    }

    private void DisplayAbilityName()
    {
        AbilityContext context = (AbilityContext)target;

        if (context.AbilityName != context.name)
        {
            UpdateAbilityName();
        }

        EditorGUILayout.LabelField("Ability Name", context.AbilityName);
        EditorGUILayout.Space();
    }

    private void DisplayStepList<T>(SerializedProperty property, string label) where T : class
    {
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

        for (int i = property.arraySize - 1; i >= 0; i--) // Iterate backward
        {
            var stepProperty = property.GetArrayElementAtIndex(i);

            if (stepProperty.objectReferenceValue == null) continue;

            EditorGUILayout.BeginVertical("box");

            SerializedObject stepSerializedObject = new SerializedObject(stepProperty.objectReferenceValue);
            EditorGUILayout.LabelField($"Step {i + 1}: {stepProperty.objectReferenceValue.GetType().Name}", EditorStyles.boldLabel);

            // Display editable fields for the step
            var iterator = stepSerializedObject.GetIterator();
            iterator.NextVisible(true); // Skip "Object" fields
            while (iterator.NextVisible(false))
            {
                EditorGUILayout.PropertyField(iterator, true);
            }

            stepSerializedObject.ApplyModifiedProperties();

            // Remove button
            if (GUILayout.Button("Remove Step"))
            {
                RemoveStep(property, i);
            }

            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button($"Add {label}"))
        {
            ShowAddStepMenu<T>(property);
        }

        EditorGUILayout.Space();
    }

    private void DisplayStepDetails(SerializedProperty stepProperty, int index)
    {
        SerializedObject stepSerializedObject = new SerializedObject(stepProperty.objectReferenceValue);

        EditorGUILayout.LabelField($"Step {index + 1}: {stepProperty.objectReferenceValue.GetType().Name}", EditorStyles.boldLabel);

        var iterator = stepSerializedObject.GetIterator();
        iterator.NextVisible(true); // Skip "Object" fields

        while (iterator.NextVisible(false))
        {
            EditorGUILayout.PropertyField(iterator, true);
        }

        stepSerializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Remove Step"))
        {
            RemoveStep(stepProperty, index);
        }
    }

    private void ShowAddStepMenu<T>(SerializedProperty property) where T : class
    {
        GenericMenu menu = new GenericMenu();

        foreach (var type in AbilityFunctionRegistry.GetAllAbilityFunctions<T>())
        {
            menu.AddItem(new GUIContent(type.Name), false, () =>
            {
                var newStep = CreateInstance(type) as AbilityFunctionBase;
                if (newStep == null) return;

                AssetDatabase.AddObjectToAsset(newStep, target);
                property.arraySize++;
                property.GetArrayElementAtIndex(property.arraySize - 1).objectReferenceValue = newStep;

                UpdateInspector();
            });
        }

        menu.ShowAsContext();
    }

    private void RemoveStep(SerializedProperty stepProperty, int index)
    {
        var step = stepProperty.GetArrayElementAtIndex(index).objectReferenceValue as ScriptableObject;
        if (step != null)
        {   
            AssetDatabase.RemoveObjectFromAsset(step);
            DestroyImmediate(step, true);
        }

        stepProperty.DeleteArrayElementAtIndex(index);
        UpdateInspector();
    }

    private void DisplayValidationMessage()
    {
        AbilityContext context = (AbilityContext)target;
        validationMessage = context.Validate();

        if (!string.IsNullOrEmpty(validationMessage))
        {
            EditorGUILayout.HelpBox(validationMessage, MessageType.Error);
        }
    }

    private void UpdateAbilityName()
    {
        AbilityContext context = (AbilityContext)target;

        if (context.AbilityName != context.name)
        {
            context.AbilityName = context.name;
            UpdateInspector();
        }
    }

    //ensure inspector updates properly
    private void UpdateInspector()
    {
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}