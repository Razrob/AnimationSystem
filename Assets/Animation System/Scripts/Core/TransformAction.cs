using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TransformAction : AnimationAction
{
    public sealed override void CallAction(Animator animator) => CallAction(animator.transform);
    public abstract void CallAction(Transform transform);
}
