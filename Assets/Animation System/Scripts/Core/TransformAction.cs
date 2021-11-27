using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TransformAction : AnimationActionBase
{
    public override abstract void CallAction(Transform transform);
}
