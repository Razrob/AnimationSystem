using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

using Object = UnityEngine.Object;

public class AssetDataHandler
{
    private void SaveAsset(Object asset, string path, string name)
    {
        AssetDatabase.CreateAsset(asset, $"Assets/{path}/{name}.asset");
    }

    private string GenerateFileName(string defaultName, string assetPath)
    {
        int fileCount = 0;
        while (File.Exists($"{Application.dataPath}/{assetPath}/{defaultName}.asset"))
        {
            if (fileCount > 0) defaultName = defaultName.Remove(defaultName.Length - 1);
            defaultName += (++fileCount).ToString();
        }

        return defaultName;
    }

    public Object DeleteAsset(Object asset)
    {
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(asset));

        return null;
    }

    public Object CreateAsset(Type type, string path)
    {
        Object asset = ScriptableObject.CreateInstance(type);
        SaveAsset(asset, path, GenerateFileName(type.Name, path));

        return asset;
    }

    public Object DuplicateAsset(Type type, Object @object, string path)
    {
        Object asset = Object.Instantiate(@object);
        SaveAsset(asset, path, GenerateFileName(type.Name, path));

        return asset;
    }
}
