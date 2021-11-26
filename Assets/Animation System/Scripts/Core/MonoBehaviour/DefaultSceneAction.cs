using System;
using UnityEngine;

public class DefaultSceneAction : AnimationSceneActionBase
{
    [field: SerializeField] public override string ActionID { get; protected set; }

    public event Action OnActionCalled;

    public override void CallAction() => OnActionCalled?.Invoke();
}
