using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.Reflection;
using UnityEditorInternal;
using System.IO;

[CustomPropertyDrawer(typeof(AnimationActionBase), true)]
public class AnimationActionDrawer : PropertyDrawer
{
    private SubclassesTypesFinder<AnimationActionBase> actionTypesFinder = new SubclassesTypesFinder<AnimationActionBase>();
    private AssetDataHandler assetDataHandler = new AssetDataHandler();

    private bool expandProperty;
    private int fieldCount;
    private bool inited;

    private int selectedTypeIndex;

    //private const string actionsPath = "Source/Scripts/AnimationsActions";
    private const string actionsPath = "AnimationActions";

    private const float FieldHeight = 18;
    private const float PropertyHeight = 20;
    private const float ClosePropertyHeight = 40;


    private int DrawPopup(Rect position)
    {
        position.x += 117;
        position.width -= 117;
        List<string> typesNames = actionTypesFinder.ActionsTypes.Select(type => type.Name).ToList();
        typesNames.Insert(0, "Not specified");
        return EditorGUI.Popup(position, selectedTypeIndex + 1, typesNames.ToArray()) - 1;
    }

    private void TryInitialize(UnityEngine.Object property, Type targetType)
    {
        if (inited) return;

        inited = true;
        if (property != null)
            selectedTypeIndex = actionTypesFinder.ActionsTypes.ToList().IndexOf(targetType);
    }

    private SerializedProperty ReplaceAction(SerializedProperty property)
    {
        UnityEngine.Object createdAction = assetDataHandler.CreateAsset(actionTypesFinder.ActionsTypes[selectedTypeIndex], actionsPath);
        if (property.objectReferenceValue != null)
            assetDataHandler.DeleteAsset(property.objectReferenceValue);
        property.objectReferenceValue = createdAction;

        return property;
    }

    private SerializedProperty RefreshProperty(SerializedProperty property, ref Rect position, Type targetType)
    {
        if (property.objectReferenceValue == null)
        {
            property.objectReferenceValue = null;
            selectedTypeIndex = -1;
        }
        position.height = FieldHeight;

        if (GUI.Button(new Rect(position.x, position.y, 100, position.height), new GUIContent("Duplicate")))
            if (property.objectReferenceValue != null)
                property.objectReferenceValue = assetDataHandler.DuplicateAsset(targetType, property.objectReferenceValue, actionsPath);

        EditorGUI.indentLevel--;

        int newTypeIndex = DrawPopup(position);

        if (selectedTypeIndex != newTypeIndex)
        {
            selectedTypeIndex = newTypeIndex;

            if (selectedTypeIndex > -1) property = ReplaceAction(property);
            else property.objectReferenceValue = assetDataHandler.DeleteAsset(property.objectReferenceValue);
        }

        position.y += PropertyHeight;

        EditorGUI.indentLevel++;

        return property;
    }

    private FieldInfo[] GetObjectFields(Type targetType)
    {
        FieldInfo[] fields = targetType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(field => field.GetCustomAttribute<SerializeField>() != null)
                .ToArray();
        return fields;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        Type targetType = actionTypesFinder.GetTargetSerializedType(property.objectReferenceValue);

        TryInitialize(property.objectReferenceValue, targetType);
        property = RefreshProperty(property, ref position, targetType);

        expandProperty = EditorGUI.BeginFoldoutHeaderGroup(new Rect(position.x + 15, position.y, 81, position.height), expandProperty, new GUIContent("Show")) && property.objectReferenceValue != null;
        
        EditorGUI.PropertyField(position, property, new GUIContent(" "));

        if (expandProperty && property.objectReferenceValue != null)
        {
            SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
            serializedObject.Update();

            FieldInfo[] fields = GetObjectFields(targetType);

            fieldCount = fields.Length;
            for (int i = 0; i < fields.Length; i++)
            {
                position.y += PropertyHeight;
                SerializedProperty serializedProperty = serializedObject.FindProperty(fields[i].Name);
                if(serializedProperty != null) EditorGUI.PropertyField(position, serializedProperty);
            }

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        EditorGUI.EndFoldoutHeaderGroup();
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!expandProperty || property.objectReferenceValue == null)
            return ClosePropertyHeight;
        return fieldCount * PropertyHeight + ClosePropertyHeight;
    }
}
