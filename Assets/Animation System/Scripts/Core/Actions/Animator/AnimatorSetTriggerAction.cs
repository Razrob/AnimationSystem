using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSetTriggerAction : AnimatorAction
{
    public override void CallAction(Animator animator) => animator.SetTrigger(PropertyHash);
}
