using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubclassesTypesFinder<T>
{
    public Type[] ActionsTypes { get; private set; }

    public Type GetTargetSerializedType(object property)
    {
        TryFindActionTypes();

        for (int i = 0; i < ActionsTypes.Length; i++)
        {
            try
            {
                Convert.ChangeType(property, ActionsTypes[i]);
                return ActionsTypes[i];
            }
            catch { }
        }

        throw new KeyNotFoundException();
    }
    private void TryFindActionTypes()
    {
        if (ActionsTypes == null)
            ActionsTypes = typeof(T).Assembly.GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(T))).ToArray();
    }
}
