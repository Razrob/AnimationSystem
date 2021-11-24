using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.Reflection;
using UnityEditorInternal;
using System.IO;

[CustomPropertyDrawer(typeof(AnimationAction), true)]
public class AnimationActionDrawer : PropertyDrawer
{
    private bool expandProperty;
    private int fieldCount;
    private bool inited;

    private int selectedTypeIndex;

    private Type[] actionsTypes;

    //private const string actionsPath = "Source/Scripts/AnimationsActions";
    private const string actionsPath = "AnimationActions";

    private const float FieldHeight = 18;
    private const float PropertyHeight = 20;
    private const float ClosePropertyHeight = 40;

    private Type GetTargetSerializedType(object property)
    {
        for (int i = 0; i < actionsTypes.Length; i++)
        {
            try
            {
                Convert.ChangeType(property, actionsTypes[i]);
                return actionsTypes[i];
            }
            catch { }
        }

        throw new KeyNotFoundException();
    }

    private UnityEngine.Object CreateAction(Type type)
    {
        AnimationAction action = ScriptableObject.CreateInstance(type) as AnimationAction;

        string fileName = type.Name;
        int fileCount = 0;
        while (File.Exists($"{Application.dataPath}/{actionsPath}/{fileName}.asset"))
            fileName += (++fileCount).ToString();

        AssetDatabase.CreateAsset(action, $"Assets/{actionsPath}/{fileName}.asset");
        return action;
    }

    private int DrawPopup(Rect position)
    {
        position.x += 117;
        position.width -= 117;
        List<string> typesNames = actionsTypes.Select(type => type.Name).ToList();
        typesNames.Insert(0, "Not specified");
        return EditorGUI.Popup(position, selectedTypeIndex + 1, typesNames.ToArray()) - 1;
    }

    private void TryFindActionTypes()
    {
        if (actionsTypes == null)
            actionsTypes = typeof(AnimationAction).Assembly.GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(AnimationAction))).ToArray();
    }

    private void TryInitialize(UnityEngine.Object property, Type targetType)
    {
        if (inited) return;

        inited = true;
        if (property != null)
            selectedTypeIndex = actionsTypes.ToList().IndexOf(targetType);
    }

    private void ReplaceAction(SerializedProperty property)
    {
        UnityEngine.Object createdAction = CreateAction(actionsTypes[selectedTypeIndex]);
        
        if (property.objectReferenceValue != null)
            NullifyAction(property);
        property.objectReferenceValue = createdAction;
    }

    private void NullifyAction(SerializedProperty property)
    {
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(property.objectReferenceValue));
        property.objectReferenceValue = null;
    }

    private void DuplicateAction(SerializedProperty property)
    {
        UnityEngine.Object createdAction = CreateAction(actionsTypes[selectedTypeIndex]);
        property.objectReferenceValue = createdAction;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        TryFindActionTypes();

        EditorGUI.BeginProperty(position, label, property);
        Type targetType = GetTargetSerializedType(property.objectReferenceValue);

        TryInitialize(property.objectReferenceValue, targetType);

        if (property.objectReferenceValue == null)
        {
            property.objectReferenceValue = null;
            selectedTypeIndex = -1;
        }
        position.height = FieldHeight;
        
        if (GUI.Button(new Rect(position.x, position.y, 100, position.height), new GUIContent("Duplicate")))
            if(property.objectReferenceValue != null) 
                DuplicateAction(property);

        EditorGUI.indentLevel--;

        int newTypeIndex = DrawPopup(position);

        if (selectedTypeIndex != newTypeIndex)
        {
            selectedTypeIndex = newTypeIndex;

            if (selectedTypeIndex > -1) ReplaceAction(property);
            else NullifyAction(property);
        }

        position.y += PropertyHeight;

        EditorGUI.indentLevel++;
        
        expandProperty = EditorGUI.BeginFoldoutHeaderGroup(new Rect(position.x + 15, position.y, 81, position.height), expandProperty, new GUIContent("Show"));
        EditorGUI.PropertyField(position, property, new GUIContent(" "));
        if (property.objectReferenceValue == null) expandProperty = false;

        if (expandProperty && property.objectReferenceValue != null)
        {
            SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
            serializedObject.Update();

            FieldInfo[] fields = targetType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            fieldCount = fields.Length;

            for (int i = 0; i < fields.Length; i++)
            {
                position.y += PropertyHeight;
                EditorGUI.PropertyField(position, serializedObject.FindProperty(fields[i].Name));
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
