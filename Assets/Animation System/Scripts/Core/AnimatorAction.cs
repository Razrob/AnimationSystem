using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimatorAction : AnimationActionBase
{
    [SerializeField] protected string propertyName;

    private int? propertyHash;
    protected int PropertyHash => GetHash();

    public sealed override void CallAction(Transform transform)
    {
        if (transform.TryGetComponent(out Animator animator))
            CallAction(animator);
    }
    public abstract void CallAction(Animator animator);

    private int GetHash()
    {
        if (!propertyHash.HasValue) propertyHash = Animator.StringToHash(propertyName);
        return propertyHash.Value;
    }
}
