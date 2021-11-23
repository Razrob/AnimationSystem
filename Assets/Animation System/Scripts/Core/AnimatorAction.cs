using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimatorAction : AnimationActionBase
{
    [SerializeField] private string propertyName;

    private int? propertyHash;
    public int PropertyHash => GetHash();

    public override abstract void CallAction(Animator animator);

    private int GetHash()
    {
        if (!propertyHash.HasValue) propertyHash = Animator.StringToHash(propertyName);
        return propertyHash.Value;
    }
}
