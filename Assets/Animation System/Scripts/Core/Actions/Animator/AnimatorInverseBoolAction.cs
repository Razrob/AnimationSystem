using UnityEngine;

public class AnimatorInverseBoolAction : AnimatorAction
{
    public override void CallAction(Animator animator) => 
        animator.SetBool(PropertyHash, !animator.GetBool(PropertyHash));
}
