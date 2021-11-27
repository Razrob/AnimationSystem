using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AnimationActionBase : ScriptableObject
{
    [field: SerializeField] [field: Range(0, 1)] public float CallTimeOffcet { get; private set; }

    public abstract void CallAction(Transform transform);
}