using System;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class Singleton<T> where T : Component
{
    private static object empty;

    private static T instance;
    public static T Instance => GetInstance();

    private static T GetInstance()
    {
        if (instance == null) 
            lock (empty)
            {
                instance = Object.FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject($"Singleton{typeof(T).Name}");
                    instance = singletonObject.AddComponent<T>();
                }
            }

        return instance;
    }
}
