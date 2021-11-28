using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AnimationActionBase : ScriptableObject
{
    [SerializeField] [Range(0, 1)] protected float callTimeOffcet;
    public float CallTimeOffcet => callTimeOffcet;

    public abstract void CallAction(Transform transform);
}