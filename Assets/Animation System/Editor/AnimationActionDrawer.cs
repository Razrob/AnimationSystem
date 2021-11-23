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
    private bool show;
    private int fieldCount;
    private int selectedTypeIndex;

    private const string actionsPath = "AnimationActions";

    private Type[] actionsTypes;

    private Type GetTargetType(object property)
    {
        if(actionsTypes == null)
            actionsTypes = typeof(AnimationActionBase).Assembly.GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(AnimationActionBase))).ToArray();

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

    private string CreateAction(Type type)
    {
        AnimationActionBase action = ScriptableObject.CreateInstance(type) as AnimationActionBase;

        string fileName = type.Name;
        int fileCount = 0;
        while(File.Exists($"{Application.dataPath}/{actionsPath}/{fileName}.asset"))
        {
            fileCount++;
            fileName += fileCount.ToString();
        }

        AssetDatabase.CreateAsset(action, $"Assets/{actionsPath}/{fileName}.asset");
        return $"Assets/{actionsPath}/{fileName}.asset";
    }

    private int DrawPopup(Rect position)
    {
        if (actionsTypes == null)
            actionsTypes = typeof(AnimationActionBase).Assembly.GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(AnimationActionBase))).ToArray();
        List<string> types = actionsTypes.Select(type => type.Name).ToList();
        types.Insert(0, "Not specified");
        return EditorGUI.Popup(position, selectedTypeIndex, types.ToArray());
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Type type = GetTargetType(property.objectReferenceValue);
        if (property.objectReferenceValue != null) selectedTypeIndex = actionsTypes.ToList().IndexOf(type) + 1;
        else selectedTypeIndex = 0;
        position.height = 18;
        EditorGUI.indentLevel--;
        int newIndex = DrawPopup(position);
        if (property.objectReferenceValue == null)
            property.objectReferenceValue = null;

        if(newIndex == 0 && property.objectReferenceValue != null)
        {
            selectedTypeIndex = newIndex;
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(property.objectReferenceValue));
            property.objectReferenceValue = null;
        }
        else if(newIndex != selectedTypeIndex)
        {
            selectedTypeIndex = newIndex;
            string path = CreateAction(actionsTypes[selectedTypeIndex - 1]);
            if(property.objectReferenceValue != null) AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(property.objectReferenceValue));

            UnityEngine.Object action = AssetDatabase.LoadAssetAtPath(path, actionsTypes[selectedTypeIndex - 1]);
            property.objectReferenceValue = action;
        }


        position.y += 20;

        show = EditorGUI.BeginFoldoutHeaderGroup(new Rect(position.x, position.y, 
            EditorGUIUtility.currentViewWidth * 0.22f * Mathf.Pow(EditorGUIUtility.currentViewWidth / 500, 0.5f), position.height), show, new GUIContent("Show"));
        EditorGUI.PropertyField(position, property, new GUIContent(" "));
        EditorGUI.indentLevel++;

        if (show && property.objectReferenceValue == null)
            show = false;

        if (show && property.objectReferenceValue != null)
        {
            SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
            serializedObject.Update();

            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            fieldCount = fields.Length;

            for(int i = 0; i < fields.Length; i++)
            {
                position.y += 20;
                EditorGUI.PropertyField(position, serializedObject.FindProperty(fields[i].Name));
            }

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        EditorGUI.EndFoldoutHeaderGroup();
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!show) return 40;
        if (property.objectReferenceValue != null)
            if(!show) return 40;
        return (fieldCount + 2) * 20;
    }
}
