using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSetFloatAction : AnimatorAction
{
    [SerializeField] private float value;

    public override void CallAction(Animator animator) => animator.SetFloat(PropertyHash, value);
}
